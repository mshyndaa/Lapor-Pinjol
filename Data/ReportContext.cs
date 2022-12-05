#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LaporPinjol.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LaporPinjol.Data
{
    public class ReportContext : IdentityDbContext
    {
        public ReportContext (DbContextOptions<ReportContext> options)
            : base(options)
        {
        }

        public DbSet<LaporPinjol.Models.Report> Report { get; set; }
    }
}
