using System;
using System.Collections.Generic;
using System.Text;

namespace PharmoSys.Data.Entities
{
    public class RoleEntity
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<UserEntity> Users { get; set; }
    }
}
