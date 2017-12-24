using System;
using System.ComponentModel.DataAnnotations;

namespace TimeCalculator.Data
{
    public class SetupEntity
    {
        [Key]
        public int Id { get; set; }

        public int Value { get; set; }

        public SetupType SetupType { get; set; }

        public DateTime StartTime { get; set; }
    }
}