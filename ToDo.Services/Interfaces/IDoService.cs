using System.Collections.Generic;
using ToDo.Data.Models.Static;
using ToDo.Services.Models;

namespace ToDo.Services.Interfaces
{
    public interface IDoService
    {
        DoServiceModel GetDo(int id);
        int? CreateDo(DoServiceModel model);
        void UpdateDo(DoServiceModel model);
        void DeleteDo(int id);
        IEnumerable<DoServiceModel> GetDoes();
        void ChangeDoStatus(int id, DoStatus status);
    }
}
