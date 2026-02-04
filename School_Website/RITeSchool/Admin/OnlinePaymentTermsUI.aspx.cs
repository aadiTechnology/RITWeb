///* -------------------------------------------------------------------------------------------------------
// *	Filename	: OnlinePaymentTermsUI.aspx.cs
// *	Author		: Yogesh
// *	Date		: 7-Aug-2015
// *	Description	: This class is used to insert, update, delete description for terms of use page.
// * ------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class OnlinePaymentTermsUI : SchoolBase
{

    #region Data Member(s) & Constant(s)

    const string S_SCREENS_URL = "ScreensUI.aspx";
    static string msURL = String.Empty;

    #endregion

    #region Event(s)


    /// <summary>
    /// This event is used to set master page based whether this screen is invoked from 
    /// super admin or from Admin.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);

            if (!IsPostBack)
                msURL = GetSourceUrl();
            if (msURL.Contains(S_SCREENS_URL))
                this.Page.MasterPageFile = "~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master";
            else
                this.Page.MasterPageFile = "../MasterPages/MasterPage.master";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());

        }
    }


    /// <summary>
    /// This event is fired when page will loaded.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                
                hidDescriptionId.Value = Constants.S_ZERO;
                FiilOnlineTermsCatagoryCombo();
                FillOnlinePaymentTermList();
               
                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event will fire when click on save button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save(hidDescriptionId.Value.ToInt(), txtDescription.Text.Trim(), cmbCatagory.SelectedValue.ToInt());

            if (btnSave.Text == "Update")
            {
                lblMessage.Text = "Description updated successfully.";
                btnSave.Text = "Save";
            }
            else
                lblMessage.Text = "Description saved successfully.";
            FillOnlinePaymentTermList();
            clearcontroles();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event will fired when we change catagory combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillOnlinePaymentTermList();
            clearcontroles();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event will fire when we click on edit and delete button of listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwHomeworkTeacher_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            OnlinePaymentTermsBL moOnlinePaymentTermsBL = new OnlinePaymentTermsBL(miSchoolId, miAcademicYearId, miUserId);
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;

            hidDescriptionId.Value = lstvwHomeworkTeacher.DataKeys[oCurrentItem.DisplayIndex]["Id"].ToString();
            if (e.CommandName == Constants.S_COMMAND_UPDATE)
            {
                Label lblDescription = e.Item.FindControl("lblDescription") as Label;
                cmbCatagory.SelectedValue = lstvwHomeworkTeacher.DataKeys[e.Item.DisplayIndex]["TermsCatagoryId"].ToString();
                txtDescription.Text = HttpUtility.HtmlDecode(lblDescription.Text);
                btnSave.Text = "Update";
            }
            else if (e.CommandName == Constants.S_COMMAND_REMOVE)
            {
                moOnlinePaymentTermsBL.Delete(hidDescriptionId.Value.ToInt());
                lblMessage.Text = "Description deleted successfully.";
                FillOnlinePaymentTermList();
                clearcontroles();
                btnSave.Text = "Save";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event will fire when listview will fill.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwHomeworkTeacher_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            ImageButton imgBtnDelete = oCurrentItem.FindControl("btnDelete") as ImageButton;

            imgBtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()){return false;}");
        }
        catch (Exception ex)
        {

            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to clear the controls.
    /// </summary>
    private void clearcontroles()
    {
        txtDescription.Text = string.Empty;
        hidDescriptionId.Value = "0";
    }


    /// <summary>
    /// This method is used to ger referrence page URL.
    /// </summary>
    /// <returns></returns>
    private string GetSourceUrl()
    {
        string sSourcePageUrl = string.Empty;
        if (Request.UrlReferrer != null)
        {
            sSourcePageUrl = Request.UrlReferrer.AbsolutePath;
            sSourcePageUrl = sSourcePageUrl.Substring(sSourcePageUrl.LastIndexOf("/") + 1);
        }
        return sSourcePageUrl;
    }

    /// <summary>
    /// This method is used to fill catagory combo.
    /// </summary>
    private void FiilOnlineTermsCatagoryCombo()
    {
        OnlinePaymentTermsBL moOnlinePaymentTermsBL = new OnlinePaymentTermsBL(miSchoolId, miAcademicYearId, miUserId);
        DataTable oDTOnlineTermsCatagory = moOnlinePaymentTermsBL.GetOnlineTermsCatagory();
        if (oDTOnlineTermsCatagory.Rows.Count > 0)
            ControlUtility.FillDropDownList(oDTOnlineTermsCatagory, ref cmbCatagory, "Id", "Catagory", string.Empty);
    }

    /// <summary>
    /// This method is used to fill listview control.
    /// </summary>
    private void FillOnlinePaymentTermList()
    {
        OnlinePaymentTermsBL moOnlinePaymentTermsBL = new OnlinePaymentTermsBL(miSchoolId, miAcademicYearId, miUserId);
        List<OnlinePaymentTermsDetails> lstOnlinePaymentTerm = moOnlinePaymentTermsBL.Get(cmbCatagory.SelectedValue.ToInt());

        lstvwHomeworkTeacher.DataSource = lstOnlinePaymentTerm;
        lstvwHomeworkTeacher.DataBind();

    }

    /// <summary>
    /// This method is used to save description.
    /// </summary>
    /// <param name="aiId"></param>
    /// <param name="asDescription"></param>
    /// <param name="aiTermCategoryId"></param>
    private void Save(int aiId, string asDescription, int aiTermCategoryId)
    {
        OnlinePaymentTermsBL moOnlinePaymentTermsBL = new OnlinePaymentTermsBL(miSchoolId, miAcademicYearId, miUserId);
        moOnlinePaymentTermsBL.Save(aiId, asDescription, aiTermCategoryId);
    }

    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            clearcontroles();
            btnSave.Text = "Save";
        }
        catch (Exception ex)
        {
            
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
}