// File Name    : WeekDaysConfiguration.aspx   
// Created By   : Ketan     
// Created Date : 27/11/2007

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.ServiceModel;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using Utility;

/// <summary>
///		This class provides user interface to configure and save the Weekdays.
/// </summary>
public partial class FeeTypeConfiguration : SchoolBase
{
	#region -- CONSTANT(s) --

	private const string S_ERR_MSG_SELECT_FEE_TYPE = "At least one Fee Type Name should be selected for saving.";
	private const int I_CHECKBOX_COLUMN_NUMBER = 0;
	private const int I_SCHOOL_ID_COLUMN_NUMBER = 1; // datakey
	private const int I_ORIGINAL_FEETYPE_ID_COLUMN_NUMBER = 2; // datakey	
	private const int I_FEETYPE_COLUMN_NUMBER = 1;
	private const int I_ITR_COLUMN_NUMBER = 2;
	private const int I_BIFURCATION_COLUMN_NUMBER = 3;
    private const int I_RTE_COLUMN_NUMBER = 4;
	private const string S_CHECKBOX_FEETYPE = "ChkAllCheckedWeekDays";
	private const string S_TEXTBOX = "txtFeeType";    

	#endregion -- CONSTANT(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	///		Handles the page load event of the Page.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
                RefreshValue();
				ReadQuerystring();
				FillGridView();
				SetDefaultValues();
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
                SetDefaultValues();
            }
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is used to bind data rowwise to the grid.  
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void grdFeeTypeConfiguration_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			if (e.Row.RowIndex < Constants.I_ZERO)
				return;
			
			var chkIsSelected = e.Row.Cells[I_CHECKBOX_COLUMN_NUMBER].FindControl(S_CHECKBOX_FEETYPE) as CheckBox;
			var txtFeeType = e.Row.Cells[I_FEETYPE_COLUMN_NUMBER].FindControl(S_TEXTBOX) as TextBox;
			var chkITR = e.Row.Cells[I_ITR_COLUMN_NUMBER].FindControl("chkITR") as CheckBox;
			var chkBifurcate = e.Row.Cells[I_BIFURCATION_COLUMN_NUMBER].FindControl("chkBifurcate") as CheckBox;
            var chkRTE = e.Row.Cells[I_RTE_COLUMN_NUMBER].FindControl("chkRTE") as CheckBox;

			chkIsSelected.Attributes.Add("onclick", "ChkOnChange(" + e.Row.RowIndex + ")");
			if (hidConfigurationFlag.Value == Constants.S_YES)
			{
				if (grdFeeTypeConfiguration.DataKeys[e.Row.RowIndex][I_SCHOOL_ID_COLUMN_NUMBER].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
				{
					chkIsSelected.Checked = true;
					txtFeeType.Enabled = true;
					chkITR.Checked = grdFeeTypeConfiguration.DataKeys[e.Row.RowIndex]["ConsiderForITReconciliation"].ToBool();
					chkBifurcate.Checked = grdFeeTypeConfiguration.DataKeys[e.Row.RowIndex]["ConsiderForBifurcation"].ToBool();
                    chkRTE.Checked = grdFeeTypeConfiguration.DataKeys[e.Row.RowIndex]["ConsiderForRTEConcession"].ToBool();
                    chkRTE.Enabled = false;
				}
				else
				{
					txtFeeType.Enabled = false;
					chkITR.InputAttributes["disabled"] = "disabled";
					chkBifurcate.InputAttributes["disabled"] = "disabled";
                    chkRTE.InputAttributes["disabled"] = "disabled";
				//	chkITR.InputAttributes.Remove("disabled");
				}
			}
			else
			{
				chkIsSelected.Checked = false;
				txtFeeType.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is used to populate data and insert into DB. 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSave_Click(object sender, EventArgs e)
	{	  
		try
		{
            CheckBox chkDeleteflag, chkITR, chkBifurcate, chkRTE;
			var oFeeTypeCollection = new Collection<SchoolwiseFeeTypeConfigurationBL>();
			SchoolwiseFeeTypeConfigurationBL oSchoolwiseFeeTypeConfigurationBL;            
			var oSchoolwiseFeeTypeConfigurationCollectionBL = new SchoolwiseFeeTypeConfigurationCollectionBL();
			
			for (int iCount = 0; iCount < grdFeeTypeConfiguration.Rows.Count; iCount++)
			{
				chkDeleteflag = grdFeeTypeConfiguration.Rows[iCount].FindControl(S_CHECKBOX_FEETYPE) as CheckBox;
				chkITR = grdFeeTypeConfiguration.Rows[iCount].FindControl("chkITR") as CheckBox;
				chkBifurcate = grdFeeTypeConfiguration.Rows[iCount].FindControl("chkBifurcate") as CheckBox;
                chkRTE = grdFeeTypeConfiguration.Rows[iCount].FindControl("chkRTE") as CheckBox;
				string sFeeType = String.Empty;
				var txtFeeType = grdFeeTypeConfiguration.Rows[iCount].FindControl(S_TEXTBOX) as TextBox;
				
				if (txtFeeType != null)
					sFeeType = txtFeeType.Text.Trim();
				
				int iOrgFeeTypeID = grdFeeTypeConfiguration.DataKeys[iCount][I_ORIGINAL_FEETYPE_ID_COLUMN_NUMBER].ToInt();
				int iFeeTypeID = grdFeeTypeConfiguration.DataKeys[iCount][0].ToInt();
				
				if (chkDeleteflag.Checked && grdFeeTypeConfiguration.DataKeys[iCount][1].ToString() == Constants.S_DEFAUL_SCHOOL_ID)
				{
                    oSchoolwiseFeeTypeConfigurationBL = PopulateFeeTypeConfigurationBL(sFeeType, iOrgFeeTypeID, chkITR.Checked, chkBifurcate.Checked, chkRTE.Checked);
					oSchoolwiseFeeTypeConfigurationBL.ConfigurationAction = Constants.Action.Insert;					
					oFeeTypeCollection.Add(oSchoolwiseFeeTypeConfigurationBL);
				}
				// Check if existing standard name is being updated.
				// I.e. If the checkbox is checked and the school is not -9999 and the value in text box differs 
				// from the value in the standard name column then update the existing standard name.
				else if (chkDeleteflag.Checked &&
						 grdFeeTypeConfiguration.DataKeys[iCount][1].ToString() != Constants.S_DEFAUL_SCHOOL_ID &&
						 grdFeeTypeConfiguration.Rows[iCount].Cells[1].Text != sFeeType)
				{
                    oSchoolwiseFeeTypeConfigurationBL = PopulateFeeTypeConfigurationBL(sFeeType, iOrgFeeTypeID, chkITR.Checked, chkBifurcate.Checked, chkRTE.Checked);
					oSchoolwiseFeeTypeConfigurationBL.ConfigurationAction = Constants.Action.Update;
					oSchoolwiseFeeTypeConfigurationBL.Fee_Type_Id = grdFeeTypeConfiguration.DataKeys[iCount][0].ToInt();					
					oFeeTypeCollection.Add(oSchoolwiseFeeTypeConfigurationBL);
				}
				else if (chkDeleteflag.Checked == false && grdFeeTypeConfiguration.DataKeys[iCount][1].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
				{
                    oSchoolwiseFeeTypeConfigurationBL = PopulateFeeTypeConfigurationBL(sFeeType, iFeeTypeID, chkITR.Checked, chkBifurcate.Checked, chkRTE.Checked);
					oSchoolwiseFeeTypeConfigurationBL.ConfigurationAction = Constants.Action.Delete;
					oSchoolwiseFeeTypeConfigurationBL.Fee_Type_Id = grdFeeTypeConfiguration.DataKeys[iCount][0].ToInt();
					oFeeTypeCollection.Add(oSchoolwiseFeeTypeConfigurationBL);
				}
			}
			
			if (oFeeTypeCollection.Count > 0)
			{
				oSchoolwiseFeeTypeConfigurationCollectionBL.moFeesConfigListBL = oFeeTypeCollection;
				oSchoolwiseFeeTypeConfigurationCollectionBL.UpdateAllFeeTypeConfigurationDetails(miAcademicYearId);

				// If the Accounts module is enabled, create/update ledgers for fee types.
				if (Settings.EnableAccountsModule)
					CreateLedgerForFeeTypes();
			}

			if (hidConfigurationFlag.Value != Constants.S_YES)
				ConfigureFeeTypeForGivenSchool();

			var oMasterPage = this.Master as MasterPage;
			oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Fee_Related)));
		}
		catch (ReferenceExceptions ex)
		{
            lblErr.Text = CommonUtility.ModifyExceptionMessage(ex.Message, "Fee Type", Resources.LocalizedResources.FeeType , "can not be removed since associated with", Resources.LocalizedResources.valRemoveText); ;
			FillGridView();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
	
	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// This method is used to decrypt encrypted querystring.
	/// </summary>
	private void ReadQuerystring()
	{
		try
		{
			hidConfigurationFlag.Value = QueryString["Is_Configured"];
		}
		catch (Exception)
		{
			var oMasterPage = this.Master as MasterPage;
			oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
		}
	}

	/// <summary>
	///		Initializes and/or sets default values for certain controls on the page.
	/// </summary>
	private void SetDefaultValues()
	{
		trLedgerNotice.Visible = Settings.EnableAccountsModule;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
		btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Fee_Related));
		ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave });
	}

	/// <summary>
	/// This method is used to populate data from gridview & insert into Weekdays_master table.
	/// </summary>
	/// <param name="asFeeType"></param>
	/// <param name="aiOrgFeeTypeID"></param>
	/// <param name="abConsiderForITReconciliation"></param>
	/// <returns></returns>
	private SchoolwiseFeeTypeConfigurationBL PopulateFeeTypeConfigurationBL(string asFeeType, int aiOrgFeeTypeID, bool abConsiderForITReconciliation, bool abConsiderForBifurcation , bool abConsiderForRTEConcession)
	{
		return new SchoolwiseFeeTypeConfigurationBL
					{
						School_Id					= miSchoolId,
						Academic_Year_Id			= miAcademicYearId,
						Fee_Type					= asFeeType,
						Original_Fee_Type_Id		= aiOrgFeeTypeID,
						Updated_By_Id				= miUserId,
						Inserted_By_id				= miUserId,
						ConsiderForITReconciliation = abConsiderForITReconciliation ? Constants.I_ONE : Constants.I_ZERO,
						ConsiderForBifurcation		= abConsiderForBifurcation,
                        ConsiderForRTEConcession    = abConsiderForRTEConcession ? Constants.I_ONE : Constants.I_ZERO
					};
	}

	/// <summary>
	/// This method is used to check whether Weekdays are configure or not.
	/// </summary>
	private void ConfigureFeeTypeForGivenSchool()
	{
		var oConfiguration = new ConfigurationSchoolMasterBL
								{
									SchoolId		 = miSchoolId,
									OriginalConfigId = Constants.SchoolConfigurations.FeeType.ToInt(),
									AcademicYearId	 = miAcademicYearId
								};
		
		if (!oConfiguration.IsSchoolConfigured())
			SaveConfigDetails(Constants.SchoolConfigurations.FeeType.ToInt());
	}

	/// <summary>
	/// This method is used to retrives the Data in FeeTypeConfiguration.
	/// </summary>
	private void FillGridView()
	{
		var oSchoolwiseFeeTypeConfigurationBL = new SchoolwiseFeeTypeConfigurationBL();
		DataTable oDTFeeTypeConfigDetails = oSchoolwiseFeeTypeConfigurationBL.GetAllFeeTypeConfiguration(miSchoolId, miAcademicYearId);
		grdFeeTypeConfiguration.DataSource = oDTFeeTypeConfigDetails.DefaultView;
		grdFeeTypeConfiguration.DataBind();
		btnSave.Attributes.Add("Onclick", String.Format("if(!(ConfirmAction('{0}','{1}'))){{return false;}}", grdFeeTypeConfiguration.AllowPaging, Resources.LocalizedResources.AlertAtLeastOneFeeType));
	}

	/// <summary>
	///		Creates ledgers in the Accounts module for Fee subtypes.
	/// </summary>
	private void CreateLedgerForFeeTypes()
	{
		AccountLedgerClient oLedgerClient = null;
		try
		{
			oLedgerClient = new AccountLedgerClient();
			oLedgerClient.Open();
			oLedgerClient.CreateLedgersForFeeType(miSchoolId, miAcademicYearId, miFinancialYearId, miUserId);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), "Accounts Module : There was an error creating a ledgers for Fee types.");
		}
		finally
		{
			if (oLedgerClient != null && oLedgerClient.State != CommunicationState.Faulted)
				oLedgerClient.Close();
		}
	}

    private void RefreshValue()
    {
        hidvalFeeTypeName.Value = Resources.LocalizedResources.valFeeTypeName;
        hidvalShouleNotBeDuplicated.Value = Resources.LocalizedResources.valShouleNotBeDuplicated;
        hidFeeTypeName.Value = Resources.LocalizedResources.FeeTypeName;
    }

	#endregion -- PRIVATE METHOD(s) --
}
