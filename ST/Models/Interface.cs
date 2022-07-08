using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ST.Models
{
    public class IF_PDI_LIST
    {
        public long? _id { get; set; }
        public DateTime? _dateStart { get; set; }
        public DateTime? _dateEnd { get; set; }
    }
    public class IF_PARAM_LIST
    {
        public long? parId { get; set; }
        public int? parSequence { get; set; }
        public string parCode { get; set; }
        public string parName { get; set; }
        public bool? parIsManualType { get; set; }
        public bool? parIsFixedType { get; set; }
        public bool? parIsFormulaType { get; set; }
        public bool? parIsRangeType { get; set; }
        public bool? parIsEditable { get; set; }
        public bool? parIsRequired { get; set; }
        public string Formula { get; set; }
        public string pvalueFactors { get; set; }

        public bool? parShowInReport { get; set; }

        public bool? parHidden { get; set; }

        public string parRangeMax { get; set; }

        public string parRangeMin { get; set; }

        public bool? parShowInGraphic { get; set; }


    }

}