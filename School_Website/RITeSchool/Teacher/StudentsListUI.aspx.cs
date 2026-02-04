/* File Name :- StudentsListUI.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 2-Oct-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used for following purpose :-
 *   1)To display list of students.
 *   2)To allows user to edit or add student information.
 *   3)To allows user to export students information.
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using StudentEntities;
using Utility;
using SchoolAutoSearchService.Service;
using Resources;
using System.Resources;
using System.Globalization;
using SchoolEntities;


public partial class StudentsListUI : SchoolBase
{
    #region constants

    const int I_COL_INDEX_ROLL_NO = 1;
    const int I_COLUMN_INDEX_CLASS = 2;
    const int I_COLUMN_INDEX_DOB = 4;
    const int I_COLUMN_INDEX_EDIT = 5;
    const int I_COLUMN_INDEX_PHOTO = 6;
    const int I_COLUMN_INDEX_DELETE = 7;
    const int I_COLUMN_INDEX_LC_ADD = 8;
    const int I_DATAKEY_ID_INDEX = 2;
    const int I_DATAKEY_STANDARDID_INDEX = 4;
    const int I_DATAKEY_JOININGDATE_INDEX = 6;
    const int I_DATAKEY_DIVISIONID_INDEX = 5;   
    const int I_DATAKEY_ADMISSIONDATE_INDEX = 12;
    const string S_BLANK_GRID_MESSAGE = "No student available.";
    const string S_EXPORT_STUDENT = "usp_GetStudents_Export";
    const string S_ROWCMD_DELETE_STUDENT = "DELETE_STUDENT";
    static string msStandardName;
    static string msDivisionName;    
    #endregion

    #region Data members

    Boolean bIsUserHasfullAccess = false;
    Boolean mbIsClassSelected = true;
    #endregion

    #region Events

    /// <summary>
    /// This event is used to select masterpage.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
			base.OnPreInit(e);
            if (moUserRole != Constants.UserRoles.Admin)
                bIsUserHasfullAccess = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.Student);
	        if (moUserRole == Constants.UserRoles.Admin || bIsUserHasfullAccess)
		        this.Page.MasterPageFile = "../MasterPages/PopupMaster.master";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to initialize controls and fill grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultValues();
            if (!IsPostBack)
            {                             
                FillOperators();                
                GetPrefixes();
                GetAllRegNoPostfixes();
                InitialiseDateValues();
                GetStandardDivisionList();
                SetJavaScriptAttributes();
                SetDefaultButton(btnSearch);
                GetQuerystring();
                SetSortProperties();
                CheckRoleAndSetGridview();
                SetClassComboValue();
                SetViewForFullAccessUser();
                DisplayViewAccordingToUser();
                FillStudentGrid();
                CheckMidYear();
                txtDeletedDate.Text = caltxtDeletedDate.DateValue.ToString("dd-MMM-yyyy");
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                   // caltxtDeletedDate.DateValue = Convert.ToDateTime(DateCultureConversion(DateTime.Today.ToString(), hidCultureInfo.Value, Session[Constants.S_SESSION_LANGUAGE].ToString()).ToString("dd MMM yyyy")));
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
               // caltxtDeletedDate.DateValue = Convert.ToDateTime(DateCultureConversion(DateTime.Today.ToString(), "en", Session[Constants.S_SESSION_LANGUAGE].ToString()));
                RefreshValue();

                if (hidIsSuperAdmin.Value.Equals("Y"))
                {
                    hidIsAdmin.Value = true.ToString();
                    chkcompleteDelete.Visible = true;
                }               
            }
            //if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE])
            //{
            //    string m = DateCultureConversion(DateTime.Today.ToString(), hidCultureInfo.Value, Session[Constants.S_SESSION_LANGUAGE].ToString());
            //   // caltxtDeletedDate.DateValue = 
            //    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
            //    InitialiseDateValues();
            //    RefreshValue();
            //    SetSortProperties();
            //}

            SetIdentitylinkURL();
            SetPhotoURL();
            if (moUserRole == Constants.UserRoles.Teacher)
                btnAdd.Visible = false;
            else if ((hidStandardId.Value != "0" && hidDivisionId.Value!="0") && (moUserRole == Constants.UserRoles.Admin || hidCanEdit.Value == Constants.S_YES))
                btnAdd.Visible = true;
            else
                btnAdd.Visible = false;
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to fill division combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStd_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            mbIsClassSelected = false;
            hidStudentReg.Value = txtReg.Text.Trim();
            hidStudentName.Value = txtName.Text.Trim();
            grdStudents.PageIndex = 0;
            hidDivisionId.Value = "0";
            hidStandardId.Value = cmbStandard.SelectedValue;
            msStandardName = cmbStandard.SelectedItem.ToString();
            if (Convert.ToInt32(cmbStandard.SelectedValue) == Constants.I_ZERO)
            {
                hidSortExpression.Value = "StandardDivision";
                grdStudents.Sort("StandardDivision", SortDirection.Ascending);
                HideAddBtn(false);            
              
            }
            else
            SetSortProperties();        
            btnAdd.Visible = false;
            tdTotalRec.Visible = true;
            FillDivisionCombobox();
            SetIdentitylinkURL();
            SetPhotoURL();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display Add button and set url to identity link.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            mbIsClassSelected = false;
            if (cmbStandard.SelectedIndex != 0 && cmbDivision.SelectedIndex != 0)
            {
                int iStd = cmbStandard.SelectedValue.ToInt();
                int iDiv = cmbDivision.SelectedValue.ToInt();

                List<StandardDivisionMaster> olstStandardDivisionMaster = Session["StandardDivisionList"] as List<StandardDivisionMaster>;
                StandardDivisionMaster moStandardDivisionMaster =
                        olstStandardDivisionMaster.Where(StdDiv => StdDiv.StandardId == Convert.ToInt32(iStd) && StdDiv.DivisionId == iDiv).Select(Std => new StandardDivisionMaster { StandardDivisionId = Std.StandardDivisionId }).FirstOrDefault();
                cmbClass.SelectedValue = moStandardDivisionMaster.StandardDivisionId.ToString();
                cmbClass_SelectedIndexChanged(cmbDivision, null);
            }
            else if (cmbStandard.SelectedIndex == 0 || cmbDivision.SelectedIndex == 0)
                cmbClass.SelectedValue = Constants.S_ZERO;

            hidStudentReg.Value = txtReg.Text.Trim();
            hidStudentName.Value = txtName.Text.Trim();
            grdStudents.PageIndex = 0;
            hidDivisionId.Value = cmbDivision.SelectedValue;
            msDivisionName = cmbDivision.SelectedItem.ToString();
            if (moUserRole == Constants.UserRoles.Admin ||
                Boolean.Parse(hidUserHasFullAccess.Value) && Convert.ToChar(hidCanEdit.Value) == Constants.C_YES)
            {
                HideAddBtn(cmbDivision.SelectedIndex != 0);
            }
            SetIdentitylinkURL();
            SetPhotoURL();
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session.Add("ClassId", cmbClass.SelectedValue);
            
            if (Session["StandardDivisionList"] != null && cmbClass.SelectedValue != Constants.I_ZERO.ToString())
            {
                List<StandardDivisionMaster> olstStandardDivisionMaster = Session["StandardDivisionList"] as List<StandardDivisionMaster>;
                StandardDivisionMaster moStandardDivisionMaster =
                        olstStandardDivisionMaster.Where(StdDiv => StdDiv.StandardDivisionId == Convert.ToInt32(cmbClass.SelectedValue)).Select(Std => new StandardDivisionMaster { StandardId = Std.StandardId, DivisionId = Std.DivisionId }).FirstOrDefault();
				hidStandardDivisionId.Value = cmbClass.SelectedValue;
                cmbStandard.SelectedValue = hidStandardId.Value = moStandardDivisionMaster.StandardId.ToString();
                cmbStd_SelectedIndexChanged(cmbStandard, null);
                cmbDivision.SelectedValue = hidDivisionId.Value = moStandardDivisionMaster.DivisionId.ToString();
            }
            else
            {
                if (cmbClass.SelectedValue == Constants.I_ZERO.ToString() && mbIsClassSelected)
                {
                    cmbStandard.SelectedValue = hidStandardId.Value = Constants.S_ZERO;
                    cmbStd_SelectedIndexChanged(cmbStandard, null);
                    cmbDivision.SelectedValue = hidDivisionId.Value =Constants.S_ZERO;
                }
                else
                {
                    hidStandardId.Value = cmbStandard.SelectedValue;
                    hidDivisionId.Value = cmbDivision.SelectedValue;
                }
            }
            msStandardName = cmbStandard.SelectedItem.ToString();
            msDivisionName = cmbDivision.SelectedItem.ToString();
            hidStudentReg.Value = txtReg.Text.Trim();
            hidStudentName.Value = txtName.Text.Trim();
            grdStudents.PageIndex = 0;
            hidStandardDivisionId.Value = cmbClass.SelectedValue;
            if (Convert.ToInt32(cmbClass.SelectedValue) == Constants.I_ZERO)
            {
                hidSortExpression.Value = "StandardDivision";
                grdStudents.Sort("StandardDivision", System.Web.UI.WebControls.SortDirection.Ascending);
            }
            else
            {
                SetSortProperties();

                if(moUserRole == Constants.UserRoles.Admin || hidCanEdit.Value == Constants.S_YES)
                    btnAdd.Visible = true;
            }
            tdTotalRec.Visible = true;            
            SetIdentitylinkURL();
            SetPhotoURL();
            mbIsClassSelected = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

 
    /// <summary>
    /// This event is used to delete student information.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            bool bIsCompleteDelete = chkcompleteDelete.Checked;
            int iStudentId = Convert.ToInt32(hidStudentId.Value);
            DeleteStudentDetails(iStudentId, false);
            if (grdStudents.Rows.Count == 0)
                grdStudents.SetPageIndex(0);
            RefreshStudentCache(bIsCompleteDelete);
        }
        catch (ReferenceExceptions ex)
        {
            lblErrorMsg.Text = ex.Message.Replace("You cannot delete this student since there is data for this student is avilable for", Resources.LocalizedResources.ExceptionDeleteStudent);
            grdStudents.DataBind();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to redirect towards student photos ui.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            SetQueryString(true);
            Session.Add("ClassId", cmbClass.SelectedValue);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This method is used to redirect towards Update Reg. No UI.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdateRegNo_Click(object sender, EventArgs e)
    {
        try
        {
            SetQueryString(false);
            Session.Add("ClassId", cmbClass.SelectedValue);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to open student details popup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder sQueryString = new StringBuilder();
            if (moUserRole == Constants.UserRoles.Admin ||
                Boolean.Parse(hidUserHasFullAccess.Value) && Convert.ToChar(hidCanEdit.Value) == Constants.C_YES)
            {
            sQueryString.Append("NewMode=" + Constants.C_YES);
            sQueryString.Append("&StandardId=" + cmbStandard.SelectedValue);
            sQueryString.Append("&DivisionId=" + cmbDivision.SelectedValue);
            sQueryString.Append("&standardName=" + msStandardName);
            sQueryString.Append("&DivisionName=" + msDivisionName);
            sQueryString.Append("&Is_Configured=" + hidIsConfig.Value);
            sQueryString.Append("&ClassId=" + cmbClass.SelectedItem);
            string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString.ToString());
            string sUrl = Constants.S_PAGE_STUDENT_BASIC_DETAILS + "?" + sEncrypt;
            Response.Redirect(sUrl, false);
            Session.Add("ClassId", cmbClass.SelectedValue);
            }
            else if (moUserRole == Constants.UserRoles.Teacher)
           {
               sQueryString.Append("NewMode=" + Constants.C_YES);
               sQueryString.Append("&StandardId=" + hidStandardId.Value);
               sQueryString.Append("&DivisionId=" + hidDivisionId.Value);
               sQueryString.Append("&standardName=" + msStandardName);
               sQueryString.Append("&DivisionName=" + msDivisionName);
               sQueryString.Append("&Is_Configured=" + hidIsConfig.Value);
               sQueryString.Append("&ClassId=" + hidStandardDivisionId.Value);
               string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString.ToString());
               string sUrl = Constants.S_PAGE_STUDENT_BASIC_DETAILS + "?"+ sEncrypt;
               Response.Redirect(sUrl, false);
               Session.Add("ClassId", cmbClass.SelectedValue);
           }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to search student information according to reg. no or name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            SetPrefixCombo();
            hidStudentReg.Value = txtReg.Text.Trim();
            hidStudentName.Value = txtName.Text.Trim();
            grdStudents.PageIndex = 0;
            grdStudents.DataBind();
            if (optExact.Checked)
            {
                cmbOperation.Enabled = true;
                cmbPrefix.Enabled = true;
                txtReg.Enabled = true;
                txtName.Enabled = false;
                txtReg.Focus();
            }
            else if (optMain.Checked)
            {
                txtName.Enabled = true;
                txtName.Focus();
                cmbOperation.Enabled = false;
                cmbPrefix.Enabled = false;
                txtReg.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to export students details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentDetails, getFilterString());
            oReportDisplay.DisplayReport();
            if (optExact.Checked)
            {
                cmbOperation.Enabled = true;
                cmbPrefix.Enabled = true;
                txtReg.Enabled = true;
                txtName.Enabled = false;
                txtReg.Focus();
            }
            else if (optMain.Checked)
            {
                txtName.Enabled = true;
                txtName.Focus();
                cmbOperation.Enabled = false;
                cmbPrefix.Enabled = false;
                txtReg.Enabled = false;
            }
        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void optMain_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (optMain.Checked)
                SetControlsUponCriteria(true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void optExact_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (optExact.Checked)
                SetControlsUponCriteria(false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to enable date controlls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkSchoolLeaving_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            DisableDate(true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to disable date controlls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkcompleteDelete_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            DisableDate(false);
            txtCancFormNo.Text = string.Empty;            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to remove classid field from session.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["ClassId"] != null)
                Session.Remove("ClassId");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	protected void ddlClassTeacher_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			hidStandardId.Value = Constants.S_ZERO;
			hidDivisionId.Value = Constants.S_ZERO;
			hidStandardDivisionId.Value = ddlClassTeacher.SelectedValue;
			if (Session["StandardDivisionList"] != null && ddlClassTeacher.SelectedValue != Constants.I_ZERO.ToString())
			{
				List<StandardDivisionMaster> olstStandardDivisionMaster = Session["StandardDivisionList"] as List<StandardDivisionMaster>;
				StandardDivisionMaster moStandardDivisionMaster =
			    olstStandardDivisionMaster.Where(StdDiv => StdDiv.StandardDivisionId == Convert.ToInt32(ddlClassTeacher.SelectedValue)).Select(Std => new StandardDivisionMaster { StandardId = Std.StandardId, DivisionId = Std.DivisionId }).FirstOrDefault();
				hidStandardDivisionId.Value = ddlClassTeacher.SelectedValue;
				cmbStandard.SelectedValue = hidStandardId.Value = moStandardDivisionMaster.StandardId.ToString();
			    cmbDivision.SelectedValue = hidDivisionId.Value = moStandardDivisionMaster.DivisionId.ToString();
			}
			else
			{
				if (cmbClass.SelectedValue == Constants.I_ZERO.ToString() && mbIsClassSelected)
				{
					cmbStandard.SelectedValue = hidStandardId.Value = Constants.S_ZERO;
					cmbStd_SelectedIndexChanged(cmbStandard, null);
					cmbDivision.SelectedValue = hidDivisionId.Value = Constants.S_ZERO;
				}
				else
				{
					hidStandardId.Value = cmbStandard.SelectedValue;
					hidDivisionId.Value = cmbDivision.SelectedValue;
				}
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
    #region grid events

    /// <summary>
    /// This event is used to set sort expression and direction.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
            FillStudentGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to map gridview pageindex with combobox index.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            GridViewRow pagerRow = grdStudents.BottomPagerRow;
            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
            // Set the PageIndex property to display that page selected by the user.
            grdStudents.PageIndex = pageList.SelectedIndex;
            grdStudents.DataSourceID = GrdDSobj.ID;

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set properties to gridview columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_RowDatabound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink oHyperLink = null;
                HyperLink olnkAdditinalDetails = null;
                string sUrl = string.Empty;
                string sQueryString = string.Empty;
                if (grdStudents.DataKeys[e.Row.RowIndex]["Is_Locked"].ToString() == "Y")
                    e.Row.BackColor = System.Drawing.Color.Gainsboro;
                string IsAttendanceAvailable = grdStudents.DataKeys[e.Row.RowIndex]["IsAttendanceAvailable"].ToString();                
                if ((moUserRole == Constants.UserRoles.Admin
                || Convert.ToChar(hidCanEdit.Value) == Constants.C_YES) || (
                moUserRole == Constants.UserRoles.Teacher &&
                Convert.ToChar(Session[Constants.S_SESSION_IS_CLASS_TEACHER]) == Constants.C_YES) && !Boolean.Parse(hidUserHasFullAccess.Value))
                {
                    oHyperLink = (HyperLink)(e.Row.Cells[I_COLUMN_INDEX_EDIT].Controls[0]);
                    if ((moUserRole == Constants.UserRoles.Admin)||((moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)&&(Boolean.Parse(hidUserHasFullAccess.Value))))
                        olnkAdditinalDetails = (HyperLink)(e.Row.Cells[I_COLUMN_INDEX_LC_ADD].Controls[0]);
                    sUrl = oHyperLink.NavigateUrl;
                    int iStandardId = Convert.ToInt32(grdStudents.DataKeys[e.Row.RowIndex][I_DATAKEY_STANDARDID_INDEX]);
                    int iDivisionId = Convert.ToInt32(grdStudents.DataKeys[e.Row.RowIndex][I_DATAKEY_DIVISIONID_INDEX]);
					string sRegNo = string.Empty;
                    if (optExact.Checked)
                        sRegNo = txtReg.Text;
                    else
                        sRegNo = txtName.Text;
                    sQueryString = sUrl.Substring(sUrl.IndexOf("?") + 1)
                                            + "&StandardId=" + iStandardId.ToString()
                                            + "&DivisionId=" + iDivisionId.ToString()
                                            + "&standardName=" + string.Empty
                                            + "&DivisionName=" + string.Empty
                                            + "&NewMode=" + "N"
                                            + "&pIndex=" + grdStudents.PageIndex.ToString()
                                            + "&pSortExp=" + hidSortExpression.Value
                                            + "&pSortDirc=" + hidSortDirection.Value
                                            + "&Is_Configured=" + hidIsConfig.Value
                                            + "&DivSelectedValue=" + cmbDivision.SelectedValue
                                            + "&StdSelectedValue=" + cmbStandard.SelectedValue
                                            + "&NameOrRegNo=" + sRegNo
                                            + "&abIsExactMatch=" + optExact.Checked
                                            + "&IsSchoolLeft=" + grdStudents.DataKeys[e.Row.RowIndex][3]
                                            + "&ClassId=" + cmbClass.SelectedValue.ToString()
                                            + "&asOperator=" + cmbOperation.SelectedValue
                                            + "&asPrefix=" + cmbPrefix.SelectedValue
                                              + "&asPostfix=" + cmbPrefix.SelectedValue
                                            + "&SearchedNumber=" + sRegNo
                                            + "&Is_SuperAdmin=" + hidIsSuperAdmin.Value;
                    if (SchoolBase.Settings.AllowStudentUpdateForClassTeacher.ToLower().ToBool() == false && Session[Constants.S_SESSION_IS_CLASS_TEACHER] != null && Session[Constants.S_SESSION_IS_CLASS_TEACHER].ToString() == Constants.S_YES)
                                            {                                               
                                                sQueryString = sQueryString+"&IsStudntDtailsScrn=" + Constants.S_YES;
                                            }
							oHyperLink.NavigateUrl = sQueryString;


                    if (grdStudents.DataKeys[e.Row.RowIndex][3] == DBNull.Value)
                    {
                        if (olnkAdditinalDetails != null)      
                             olnkAdditinalDetails.Enabled = false;

                        oHyperLink.NavigateUrl = sUrl.Substring(0, sUrl.IndexOf("?") + 1) + CommonUtility.EncryptQuerystring(sQueryString);
                        oHyperLink.ImageUrl = "~/RITeSchool/images/IconGrid_Edit.gif";
                        int iStudentId = Convert.ToInt32(grdStudents.DataKeys[e.Row.RowIndex][I_DATAKEY_ID_INDEX]);
                        DateTime oDateTime = Convert.ToDateTime(grdStudents.DataKeys[e.Row.RowIndex][I_DATAKEY_JOININGDATE_INDEX]);
                        DateTime oAdmissionDateTime = Convert.ToDateTime(grdStudents.DataKeys[e.Row.RowIndex][I_DATAKEY_ADMISSIONDATE_INDEX]);
                        hidStudAdmissionDate.Value = Convert.ToString(oAdmissionDateTime);                        
                        Image oDelete = (Image)e.Row.Cells[I_COLUMN_INDEX_DELETE].Controls[Constants.I_ZERO];
                        oDelete.Attributes.Add("onclick", "ShowPopup(this," + iStudentId + ",'" + oDateTime.ToShortDateString() + "','" + oAdmissionDateTime.ToShortDateString() + "','" + IsAttendanceAvailable + "');return false;");
                    }
                    else
                    {
                        oHyperLink.NavigateUrl = sUrl.Substring(0, sUrl.IndexOf("?") + 1) + CommonUtility.EncryptQuerystring(sQueryString);
                       
                        if (hidIsSuperAdmin.Value == "Y")
                        {
                            oHyperLink.ImageUrl = "~/RITeSchool/images/IconGrid_Edit.gif";
                            oHyperLink.Enabled = true;
                        }
                        else
                            oHyperLink.Enabled = false;

                        Image oDelete = (Image)e.Row.Cells[I_COLUMN_INDEX_DELETE].Controls[Constants.I_ZERO];
                        oDelete.Visible = false;
                        //oDelete.Attributes.Add("onclick", "if(!ConfirmDelete('Delete','" + IsAttendanceAvailable + "')){return false;}");
                        if (grdStudents.DataKeys[e.Row.RowIndex][9] == DBNull.Value)
                        {
                            LinkButton lbLeftDateButon =new  LinkButton();                         
                            lbLeftDateButon.Text = "<font size='1'>Left On " + Convert.ToDateTime(grdStudents.DataKeys[e.Row.RowIndex][3]).ToString("MMM dd") + "</font>";
                            //e.Row.Cells[I_COLUMN_INDEX_EDIT].Text = "<font size='1'>Left On " + Convert.ToDateTime(grdStudents.DataKeys[e.Row.RowIndex][3]).ToString("MMM dd") + "</font>";
                           
                            string sUserGuidURL = sUrl.Substring(0, sUrl.IndexOf("?") + 1) + CommonUtility.EncryptQuerystring(sQueryString);
                            lbLeftDateButon.Attributes.Add("onclick", "OpenWindow('" + sUserGuidURL + "')");
                            if (hidIsSuperAdmin.Value != "Y")
                            {
                                e.Row.Cells[I_COLUMN_INDEX_EDIT].Text = "";
                                e.Row.Cells[I_COLUMN_INDEX_EDIT].Controls.Add(lbLeftDateButon);
                      
                            }
                        }
                        else
                        {

                           if (hidIsSuperAdmin.Value != "Y")
                           e.Row.Cells[I_COLUMN_INDEX_EDIT].Text = "<font size='1'>Form No. " + (grdStudents.DataKeys[e.Row.RowIndex][9]).ToString() + " Left On " + Convert.ToDateTime(grdStudents.DataKeys[e.Row.RowIndex][3]).ToString("MMM dd") + "</font>";
                            
                        }
                        if (olnkAdditinalDetails != null)
                        {
                            String sAdditionalDetailsUrl = olnkAdditinalDetails.NavigateUrl;
                            olnkAdditinalDetails.NavigateUrl = sAdditionalDetailsUrl.Substring(0, sAdditionalDetailsUrl.IndexOf("?") + 1) + CommonUtility.EncryptQuerystring(sQueryString);

                            char cIsLeave = Convert.ToChar(grdStudents.DataKeys[e.Row.RowIndex][0].ToString());
                            if (cIsLeave == Convert.ToChar(Constants.C_YES))
                                olnkAdditinalDetails.Text = "Edit";
                            else
                                olnkAdditinalDetails.Text = "Add";
                        }
                        
                        e.Row.Style.Add(HtmlTextWriterStyle.Color, "red");

                    }
                }
                if (moUserRole == Constants.UserRoles.Teacher && !Boolean.Parse(hidUserHasFullAccess.Value))
                {
                    if (oHyperLink != null)
                    {
                        oHyperLink.Attributes.Add("onclick", "window.open('" + oHyperLink.NavigateUrl
                         + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=700').focus(); return false;");
                    }
                    if (olnkAdditinalDetails != null && olnkAdditinalDetails.Enabled)
                    {
                        olnkAdditinalDetails.Attributes.Add("onclick", "window.open('" + olnkAdditinalDetails.NavigateUrl
                         + "' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=700').focus(); return false;");
                    }
                }

                Image oImg = (Image)e.Row.FindControl("imgPhoto");
                if (oImg != null)
                {
                    //If photo is uploaded
					if (grdStudents.DataKeys[e.Row.RowIndex][11] == DBNull.Value)
                        oImg.ImageUrl = "~/RITeSchool/images/IconGridStudentBlankPh.gif";
                    else
                        oImg.ImageUrl = "~/RITeSchool/images/IconGrid_AssignTrue.gif";
                }
                if (grdStudents.DataKeys[e.Row.RowIndex]["StudentIsOnLeave"].ToString() != "0")
                {
                    string sToolTip = grdStudents.DataKeys[e.Row.RowIndex]["StudentIsOnLeave"].ToString();
                    e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "LightSteelBlue");
                    e.Row.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                    e.Row.Attributes.Add("title", sToolTip);
                }

            }
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow pagerRow = e.Row;

                // Retrieve the DropDownList and Label controls from the row.
                DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
                Label pageLabel = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel");

                if (pageList != null)
                {
                    for (int iPageCount = 0; iPageCount < grdStudents.PageCount; iPageCount++)
                    {
                        int pageNumber = iPageCount + 1;
                        ListItem item = new ListItem(pageNumber.ToString());
                        if (iPageCount == grdStudents.PageIndex)
                            item.Selected = true;
                        pageList.Items.Add(item);
                    }
                }
                if (pageLabel != null)
                {
                    int currentPage = grdStudents.PageIndex + 1;
                    pageLabel.Text = Resources .LocalizedResources.PageNo + currentPage.ToString() +
                      Resources.LocalizedResources.Of + grdStudents.PageCount.ToString() + " " + Resources.LocalizedResources.OutOflst;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to delete student details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == S_ROWCMD_DELETE_STUDENT)
            {
                int iRowIndex = Convert.ToInt32(e.CommandArgument);
                int iStudentId = Convert.ToInt32(grdStudents.DataKeys[iRowIndex][I_DATAKEY_ID_INDEX]);
                DeleteStudentDetails(iStudentId, true);
                if (grdStudents.Rows.Count == 0)
                    grdStudents.SetPageIndex(0);
            }
            else
            {
                Session.Add("ClassId", cmbClass.SelectedValue);
                SetPrefixCombo();
            }
        }
        catch (ReferenceExceptions ex)
        {
            lblErrorMsg.Text = ex.Message.Replace("You cannot delete this student since there is data for this student is avilable for", Resources.LocalizedResources.ExceptionDeleteStudent);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill grid on change of page index.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdStudents.PageIndex = e.NewPageIndex;
            grdStudents.DataSourceID = GrdDSobj.ID;
            FillStudentGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set sort direction image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, sGridviewName.SortExpression);
                if (sortColumnIndex != -1)
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, sGridviewName.SortDirection);
                else
                    CommonUtility.AddSortImage(1, e.Row, sGridviewName.SortDirection);

                if (optExact.Checked)
                {
                    cmbPrefix.Enabled = true;
                    cmbOperation.Enabled = true;
                    txtReg.Enabled = true;
                    txtReg.Focus();
                }
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set values to record range labels.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        SetGridViewDateColumnProperties();
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
            {
                lblStartIndex.Text = Convert.ToString((grdStudents.PageSize * grdStudents.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdStudents.PageSize) - 1);
                if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                        if (e.ReturnValue.ToString() == "0")
                            tdTotalRec.Visible = false;
                        else
                            tdTotalRec.Visible = true;
                    }
                    if (lblTotal.Text != string.Empty)
                    {
                        if (Convert.ToInt32(lblTotal.Text) <= Constants.I_GRID_PAGE_COUNT)
                            tdTotalRec.Visible = false;
                        else
                            tdTotalRec.Visible = true;
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
    /// This event is used to check login user and provide delete facility.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (moUserRole != Constants.UserRoles.Admin && Convert.ToChar(hidCanEdit.Value) == Constants.C_NO)
            {
                grdStudents.Columns[I_COLUMN_INDEX_DELETE].Visible = false;
                grdStudents.Columns[I_COLUMN_INDEX_LC_ADD].Visible = false;
            }
            Session.Add("ClassId",cmbClass.SelectedValue);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #endregion

    #region Methods
    /// <summary>
    /// This method is used to get the Standard list.
    /// </summary>
    private void GetStandardDivisionList()
    {
        StandardDivisionCollectionBL oStandardDivisionCollectionBL = new StandardDivisionCollectionBL(miSchoolId, miAcademicYearId);
        List<StandardDivisionMaster> lstStandardDivisionMaster = oStandardDivisionCollectionBL.GetStandardDivisionList();
        Session.Add("StandardDivisionList", lstStandardDivisionMaster);
    }

    /// <summary>
    /// This method is used to set the combo boxes on the Registration number.
    /// </summary>
    private void SetPrefixCombo()
    {
        if (optExact.Checked && txtReg.Text.IsNullOrEmpty())
        {
            cmbOperation.SelectedIndex = Constants.I_ZERO;
            cmbPrefix.SelectedIndex = Constants.I_ZERO;
        }
    }

    /// <summary>
    /// This method is used to get the list of prefixes.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <returns></returns>
    private void GetPrefixes()
    {
        List<string> olstPrefixes = StudentBL.GetPrefixes(miSchoolId, miAcademicYearId);
        cmbPrefix.Items.Add(new ListItem(Constants.S_ALL, Constants.S_ALL));
        if (olstPrefixes.Count > Constants.I_ZERO)
            olstPrefixes.ForEach(pfx => cmbPrefix.Items.Add(new ListItem(pfx, pfx)));
    }

    /// <summary>
    /// This method is used to get the list of postfixes.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <returns></returns>
    private void GetAllRegNoPostfixes()
    {
        List<string> lstRegNoPostfixes = StudentBL.GetAllRegNoPostfixes(miSchoolId, miAcademicYearId);
        if (lstRegNoPostfixes.Count > Constants.I_ZERO)
            lstRegNoPostfixes.ForEach(pfx => cmbPrefix.Items.Add(new ListItem(pfx, pfx)));
    }


    /// <summary>
    /// This is used set the selected value in combo.
    /// </summary>
    private void SetClassComboValue()
    {
        if (Request.QueryString.ToString() != string.Empty)
        {
            if (Session["ClassId"] != null)
            {
                hidStandardDivisionId.Value = Session["ClassId"].ToString();
                Session.Remove("ClassId");
            }
        }      
    }

    /// <summary>
    /// This method is used enable/disable delete date controls.
    /// </summary>
    /// <param name="ablag"></param>
    private void DisableDate(bool ablag)
    {
        caltxtDeletedDate.Enabled = ablag;
        txtCancFormNo.Enabled = ablag;
        txtDeletedDate.Enabled = ablag;
        chkIncludeinBlackList.Checked = false;
        txtComment.Enabled = false;
    }

    /// <summary>
    /// This method is used to add javascript attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        optMain.Enabled = true;
        optExact.Checked = false;
        optMain.Checked=true;
	    ApplyMouseHoverEffect(new List<Button> { btnAdd, btnBack, btnSearch, btnExport, btnUpdateRegNo, btnUpload,btnDeleteStud,btnCancel });
	    chkcompleteDelete.Attributes.Add("onclick", " javascript:SetDate()");
        cmbClass.Attributes.Add("onchange", "javascript:ValidateInput(this)");
        cmbStandard.Attributes.Add("onchange", "javascript:ValidateInput(this)");
        cmbDivision.Attributes.Add("onchange", "javascript:ValidateInput(this)");

        if (miSchoolId == Constants.SchoolId.SNS.ToInt())
        {
            hidAllowFutureDate.Value = Constants.S_ONE;
            hidLockBlacklistOption.Value = "Y";
        }
        else
        {
            hidAllowFutureDate.Value = Constants.S_ZERO;
            hidLockBlacklistOption.Value = "N";
        }
    }
    /// <summary>
    /// This method is used to check role,access and hide grid columns.
    /// </summary>
    private void CheckRoleAndSetGridview()
    {
        
if (moUserRole == Constants.UserRoles.Supervisor ||
            moUserRole == Constants.UserRoles.Teacher)
        {
            hidCanEdit.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.Student).ToString();

            if (hidCanEdit.Value == "N")
            {
                HideAddBtn(false);
                btnExport.Visible = false;
                grdStudents.Columns[I_COLUMN_INDEX_EDIT].Visible = false;
                grdStudents.Columns[I_COLUMN_INDEX_PHOTO].Visible = false;
                grdStudents.Columns[I_COLUMN_INDEX_DELETE].Visible = false;
                grdStudents.Columns[I_COLUMN_INDEX_LC_ADD].Visible = false;
            }
            if (CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.Student).ToString() == "False")
            {
                string sIsClassTEacher = Session[Constants.S_SESSION_IS_CLASS_TEACHER].ToString();
                if (sIsClassTEacher == "Y")
                {
                    grdStudents.Columns[I_COLUMN_INDEX_EDIT].Visible = true;
                    grdStudents.Columns[I_COLUMN_INDEX_PHOTO].Visible = true;                      
                }
            }
        }
    }

    /// <summary>
    /// This method is used to delete student details.
    /// </summary>
    private void DeleteStudentDetails(int aiStudentId, bool abIsDelete)
    {
        StudentBL oStudentBL = new StudentBL(); 
        int iCancellationFormNo;
        int IsFormNo;
        int iFormCount = 0;

        bool abIncludeinBlackList = chkIncludeinBlackList.Checked;
        string asComment = txtComment.Text.Trim();

        if (txtCancFormNo.Text != string.Empty)
        {
            iCancellationFormNo = Convert.ToInt32(txtCancFormNo.Text);
            IsFormNo = 1;
        }
        else
        {
            iCancellationFormNo = 0;
            IsFormNo = 0;
            iFormCount = 0;
        }
        if (chkSchoolLeaving.Checked && abIsDelete == false)
        {
            if (txtCancFormNo.Text == string.Empty)
                oStudentBL.DeleteStudent(miSchoolId, miAcademicYearId, aiStudentId, caltxtDeletedDate.DateValue, Constants.C_NO, IsFormNo, iCancellationFormNo, miUserId, abIncludeinBlackList, asComment);
            else
            {
                iFormCount = oStudentBL.GetFormNoCount(iCancellationFormNo, miSchoolId);
                if (iFormCount == 0)
                    oStudentBL.DeleteStudent(miSchoolId, miAcademicYearId, aiStudentId, caltxtDeletedDate.DateValue, Constants.C_NO, IsFormNo, iCancellationFormNo, miUserId, abIncludeinBlackList, asComment);
                else
                {
                    lblDuplicateMsg.Visible = true;
                    lblDuplicateMsg.Text = Resources.LocalizedResources.CancelFormNumber;
                }
            }
        }
        else
        {
            if(hidIsSuperAdmin.Value==Constants.S_NO)
                oStudentBL.ValidateStudent(miSchoolId, aiStudentId);
            iCancellationFormNo = 0;
            IsFormNo = 0;
            oStudentBL.DeleteStudent(miSchoolId, miAcademicYearId, aiStudentId, caltxtDeletedDate.DateValue, Constants.C_YES, IsFormNo, iCancellationFormNo, miUserId, abIncludeinBlackList, asComment);
        }
        grdStudents.DataBind();

        chkSchoolLeaving.Checked = true;
        chkcompleteDelete.Checked = false;
        txtDeletedDate.Enabled = true;
        txtCancFormNo.Enabled = true;
        caltxtDeletedDate.Enabled = true;
        txtCancFormNo.Text = string.Empty;
        chkIncludeinBlackList.Checked = false;
        txtComment.Enabled = false;
    }


    /// <summary>
    /// This method is used to set the date format for date columns.
    /// </summary>    
    private void SetGridViewDateColumnProperties()
    {
        BoundField oReceivedDate = (BoundField)grdStudents.Columns[I_COLUMN_INDEX_DOB];
        oReceivedDate.HtmlEncode = false;
        oReceivedDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
        if (moUserRole == Constants.UserRoles.Teacher
                && Session[Constants.S_SESSION_IS_CLASS_TEACHER].ToString() == Constants.C_YES.ToString())
            grdStudents.Columns[I_COLUMN_INDEX_CLASS].Visible = false;
    }

    /// <summary>
    /// This function is used to initialise the sort variables
    /// </summary>
    private void SetSortProperties()
    {
        hidSortExpression.Value = grdStudents.Columns[I_COL_INDEX_ROLL_NO].SortExpression;
        hidSortDirection.Value = Constants.S_ASCENDING;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
    }

    /// <summary>
    /// This method is used to fill the operators in the dropdownlist.
    /// </summary>
    private void FillOperators()
    {
        List<Operator> olstOperators = StudentBL.GetOperators();
        ListSource.FillDropDownList(olstOperators, cmbOperation, "Text", "Value", string.Empty);
    }

    ///// <summary>
    ///// This method is used to fill grid according to the search criteria.
    ///// </summary>
    private void FillStudentGrid()
    {
        SetGridViewDateColumnProperties();
        grdStudents.DataSourceID = GrdDSobj.ID;
        if ((moUserRole != Constants.UserRoles.Admin
                && Convert.ToChar(hidCanEdit.Value) == Constants.C_NO) && !(
            moUserRole == Constants.UserRoles.Teacher &&
            Convert.ToChar(Session[Constants.S_SESSION_IS_CLASS_TEACHER]) == Constants.C_YES))
        {
            grdStudents.Columns[I_COLUMN_INDEX_EDIT].Visible = false;
            grdStudents.Columns[I_COLUMN_INDEX_DELETE].Visible = false;
            grdStudents.Columns[I_COLUMN_INDEX_LC_ADD].Visible = false;
            grdStudents.Columns[I_COLUMN_INDEX_PHOTO].Visible = false;
            HideAddBtn(false);
        }
        if (moUserRole != Constants.UserRoles.Admin && Convert.ToChar(hidCanEdit.Value) == Constants.C_NO)   
            grdStudents.Columns[I_COLUMN_INDEX_LC_ADD].Visible = false;       
    }


    /// <summary>
    /// This function is used to change sort order.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This function is used to initialize date values.
    /// </summary>
    private void InitialiseDateValues()
    {
        //string m = DateCultureConversion(System.DateTime.Now.ToString(), "en", Session[Constants.S_SESSION_LANGUAGE].ToString());
        string str = DateTime.Today.ToString();
        caltxtDeletedDate.DateValue = DateTime.Now;// Convert.ToDateTime(DateTime.Now, new CultureInfo("mr", false).DateTimeFormat).ToString((new CultureInfo("en", false).DateTimeFormat)).ToDateTime();
       // txtDeletedDate.Text = string.Format("dd MMM yyyy", caltxtDeletedDate.DateValue, CultureInfo.CurrentCulture);
        cmbStandard.Focus();
    }

    /// <summary>
    /// This function is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        grdStudents.PageSize = Constants.I_GRID_PAGE_COUNT;
        grdStudents.EmptyDataText = Resources .LocalizedResources.NoStudentAvailable;
        hidSchoolId.Value = miSchoolId.ToString();
        hidAcademicYearId.Value = miAcademicYearId.ToString();
        hidUserHasFullAccess.Value = bIsUserHasfullAccess.ToString();

        if (moUserRole == Constants.UserRoles.Teacher)
            btnBack.Visible = false;
    }

    /// <summary>
    /// This method is used to display view according to user.
    /// </summary>
    private void DisplayViewAccordingToUser()
    {
		tblClass.Visible = false;

        if (moUserRole != Constants.UserRoles.Admin
            && moUserRole != Constants.UserRoles.Supervisor
            && !Boolean.Parse(hidUserHasFullAccess.Value))
        {
            if (Session[Constants.S_SESSION_IS_CLASS_TEACHER] != null && Session[Constants.S_SESSION_IS_CLASS_TEACHER].ToString() == Constants.C_YES.ToString())
            {
                tblSearch.Visible = false;
                cmbDivision.Visible = false;
                cmbStandard.Visible = false;
                txtName.Visible = false;
                HideAddBtn(false);
                btnBack.Text = "Back";
                tdBack.Visible = true;
                SetStandardDivisionOfTeacher();
                SetIdentitylinkURL();
                SetPhotoURL();
                div1.Visible = false;
            }
            else
            {
                tdTotalRec.Visible = false;
                lblErrorMsg.Text = Resources.LocalizedResources.MsgAccessDenied;
            }
        }
        if (moUserRole == Constants.UserRoles.Admin
          || Boolean.Parse(hidUserHasFullAccess.Value)) 
        {
            btnBack.Attributes.Add("onclick", "refreshParent()");
            hidStandardId.Value = cmbStandard.SelectedValue;
            hidDivisionId.Value = cmbDivision.SelectedValue;
            msStandardName = cmbStandard.SelectedItem.ToString();
            msDivisionName = cmbDivision.SelectedItem.ToString();

        }
    }

    /// <summary>
    /// This method is used to hide add button.
    /// </summary>
    private void HideAddBtn(bool bVisible)
    {
        btnAdd.Visible = bVisible;
        btnUpload.Visible = bVisible;
        btnUpdateRegNo.Visible = bVisible;
    }

    /// <summary>
    /// This method is used to set standard division of a teacher.
    /// </summary>
    private void SetStandardDivisionOfTeacher()
    {
        DataTable oDT = SchoolWiseStandardDivisionTeacherAssignmentMasterBL.GetStandardDivisionOfTeacher
                      (Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID].ToString()), miAcademicYearId);
		ControlUtility.FillDropDownList(oDT, ref ddlClassTeacher, Constants.S_STANDARD_DIVISION_ID_FIELD, Constants.S_STANDARD_DIVISION_NAME_FIELD,string.Empty);

        if (oDT.Rows.Count > 0)
        {
			if (oDT.Rows.Count == 1)
				ddlClassTeacher.Enabled = false;
	        msStandardName = oDT.Rows[0]["standard_Name"].ToString();
            msDivisionName = oDT.Rows[0]["division_Name"].ToString();
            hidStandardId.Value = oDT.Rows[0]["standard_Id"].ToString();
            hidDivisionId.Value = oDT.Rows[0]["division_Id"].ToString();
            hidStandardDivisionId.Value = oDT.Rows[0]["SchoolWise_Standard_Division_Id"].ToString();
            tblHeader.Visible = false;
			tblClass.Visible = true;
        }
    }

    /// <summary>
    /// This function is used to fill combobox with all standards.
    /// </summary>
    private void FillStandardCombobox()
    {
        int iSchoolId = Convert.ToInt32(hidSchoolId.Value);
        int iAcademicYearID = Convert.ToInt32(hidAcademicYearId.Value);
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(iSchoolId, iAcademicYearID);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDtStandardCollection, ref cmbStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT_ALL);
        cmbStandard.SelectedValue = hidLastSelectedStd.Value != "" ? hidLastSelectedStd.Value : hidStandardId.Value;

    }

    /// <summary>
    /// This function is used to fill Division combobox.
    /// </summary>
    /// <param name="aiStandardId"></param>
    private void FillDivisionCombobox()
    {
        int aiStandardId = Convert.ToInt32(hidStandardId.Value);
        int iSchoolId = Convert.ToInt32(hidSchoolId.Value);
        int iAcademicYearId = Convert.ToInt32(hidAcademicYearId.Value);
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(iSchoolId, iAcademicYearId);
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(aiStandardId);

        ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbDivision,
                                       Constants.S_DIVISION_ID_FIELD,
                                       Constants.S_DIVISION_NAME_FIELD,
                                       Constants.S_SELECT_ALL);
        cmbDivision.SelectedValue = hidLastSelectedDiv.Value != "" ? hidLastSelectedDiv.Value : hidDivisionId.Value;
        SetIdentitylinkURL();
        SetPhotoURL();
    }

    private void FillClassComboBox()
    {
        int iSchoolId = Convert.ToInt32(hidSchoolId.Value);
        int iAcademicYearId = Convert.ToInt32(hidAcademicYearId.Value);
        StandardDivisionCollectionBL oStandardDivisionCollectionBL = new StandardDivisionCollectionBL(iSchoolId, iAcademicYearId);
        DataTable oClass = oStandardDivisionCollectionBL.GetAssociatedStandardsDivisions();
        ControlUtility.FillDropDownList(oClass, ref cmbClass, Constants.S_STANDARD_DIVISION_ID_FIELD, Constants.S_STANDARD_DIVISION_NAME_FIELD, Constants.S_SELECT_ALL);
		
        cmbClass.SelectedValue = hidLastSelectedClass.Value != "" ? hidLastSelectedClass.Value : hidStandardDivisionId.Value;

        if (cmbStandard.SelectedValue == Constants.S_ZERO && cmbDivision.SelectedValue == Constants.S_ZERO)
        {
            hidStandardDivisionId.Value = Constants.S_ZERO;
            cmbClass.SelectedValue = Constants.S_ZERO;
        }
    }

    /// <summary>
    /// This method is used to decrypt the querystring.
    /// </summary>
    private void GetQuerystring()
    {
        try
        {
            if (QueryString["StdSelectedValue"] != null)
                hidStandardId.Value = QueryString["StdSelectedValue"];
            else if (QueryString["StandardId"] != null)
                hidStandardId.Value = QueryString["StandardId"];
            else
                hidStandardId.Value = "0";

            if (QueryString["DivSelectedValue"] != null)
                hidDivisionId.Value = QueryString["DivSelectedValue"];
            else if (QueryString["DivisionId"] != null)
                hidDivisionId.Value = QueryString["DivisionId"];
            else
                hidDivisionId.Value = "0";

            if (QueryString["Is_SuperAdmin"] != null)
                hidIsSuperAdmin.Value = QueryString["Is_SuperAdmin"];

            if (QueryString["pIndex"] != null)
                grdStudents.PageIndex = QueryString["pIndex"].ToInt();
            if (QueryString["pSortExp"] != null)
                hidSortExpression.Value = QueryString["pSortExp"];
            if (QueryString["pSortDirc"] != null)
                hidSortDirection.Value = QueryString["pSortDirc"];
            if (QueryString["Is_Configured"] != null)
                hidIsConfig.Value = QueryString["Is_Configured"];
            if (QueryString["asOperator"] != null)
            {
                hidOperator.Value = QueryString["asOperator"];
                cmbOperation.SelectedValue =hidOperator.Value.ToString();
            }

            if (QueryString["asPrefix"] != null)
            {
                hidPrefix.Value = QueryString["asPrefix"];
                cmbPrefix.SelectedValue = hidPrefix.Value.ToString();
            }


            if (QueryString["asPostfix"] != null)
            {
                hidPostfix.Value = QueryString["asPostfix"];
                cmbPrefix.SelectedValue = hidPostfix.Value.ToString();
            }
            
			if (QueryString["SearchedNumber"] != null)
                hidSearchedNumber.Value = QueryString["SearchedNumber"];
			string sSourcePageUrl = string.Empty;
            if (Request.UrlReferrer != null)
            {
                sSourcePageUrl = Request.UrlReferrer.AbsolutePath;
                sSourcePageUrl = sSourcePageUrl.Substring(sSourcePageUrl.LastIndexOf("/") + 1);
            }

            if (QueryString["abIsExactMatch"] != null)
            {
				if (!hidPrefix.Value.IsNullOrEmpty() && !hidOperator.Value.IsNullOrEmpty() && sSourcePageUrl == "StudentPhotoUploadUI.aspx")
                        hidIsExactMatch.Value = true.ToString();
                    else
                        hidIsExactMatch.Value = QueryString["abIsExactMatch"];
                    
                if (Convert.ToBoolean(hidIsExactMatch.Value))
                {
                    optExact.Checked = true;
                    SetControlsUponCriteria(false);
                    if (QueryString["RegNo"] != null)
                    {
						if (sSourcePageUrl != "StudentUI.aspx" && sSourcePageUrl!="LeavingCertificateUI.aspx")
                            {
                                txtReg.Text = QueryString["RegNo"];
                                hidStudentReg.Value = txtReg.Text.Trim();
                            }
						if (sSourcePageUrl == "StudentUI.aspx" || sSourcePageUrl == "LeavingCertificateUI.aspx")
                        {
                            txtReg.Text = hidSearchedNumber.Value;
                            hidStudentReg.Value = txtReg.Text;
                        }
                    }
                }
                else
                {
                    optMain.Checked = true;
                    SetControlsUponCriteria(true);
                    if (QueryString["NameOrRegNo"] != null)
                    {
						if (sSourcePageUrl != "StudentUI.aspx" && sSourcePageUrl != "LeavingCertificateUI.aspx")
                            {
                                txtName.Text = QueryString["NameOrRegNo"];
                                hidStudentName.Value = txtName.Text.Trim();
                            }
						if (sSourcePageUrl == "StudentUI.aspx" || sSourcePageUrl == "LeavingCertificateUI.aspx")
                        {
                            txtName.Text = hidSearchedNumber.Value;
                            hidStudentName.Value = txtName.Text;
                        }
                    }
                }
            }
            else
            {
                optMain.Checked = true;
                SetControlsUponCriteria(true);
            }

            if (!string.IsNullOrEmpty(hidStandardId.Value) && hidStandardId.Value != "0")
            {
                DataTable oDT = SchoolWiseAcademicYearMasterBL.GetAcademicDatesForStandard(miSchoolId,
                                                                                           miAcademicYearId,
                                                                                           hidStandardId.Value.ToInt());
                if (oDT.Rows.Count > 0)
                {
                    hidAcademicStartDate.Value = oDT.Rows[0]["StartDate"].ToString();
                    hidAcademicEndDate.Value = oDT.Rows[0]["EndDate"].ToString();
                }
                else
                {
                    trMsg.Visible = true;
                    lblMsg.Text = Resources.LocalizedResources.ErrMsgConfigureAcadamicYear;
                }
            }
            else
            {
                hidAcademicStartDate.Value = Convert.ToString(Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE]);
                hidAcademicEndDate.Value = Convert.ToString(Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE]);
            }
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master; oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    /// <summary>
    /// This method is used to set decrypted URL to toppres links
    /// </summary>
    private void SetIdentitylinkURL()
    {
        hlnkIdentity.Enabled = true;
        int iStandardId = 0;
        int iDivisionId = 0;
        if (moUserRole == Constants.UserRoles.Teacher)
        {
            iStandardId = Convert.ToInt32(hidStandardId.Value);
            iDivisionId = Convert.ToInt32(hidDivisionId.Value);
        }
        else
        {
            iStandardId = cmbStandard.SelectedValue == string.Empty ? 0 : Convert.ToInt32(cmbStandard.SelectedValue);
            iDivisionId = cmbDivision.SelectedValue == string.Empty ? 0 : Convert.ToInt32(cmbDivision.SelectedValue);
        }
        String QueryString = "iStandardId=" + iStandardId.ToString() + "&iDivisionId=" + iDivisionId.ToString() +
                             "&sStudentName=" + txtName.Text.Trim() + "&sStudentReg=" + txtReg.Text.Trim();
        QueryString = "../Teacher/StudentIdentityCards.aspx?" + CommonUtility.EncryptQuerystring(QueryString);
        hlnkIdentity.Attributes.Add("onclick", "ShowIdentities('" + QueryString + "');return false;");
    }

    /// <summary>
    /// This method is used to set decrypted URL to toppres link
    /// </summary>
    private void SetPhotoURL()
    {
        hlnkPhotos.Enabled = true;
        int iStandardId = 0;
        int iDivisionId = 0;
        if (moUserRole == Constants.UserRoles.Teacher)
        {
            iStandardId = Convert.ToInt32(hidStandardId.Value);
            iDivisionId = Convert.ToInt32(hidDivisionId.Value);
        }
        else
        {
            iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
            iDivisionId = Convert.ToInt32(cmbDivision.SelectedValue);
        }

		int IsExact=0;
        if(optExact.Checked)
            IsExact=1;
        String QueryString = "StandardId=" + iStandardId.ToString() + "&DivisionId=" + iDivisionId.ToString() +
                             "&NameOrRegNo=" + txtName.Text.Trim() + "&sStudentReg=" + txtReg.Text.Trim() +
                             "&Operator=" + cmbOperation.SelectedValue + "&Prefix=" + cmbPrefix.SelectedItem.Value + "&Postfix=" + cmbPrefix.SelectedItem.Value +
                             "&IsExactMatch=" + IsExact;
        QueryString = "../Teacher/PhotoForStudent.aspx?" + CommonUtility.EncryptQuerystring(QueryString);
        hlnkPhotos.Attributes.Add("onclick", "ShowPhotos('" + QueryString + "');return false;");

    }


    /// <summary>
    /// This method generates the report filter as per the field selection.
    /// </summary>
    /// <returns></returns>
    private string getFilterString()
    {
        StringBuilder sFilter = new StringBuilder();
        string sSchoolYearFilter;
        int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
        int iDivisionId = Convert.ToInt32(cmbDivision.SelectedValue);
        string sViewNameSchID = S_EXPORT_STUDENT + ".iSchoolId}";
        string sViewNameAcdYearId = S_EXPORT_STUDENT + ".iAcademicYrId}";
        string sViewNameStdID = S_EXPORT_STUDENT + ".Standard_Id}";
        string sViewNameDivID = S_EXPORT_STUDENT + ".Division_id }";
        string sViewNameName = S_EXPORT_STUDENT + ".Name }";
        string sViewNameRegNo = S_EXPORT_STUDENT + ".RegNo }";
        string sViewIsIncludeLeft = S_EXPORT_STUDENT + ".IncludeLeft }";
        string sViewNameIsExact = S_EXPORT_STUDENT + ".IsExact }";
        string sPrefix = S_EXPORT_STUDENT + ".Prefix }";
        string sOperator = S_EXPORT_STUDENT + ".Operator }";
        string sStandardList = S_EXPORT_STUDENT + ".StandardList }";
        string sDivisionList = S_EXPORT_STUDENT + ".DivisionList }";
        string isSuperAdmin = S_EXPORT_STUDENT + ".IsSuperAdmin}";
        if (iStandardId == 0)
        {
            
            if (optExact.Checked)
            {
                sSchoolYearFilter = "(" + sViewNameSchID + "=" + miSchoolId + " AND " +
                                                               sViewNameAcdYearId + "=" + miAcademicYearId +
                                                                " AND " +
                                          sViewNameStdID + "=" + iStandardId + " AND " +
                                          sViewNameDivID + "=" + iDivisionId + " AND " +
                                          sViewNameName + "=" + (txtName.Text.Trim() == "" ? "-9999" : txtName.Text.Trim()) + " AND " +
                                          sViewIsIncludeLeft + "=" + hidIncludeLeft.Value + " AND " +
                                          sViewNameRegNo + "=" + (txtReg.Text.Trim() == "" ? "-9999" : txtReg.Text.Trim()) + " AND " +
                                          sViewNameIsExact + "=" + (Convert.ToInt32(optExact.Checked)) + " AND " +
                                          sPrefix + "=" + (cmbPrefix.SelectedValue.ToString() == "All" ? "" : cmbPrefix.SelectedValue.ToString()) + " AND " +
                                          sOperator + "=" + (cmbOperation.SelectedItem.Text.ToString().Replace("=","$")) + " AND " +
                                          sStandardList + "=null" + " AND " +
                                          sDivisionList + "=null" + " AND " +
                                          isSuperAdmin + "=" + hidIsSuperAdmin.Value +
                                          ")";
            }
            else
            {
                sSchoolYearFilter = "(" + sViewNameSchID + "=" + miSchoolId + " AND " +
                                          sViewNameAcdYearId + "=" + miAcademicYearId + " AND " +
                                          sViewNameStdID + "=" + iStandardId + " AND " +
                                          sViewNameDivID + "=" + iDivisionId + " AND " +
                                          sViewNameName + "=" + (txtName.Text.Trim() == "" ? "-9999" : txtName.Text.Trim()) + " AND " +
                                          sViewIsIncludeLeft + "=" + hidIncludeLeft.Value + " AND " +
                                          sViewNameRegNo + "=" + (txtReg.Text.Trim() == "" ? "-9999" : txtReg.Text.Trim()) + " AND " +
                                          sViewNameIsExact + "=" + (Convert.ToInt32(optExact.Checked)) + " AND " +
                                          sPrefix + "="+ "''"+" AND "+
                                          sOperator + "=" + "" + " AND " +
                                          sStandardList + "=null" + " AND " +
                                          sDivisionList + "=null" + " AND " +
                                          isSuperAdmin + "=" + hidIsSuperAdmin.Value +
                                          ")";
            }
        }
        else
        {
            if (iDivisionId != 0)
            {
                if (optExact.Checked)
                {
                    sSchoolYearFilter = "(" + sViewNameSchID + "=" + miSchoolId + " AND " +
                                          sViewNameAcdYearId + "=" + miAcademicYearId + " AND " +
                                          sViewNameStdID + "=" + iStandardId + " AND " +
                                          sViewNameDivID + "=" + iDivisionId + " AND " +
                                          sViewNameName + "=" + (txtName.Text.Trim() == "" ? "-9999" : txtName.Text.Trim()) + " AND " +
                                          sViewIsIncludeLeft + "=" + hidIncludeLeft.Value + " AND " +
                                          sViewNameRegNo + "=" + (txtReg.Text.Trim() == "" ? "-9999" : txtReg.Text.Trim()) + " AND " +
                                          sViewNameIsExact + "=" + (Convert.ToInt32(optExact.Checked)) + " AND " +
                                          sPrefix + "=" + (cmbPrefix.SelectedValue.ToString() == "All" ? "" : cmbPrefix.SelectedValue.ToString()) + " AND " +
                                          sOperator + "=" + (cmbOperation.SelectedItem.Text.ToString().Replace("=", "$")) + " AND " +
                                          sStandardList + "=null" + " AND " +
                                          sDivisionList + "=null" + " AND " +
                                          isSuperAdmin + "=" + hidIsSuperAdmin.Value +
                                          ")";
                }
                else
                {
                    sSchoolYearFilter = "(" + sViewNameSchID + "=" + miSchoolId + " AND " +
                                         sViewNameAcdYearId + "=" + miAcademicYearId + " AND " +
                                         sViewNameStdID + "=" + iStandardId + " AND " +
                                         sViewNameDivID + "=" + iDivisionId + " AND " +
                                         sViewNameName + "=" + (txtName.Text.Trim() == "" ? "-9999" : txtName.Text.Trim()) + " AND " +
                                         sViewIsIncludeLeft + "=" + hidIncludeLeft.Value + " AND " +
                                         sViewNameRegNo + "=" + (txtReg.Text.Trim() == "" ? "-9999" : txtReg.Text.Trim()) + " AND " +
                                         sViewNameIsExact + "=" + (Convert.ToInt32(optExact.Checked)) + " AND " +
                                         sPrefix + "=" + "''" + " AND " +
                                         sOperator + "=" + "" + " AND " +
                                         sStandardList + "=null" + " AND " +
                                         sDivisionList + "=null" + " AND " +
                                         isSuperAdmin + "=" + hidIsSuperAdmin.Value +
                                         ")";
                }
            }
            else
            {
                if (optExact.Checked)
                {
                    sSchoolYearFilter = "(" + sViewNameSchID + "=" + miSchoolId + " AND " +
                                          sViewNameAcdYearId + "=" + miAcademicYearId + " AND " +
                                          sViewNameStdID + "=" + iStandardId + " AND " +
                                          sViewNameDivID + "=0" + " AND " +
                                          sViewNameName + "=" + (txtName.Text.Trim() == "" ? "-9999" : txtName.Text.Trim()) + " AND " +
                                          sViewIsIncludeLeft + "=" + hidIncludeLeft.Value + " AND " +
                                          sViewNameRegNo + "=" + (txtReg.Text.Trim() == "" ? "-9999" : txtReg.Text.Trim()) + " AND " +
                                          sViewNameIsExact + "=" + (Convert.ToInt32(optExact.Checked)) + " AND " +
                                          sPrefix + "=" + (cmbPrefix.SelectedValue.ToString() == "All" ? "" : cmbPrefix.SelectedValue.ToString()) + " AND " +
                                          sOperator + "=" + (cmbOperation.SelectedItem.Text.ToString().Replace("=", "$")) + " AND " +
                                          sStandardList + "=null" + " AND " +
                                          sDivisionList + "=null" + " AND " +
                                          isSuperAdmin + "=" + hidIsSuperAdmin.Value +
                                          ")";
                }
                else
                {
                    sSchoolYearFilter = "(" + sViewNameSchID + "=" + miSchoolId + " AND " +
                                              sViewNameAcdYearId + "=" + miAcademicYearId + " AND " +
                                              sViewNameStdID + "=" + iStandardId + " AND " +
                                              sViewNameDivID + "=0" + " AND " +
                                              sViewNameName + "=" + (txtName.Text.Trim() == "" ? "-9999" : txtName.Text.Trim()) + " AND " +
                                              sViewIsIncludeLeft + "=" + hidIncludeLeft.Value + " AND " +
                                              sViewNameRegNo + "=" + (txtReg.Text.Trim() == "" ? "-9999" : txtReg.Text.Trim()) + " AND " +
                                              sViewNameIsExact + "=" + (Convert.ToInt32(optExact.Checked)) + " AND " +
                                              sPrefix + "="+ "''"+" AND "+
                                              sOperator + "=" + "" + " AND " +
                                              sStandardList + "=null" + " AND " +
                                              sDivisionList + "=null" + " AND " +
                                              isSuperAdmin + "=" + hidIsSuperAdmin.Value +
                                              ")";
                }
            }
          
        }

        sFilter.AppendFormat("{0}@ ", sSchoolYearFilter);
        return sFilter.ToString();
    }

    /// <summary>
    /// This method is used to set querystring and redirect towards Student Photos / Update Reg. No UI.
    /// </summary>
    /// <param name="abIsUpload"></param>
    private void SetQueryString(bool abIsUpload)
    {
        string UploadPopUp = string.Empty;
        if (abIsUpload)
            UploadPopUp = "~/RITeSchool/Teacher/StudentPhotoUploadUI.aspx";
        else
            UploadPopUp = "~/RITeSchool/Admin/RegNoReassignUI.aspx";

        StringBuilder sQueryString = new StringBuilder();
        
        sQueryString.AppendFormat("StandardId={0}", cmbStandard.SelectedValue);
        sQueryString.AppendFormat("&DivisionId={0}", cmbDivision.SelectedValue);
        sQueryString.AppendFormat("&NameOrRegNo={0}", txtName.Text.Trim());
        
        if (optExact.Checked)
        {
            hidPrefix.Value = cmbPrefix.SelectedValue;
            hidOperator.Value = cmbOperation.SelectedValue;
        }
        sQueryString.AppendFormat("&RegNo={0}", txtReg.Text.Trim());
        sQueryString.AppendFormat("&abIsExactMatch={0}", hidIsExactMatch.Value);
        sQueryString.AppendFormat("&asOperator={0}", hidOperator.Value);
        sQueryString.AppendFormat("&asPrefix={0}", hidPrefix.Value);
        sQueryString.AppendFormat("&asPostfix={0}", hidPostfix.Value);
		sQueryString.AppendFormat("&Is_SuperAdmin={0}", hidIsSuperAdmin.Value);
        string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString.ToString());
        string sUrl = String.Format("{0}?{1}", UploadPopUp, sEncrypt);
        Response.Redirect(sUrl, false);
    }

    /// <summary>
    /// This method is used to set view for full access user.
    /// </summary>
    private void SetViewForFullAccessUser()
    {
        if (moUserRole == Constants.UserRoles.Admin ||
                   Boolean.Parse(hidUserHasFullAccess.Value))
        {
            tdTotalRec.Visible = true;
            FillStandardCombobox();
            FillDivisionCombobox();
            FillClassComboBox();
            SetIdentitylinkURL();
            SetPhotoURL();
        }
    }

    /// <summary>
    /// This method is used to convert filter string into required format.
    /// </summary>
    /// <param name="sFilter"></param>
    /// <returns>string</returns>
    private string FormatFilterString(string asFilterString)
    {
        asFilterString = asFilterString.Replace("AND", "@");
        asFilterString = asFilterString.Replace("OR", "@");
        asFilterString = asFilterString.Replace("(", string.Empty);
        asFilterString = asFilterString.Replace(")", string.Empty);
        asFilterString = asFilterString.Replace("{", string.Empty);
        asFilterString = asFilterString.Replace("}", string.Empty);
        asFilterString = asFilterString.Remove(asFilterString.Length - 1);
        return asFilterString;
    }

    /// <summary>
    /// This method is used to set control depending on criteria.
    /// </summary>
    /// <param name="flag"></param>
    private void SetControlsUponCriteria(bool flag)
    {
        if (flag)
        {
            cmbPrefix.ClearSelection();
            cmbOperation.ClearSelection();
            txtName.Focus();
        }
        optMain.Checked = flag;
        optExact.Checked = !flag;
        txtName.Text = "";
        txtReg.Text = "";
        txtName.Enabled = flag;
        txtReg.Enabled = !flag;
        cmbOperation.Enabled = !flag;
        cmbPrefix.Enabled = !flag;
        hidIsExactMatch.Value = (!flag).ToString();
        hidStudentReg.Value = txtReg.Text.Trim();
        hidStudentName.Value = txtName.Text.Trim();
    }

    /// <summary>
    /// This method is used to refresh student cache.
    /// </summary>
    private void RefreshStudentCache(bool abIsCompleteDelete)
    {
        int iStudentId = Convert.ToInt32(hidStudentId.Value);

        List<int> lstYearwiseStudentIds = new List<int>();
        AutoSearchService oAutoSearchService = new AutoSearchService();

        Constants.Action oAction = Constants.Action.Update;
        if (abIsCompleteDelete)
        {
            oAction = Constants.Action.Delete;
            lstYearwiseStudentIds.Add(iStudentId);
        }
        else
        {
            StudentBL oStudentBL = new StudentBL(miSchoolId, miAcademicYearId, iStudentId);
            lstYearwiseStudentIds.Add(oStudentBL.YearWiseStudentId);
        }

        oAutoSearchService.RefreshStudentCache(miSchoolId, miAcademicYearId, lstYearwiseStudentIds, oAction);
    }

    private void RefreshValue()
    {
        hidValSchoolLeavingDate.Value = Resources.LocalizedResources.ValSchoolLeavingDate;
        hidValSchoolLeavingDateForAdmission.Value = Resources.LocalizedResources.ValSchoolLeavingDateForAdmission;
        hidValSchoolLeavingFutureDate.Value = Resources.LocalizedResources.ValSchoolLeavingFutureDate;
        hidValSchoolLeavingDateBlank.Value = Resources.LocalizedResources.ValSchoolLeavingDateBlank;        
        hidValLeavingDateOutSide.Value = Resources.LocalizedResources.ValLeavingDateOutSide;
        hidValStudentLeaving.Value = Resources.LocalizedResources.ValStudentLeaving;
        hidStudentDelete.Value = Resources.LocalizedResources.StudentDelete;
        hidDeleteStudent.Value = Resources.LocalizedResources.DeleteStudent;
    }

    private void CheckMidYear()
    {
        StudentBL oStudentBL = new StudentBL();
        DataTable oDataTable = oStudentBL.RetriveMidYearInfo(miSchoolId, miAcademicYearId);
        hidIsMidYear.Value = Constants.S_ZERO;
        if (oDataTable.Rows.Count > 0)
            hidIsMidYear.Value = Convert.ToString(oDataTable.Rows[0]["Is_NewlyCreated"]);
    }

    #endregion
	
}