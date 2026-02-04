using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlEntities
{
    public class AdditionalFields
    {
        public string DisplayText { get; set; }
        public string Control { get; set; }
        public bool IsMandatory { get; set; }
        public int MaxLength { get; set; }
        public string AdditionalFieldId { get; set; }
    }
}
