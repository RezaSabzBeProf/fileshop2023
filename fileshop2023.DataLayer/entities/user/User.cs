using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileshop2023.DataLayer.entities.user
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string? Name { get; set; }
        public string Phone { get; set; }
        public int Money { get; set; }

        public int ActiveCode { get; set; }

        public UserRole Role { get; set; } = UserRole.Normal;

    }
    public enum UserRole
    {
        Admin,
        Normal
    }
}
