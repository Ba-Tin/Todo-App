using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Models
{
    public class Todos
    {
        [Key]   
        public int Id { get; set; }
        public string? content { get; set; }
        [DefaultValue(false)]
        public bool status { get; set; }
 
    }
}
