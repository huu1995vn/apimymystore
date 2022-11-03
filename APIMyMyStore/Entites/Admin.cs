﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIMyMyStore.Entites
{
    public class Admin
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [MaxLength(150)]
        public string name { get; set; }
        [MaxLength(10)]
        public string phone { get; set; }
        [MaxLength(250)]
        public string email { get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public DateTime? updatedate { get; set; }
        public DateTime createdate { get; set; }

    }
}