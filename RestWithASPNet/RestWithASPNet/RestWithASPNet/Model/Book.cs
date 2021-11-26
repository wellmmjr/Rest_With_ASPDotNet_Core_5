﻿using RestWithASPNet.Model.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestWithASPNet.Model
{

    [Table("books")]
    public class Book : BaseEntity
    {
        
        [Column("author")]
        public string Author { get; set; }
        
        [Column("launch_date")]
        public DateTime LaunchDate { get; set; }
        
        [Column("price")]
        public decimal price { get; set; }

        [Column("title")]
        public string title { get; set; }
    }
}
