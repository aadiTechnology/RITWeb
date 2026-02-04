/* File Name :- DisplayMenuContents.aspx.cs
 * Modified By ;- Sachin
 * Modified Date ;- 25-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to display menu contents.
*/

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Linq;

public partial class DisplayMenuContents : SchoolBase
{
    #region -- MEMBER(s) --

    private int miMenuId;
    private string msMenuName;
    private bool mbIsPreview;
    private bool mbIsExternalPages;

    protected string sMenuTitle = String.Empty;
    protected string sUrlTitle = String.Empty;
    protected string sMenuContent = String.Empty;

    #endregion -- MEMBER(s) --

    #region -- EVENT HANDLER(s) --

    /// <summary>
    ///		Reads the querystring and sets the master page.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);

            ReadQueryString();

            if (mbIsPreview)
                this.Page.MasterPageFile = "../MasterPages/PopupMasterSml.master";
            if (mbIsExternalPages)
                this.Page.MasterPageFile = "../MasterPages/EmptyMasterPage.master";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///		Handles the load event of the page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                bool bIsUseSubmitBehavior = CommonUtility.CheckCancelOrBackClickEvent(this.Page);
                if (bIsUseSubmitBehavior)
                    DisplayContents();
                CheckIsPreviewMode();
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///		Used to hide certain controls on the master page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            HideMasterControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///		This event is used to either close popup or go back towards dashboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            if (mbIsPreview)
                Response.Write("<script language='javascript' type='text/javascript'>window.close();</script>");
            else if ( mbIsExternalPages)
                Response.Write("<script language='javascript' type='text/javascript'>window.close();</script>");
            else
            {
                var oMasterPage = this.Master as MasterPage;
                oMasterPage.RedirectToNextPage("~/Common/ControlPanel.aspx");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///		Sets attributes for each attachment.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAttachments_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oMenuFile = e.Item.DataItem as MenuFile;
                if (!oMenuFile.IsNull())
                {

                    var hlinkPath = e.Item.FindControl("attchmentLink") as HyperLink;
                    if (oMenuFile.IsURL)
                        hlinkPath.Attributes["onclick"] = String.Format("window.open('{0}', '_new'); return false;", oMenuFile.Path);
                    else
                        hlinkPath.Attributes["onclick"] = String.Format("window.open('{0}', '_new'); return false;", Page.ResolveUrl(@"~\" + oMenuFile.Path));
                }
            }
        }

        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    

    /// <summary>
    ///		Sets the title for the ListView which includes the Name of the Menu being displayed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAttachments_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwAttachments.Items.Count > 0)
            {
                var lblAttachmentTitle = lstvwAttachments.FindControl("lblAttachmentTitle") as Label;
                lblAttachmentTitle.Text = sMenuTitle;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// Sets attributes for each attachment.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwURLs_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            var oMenuFile = e.Item.DataItem as MenuFile;
            if (!oMenuFile.IsNull())
            {
                var hlinkPath = e.Item.FindControl("attchmentLink") as HyperLink;
                hlinkPath.Attributes["onclick"] = String.Format("window.open('{0}', '_new'); return false;", oMenuFile.Path);
            }
        }
    }
    /// <summary>
    /// Sets the title for the ListView which includes the Name of the Menu being displayed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwURLs_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwURLs.Items.Count > 0)
            {
                var lblAttachmentTitle = lstvwURLs.FindControl("lblAttachmentTitle") as Label;
                lblAttachmentTitle.Text = sUrlTitle;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion -- EVENT HANDLER(s) --

    #region -- PRIVATE METHOD(s) --

    /// <summary>
    ///		This method is used to display contents of selected menu.
    /// </summary>
    private void DisplayContents()
    {
        var oConfigureMenuBL = new ConfigureMenuBL();
        SchoolEntities.Menu oMenu = oConfigureMenuBL.GetMenu(miMenuId);

        if (!oMenu.IsNull())
        {   
            sMenuTitle = (oMenu.Name.IsNullOrEmpty() ? String.Empty : oMenu.Name + " - ") + "Attachment(s)";
            sUrlTitle = (oMenu.Name.IsNullOrEmpty() ? String.Empty : oMenu.Name + " - ") + "URL(s)";
            sMenuContent = HttpUtility.HtmlDecode(oMenu.Content);

            if (oMenu.MenuFiles.Count > 0)
            {
                lstvwAttachments.DataSource = oMenu.MenuFiles.Where(sb=>!sb.IsURL).ToList();
                lstvwAttachments.DataBind();

                lstvwURLs.DataSource = oMenu.MenuFiles.Where(sb=>sb.IsURL).ToList();
                lstvwURLs.DataBind();
            }
        }
    }

    /// <summary>
    ///		This method is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (!QueryString["MenuId"].IsNull())
            miMenuId = QueryString["MenuId"].ToInt();

        if (!QueryString["IsPreview"].IsNull())
            mbIsPreview = QueryString["IsPreview"].ToBool();

        if (!QueryString["MenuName"].IsNull())
        {
            msMenuName = QueryString["MenuName"];
            msMenuName = msMenuName.Replace(" sps ", "&");
        }

        if (!QueryString["ExternalPages"].IsNull())
            mbIsExternalPages = QueryString["ExternalPages"].ToBool();
    }

    /// <summary>
    ///		This method is used to check whether current mode is preview mode or not.
    /// </summary>
    private void CheckIsPreviewMode()
    {
        if (!mbIsPreview && !mbIsExternalPages)
        {
            var oMasterPage = this.Master as MasterPage;
            oMasterPage.SetCurrentNodeText(msMenuName, miAcademicYearId, miSchoolId);
            btnBack.Visible = false;
        }
        else
            btnBack.Text = "Close";
    }

    /// <summary>
    ///		This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnBack });

        if (mbIsPreview || mbIsExternalPages)
            btnBack.OnClientClick = "window.close();";
        else if ((Constants.SuperAdminRoles)Session[Constants.S_SESSION_SUPERADMIN_ROLE_ID].ToInt() == Constants.SuperAdminRoles.ManagementUser)
        {
            btnBack.Visible = true;
            btnBack.PostBackUrl = Constants.S_PAGE_MANAGEMENT_DASHBOARD;
        }
    }

    /// <summary>
    ///		Hides certain controls on the master page.
    /// </summary>
    private void HideMasterControls()
    {
        var hlnkEmail = this.Master.FindControl("hlnkEmail") as HyperLink;
        if (hlnkEmail != null)
            hlnkEmail.Visible = false;

        var hlnkSupport = this.Master.FindControl("hlnkSupport") as HyperLink;
        if (hlnkSupport != null)
            hlnkSupport.Visible = false;

        var lnkFeedback = this.Master.FindControl("lnkFeedback") as LinkButton;
        if (lnkFeedback != null)
            lnkFeedback.Visible = false;
    }

    #endregion -- PRIVATE METHOD(s) --

   
}