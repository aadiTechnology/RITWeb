// File Name  : SmsTemplateUI.aspx.cs
// Created By : Deepak
// Date       : 07/12/2009
//Description :This class is used to add ,delete sms templates and modify existing one. 


using System;
using System.Data;
using BusinessLogic;
using Utility;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;


public partial class SmsTemplateUI : SchoolBase
{
    
    #region "EVENT"

    /// <summary>
    /// This event is used to intialize controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            

            if (!IsPostBack)
            {
                FillSmsNameComboAndDyanamicVariableList();
                SetJavaScriptAttributes();
            }
            EnableControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }
    /// <summary>
    /// This Event adds selected value of dynamic variables abbriavations into sms templet text. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstAbbreviation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (lstAbbreviation.Items.Count > 0)
            {
                int iSelectedIndex = lstAbbreviation.SelectedIndex;
                lstAbbreviationName.SelectedIndex = iSelectedIndex;
                txtTemplate.Text = txtTemplate.Text + " " + lstAbbreviation.SelectedItem + " ";
                txtTemplate.Focus();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This Event adds selected value of dynamic variables abbriavations into sms templet text. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstAbbreviationName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (lstAbbreviationName.Items.Count > 0)
            {
                int iSelectedIndex = lstAbbreviationName.SelectedIndex;
                lstAbbreviation.SelectedIndex = iSelectedIndex;
                txtTemplate.Text = txtTemplate.Text + " " + lstAbbreviation.SelectedItem + " ";
                txtTemplate.Focus();
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
            if (cmbSmsName.SelectedValue != "0")
            {
                SmsTemplateBL oSmsTemplateBL = PopulateSmsTemplate();
                if (!HasTemplate(Convert.ToInt32(cmbSmsName.SelectedValue)))
                {
                    oSmsTemplateBL.UpdateSmsTemplate();
                    lblUpdateSucess.Visible = true;
                    lblUpdateSucess.Text = "Sms Template saved successfully!!!";
                }
                else
                {
                    oSmsTemplateBL.UpdateSmsTemplate();
                    lblUpdateSucess.Visible = true;
                    lblUpdateSucess.Text = "Sms Template updated successfully!!!";
                }
                btnDelete.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This Event is used to display saved templet for selected sms name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbSmsName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbSmsName.SelectedValue != "0")
        {
            if (!HasTemplate(Convert.ToInt32(cmbSmsName.SelectedValue)))
            {
                txtTemplate.Text = "";
            }
            else
            {
                txtTemplate.Text = hidTemplateText.Value;
            }

        }
        else
        {
            txtTemplate.Text = "";
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
        }
    }
    /// <summary>
    /// This Event is used to delete saved sms template
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (cmbSmsName.SelectedValue != "0")
        {
            if (HasTemplate(Convert.ToInt32(cmbSmsName.SelectedValue)))
            {
                SmsTemplateBL oSmsTemplateBL = new SmsTemplateBL();
                int iTemplateId = Convert.ToInt32(cmbSmsName.SelectedValue);
                oSmsTemplateBL.Delete(iTemplateId);
                lblUpdateSucess.Visible = true;
                txtTemplate.Text = "";
                lblUpdateSucess.Text = "Sms Template deleted successfully!!!";
                btnDelete.Visible = false;
            }
        }
    }
    /// <summary>
    /// This Event is used to cancle saving and go back to superadmin screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            SuperAdminMasterPage oMasterPage = (SuperAdminMasterPage)this.Master;
            oMasterPage.RedirectToNextPage("~/SuperAdmin/ScreensUI.aspx");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region "PRIVATE METHODS"

    /// <summary>
    /// This method is used to fill sms name combo,dynamic variable abbriavation list 
    /// and dynamic variable name list.
    /// </summary>
    private void FillSmsNameComboAndDyanamicVariableList()
    {
        DataSet oDataSet = SmsTemplateBL.FillSmsNameComboAndDyanamicVariableList(miSchoolId, miAcademicYearId);
        ControlUtility.FillDropDownList(oDataSet.Tables[0], ref cmbSmsName, "SmsTemplateId", "SmsTemplateName", Constants.S_SELECT);
        lstAbbreviation.DataSource = oDataSet.Tables[1];
        lstAbbreviation.DataTextField = "VariableAbbreviation";
        lstAbbreviation.DataValueField = "VariableId";
        lstAbbreviation.DataBind();
        lstAbbreviationName.DataSource = oDataSet.Tables[1];
        lstAbbreviationName.DataTextField = "VariableName";
        lstAbbreviationName.DataValueField = "VariableId";
        lstAbbreviationName.DataBind();
    }
    /// <summary>
    /// This method creates SmsTemplateBL object and return it.
    /// </summary>
    /// <returns>SmsTemplateBL</returns>
    private SmsTemplateBL PopulateSmsTemplate()
    {
        SmsTemplateBL oSmsTemplateBL = new SmsTemplateBL();
        oSmsTemplateBL.TemplateId = Convert.ToInt32(cmbSmsName.SelectedValue);
        oSmsTemplateBL.SmsName = Convert.ToString(cmbSmsName.SelectedItem);
        oSmsTemplateBL.TemplateText = txtTemplate.Text;
        oSmsTemplateBL.SchoolId = miSchoolId;
        oSmsTemplateBL.IsDeleted = false;
        oSmsTemplateBL.AcademicYearId = miAcademicYearId;
        return oSmsTemplateBL;
    }
    /// <summary>
    /// This method is used to check if template is saved for selected sms name or not.
    /// </summary>
    /// <param name="iSmsId"></param>
    /// <returns></returns>
    private bool HasTemplate(int iSmsId)
    {
        bool sMessage = false;
        DataTable oDataTable = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
        if (oDataTable.Rows.Count != 0)
        {
            if (oDataTable.Rows[0][2] != DBNull.Value)
            {
               hidTemplateText.Value = Convert.ToString(oDataTable.Rows[0][2]);
                btnDelete.Visible = true;
                sMessage = true;
            }
            else
            {
                btnDelete.Visible = false;
                sMessage = false;
            }
        }
        return sMessage;
    }
    /// <summary>
    /// This Event is used enable or disable controls.
    /// </summary>
    private void EnableControls()
    {
        if (cmbSmsName.SelectedValue != "0")
        {
            lstAbbreviation.Enabled = true;
            lstAbbreviationName.Enabled = true;
            txtTemplate.Enabled = true;
            btnSave.Enabled = true;
            btnDelete.Enabled = true;
        }
        else
        {
            lstAbbreviation.Enabled = false;
            lstAbbreviationName.Enabled = false;
            txtTemplate.Enabled = false;
            btnSave.Enabled = false;
            btnDelete.Visible = false;
        }
    }
    /// <summary>
    /// This method is used to set javascript attributes for buttons.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnDelete, btnCancel });
    }
    #endregion
}
