using System;
using System.Collections.Generic;
using System.Text;
using ef = System.ComponentModel.DataAnnotations;
using ef1 = System.ComponentModel.DataAnnotations.Schema;
using SqlSugar;

namespace OrmBenchmark.Models
{
    [ef1.Table("Orders")]
    [SugarTable("Orders")]
    public class Order
    {
        [ef.Key, ef1.DatabaseGenerated(ef1.DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifyDate { get; set; }

        public int State { get; set; }

        [SugarColumn(IsIgnore = true)]
        public virtual Customer Customer { get; set; }

        [SugarColumn(IsIgnore = true)]
        public virtual ICollection<OrderDetail> Details { get; set; }
    }
}
