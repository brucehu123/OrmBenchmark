using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrmBenchmark.Models
{
    public class EFCoreContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Utility.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Warehouse>().HasKey(e => new { e.Id, e.Number });
            builder.Entity<Product>().Property(a => a.Id).UseSqlServerIdentityColumn();

            builder.Entity<Order>()
               .HasOne(a => a.Customer)
               .WithMany(a => a.Orders)
               .HasForeignKey(a => a.CustomerId);

            builder.Entity<OrderDetail>()
                .HasOne(a => a.Product)
                .WithMany()
                .HasForeignKey(a => a.ProductId);

            builder.Entity<Order>()
              .HasMany(a => a.Details)
              .WithOne(a => a.Order)
              .HasForeignKey(a => a.OrderId);
        }

        public void InitialData()
        {
            int productcount = 9000;
            int customercount = 10000;
            int warehousescount = 5000;
            int ordercount = 100000;

            if (this.Products.Count() == 0)
            {
                Random r = new Random(DateTime.Now.Millisecond);
                Product[] products = new Product[productcount];
                for (int i = 0; i < products.Length; i++)
                {
                    products[i] = new Product()
                    {
                        Code = "Pro" + i.ToString(),
                        Name = "Product " + i.ToString().PadLeft(4, '0'),
                        Category = r.Next(1, 5),
                        IsValid = ((i / 5) == 0),
                        UpdateDate = DateTime.Now.AddDays(r.Next(-365, 365)),
                    };
                }
                this.Products.AddRange(products);
               

                Customer[] customers = new Customer[customercount];
                for (int i = 0; i < customers.Length; i++)
                {
                    customers[i] = new Customer()
                    {
                        Id = i + 1,
                        Code = "C" + i.ToString().PadLeft(2, '0'),
                        Name = "Customer " + i.ToString().PadLeft(3, '0'),
                        Zip = r.Next(100, 400).ToString(),
                        Address1 = "Address Master " + r.NextDouble().ToString(),
                        Address2 = "This is data  " + r.NextDouble().ToString(),
                    };
                }
                this.Customers.AddRange(customers);

                List<Warehouse> warehouses = new List<Warehouse>();
                for (int i = 0; i < warehousescount; i++)
                {
                    var count = r.Next(3, 10);
                    for (int j = 0; j < count; j++)
                    {
                        warehouses.Add(new Warehouse()
                        {
                            Id = i,
                            Number = j,
                            Name = "Warehouse" + i.ToString().PadLeft(3, '0') + j.ToString().PadLeft(4, '0'),
                            Address = "Address Master " + r.NextDouble().ToString(),
                        });
                    }
                }
                this.Warehouses.AddRange(warehouses);

                Order[] orders = new Order[ordercount];
                for (int i = 0; i < orders.Length; i++)
                {
                    orders[i] = new Order()
                    {
                        Id = i + 1,
                        CreateDate = DateTime.Now.AddDays(r.Next(-365, 365)),
                        ModifyDate = DateTime.Now.AddDays(r.Next(-365, 365)),
                        State = r.Next(1, 10),
                        CustomerId= customers[r.Next(0, customers.Length - 1)].Id
                    };
                    this.Orders.Add(orders[i]);

                    var count = r.Next(3, 10);
                    for (int j = 0; j < count; j++)
                    {
                        var detail = new OrderDetail()
                        {
                            Quantity = r.Next(100, 500),
                            Discount = r.Next(1, 100),
                            Price = Convert.ToDecimal(r.NextDouble() * 100),
                            OrderId= orders[i].Id,
                            ProductId= products[r.Next(0, products.Length - 1)].Id
                        };
                        this.OrderDetails.Add(detail);
                    }
                }
              
            }
        }
    }
}
