using PharmoSys.Data.Entities;
using System.Threading.Tasks;

namespace PharmoSys.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity> GetUserByUsernameAsync(string username);
        Task<System.Collections.Generic.List<UserEntity>> GetAllUsersAsync();
        Task AddUserAsync(UserEntity user);
        Task UpdateUserAsync(UserEntity user);
        Task DeleteUserAsync(int id);
        Task EnsureAdminExistsAsync();
    }
}
