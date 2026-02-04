using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class DepositeBankDetails
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int CategoryId { get; set; }
        public int MonthId { get; set; }
        public DateTime Date { get; set; }
        public string ChequeNo { get; set; }
        public int TotalRows { get; set; }
        public string Month { get; set; }
        public string Category { get; set; }
    }
}
