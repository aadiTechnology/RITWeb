// File Name     : SchoolwiseExamStatusConfigUI.aspx.cs
// Modified By   : Ashish 
// Modified Date : 22/10/2013
// Description   : This class is used to exam status for the school that will usefull to display on Reult ,Progress Report  .
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using StandardWiseExamConfigurationEntities;
using Utility;
using System.Web.UI;

/// <summary>
///This class use to edit and save schoolwise exam status 
/// </summary>
public partial class SchoolwiseExamStatusConfigUI : SchoolBase
{
    #region "Constants"
    
    private StandardWiseExamConfigurationBL moStandardWiseExamConfigurationBL = null;
    private const string S_BACK_COLOR_FOR_COMBO = "White";
    #endregion "Constants"

    #region "Events"
   
   /// <summary>
   /// This page load event use to fill color combo and fill listview
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStandardWiseExamConfigurationBL = new StandardWiseExamConfigurationBL(miSchoolId, miAcademicYearId);
            if (!IsPostBack)
            {
                RefreshValue();
                InitializeFields();
                FillExamStatusCombo();
                FillColorCombo();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    RefreshValue();
                }
            }

            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }

            if ((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Teacher)
            {
                btnBackEnd.Visible = false;
            }

            FillExamStatusList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
       
    }

    /// <summary>
    /// This event is used to save exam status details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveExamStatus();
            bool bIsConfigured = QueryString[Constants.S_IS_CONFIGURED] != Constants.S_YES;
            if (bIsConfigured)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.SchoolwiseExamStatus));
        }
        catch (SqlException ex)
        {
            lblSuccess.Visible = false;
            lblError.Visible = true;
            lblError.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event use to select exam status name from drop down box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbDisplayName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try 
        {
            if (cmbDisplayName.SelectedIndex != Constants.I_ZERO)
            {
                int iStatusId = Convert.ToInt32(cmbDisplayName.SelectedValue);
                ExamStatusConfiguration oExamStatusConfiguration = moStandardWiseExamConfigurationBL.GetExamStatusForSelectedStatusName(iStatusId);
                SetExamStatusDetails(oExamStatusConfiguration);
                SetColor();
            }
            else
                ResetControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event use to bind treu image when consider specific status for exam 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwExamStatus_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                HiddenField hidConsiderInTtl = oCurrentItem.FindControl("hidConsiderInTtl") as HiddenField;
                HiddenField hidDsplyTtl = oCurrentItem.FindControl("hidDsplyTtl") as HiddenField;
                Label lblBackColor = (Label)oCurrentItem.FindControl("lblForeColor");
                System.Web.UI.WebControls.Image oImgConsiderTotal = oCurrentItem.FindControl("imgConsiderInTotal") as System.Web.UI.WebControls.Image;
                System.Web.UI.WebControls.Image oImgDisplayTotal = oCurrentItem.FindControl("imgDisplayTotal") as System.Web.UI.WebControls.Image;
                System.Web.UI.WebControls.Image oImgConsiderAsPrsent = oCurrentItem.FindControl("imgConsiderAsPresent") as System.Web.UI.WebControls.Image;
                oImgConsiderTotal.Visible = hidConsiderInTtl.Value == Constants.S_YES;
                oImgDisplayTotal.Visible = hidDsplyTtl.Value == Constants.S_YES;
                var tableRow3 = oCurrentItem.FindControl("tdforecolor") as HtmlTableCell;
                if (tableRow3 != null)
                    tableRow3.Style.Add(HtmlTextWriterStyle.Color, lblBackColor.Text);
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This method use to send user on exam related link after click on btnbackend
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBackEnd_Click(object sender, EventArgs e)
    {
        Response.Redirect(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related)), false);
    }

    /// <summary>
    /// This button clear all controls on the screen
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        SetColor();
        cmbForColor.Attributes.Add("style", "background-color:" + S_BACK_COLOR_FOR_COMBO);
    }

   #endregion "Events"

    #region "Private Method"
   
    /// <summary>
    /// This method is used to initialize control values.
    /// </summary>
    private void InitializeFields()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBackEnd, });
        cmbForColor.Attributes.Add("onclick", "ChangeColor(this)");
    }

    /// <summary>
    /// This method is used to fill user role combo box.
    /// </summary>
    private void FillExamStatusCombo()
    {
        List<ExamStatusConfiguration> olstExamStatus = moStandardWiseExamConfigurationBL.GetSchoolwiseExamStatusConfiguration();
        ListSource.FillDropDownList(olstExamStatus,cmbDisplayName,
                                      "DisplayName",
                                      "ExamStatusId",
                                      Constants.S_SELECT);
    }
 
    /// <summary>
    /// This mathod use to save exam status configuration details 
    /// </summary>
    private void SaveExamStatus()
    {
        ExamStatusConfiguration oExamStatusConfiguration = Populate();
        moStandardWiseExamConfigurationBL.UpdateExamStatusConfiguration(oExamStatusConfiguration);
        cmbForColor.Attributes.Add("style", "background-color:" + S_BACK_COLOR_FOR_COMBO);
        SetColor();
        lblSuccess.Text = "<b>" + Resources.LocalizedResources.UpdateExamStatus + "</b>";
        FillExamStatusList();
        ResetControls();
        lblSuccess.Visible = true;
        lblSuccess.Text = "Exam Status saved successfully !!!";
    }
    
    /// <summary>
    /// This method is used to fill color combobox;
    /// </summary>
    /// <param name="cmbColors"></param>
    private void  FillColorCombo()
    {
        Type tColors = typeof(Color);
        PropertyInfo[] oPropInfoArr = tColors.GetProperties(BindingFlags.Static | BindingFlags.Public);
        ListItem oListItemSelect = new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO };
        oListItemSelect.Attributes.Add("style", "background-color:white;");
        cmbForColor.Items.Add(oListItemSelect);
        foreach (PropertyInfo oProperty in oPropInfoArr)
        {
            if (oProperty.DeclaringType.Equals(typeof(Color)))
            {
                ListItem oListItem = new ListItem { Text = oProperty.Name, Value = oProperty.Name };
                oListItem.Attributes.Add("style", "background-color:" + oProperty.Name);
                ListItem oListItemForBackColor = new ListItem { Text = oProperty.Name, Value = oProperty.Name };
                oListItemForBackColor.Attributes.Add("style", "background-color:" + oProperty.Name);
                cmbForColor.Items.Add(oListItem);
            }
        }
    }

    /// <summary>
    /// This method use to populate ExamStatusConfiguration object that use at save time
    /// </summary>
    /// <returns></returns>
    private ExamStatusConfiguration Populate()
    {
        ExamStatusConfiguration oExamStatusConfiguration = new ExamStatusConfiguration
                                                           {
                                                               ExamStatusId = Convert.ToInt32(cmbDisplayName.SelectedValue),
                                                               ForeColor = cmbForColor.SelectedItem.Text.Trim(),
                                                               ConsiderInTotal = chkbxConsiderInTotal.Checked ? Constants.C_YES : Constants.C_NO,
                                                               DisplayTotal = chkbxDisplayTotal.Checked ? Constants.C_YES : Constants.C_NO,
                                                            };
        return oExamStatusConfiguration;
    }
    
    
    /// <summary>
    /// This mothod use to reset all controls 
    /// </summary>
    private void ResetControls()
    {
        lblStatusDisplayValue.Text = " ";
        lblSuccess.Visible = false;
        cmbDisplayName.SelectedValue =Constants.S_ZERO;
        cmbForColor.SelectedValue = Constants.S_ZERO;
        chkbxConsiderInTotal.Checked = false;
        chkbxDisplayTotal.Checked=false;
        cmbForColor.Attributes.Add("style", "background-color:" + S_BACK_COLOR_FOR_COMBO);
     }
   
    /// <summary>
    /// This method use to set exam status information to the respective controls 
    /// </summary>
    /// <param name="aoExamStatusConfiguration"></param>
    private void SetExamStatusDetails(ExamStatusConfiguration aoExamStatusConfiguration)
    {
        lblStatusDisplayValue.Text = aoExamStatusConfiguration.DisplayValue;
        chkbxDisplayTotal.Checked = aoExamStatusConfiguration.DisplayTotal == Constants.C_YES;
        chkbxConsiderInTotal.Checked = aoExamStatusConfiguration.ConsiderInTotal == Constants.C_YES;
        cmbForColor.SelectedValue=aoExamStatusConfiguration.ForeColor;
        cmbForColor.Attributes.Add("style", "background-color:" + aoExamStatusConfiguration.ForeColor);
    }
 
    /// <summary>
    /// This method is used fill listview of homework assigned by selected date.
    /// </summary>
    private void FillExamStatusList()
    {
        List<ExamStatusConfiguration> lstExamStatusConfiguration = moStandardWiseExamConfigurationBL.GetSchoolwiseExamStatusConfiguration();
        lstvwExamStatus.DataSource = lstExamStatusConfiguration;
        lstvwExamStatus.DataBind();
    }
    
    /// <summary>
    /// This method use to refresh controls value when culture changes
    /// </summary>
    private void RefreshValue()
    {
        hidCstShortNameDuplicate.Value = Resources.LocalizedResources.CstShortNameDuplicate;
        hidCstForeColorDuplicate.Value = Resources.LocalizedResources.CstForeColorDuplicate;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
    }

    /// <summary>
    /// This method use to set color of fore and back color combo
    /// </summary>
    private void SetColor()
    {
        foreach (ListItem oListItem in cmbForColor.Items)
        {
            oListItem.Attributes.Add("style", "background-color:" + oListItem.Text);
        }
    }
    #endregion
   
   
}