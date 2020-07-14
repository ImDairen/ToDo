using System;

namespace ToDo.Web.Models.Do
{
    public class AddDoModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Executors { get; set; }

        public DateTime Created { get; set; }

        public string Status { get; set; }

        public DateTime Done { get; set; }

        public int Plan { get; set; }

        public int Fact { get; set; }
    }
}
