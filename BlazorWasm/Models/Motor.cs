using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorWasm.Models
{
     public record Motor
    {
        public Motor()
        {
            
        }

        public Guid MotorId { get; set; }
        [Required]
        public string MotorNo { get; set; }
        public int RatedCurrent { get; set; } 
        public string FrontBearingType { get; set; }
        public string RearBearingType { get; set; }
        public string Brand { get; set; }
        public string PartNo { get; set; }
    }
}


