using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hack.Model;

namespace Hack2021.Data
{
    public class Hack2021Context : DbContext
    {
        public Hack2021Context (DbContextOptions<Hack2021Context> options)
            : base(options)
        {
        }

        public DbSet<Hack.Model.CreditCard> CreditCard { get; set; }

        public DbSet<Hack.Model.Transaction> Transaction { get; set; }
    }
}
