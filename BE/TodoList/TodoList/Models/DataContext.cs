using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TodoList.Models
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext> opt) : base(opt)
        {

        }
        #region DBSet
        public DbSet<Todos>? Todo { get; set; }

        #endregion
    }
}
