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
///		This class provides user interface to configure and save Fee subtypes.
/// </summary>
public partial class FeeSubTypeConfiguration : SchoolBase
{
	#region -- CONSTANT(s) --

	private const string S_ERR_MSG_SELECT_FEE_TYPE = "At least one fee sub type name should be selected for saving.";
	private const int I_CHECKBOX_COLUMN_NUMBER = 0;
	private const int I_KEY_SCHOOL_ID = 1;
	private const int I_KEY_FEE_SUBTYPE = 3;    
	private const string S_CHECKBOX_FEETYPE = "ChkSelect";
	private const string S_TXT_FEETYPE = "txtFeeSubType";	

	#endregion -- CONSTANT(s) --	

	#region -- EVENT HANDLER(s) --

	/// <summary>
	///		Handles the page load event of the page.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			if (!IsPostBack)
			{
				ReadQuerystring();
				FillGridView();
				Initialize();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();

                }
			}
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                Initialize();
                btnSave.Attributes.Add("Onclick", string.Format("if(!(ConfirmAction('{0}','{1}'))){{return false;}}", grdFeeSubTypeConfiguration.AllowPaging, Resources.LocalizedResources.AlertAtLeastOneFeeSubType));
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
	protected void grdFeeSubTypeConfiguration_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			if (e.Row.RowIndex >= 0)
			{
				var chkIsSelected = e.Row.Cells[I_CHECKBOX_COLUMN_NUMBER].FindControl(S_CHECKBOX_FEETYPE) as CheckBox;
                chkIsSelected.Attributes.Add("onclick", "ChkOnChange(" + e.Row.RowIndex + ")");
				if (hidConfigurationFlag.Value == Constants.S_YES)
				{
					if (grdFeeSubTypeConfiguration.DataKeys[e.Row.RowIndex][I_KEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
						chkIsSelected.Checked = true;
				}
				else
					chkIsSelected.Checked = false;
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
			CheckBox oCheckBoxDeleteflag;
			var oSchoolwiseFeeSubTypeConfigurationBL = new SchoolwiseFeeSubTypeConfigurationBL();
			var oSchoolwiseFeeSubTypeConfigurationCollectionBL = new SchoolwiseFeeSubTypeConfigurationCollectionBL();
			var oFeeTypeCollection = new Collection<SchoolwiseFeeSubTypeConfigurationBL>();		

			int iIndex = Constants.I_ZERO;

			for (; iIndex < grdFeeSubTypeConfiguration.Rows.Count; iIndex++)
			{
				oCheckBoxDeleteflag = grdFeeSubTypeConfiguration.Rows[iIndex].FindControl(S_CHECKBOX_FEETYPE) as CheckBox;                
				int iOrgFeeSubTypeId = grdFeeSubTypeConfiguration.DataKeys[iIndex][Constants.I_TWO].ToInt();
				string sFeeSubType = (grdFeeSubTypeConfiguration.Rows[iIndex].FindControl(S_TXT_FEETYPE) as TextBox).Text;
				int iFeeSubTypeId = grdFeeSubTypeConfiguration.DataKeys[iIndex][Constants.I_ZERO].ToInt();
                
				if (oCheckBoxDeleteflag.Checked && grdFeeSubTypeConfiguration.DataKeys[iIndex][Constants.I_ONE].ToString() == Constants.S_DEFAUL_SCHOOL_ID)
				{
					oSchoolwiseFeeSubTypeConfigurationBL = PopulateFeeTypeConfigurationBL(sFeeSubType, iOrgFeeSubTypeId);
					oSchoolwiseFeeSubTypeConfigurationBL.ConfigurationAction = Constants.Action.Insert;
					oFeeTypeCollection.Add(oSchoolwiseFeeSubTypeConfigurationBL);
				}

				// Check if existing standard name is being updated.
				// I.e. If the checkbox is checked and the school is not -9999 and the value in text box differs 
				// from the value in the standard name column then update the existing standard name.
				else if (oCheckBoxDeleteflag.Checked
							&& grdFeeSubTypeConfiguration.DataKeys[iIndex][Constants.I_ONE].ToString() != Constants.S_DEFAUL_SCHOOL_ID
							&& grdFeeSubTypeConfiguration.Rows[iIndex].Cells[Constants.I_ONE].Text != sFeeSubType)
				{

					oSchoolwiseFeeSubTypeConfigurationBL = PopulateFeeTypeConfigurationBL(sFeeSubType, iOrgFeeSubTypeId);
					oSchoolwiseFeeSubTypeConfigurationBL.ConfigurationAction = Constants.Action.Update;
					oSchoolwiseFeeSubTypeConfigurationBL.Fee_SubType_Id = grdFeeSubTypeConfiguration.DataKeys[iIndex][Constants.I_ZERO].ToInt();
					oFeeTypeCollection.Add(oSchoolwiseFeeSubTypeConfigurationBL);
				}
				else if (oCheckBoxDeleteflag.Checked == false && grdFeeSubTypeConfiguration.DataKeys[iIndex][Constants.I_ONE].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
				{
					oSchoolwiseFeeSubTypeConfigurationBL = PopulateFeeTypeConfigurationBL(sFeeSubType, iFeeSubTypeId);
					oSchoolwiseFeeSubTypeConfigurationBL.ConfigurationAction = Constants.Action.Delete;
					oSchoolwiseFeeSubTypeConfigurationBL.Fee_SubType_Id = grdFeeSubTypeConfiguration.DataKeys[iIndex][Constants.I_ZERO].ToInt();
					oFeeTypeCollection.Add(oSchoolwiseFeeSubTypeConfigurationBL);
				}
			}

			if (oFeeTypeCollection.Count > Constants.I_ZERO && iIndex >= grdFeeSubTypeConfiguration.Rows.Count)
			{
				oSchoolwiseFeeSubTypeConfigurationCollectionBL.moFeeSubTypeConfigListBL = oFeeTypeCollection;
				oSchoolwiseFeeSubTypeConfigurationCollectionBL.UpdateAllFeeTypeConfigurationDetails(miAcademicYearId);
			}

			if (hidConfigurationFlag.Value != Constants.S_YES)
				ConfigureFeeTypeForGivenSchool();

			if (lblErr.Text.IsNullOrEmpty())
			{
				var oMasterPage = Master as MasterPage;
				oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Fee_Related)));
			}
		}
		catch (ReferenceExceptions ex)
		{
            lblErr.Text = CommonUtility.ModifyExceptionMessage(ex.Message, "Fee Sub type", Resources.LocalizedResources.FeeSubType , "can not be removed since associated with", Resources.LocalizedResources.valRemoveText); ;
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
		if (!Request.QueryString.ToString().IsNullOrEmpty())
			hidConfigurationFlag.Value = QueryString["Is_Configured"];
	}

	/// <summary>
	/// This method is used to initialize controls on the page.
	/// </summary>
	private void Initialize()
	{
		valSumErrorMsg.HeaderText = Resources .LocalizedResources.PleaseFixFollowingError;
		
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel });
		
		btnSave.Attributes.Add("Onclick", string.Format("if(!(ConfirmAction('{0}','{1}'))){{return false;}}", grdFeeSubTypeConfiguration.AllowPaging, Resources.LocalizedResources.AlertAtLeastOneFeeSubType));
		btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Fee_Related));
	}

	/// <summary>
	/// This method is used to populate data from gridview & insert into Weekdays_master table.
	/// </summary>
	/// <param name="asFeeSubType"></param>
	/// <param name="aiOrgFeeTypeID"></param>
	/// <returns></returns>
	private SchoolwiseFeeSubTypeConfigurationBL PopulateFeeTypeConfigurationBL(string asFeeSubType, int aiOrgFeeTypeID)
	{
		return new SchoolwiseFeeSubTypeConfigurationBL
					{
						School_Id = miSchoolId,
						Academic_Year_Id = miAcademicYearId,
						Fee_SubType = asFeeSubType,
						Original_Fee_SubType_Id = aiOrgFeeTypeID,
						Updated_By_Id = miUserId,
						Inserted_By_id = miUserId                
					};
	}

	/// <summary>
	/// This method is used to check whether Weekdays are configure or not.
	/// </summary>
	private void ConfigureFeeTypeForGivenSchool()
	{
		var oConfiguration = new ConfigurationSchoolMasterBL
								{
									SchoolId = miSchoolId,
									OriginalConfigId = Constants.SchoolConfigurations.FeeSubType.ToInt(),
									AcademicYearId = miAcademicYearId
								};
        if (!oConfiguration.IsSchoolConfigured())
            SaveConfigDetails(Constants.SchoolConfigurations.FeeSubType.ToInt());
	}

	/// <summary>
	/// This method is used to retrives the Data in Weekdaysgridview.
	/// </summary>
	private void FillGridView()
	{
		var oSchoolwiseFeeSubTypeConfigurationBL = new SchoolwiseFeeSubTypeConfigurationBL();
		DataTable oDTFeeSubTypeConfigDetails = oSchoolwiseFeeSubTypeConfigurationBL.GetAllFeeSubTypeConfiguration(miSchoolId, miAcademicYearId);
		
		grdFeeSubTypeConfiguration.DataSource = oDTFeeSubTypeConfigDetails.DefaultView;
		grdFeeSubTypeConfiguration.DataBind();

		for (int iCount = 0; iCount < grdFeeSubTypeConfiguration.Rows.Count; iCount++)
		{
			var oTextBox = grdFeeSubTypeConfiguration.Rows[iCount].FindControl(S_TXT_FEETYPE) as TextBox;
			oTextBox.EnableViewState = true;
			oTextBox.Text = grdFeeSubTypeConfiguration.DataKeys[iCount][I_KEY_FEE_SUBTYPE].ToString();
			var chkSelect = grdFeeSubTypeConfiguration.Rows[iCount].FindControl(S_CHECKBOX_FEETYPE) as CheckBox;

			if (chkSelect.IsNull() || !chkSelect.Checked)
			{
                oTextBox.Enabled = false;
				continue;
			}
		}
	}

	#endregion -- PRIVATE METHOD(s) --
}