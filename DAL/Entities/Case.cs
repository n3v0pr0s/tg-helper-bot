using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Case
    {
        [Key]
        public int kod_razb { get; set; }
        public string ls { get; set; }
        public string abo { get; set; }
        public DateTime period { get; set; }
        public DateTime dnper { get; set; }
        public DateTime dkper { get; set; }
        public double summ { get; set; }
        public string nom_dela { get; set; }
        public DateTime? dvrs { get; set; }
        public int kod_stat_vz { get; set; }
    }
}
