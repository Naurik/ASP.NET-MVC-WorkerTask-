using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskUser.Models
{
    public class Tasks
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string NameTask { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StateTask { get; set; }
    }
}
