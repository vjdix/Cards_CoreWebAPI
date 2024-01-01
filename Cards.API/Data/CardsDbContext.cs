using Cards.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Cards.API.Data
{
	public class CardsDbContext : DbContext
	{
		public CardsDbContext(DbContextOptions options) : base(options)
		{
		}
		//DbSet

		public DbSet<Card> Cards { get;set; }
}
}
