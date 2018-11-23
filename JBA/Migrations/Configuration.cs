namespace JBA.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using JBA.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<JBA.JBAContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(JBA.JBAContext context)
        {

            var headers = new List<FileHeader>
            {
                new FileHeader{Header = "long", HeaderRegex = "long\\s*=(\\-?\\d+[\\-\\d\\.]+),\\d*([\\-\\d\\.]+)"},
                new FileHeader{Header = "lati", HeaderRegex = "lati\\*=([\\-\\d\\.]+),\\d*([\\-\\d\\.]+)"},
                new FileHeader{Header = "grid", HeaderRegex = "grid\\s*X,Y=\\s*(\\-?[\\d]+)\\s*,\\s*(\\-[\\d]+)"},
                new FileHeader{Header = "boxes", HeaderRegex = "boxes\\s?X,Y=([\\-\\d]+),\\d*([\\-\\d]+)"},
                new FileHeader{Header = "years", HeaderRegex = "years\\s*=\\s*([12][\\d]{3})\\s*\\-\\s*([12][\\d]{3})"},
                
            };
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
