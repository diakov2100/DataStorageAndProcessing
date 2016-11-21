namespace DataStorageAndProcessing.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Net;
    using System.Text;
    public sealed class Configuration : DbMigrationsConfiguration<DataStorageAndProcessing.Data.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DataStorageAndProcessing.Data.Context context)
        {
            FillDatabase.Fill(context);
        }
    }
}