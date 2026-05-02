using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PharmoSys.Data.Entities
{
    public class UserEntity
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int UserId { get; set; }

        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public string FullName { get; set; }
        public string Phone { get; set; }

        public int RoleId { get; set; }
        public virtual RoleEntity Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<SaleEntity> Sales { get; set; }

    }
}
