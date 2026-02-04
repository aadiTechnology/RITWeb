// File Name  : SmsTemplateUI.aspx.cs
// Created By : Deepak
// Date       : 07/12/2009
//Description :This class is used to add ,delete sms templates and modify existing one. 
// Modified By : Pravin Shinde
// Date       : 12-Aug-2013
//Description :This class is modified to give a facility of SMS template on SMS UI screen.


using System;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BusinessLogic.Exceptions;
using System.Web;
using BusinessLogic;
using Utility;
using System.Drawing;
using System.Web.UI;

public partial class SmsTemplateUI : SchoolBase
{

    #region -- CONSTANTS(s) --

    private const string S_SAVE_MESSAGE = "Template saved successfully!!!";
    private const string S_UPDATE_MESSAGE = "Template updated successfully!!!";
    private const string S_DELETE_MESSAGE = "Template deleted successfully!!!";
    private const string S_SAVE_BUTTON = "Save";
    private const string S_UPDATE_BUTTON = "Update";
    private const string S_DASHBOARD_URL = "/RITeSchool/Common/ControlPanel.aspx";
    private const string S_DELETED = Constants.S_ONE;
    private SmsTemplateBL moSmsTemplateBL;
    
    #endregion -- CONSTANTS(s) --

    #region "EVENT"

    /// <summary>
    /// This event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            base.AddSortImage(lstvwTemplates, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Sets the MasterPage depending upong the logged in user or request query string.
    /// </summary>
    /// <param name="e"> </param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);
            if (Request.QueryString.Count <= 0)
                Page.MasterPageFile = "../MasterPages/MasterPage.master";                
            else            
                Page.MasterPageFile = "../MasterPages/PopupMaster.master";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to intialize controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moSmsTemplateBL = new SmsTemplateBL(miSchoolId, miAcademicYearId);
            if (!IsPostBack)
            {
                Initialize();
                SetJavaScriptAttributes();
                FillTemplateDetails();
            }            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to save sms template.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            PopulateSmsTemplate();                        
            moSmsTemplateBL.Insert();

            lblUpdateSucess.Visible = true;
            if (hidTemplateId.Value.IsNullOrEmpty() || hidTemplateId.Value == Constants.S_ZERO)
                lblUpdateSucess.Text = S_SAVE_MESSAGE;
            else
                lblUpdateSucess.Text = S_UPDATE_MESSAGE;      
          
