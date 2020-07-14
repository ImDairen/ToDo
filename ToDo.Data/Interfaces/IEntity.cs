using System.ComponentModel.DataAnnotations;

namespace ToDo.Data.Interfaces
{
    public interface IEntity<TKey>
    {
        [Key]
        public int Id { get; set; }
    }
}
