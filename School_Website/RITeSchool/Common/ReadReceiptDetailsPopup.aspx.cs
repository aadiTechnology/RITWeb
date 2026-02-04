using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SchoolEntities;

public partial class ReadReceiptDetailsPopup : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetJavascriptAttrinutes();
            FillReadReceiptDetails();
        }
    }

    private void FillReadReceiptDetails()
    {
        int iMessageDetailId = Convert.ToInt32(QueryString["MessageDetailId"]);
        MessageReceiverDetailsCollectionBL oMessageReceiverDetailsCollectionBL = new MessageReceiverDetailsCollectionBL();
        List<ReadReceiptDetails> lstReadReceiptDetails = oMessageReceiverDetailsCollectionBL.GetAllReadReceiptDetails(iMessageDetailId, miSchoolId, miAcademicYearId);
        lstvwUsers.DataSource = lstReadReceiptDetails;
        lstvwUsers.DataBind();
    }

    private void SetJavascriptAttrinutes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnClose });
        btnClose.Attributes.Add("onclick","window.close();");
    }
}