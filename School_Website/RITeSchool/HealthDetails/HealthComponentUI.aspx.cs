// File Name - HealthComponentUI.aspx.cs
// Creator - Sachin Wagh
// Created Date - 21-11-2018
// Description - This class is used to configure Health Component.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Data.SqlClient;
using SchoolEntities;
using System.Data;

public partial class HealthComponentUI : SchoolBase
{
    #region Data Member(s)
    private HealthComponentBL moHealthComponentBL;    
    #endregion

    #region Event(s)
    /// <summary>
    /// This event is used to fill health component details in list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           moHealthComponentBL = new HealthComponentBL(miSchoolId, miFinancialYearId, miAcademicYearId, miUserId);
           if (!IsPostBack)
           {
                SetJavaScriptAttributes();
                txtComponentName.Focus();
                FillComponents();
           }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This method is  used to save/update health component details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)  
    {
        try
        {
            HealthComponent oHealthComponent = Populate();
            moHealthComponentBL.Save(oHealthComponent);
            if (btnSave.Text == Constants.ButtonText.Update.ToString())
                DisplayMessage(Constants.ItemState.updated, false);
            else
            {
                DisplayMessage(Constants.ItemState.saved, false);
                if (hidIsConfigured.Value == Constants.S_NO)
                {
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.HealthComponent));
                    hidIsConfigured.Value = Constants.S_YES;
                }
            }
            FillComponents();
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
    /// This event is used to assign image when IsFitnessComponent is true or false
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwComponents_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Image oimgIsFitnessComponent = e.Item.FindControl("imgIsFitnessComponent") as Image;
                HealthComponent oHealthComponent = e.Item.DataItem as HealthComponent;
                oimgIsFitnessComponent.ImageAlign = ImageAlign.Middle;
                if (oHealthComponent.IsFitnessComponent)
                {
                    oimgIsFitnessComponent.ImageUrl = "~/RITeSchool/images/IconGrid_AssignTrue.gif";
                }
                else
                {
                    oimgIsFitnessComponent.ImageUrl = "~/RITeSchool/images/IconGrid_Delete.gif";
                } 
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to edit/delete configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwComponents_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            int iHealthComponentId = Convert.ToInt32(lstvwComponents.DataKeys[e.Item.DisplayIndex]["Id"]);
            if (e.CommandName == Constants.S_COMMAND_UPDATE)
            {
                hidHealthComponentId.Value = iHealthComponentId.ToString();
                HealthComponent oHealthComponent = moHealthComponentBL.Get(iHealthComponentId);
                if (oHealthComponent != null)
                {
                    txtComponentName.Text = oHealthComponent.ComponentName.ToString();
                    txtSortOrder.Text = oHealthComponent.SortOrder.ToString();
                    chkIsFitnessComponent.Checked = oHealthComponent.IsFitnessComponent;
                }
                btnSave.Text = Constants.ButtonText.Update.ToString();
            }
            else if (e.CommandName == Constants.S_COMMAND_REMOVE)
            {
                moHealthComponentBL.Delete(iHealthComponentId);
                DisplayMessage(Constants.ItemState.deleted, false);
                FillComponents();
                if (hidIsConfigured.Value == Constants.S_YES && lstvwComponents.Items.Count == 0)
                {
                    DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.HealthComponent));
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
    /// This event is used to go back to Health Related dashboard.
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
    /// This method is used to populate object.
    /// </summary>
    /// <returns></returns> 
    private HealthComponent Populate()
    {
        HealthComponent oHealthComponent = new HealthComponent
        {
            Id = hidHealthComponentId.Value.ToInt(),        
            ComponentName = txtComponentName.Text,
            SortOrder = Convert.ToInt32(txtSortOrder.Text),
            IsFitnessComponent = chkIsFitnessComponent.Checked,   
        };
        return oHealthComponent;
    }
    /// <summary>
    /// This method is used to fill listview of Health Component.
    /// </summary>
    private void FillComponents()
    {
        List<HealthComponent> lstvwComponent = moHealthComponentBL.GetAll(0);
        lstvwComponents.DataSource = lstvwComponent;
        lstvwComponents.DataBind();
    }
    
    private void SetJavaScriptAttributes()
    {
        hidIsConfigured.Value = QueryString["Is_Configured"];
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnBack, btnSave });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;      
    }
    /// <summary>
    /// This method is used to reset fields.
    /// </summary>
    private void ResetFields()
    {
        hidHealthComponentId.Value = Constants.S_ZERO;    
        txtComponentName.Text = string.Empty;
        txtComponentName.Focus();
        txtSortOrder.Text = string.Empty;
        btnSave.Text = Constants.ButtonText.Save.ToString();
        chkIsFitnessComponent.Checked = false;
    }
    /// <summary>
    /// This method is used to display message.
    /// </summary>
    /// <param name="aoItemState"></param>
    /// <param name="abIsErrorMessage"></param>
    private void DisplayMessage(Constants.ItemState aoItemState, bool abIsErrorMessage)
    {
        string sMessage = "Health Component " + aoItemState.ToString() + " successfully !!!";
        DisplayMessage(sMessage, abIsErrorMessage, tdMessage);
    }  
    #endregion
}