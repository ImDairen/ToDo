using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Services.Models;

namespace ToDo.Services.Interfaces
{
    public interface IDoService
    {
        public IEnumerable<DoServiceModel> GetAll();
        public DoServiceModel GetById(int id);
        public Task Add(DoServiceModel model);
        public Task<DoServiceModel> Delete(int id);
        public Task<DoServiceModel> Update(DoServiceModel model);
    }
}
