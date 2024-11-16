using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Data.Repositories.Implementations;
using api.Data.Repositories.Interfaces;
using api.DTOs.Account;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;
        private readonly ISubjectRepository _subjectRepo;

        private readonly IAccountRepository _accountRepo;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IAccountRepository accountRepo, ISubjectRepository subjectRepo, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _subjectRepo = subjectRepo;
            _accountRepo = accountRepo;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = await _accountRepo.GetUserByUsernameAsync(loginDto.Username);
            // var user = await _userManager.Users
            // .Include(u => u.Expertise)
            // .FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) {
                return Unauthorized("Invalid username or password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) {
                return Unauthorized("Invalid username or password");
            }

            var role = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return Ok(
                new NewUserDto {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user),
                    Bio = user.Bio,
                    ProfilePicture = user.ProfilePicture,
                    IsAvailableForSession = user.IsAvailableForSession,
                    DateOfBirth = user.DateOfBirth,
                    Role = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
                    ExpertiseIds = user.Expertise.Select(s => s.SubjectId).ToList()
                }
            );
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try 
            {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var user  = new User {
                    UserName = registerDto.Username,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    Bio = registerDto.Bio,
                    ProfilePicture = registerDto.ProfilePicture,
                    IsAvailableForSession = registerDto.IsAvailableForSession,
                    DateOfBirth = registerDto.DateOfBirth,
                    Expertise = new List<Subject>()
                };

                foreach (var subjectId in registerDto.ExpertiseIds) {
                    var subject = await _subjectRepo.GetSubjectByIdAsync(subjectId);

                    if (subject != null) {
                        user.Expertise.Add(subject);
                    }
                }

                var createdUser = await _accountRepo.CreateUserAsync(user, registerDto.Password);

                if (createdUser.Succeeded) {

                    await _accountRepo.AddUserToRoleAsync(user, registerDto.Role);

                    return Ok(
                            new NewUserDto {
                                Username = user.UserName,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Email = user.Email,
                                Token = _tokenService.CreateToken(user),
                                Bio = user.Bio,
                                Role = registerDto.Role,
                                IsAvailableForSession = user.IsAvailableForSession,
                                DateOfBirth = user.DateOfBirth,
                                ExpertiseIds = user.Expertise.Select(s => s.SubjectId).ToList()
                            }
                    );
                } else {
                    return StatusCode(500, createdUser.Errors);
                }
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile() {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var user = await _accountRepo.GetUserByIdAsync(userId);

            // var user = await _userManager.Users
            //     .Include(u => u.Expertise)
            //     .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound("User not found");

            var profileDto = user.ToUserProfileDto();

            return Ok(profileDto);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetOtherProfile(string username) {
            var user = await _accountRepo.GetUserByUsernameAsync(username);

            if (user == null) return NotFound("User not found");

            var ProfileDto = user.ToUserProfileDto();

            return Ok(ProfileDto);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto updateProfileDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var userProfile = await _accountRepo.UpdateUserProfileAsync(userId, updateProfileDto);

            if (userProfile == null) return NotFound();

            return Ok(userProfile.ToUpdateProfileDto());

            // var user = await _accountRepo.GetUserByIdAsync(userId);

            // if (user == null) return NotFound("User not found");

            // user.UserName = updateProfileDto.Username;
            // user.FirstName = updateProfileDto.FirstName;
            // user.LastName = updateProfileDto.LastName;
            // user.Bio = updateProfileDto.Bio;
            // user.ProfilePicture = updateProfileDto.ProfilePicture;
            // user.IsAvailableForSession = updateProfileDto.IsAvailableForSession;
            // user.DateOfBirth = updateProfileDto.DateOfBirth;
            // user.Expertise = new List<Subject>();

            // var result = await _userManager.UpdateAsync(user);
            
            // if (result.Succeeded)
            // {
            //     return Ok(
            //         new UserProfileDto
            //         {
            //             Username = user.UserName,
            //             FirstName = user.FirstName,
            //             LastName = user.LastName,
            //             Email = user.Email,
            //             Role = user.Role,
            //             Bio = user.Bio,
            //             ProfilePicture = user.ProfilePicture,
            //             IsAvailableForSession = user.IsAvailableForSession,
            //             DateOfBirth = user.DateOfBirth,
            //             ExpertiseIds = user.Expertise.Select(s => s.SubjectId).ToList()
            //         }
            //     );
            // }
            

            // return StatusCode(500, result.Errors);
        }

    }
}