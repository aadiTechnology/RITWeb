/* File Name:- StandardwiseExamSchedulePopUp.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 26- June-2009
 * Purpose :- This class is used to define standardwise exam schedule.
*/
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using System.Xml;
using BusinessLogic;
using BookEntities;
using Utility;

public partial class StandardwiseExamSchedulePopup : SchoolBase
{
    #region Constants
    const string S_EXAM_SCHEDULE_DATATABLE = "examScheduleDataTable";
    const string S_UNSUBMIT = "Unsubmit";
    const string S_SUBMIT = "Submit";
    const string S_SUBMIT_MESSAGE = "Exam schedule has been submited successfully!!!";
    const string S_UNSUBMIT_MESSAGE = "Exam schedule has been Unsubmited successfully!!!";
    #endregion

    #region Events

    /// <summary>
    /// This event is used for following purposes :-
    ///  - to set values to controls.
    ///  - to fill exam schedule details.
    ///  - to set javascript attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                SetValuesToControls();
                DataTable oDTInstruction = FillSubjectwiseExamScheduleGridview();
                DisplayInstruction(oDTInstruction);
                SetJavascriptAttributes();
                FillStandardsCheckboxList();
             }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }




   /// <summary>
    /// This event is used to set values to grid columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwExam_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            string sAMPM;
            int iHours;
            int iMinutes;

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;

                int iExamScheduleId = Convert.ToInt32(oDataRowView["SubjectWize_Standard_Exam_Schedule_Id"]);
                CheckBox chkRow = (CheckBox)oCurrentItem.FindControl("chkRow");
                if (iExamScheduleId == 0)
                    chkRow.Checked = false;
                else
                    chkRow.Checked = true;
                chkRow.Attributes.Add("OnClick", "EnableDisableControls(" + iRowId + ")");
               TextBox txtExamDate = (TextBox)oCurrentItem.FindControl("calstartdate");
                txtExamDate.Attributes.Add("onkeyup", "OnGridKeyUpNumber(this,0,false,event);");
                txtExamDate.Attributes.Add("OnChange", "HideMessage(true)");
                
                DropDownList ddlStartHour = (DropDownList)oCurrentItem.FindControl("ddlStartHr");
                DropDownList ddlStartMin = (DropDownList)oCurrentItem.FindControl("ddlStartMin");
                DropDownList ddlEndHour = (DropDownList)oCurrentItem.FindControl("ddlEndHr");
                DropDownList ddlEndMin = (DropDownList)oCurrentItem.FindControl("ddlEndMin");
                CheckBox chkIsTimeApplicable = (CheckBox)oCurrentItem.FindControl("chkIsTimeApplicable");
                int iStartMinute = Convert.ToDateTime(oDataRowView["Start_DateTime"]).Minute;
                int iMinute = Convert.ToDateTime(oDataRowView["Start_DateTime"]).Minute;
                string sStartTime = Convert.ToDateTime(oDataRowView["Start_DateTime"]).ToShortTimeString();
                string sEndTime = Convert.ToDateTime(oDataRowView["End_DateTime"]).ToShortTimeString();
                if (sStartTime == "12:00 AM" || iMinute % 5 != 0 || sStartTime == sEndTime)
                {
                    chkIsTimeApplicable.Checked = false;
                    ddlStartHour.Enabled = false;
                    ddlStartMin.Enabled = false;
                    ddlEndHour.Enabled = false;
                    ddlEndMin.Enabled = false;
                    ddlStartHour.SelectedValue = "8 am";
                    ddlStartMin.SelectedValue = "00";
                    ddlEndHour.SelectedValue = "8 am";
                    ddlEndMin.SelectedValue = "00";
                    if (hidItemCount.Value == "1")
                        ResetHeaderControls();
                }
                else
                {
                    if (iExamScheduleId != 0)
                        chkIsTimeApplicable.Checked = true;

                    sAMPM = Convert.ToDateTime(oDataRowView["Start_DateTime"]).ToString("tt").ToLower();
                    iHours = Convert.ToDateTime(oDataRowView["Start_DateTime"]).Hour;
                    if (iHours > 12)
                        iHours = iHours - 12;
                    ddlStartHour.SelectedValue = iHours + " " + sAMPM;

                    iMinutes = Convert.ToDateTime(oDataRowView["Start_DateTime"]).Minute;
                    ddlStartMin.SelectedValue = (iMinutes.ToString().Length == 1 ? "0" : "") + iMinutes.ToString();

                    sAMPM = Convert.ToDateTime(oDataRowView["End_DateTime"]).ToString("tt").ToLower();
                    iHours = Convert.ToDateTime(oDataRowView["End_DateTime"]).Hour;
                    if (iHours > 12)
                        iHours = iHours - 12;
                    ddlEndHour.SelectedValue = iHours + " " + sAMPM;

                    iMinutes = Convert.ToDateTime(oDataRowView["End_DateTime"]).Minute;
                    ddlEndMin.SelectedValue = (iMinutes.ToString().Length == 1 ? "0" : "") + iMinutes.ToString();
                }
                chkIsTimeApplicable.Attributes.Add("OnClick", "EnableDisableTimeControls(" + iRowId + ")");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save/edit/delete exam schedule and display message according to operation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        const int I_MESSAGE = 0;
        const int I_NEW_SCHEDULE = 1;
        try
        {
            if (Page.IsValid)
            {
                UpdateExamSchedule();
                SchoolwiseStandardExamScheduleMasterBL oSchoolwiseStandardExamScheduleMasterBL = InitializeExamScheduleDetails();
                DataSet oDataSet = oSchoolwiseStandardExamScheduleMasterBL.InsertExamScheduleDetails();
                btnSubmit.Enabled = true;
                
                if (hidIsConfig.Value != "Y")
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.StandardwiseExamScheduleConfig));
                if (oDataSet != null && oDataSet.Tables.Count > 0)
                {
                    DataTable oDataTable = oDataSet.Tables[I_MESSAGE];
                    if (oDataTable != null && oDataTable.Rows.Count > 0)
                    {
                        if (oDataTable.Rows[0]["Message"].ToString().Contains("SUCCESS"))
                        {
                            hidActionFlag.Value = "EDIT";
                            lblMessage.Visible = true;
                            lblMessage.Text = "Exam schedule has been saved successfully and you can copy exam schedule!!!";
                            hidStandardwiseExamScheduleId.Value = oDataTable.Rows[0]["ExamScheduleId"].ToString();
                            HideInstructionLinkAndCopyDiv(true);
                            tdMessage.Align = "Center";
                            SetInstructionslinkURL();
                        }
                        else if (oDataTable.Rows[0]["Message"].ToString().Contains("DELETED"))
                        {
                            hidActionFlag.Value = "NEW";
                            // set view as a new schedule.
                            lstvwExam.DataSource = oDataSet.Tables[I_NEW_SCHEDULE];
                            lstvwExam.DataBind();
                            lblMessage.Visible = true;
                            lblMessage.Text = "Exam schedule has been deleted successfully!!!";
                            hidStandardwiseExamScheduleId.Value = "0";
                            HideInstructionLinkAndCopyDiv(false);
                            tdMessage.Align = "Center";
                        }
                        else
                        {
                            ShowErrorMessage(oDataTable.Rows[0]["Message"].ToString());
                            tdMessage.Align = "Left";
                        }
                    }
                }
                else
                    ShowErrorMessage("Failed to save exam's schedule.");
            }
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to copy exam schedule to another standard 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCopy_Click(object sender, EventArgs e)
    {
        try
        {
            SchoolwiseStandardExamScheduleMasterBL oSchoolwiseStandardExamScheduleMasterBL = new SchoolwiseStandardExamScheduleMasterBL();
            List<int> lstStandrdId = GenerateStandatdIdList();
            string sTargetStandardXml = base.GenerateXml(lstStandrdId);
            oSchoolwiseStandardExamScheduleMasterBL.CopyExamScheduleToSelectedStandards(miSchoolId, miAcademicYearId, hidStandardId.Value.ToInt(), hidExamId.Value.ToInt(), sTargetStandardXml);
            lblMessage.Text = "Exam Schedule has been copied successfully!!!";
            ClearCheckBoxListItem();
        }
        catch (Exception ex)
        {

            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        } 
       
    }

    /// <summary>
    /// This event is used to add new item into listview with same subject name as a selected item's.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwExam_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "NEW")
        {
            hidItemCount.Value = "1";
            lblMessage.Visible = false;
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;

            // retrieve datatabel from viewstate and store it into temporary datatable.
            DataTable oDataTable = (DataTable)ViewState[S_EXAM_SCHEDULE_DATATABLE];
            int iExamScheduleId = Convert.ToInt32(lstvwExam.DataKeys[iRowId]["SubjectWize_Standard_Exam_Schedule_Id"]);
            int iLastRowId = Convert.ToInt32(hidLastRowId.Value);
            // Save the changes in temporary database.
            SaveChangesToTable(iLastRowId, ref oDataTable);

            //Add new row into table with default values.
            oDataTable.Rows.Add();
            int iRowCount = oDataTable.Rows.Count;
            iRowCount = iRowCount - 1;
            oDataTable.Rows[iRowCount]["SubjectWize_Standard_Exam_Schedule_Id"] = 0;
            oDataTable.Rows[iRowCount]["Subject_Id"] = Convert.ToInt32(lstvwExam.DataKeys[iRowId]["Subject_Id"]);
            oDataTable.Rows[iRowCount]["Subject_Name"] = ((Label)oCurrentItem.FindControl("lblSubjects")).Text;
            oDataTable.Rows[iRowCount]["TestType"] = "";
            oDataTable.Rows[iRowCount]["Start_DateTime"] = DateTime.Now;
            oDataTable.Rows[iRowCount]["End_DateTime"] = DateTime.Now;
            oDataTable.Rows[iRowCount]["TotalTime"] = 0;
            oDataTable.Rows[iRowCount]["Description"] = "";
            oDataTable.Rows[iRowCount]["Marks"] = 0;
            oDataTable.Rows[iRowCount]["IsNewRow"] = 1;

            //Bind datatable to exam schedule listview.
            oDataTable.DefaultView.Sort = "Subject_Id";
            lstvwExam.DataSource = oDataTable.DefaultView;
            ViewState.Add(S_EXAM_SCHEDULE_DATATABLE, oDataTable);
            lstvwExam.DataBind();

            hidLastRowId.Value = iRowCount.ToString();
            hidTempExamScheduleId.Value = iExamScheduleId.ToString();
        }
    }

    /// <summary>
    /// This event is used to close popup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            if (lstvwExam.Items.Count > 0)
            {
                string sQueryString = "standardId=" + hidStandardId.Value + "&Is_Configured=" + hidIsConfig.Value;
                string sEncryptQueryString = Utility.CommonUtility.EncryptQuerystring(sQueryString);
                sQueryString = "'?" + sEncryptQueryString + "'";
                Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + sQueryString + ";window.close();window.opener.focus(); </Script>");
            }
            else
                Response.Redirect("../common/controlpanel.aspx", false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Submit the Exam Schedule.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            bool bIsUnpublished = false;
            SchoolwiseStandardExamScheduleMasterBL oSchoolwiseStandardExamScheduleMasterBL = new SchoolwiseStandardExamScheduleMasterBL();
            if (btnSubmit.Text == S_SUBMIT)
            {
                SetButtonState(false, S_UNSUBMIT);
                bIsUnpublished = false;
            }
            else
            {
               SetButtonState(true, S_SUBMIT);
               bIsUnpublished = true;
            }
            oSchoolwiseStandardExamScheduleMasterBL.SubmitExamSchedule(miSchoolId,miAcademicYearId,miUserId, Convert.ToInt32(hidStandardId.Value), bIsUnpublished,Convert.ToInt32(hidExamId.Value));
            if (bIsUnpublished == false)
                lblMessage.Text = S_SUBMIT_MESSAGE;
            else
                lblMessage.Text = S_UNSUBMIT_MESSAGE;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region private Methods
   
    /// <summary>
    /// This mothod use to fill standard list 
    /// </summary>
    
    private void FillStandardsCheckboxList()
    {
        List<ClassDetails> lstClasses = SchoolwiseStandardExamScheduleMasterBL.GetStandards(miSchoolId, miAcademicYearId, Convert.ToInt32(hidExamId.Value));
        lstClasses.ForEach(ClassNames => chkListClasses.Items.Add(new ListItem(ClassNames.Classname, ClassNames.StandardDivisionId.ToString())));
        chkListClasses.Items.Remove( new ListItem(hidStandardName.Value.ToString(),hidStandardId.Value.ToString()));   
    }
    /// <summary>
    /// This method is used to display message when Insert/Edit/Delete operation is successfully compleated.
    /// </summary>
    /// <param name="abAction"></param>
    private void HideInstructionLinkAndCopyDiv(bool abAction)
    {
        lblMessage.Visible = true;
        trInstruction.Visible = abAction;
        hlnkInstructions.Visible = abAction;
        CopyExamDiv.Visible = abAction;

    }

    /// <summary>
    /// This method is used to display eroor messages.
    /// </summary>
    /// <param name="asMessage"></param>
    private void ShowErrorMessage(string asMessage)
    {
        trValidationSummary.Visible = true;
        lblMessage.Visible = false;
        lblErrorMsg.Visible = true;
        lblErrorMsg.Text = asMessage;
    }

    /// <summary>
    /// /// This method is used to initialize exam schedule details.
    /// </summary>
    /// <returns></returns>
    private SchoolwiseStandardExamScheduleMasterBL InitializeExamScheduleDetails()
    {
        SchoolwiseStandardExamScheduleMasterBL oSchoolwiseStandardExamScheduleMasterBL = new SchoolwiseStandardExamScheduleMasterBL();
        string sXml = GenerateExamConfigXML();
        oSchoolwiseStandardExamScheduleMasterBL.School_Id = miSchoolId;
        oSchoolwiseStandardExamScheduleMasterBL.academic_Year_Id = miAcademicYearId;
        oSchoolwiseStandardExamScheduleMasterBL.Standard_Id =Convert.ToInt32(hidStandardId.Value);
        oSchoolwiseStandardExamScheduleMasterBL.SchoolWise_Test_Id = Convert.ToInt32(hidExamId.Value);
        oSchoolwiseStandardExamScheduleMasterBL.Standard_Test_Id = Convert.ToInt32(hidStandardTestId.Value);
        oSchoolwiseStandardExamScheduleMasterBL.Inserted_By_id = Convert.ToString(miUserId);
        oSchoolwiseStandardExamScheduleMasterBL.Schoolwise_Standard_Exam_Schedule_Id =Convert.ToInt32(hidStandardwiseExamScheduleId.Value);
        oSchoolwiseStandardExamScheduleMasterBL.Exam_Details = sXml;
        return oSchoolwiseStandardExamScheduleMasterBL;
    }

    ///// <summary>
    ///// This method is used to get values coming through querystring.
    ///// </summary>
    private void SetValuesToControls()
    {
        if (QueryString["Standard_Id"] != null)
            hidStandardId.Value = QueryString["Standard_Id"];
        
		if (QueryString["Schoolwise_Test_Id"] != null)
            hidExamId.Value = QueryString["Schoolwise_Test_Id"];
        
		if (QueryString["Schoolwise_Standard_Exam_Schedule_Id"] != null)
            hidStandardwiseExamScheduleId.Value = QueryString["Schoolwise_Standard_Exam_Schedule_Id"];
        
		if (QueryString["Test_Name"] != null)
            lblExamName.Text = QueryString["Test_Name"];
        
		if (QueryString["Standard_Name"] != null)
            hidStandardName.Value = QueryString["Standard_Name"];
        
		txtStandardName.Text = hidStandardName.Value;
        
		if (QueryString["Standard_Test_Id"] != null)
            hidStandardTestId.Value = QueryString["Standard_Test_Id"];
        
		if (QueryString["Mode"] != null)
            hidActionFlag.Value = QueryString["Mode"];
        
		hidIsConfig.Value = QueryString["Is_Configured"];

        //Standardwise academic year change.
        if (!hidStandardId.Value.IsNullOrEmpty())
        {
            DataTable oDT = SchoolWiseAcademicYearMasterBL.GetAcademicDatesForStandard(miSchoolId,miAcademicYearId,Convert.ToInt32(hidStandardId.Value));
            if (oDT.Rows.Count > 0)
            {
                hidYearStartDate.Value = oDT.Rows[0]["StartDate"].ToString();
                hidYearEndDate.Value = oDT.Rows[0]["EndDate"].ToString();
            }
            else btnSave.Enabled = false;
        }
        else
        {
            hidYearStartDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToString();
            hidYearEndDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToString();
        }
        // End.
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

     /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        btnSave.Attributes.Add("onclick", "DisableButtons()");
        btnCopy.Attributes.Add("onclick", "if(!SelectCheckBox()){return false;}");
        btnCloseDiv.Attributes.Add("onclick", "return HidePopup()");
        btnCopyToShowDiv.Attributes.Add("onclick", "ShowPopup()");
        btnCancel.Attributes.Add("onclick","return ClearCheckBxList()");
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnClose, btnCopy, btnCancel, btnCloseDiv, btnCopyToShowDiv,btnSubmit});
    }

    ///// <summary>
    /////  This method is used to fill subjectwise exam schedule.
    ///// </summary>
    private DataTable FillSubjectwiseExamScheduleGridview()
    {
        const int I_EXAM_SCHEDULE = 0;
        const int I_INSTRUCTION = 1;
        const int I_ISSUBMIT = 2;
        int iStandardId = Convert.ToInt32(hidStandardId.Value);        
        int iStandardwiseExamScheduleId = Convert.ToInt32(hidStandardwiseExamScheduleId.Value);
        SubjectwiseStandardExamScheduleBL oSubjectwiseStandardExamScheduleBL = new SubjectwiseStandardExamScheduleBL();
        DataSet oDSExamSchedule = oSubjectwiseStandardExamScheduleBL.getSubjectExamScheduleList(miSchoolId, miAcademicYearId, iStandardId, Convert.ToInt32(hidStandardwiseExamScheduleId.Value));
        oDSExamSchedule.Tables[I_EXAM_SCHEDULE].DefaultView.Sort = "Subject_Id";
        lstvwExam.DataSource = oDSExamSchedule.Tables[I_EXAM_SCHEDULE].DefaultView;
        oDSExamSchedule.Tables[I_EXAM_SCHEDULE].Columns.Add("IsNewRow");
        ViewState.Add(S_EXAM_SCHEDULE_DATATABLE, oDSExamSchedule.Tables[I_EXAM_SCHEDULE]);
        lstvwExam.DataBind();

        //this code is used for set the Submit button state as per condition.
        DataTable dtSubmitStatus = oDSExamSchedule.Tables[I_ISSUBMIT];
        if (dtSubmitStatus.IsNonEmpty())
        {
            int iIsSubmit = dtSubmitStatus.Rows[0]["IsSubmited"].ToInt();
            SetSubmitButtonState(iIsSubmit);
        }
        else
            btnSubmit.Enabled = false;

        return oDSExamSchedule.Tables[I_INSTRUCTION];
    }
   /// <summary>
   /// This method use to clear checkbox list item after copy schedule.
   /// </summary>
    private void ClearCheckBoxListItem()
    {
        foreach (ListItem item in chkListClasses.Items)
        {
            //check anything out here
            if (item.Selected)
                item.Selected = false;
        }
    }
    /// <summary>
    /// This method is used to update exam schedule.
    /// </summary>
    private void UpdateExamSchedule()
    {
        if (ViewState[S_EXAM_SCHEDULE_DATATABLE] != null)
        {
            DataTable oDTExamSchedule = (DataTable)ViewState[S_EXAM_SCHEDULE_DATATABLE];
            SaveChangesToTable(Convert.ToInt32(hidLastRowId.Value), ref oDTExamSchedule);
            oDTExamSchedule.DefaultView.Sort = "Subject_Id";
            lstvwExam.DataSource = oDTExamSchedule.DefaultView;
            lstvwExam.DataBind();
        }
    }

    /// <summary>
    /// This method is used to display/hide instruction.
    /// </summary>
    /// <param name="oDtSubjSchedule"></param>
    private void DisplayInstruction(DataTable aoDTInstruction)
    {
        if (aoDTInstruction != null && aoDTInstruction.Rows.Count > 0 && aoDTInstruction.Rows[0][0] != DBNull.Value)
        {
            txtInstructions.Text = aoDTInstruction.Rows[0]["Instructions"].ToString();
            if (txtInstructions.Text.Trim() == string.Empty)
            {
                hlnkInstructions.Text = "Add Instructions";
                tdlblInstructions.Visible = false;
                tdtxtInstructions.Visible = false;
                trInstruction.Visible = true;
                hlnkInstructions.Visible = true;
                tdlblInstructions.Visible = false;
             }
            else
            {
                hlnkInstructions.Visible = true;
                trInstruction.Visible = true;
                hlnkInstructions.Text = "Update Instructions";
                tdlblInstructions.Visible = true;
                tdtxtInstructions.Visible = true;
            }
            SetInstructionslinkURL();
            CopyExamDiv.Visible = true;
        }
        else
        {
            hlnkInstructions.Text = "Add Instructions";
            txtInstructions.Text = string.Empty;
            tdlblInstructions.Visible = false;
            tdtxtInstructions.Visible = false;
        }
    }

    ///// <summary>
    ///// This method is used to set decrypted URL to toppres link
    ///// </summary>
    private void SetInstructionslinkURL()
    {
        string sQuerystring = "Standard_Id=" + hidStandardId.Value
                            + "&Test_Name=" + lblExamName.Text
                            + "&Standard_Name=" + hidStandardName.Value
                            + "&Standard_Test_Id=" + hidStandardTestId.Value
                            + "&Is_Configured=" + hidIsConfig.Value
                            + "&StdExamSchedId=" + hidStandardwiseExamScheduleId.Value
                            + "&Schoolwise_Test_Id=" + hidExamId.Value;
        sQuerystring = "ExamScheduleInstructionsPopUp.aspx?" + CommonUtility.EncryptQuerystring(sQuerystring);
        hlnkInstructions.Attributes.Add("onclick", "ShowInstructions('" + sQuerystring + "');return false;");
    }

  

    /// <summary>
    /// This method is used to generate xml string of exam schedule details.
    /// </summary>
    /// <returns></returns>
    private string GenerateExamConfigXML()
    {
        const string S_ELEMENT = "element";
        string sAttribute;
        int subjectId;
        string sTime;
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("SubjectwiseStandardExamSchedule");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "SubjectwiseStandardExamSchedule", "");

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount <= lstvwExam.Items.Count - 1; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwExam.Items[iRowCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;

            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "SubjectwiseStandardExamSchedule", "");
            CheckBox chkRow = (CheckBox)oCurrentItem.FindControl("chkRow");
            if (chkRow.Checked)
            {
                sAttribute = "Subject_Id";
                XmlAttribute attr = oDoc.CreateAttribute(sAttribute);
                subjectId = Convert.ToInt32(lstvwExam.DataKeys[iRowCount]["Subject_Id"]);
                attr.Value = subjectId.ToString();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "ExamTypes";
                attr = oDoc.CreateAttribute(sAttribute);
                TextBox txtExamTypes = (TextBox)oCurrentItem.FindControl("txtExamTypes");
                attr.Value = txtExamTypes.Text.Trim();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "Description";
                attr = oDoc.CreateAttribute(sAttribute);
                TextBox txtDescription = (TextBox)oCurrentItem.FindControl("txtDescription");
                attr.Value = txtDescription.Text.Trim();
                oXmlNode.Attributes.Append(attr);

                CheckBox chkIsTimeApplicable = (CheckBox)oCurrentItem.FindControl("chkIsTimeApplicable");

                sAttribute = "Exam_Start_Date";
                attr = oDoc.CreateAttribute(sAttribute);
                TextBox txtExamDate = (TextBox)oCurrentItem.FindControl("calstartdate");


                DropDownList ddlHour = (DropDownList)oCurrentItem.FindControl("ddlStartHr");
                DropDownList ddlMin = (DropDownList)oCurrentItem.FindControl("ddlStartMin");
                if (chkIsTimeApplicable.Checked)
                    sTime = txtExamDate.Text + " " + ddlHour.SelectedValue.Substring(0, ddlHour.SelectedValue.Length - 2) + ":" + ddlMin.SelectedValue + " " + ddlHour.SelectedValue.Substring(ddlHour.SelectedValue.Length - 2);
                else
                    sTime = txtExamDate.Text + " " + "12:00 AM";
                attr.Value = sTime;
                oXmlNode.Attributes.Append(attr);

                sAttribute = "Exam_End_Date";
                attr = oDoc.CreateAttribute(sAttribute);
                txtExamDate = (TextBox)oCurrentItem.FindControl("calstartdate");
                ddlHour = (DropDownList)oCurrentItem.FindControl("ddlEndHr");
                ddlMin = (DropDownList)oCurrentItem.FindControl("ddlEndMin");
                if (chkIsTimeApplicable.Checked)
                    sTime = txtExamDate.Text + " " + ddlHour.SelectedValue.Substring(0, ddlHour.SelectedValue.Length - 2) + ":" + ddlMin.SelectedValue + " " + ddlHour.SelectedValue.Substring(ddlHour.SelectedValue.Length - 2);
                else
                    sTime = txtExamDate.Text + " " + "12:00 AM";
                attr.Value = sTime;
                oXmlNode.Attributes.Append(attr);

                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
        }

        // Add the root node to document element.         
        root.AppendChild(oXmlRootNode);
        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to save changes of listview into temporary database.
    /// </summary>
    /// <param name="aiLastRowId"></param>
    /// <param name="aoDataTable"></param>
    /// <returns></returns>
    private void SaveChangesToTable(int aiLastRowId, ref DataTable aoDataTable)
    {
        ListViewItem oListViewItem;
        int iTotalRowCount = lstvwExam.Items.Count - 1;
        for (int iRowCount = 0; iRowCount <= iTotalRowCount; iRowCount++)
        {
            oListViewItem = lstvwExam.Items[iRowCount];
            CheckBox chkRow = (CheckBox)oListViewItem.FindControl("chkRow");
            if (chkRow.Checked == false)
                aoDataTable.Rows[iRowCount]["SubjectWize_Standard_Exam_Schedule_Id"] = 0;

            // newly checked subject.
            if (chkRow.Checked && Convert.ToInt32(aoDataTable.Rows[iRowCount][0]) == 0)
                aoDataTable.Rows[iRowCount]["SubjectWize_Standard_Exam_Schedule_Id"] = -1;
            else if (chkRow.Checked == false && Convert.ToInt32(aoDataTable.Rows[iRowCount][0]) == -1)
                aoDataTable.Rows[iRowCount]["SubjectWize_Standard_Exam_Schedule_Id"] = 0;

            aoDataTable.Rows[iRowCount]["Subject_Id"] = Convert.ToInt32(lstvwExam.DataKeys[iRowCount]["Subject_Id"]);
            aoDataTable.Rows[iRowCount]["Subject_Name"] = ((Label)oListViewItem.FindControl("lblSubjects")).Text;
            aoDataTable.Rows[iRowCount]["TestType"] = ((TextBox)oListViewItem.FindControl("txtExamTypes")).Text;
            aoDataTable.Rows[iRowCount]["TotalTime"] = 0;
            aoDataTable.Rows[iRowCount]["Description"] = ((TextBox)oListViewItem.FindControl("txtDescription")).Text;
            aoDataTable.Rows[iRowCount]["Marks"] = 0;

            string sTime;
            TextBox txtExamDate = (TextBox)oListViewItem.FindControl("calstartdate");
            DropDownList ddlHour = (DropDownList)oListViewItem.FindControl("ddlStartHr");
            DropDownList ddlMin = (DropDownList)oListViewItem.FindControl("ddlStartMin");

            CheckBox chkIsTimeApplicable = (CheckBox)oListViewItem.FindControl("chkIsTimeApplicable");
            if (chkIsTimeApplicable.Checked)
                sTime = txtExamDate.Text + " " + ddlHour.SelectedValue.Substring(0, ddlHour.SelectedValue.Length - 2) + ":" + ddlMin.SelectedValue + " " + ddlHour.SelectedValue.Substring(ddlHour.SelectedValue.Length - 2);
            else
                sTime = txtExamDate.Text + " " + "12:00 AM";
            aoDataTable.Rows[iRowCount]["Start_DateTime"] = sTime;

            txtExamDate = (TextBox)oListViewItem.FindControl("calstartdate");
            ddlHour = (DropDownList)oListViewItem.FindControl("ddlEndHr");
            ddlMin = (DropDownList)oListViewItem.FindControl("ddlEndMin");
            if (chkIsTimeApplicable.Checked)
                sTime = txtExamDate.Text + " " + ddlHour.SelectedValue.Substring(0, ddlHour.SelectedValue.Length - 2) + ":" + ddlMin.SelectedValue + " " + ddlHour.SelectedValue.Substring(ddlHour.SelectedValue.Length - 2);
            else
                sTime = txtExamDate.Text + " " + "12:00 AM";
            aoDataTable.Rows[iRowCount]["End_DateTime"] = sTime;
        }
    }
    
    /// <summary>
    /// This method use to return list of standard id
    /// </summary>
    /// <returns></returns>
    private List<int> GenerateStandatdIdList()
    {
        int iTotalStandards = chkListClasses.Items.Count;
        List<int> lstStandardId = new List<int>();
        for (int iListIndex = 0; iListIndex < iTotalStandards; iListIndex++)
        {
            if (chkListClasses.Items[iListIndex].Selected == true)
                lstStandardId.Add(chkListClasses.Items[iListIndex].Value.ToInt());
        }
        return lstStandardId;
    }

    /// <summary>
    /// This method is used to reset Hreader  controls
    /// </summary>
    private void ResetHeaderControls()
    {
        System.Web.UI.HtmlControls.HtmlTableRow oTR = (System.Web.UI.HtmlControls.HtmlTableRow)lstvwExam.FindControl("trHeaderControls");
        CheckBox chkAllTimeApplicable = (CheckBox)oTR.FindControl("chkAllTimeApplicable");
        chkAllTimeApplicable.Checked = false;
        hidItemCount.Value = "0";
    }

    /// <summary>
    /// This method is used to set Button State.
    /// </summary>
    private void SetButtonState(bool abValue, string asText)
    {
        btnSubmit.Text = asText;
        btnSave.Enabled = abValue;
        lstvwExam.Enabled = abValue;
    }

    /// <summary>
    /// This method is used to set Submit Button State.
    /// </summary>
    private void SetSubmitButtonState(int aiValue)
    {
        if (aiValue == Constants.I_ONE)
        {
            btnSave.Enabled = false;
            btnSubmit.Text = S_UNSUBMIT;
            lstvwExam.Enabled = false;
        }
        else if (aiValue == Constants.I_ZERO)
        {
            btnSubmit.Text = S_SUBMIT;
            btnSubmit.Enabled = true;
            lstvwExam.Enabled = true;
        }
    }
    #endregion   
}