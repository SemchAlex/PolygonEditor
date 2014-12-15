using System.Data.Entity;
using PolygonEditor.Model;

namespace PolygonEditor.Helper
{
    public class DataContext : DbContext
    {
        public DbSet<Polygon> Polygons { get; set; }
        public DbSet<Point> Points { get; set; }

        public DataContext()
            : base("name=DataContext")
        {
        }

        public DataContext(string connString) : base(connString)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    }
}
