using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Linq;
using Utility;

public partial class FeedbackListUI : SchoolBase
{
    #region "Data Member"

    private string msServerPath = "./RITeSchool/downloads/Feedbacks/";

    #endregion
    #region "Constants"
    private const string msSortexpression = "AND IsSelected=1";
    #endregion

    /// <summary>
    /// This event is used to display the listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillUserDetailGrid();
                FillOtherFeedbackListView();
                GetSourceUrl();
            }
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to bind file path to link.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwOtherFeedback_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            if (oCurrentItem != null)
            {
                string sOldFilePath = lstvwOtherFeedback.DataKeys[oCurrentItem.DisplayIndex]["FilePath"].ToString();
                Label lblDate = oCurrentItem.FindControl("lblDate") as Label;
                HyperLink oLinkButton = oCurrentItem.FindControl("lnkName") as HyperLink;
                oLinkButton.NavigateUrl = msServerPath + sOldFilePath;
                oLinkButton.Attributes.Add("onclick", "window.open('" + oLinkButton.NavigateUrl
                                               + "' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600'); return false;");

                trNoRecord.Visible = Convert.ToBoolean(lstvwOtherFeedback.Items.Count);
            }
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill userdetail's grid of a particular user.
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    private void FillUserDetailGrid()
    {
        List<FeedbackDetailsBL> lstDetails = FeedbackDetailsBL.GetSelectedFeedback(Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]));

        foreach (FeedbackDetailsBL oUserFeedbackDetail in lstDetails)
        {
            HtmlTableRow trHeader = new HtmlTableRow();
            AddCell(trHeader, oUserFeedbackDetail.UserName, "ClsProgressGridTestHeader1", "Left", 0, "font-weight:bold;border-style:solid;border-width:1px;border-color:skyblue;color:brown;width:50%;");
            AddCell(trHeader, oUserFeedbackDetail.InsertedDate.ToString(Constants.S_DATE_FORMAT), "ClsProgressGridTestHeader1", "Right", 0, "font-weight:bold;border-style:solid;border-width:1px;border-color:skyblue;color:brown;width:50%;");
            tblParameterUser.Rows.Add(trHeader);

            HtmlTableRow trDescription = new HtmlTableRow();
            AddCell(trDescription, "<br />" + oUserFeedbackDetail.FeedbackDescription, "ClsProgressGridTestBody1", "Left", 2, "border-style:solid;border-width:1px;border-color:skyblue;width:100%;color:navy");
            tblParameterUser.Rows.Add(trDescription);

            HtmlTableRow trNewLine = new HtmlTableRow();
            AddCell(trNewLine, "<br />", "", "Left", 2);
            tblParameterUser.Rows.Add(trNewLine);
        }
    }


    /// <summary>
    /// This method is used to fill other appreciaton listview.
    /// </summary>
    private void FillOtherFeedbackListView()
    {
        List<SchoolEntities.FeedbackDetails> lstDetails = FeedbackDetailsBL.GetOtherFeedback(Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]), msSortexpression);
        lstvwOtherFeedback.DataSource = lstDetails;
        lstvwOtherFeedback.DataBind();
    }

    /// <summary>
    /// This method is used to ger referrence page URL.
    /// </summary>
    /// <returns></returns>
    private string GetSourceUrl()
    {
        string sSourcePageUrl = string.Empty;
        string sQuery = string.Empty;
        if (Request.Url != null)
        {
            sQuery = Request.QueryString.ToString();
            if (sQuery == "wlHZOyPUhfm5%2fwwtuLvzmg=%3dq" || string.IsNullOrEmpty(sQuery))
            {
                trUser.Visible = true;
                trOther.Visible = false;
                lblheadr.InnerText = "Feedback from Users";
                trNoRecord.Visible = false;
            }
            else if (sQuery == "HTM+HFqsE3QbEML4MpFILg=%3dq")
            {
                trUser.Visible = false;
                trOther.Visible = true;
                lblheadr.InnerText = "Feedback from Others";
            }
        }

        return sQuery;
    }
}