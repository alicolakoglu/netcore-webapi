using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExampleProject.Core.Domain.Users
{
    public class User : BaseEntity
    {
        public User()
        {
          
        }

        [Column(Order = 1), Key]
        public Guid Key { get; set; }

        [Column(Order = 2)]
        public DateTime CreatedDate { get; set; }

        [Column(Order = 3, TypeName = "varchar(20)")]
        public string Username { get; set; }

        [Column(Order = 4, TypeName = "varchar(150)")]
        public string DisplayName { get; set; }

        [Column(Order = 5, TypeName = "varchar(150)")]
        public string Email { get; set; }

        [Column(Order = 6, TypeName = "varchar(50)")]
        public string Password { get; set; }

        [Column(Order = 7)]
        public bool IsAdministrator { get; set; }

        [Column(Order = 8)]
        public bool IsDeleted { get; set; }

        [Column(Order = 9)]
        public bool IsActive { get; set; }
        
    }
}

