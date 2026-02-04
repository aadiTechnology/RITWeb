// File Name  : StandardwiseFeeConfigurationDetails.aspx.cs
// Created By : Anugandha
// Date       : 07/02/2008
//Description :This class is used to set fee amount to 
//             fee subtypes and also calculate total fee.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class StandardwiseFeeDetails : SchoolBase
{
    #region Constants

    private const int I_FEE_SUBTYPE_ID = 0;
    private const int I_ISCONFIGURED = 4;
    private const int I_FEETYPE_FOR_NEW_STUDENT = 1;
    private const int I_FEETYPE_FOR_OLD_STUDENT = 2;
    private const int I_FEE_SUBTYPES = 1;
    private const int I_FEE_AMOUNT = 2;
    private const int I_CHECKED = 0;
    private const string S_ERR_MSG_ADMIN = "Please contact your administrator for fee subtype configuration.";

    #endregion

    #region Data members

    private int miStandardId;
    private int miFeeTypeId;
    private int miStandardFeeConfigId;
    private string msFeeType;
    private string msStd;
    private DataSet moStandardwiseFeeConfigDetails;

    #endregion    

    #region Events

    /// <summary>
    /// This method is used to get parameter values.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (CheckPreCondition())
            {
                bool bIsUseSubmitBehavior = CommonUtility.CheckCancelOrBackClickEvent(this.Page);
                if (bIsUseSubmitBehavior == true)
                {
                    ReadQueryString();
                    SchoolwiseStandardFeeConfigurationMasterBL oSchoolwiseStandardFeeConfigurationMasterBL = new SchoolwiseStandardFeeConfigurationMasterBL();
                    if (hidMode.Value.Equals(Constants.ViewMode.Edit.ToString()))
                    {
	                    if (QueryString["Schoolwise_Standard_Fee_Configuration_Id"] != null)
		                    miStandardFeeConfigId = QueryString["Schoolwise_Standard_Fee_Configuration_Id"].ToInt();
                    }
                }

                if (!IsPostBack)
                {
                    if (CheckPreCondition())
                    {
                        if (Session[Constants.S_SESSION_LANGUAGE] != null)
                        {
                            hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                        }
                        DesignSettingAccordingLanguage();
                        InitializeFields();
                        InitializeAcademicDates();                        
                    }
                }
                if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    DesignSettingAccordingLanguage();
                }
            }

            ApplyMouseHoverEffect(new List<Button> { btnSave, btnBack, btnOk, btnCancel });
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to navigate back to StandardwiseFeeConfigurationUI screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        string S_PAGE = "~/Admin/StandardwiseFeeConfigurationUI.aspx";
        try
        {
            string sQuerystring = "Is_Configured=" + hidIsConfig.Value;
            string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
            string sRedirectUrl = S_PAGE + "?" + sEncrypt;
            MasterPage oMasterPage = (MasterPage)this.Master; 
            oMasterPage.RedirectToNextPage(sRedirectUrl);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used for saving fee details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string S_PAGE = "~/Admin/StandardwiseFeeConfigurationUI.aspx";
        try
        {
            SchoolwiseStandardFeeConfigurationMasterBL oSchoolwiseStandardFeeConfigurationMasterBL = new SchoolwiseStandardFeeConfigurationMasterBL();
            Collection<SchoolwiseStandardFeeConfigurationDetailsBL> oFeeDetailCollection = new Collection<SchoolwiseStandardFeeConfigurationDetailsBL>();
            oFeeDetailCollection = PopulateFeelCollection();
            oSchoolwiseStandardFeeConfigurationMasterBL = PopulateStandardFeeConfigMasterBL();
            UpdateStandardFeeDetails(oSchoolwiseStandardFeeConfigurationMasterBL, oFeeDetailCollection);			

            string sQuerystring = "Is_Configured=" + hidIsConfig.Value;
            string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerystring);
            string sRedirectUrl = S_PAGE + "?" + sEncrypt;
            MasterPage oMasterPage = (MasterPage)this.Master; oMasterPage.RedirectToNextPage(sRedirectUrl);
			if (Settings.IsRTEApplicable)
			{
				StudentBL oStudentBL = new StudentBL();
				List<int> lstRTEStudIDs = oStudentBL.GetStandardwiseRTEStudentIDs(miSchoolId, miAcademicYearId, miStandardId);
				if (lstRTEStudIDs != null)
				{
					StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
					Accounts oAccounts = new Accounts();					
					foreach (var iStudentId in lstRTEStudIDs)
					{	
						string sReceiptNumber = oStudentFeeDetailsBL.AddConcessionForRTEStudent(iStudentId,miSchoolId,miAcademicYearId);

						// Create a fee voucher for the fee concession for RTE student.
						if (Settings.EnableAccountsModule)
						{							
							oAccounts.RecordCashPaymentForFeeConcession(iStudentId, sReceiptNumber);
						}
					}
				}
			}

        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblErr.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Grid Events

    /// <summary>
    /// This method is used to set attribute to textbox of fee amount ,
    /// to enable or disable textbox and to checked or unchecked checkbox of grid. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdFeeTypes_rowDatabound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int iRowIndex = e.Row.RowIndex;
            if (iRowIndex >= Constants.I_ZERO)
            {
                SchoolwiseFeeSubTypeConfigurationCollectionBL oSchoolwiseFeeSubTypeConfigurationCollectionBL = new SchoolwiseFeeSubTypeConfigurationCollectionBL();
                DataSet oDSStandardFeeDetails = oSchoolwiseFeeSubTypeConfigurationCollectionBL.GetConfiguredFeeSubType(miSchoolId, miAcademicYearId, miStandardId, miFeeTypeId);
                CheckBox chkFeeSubType = ((CheckBox)e.Row.Cells[I_FEETYPE_FOR_NEW_STUDENT].FindControl("chkFeeSubType"));
                TextBox txtFeeAmountOld = ((TextBox)e.Row.Cells[I_FEETYPE_FOR_NEW_STUDENT].FindControl("txtFeeAmountForOld"));
                txtFeeAmountOld.Attributes.Add("onChange", "SetTotals()");

                TextBox txtTotalFeeNew = ((TextBox)e.Row.Cells[I_FEETYPE_FOR_OLD_STUDENT].FindControl("txtFeeAmountForNew"));
                txtTotalFeeNew.Attributes.Add("onChange", "SetTotals()");

                HiddenField hidFeeSubTypeForNew = (HiddenField)e.Row.Cells[I_FEETYPE_FOR_NEW_STUDENT].FindControl("hidFeeSubTypeForNew");
                hidFeeSubTypeForNew.Value = e.Row.Cells[I_FEETYPE_FOR_NEW_STUDENT].Text;

                HiddenField hidFeeSubTypeForOld = (HiddenField)e.Row.Cells[I_FEETYPE_FOR_NEW_STUDENT].FindControl("hidFeeSubTypeForOld");
                hidFeeSubTypeForOld.Value = e.Row.Cells[I_FEETYPE_FOR_OLD_STUDENT].Text;

                chkFeeSubType.Attributes.Add("onclick", "EnableDisableGridTextBox(this," + iRowIndex + ")");
                string sIsConfig = oDSStandardFeeDetails.Tables[I_FEE_SUBTYPES].Rows[iRowIndex][I_ISCONFIGURED].ToString();

                if (oDSStandardFeeDetails.Tables[I_FEE_SUBTYPES].Rows.Count >= 1)
                {
                    EnableOrDisableControls(sIsConfig, chkFeeSubType, txtFeeAmountOld);
                    EnableOrDisableControls(sIsConfig, chkFeeSubType, txtTotalFeeNew);
                }
                else
                {
                    chkFeeSubType.Checked = false;
                    txtFeeAmountOld.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is for the AllowPaging propetry of the grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param> 
    protected void grdFeeTypes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdFeeTypes.PageIndex = e.NewPageIndex;
            FillFeeSubTypeGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to apply style for paging.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdFeeTypes_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Table PagerTable = (Table)e.Row.Cells[0].Controls[0];
                PagerTable.CssClass = "ClsNwGridPaging";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This function is used to initialize academic year dates.
    /// </summary>
    private void InitializeAcademicDates()
    {
        btnBack.Attributes["onclick"] = "javascript:DisableButtons()";
        hidStartDate.Value = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE]).ToString(Constants.S_STANDARD_DATE_FORMAT);
        hidEndDate.Value = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE]).ToString(Constants.S_STANDARD_DATE_FORMAT);
    }

    /// <summary>
    /// This method is used to decrypt querystring and get parameter values.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["Standard_Id"] != null)
            miStandardId = QueryString["Standard_Id"].ToInt();
        if (QueryString["Fee_Type_Id"] != null)
            miFeeTypeId = QueryString["Fee_Type_Id"].ToInt();
        if (QueryString["ViewMode"] != null)
            hidMode.Value = QueryString["ViewMode"];
        if (QueryString["FeeType"] != null)
            msFeeType = QueryString["FeeType"];
        if (QueryString["Std"] != null)
            msStd = QueryString["Std"];
        hidIsConfig.Value = QueryString["Is_Configured"];
        SetStandardAndFeeType(msFeeType, msStd);
    }

    /// <summary>
    /// This method is used to get feetype name.
    /// </summary>
    /// <param name="aiFeeTypeId"></param>
    private void SetStandardAndFeeType(string asFeeType, string asStdName)
    {
        lblStandard.Text = asStdName;
        lblFeeType.Text = asFeeType;
    }

    /// <summary>
    /// This method is used to fill grid of feesubtype.
    /// </summary>
    private void FillFeeSubTypeGrid()
    {
        SchoolwiseFeeSubTypeConfigurationCollectionBL oSchoolwiseFeeSubTypeConfigurationCollectionBL =
                               new SchoolwiseFeeSubTypeConfigurationCollectionBL(miSchoolId, miAcademicYearId);
        DataSet oDSFeeSubTypes = oSchoolwiseFeeSubTypeConfigurationCollectionBL.GetConfiguredFeeSubType(miSchoolId, miAcademicYearId, miStandardId, miFeeTypeId);
        grdFeeTypes.DataSource = oDSFeeSubTypes.Tables[I_FEE_SUBTYPES];
        grdFeeTypes.DataBind();
        moStandardwiseFeeConfigDetails = oDSFeeSubTypes;

        if (oDSFeeSubTypes.Tables.Count > 1)
        {
            hidOldStudentAmt.Value = oDSFeeSubTypes.Tables[I_FEE_AMOUNT].Rows[0]["AmountForOldStudent"].ToString();
            hidNewStudentAmt.Value = oDSFeeSubTypes.Tables[I_FEE_AMOUNT].Rows[0]["AmountForNewStudent"].ToString();
        }

        for (int iCellCount = 0; iCellCount < grdFeeTypes.Rows.Count; iCellCount++)
        {
            CheckBox chkFeeSubType = (CheckBox)(grdFeeTypes.Rows[iCellCount].Cells[I_FEE_SUBTYPE_ID].FindControl("chkFeeSubType"));
            TextBox txtFeeAmtNew = (TextBox)(grdFeeTypes.Rows[iCellCount].Cells[I_FEE_AMOUNT].FindControl("txtFeeAmountForNew"));
            if (chkFeeSubType.Checked && txtFeeAmtNew != null)
            {
                txtFeeAmtNew.Focus();
                return;
            }
        }
    }

    /// <summary>
    /// This method is used to set properties of SchoolwiseStandardFeeConfigurationMasterBL class.
    /// </summary>
    /// <returns>SchoolwiseStandardFeeConfigurationMasterBL</returns>
    private SchoolwiseStandardFeeConfigurationMasterBL PopulateStandardFeeConfigMasterBL()
    {
        SchoolwiseStandardFeeConfigurationMasterBL oSchoolwiseStandardFeeConfigurationMasterBL = new SchoolwiseStandardFeeConfigurationMasterBL();
        oSchoolwiseStandardFeeConfigurationMasterBL.School_Id = miSchoolId;
        oSchoolwiseStandardFeeConfigurationMasterBL.academic_Year_Id = miAcademicYearId;
        oSchoolwiseStandardFeeConfigurationMasterBL.Fee_Type_Id = miFeeTypeId;
        oSchoolwiseStandardFeeConfigurationMasterBL.Inserted_By_id = Convert.ToString(miUserId);
        oSchoolwiseStandardFeeConfigurationMasterBL.Standard_Id = miStandardId;
        oSchoolwiseStandardFeeConfigurationMasterBL.Total_FeesForOld= Convert.ToDouble(txtTotalFeeOld.Text);
        oSchoolwiseStandardFeeConfigurationMasterBL.Total_FeesForNew = Convert.ToDouble(txtTotalFeeNew.Text);

        if (hidMode.Value.Equals(Constants.ViewMode.Edit.ToString()))
        {
            oSchoolwiseStandardFeeConfigurationMasterBL.Schoolwise_Standard_Fee_Configuration_Id = miStandardFeeConfigId;
            if (hidIsStudentPayFee.Value == Constants.S_YES)
            {
                oSchoolwiseStandardFeeConfigurationMasterBL.DueDate = Convert.ToDateTime(txtDueDate.Text);
                oSchoolwiseStandardFeeConfigurationMasterBL.AmountForNewStudent = Convert.ToInt32(hidNewStudentAmt.Value);
                oSchoolwiseStandardFeeConfigurationMasterBL.AmountForOldStudent = Convert.ToInt32(hidOldStudentAmt.Value);
                oSchoolwiseStandardFeeConfigurationMasterBL.IsStudentPayFee = true;
            }
            else
                oSchoolwiseStandardFeeConfigurationMasterBL.IsStudentPayFee = false;

            oSchoolwiseStandardFeeConfigurationMasterBL.ConfigurationAction = Constants.Action.Update;
        }
        else
            oSchoolwiseStandardFeeConfigurationMasterBL.ConfigurationAction = Constants.Action.Insert;

        return oSchoolwiseStandardFeeConfigurationMasterBL;
    }

    /// <summary>
    /// This method is used to set properties of SchoolwiseStandardFeeConfigurationDetailsBL class.
    /// </summary>
    /// <param name="aiFee_SubType"></param>
    /// <param name="aiFeeAmount"></param>
    /// <returns></returns>
    private SchoolwiseStandardFeeConfigurationDetailsBL PopulateStandardFeeConfigDetailBL(int aiFee_SubType, double aiFeeAmountOld, double aiFeeAmountNew)
    {
        SchoolwiseStandardFeeConfigurationDetailsBL oSchoolwiseStandardFeeConfigurationDetailsBL;
        return oSchoolwiseStandardFeeConfigurationDetailsBL = new SchoolwiseStandardFeeConfigurationDetailsBL
        {
            Schoolwise_Standard_Fee_Configuration_Id = miStandardFeeConfigId,
            School_Id = miSchoolId,
            academic_Year_Id = miAcademicYearId,
            Fee_SubType_Id = aiFee_SubType,
            Fee_AmountOld = aiFeeAmountOld,
            Fee_AmountNew = aiFeeAmountNew,
            Standard_Id = miStandardId,
            Inserted_By_id = Convert.ToString(miUserId)
        };        
    }

    /// <summary>
    /// This function checks the preconditons for Standardwise Fee Configuration Details configuration.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.StandardwiseFeeConfiguration);
        if (sLinks.IsNullOrEmpty())
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            VisibleOrHideControls();
        }

        return bReturn;
    }

    /// <summary>
    /// This method is used to visible or hide controls depends on configuration is done or not.
    /// </summary>
    private void VisibleOrHideControls()
    {
        pnlContainer.Visible = false;
        divGridView.Visible = false;
        btnSave.Visible = false;
        btnBack.Visible = true;
    }

    /// <summary>
    /// This method is used to get values from querystring,to fill grid and to set value to control.
    /// </summary>
    private void InitializeFields()
    {
        //Set attribute to save button.
        btnSave.Attributes.Add("onclick", "if(!(ValidateFee(this))){return false;}");
        FillFeeSubTypeGrid();

        //set last saved value.
        if (hidMode.Value.Equals(Constants.ViewMode.Edit.ToString()))
        {
            if (QueryString["Schoolwise_Standard_Fee_Configuration_Id"] != null)
            {
                int iStdFeeConfigId = QueryString["Schoolwise_Standard_Fee_Configuration_Id"].ToInt();
                DataRow[] oDRtxttotFee = moStandardwiseFeeConfigDetails.Tables[0].Select("Schoolwise_Standard_Fee_Configuration_Id=" + iStdFeeConfigId);
                
                txtTotalFeeOld.Text = oDRtxttotFee[0]["OldStudent_TotalFee"].ToString();
                hidOldStudentAmt.Value = oDRtxttotFee[0]["OldStudent_TotalFee"].ToString();

                txtTotalFeeNew.Text = oDRtxttotFee[0]["NewStudent_TotalFee"].ToString();
                hidNewStudentAmt.Value = oDRtxttotFee[0]["NewStudent_TotalFee"].ToString();
            }
        }

        SchoolwiseStandardFeeConfigurationMasterBL oSchoolwiseStandardFeeConfigurationMasterBL = new SchoolwiseStandardFeeConfigurationMasterBL();
        oSchoolwiseStandardFeeConfigurationMasterBL.academic_Year_Id = miAcademicYearId;
        oSchoolwiseStandardFeeConfigurationMasterBL.School_Id = miSchoolId;
        oSchoolwiseStandardFeeConfigurationMasterBL.Schoolwise_Standard_Fee_Configuration_Id = miStandardFeeConfigId;
        string sMSg = oSchoolwiseStandardFeeConfigurationMasterBL.CheckDependenciesForFees(lblFeeType.Text);

        if (sMSg != string.Empty)
            hidIsStudentPayFee.Value = Constants.S_YES;
        else
            hidIsStudentPayFee.Value = Constants.S_NO;
    }

    /// <summary>
    /// This method is used to set configuration action as well set fee amount and make collection object.
    /// </summary>
    /// <returns>Collection<SchoolwiseStandardFeeConfigurationDetailsBL></returns>
    private Collection<SchoolwiseStandardFeeConfigurationDetailsBL> PopulateFeelCollection()
    {
        Collection<SchoolwiseStandardFeeConfigurationDetailsBL> oFeeDetailCollection = new Collection<SchoolwiseStandardFeeConfigurationDetailsBL>();
        int iFeeSubTypeId = 0;
        double iFeeSubTypeAmountOld = 0, iFeeSubTypeAmountNew = 0;
        CheckBox ochkFeeSubType;
        double iTotFeeAmtOld = 0, iTotFeeAmtNew=0;
        for (int iRowCount = 0; iRowCount < grdFeeTypes.Rows.Count; iRowCount++)
        {
            ochkFeeSubType = (CheckBox)(grdFeeTypes.Rows[iRowCount].Cells[I_CHECKED].FindControl("chkFeeSubType"));
            iFeeSubTypeId = Convert.ToInt32(grdFeeTypes.DataKeys[iRowCount][I_FEE_SUBTYPE_ID].ToString());
            TextBox txtFeeAmtOld = (TextBox)(grdFeeTypes.Rows[iRowCount].Cells[I_FEETYPE_FOR_OLD_STUDENT].FindControl("txtFeeAmountForOld"));
            TextBox txtFeeAmtNew = (TextBox)(grdFeeTypes.Rows[iRowCount].Cells[I_FEETYPE_FOR_NEW_STUDENT].FindControl("txtFeeAmountForNew"));
            iFeeSubTypeAmountOld = 0;
            iFeeSubTypeAmountNew = 0;

            if (ochkFeeSubType.Checked == true)
            {
                iFeeSubTypeAmountOld = Convert.ToDouble(txtFeeAmtOld.Text);
                iFeeSubTypeAmountNew = Convert.ToDouble(txtFeeAmtNew.Text);
                SchoolwiseStandardFeeConfigurationDetailsBL oSchoolwiseStandardFeeConfigurationDetailsBL = PopulateStandardFeeConfigDetailBL(iFeeSubTypeId, iFeeSubTypeAmountOld, iFeeSubTypeAmountNew);
                oSchoolwiseStandardFeeConfigurationDetailsBL.ConfigurationAction = Constants.Action.Insert;
                oFeeDetailCollection.Add(oSchoolwiseStandardFeeConfigurationDetailsBL);                
                iTotFeeAmtOld = iTotFeeAmtOld + iFeeSubTypeAmountOld;
                iTotFeeAmtNew = iTotFeeAmtNew + iFeeSubTypeAmountNew;
            }
        }

        txtTotalFeeOld.Text = iTotFeeAmtOld.ToString();
        txtTotalFeeNew.Text = iTotFeeAmtNew.ToString();
        return oFeeDetailCollection;
    }

    /// <summary>
    /// This method is used to save fee details.
    /// </summary>
    /// <param name="aoStandardFeeConfigMasterBL"></param>
    /// <param name="aoFeeDetailCollection"></param>
    private void UpdateStandardFeeDetails(SchoolwiseStandardFeeConfigurationMasterBL aoStandardFeeConfigMasterBL, Collection<SchoolwiseStandardFeeConfigurationDetailsBL> aoFeeDetailCollection)
    {
        if (aoFeeDetailCollection.Count > 0)
        {
            aoStandardFeeConfigMasterBL.SchoolWiseFeeSubTypeCollection = aoFeeDetailCollection;
            aoStandardFeeConfigMasterBL.UpdateStandardFeeTypes(aoFeeDetailCollection, lblFeeType.Text);

            if (hidIsConfig.Value != Constants.S_YES)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.StandardwiseFeeConfiguration));
        }
    }

    /// <summary>
    /// This method is used to enable or disable controls.
    /// </summary>
    /// <param name="sIsConfig"></param>
    /// <param name="chkFeeSubType"></param>
    /// <param name="txtFeeAmount"></param>
    private void EnableOrDisableControls(String sIsConfig, CheckBox chkFeeSubType, TextBox txtFeeAmount)
    {
        if (sIsConfig != Convert.ToString(Constants.C_NO))
        {
            chkFeeSubType.Checked = true;
            txtFeeAmount.Enabled = true;
        }
        else
        {
            chkFeeSubType.Checked = false;
            txtFeeAmount.Enabled = false;
        }
    }
    /// <summary>
    /// This method is used to set design according to selected language.
    /// </summary>
    private void DesignSettingAccordingLanguage()
    {
        valSave.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidPleaseFixFollowingErrors.Value = Resources.LocalizedResources.PleaseFixFollowingError;
        hidAtLeastOneFeeSubTypeShouldBeSelectedForSaving.Value = Resources.LocalizedResources.AtLeastOneFeeSubTypeShouldBeSelectedForSaving;
        hidFeeAmountShouldNotBe0ForFollowingSubTypes.Value = Resources.LocalizedResources.FeeAmountShouldNotBe0ForFollowingSubTypes;
        hidUpdatedFeeAmountShouldBeGreaterThanPreviousAmount.Value = Resources.LocalizedResources.UpdatedFeeAmountShouldBeGreaterThanPreviousAmount;
        hidAreYouSureYouWantToReviseTheFeeStructure.Value = Resources.LocalizedResources.AreYouSureYouWantToReviseTheFeeStructure;
        hidAreYouSureYouWantToReturnThisBook.Value = Resources.LocalizedResources.AreYouSureYouWantToReturnThisBook;
        hidDueDateShouldNotBeBlank.Value = Resources.LocalizedResources.DueDateShouldNotBeBlank;
    }
    
    #endregion
}
