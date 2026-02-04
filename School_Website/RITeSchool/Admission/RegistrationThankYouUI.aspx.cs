using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class RegistrationThankYouUI : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (QueryString["EnquiryNo"] != null && QueryString["EnquiryNo"].ToString() != string.Empty)
        {  
            spnMessage.InnerHtml = "<h3>Your enquiry form has been submitted successfully. Enquiry No. : " + QueryString["EnquiryNo"].ToString() + " </h3>";
        }
        else
            spnMessage.InnerHtml = "<h3>Your enquiry form has been submitted successfully.</h3>";
    }
}