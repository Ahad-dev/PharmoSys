using System;
using System.Collections.Generic;
using System.Text;

namespace PharmoSys.Data.Entities
{
    class RoleEntity
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public ICollection<UserEntity> Users { get; set; }
    }
}
