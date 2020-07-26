using System.ComponentModel.DataAnnotations;
using ToDo.Data.Interfaces;

namespace ToDo.Data.Abstract
{
    public abstract class Entity : IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
