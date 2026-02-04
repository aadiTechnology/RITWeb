// -----------------------------------------------------------------------
// <copyright file="OnlinePaymentTermsDetails.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
namespace SchoolEntities
{
    

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OnlinePaymentTermsDetails : SchoolEntity
    {
        public int Id {get; set;}
        public string Discription {get; set;}
        public int TermsCatagoryId { get; set; }
        
    }
}
