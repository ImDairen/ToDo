using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel;
using System.Web.Mvc;
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
        [Required(ErrorMessage = "Необходимо ввести название")]
        [StringLength(125, MinimumLength = 3, 
            ErrorMessage = "Название должно содержать не менее 3 и не более 125 символов")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [StringLength(250, MinimumLength = 3,
            ErrorMessage = "Описание должно содержать не менее 3 и не более 250 символов")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Executors")]
        public string Executors { get; set; }

        [Range(1,60, ErrorMessage = "Планируемое время должно находиться в промежутке от 1 до 60")]
        [Display(Name = "Plan")]
        public string Plan { get; set; }
    }

    public class DoUpdateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Необходимо ввести название")]
        [StringLength(35, MinimumLength = 3,
            ErrorMessage = "Название должно содержать не менее 3 и не более 35 символов")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Executors")]
        public string Executors { get; set; }

        [Display(Name = "Status")]
        public DoStatus Status { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", 
            ApplyFormatInEditMode = true)]
        [Display(Name = "Done")]
        public DateTime? Done { get; set; }

        [Display(Name = "Fact")]
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

    public class DoDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Executors")]
        public string Executors { get; set; }

        [Display(Name = "Status")]
        public DoStatus Status { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        [Display(Name = "Created")]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        [Display(Name = "Done")]
        public DateTime? Done { get; set; }

        [Display(Name = "Plan")]
        public int Plan { get; set; }

        [Display(Name = "Fact")]
        public int? Fact { get; set; }


        public List<DoDetailsViewModel> SubTasks { get; set; }

        public DoDetailsViewModel(DoServiceModel model)
        {
            Id = model.Id;
            Title = model.Title;
            Description = model.Description;
            Executors = model.Executors;
            Status = model.Status;
            Created = model.Created;
            Done = model.Done;
            Plan = model.Plan;
            Fact = model.Fact;

            if (model.SubTasks != null && model.SubTasks.Any())
            {
                SubTasks = model.SubTasks.Select(s => new DoDetailsViewModel(s)).ToList();
            }
            else
            {
                SubTasks = new List<DoDetailsViewModel>();
            }
        }
    }

    public class DoDeleteViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public DoDeleteViewModel(DoServiceModel model)
        {
            Id = model.Id;
            Title = model.Title;
            Description = model.Description;
        }
    }

}
