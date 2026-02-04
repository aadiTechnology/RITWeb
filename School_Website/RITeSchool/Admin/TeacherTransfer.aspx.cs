// File Name   : TeacherTransferUI.aspx.cs
// Created By  : -
// Date        : -
// Modified By : Milind
// Date        : 09 Oct 09
// Description : This class displays the UI for teacher transfer.
                /// 1. User selects source and target teachers. And clicks on Show.
                /// 2. The following details of the teachers are displayed:  
                ///     a. Class teacher assignments.
                ///     b. Subject teacher assignments.
                ///     c. Timetable of both the teachers
                /// 3. User can then manipulate the above details.
                /// 4. Clicking on save will make the effective changes.
                /// 5. User can view the previev of the changes he/she has made.

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using System.IO;
using BusinessLogic;
using System.Collections;
using Utility;
using System.Resources;

/// <summary>
/// </summary>
public partial class TeacherTransferUI : Timetable
{
    #region Constants

    const string S_SHOW = "Show";
    const string S_CHANGE_INPUTS = "Change Input";

    //datakeys constants
    const int I_DATAKEY_ROWSTATE = 1;
    const int I_DATAKEY_CANTRANSFER = 4;
    const int I_DATAKEY_TEACHERSUBJECTID = 3;
    //table indices
    private const int I_TBL_CLASSTEACHER = 0;
    private const int I_TBL_SRCSUBJECTTEACHER = 1;
    private const int I_TBL_TARGETSUBJECTTEACHER = 2;

    private const int I_TBL_TTWEEKDAY = 4;
    private const int I_TBL_TEACHERSTT = 3;
    private const int I_TBL_MAINTT = 5;
    private const int I_TBL_LECTURES = 6;

    private const int I_TBL_ASSEMBLY = 8;
    private const int I_TBL_STAYBACK = 9;

    const string S_NA = "N/A";

    //field names
    const string S_DB_COL_CANTRANSFER = "CanTransfer";
    private const string S_DB_COL_ROWSTATE = "RowState";
    const string S_FLD_CLASSNMAE = "StdDiv";

    #endregion

    #region Data Members

