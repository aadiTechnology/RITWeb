using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using SchoolEntities.Admin;
using Utility;

public partial class OnlineExamConfigUI : SchoolBase
{
    #region Data Member(s)

    private OnlineExamConfigurationBL moOnlineExamConfigurationBL = null; 

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty)
            {
                hidSortExpression.Value = "StartDateAndTime";
                hidSortDirection.Value = Constants.S_DESCENDING;
            }

            AddSortImage(lstvwQuestions, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moOnlineExamConfigurationBL = new OnlineExamConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                FillStandard();
                FillSubjectCombo();
                //  FillStandardDivisionLstBox();
                FillQuestions();
                FillTestCombobox();
                FillConfiguration();
                SetJavascriptAttributes();
                SetButtonState();
                ReadQueryString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwQuestions_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetSortVariables();
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e) //fill divisions on standardclick
    {
        FillDivisions();
        FillSubjectCombo();
        FillQuestions();
        ClearFields();
    }

    protected void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillSubjectCombo();
            FillQuestions();
            FillConfiguration();
            ClearFields();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillQuestions();
            FillConfiguration();
            ClearFields();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwQuestions_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName != Constants.S_COMMAND_SORT)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iListIndex = oCurrentItem.DisplayIndex;
                int iId = Convert.ToInt32(lstvwQuestions.DataKeys[iListIndex]["Id"]);
                hidExamConfigId.Value = Convert.ToString(iId);
                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                    DeleteTransportStaffDetails(iId);
                else if (e.CommandName == Constants.S_COMMAND_UPDATE)
                    FillControlsForUpdate(iId);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveExamConfigDetails();

            if (hidExamConfigId.Value == Constants.S_ZERO)
                DisplayMessage("Exam details saved successfully!!!", false);
            else
                DisplayMessage("Exam details updated successfully!!!", false);

            if (hidIsConfigured.Value != Constants.S_YES)
                base.SaveConfigDetails(Constants.SchoolConfigurations.OnlineExamConfiguration.ToInt());

            hidIsConfigured.Value = Constants.S_YES;

            ClearFields();
            FillQuestions();
            FillConfiguration();
        }
        catch (SqlException ex)
        {
            DisplayMessage(ex.Message, true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwQuestions_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton imgBtnEdit = e.Item.FindControl("imgBtnEdit") as ImageButton;
                ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");

                OnlineExamConfiguration oOnlineExamConfiguration = e.Item.DataItem as OnlineExamConfiguration;
                oimgbtnDelete.Visible = !oOnlineExamConfiguration.IsSubmitted;
                imgBtnEdit.Visible = !oOnlineExamConfiguration.IsSubmitted;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwQuestions_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwQuestions.Items.Count > 0)
                ControlUtility.FillListViewPagerFooter(lstvwQuestions, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwQuestions);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwExamQuestionConfiguration_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                CheckBox oChkSelect = e.Item.FindControl("ChkSelect") as CheckBox;
                if (Convert.ToInt32(lstvwExamQuestionConfiguration.DataKeys[iRowId]["Id"].ToString()) > 0)
                    oChkSelect.Checked = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearFields();
        FillQuestions();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            SubmitExam(true);
            DisplayMessage("Exam details submitted successfully!!!", false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnUnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            SubmitExam(false);
            DisplayMessage("Exam details unsubmitted successfully!!!", false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    private void SaveExamConfigDetails()
    {
        OnlineExamConfiguration oVehicleDetailsBL = PopulateExamBL();  //////
        string ExamXML = GetExamXML();
        moOnlineExamConfigurationBL.Save(ExamXML, oVehicleDetailsBL, cmbStandard.SelectedValue.ToInt());
    }

    private string GetExamXML()
    {
        CheckBox oChkIsStaffSelected;
        const string S_ELEMENT = "element";
        string sAttribute;
        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement oRoot = oDoc.CreateElement("ExamQuestion");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "ExamQuestion", "");
        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount <= lstvwExamQuestionConfiguration.Items.Count - 1; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwExamQuestionConfiguration.Items[iRowCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            int iTransportStaffId = Convert.ToInt32(lstvwExamQuestionConfiguration.DataKeys[iRowId]["Id"]);
            int iQuestionId = Convert.ToInt32(lstvwExamQuestionConfiguration.DataKeys[iRowId]["QuestionId"]);
            oChkIsStaffSelected = (CheckBox)oCurrentItem.FindControl("ChkSelect");

            if ((oChkIsStaffSelected.Checked == true && iQuestionId == 0) || iQuestionId > 0)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "ExamQuestion", "");
                sAttribute = "ExamConfigurationId";
                XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = iTransportStaffId.ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "QuestionId";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = iQuestionId.ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "IsDeleted";
                oAttr = oDoc.CreateAttribute(sAttribute);
                if (oChkIsStaffSelected.Checked)
                    oAttr.Value = "0";
                else
                    oAttr.Value = "1";
                oXmlNode.Attributes.Append(oAttr);
                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
        }
        // Add the root node to document element.         
        oRoot.AppendChild(oXmlRootNode);
        return oRoot.InnerXml;
    }

    private OnlineExamConfiguration PopulateExamBL()  //
    {
        OnlineExamConfiguration oExamWiseQueConfig = new OnlineExamConfiguration();  //////
        oExamWiseQueConfig.ExamId = cmbExam.SelectedValue.ToInt();
        oExamWiseQueConfig.SubjectId = cmbSubject.SelectedValue.ToInt();
        oExamWiseQueConfig.StandardDivisionId = cmbClass.SelectedValue.ToInt();
        oExamWiseQueConfig.StartDateAndTime = txtStartDate.Text.ToDateTime();
        oExamWiseQueConfig.EndDateAndTime = txtEndDate.Text.ToDateTime();
        oExamWiseQueConfig.NoOfQuestions = txtNoOfQuestions.Text.ToInt();
        oExamWiseQueConfig.ShuffleForCount = chkSuffleForCount.Checked;
        oExamWiseQueConfig.ShuffleForSequence = chkShuffleForSequence.Checked;
        oExamWiseQueConfig.StartTime = txtExamStartTime.Text;
        oExamWiseQueConfig.EndTime = txtExamEndTime.Text;


        oExamWiseQueConfig.SchoolId = miSchoolId;
        oExamWiseQueConfig.AcademicYearId = miAcademicYearId;
        oExamWiseQueConfig.InsertedById = miUserId;
        oExamWiseQueConfig.Id = Convert.ToInt32(hidExamConfigId.Value);

        return oExamWiseQueConfig;
    }

    private void ClearFields()
    {
        txtNoOfQuestions.Text = string.Empty;
        txtStartDate.Text = string.Empty;
        txtExamStartTime.Text = string.Empty;
        txtEndDate.Text = string.Empty;
        hidExamConfigId.Value = "0";
        txtExamEndTime.Text = string.Empty;
        chkSuffleForCount.Checked = false;
        chkShuffleForSequence.Checked = false;
        cmbExam.ClearSelection();
        cmbStandard.Enabled = true;  //
        cmbClass.Enabled = true;  //
        cmbSubject.Enabled = true; //
        chkShuffleForSequence.Enabled = true;
    }

    private void SubmitExam(bool abIsSubmit)
    {
        moOnlineExamConfigurationBL.Submit(cmbStandard.SelectedValue.ToInt(), cmbClass.SelectedValue.ToInt(), cmbSubject.SelectedValue.ToInt(), abIsSubmit);
        FillConfiguration();
    }

    private void SetButtonState()
    {
        ButtonStateDetails oButtonStateDetails = moOnlineExamConfigurationBL.GetButtonState(cmbStandard.SelectedValue.ToInt(), cmbClass.SelectedValue.ToInt(), cmbSubject.SelectedValue.ToInt());
        btnSubmit.Enabled = oButtonStateDetails.EnableSubmitButtton;
        btnUnsubmit.Enabled = oButtonStateDetails.EnableUnSubmitButtton;
    }

    private void ReadQueryString()
    {
        if (QueryString["Is_Configured"] != null)
            hidIsConfigured.Value = QueryString["Is_Configured"].ToString();
    }

    private void SetJavascriptAttributes()
    {
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.OnlineExamRelated));
        valSumTaskDetails.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    private void FillQuestions()  //fill listview
    {
        DataTable oDt = moOnlineExamConfigurationBL.GetAllQuestions(cmbStandard.SelectedValue.ToInt(), cmbClass.SelectedValue.ToInt(), cmbSubject.SelectedValue.ToInt());
        lstvwExamQuestionConfiguration.DataSource = oDt;
        lstvwExamQuestionConfiguration.DataBind();

        CheckBox oChkHeader = (CheckBox)lstvwExamQuestionConfiguration.FindControl("ChkSelectAll");
        if (oChkHeader != null)
            oChkHeader.Checked = false;
    }

    private void FillConfiguration()  //fill questionlistview
    {
        lstvwQuestions.DataSourceID = ObjDSVehicleStaffDetails.ID;
        lstvwQuestions.DataBind();
        SetButtonState();
    }

    private void FillControlsForUpdate(int aiVehicleId)
    {
        DataSet oDSVehicleStaffDetails = moOnlineExamConfigurationBL.GetDetailsForUpdateQuestions(aiVehicleId);
        if (oDSVehicleStaffDetails != null && oDSVehicleStaffDetails.Tables.Count > 0)
        {
            cmbStandard.Enabled = false;   //  disable for edit mode
            cmbSubject.Enabled = false;  // disable for edit mode
            cmbClass.Enabled = false; // disable for edit mode
            txtNoOfQuestions.Text = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["NoOfQuestions"]);
            CalDtPopup.DateValue = Convert.ToDateTime(oDSVehicleStaffDetails.Tables[1].Rows[0]["StartDateAndTime"]);
            txtStartDate.Text = CalDtPopup.DateValue.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["EndDateAndTime"]);
            CalEndDtPopup.DateValue = Convert.ToDateTime(oDSVehicleStaffDetails.Tables[1].Rows[0]["EndDateAndTime"]);
            txtEndDate.Text = CalEndDtPopup.DateValue.ToString("dd-MMM-yyyy");
            cmbClass.SelectedValue = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["StandardDivisionId"]);
            cmbExam.SelectedValue = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["ExamId"]);
            cmbSubject.SelectedValue = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["SubjectId"]);
            chkShuffleForSequence.Checked = Convert.ToBoolean(oDSVehicleStaffDetails.Tables[1].Rows[0]["ShuffleForSequence"]);
            chkSuffleForCount.Checked = Convert.ToBoolean(oDSVehicleStaffDetails.Tables[1].Rows[0]["ShuffleForCount"]);
            txtExamStartTime.Text = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["StartTime"].ToDateTime().ToString("hh:mm tt"));  //
            txtExamEndTime.Text = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["EndTime"].ToDateTime().ToString("hh:mm tt"));

            if (chkSuffleForCount.Checked)
                chkShuffleForSequence.Enabled = false;

            FillQuestions();
            CheckBox oChkHeader = (CheckBox)lstvwExamQuestionConfiguration.FindControl("ChkSelectAll");
            int iIndex = 0;
            foreach (ListViewDataItem olstItem in lstvwExamQuestionConfiguration.Items)
            {
                if (olstItem.ItemType == ListViewItemType.DataItem)
                {
                    CheckBox ChkSelect = olstItem.FindControl("ChkSelect") as CheckBox;
                    DataRow[] dr = oDSVehicleStaffDetails.Tables[0].Select("QuestionId=" + lstvwExamQuestionConfiguration.DataKeys[olstItem.DisplayIndex]["QuestionId"]);
                    if (dr.Length > 0)
                    {
                        ChkSelect.Checked = true;
                        iIndex++;
                    }
                    else
                        ChkSelect.Checked = false;
                }
            }

            if (iIndex == lstvwExamQuestionConfiguration.Items.Count)
                oChkHeader.Checked = true;
            else
                oChkHeader.Checked = false;
        }
    }

    private void DeleteTransportStaffDetails(int aiVehicleId)
    {
        moOnlineExamConfigurationBL.Delete(aiVehicleId);
        DisplayMessage("Exam details deleted successfully", false);
        ClearFields();
        FillConfiguration();
        FillQuestions();
    }

    /// <summary>
    /// This method  is used to fill test combobox.
    /// </summary>
    private void FillTestCombobox()
    {
        using (DataTable oDsAllTests = moOnlineExamConfigurationBL.GetAllTestsForClass())
        {
            ControlUtility.FillDropDownList(
                oDsAllTests,
                ref cmbExam,
                "Id",
                "Name",
                Constants.S_SELECT);  //
        }
    }

    private void FillDivisions()
    {
        DataTable oDtStandardCollection = moOnlineExamConfigurationBL.GetAssociatedStandards(cmbStandard.SelectedValue.ToInt());
        ControlUtility.FillDropDownList(oDtStandardCollection, ref cmbClass, Constants.S_STANDARD_DIVISION_ID_FIELD, Constants.S_DIVISION_NAME_FIELD, Constants.S_ALL);
    }

    private void FillStandard()
    {
        DataTable oDT = moOnlineExamConfigurationBL.GetAllStandards();
        ControlUtility.FillDropDownList(oDT, ref cmbStandard, Constants.S_STANDARD_ID_FIELD, Constants.S_STANDARD_NAME_FIELD, Constants.S_SELECT);

        FillDivisions();
    }

    private void FillSubjectCombo()
    {
        List<YearWiseSubjectsDetails> lstSubjects = moOnlineExamConfigurationBL.GetAllYearwiseSubjects(cmbStandard.SelectedValue.ToInt(), cmbClass.SelectedValue.ToInt());
        ListSource.FillDropDownList(lstSubjects, cmbSubject, "SubjectName", "SubjectId", Constants.S_SELECT);
    }

    private void DisplayMessage(string asMessage, bool abIsError)
    {
        lblUpdateMessage.Text = asMessage;
        if (abIsError)
        {
            lblUpdateMessage.ForeColor = System.Drawing.Color.Red;
            lblUpdateMessage.Font.Bold = false;
        }
        else
        {
            lblUpdateMessage.ForeColor = System.Drawing.Color.Blue;
            lblUpdateMessage.Font.Bold = true;
        }
    } 

    #endregion
}