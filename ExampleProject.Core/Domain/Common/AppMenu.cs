using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExampleProject.Core.Domain.Common
{
    public class AppMenu : BaseEntity
    {
        public AppMenu()
        {

        }
        
        [Column(Order = 1), Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Column(Order = 2, TypeName = "nvarchar(150)")]
        public string Title { get; set; }

        [Column(Order = 3, TypeName = "nvarchar(150)")]
        public string Href { get; set; }

        [Column(Order = 4, TypeName = "nvarchar(50)")]
        public string Icon { get; set; }

        [Column(Order = 5)]
        public int? ParentId { get; set; }

        [Column(Order = 6)]
        public int SortIndex { get; set; }

        [Column(Order = 7)]
        public bool IsActive { get; set; }
    }
}