            FillTemplateDetails();
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }   
    
    /// <summary>
    /// This event is used to handle the item databound of templates. Here we deside the controls visibility depend on from where it is opened.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTemplates_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var oCurrentItem = e.Item as ListViewDataItem;            
                var imgEdit = oCurrentItem.FindControl("imgEdit") as ImageButton;
                var imgDelete = oCurrentItem.FindControl("imgDelete") as ImageButton;
                bool bIsSystemDefined = lstvwTemplates.DataKeys[oCurrentItem.DataItemIndex]["IsSystemDefined"].ToBool();
         
                if (Request.QueryString.Count <= 0)
                {
                    var otdSelect = oCurrentItem.FindControl("tdSelect") as HtmlTableCell;
                   
                        if (bIsSystemDefined)
                        {
                            var oHTMLCurrentRow = oCurrentItem.FindControl("trlistvw") as HtmlTableRow;
                            if(oHTMLCurrentRow != null)
                                oHTMLCurrentRow.Style.Add(HtmlTextWriterStyle.BackgroundColor, "LightBlue");
                        }
                    
                    otdSelect.Visible = false;
                }
                else
                {
                    var otdEdit = oCurrentItem.FindControl("tdEdit") as HtmlTableCell;
                    var otdDelete = oCurrentItem.FindControl("tdDelete") as HtmlTableCell;
                    otdEdit.Visible = false;
                    otdDelete.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to set sort order in list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTemplates_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            base.RevertSortOrder(hidSortDirection);
            if (hidTemplateId.Value.IsNullOrEmpty() || hidTemplateId.Value == Constants.S_ZERO)
                btnSave.Text = S_SAVE_BUTTON;
            else
                btnSave.Text = S_UPDATE_BUTTON;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to hide/show header controls depends on from where the page is opened.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTemplates_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwTemplates.Items.Count > 0)
            {
                if (Request.QueryString.Count <= 0)
                {
                    var othSelect = lstvwTemplates.FindControl("thSelect") as HtmlTableCell;

                    othSelect.Visible = false;
                }
                else
                {
                    var othEdit = lstvwTemplates.FindControl("thEdit") as HtmlTableCell;
                    var othDelete = lstvwTemplates.FindControl("thDelete") as HtmlTableCell;
                    othEdit.Visible = false;
                    othDelete.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle the command event of the templates. It it used to handle edit,delete events.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTemplates_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var iTemplateId = e.CommandArgument;
                var oCurrentItem = e.Item as ListViewDataItem;                
                Label lblName = oCurrentItem.FindControl("lblName") as Label;
                Label lblTemplate = oCurrentItem.FindControl("lblTemplate") as Label;
                Label lblregno = oCurrentItem.FindControl("LblRegno") as Label;

                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moSmsTemplateBL.Delete(iTemplateId.ToInt());
                    lblUpdateSucess.Visible = true;
                    txtTemplate.Text = string.Empty;
                    lblUpdateSucess.Text = S_DELETE_MESSAGE;
                    FillTemplateDetails();
                    ClearFields();
                }
                else if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    hidTemplateId.Value = iTemplateId.ToString();
                    txtTemplateName.Text = lblName.Text;
                    txtTemplate.Text = lblTemplate.Text;
                    txtRegNo.Text = lblregno.Text;
                    lblUpdateSucess.Text = string.Empty;
                    btnSave.Text = S_UPDATE_BUTTON;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to collect the template to use on the SMS screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            string sTemplate = string.Empty;
            string tempregid = string.Empty;
            for (int iRowCount = 0; iRowCount < lstvwTemplates.Items.Count; iRowCount++)
            {
                ListViewDataItem oListViewDataItem = lstvwTemplates.Items[iRowCount];
                RadioButton rdoTemplate = oListViewDataItem.FindControl("rdoTemplate") as RadioButton;
                Label lblTemplate = oListViewDataItem.FindControl("lblTemplate") as Label;
                Label lblRegNo = oListViewDataItem.FindControl("LblRegno") as Label; // for template registration id 
                if (rdoTemplate != null && rdoTemplate.Checked)
                {
                    sTemplate = HttpUtility.JavaScriptStringEncode(lblTemplate.Text);
                    tempregid = HttpUtility.JavaScriptStringEncode(lblRegNo.Text);       //                          
                    break;
                }
            }

            Response.Write(String.Format("<Script  type='text/javascript'>window.opener.SetTemplate('" + sTemplate + "', '" + tempregid  + "');</Script>"));
            Response.Write("<Script type='text/javascript'>window.close();</Script>");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel the selected operation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearFields();
    }
    #endregion

    #region "PRIVATE METHODS"

    /// <summary>
    /// This method initializes the conrols and called from pageload.
    /// </summary>
    private void Initialize()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ValidationSummary1.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        if (Request.QueryString.Count < 0)
            hidUrl.Value = Request.UrlReferrer.AbsolutePath;
        if (hidUrl.Value == S_DASHBOARD_URL)
            hidShowSystemDefined.Value = Constants.S_YES;
        
        if (Request.QueryString.Count <= 0)
            hidShowSystemDefined.Value = Constants.S_YES;        
        else
        {
            hidShowSystemDefined.Value = Constants.S_NO;
            VisibleHideControls();
        }
    }
     
    /// <summary>
    /// This method is used to fill the templates with their associating status in the listview.
    /// </summary>
    /// <param name="aiGroupId"></param>
    private void FillTemplateDetails()
    {
        lstvwTemplates.DataSourceID = lstvwDSobj.ID;
        lstvwTemplates.DataBind();
        if (lstvwTemplates.Items.Count == Constants.I_ZERO)
            btnOk.Enabled = false;
        else
            btnOk.Enabled = true;
    }
    
    /// <summary>
    /// This method is used to populate the SMS details and return the object.
    /// </summary>
    /// <returns></returns>
    private void PopulateSmsTemplate()
    {
        moSmsTemplateBL.TemplateId = !hidTemplateId.Value.IsNullOrEmpty() ? hidTemplateId.Value.ToInt() : 0;
        moSmsTemplateBL.SmsName = txtTemplateName.Text.Trim();
        moSmsTemplateBL.TemplateText = txtTemplate.Text.Trim();
        moSmsTemplateBL.TemplateRegistrationId = txtRegNo.Text.Trim();
        moSmsTemplateBL.IsDeleted = false;                                
    }
    
    /// <summary>
    /// This method is used to Visible and hide controls depending on from where the page is opened.
    /// </summary>
    private void VisibleHideControls()
    {
        tblSms.Visible = false;
        btnClose.Visible = true;
        btnOk.Visible = true;
        btnOk.Text = "Select";
        trTitle.Visible = true;
        tblMainBody.Width = "100%";
    }

    /// <summary>
    /// This function is used to clear controls after deleting,saving data.
    /// </summary>
    private void ClearFields()
    {
        txtTemplate.Text = string.Empty;
        txtTemplateName.Text = string.Empty;
        txtRegNo.Text = string.Empty;
        hidTemplateId.Value = Constants.S_ZERO;
        btnSave.Text = S_SAVE_BUTTON;
    }

    /// <summary>
    /// This method is used to set javascript attributes for buttons.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        hidSortDirection.Value = Constants.S_ASCENDING;
        btnSave.Text = S_SAVE_BUTTON;
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnOk, btnClose });
        txtTemplateName.Focus();
    }

    #endregion       
}
