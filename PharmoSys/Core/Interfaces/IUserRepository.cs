using PharmoSys.Data.Entities;
using System.Threading.Tasks;

namespace PharmoSys.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity> GetUserByUsernameAsync(string username);
        Task EnsureAdminExistsAsync();
    }
}
