using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolEntities
{
    public class HomeworkDailyLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string AttachmentsName { get; set; }
        public bool IsPublished { get; set; }
        public int StandardDivisionId { get; set; }
        public int InsertedById { get; set; }
        public int TotalRows { get; set; }
    }
}


