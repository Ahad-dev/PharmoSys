using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PharmoSys.Models
{
    class User
    {
        public int UserId { get; set; }

        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public string FullName { get; set; }
        public string Phone { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Sale> Sales { get; set; }

    }
}
