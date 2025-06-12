using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace BOOLOG.Domain.Model
{
    public class PropertyRecommendation
    {
        [ColumnName("Score")]
        public float Score { get; set; }
    }
}
