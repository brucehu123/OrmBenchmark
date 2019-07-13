using System;
using System.Collections.Generic;
using System.Text;
using ef = System.ComponentModel.DataAnnotations;
using ef1 = System.ComponentModel.DataAnnotations.Schema;
using SqlSugar;


namespace OrmBenchmark.Models
{
    [ef1.Table("Warehouses")]
    [SugarTable("Warehouses")]
    public class Warehouse
    {
        [ef.Key, ef1.Column(nameof(Id), Order = 1)]
        [ef1.DatabaseGenerated(ef1.DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ef.Key, ef1.Column(nameof(Number), Order = 2)]
        [ef1.DatabaseGenerated(ef1.DatabaseGeneratedOption.None)]
        public int Number { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
    }
}
