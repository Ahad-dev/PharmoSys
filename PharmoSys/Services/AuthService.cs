using Microsoft.EntityFrameworkCore;
using PharmoSys.Core.Interfaces;
using PharmoSys.Core.Mappers;
using PharmoSys.Core.Models;
using PharmoSys.Data.Repositories;
using PharmoSys.Helpers;
using System;
using System.Threading.Tasks;

namespace PharmoSys.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService()
        {
            // For now, instantiate directly. Later, use DI.
            _userRepository = new UserRepository();
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var userEntity = await _userRepository.GetUserByUsernameAsync(username);

            if (userEntity == null)
            {
                return null;
            }

            bool isPasswordValid = SecurityHelper.VerifyPassword(password, userEntity.PasswordHash);
            
            if (isPasswordValid)
            {
                return userEntity.ToModel();
            }

            return null;
        }

        public async Task EnsureAdminExistsAsync()
        {
            await _userRepository.EnsureAdminExistsAsync();
        }
    }
}
