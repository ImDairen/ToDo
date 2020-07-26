using System.Collections.Generic;
using System.Linq;
using ToDo.Services.Models;

namespace ToDo.Web.Models.Do
{
    public class DoListingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual List<DoListingModel> SubTasks { get; set; }


        public DoListingModel(DoServiceModel model)
        {
            Id = model.Id;
            Title = model.Title;

            if (model.SubTasks != null && model.SubTasks.Any())
            {
                SubTasks = model.SubTasks.Select(s => new DoListingModel(s)).ToList();
            }
            else
            {
                SubTasks = new List<DoListingModel>();
            }
        }
    }
}
