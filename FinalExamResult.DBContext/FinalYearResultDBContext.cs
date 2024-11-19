using FinalExamResult.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExamResult.DBContext
{
    public class FinalYearResultDBContext : DbContext
    {
        public FinalYearResultDBContext(DbContextOptions options)
           : base(options)
        {
        }

        public FinalYearResultDBContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<StudentMarks> StudentMarks { get; set; }

    }
}
