using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MyPos.DAL.Context;
using MyPos.DAL.Entity;
using System.Data.Entity.Migrations;

namespace MyPos.DAL.Migrations
{
    public class Configurations: DropCreateDatabaseIfModelChanges<MyPosDb>
    {
        public Configurations()
        {

        }
        protected override void Seed(MyPosDb context)
        {
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

        //private void AddData(MyPosDb context)
        //{
        //    var customers = new List<Customer>
        //    {
        //        new Customer()
        //        {
        //            Name="Sam",
        //            Address="Colombo",
        //            EMail="Sam@gmail.com"

        //        },new Customer()
        //        {
        //            Name="Adam",
        //            Address="Kalutara",
        //            EMail="Adam@gmail.com"

        //        },
        //        new Customer()
        //        {
        //            Name="Mark",
        //            Address="Negambo",
        //            EMail="Mark@gmail.com"

        //        }
        //   };
        }
}


