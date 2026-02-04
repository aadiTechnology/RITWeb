// File Name   : ScreensUI.aspx.cs
// Created By  : Ashish
// Date        : 05/12/2008
// Description : This class is used to display company admin menu items.

using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Ionic.Zip;
using Utility;

public partial class ScreensUI : SchoolBase
{

	#region -- CONSTANT(s) --

	private const string I_SESSION_YEAR_ID = "I_SESSION_YEAR_ID";

	#endregion -- CONSTANT(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// This event is used to set default properties to page controls.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			//calling base class method
			InitializeMemberVariables();
			if (!IsPostBack)
			{
				SetDashboard();
				HideLink();
				if (Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] != null)
				{
					UpdateSessionVariable();
					FillAcademicYearCombo();
					hlnkDashBoard.Text = Session[Constants.S_SESSION_SCHOOL_NAME].ToString() + " Dashboard";
				}
				txtReason.Text = string.Empty;
				SetQuerystring();
			}
			SetJavaScriptAttribute();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to change academic year.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void cmbAcademicYearID_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR] = cmbAcademicYearID.SelectedValue;
			DataTable oDtYearInfo = GetDataForAcademicYear();
			int iSelectedAcademicYear = cmbAcademicYearID.SelectedValue.ToInt();
			DataRow[] oDataRow = oDtYearInfo.Select("Academic_Year_ID =" + iSelectedAcademicYear);
			Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID] = iSelectedAcademicYear;
			Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE] = oDataRow[0]["Start_date"];
			Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE] = oDataRow[0]["End_Date"];
			Session[I_SESSION_YEAR_ID] = iSelectedAcademicYear;
			hlnkDashBoard.Text = oDataRow[0]["School_Name"].ToString() + " Dashboard";
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to create zip file of all photo galaries.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lnkZipAllPhotoGalleries_Click(object sender, EventArgs e)
	{
		try
		{
			// Table Indices
			int I_TABLE_GALARIES = 0;
			int I_TABLE_IMAGES = 1;
			DataSet oDataSet = ImageGalleryBL.GetAllImages(miSchoolId);
			if (oDataSet != null && oDataSet.Tables.Count > 0)
			{
				string sFileName = string.Empty;
				string sGalleryName = string.Empty;
				string sDestination = string.Empty;
				DataTable oDTGalleries = oDataSet.Tables[I_TABLE_GALARIES];
				DataTable oDTImages = oDataSet.Tables[I_TABLE_IMAGES];
				if (oDTImages != null && oDTImages.Rows.Count > 0 && oDTImages.Rows[0][0] != DBNull.Value)
				{
					for (int iGalleryIndex = 0; iGalleryIndex < oDTGalleries.Rows.Count; iGalleryIndex++)
					{
						sGalleryName = oDTGalleries.Rows[iGalleryIndex][0].ToString();
						sDestination = Server.MapPath("..") + "\\DOWNLOADS\\" + sGalleryName + ".zip";
						if (File.Exists(sDestination))
							File.Delete(sDestination);
						DataRow[] oDRImages = oDTImages.Select("Gallery_Name='" + sGalleryName + "'");
						using (var zip = new ZipFile(sDestination))
						{
							try
							{
								foreach (DataRow oDR in oDRImages)
								{
									sFileName = Server.MapPath("..") + "\\" + oDR[0].ToString();
									zip.AddFile(sFileName, sGalleryName);
								}
								zip.Save();
							}
							catch (Exception ex)
							{
								ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
							}
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This method is used to publish all exams which are previously published.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnPublishAll_Click(object sender, EventArgs e)
	{
		try
		{
			var oSuperAdminBL = new SuperAdminBL();
			string sReason = txtReason.Text.Trim();
			oSuperAdminBL.PublishAllExams(miSchoolId, miAcademicYearId, miUserId, sReason);
			txtReason.Text = string.Empty;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}

	}

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// this function is used to redirect the student ListUI.
	/// </summary>
	private void SetQuerystring()
	{
        string sQuerystring = string.Format("StandardId={0}&DivisionId={0}&NewMode={1}&Is_Configured={0}&Is_SuperAdmin={2}", Constants.I_ZERO, Constants.C_YES, Constants.C_YES);
		string sEncrypt = CommonUtility.EncryptQuerystring(sQuerystring);
		hidQueryString.Value = sEncrypt;
        linkReadmit.Attributes.Add("onclick", "window.open('../Student/LeftStudentsDetailsUI.aspx?" + sEncrypt
                                  + "' , '_self'); return false;");
	}

	/// <summary>
	/// This method is used to set javascript attrribute.
	/// </summary>
	private void SetJavaScriptAttribute()
	{
		hlnkDashBoard.Attributes.Add("onclick", "ShowPopUpWindow('" + hlnkDashBoard.NavigateUrl + "');return false;");
		lnkZipAllPhotoGalleries.Attributes["onclick"] = "if(!ConfirmZip()){return false;}";
		hlnkPublishAll.Attributes.Add("onclick", "ShowPopup() ");
		valsumReturnRenewBook.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
	}

	/// <summary>
	/// This method is used to update session variable and redirect to screenui page.
	/// </summary>
	private void UpdateSessionVariable()
	{
		int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
		string sLoginName = SuperAdminBL.GetSchoolAdminLoginName(iSchoolId);
		var oUserAuthentication = new UserAuthentication(iSchoolId, sLoginName, string.Empty, String.Empty);
		oUserAuthentication.UpdateSession();
		InitializeMemberVariables();
	}

	/// <summary>
	/// This method is used to provide data about all academic years of school.
	/// </summary>
	/// <returns></returns>
	private DataTable GetDataForAcademicYear()
	{
		var oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
		DataTable oDtYearInfo = oSchoolWiseAcademicYearMasterBL.GetAllAcademicYearsForSchool(miSchoolId, miUserId, moUserRole.ToInt());
		return oDtYearInfo;
	}

	/// <summary>
	/// This method is used to fill academic year combo on page load.
	/// </summary>
	private void FillAcademicYearCombo()
	{
		DataTable oDtYearInfo = GetDataForAcademicYear();
		cmbAcademicYearID.Bind(oDtYearInfo, "Academic_Year_ID", "YearValue", String.Empty);

		if (Session[I_SESSION_YEAR_ID] != null && Session[I_SESSION_YEAR_ID] != Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID])
		{
			Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR] = Session[I_SESSION_YEAR_ID];
			DataTable oDtYear = GetDataForAcademicYear();
			int iSelectedAcademicYear = Session[I_SESSION_YEAR_ID].ToInt();
			DataRow[] oDataRow = oDtYearInfo.Select("Academic_Year_ID =" + iSelectedAcademicYear);
			Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID] = iSelectedAcademicYear;
			Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE] = oDataRow[0]["Start_date"];
			Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE] = oDataRow[0]["End_Date"];
			// We need to initialize base calss member variables since session has been updated.
			InitializeMemberVariables();
		}
		else
			Session[I_SESSION_YEAR_ID] = null;

		cmbAcademicYearID.SelectedValue = miAcademicYearId.ToString();
	}

	/// <summary>
	/// This method is used to hide dachboard link.
	/// </summary>
	public void HideLink()
	{
		var oSuperAdminMasterPage = this.Master as SuperAdminMasterPage;
		oSuperAdminMasterPage.HideLink();
	}

	/// <summary>
	/// This method is used to set dashboard view.
	/// </summary>
	private void SetDashboard()
	{
		if (Session[Constants.S_SESSION_SUPERADMIN_ROLE_ID] != null && (Constants.SuperAdminRoles)Session[Constants.S_SESSION_SUPERADMIN_ROLE_ID] == Constants.SuperAdminRoles.ManagementUser)
		{
			tblAdmin.Visible = false;
			tblManagement.Visible = true;
		}
		else
		{
			tblAdmin.Visible = true;
			tblManagement.Visible = false;
		}
	}

	#endregion -- PRIVATE METHOD(s) --

}
