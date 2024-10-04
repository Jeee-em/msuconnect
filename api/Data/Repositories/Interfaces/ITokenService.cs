using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Data.Repositories.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}