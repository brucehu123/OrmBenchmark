﻿using OrmBenchmark.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OrmBenchmark.Frameworks
{
    class AdoNetTest : IPerformanceTest
    {
        public string Framework => "AdoNet";

        public long GetCustomerById(int id)
        {
            return Utility.Watch(delegate ()
            {
                using (var conn = Utility.CreateConnection())
                {
                    conn.Open();
                    using (var adapter = Utility.CreateAdapter(
@"SELECT  Id ,
        Address1 ,
        Address2 ,
        Code ,
        Name ,
        Zip
FROM    dbo.Customers
WHERE   Id = @Id;", conn))
                    {
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@Id", id));
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                    }
                }
            });
        }

        public long GetDetailsByOrder(int orderId)
        {
            return Utility.Watch(delegate ()
            {
                using (var conn = Utility.CreateConnection())
                {
                    conn.Open();
                    using (var adapter = Utility.CreateAdapter(
@"SELECT  Id ,
        Discount ,
        [Key] ,
        OrderId ,
        Price ,
        ProductId ,
        Quantity
FROM    dbo.OrderDetails
WHERE   OrderId = @OrderId;", conn))
                    {
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@OrderId", orderId));
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                    }
                }
            });
        }

        public long GetOrderAndDetails(int orderId)
        {
            return Utility.Watch(delegate ()
            {
                using (var conn = Utility.CreateConnection())
                {
                    conn.Open();
                    using (var adapter = Utility.CreateAdapter(
@"SELECT  od.Id ,
        od.Discount ,
        od.[Key] ,
        od.OrderId ,
        od.Price ,
        od.ProductId ,
        od.Quantity ,
        o.Id OrderId ,
        o.CreateDate ,
        o.CustomerId ,
        o.ModifyDate ,
        o.State
FROM    dbo.OrderDetails od
        INNER JOIN dbo.Orders o ON o.Id = od.OrderId
WHERE   o.Id = @OrderId;", conn))
                    {
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@OrderId", orderId));
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                    }
                }
            });
        }

        public long InsertDiscreteCustomers(Customer[] customers)
        {
            return Utility.Watch(delegate ()
            {
                using (var conn = Utility.CreateConnection())
                {
                    conn.Open();

                    using (var adapter = Utility.CreateAdapter("SELECT Id, Name, Code, Address1, Address2, Zip FROM dbo.Customers", conn))
                    {
                        DataTable table = new DataTable();
                        SqlCommandBuilder bulder = new SqlCommandBuilder(adapter);
                        adapter.FillSchema(table, SchemaType.Source);
                        adapter.InsertCommand = bulder.GetInsertCommand();

                        foreach (var c in customers)
                        {
                            table.Rows.Add(c.Id, c.Name, c.Code, c.Address1, c.Address2, c.Zip);
                        }
                        int val = adapter.Update(table);
                    }
                }
            });
        }

        public long InsertDiscreteProducts(Product[] products)
        {
            return Utility.Watch(delegate ()
            {
                using (var conn = Utility.CreateConnection())
                {
                    conn.Open();
                    using (var adapter = Utility.CreateAdapter("SELECT Category ,Code ,IsValid ,Name ,UpdateDate,Id FROM dbo.Products", conn))
                    {
                        DataTable table = new DataTable();
                        SqlCommandBuilder bulder = new SqlCommandBuilder(adapter);
                        adapter.FillSchema(table, SchemaType.Source);
                        adapter.InsertCommand = bulder.GetInsertCommand();

                        foreach (var c in products)
                        {
                            table.Rows.Add(c.Category, c.Code, c.IsValid, c.Name, c.UpdateDate);
                        }
                        int val = adapter.Update(table);
                    }
                }
            });
        }

        public long UpdateDiscreteCustomers(Customer[] customers)
        {
            return Utility.Watch(delegate ()
            {
                using (var conn = Utility.CreateConnection())
                {
                    conn.Open();
                    using (var adapter = Utility.CreateAdapter("SELECT Id, Name, Code, Address1, Address2, Zip FROM dbo.Customers", conn))
                    {
                        DataTable table = new DataTable();
                        adapter.FillSchema(table, SchemaType.Source);
                        SqlCommandBuilder bulder = new SqlCommandBuilder(adapter);
                        var update = new SqlCommand(
@"UPDATE [dbo].[Customers]
   SET [Address1] = @Address1
      ,[Address2] = @Address2
      ,[Code] = @Code
      ,[Name] = @Name
      ,[Zip] = @Zip
 WHERE [Id] =@Id;"
, conn);
                        update.Parameters.Add(new SqlParameter() { ParameterName = "@Id", SourceColumn = "Id" });
                        update.Parameters.Add(new SqlParameter() { ParameterName = "@Address1", SourceColumn = "Address1" });
                        update.Parameters.Add(new SqlParameter() { ParameterName = "@Address2", SourceColumn = "Address2" });
                        update.Parameters.Add(new SqlParameter() { ParameterName = "@Code", SourceColumn = "Code" });
                        update.Parameters.Add(new SqlParameter() { ParameterName = "@Name", SourceColumn = "Name" });
                        update.Parameters.Add(new SqlParameter() { ParameterName = "@Zip", SourceColumn = "Zip" });
                        adapter.UpdateCommand = update;
                        foreach (var c in customers)
                        {
                            table.Rows.Add(c.Id, c.Name, c.Code, c.Address1, c.Address2, c.Zip);
                        }
                        table.AcceptChanges();
                        foreach (DataRow row in table.Rows)
                        {
                            row.SetModified();
                        }
                        int val = adapter.Update(table);
                    }
                }
            });
        }

        public long DeleteDiscreteDetails(OrderDetail[] details)
        {
            return Utility.Watch(delegate ()
            {
                using (var conn = Utility.CreateConnection())
                {
                    conn.Open();
                    using (var adapter = Utility.CreateAdapter("SELECT Id FROM dbo.OrderDetails", conn))
                    {
                        DataTable table = new DataTable();
                        SqlCommandBuilder bulder = new SqlCommandBuilder(adapter);
                        adapter.FillSchema(table, SchemaType.Source);
                        adapter.DeleteCommand = bulder.GetDeleteCommand();

                        foreach (var c in details)
                        {
                            var row = table.Rows.Add(c.Id);
                            row.AcceptChanges();
                            row.Delete();
                        }
                        int val = adapter.Update(table);
                    }
                }
            });
        }

        public long DeleteDiscreteWarehouses(Warehouse[] warehouses)
        {
            return Utility.Watch(delegate ()
            {
                using (var conn = Utility.CreateConnection())
                {
                    conn.Open();
                    using (var adapter = Utility.CreateAdapter("SELECT Id,Number FROM dbo.Warehouses", conn))
                    {
                        DataTable table = new DataTable();
                        SqlCommandBuilder bulder = new SqlCommandBuilder(adapter);
                        adapter.FillSchema(table, SchemaType.Source);
                        adapter.DeleteCommand = bulder.GetDeleteCommand();

                        foreach (var c in warehouses)
                        {
                            var row = table.Rows.Add(c.Id, c.Number);
                            row.AcceptChanges();
                            row.Delete();
                        }
                        int val = adapter.Update(table);
                    }
                }
            });
        }
    }
}
