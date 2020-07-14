﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using ToDo.Services.Models;

namespace ToDo.Web.Models.Do
{
    public class DoListingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual IEnumerable<DoListingModel> SubTasks { get; set; }

        public DoListingModel(DoServiceModel model)
        {
            Id = model.Id;
            Title = model.Title;
            Description = model.Description;
            SubTasks = model.SubTasks.Select(sub => new DoListingModel(sub));
        }
    }
}
