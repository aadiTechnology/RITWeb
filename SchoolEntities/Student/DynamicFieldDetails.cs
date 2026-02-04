/* Author - Yogesh Karne
 * Date - 01 June 2016
 * Description - This entity class is used to store Dynamic Report Fields Details.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SchoolEntities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DynamicFieldDetails
    {
        public string FieldText { get; set; }
        public bool IsSelected { get; set; }
        public int DynamicReportFieldMasterId { get; set; } 
    }
}
