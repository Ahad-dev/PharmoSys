using System;
using System.Collections.Generic;
using System.Text;

namespace PharmoSys.Models
{
    class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
