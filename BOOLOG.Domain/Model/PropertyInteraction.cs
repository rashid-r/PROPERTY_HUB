using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace BOOLOG.Domain.Model
{
    public class PropertyInteraction
    {
        [LoadColumn(0)]
        public uint UserId { get; set; }

        [LoadColumn(1)]
        public uint PropertyId { get; set; }

        [LoadColumn(2)]
        public float Label { get; set; } 
    }
}
