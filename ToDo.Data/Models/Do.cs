using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDo.Data.Abstract;
using ToDo.Data.Models.Static;

namespace ToDo.Data.Models
{
    public class Do : DoEntity
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Executors { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DoStatus Status { get; set; }

        public DateTime Done { get; set; }

        public int Plan { get; set; }

        public int Fact { get; set; }

        public virtual ICollection<Do> SubTasks { get; set; }
    }
}
