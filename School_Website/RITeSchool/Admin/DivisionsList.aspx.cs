// File Name     : DivisionsList.aspx.cs
// Modified By   : Amit
// Modified Date : 11/09/2009
// Description   : This class is used to save division configuration.

using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;

public partial class DivisionsList : SchoolBase
{
    #region Constants 

    const string S_SELECT_AT_LEAST_ONE_DIVISION = "At least one division name should be selected for saving.";
    
    #endregion

    #region Data Members

    private string IsConfig;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to initialize form controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                InitializeForm();
                FillDivisionGridView();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
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
    /// This event is used to fill textbox and checkbox in grid for associated divisions in school.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdGroupDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                // Set the Division name in the textbox.
                string sName = grdDivisions.DataKeys[e.Row.RowIndex][0].ToString();
                TextBox txtDivisionName = ((TextBox)e.Row.Cells[1].FindControl("txtDivisionName"));
                txtDivisionName.Text = sName;

                // If the school id is not the default id i.e. -9999 that means the Division is already assigned
                // to the school. Thus check the checkbox.
                if (grdDivisions.DataKeys[e.Row.RowIndex][3].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                    ((CheckBox)e.Row.FindControl("ChkBoxDelete")).Checked = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save divisions in school.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnSave_Click(object sender, EventArgs e)
    {
        // Save the changes to database.
        try
        {
            CheckBox Deleteflag = new CheckBox();
            Collection<DivisionMasterBL> oDivisions = new Collection<DivisionMasterBL>();

            for (int i = 0; i < grdDivisions.Rows.Count; i++)
            {
                Deleteflag = (CheckBox)grdDivisions.Rows[i].FindControl("ChkBoxDelete");
                TextBox txtPrefixEdit = ((TextBox)grdDivisions.Rows[i].FindControl("txtDivisionName"));

                // Check if new Division is being inserted.
                // I.e. If the checkbox is checked and the school id is -9999 then it is the new Division being
                // introduced.
                if (Deleteflag.Checked == true && grdDivisions.DataKeys[i][3].ToString() == Constants.S_DEFAUL_SCHOOL_ID)
                {
                    DivisionMasterBL oDivisionMasterBL = GetCommonDivisionMasterBL(txtPrefixEdit.Text,Convert.ToInt32(grdDivisions.DataKeys[i][2].ToString()));
                    oDivisionMasterBL.ConfigurationAction = Constants.Action.Insert;
                    oDivisions.Add(oDivisionMasterBL);
                }

                // Check if existing Division name is being updated.
                // I.e. If the checkbox is checked and the school is not -9999 and the value in text box differs 
                // from the value in the Division name column then update the existing Division name.
                else if (Deleteflag.Checked == true &&
                       grdDivisions.DataKeys[i][3].ToString() != Constants.S_DEFAUL_SCHOOL_ID &&
                       grdDivisions.DataKeys[i][0].ToString() != txtPrefixEdit.Text.Trim())
                {
                    DivisionMasterBL oDivisionMasterBL = GetCommonDivisionMasterBL(txtPrefixEdit.Text,Convert.ToInt32(grdDivisions.DataKeys[i][2].ToString()));
                    oDivisionMasterBL.ConfigurationAction = Constants.Action.Update;
                    oDivisionMasterBL.DivisionId = Convert.ToInt32(grdDivisions.DataKeys[i][1].ToString());
                    oDivisions.Add(oDivisionMasterBL);
                }

                // Check if existing Division is being removed.
                // I.e. If the checkbox is NOT checked and the school id is not -9999. 
                // In such case need to check if any of the related data is entered for the unchecked Division then
                // the warning message should be given to user and the related data should be removed from db.
                else if (Deleteflag.Checked == false && grdDivisions.DataKeys[i][3].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                {
                    DivisionMasterBL oDivisionMasterBL = GetCommonDivisionMasterBL(txtPrefixEdit.Text,Convert.ToInt32(grdDivisions.DataKeys[i][2].ToString()));
                    oDivisionMasterBL.ConfigurationAction = Constants.Action.Delete;
                    oDivisionMasterBL.DivisionId = Convert.ToInt32(grdDivisions.DataKeys[i][1].ToString());
                    oDivisions.Add(oDivisionMasterBL);
                }
            }
            // Update database with the configured Divisions.
            if (oDivisions.Count > 0)
            {
                DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId);
                oDivisionCollectionBL.UpdateDivisions(oDivisions, miAcademicYearId);
            }
            ReadQuerystring();
            if (IsConfig != "Y")
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.Division));
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration)));            
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
			lblErr.Text = CommonUtility.ModifyExceptionMessage(ex.Message, "Division", Resources.LocalizedResources.Division, "can not be removed since associated with", Resources.LocalizedResources.valRemoveText);
            FillDivisionGridView();            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods 
    
    /// <summary>
    /// This method is used to initialize form controls.
    /// </summary>
    private void InitializeForm()
    {
        grdDivisions.PageSize = 30;
        grdDivisions.EmptyDataText = Resources.LocalizedResources.NoRecordsFound;
        ViewState["DefaultSort"] = null;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";        
        ApplyMouseHoverEffect(new List<Button> { imgBtnSave, btnCancel });
        imgBtnSave.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdDivisions.AllowPaging + "','" + Resources.LocalizedResources.sSelectAtLeastOneDivision + "'))){return false;}");
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration));        
    }

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        try
        {
            if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
            {
                IsConfig = QueryString["Is_Configured"];
            }
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master; 
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    /// <summary>
    /// This method is used to fill division grid view.
    /// </summary>
    private void FillDivisionGridView()
    {
        // This method fills the Grid with available Group details.
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDTUserDetails = oDivisionCollectionBL.GetAllDivisions();
        grdDivisions.DataSource = oDTUserDetails.DefaultView;
        grdDivisions.DataBind();
        imgBtnSave.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdDivisions.AllowPaging + "','" + Resources.LocalizedResources.sSelectAtLeastOneDivision + "'))){return false;}");
    }

    /// <summary>
    /// This method is used to populate DivisionMasterBL for the division and returns the same.
    /// </summary>
    /// <param name="asFieldValue"></param>
    /// <param name="aiOriginalFieldId"></param>
    /// <returns></returns>
    private DivisionMasterBL GetCommonDivisionMasterBL(string asFieldValue, int aiOriginalFieldId)
    {
        // This method creates the default object for the configuration and returns the same.
        DivisionMasterBL oDivisionMasterBL = new DivisionMasterBL();
        oDivisionMasterBL.SchoolId = miSchoolId;
        oDivisionMasterBL.AcademicYearId = miAcademicYearId;
        oDivisionMasterBL.UpdatedById = miUserId;
        oDivisionMasterBL.InsertedByid = miUserId;
        oDivisionMasterBL.DivisionName = asFieldValue;
        oDivisionMasterBL.OriginalDivisionId = aiOriginalFieldId;

        return oDivisionMasterBL;

    }
    /// <summary>
    /// This method used to value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        grdDivisions.EmptyDataText = Resources.LocalizedResources.NoRecordsFound;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        imgBtnSave.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdDivisions.AllowPaging + "','" + Resources.LocalizedResources.sSelectAtLeastOneDivision + "'))){return false;}");
    }
   
    #endregion
}
