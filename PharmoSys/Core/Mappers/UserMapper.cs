using PharmoSys.Core.Models;
using PharmoSys.Data.Entities;

namespace PharmoSys.Core.Mappers
{
    public static class UserMapper
    {
        public static User ToModel(this UserEntity entity)
        {
            if (entity == null) return null;
            
            return new User
            {
                Id = entity.UserId,
                Username = entity.Username,
                FullName = entity.FullName,
                Role = entity.Role?.RoleName ?? "Unknown"
            };
        }
    }
}
