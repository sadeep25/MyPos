namespace MyPos.DAL.Migrations
{
    using MyPos.DAL.Entity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyPos.DAL.Context.MyPosDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MyPos.DAL.Context.MyPosDb context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Customers.AddOrUpdate(e => e.Name,
                 new Customer()
                 {
                     Name = "Sam",
                     Address = "Colombo",
                     EMail = "Sam@gmail.com"

                 }, new Customer()
                 {
                     Name = "Adam",
                     Address = "Kalutara",
                     EMail = "Adam@gmail.com"

                 },
                new Customer()
                {
                    Name = "Mark",
                    Address = "Negambo",
                    EMail = "Mark@gmail.com"

                }
);
        }
    }
}
