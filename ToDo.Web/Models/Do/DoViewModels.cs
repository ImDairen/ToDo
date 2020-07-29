using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using ToDo.Data.Models.Static;
using ToDo.Services.Models;

namespace ToDo.Web.Models.Do
{
    public class DoListingViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public virtual List<DoListingViewModel> SubTasks { get; set; }


        public DoListingViewModel(DoServiceModel model)
        {
            Id = model.Id;
            Title = model.Title;

            if (model.SubTasks != null && model.SubTasks.Any())
            {
                SubTasks = model.SubTasks.Select(s => new DoListingViewModel(s)).ToList();
            }
            else
            {
                SubTasks = new List<DoListingViewModel>();
            }
        }
    }

    public class DoCreateViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Executors { get; set; }

        public int Plan { get; set; }
    }

    public class DoUpdateViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Executors { get; set; }

        public DoStatus Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Done { get; set; }

        public int? Fact { get; set; }

        public DoUpdateViewModel(DoServiceModel model)
        {
            Id = model.Id;
            Title = model.Title;
            Description = model.Description;
            Executors = model.Executors;
            Status = model.Status;
            Done = model.Done;
            Fact = model.Fact;
        }

        public DoUpdateViewModel()
        {

        }
    }

    public class DoDescriptionViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public DoDescriptionViewModel(DoServiceModel model)
        {
            Id = model.Id;
            Description = model.Description;
        }
    }

}
