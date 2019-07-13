using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using mego = Caredev.Mego.DataAnnotations;
using ef = System.ComponentModel.DataAnnotations;
using ef1 = System.ComponentModel.DataAnnotations.Schema;

namespace OrmBenchmark.Models
{
    [mego.Table("Customers")]
    [ef1.Table("Customers")]
    [SugarTable("Customers")]
    public class Customer
    {
        [mego.Key]
        [ef.Key, ef1.DatabaseGenerated(ef1.DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Zip { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [SugarColumn(IsIgnore = true)]
        [mego.InverseProperty("CustomerId", "Id")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
