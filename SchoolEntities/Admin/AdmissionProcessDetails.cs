using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
namespace SchoolEntities
{
    public class AdmissionProcessDetails : SchoolEntity
    {
        public int AdmissionProcessId { get; set; }
        public int StanderdId { get; set; }
        public int TotalForms { get; set; }
        public int TotalOnlineForms { get; set; }
        public DateTime FormOpenDate { get; set; }
        public DateTime FormCloseDate { get; set; }
        public DateTime LottoryDate { get; set; }
        public DateTime AdmissionConfirmLastDate { get; set; }
        public bool IsLotteryConfirmed { get; set; }
        public bool CanConfirmDirectly { get; set; }
        public int Amount { get; set; }
        public DateTime DOBMax { get; set; }
        public DateTime DOBMin { get; set; }
        public bool EnableAdmissionFormFee { get; set; }
        public bool IsInternalAdmission { get; set; }
        public string StandardName { get; set; }
        public bool EnableWaitingList { get; set; }
        public string WaitingListURL { get; set; }
        // New fields added for internal link functionality
        public bool EnableInternalLink { get; set; }
        public string ExternalSiteMessage { get; set; }

    }

    /// <summary>
    /// Class to hold internal link standard details
    /// </summary>
    public class InternalLinkStandardDetails
    {
        public string StandardName { get; set; }
        public string DisplayMessage { get; set; }
    }
}
