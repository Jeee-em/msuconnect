using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Data.Repositories.Implementations;
using api.Data.Repositories.Interfaces;
using api.DTOs.Account;
using api.Models;
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
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ISubjectRepository subjectRepo, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _subjectRepo = subjectRepo;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) {
                return Unauthorized("Invalid username or password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) {
                return Unauthorized("Invalid username or password");
            }

            return Ok(
                new NewUserDto {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
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
                    Email = registerDto.Email,
                    Bio = registerDto.Bio,
                    Expertise = new List<Subject>()
                };

                foreach (var subjectId in registerDto.ExpertiseIds) {
                    var subject = await _subjectRepo.GetSubjectByIdAsync(subjectId);

                    if (subject != null) {
                        user.Expertise.Add(subject);
                    }
                }

                var createdUser = await _userManager.CreateAsync(user, registerDto.Password);

                if (createdUser.Succeeded) {

                    var roleClaim = new Claim(ClaimTypes.Role, registerDto.Role); // Use Claims for Role
                    await _userManager.AddClaimAsync(user, roleClaim);

                    return Ok(
                            new NewUserDto {
                                Username = user.UserName,
                                Email = user.Email,
                                Token = _tokenService.CreateToken(user),
                                Bio = user.Bio,
                                Role = registerDto.Role,
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
    }
}