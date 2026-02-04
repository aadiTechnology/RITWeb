/* ----------------------------------------------------------------------------
 *	FileName	: DashboardUI.aspx
 *	Author		: Vishal B. Shah
 *	Date		: 4-Dec-2012
 *	Description	: This is the dashboard / control panel page for users from the
 *				  Management user role group.
 * ----------------------------------------------------------------------------
 */

using System;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using Management.Entities;
using Utility;

namespace Management
{
	public partial class DashboardUI : SchoolBase
	{
		#region -- PROPERTIES --

		/// <summary>
		///		Exposes the ManagementClientUtil object, which is used to communicated with the service.
		/// </summary>
        //private ManagementClientUtil ManagementServiceClient
        //{
        //    get
        //    {
        //        var mgmtServiceClient = Session[Constants.S_SESSION_MANAGEMENT_CLIENT] as ManagementClientUtil;
        //        if (mgmtServiceClient == null)
        //        {
        //            mgmtServiceClient = new ManagementClientUtil();
        //            Session[Constants.S_SESSION_MANAGEMENT_CLIENT] = mgmtServiceClient;
        //        }
        //        return mgmtServiceClient;
        //    }
        //}
		
		#endregion -- PROPERTIES --

		#region -- EVENT HANDLER(s) --

		/// <summary>
		///		Handles the page load event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{				
				if (!IsPostBack)
				{
					base.SetDocType();
					InitializeServiceClient();
					FillCombos();
					SerializeData();
				}
				RemoveOrphanTd();
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
			}
		}

		/// <summary>
		///		Hides certain controls on the page.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_PreRender(object sender, EventArgs e)
		{
			try
			{
				HideMasterControls();
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
			}
		}

		/// <summary>
		///		Updates the data according to the newly selected academicyear.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddlAcademicYear_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				SetCurrentYear();
				//ManagementServiceClient.InitializeWidgets();
				SerializeData();
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
			}
		}

		/// <summary>
		///		Updates the data as per the newly selected financial year.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddlFinancialYear_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				SetCurrentYear();
				//ManagementServiceClient.InitializeWidgets();
				SerializeData();
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
			}
		}

		/// <summary>
		///		Updates the student attendance details as per the selected date.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void studentAttendanceDate_TextChanged(object sender, EventArgs e)
		{
			try
			{
				SetStudentAttendanceDate();
				//ManagementServiceClient.GetStudentDetails();
				SerializeData();
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
			}
		}

		/// <summary>
		///		Updates the fee collection details as per configuration.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void chkFeeCollection_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
                //ManagementServiceClient.IncludeInternalFees = chkIncludeInternalFees.Checked;
                //ManagementServiceClient.IncludeCautionMoney = chkIncludeCautionMoney.Checked;
                //ManagementServiceClient.GetFeeCollectionDetails();
				SerializeData();
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
			}
		}
		
		/// <summary>
		///		Opens the MISReport for the respective school, in a new window.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnViewMISReport_Click(object sender, EventArgs e)
		{
			try
			{
				if (hidMISReportCurrentSchoolId.Value.IsNullOrEmpty())
					return;

				int iSchoolId = hidMISReportCurrentSchoolId.Value.ToInt();
				string sScriptBlock = String.Empty;

				if (iSchoolId == 0)
				{
					//Session[Constants.S_SESSION_MIS_FINANCIAL_YEAR] = ManagementServiceClient.GetFinancialYear(miSchoolId);
					sScriptBlock = String.Format("window.open('../Accounts/MISReportUI.aspx?{0}', '_blank');",
												 CommonUtility.EncryptQuerystring("IsFromMgmtDashboard=Y"));
				}
				else
				{
					
					//Session[Constants.S_SESSION_MANAGEMENT_MISREPORT] = ManagementServiceClient.GetMISReport(iSchoolId);
					//Session[Constants.S_SESSION_MIS_FINANCIAL_YEAR] = ManagementServiceClient.GetFinancialYear(iSchoolId);
					sScriptBlock = String.Format("window.open('../Accounts/MISReportUI.aspx?{0}', '_blank', 'location=0,menubar=0,status=0,titlebar=0,toolbar=0,scrollbars=1,resizable=1,top=0,left=0,width=1200,height=700');",
												 CommonUtility.EncryptQuerystring("IsPopup=Y&IsFromMgmtDashboard=Y"));
				}

				ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "MISReport", sScriptBlock, true);
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
			}
		}

		#endregion -- EVENT HANDLER(s) --

		#region -- PRIVATE METHOD(s) --
	
		/// <summary>
		///		Sets the DOCTYPE for the page.
		/// </summary>
        //private void SetDocType()
        //{
        //    var literal = Page.Master.FindControl("docType") as Literal;
        //    literal.Text = "<!DOCTYPE HTML>" + Environment.NewLine;
        //}

		/// <summary>
		///		Initializes the service client util object.
		/// </summary>
		private void InitializeServiceClient()
		{
			// Student attendance properties
            //ManagementServiceClient.StudentAttendanceDate = DateTime.Now.Date;
            //studentAttendanceDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
			
            //// Fee collection properties
            //ManagementServiceClient.IncludeInternalFees = chkIncludeInternalFees.Checked;
            //ManagementServiceClient.IncludeCautionMoney = chkIncludeCautionMoney.Checked;

            //ManagementServiceClient.Initialize();
		}

		/// <summary>
		///		Fills the academic year and financial year combo boxes.
		/// </summary>
		private void FillCombos()
		{
            //ddlAcademicYear.Bind(ManagementServiceClient.AcademicYears , "Id", "DisplayValue");
            //AcademicYear oCurrentAcademicYear = ManagementServiceClient.AcademicYears.First(ay => ay.IsCurrent);
            //if (oCurrentAcademicYear != null)
            //    ddlAcademicYear.SelectedValue = oCurrentAcademicYear.Id.ToString();

            //if (ManagementServiceClient.FinancialYears == null || ManagementServiceClient.FinancialYears.Count <= 0)
			{
				lblFinancialYear.Visible = false;
				ddlFinancialYear.Visible = false;
				misreportWidget.Visible = false;
				return;
			}
			
            //ddlFinancialYear.Bind(ManagementServiceClient.FinancialYears, "Id", "DisplayValue");
            //FinancialYear oCurrentFinancialYear = ManagementServiceClient.FinancialYears.First(ay => ay.IsCurrent);
            //if (oCurrentFinancialYear != null)
            //    ddlFinancialYear.SelectedValue = oCurrentFinancialYear.Id.ToString();
		}

		/// <summary>
		///		Hides certain controls on the master page.
		/// </summary>
		private void HideMasterControls()
		{
			var masterPage = this.Master as MasterPage;
			
			var hlnkEmail = masterPage.FindControl("hlnkEmail") as HyperLink;
			if (hlnkEmail != null)
				hlnkEmail.Visible = false;

			var hlnkSupport = masterPage.FindControl("hlnkSupport") as HyperLink;
			if (hlnkSupport != null)
				hlnkSupport.Visible = false;

			var lnkFeedback = masterPage.FindControl("lnkFeedback") as LinkButton;
			if (lnkFeedback != null)
				lnkFeedback.Visible = false;
		}

		/// <summary>
		///		Serializes the data in JSON format to a hidden field.
		/// </summary>
		private void SerializeData()
		{
			//hidJSON.Value = ManagementServiceClient.GetSerializedData();
		}

		/// <summary>
		///		Sets the current year in Service client object as per the current chosen year on the page.
		/// </summary>
		private void SetCurrentYear()
		{
			//ManagementServiceClient.CurrentAcademicYearId = ddlAcademicYear.SelectedValue.ToInt();
			//ManagementServiceClient.CurrentFinancialYearId = ddlFinancialYear.SelectedValue.ToInt();
		}

		/// <summary>
		///		Sets the student attendance date in the service client object.
		/// </summary>
		private void SetStudentAttendanceDate()
		{
			//ManagementServiceClient.StudentAttendanceDate = studentAttendanceDate.Text.ToDateTime();
		}

		/// <summary>
		///		Removes unwanted dummy td objects in the grid header row.
		/// </summary>
		private void RemoveOrphanTd()
		{
			trHeaderRow.Cells.Clear();
		}

		#endregion -- PRIVATE METHOD(s) --
	}
}