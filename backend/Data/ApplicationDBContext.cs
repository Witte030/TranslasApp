using Microsoft.EntityFrameworkCore;

namespace TranslasApp.Backend.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {

        }

        public DbSet<Models.ReceiverModel> Receivers { get; set; }
        public DbSet<Models.SupplierModel> Suppliers { get; set; }
        public DbSet<Models.CarrierModel> Carriers { get; set; }
        public DbSet<Models.CardModel> Cards { get; set; }

    }
}
