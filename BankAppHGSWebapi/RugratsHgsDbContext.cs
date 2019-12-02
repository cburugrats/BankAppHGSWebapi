using BankAppHGSWebapi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAppHGSWebapi
{
	public class RugratsHgsDbContext:DbContext
	{
		public DbSet<HgsUser> User { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"tcp:rugratsvt.database.windows.net,1433;Initial Catalog=RugratsHgs;Persist Security Info=False;User ID=Rugrat;Password=Pas314159;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}

}