    DataSet moDSTeacherTransfer;
    TeacherTransferBL moTeacherTransferBL;
    bool mbPreconditionMet;
    private ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));

    #endregion

    #region Events

    /// <summary>
    /// This event is triggered every time the page is loaded.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                Initialize();
                FillTeacherComboes();
                SetSettings();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
            }
            else
            {
                if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
                {

                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    RefreshValues();
                }
                lblStatus.Text = String.Empty;
                bool bIsUseSubmitBehavior = true;
                if (bIsUseSubmitBehavior)
                {
                    //get the dataset from session
                    ExtractDsFromSession();
                    if (moDSTeacherTransfer != null)
                        ShowTransferTT();
                }
            }

            ApplyMouseHoverEffect(new List<Button> { btnBack, bnt_Back, btnPreVw, btnSave, btnShow });
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// event is triggered at the end of the page load stage. 
    /// At this point in the page life cycle, 
    /// all postback data and view-state data is loaded into controls on the page. 
    /// In the current page, every postback makes changes to dataset.
    /// This dataset is to be saved in session for further postbacks.
    /// Hence, insted of writing the same code in each eventhandler, this method is used.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLoadComplete(EventArgs e)
    {
        try
        {
            if (moDSTeacherTransfer != null)
            {
                moDSTeacherTransfer.AcceptChanges();
                Session[Constants.S_TEMP_SESSION_DS] = moDSTeacherTransfer;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to go back to the school configuration control panel
    /// </summary>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Teacher_Related)));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  The event is fired when user clicks on show button.
    /// if user has clicked to show the transfer result it calls the methods to display results.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (ToggleStatus())
            {
                int iSrcTeacherId = Convert.ToInt32(cmbSrcTeacher.SelectedValue);
                int iTargetTeacherId = Convert.ToInt32(cmbTargetTeacher.SelectedValue);
                moTeacherTransferBL = InitializeTeacherTransferBL(true);
                moDSTeacherTransfer = moTeacherTransferBL.GetTeacherTransfer(miSchoolId, miAcademicYearId, hidIsTTConfig.Value);

                //get the data in session
                Session.Add(Constants.S_TEMP_SESSION_DS, moDSTeacherTransfer);

                DataTable odtSrcTeacherSubject = moDSTeacherTransfer.Tables[I_TBL_SRCSUBJECTTEACHER];
                DataTable odtTimeTable = moDSTeacherTransfer.Tables[I_TBL_MAINTT];
                ManageAdditionalLecture(odtSrcTeacherSubject, odtTimeTable);

                ShowClassTeacherTransferDetails();
                ShowSubjectsTransferDetails();
                ShowTransferTT();
                ShowMsgs();
                ShowAssemblyMPT();
            }
            else
                Session.Remove(Constants.S_TEMP_SESSION_DS);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is triggered when user clicks on save button.
    /// It gets the XMLS for required datatables and calls the methods to make changes into database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            moTeacherTransferBL = InitializeTeacherTransferBL(true);
            moTeacherTransferBL.ValidateTT();            
            if (string.IsNullOrEmpty(moTeacherTransferBL.TransferTTMsg))
            {
                moTeacherTransferBL.SaveTransfer(miSchoolId, miAcademicYearId);
                Session.Remove(Constants.S_TEMP_SESSION_DS);
                ToggleStatus();
                cmbSrcTeacher.SelectedIndex = 0;
                cmbTargetTeacher.SelectedIndex = 0;
                lblStatus.Text = Resources.LocalizedResources.MsgTeacherTransferSuccess;
            }
            else
            {
                lblStatus.Text = moTeacherTransferBL.TransferTTMsg.Replace("%replace%",Resources.LocalizedResources.MsgMaxLecturesPerWeekLimitOf);
                lblStatus.Text = lblStatus.Text.Replace("%replace1%", Resources.LocalizedResources.MsgExceededFor);
                lblStatus.Text = lblStatus.Text.Replace("%replace2%", Resources.LocalizedResources.Msgby);
                lblStatus.Text = lblStatus.Text.Replace("%replace3%", Resources.LocalizedResources.Msglectures);
                ShowMsgs();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display the teacher transfer changes on the popup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrevw_Click(object sender, EventArgs e)
    {
        try
        {
            ShowClassTeacherMsgs();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// If the "rowstate " field  in datatable is Set to Updated the check box for source teacher is checked else checked.
    /// If the "CanTransferFlag" in datatable is Set the check box for source teacher is displayed else it is made invisible.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdTeacher_Rowdatabound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int iRowIndex = e.Row.RowIndex;
            if (iRowIndex >= Constants.I_ZERO)
            {
                string sCanTransfer = grdSrcTeacher.DataKeys[iRowIndex][I_DATAKEY_CANTRANSFER].ToString();
                CheckBox chkTeacherSubject = ((CheckBox)e.Row.Cells[1].FindControl("chkTeacherSubject"));
                if (grdSrcTeacher.DataKeys[iRowIndex][I_DATAKEY_ROWSTATE].ToString().Equals(Constants.S_UPDATED))
                {

                    chkTeacherSubject.Checked = true;
                    chkTeacherSubject.Visible = true;
                }
                else if (!sCanTransfer.Equals(Constants.C_YES.ToString()))
                {
                    chkTeacherSubject.Enabled = false;
                    hidIsDisable.Value = true.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to set check boxes of target teacher's subjects according to their status
    /// Either the subject is deleted(Checked) or not deleted (Uncheck)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdTargetTeacher_Rowdatabound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int iRowIndex = e.Row.RowIndex;
            if (iRowIndex >= Constants.I_ZERO)
            {
                string sCanTransfer = grdTargetTeacher.DataKeys[iRowIndex][I_DATAKEY_CANTRANSFER].ToString();
                CheckBox chkTeacherSubject = ((CheckBox)e.Row.Cells[1].FindControl("chkTeacherSubject"));
                string sRowState = grdTargetTeacher.DataKeys[iRowIndex][I_DATAKEY_ROWSTATE].ToString();
                if (sRowState.Equals(Constants.S_DELETED))
                    chkTeacherSubject.Checked = true;
                else
                    chkTeacherSubject.Checked = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change the status of the source teacher's subject on change of
    /// respective check box status (Checked/Unchecked)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkTeacherSubject_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            Control oChk = (Control)sender;
            //get the binding row 
            Control oCntrl = oChk.Parent.NamingContainer;
            GridViewRow oRow = (GridViewRow)oCntrl;
            int iRowIndex = oRow.RowIndex;
            //get the datakey corresponding to the row
            string sTeacherSubjectId = grdSrcTeacher.DataKeys[iRowIndex][I_DATAKEY_TEACHERSUBJECTID].ToString();

            if (CheckIsAdditionalLecture(Convert.ToInt32(sTeacherSubjectId), true, ((CheckBox)oChk).Checked))
            {
                // make appropiate change in class subject assignment
                PreapareClassSubjectTransfer(sTeacherSubjectId, ((CheckBox)oChk).Checked, true);
            }
            
            PrepareTTForSubjectTeacher(sTeacherSubjectId, true);
            
            ShowTransferTT();
            ShowClassTeacherMsgs();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to assing or remove assingment of Assembly to target teacher. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkTrgtAssembly_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable oDT = moDSTeacherTransfer.Tables[I_TBL_ASSEMBLY];
            CheckBox oChkTargetAssembly = (CheckBox)sender;
            if (oChkTargetAssembly.Checked)
                oDT.Rows[1]["Assembly_Applicable"] = Constants.C_YES;
            else
                oDT.Rows[1]["Assembly_Applicable"] = Constants.C_NO;
            oDT.AcceptChanges();
            ShowTransferTT();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to assing or remove assingment of Assembly to Source teacher. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkSrcAssembly_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable oDT = moDSTeacherTransfer.Tables[I_TBL_ASSEMBLY];
            CheckBox oChkTargetAssembly = (CheckBox)sender;
            if (oChkTargetAssembly.Checked)
                oDT.Rows[0]["Assembly_Applicable"] = Constants.C_YES;
            else
                oDT.Rows[0]["Assembly_Applicable"] = Constants.C_NO;
            oDT.AcceptChanges();
            ShowTransferTT();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to assing or remove assingment of MPT to Source teacher. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkSrcMPT_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable oDT = moDSTeacherTransfer.Tables[I_TBL_ASSEMBLY];
            CheckBox oChkTargetAssembly = (CheckBox)sender;
            if (oChkTargetAssembly.Checked)
                oDT.Rows[0]["MPT_Applicable"] = Constants.C_YES;
            else
                oDT.Rows[0]["MPT_Applicable"] = Constants.C_NO;
            oDT.AcceptChanges();
            ShowTransferTT();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to assing or remove assingment of MPT to target teacher. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkTrgtMPT_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable oDT = moDSTeacherTransfer.Tables[I_TBL_ASSEMBLY];
            CheckBox oChkTargetAssembly = (CheckBox)sender;
            if (oChkTargetAssembly.Checked)
                oDT.Rows[1]["MPT_Applicable"] = Constants.C_YES;
            else
                oDT.Rows[1]["MPT_Applicable"] = Constants.C_NO;
            oDT.AcceptChanges();
            ShowTransferTT();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to assing or remove assingment of Stay Back to Source teacher. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkSrcStayBack_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable oDT = moDSTeacherTransfer.Tables[I_TBL_ASSEMBLY];
            CheckBox oChkSrcStayback = (CheckBox)sender;
            oDT.Rows[0]["Stayback_Applicable"] = oChkSrcStayback.Checked;
            oDT.AcceptChanges();
            ShowTransferTT();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to assing or remove assingment of Stay Back to target teacher. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkTrgtStayBack_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable oDT = moDSTeacherTransfer.Tables[I_TBL_ASSEMBLY];
            CheckBox oChkTargetStayback = (CheckBox)sender;
            oDT.Rows[1]["Stayback_Applicable"] = oChkTargetStayback.Checked;
            oDT.AcceptChanges();
            ShowTransferTT();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is triggered when user clicks on the "Remove" check boxes on target subject teacher grid.
    /// It calls the functions to make corresponding changes in subject teacher assignments.
    /// If the checkbox is checked: The assignment and  the lectures corresponding to the assignment are removed.
    /// If the checkbox is unchecked: The assignment is kept as it is. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkTargetTeacherSubject_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            Control oChk = (Control)sender;
            Control oCntrl = oChk.Parent.NamingContainer;
            //get the container gridview row
            GridViewRow oRow = (GridViewRow)oCntrl;
            //get the index of the contaainer row to get correponding datakey
            int iRowIndex = oRow.RowIndex;
            string sTeacherSubjectId = grdTargetTeacher.DataKeys[iRowIndex][I_DATAKEY_TEACHERSUBJECTID].ToString();

            if (CheckIsAdditionalLecture(Convert.ToInt32(sTeacherSubjectId), false, ((CheckBox)oChk).Checked))
            {
                PreapareClassSubjectTransfer(sTeacherSubjectId, ((CheckBox)oChk).Checked, false);
                PrepareTTForSubjectTeacher(sTeacherSubjectId, false);
            }
            
            ShowTransferTT();
            ShowClassTeacherMsgs();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is triggered when user clicks on src teacher's class teacher assignment transfer  checkbox.
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkTransfer_CheckChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkTransfer.Checked)
                SrcTransfered(true);
            else
                SrcTransfered(false);
            PreapareClassTracherTransfer();
            ShowClassTeacherMsgs();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }
    /// <summary>
    /// This event is triggered when user clicks on the remove checkbox for class teacher assignment.
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkRemove_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //change class teacher datatable
            PreapareClassTracherTransfer();            
            ShowClassTeacherMsgs();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }
    /// <summary>
    /// This event is trigered when user clicks on the transfer checkbox(of src lecture) of timetable.
    /// It makes according changes in TT datatable.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void oChkTransfer_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox oChk = (CheckBox)sender;
            string sWeekdayName = oChk.ID.Substring(0, oChk.ID.Length - 2);
            string sLectureNo = oChk.ID.Substring(oChk.ID.Length - 2);
            moTeacherTransferBL = InitializeTeacherTransferBL(true);
            moTeacherTransferBL.PreapareTransferTTForLecture(sWeekdayName, sLectureNo, oChk.Checked);
            ShowTransferTT();
            ShowClassTeacherMsgs();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

	protected void bnt_Back_Click(object sender, EventArgs e) {
		try {
			MasterPage oMasterPage = (MasterPage)this.Master;
			oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Teacher_Related)));
		}
		catch(Exception ex) {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
    
    #endregion

    #region Helper methods

    /// <summary>
    /// This method initialise the display when the page is loaded for the first time.
    /// </summary>
    private void Initialize()
    {
        hidbtnShow.Value = S_SHOW;
        btnShow.Text = Resources.LocalizedResources.Show;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        setTTConfigStatus();
        VisibleHideData(false);
    }
    /// <summary>
    /// This method checks if the time table preconfigurations are done.
    /// and sets appropriate value to hidden field hidIsTTConfig.
    /// </summary>
    private void setTTConfigStatus()
    {
        string sMsg = ReferenceBL.CheckPrecondition(miSchoolId, miAcademicYearId, Convert.ToInt32(Constants.SchoolConfigurations.TeacherTimeTable));
        if (string.IsNullOrEmpty(sMsg))
            hidIsTTConfig.Value = true.ToString();
        else
            hidIsTTConfig.Value = false.ToString();
    }
    /// <summary>
    /// This method fills the combo boxes for source and target teachers. 
    /// 1. It retrives the all the teachers (in datatable). and  binds it to both the combo boxes.
    /// </summary>
    private void FillTeacherComboes()
    {
        DataSet oDsTeachers = SchoolWiseTeacherMasterCollectionBL.FetchAllTeachers(miSchoolId, miAcademicYearId);
        if (oDsTeachers != null)
        {
            ControlUtility.FillDropDownList(oDsTeachers.Tables[0], ref cmbSrcTeacher, "Teacher_Id", "TeacherName", Constants.S_SELECT);
            ControlUtility.FillDropDownList(oDsTeachers.Tables[1], ref cmbTargetTeacher, "Teacher_Id", "TeacherName", Constants.S_SELECT);
            cmbSrcTeacher.Items[0].Value = "-1";
        }
        
        CheckPrecondition();
        if(cmbSrcTeacher.Items.Count <= 1 || cmbSrcTeacher.Items.Count < 3)
		{
			if(!mbPreconditionMet) {
                divErr.InnerHtml = divErr.InnerHtml.Insert(divErr.InnerHtml.LastIndexOf("</table>"), "<tr><td><a class=\"ClsConfigLink\" href=\"TeacherSubjectAssignmentUI.aspx\">Teacher Class-Subject Assignment </a></td></tr>");
			}
			else {
                string sErrHTML = "<table class=\"LblNoRecord\" width=\"100%\"  cellpadding=\"0\" cellspacing=\"0\"><tr><td class=\"ClsConfigText\">" + Resources.LocalizedResources.PleaseConfigureFollowingDetailsForSchool + "</td></tr>";
				sErrHTML = sErrHTML + "<tr><td><a class=\"ClsConfigLink\" href=\"TeacherSubjectAssignmentUI.aspx\">Teachers</a></td></tr></table>";
				divErr.InnerHtml = sErrHTML;// "Teachers not available.";
			}
            trdivErr.Visible = true;
            trButtons.Visible = true;
            trTeachers.Visible = false;
            trTransfer.Visible = false;
            tblInputFields.Visible = false;
        }
    }
    
    private bool CheckPrecondition() {
		string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.TeacherTransfer);
		
		if(sLinks.Equals(string.Empty)) {
			mbPreconditionMet = true;
		}
		else {
			divErr.InnerHtml = sLinks;
			trdivErr.Visible = true;
			trButtons.Visible = true;
			trTeachers.Visible = false;
			trTransfer.Visible = false;
			tblInputFields.Visible = false;
		}
		
		return mbPreconditionMet;
    }    
    
    /// <summary>
    /// This method checks if the session dataset is set and assigns it to the member dataset.
    /// </summary>
    private void ExtractDsFromSession()
    {
        if (Session[Constants.S_TEMP_SESSION_DS] != null)
            moDSTeacherTransfer = (DataSet)Session[Constants.S_TEMP_SESSION_DS];
    }
    /// <summary>
    /// This method is called from click event handler of show button.
    /// it checks if user has clicked the button to show  grid or to change the inputs.
    /// It changes the caption of the button.
    /// And changes read only status of the registration no. text box.
    /// </summary>
    /// <returns>
    /// True: if user has clicked the button to show  grid
    /// False:if user has clicked to change the inputs.
    /// </returns>
    private bool ToggleStatus()
    {
        bool bReturn = true;
        if (btnShow.Text.Equals(Resources.LocalizedResources.Show))
        {
            EnableDisableComboes(false);
            hidbtnShow.Value = S_CHANGE_INPUTS;
            btnShow.Text =Resources.LocalizedResources.ChangeInput;
            VisibleHideData(true);
        }
        else
        {
            hidbtnShow.Value = S_SHOW;
            btnShow.Text = Resources.LocalizedResources.Show;
            grdSrcTeacher.DataSource = null;
            grdSrcTeacher.DataBind();
            grdTargetTeacher.DataSource = null;
            grdTargetTeacher.DataBind();
            EnableDisableComboes(true);
            VisibleHideData(false);
            bReturn = false;
        }

        return bReturn;
    }
    /// <summary>
    /// this method enales or disables the combo-boxes according to the parameter specified.
    /// </summary>
    /// <param name="abAction"></param>
    private void EnableDisableComboes(bool abAction)
    {
        cmbSrcTeacher.Enabled = abAction;
        cmbTargetTeacher.Enabled = abAction;
    }
    /// <summary>
    /// This method hides or displays the data part i.e the part other than input.
    /// </summary>
    /// <param name="abAction"></param>
    private void VisibleHideData(bool abAction)
    {
        trSubjectGrd.Visible = abAction;

        divTT.Visible = abAction;
        divSrcClassteacher.Visible = abAction;
        divTargetClassteacher.Visible = abAction;
        btnSave.Visible = abAction;
        btnPreVw.Visible = abAction;
        trClassHeader.Visible = abAction;
        trSubjectHeader.Visible = abAction;
        trMsgs.Visible = abAction;
        trTT.Visible = abAction;
        trAddNote.Visible = abAction;
        trLegendTable.Visible = abAction;
        imgArrow.Visible = abAction;
        imgArrow2.Visible = abAction;
        trDisabled.Visible = abAction;
        tdBrderLine.Visible = abAction;

        if (Settings.IsAssemblyApplicable)
        {
            tdSrcAssembly.Visible = abAction;
            tdTrgtAssembly.Visible = abAction;
        }
        else
        {
            tdSrcAssembly.Visible = false;
            tdTrgtAssembly.Visible = false;
        }

        if (Settings.IsMPTApplicable)
        {
            tdSrcMPT.Visible = abAction;
            tdTrgtMPT.Visible = abAction;
        }
        else
        {
            tdSrcMPT.Visible = false;
            tdTrgtMPT.Visible = false;
        }

        if (Settings.IsStaybackApplicable)
        {
            tdSrcStayBack.Visible = abAction;
            tdTrgtStayBack.Visible = abAction;
        }
        else
        {
            tdSrcStayBack.Visible = false;
            tdTrgtStayBack.Visible = false;
        }
    }

    #region Display
    /// <summary>
    /// This is a wrapper method to call the methods for src and target  class teacher transfer status display.
    /// </summary>
    private void ShowClassTeacherTransferDetails()
    {
        ShowClassSrcTeacherTransferDetails();
        ShowTargetClassTeacherTransferDetails();
    }
    /// <summary>
    /// This method displays class teacher transfer status of source teacher according to the datatable.
    /// </summary>
    private void ShowClassSrcTeacherTransferDetails()
    {
        DataTable oDtClassTeacher = moDSTeacherTransfer.Tables[I_TBL_CLASSTEACHER];
        string sCanTransfer = oDtClassTeacher.Rows[0][S_DB_COL_CANTRANSFER].ToString();
        //class can be transfered to target teacher.
		if (oDtClassTeacher.Rows.Count > 2)
		{			
			for (int iClassCount = 0; iClassCount < oDtClassTeacher.Rows.Count - 1; iClassCount++)
				if (oDtClassTeacher.Rows[iClassCount]["RowState"].ToString() != "Deleted")
				lblSrc.Text +=" "+ oDtClassTeacher.Rows[iClassCount][S_FLD_CLASSNMAE].ToString() +", " ;

			lblSrc.Text = lblSrc.Text.TrimEnd(' ').TrimEnd(',');
		}
		else
			lblSrc.Text = oDtClassTeacher.Rows[0][S_FLD_CLASSNMAE].ToString();
        string sRowstate;
        if (sCanTransfer.Equals(Constants.C_YES.ToString()))
        {
            chkTransfer.Visible = true;
            chkTransfer.Enabled = true;
            sRowstate = oDtClassTeacher.Rows[0][S_DB_COL_ROWSTATE].ToString();
            //display class transfer
            if (sRowstate.Equals(Constants.S_UPDATED))
            {
                chkTransfer.Checked = true;
                SrcTransfered(true);
            }
            //class not to be transfered
            //enable target teacher checkbox
            else
            {
                chkTransfer.Checked = false;
                SrcTransfered(false);
            }
        }
        else
        {
            chkTransfer.Enabled = false;
            chkTransfer.Checked = false;
            chkRemove.Enabled = true;
        }
    }
    /// <summary>
    /// This method displays class teacher transfer status of target teacher according to the datatable.
    /// </summary>
    private void ShowTargetClassTeacherTransferDetails()
    {
        string sRowstate;

        DataTable oDtClassTeacher = moDSTeacherTransfer.Tables[I_TBL_CLASSTEACHER];
        //check for target teacher
		if (oDtClassTeacher.Rows.Count > 2)
		{
			for (int iClassCount = 0; iClassCount < oDtClassTeacher.Rows.Count; iClassCount++)
				if (oDtClassTeacher.Rows[iClassCount]["RowState"].ToString() == "Deleted")
					lblTarget.Text += " " + oDtClassTeacher.Rows[iClassCount][S_FLD_CLASSNMAE].ToString() + ", ";

			lblTarget.Text = lblTarget.Text == string.Empty ? Resources.LocalizedResources.NA : lblTarget.Text.TrimEnd(' ').TrimEnd(',');
		}
		else
			lblTarget.Text = oDtClassTeacher.Rows[oDtClassTeacher.Rows.Count - 1][S_FLD_CLASSNMAE].ToString();

		sRowstate = oDtClassTeacher.Rows[oDtClassTeacher.Rows.Count -1][S_DB_COL_ROWSTATE].ToString();
        string sStdDivId = oDtClassTeacher.Rows[1]["StdDivTeacher_Id"].ToString();
        if (!sStdDivId.Equals("0"))
        {
            if (sRowstate.Equals(Constants.S_DELETED))
                chkRemove.Checked = true;
            else
                chkRemove.Checked = false;
        }
        else
        {
            chkRemove.Checked = false;
            chkRemove.Enabled = false;
        }
    }
    /// <summary>
    /// This method calls the base class method to display timetable.
    /// The base class method structures Timetable format into a Panel which then is added to the container panel in the class.
    /// Note: This method is called on every postback and identical controls get added every time. To avoid this,
    /// The container panel is cleared before adding the Timetable panel(from base class) coz
    /// </summary>
    private void ShowTransferTT()
    {
        if (moDSTeacherTransfer.Tables.Count > I_TBL_TTWEEKDAY)
        {
            //set base class members
            Timetable oTT = new Timetable(moDSTeacherTransfer.Tables[I_TBL_TEACHERSTT], moDSTeacherTransfer.Tables[I_TBL_MAINTT], moDSTeacherTransfer.Tables[I_TBL_TTWEEKDAY], moDSTeacherTransfer.Tables[I_TBL_LECTURES], moDSTeacherTransfer.Tables[I_TBL_ASSEMBLY], moDSTeacherTransfer.Tables[I_TBL_STAYBACK]);
            //call the method to display timetable
            oTT.DisplayTT();
            AddCheckboxes(oTT);
            //clear pnlContainer
            pnlContainer.Controls.Clear();
            //Add the TT panel into container panel.
            pnlContainer.Controls.Add(oTT.moPnl);
        }
    }
    /// <summary>
    /// This method displays the checkboxes to transfer the lectures.
    /// The checkboxes are added to src teacher row if the lecture can be transferred.
    /// The style of the corresponding target lecture is set accordingly.
    /// Implemetation Logic: This method loops through teachers(only 2 in this case src and target) and lectures.
    /// The detaile logic is explained in the comments inside the method .
    /// </summary>
    /// <param name="aoDtClassTeacher"></param>
    /// <param name="aoDtSubjectTeacher"></param>
    private void AddCheckboxes(Timetable oTransferTT)
    {
        // This flag is used to for MPT,Assembly and Stay back Change for deleted subject
        // If bFlag is false then show css class delete subject other wise
        // show that subject as it is with normal css class.
        bool bFlag = false;
        HtmlTable oTbl = oTransferTT.tblTT;
        int iWeekDayCnt = moDSTeacherTransfer.Tables[I_TBL_TTWEEKDAY].Rows.Count;
        const string S_TARGET_UPDATECSS = "SubUpdate";
        const string S_TARGET_UPDATEDELCSS = "SubUpdateDel";
        const string S_TARGET_DELETED = "SubDeleted";
        DataTable oDtClassTeacher = moDSTeacherTransfer.Tables[I_TBL_CLASSTEACHER];
        DataTable oDtSubjectTeacher = moDSTeacherTransfer.Tables[I_TBL_SRCSUBJECTTEACHER];
        int iTeacherId;
        const string S_FLD_SUBJECTTEACHERID = "Teacher_Subject_Id";
        const string S_FLD_TEACHERID = "Teacher_Id";
        DataRow oDrClassTeacher;
        DataColumn[] oDtCols = new DataColumn[1];
        oDtCols[0] = oDtSubjectTeacher.Columns[S_FLD_SUBJECTTEACHERID];
        oDtSubjectTeacher.PrimaryKey = oDtCols;
        int iSrcTeacherId = Convert.ToInt32(oDtClassTeacher.Rows[0][S_FLD_TEACHERID]);
        int iTargetTeacherId = Convert.ToInt32(oDtClassTeacher.Rows[1][S_FLD_TEACHERID]);
        //loop through teachers.
        // As header row is made up of 2 rows, 1st 2 rows are skipped.  and loop starts with 2.
        for (int iTeacherIndex = 2; iTeacherIndex < 4; iTeacherIndex++)
        {
            if (iTeacherIndex == 2)
            {
                iTeacherId = iSrcTeacherId;
                oDrClassTeacher = oDtClassTeacher.Rows[0];
            }
            else
            {
                iTeacherId = iTargetTeacherId;
                oDrClassTeacher = oDtClassTeacher.Rows[1];
            }
            int iCellIndex = 0;
            //loop through weekdays 
            for (int iWeekDay = 0; iWeekDay < iWeekDayCnt; iWeekDay++)
            {
                int iLectureCnt = Convert.ToInt32(moDSTeacherTransfer.Tables[I_TBL_TTWEEKDAY].Rows[iWeekDay]["LecturesCnt"]);
                string sWeekDayId = moDSTeacherTransfer.Tables[I_TBL_TTWEEKDAY].Rows[iWeekDay]["Weekdays_Id"].ToString();
                string sWeekDayName = moDSTeacherTransfer.Tables[I_TBL_TTWEEKDAY].Rows[iWeekDay]["Weekday_Name"].ToString();
                string sStayback = Settings.StaybackName;
                string sAssembly = Settings.AssemblyName;
                string sMPT = Settings.MPTName;

                //loop through lectures of current weekdays.
                for (int i = 1; i <= iLectureCnt; i++)
                {
                    HtmlTableCell oCell = oTbl.Rows[iTeacherIndex].Cells[iCellIndex];
                    
                    //check if the lecture exists
                    DataRow[] oDtRows = moDSTeacherTransfer.Tables[I_TBL_MAINTT].Select("Teacher_Id= " + iTeacherId.ToString() + " AND Weekday_Id=" + sWeekDayId + " AND Lecture_Number=" + i.ToString());
                    string sRowState = Constants.S_ORIGINAL;
                    if (oDtRows.Length > 0)//if yes
                    {
                        for (int iCount = 0; iCount < oDtRows.Length; iCount++)
                        {
                            if (!oDtRows[iCount][S_DB_COL_ROWSTATE].ToString().Equals(Constants.S_ORIGINAL))
                                sRowState = oDtRows[iCount][S_DB_COL_ROWSTATE].ToString();
                        }
                    }

                    if (iTeacherIndex == 2)//if src teacher
                    {
                        CheckBox oChkTransfer = new CheckBox();

                        if (i.ToString().Length == 1)
                            oChkTransfer.ID = sWeekDayName + "0" + i.ToString();
                        else
                            oChkTransfer.ID = sWeekDayName + i.ToString();

                        oChkTransfer.AutoPostBack = true;
                        oChkTransfer.CausesValidation = false;
                        oChkTransfer.TabIndex = 14;
                        oChkTransfer.CheckedChanged += new EventHandler(oChkTransfer_CheckedChanged);

                        if (!oCell.InnerHtml.Equals("<b>" + sStayback + "</b>") && !oCell.InnerHtml.Equals("<b>" + sAssembly + "</b>") && !oCell.InnerHtml.Equals("<b>" + sMPT + "</b>"))
                        {
                            //if the lecture is not off
                            if (oDtRows.Length > 0)
                            {
                                string sSrcLectureDesc = oTbl.Rows[iTeacherIndex].Cells[iCellIndex].InnerHtml;
                                string sTargetLectureDesc = oTbl.Rows[iTeacherIndex + 1].Cells[iCellIndex].InnerHtml;
                                
                                //if lecture cannot be transfered 
                                //the chkbox is not displayed
                                if (sRowState.Equals(Constants.S_ORIGINAL))
                                    oChkTransfer.Visible = false;                                
                                else
                                {
                                    //lecture transferred
                                    if (sRowState.Equals(Constants.S_UPDATED))
                                    {
                                        string sTransferStyle = "";
                                        string sEndTransferStyle = "";

                                        if (!sTargetLectureDesc.Equals("<b>" + sStayback + "</b>") && !sTargetLectureDesc.Equals("<b>" + sAssembly + "</b>") && !sTargetLectureDesc.Equals("<b>" + sMPT + "</b>"))
                                        {
                                            sTransferStyle = "<font color=\"blue\" style=\"text-decoration:line-through\">";
                                            sEndTransferStyle = "</font>";
                                            oChkTransfer.Checked = true;
                                            oTbl.Rows[iTeacherIndex + 1].Cells[iCellIndex].InnerHtml = sTransferStyle + sTargetLectureDesc + sEndTransferStyle + "<BR>" + sSrcLectureDesc;
                                            oTbl.Rows[iTeacherIndex + 1].Cells[iCellIndex].Attributes.Remove("class");
                                            oTbl.Rows[iTeacherIndex + 1].Cells[iCellIndex].Attributes.Add("class", S_TARGET_UPDATECSS);
                                        }
                                        else
                                        {
                                            bFlag = true;
                                            oTbl.Rows[iTeacherIndex + 1].Cells[iCellIndex].InnerHtml = sTargetLectureDesc;
                                            oChkTransfer.Checked = false;
                                            oChkTransfer.Visible = false;
                                            string sWeekdayName = oChkTransfer.ID.Substring(0, oChkTransfer.ID.Length - 2);
                                            string sLectureNo = oChkTransfer.ID.Substring(oChkTransfer.ID.Length - 2);
                                            moTeacherTransferBL = InitializeTeacherTransferBL(true);
                                            moTeacherTransferBL.PreapareTransferTTForLecture(sWeekdayName, sLectureNo, oChkTransfer.Checked);
                                        }
                                    }
                                    else if (!sTargetLectureDesc.Equals("<b>" + sStayback + "</b>") && !sTargetLectureDesc.Equals("<b>" + sAssembly + "</b>") && !sTargetLectureDesc.Equals("<b>" + sMPT + "</b>"))
                                        oChkTransfer.Checked = false;
                                    else
                                    {
                                        bFlag = true;
                                        oChkTransfer.Visible = false;
                                    }
                                }
                                oCell.Controls.Add(oChkTransfer);
                            }
                        }
                        else
                        {
                            bFlag = true;
                            string sTargetLectureDesc = oTbl.Rows[iTeacherIndex + 1].Cells[iCellIndex].InnerHtml;
                            if (!sTargetLectureDesc.Equals("<b>" + sStayback + "</b>") && !sTargetLectureDesc.Equals("<b>" + sAssembly + "</b>") && !sTargetLectureDesc.Equals("<b>" + sMPT + "</b>"))
                            {
                                if (sRowState.Equals(Constants.S_ORIGINAL))
                                    oChkTransfer.Visible = false;
                                else
                                {
                                    DataRow[] oDtRows1 = moDSTeacherTransfer.Tables[I_TBL_MAINTT].Select("Teacher_Id= " + iTargetTeacherId.ToString() + " AND Weekday_Id=" + sWeekDayId + " AND Lecture_Number=" + i.ToString());
                                    string sRowState1 = Constants.S_ORIGINAL;
                                    if (oDtRows.Length > 0)//if yes
                                    {
                                        for (int iCount = 0; iCount < oDtRows1.Length; iCount++)
                                        {
                                            if (!oDtRows1[iCount][S_DB_COL_ROWSTATE].ToString().Equals(Constants.S_ORIGINAL))
                                                sRowState1 = oDtRows1[iCount][S_DB_COL_ROWSTATE].ToString();
                                        }
                                    }

                                    if (sRowState1.Equals(Constants.S_DELETED))
                                    {
                                        oTbl.Rows[iTeacherIndex + 1].Cells[iCellIndex].Attributes.Remove("class");
                                        oTbl.Rows[iTeacherIndex + 1].Cells[iCellIndex].Attributes.Add("class", S_TARGET_DELETED);
                                    }
                                    else
                                    {
                                        oChkTransfer.Checked = false;
                                        oChkTransfer.Visible = false;
                                        string sWeekdayName = oChkTransfer.ID.Substring(0, oChkTransfer.ID.Length - 2);
                                        string sLectureNo = oChkTransfer.ID.Substring(oChkTransfer.ID.Length - 2);
                                        moTeacherTransferBL = InitializeTeacherTransferBL(true);
                                        moTeacherTransferBL.PreapareTransferTTForLecture(sWeekdayName, sLectureNo, oChkTransfer.Checked);
                                    }
                                }
                            }
                        }
                    }
                    //for target teacher lectures
                    else
                    {
                        string sTargetLectureDesc = oTbl.Rows[iTeacherIndex].Cells[iCellIndex].InnerHtml;
                        //overwritten
                        if (!sTargetLectureDesc.Equals("<b>" + sStayback + "</b>") && !sTargetLectureDesc.Equals("<b>" + sAssembly + "</b>") && !sTargetLectureDesc.Equals("<b>" + sMPT + "</b>"))
                        {
                            if (sRowState.Equals(Constants.S_UPDATED))
                            {
                                oCell.Attributes.Remove("class");
                                oCell.Attributes.Add("class", S_TARGET_UPDATECSS);
                            }
                            //overwritten and delete
                            if (sRowState.Equals(Constants.S_UPDATEDEL))
                            {
                                oCell.Attributes.Remove("class");
                                oCell.Attributes.Add("class", S_TARGET_UPDATEDELCSS);
                            }
                        }
                        else
                            bFlag = true;
                    }
                    //if lecture is deleted 
                    if (sRowState.Equals(Constants.S_DELETED) && (!bFlag))
                    {
                        oCell.Attributes.Remove("class");
                        oCell.Attributes.Add("class", S_TARGET_DELETED);
                    }
                    else
                        bFlag = false;
                    iCellIndex = iCellIndex + 1;
                }
            }
        }
    }
    /// <summary>
    /// 
    /// This method displays the subject teacher assignments and the checkboxes (according to suitable conditions).
    /// </summary>
    private void ShowSubjectsTransferDetails()
    {
        DataView oDtVw = moDSTeacherTransfer.Tables[I_TBL_SRCSUBJECTTEACHER].DefaultView;
        grdSrcTeacher.DataSource = oDtVw;
        hidIsDisable.Value = false.ToString();
        grdSrcTeacher.DataBind();
        oDtVw = moDSTeacherTransfer.Tables[I_TBL_TARGETSUBJECTTEACHER].DefaultView;
        grdTargetTeacher.DataSource = oDtVw;
        grdTargetTeacher.DataBind();
    }
    /// <summary>
    /// This method changes the status of target teacher's class teacher assignment checkbox.
    /// </summary>
    /// <param name="abAction">true if targe checkboxshould be checked.</param>
    private void SrcTransfered(bool abAction)
    {
        if (lblTarget.Text.Equals(Resources.LocalizedResources.NA))
        {
            chkRemove.Checked = false;
            chkRemove.Enabled = false;
        }
        else
        {
            chkRemove.Checked = abAction;
            chkRemove.Enabled = !abAction;
        }
    }
    #endregion

    #region dataset manipulation

    /// <summary>
    ///This method will manipulate class teacher data according to changes in class teacher assignments(checkboxes). 
    ///The Rowstate field of the corresponding datatable is modified.
    /// </summary>
    private void PreapareClassTracherTransfer()
    {
        bool bIsTransfer, bIsRemove;
        //check source
        if (chkTransfer.Checked) //transfered
            bIsTransfer = true;
        else
            bIsTransfer = false;
        //check target
        if (chkRemove.Checked) //removed
            bIsRemove = true;
        else
            bIsRemove = false;

        moTeacherTransferBL = InitializeTeacherTransferBL(true);
        moTeacherTransferBL.PreapareClassTracherTransfer(bIsTransfer, bIsRemove);
        ShowClassTeacherMsgs();
    }

    /// <summary>
    /// This method calls the BL method to modify the data of teacher subject.
    /// </summary>
    /// <param name="asTeacherSubectId"></param>
    /// <param name="bIsChecked"></param>
    /// <param name="bIsSrc"></param>
    private void PreapareClassSubjectTransfer(string asTeacherSubectId, bool bIsChecked, bool abIsSrc)
    {
        moTeacherTransferBL = InitializeTeacherTransferBL(abIsSrc);
        moTeacherTransferBL.PreapareClassSubjectTransfer(asTeacherSubectId, bIsChecked, abIsSrc);
        ShowClassTeacherMsgs();
    }

    /// <summary>
    /// This is a wrapper method to call the BL methods to change the timetable according
    /// to class teacher assignment changes.
    /// </summary>
    /// <param name="IsTransfer">
    /// True: if class teacher assignment is to be transferred from src to target. 
    /// False: if target class teacher assignment is to be removed.
    /// </param>
    /// <returns></returns>
    private int PrepareTTForClassTeacher(bool IsSrc)
    {
        moTeacherTransferBL = InitializeTeacherTransferBL(true);
        int iChangedCnt = 0;
        if (IsSrc)
            iChangedCnt = moTeacherTransferBL.PreapareTransferTTForClassTeacher(cmbSrcTeacher.SelectedValue, IsSrc);
        else
            iChangedCnt = moTeacherTransferBL.PreapareTransferTTForClassTeacher(cmbTargetTeacher.SelectedValue, IsSrc);

        return iChangedCnt;
    }

    /// <summary>
    /// This method calls BL method to change timetable according to changes in subject teacher assignments. 
    /// </summary>
    /// <param name="asSubjectTeacherId"></param>
    /// <param name="abIsSrc"></param>
    private void PrepareTTForSubjectTeacher(string asSubjectTeacherId, bool abIsSrc)
    {
        moTeacherTransferBL = InitializeTeacherTransferBL(abIsSrc);
        moTeacherTransferBL.PreapareTransferTTForSubjects(asSubjectTeacherId, abIsSrc);
        moDSTeacherTransfer = moTeacherTransferBL.TeacherTransferDS;
    }
    /// <summary>
    /// This method creates,  initialises  and returns a TransferBL object. 
    /// </summary>
    /// <returns></returns>
    private TeacherTransferBL InitializeTeacherTransferBL(bool abIsSrc)
    {
        int iSrcTeacherId = Convert.ToInt32(cmbSrcTeacher.SelectedValue);
        int iTargetTeacherId = Convert.ToInt32(cmbTargetTeacher.SelectedValue);
        moTeacherTransferBL = new TeacherTransferBL(iSrcTeacherId, iTargetTeacherId, moDSTeacherTransfer);
        return moTeacherTransferBL;
    }
    /// <summary>
    /// 
    /// </summary>
    private void ShowMsgs()
    {
        ShowClassTeacherMsgs();
        ShowSubjectMsges();
    }

    /// <summary>
    /// This method is used to show the MPT,Assembly and Stay Back assignment.
    /// </summary>
    private void ShowAssemblyMPT()
    {
        if (Settings.IsMPTApplicable)
        {
            DataRow oDRSrc = moDSTeacherTransfer.Tables[I_TBL_ASSEMBLY].Rows[0];
            DataRow oDRTarget = moDSTeacherTransfer.Tables[I_TBL_ASSEMBLY].Rows[1];

            if (oDRSrc["MPT_Applicable"].ToString() == Constants.C_YES.ToString())
                chkSrcMPT.Checked = true;
            else
                chkSrcMPT.Checked = false;

            if (oDRTarget["MPT_Applicable"].ToString() == Constants.C_YES.ToString())
                chkTrgtMPT.Checked = true;
            else
                chkTrgtMPT.Checked = false;
        }

        if (Settings.IsAssemblyApplicable)
        {
            DataRow oDRSrc = moDSTeacherTransfer.Tables[I_TBL_ASSEMBLY].Rows[0];
            DataRow oDRTarget = moDSTeacherTransfer.Tables[I_TBL_ASSEMBLY].Rows[1];

            if (oDRSrc["Assembly_Applicable"].ToString() == Constants.C_YES.ToString())
                chkSrcAssembly.Checked = true;
            else
                chkSrcAssembly.Checked = false;

            if (oDRTarget["Assembly_Applicable"].ToString() == Constants.C_YES.ToString())
                chkTrgtAssembly.Checked = true;
            else
                chkTrgtAssembly.Checked = false;
        }

        if (Settings.IsStaybackApplicable)
        {
            DataRow oDRSrc = moDSTeacherTransfer.Tables[I_TBL_ASSEMBLY].Rows[0];
            DataRow oDRTarget = moDSTeacherTransfer.Tables[I_TBL_ASSEMBLY].Rows[1];

            if (Convert.ToBoolean(oDRSrc["Stayback_Applicable"]))
                chkSrcStayBack.Checked = true;
            else
                chkSrcStayBack.Checked = false;

            if (Convert.ToBoolean(oDRTarget["Stayback_Applicable"]))
                chkTrgtStayBack.Checked = true;
            else
                chkTrgtStayBack.Checked = false;
        }
    }

    /// <summary>
    /// This method displays the messages.
    /// </summary>
    /// <param name="aoTransferObj"></param>
    private void ShowClassTeacherMsgs()
    {
        TeacherTransferBL oTransferObj = InitializeTeacherTransferBL(true);
        oTransferObj.PrepareMsgsForTransfer();
        lblMsg.Text = oTransferObj.SrcClassTeacherTransferMsg;
        lblTargetMsg.Text = oTransferObj.TargetClassTeacherTransferMsg;
        if (string.IsNullOrEmpty(oTransferObj.TransferTTMsg))
            divTTmsg.Visible = false;
        else
        {
            divTTmsg.Visible = true;
            lblTTStatus.Text = oTransferObj.TransferTTMsg;
        }
    }
    /// <summary>
    /// This method displays the messages for the subjects.
    /// </summary>
    private void ShowSubjectMsges()
    {
        if (hidIsDisable.Value.Equals(true.ToString()))
        {
            trDisabled.Visible = true;
            lblDisabledMsg.Text = Resources.LocalizedResources.MsgTransferTeacher3+" " + cmbTargetTeacher.SelectedItem.Text + ".";
        }
        else
            trDisabled.Visible = false;

        lblSubjectMsg.Text = Resources.LocalizedResources.MsgTransferTeacher + " " + cmbTargetTeacher.SelectedItem.Text;
        if (grdTargetTeacher.Rows.Count > 0)
            lblTargetSubjectMsg.Text = Resources.LocalizedResources.MsgTransferTeacher1 + " " + cmbTargetTeacher.SelectedItem.Text;
        else
            lblTargetSubjectMsg.Text = Resources.LocalizedResources.MsgTransferTeacher2+" " + cmbTargetTeacher.SelectedItem.Text;
    }

    ///  <summary>
    ///  This method used to manage addtional lectures.
    ///  While transferring additional or base lecture if standard/subject of
    ///  any one of lecture is not associated with target teacher then both lecture can not be transferred. 
    ///  Datatable odtSrcTeacherSubject contains data about subject and standard applicale to both teacher
    ///  Datatable odtTimeTable contains data about weekly time table of both the teacher.
    ///  </summary>
    private void ManageAdditionalLecture(DataTable odtSrcTeacherSubject, DataTable odtTimeTable)
    {
        bool bIsUpdated = false;
        //Get subject which are going to transfer
        DataRow[] oRow = odtSrcTeacherSubject.Select(S_DB_COL_ROWSTATE + "= '" + Constants.S_UPDATED + "'");

        for (int iCount = 0; iCount < oRow.Length; iCount++)
        {
            //Get lecture of the source teacher for selected standard.
            DataRow[] oRowArrLect = odtTimeTable.Select("Teacher_Subject_Id = " + oRow[iCount]["Teacher_Subject_Id"].ToString());

            if (oRowArrLect.Length > 0)
            {
                for (int i = 0; i < oRowArrLect.Length; i++)
                {
                    //get lecture for selectd weekday  
                    DataRow[] oRowArrLectForDay = odtTimeTable.Select("Teacher_Id = " 
                                            + oRowArrLect[i]["Teacher_Id"].ToString() + " AND Lecture_Number = " 
                                            + oRowArrLect[i]["Lecture_Number"].ToString() + " AND Weekday_Name = '" 
                                            + oRowArrLect[i]["Weekday_Name"].ToString() + "'");

                    //Check if there is a additional lecture with seleted weekday then check that 
                    //that additional lecture's subject as well as standard is also associated with target teacher
                    //If yes then transfer both lecture other wise do not transfer either lecture. 
                    if (oRowArrLectForDay.Length > 1)
                    {
                        for (int iAdd = 0; iAdd < oRowArrLectForDay.Length; iAdd++)
                        {
                            DataRow[] row = odtSrcTeacherSubject.Select("Teacher_Subject_Id = " + oRowArrLectForDay[iAdd]["Teacher_Subject_Id"].ToString());
                            //If S_DB_COL_ROWSTATE is equal to updated/transferrable then selected subject 
                            //and standard is applicable to the target teacher other wise either standard 
                            //or subject or both are not applicable to target teacher.
                            if (row[0][S_DB_COL_ROWSTATE].ToString() != Constants.S_UPDATED)
                                bIsUpdated = true;
                        }

                        //bIsUpdated is true when from source teacher's additional lectures either standard or 
                        //subject or both are not applicable to the target teacher. 
                        if (bIsUpdated)
                        {
                            //From the additional lecture if subject or standard or both are not applicable to
                            //target teacher then do not transfer either lecture to target teacher
                            for (int iAdd = 0; iAdd < oRowArrLectForDay.Length; iAdd++)
                            {
                                //This is used for not transferring the subject to the target teacher
                                //For this set the value of S_DB_COL_ROWSTATE for that subject to Original.
                                //and CanTransfer to N
                                DataRow[] oRowTargetTeacherSubStds = odtSrcTeacherSubject.Select("Teacher_Subject_Id = " + oRowArrLectForDay[iAdd]["Teacher_Subject_Id"].ToString());
                                oRowTargetTeacherSubStds[0][S_DB_COL_ROWSTATE] = Constants.S_ORIGINAL;
                                oRowTargetTeacherSubStds[0]["CanTransfer"] = "N";

                                //This is used for cancelling the transfer of source teacher's that subject lectures
                                //from the weekly time table.
                                //For this set the value of S_DB_COL_ROWSTATE for that subject to Original.
                                //and CanTransfer to N
                                DataRow[] oRowTrgAddtional = odtTimeTable.Select("Teacher_Subject_Id = " + oRowTargetTeacherSubStds[0]["Teacher_Subject_Id"].ToString());
                                for (int iAddTrg = 0; iAddTrg < oRowTrgAddtional.Length; iAddTrg++)
                                {
                                    oRowTrgAddtional[iAddTrg][S_DB_COL_ROWSTATE] = Constants.S_ORIGINAL;
                                    oRowTrgAddtional[iAddTrg]["CanTransfer"] = "N";

                                    DataRow[] oAddTrg = odtTimeTable.Select("Lecture_Number = " + oRowTrgAddtional[iAddTrg]["Lecture_Number"].ToString() + " AND Weekday_Name = '" + oRowTrgAddtional[iAddTrg]["Weekday_Name"].ToString() + "'");
                                    for (int iTrg = 0; iTrg < oAddTrg.Length; iTrg++)
                                    {
                                        oAddTrg[iTrg][S_DB_COL_ROWSTATE] = Constants.S_ORIGINAL;
                                        oAddTrg[iTrg]["CanTransfer"] = "N";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        odtSrcTeacherSubject.AcceptChanges();
        odtTimeTable.AcceptChanges();
    }

    ///  <summary>
    ///  This method used to check if there is any addtional lecture associated with selected lecture.
    ///  In case of additional lecture both(either base or additional)
    ///  lectures be transfer/remove or vice versa.
    ///  </summary>
    private bool CheckIsAdditionalLecture(int aiTeacherSubjectId, bool abIsSrc, bool IsChecked)
    {
        DataRow[] oRowAddWeekDay;
        DataTable oDtAddWeekDay = moDSTeacherTransfer.Tables[5];
        DataRow[] oRowSrcTrg;
        List<int> oListTeacherSubjectId = new List<int>();

        oListTeacherSubjectId.Add(aiTeacherSubjectId);
        //To get selected subject's lectures from the weekly time table. 
        oRowAddWeekDay = oDtAddWeekDay.Select("Teacher_Subject_Id = " + aiTeacherSubjectId.ToString());

        //Check that there is atleast one lecture in the time table for selected subject 
        //for that selected teacher (Either target or source).
        if (oRowAddWeekDay.Length > 0)
        {
            //Loop through  the lecture
            for (int iCount = 0; iCount < oRowAddWeekDay.Length; iCount++)
            {
                //To get additional lecture for selected subject if any.
                oRowSrcTrg = oDtAddWeekDay.Select("Teacher_Id = " + oRowAddWeekDay[iCount]["Teacher_Id"].ToString() + 
                                                 " AND Lecture_Number = " + oRowAddWeekDay[iCount]["Lecture_Number"].ToString()
                                                 + " AND Weekday_Name = '" + oRowAddWeekDay[iCount]["Weekday_Name"].ToString()+ "'");

                //To check that there is any additional lecture for selected subject.
                //if yes then add that subject's Teacher_Subject_Id(Teacher subject association)
                //to oListTeacherSubjectId
                if (oRowSrcTrg.Length > 0)
                {
                    for (int i = 0; i < oRowSrcTrg.Length; i++)
                    {
                        int iTeacherSubId = Convert.ToInt32(oRowSrcTrg[i]["Teacher_Subject_Id"]);
                        if (iTeacherSubId != aiTeacherSubjectId)
                        {
                            if (!oListTeacherSubjectId.Contains(iTeacherSubId))
                                oListTeacherSubjectId.Add(iTeacherSubId);
                        }
                    }
                }
            }
        }

        //If oListTeacherSubjectId contains only one item that means it contain only
        //Teacher_Subject_Id for the selected subject only.
        if (oListTeacherSubjectId.Count == 1)
            return true;
        //If there is more that one items in the oListTeacherSubjectId then it means there is
        //atleast one addiional lecture associated with selected subject. 
        else
        {
            //For source teacher.
            if (abIsSrc)
            {
                //To make additional lecture's check box status according to the 
                //status of check box of the selected subject.
                for (int iListItm = 0; iListItm < oListTeacherSubjectId.Count; iListItm++)
                {
                    for (int i = 0; i < grdSrcTeacher.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(grdSrcTeacher.DataKeys[i][I_DATAKEY_TEACHERSUBJECTID]) == Convert.ToInt32(oListTeacherSubjectId[iListItm]))
                        {
                            CheckBox oChk = grdSrcTeacher.Rows[i].FindControl("chkTeacherSubject") as CheckBox;
                            oChk.Checked = IsChecked;
                            break;
                        }
                    }
                    // make appropiate change in class subject assignment for the source teacher.
                    PreapareClassSubjectTransfer(oListTeacherSubjectId[iListItm].ToString(), IsChecked, true);
                }
            }
            //For target teacher.
            else
            {
                //To make additional lecture's check box status according to the 
                //status of check box of the selected subject.
                for (int iListItm = 0; iListItm < oListTeacherSubjectId.Count; iListItm++)
                {
                    for (int i = 0; i < grdTargetTeacher.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(grdTargetTeacher.DataKeys[i][I_DATAKEY_TEACHERSUBJECTID]) == Convert.ToInt32(oListTeacherSubjectId[iListItm]))
                        {
                            CheckBox oChk = grdTargetTeacher.Rows[i].FindControl("chkTeacherSubject") as CheckBox;
                            oChk.Checked = IsChecked;
                            break;
                        }
                    }
                    // make appropiate change in class subject assignment and in the
                    // weekly time table for the target teacher.
                    PreapareClassSubjectTransfer(oListTeacherSubjectId[iListItm].ToString(), IsChecked, false);
                    PrepareTTForSubjectTeacher(oListTeacherSubjectId[iListItm].ToString(), false);
                }
            }
            return false;
        }
    }
    /// <summary>
    /// This Method used
    /// </summary>
    private void SetSettings()
    {
        if (Settings.IsAssemblyApplicable)
        {
            chkSrcAssembly.Text = Resources.LocalizedResources.Is + " " + oResourceManager.GetString(Settings.AssemblyName.Replace(" ",string.Empty)) + " " + Resources.LocalizedResources.Applicable;
            chkTrgtAssembly.Text = Resources.LocalizedResources.Is + " " + oResourceManager.GetString(Settings.AssemblyName.Replace(" ", string.Empty)) + " " + Resources.LocalizedResources.Applicable;
        }
        if (Settings.IsMPTApplicable)
        {
            chkTrgtMPT.Text = Resources.LocalizedResources.Is + " " + Settings.MPTName + " " + Resources.LocalizedResources.Applicable;
            chkSrcMPT.Text = Resources.LocalizedResources.Is + " " + Settings.MPTName + " " + Resources.LocalizedResources.Applicable;
        }
        if (Settings.IsStaybackApplicable)
        {
            chkTrgtStayBack.Text = Resources.LocalizedResources.Is + " " + oResourceManager.GetString(Settings.StaybackName.Replace(" ", string.Empty)) + " " + Resources.LocalizedResources.Applicable;
            chkSrcStayBack.Text = Resources.LocalizedResources.Is + " " + oResourceManager.GetString(Settings.StaybackName.Replace(" ", string.Empty)) + " " + Resources.LocalizedResources.Applicable;
        }
    }

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        Response.Redirect("~/RITeSchool/Admin/TeacherTransfer.aspx", false);
        btnShow.Text = oResourceManager.GetString(hidbtnShow.Value.Replace(" ", string.Empty));
        //ShowSubjectMsges();
        SetSettings();        
    }
    #endregion
    
    #endregion
}
