using System;
using System.Collections.Generic;
using System.Text;

namespace ContactBookAPI.Lib.Core.Services
{
    public interface ITokenGeneration
    {
        object GenerateToken(string userId, string Email);
    }
}
