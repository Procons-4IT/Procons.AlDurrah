namespace Procons.Durrah.Main
{
    using System.Data.Entity;
    using Procons.Durrah.Common;

    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("name=DefaultConnection")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.IncludeMetadataInDatabase = false;
        //}

        public DbSet<SH15> SH15 { get; set; }
    }
}
