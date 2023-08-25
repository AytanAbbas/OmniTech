using Core.Utilities.Security.Hashing;
using DocumentFormat.OpenXml.Spreadsheet;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Omnitech.Dtos;
using Omnitech.DTOs;
using Omnitech.Models;
using System;
using System.Threading.Tasks;

namespace Omnitech.Service
{
    public class AuthManager
    {
        private UserManager _userService;


        public AuthManager(UserManager userService)
        {
            _userService = userService;
        }

        public async Task<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Username = userForRegisterDto.Username,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true
            };
            await _userService.AddAsync(user);
            return user;
        }

        public async Task<User> LoginAsync(UserForLoginDto userForLoginDto)
        {
            var userToCheck = await _userService.GetByUsernameAsync(userForLoginDto.Username);

            if (userToCheck.ID == 0)
            {
                throw new System.Exception("User not found!");
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                throw new System.Exception("Pasword incorrect!");
            }

            return userToCheck;
        }

        public async Task<bool> UserExists(string email)
        {
            User user = await _userService.GetByUsernameAsync(email);

            if (user == null || user == default || user.ID > 0)
                return true;

            return false;
        }
    }
}
