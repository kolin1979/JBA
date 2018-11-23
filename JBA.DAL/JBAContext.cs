using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JBA.Models;
using System.Data.Entity;

namespace JBA.DAL
{
    public class JBAContext : DbContext
    {
        public DbSet<SimpleData> SimpleDataCollection { get; set; }
        public DbSet<DataCollection> DataCollections { get; set; }
        public DbSet<GridReference> GridReferences { get; set; }
        public DbSet<GridReferenceData> GridReferenceDataCollection { get; set; }
        public DbSet<FileHeader> FileHeaders { get; set; }

    }
}
