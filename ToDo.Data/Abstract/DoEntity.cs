using System.ComponentModel.DataAnnotations;
using ToDo.Data.Interfaces;

namespace ToDo.Data.Abstract
{
    public class DoEntity : IDoEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
