using System.Collections.Generic;

namespace ToDo.Web.Models.Do
{
    public class DoIndexModel
    {
        public IEnumerable<DoListingModel> ToDoList { get; set; }

        public DoIndexModel(IEnumerable<DoListingModel> doListingModels)
        {
            ToDoList = doListingModels;
        }

        public DoIndexModel(DoListingModel model)
        {
            ToDoList = model.SubTasks;
        }
    }
}
