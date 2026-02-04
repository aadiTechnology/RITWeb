using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using PhotoUploadEntities;
using BusinessLogic;

public partial class DocumentDetailsUI : SchoolBase
{
    #region "Event(s)"

    /// <summary>
    /// This method is used to fill conrols.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FillUserDocumentDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to to fill listview details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUserDocuments_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                LinkButton lnkDocumentName = (LinkButton)oCurrentItem.FindControl("lnkDocumentName");
                string sFileName = lstvwUserDocuments.DataKeys[oCurrentItem.DisplayIndex]["DocumentFilePath"].ToString();
                lnkDocumentName.Attributes.Add("Onclick", "OpenDocumentPopUp('" + sFileName + "');return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region "Private Method"

    private void FillUserDocumentDetails()
    {
        List<UserRolewiseDocumentDetails> lstUserRolewiseDocumentDetails = new List<UserRolewiseDocumentDetails>();
        UploadUserDocumentBL oUploadUserDocumentBL = new UploadUserDocumentBL(miSchoolId, miAcademicYearId);
        lstUserRolewiseDocumentDetails = oUploadUserDocumentBL.GetUserDocumentDetails(miUserId, miFinancialYearId);

        lstvwUserDocuments.DataSource = lstUserRolewiseDocumentDetails;
        lstvwUserDocuments.DataBind();
    }

    #endregion

}