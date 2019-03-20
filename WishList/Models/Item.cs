using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WishList.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime Deadline { get; set; }

        public bool Overdue { get; set; }

    }
}
