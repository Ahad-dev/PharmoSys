using Microsoft.EntityFrameworkCore;
using PharmoSys.Core.Interfaces;
using PharmoSys.Data.Context;
using PharmoSys.Data.Entities;
using PharmoSys.Helpers;
using System.Threading.Tasks;

namespace PharmoSys.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PharmoSysDbContext _dbContext;

        public UserRepository()
        {
            _dbContext = new PharmoSysDbContext();
        }

        public async Task<UserEntity> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task EnsureAdminExistsAsync()
        {
            if (!await _dbContext.Roles.AnyAsync())
            {
                var adminRole = new RoleEntity { RoleName = "Admin" };
                var cashierRole = new RoleEntity { RoleName = "Cashier" };
                var managerRole = new RoleEntity { RoleName = "Manager" };
                
                _dbContext.Roles.AddRange(adminRole, cashierRole, managerRole);
                await _dbContext.SaveChangesAsync();
            }

            var validHash = SecurityHelper.HashPassword("admin123");

            await SeedUserAsync("admin", "Admin", "System Administrator", validHash);
            await SeedUserAsync("cashier1", "Cashier", "John Cashier", validHash);
            await SeedUserAsync("manager1", "Manager", "Alice Manager", validHash);
        }

        private async Task SeedUserAsync(string username, string roleName, string fullName, string passwordHash)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                var role = await _dbContext.Roles.FirstAsync(r => r.RoleName == roleName);
                user = new UserEntity
                {
                    Username = username,
                    PasswordHash = passwordHash,
                    FullName = fullName,
                    Phone = "1234567890",
                    Role = role,
                    RoleId = role.RoleId
                };
                _dbContext.Users.Add(user);
            }
            else
            {
                // Force update the password in case it was corrupted by the SQL script
                user.PasswordHash = passwordHash;
                _dbContext.Users.Update(user);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
