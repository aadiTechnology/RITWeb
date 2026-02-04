using System;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Xml;
using RJS.Web.WebControl;
using System.Collections.Generic;
using TransportEntities;
using System.Reflection;
using Utility;
using BusinessLogic.Exceptions;
using System.Data.SqlClient;

public partial class TransportLateFeeSettingUI : SchoolBase
{
    #region Constant (s)

    private const string S_TRANSPORT_DURATION = "Transport Service duration is from ";

    #endregion Constant (s)
    
    #region Data Member (s)

    private TransportLateFeeSettingsBL moTransportLateFeeSettingsBL = null;
  
    #endregion

    #region Events (s)
    
    /// <summary>
    /// This event use to load list view with late fee configuration
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moTransportLateFeeSettingsBL = new TransportLateFeeSettingsBL(miSchoolId, miAcademicYearId);
            if (!IsPostBack)
            {
               SetAttributes();
               FillLateFeeSettings();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    RefreshValue();
                }
            }
            if (Session[Constants.S_SESSION_LANGUAGE] != null)
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }
       
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
      
    }
  
   /// <summary>
    /// This event use to save transport late fee configuration
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveLateFeeSettings();
            bool bIsConfigured = QueryString[Constants.S_IS_CONFIGURED] != Constants.S_YES;
            if (bIsConfigured)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.TransportLateFeeSettings));

        }
        catch (SqlException ex)
        {
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method use to redirect user on Transport Related Screen
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Transport_Releted)), false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
  
    #endregion

    #region Private Method (s)
   
    /// <summary>
    /// This method use to set javascript attributes for controls on page
    /// </summary>
    private void SetAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel });
    }
    
    /// <summary>
    /// This method use to gill listview and other controls with late fee settings
    /// </summary>
    private void FillLateFeeSettings()
    {
        TransportLateFeeSetting oTransportLateFeeSetting = null;
        List<TransportLateFeeDueDate> lstTransportLateFeeSetting = moTransportLateFeeSettingsBL.GetAll(out oTransportLateFeeSetting);
        if (lstTransportLateFeeSetting.Count >= Constants.I_ONE)
        {
            txtValueForType.Text = oTransportLateFeeSetting.ValueForType.ToString();
            cmbFeeType.SelectedValue = Convert.ToString(oTransportLateFeeSetting.LateFeePerTypeId);
            txtAmount.Text = Convert.ToString(oTransportLateFeeSetting.LateFeeAmount);
            hidServiceStartDate.Value = oTransportLateFeeSetting.TransportStartDate.ToString();
            if (Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE] != null)
                hidAcademicStartDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToString();
            if (Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE] != null)
                hidAcademicEndDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToString();
            hidServiceEndDate.Value = oTransportLateFeeSetting.TransportEndDate.ToString();
            lstvwltfee.DataSource = lstTransportLateFeeSetting;
            lstvwltfee.DataBind();
        }

        if ((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] != Constants.UserRoles.Admin)
            btnCancel.Visible = false;
    }
   
    /// <summary>
    /// This method use read listview data and return list of that data
    /// </summary>
    /// <returns></returns>
    private List<TransportLateFeeDueDate> GetDueDates()
    {
        List<TransportLateFeeDueDate> lstTransportDueDate = new List<TransportLateFeeDueDate>();
         // Loop through all the list rows.
        foreach(ListViewDataItem oCurrentItem in lstvwltfee.Items)
        {
           
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            TextBox txtDueDate = (TextBox)oCurrentItem.FindControl("txtDueDate");
            TransportLateFeeDueDate oTransportDueDate = new TransportLateFeeDueDate()
            {
              Id=Convert.ToInt32(lstvwltfee.DataKeys[iRowId]["Id"]),
              DueDate =Convert.ToDateTime(txtDueDate.Text),
            };
            lstTransportDueDate.Add(oTransportDueDate);
        }
        return lstTransportDueDate;
    }
 
    /// <summary>
    /// This method use to pipulate latefeesettings value object
    /// </summary>
    private TransportLateFeeSetting Populate()
    {
        TransportLateFeeSetting oTransportLateFeeValue = new TransportLateFeeSetting()
        {
            ValueForType =Convert.ToInt32(txtValueForType.Text),
            LateFeePerTypeId = cmbFeeType.SelectedValue.ToInt(),
            LateFeeAmount =Convert.ToInt32(txtAmount.Text),
            InsertedById=miUserId,
        };
        return oTransportLateFeeValue;
    }
    /// <summary>
    /// This method call save transport late fee settings 
    /// </summary>
    private void SaveLateFeeSettings()
    {
        TransportLateFeeSetting oTransportLateFeeValue = Populate();
        List<TransportLateFeeDueDate> lstIdDueDate = GetDueDates();
        string sDueDateXml = base.GenerateXml(lstIdDueDate);
        moTransportLateFeeSettingsBL.Insert(sDueDateXml, oTransportLateFeeValue);
        lblSuccessMessage.Visible = true;
        lblSuccessMessage.Text = Resources.LocalizedResources.LateFeeSettingSavedSuccessfully;
    }

    /// <summary>
    /// This method use to refresh hidden field value base on language selected
    /// </summary>
    private void RefreshValue()
    {
        hidDueDateShouldNotBlank.Value = Resources.LocalizedResources.DueDateBlankVal;
        valsumLateFee.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        if (string.IsNullOrEmpty(hidServiceStartDate.Value) && string.IsNullOrEmpty(hidServiceStartDate.Value))
            lblServiceDuration.Text = Resources.LocalizedResources.TransportServiceDuration;
        else
            lblServiceDuration.Text = S_TRANSPORT_DURATION + hidServiceStartDate.Value.ToDateTime().ToString("dd-MMMM-yyyy") + " To " + hidServiceEndDate.Value.ToDateTime().ToString("dd-MMMM-yyyy");
    }
    #endregion


   
}