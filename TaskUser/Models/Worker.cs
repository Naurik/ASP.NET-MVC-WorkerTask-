using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskUser.Models
{
    public class Worker
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [Display(Name = "ФИО")]
        public string FIO { get; set; }
        [Required]
        [Display(Name = "Должность")]
        public string Position { get; set; }
        [Required]
        [Display(Name = "Выполнено, %")]
        public string State { get; set; }
        [Required]
        public string TaskName { get; set; }
        public Guid TaskId { get; set; }
        public Tasks Tasks { get; set; }
    }
}