// File Name - HealthParametersUI.aspx.cs
// Creator - Sachin Wagh
// Created Date - 10-12-2018
// Description - This class is used to configure Health Parameter.

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
public partial class HealthParametersUI : SchoolBase
{
    #region Data Member(s)
    private HealthParameterBL moHealthParameterBL;  
    #endregion

    #region Event(s)
    /// <summary>
    /// This event is used to fill components and parameters.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moHealthParameterBL = new HealthParameterBL(miSchoolId, miFinancialYearId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {                
                SetJavaScriptAttributes();
                FillComponent();
                FillParameters();
            }  
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to save parameter details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            HealthParameter oHealthParameter = Populate();
            moHealthParameterBL.Save(oHealthParameter);
            if (btnSave.Text == Constants.ButtonText.Update.ToString())
                DisplayMessage(Constants.ItemState.updated, false);
            else
            {
                DisplayMessage(Constants.ItemState.saved, false);
                if (hidIsConfigured.Value == Constants.S_NO)
                {
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.HealthParameter));
                    hidIsConfigured.Value = Constants.S_YES;
                }
            }
            FillParameters();
            ResetFields();
        }
        catch (SqlException se)
        {
            DisplayMessage(se.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }   
    
    /// <summary>
    /// This event is used to handle commands.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwParameters_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            int iHealthParameterId = Convert.ToInt32(lstvwParameters.DataKeys[e.Item.DisplayIndex]["Id"]);
            
            if (e.CommandName == Constants.S_COMMAND_UPDATE)
            {
                hidHealthParameterId.Value = iHealthParameterId.ToString();          
                HealthParameter oHealthParameter = moHealthParameterBL.Get(iHealthParameterId);
                if (oHealthParameter != null)
                {
                    cmbComponentName.SelectedValue = oHealthParameter.HealthComponentId.ToString();
                    txtParameterName.Text = oHealthParameter.ParameterName;
                    txtTest.Text = oHealthParameter.TestName;
                    txtMeasure.Text = oHealthParameter.Measure;
                    txtSortOrder.Text = oHealthParameter.SortOrder.ToString();                    
                }
                btnSave.Text = Constants.ButtonText.Update.ToString();               
            }
            else if (e.CommandName == Constants.S_COMMAND_REMOVE)
            {
                moHealthParameterBL.Delete(iHealthParameterId);
                DisplayMessage(Constants.ItemState.deleted, false);
                FillParameters();
                if (hidIsConfigured.Value == Constants.S_YES && lstvwParameters.Items.Count == 0)
                {
                    DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.HealthParameter));
                    hidIsConfigured.Value = Constants.S_NO;
                }
                ResetFields();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }  
    /// <summary>
    /// This event is used to set back page attribute.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Health_Related)));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to reset fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ResetFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to populate fields.
    /// </summary>
    /// <returns></returns>
    private HealthParameter Populate()
    {
        HealthParameter oHealthParameter = new HealthParameter
        {
            Id = hidHealthParameterId.Value.ToInt(),
            HealthComponentId = cmbComponentName.SelectedValue.ToInt(),
            ParameterName = txtParameterName.Text.Trim(),
            TestName = txtTest.Text.Trim(),
            Measure = txtMeasure.Text.Trim(),
            SortOrder = Convert.ToInt32(txtSortOrder.Text),   
        };
        return oHealthParameter;
    }

    /// <summary>
    /// This method is used to fill components.
    /// </summary>
    private void FillComponent()
    {
        HealthComponentBL oHealthComponentBL = new HealthComponentBL(miSchoolId,miFinancialYearId, miAcademicYearId, miUserId);
        List<HealthComponent> lstHealthComponent = oHealthComponentBL.GetAll(0);
        ListSource.FillDropDownList(lstHealthComponent, cmbComponentName, "ComponentName", "Id", Constants.S_SELECT);

       var lstIds =  lstHealthComponent.Where(hc => hc.IsFitnessComponent).Select(hc => hc.Id).ToList();
       if (lstIds.Count > 0)
           hidHealthComponentIdIsFitnessComponent.Value = string.Join(",", lstIds);
    }    
    /// <summary>
    /// This method is used to fill parameters.
    /// </summary>
    private void FillParameters()
    {
        List<HealthParameter> lstvwParameter = moHealthParameterBL.GetAll(0);
        lstvwParameters.DataSource = lstvwParameter;
        lstvwParameters.DataBind();
    }
    /// <summary>
    /// This method is used to set javascriot attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnBack, btnSave });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidIsConfigured.Value = QueryString["Is_Configured"];
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        cmbComponentName.Attributes.Add("onchange", "ActivateTestMeasure()");
        cmbComponentName.Focus();
    }
    /// <summary>
    /// This method is used to reset fields.
    /// </summary>
    private void ResetFields()
    {
        hidHealthParameterId.Value = Constants.S_ZERO;
        cmbComponentName.ClearSelection();
        txtParameterName.Text = string.Empty;
        txtTest.Text = string.Empty;
        txtMeasure.Text = string.Empty;
        txtSortOrder.Text = string.Empty;
        cmbComponentName.Focus();
        btnSave.Text = Constants.ButtonText.Save.ToString();
    }
    /// <summary>
    /// This method is used to display message. 
    /// </summary>
    /// <param name="aoItemState"></param>
    /// <param name="abIsErrorMessage"></param>
    private void DisplayMessage(Constants.ItemState aoItemState, bool abIsErrorMessage)
    {
        string sMessage = "Health Parameter " + aoItemState.ToString() + " successfully !!!";
        DisplayMessage(sMessage, abIsErrorMessage, tdMessage);
    }

    #endregion
}