using ExampleProject.Core.Domain.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExampleProject.Core.Domain.Todo
{
    public class TodoItem : BaseEntity
    {
        public TodoItem()
        {

        }

        [Column(Order = 1), Key]
        public Guid Key { get; set; }

        [Column(Order = 2)]
        public DateTime CreatedDate { get; set; }

        [Column(Order = 3)]
        public string Description { get; set; }

        [Column(Order = 4)]
        public bool IsCompleted { get; set; }

        [Column(Order = 5)]
        public Guid CreatorUserKey { get; set; }
        public virtual User CreatorUser { get; set; }
    }
}

