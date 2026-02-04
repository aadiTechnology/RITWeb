using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using BusinessLogic;
using SchoolEntities;
using Utility;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class StudentOnlineExamUI : SchoolBase
{
    #region Constant(s)

    private const string S_SAVE_MESSAGE = "Online Exam saved successfully !!!";
    private const string S_SUBMIT_MESSAGE = "Online Exam submitted successfully !!!";
    const string S_UPLOAD_FILE_FOLDER_PATH = "\\RITeSchool\\Uploads\\OnlineExamImages\\";
    private const string S_FOLDER_PATH = @"../Uploads/OnlineExamImages/";

    #endregion

    #region DataMember

    private OnlineExamWiseQueConfigBL moOnlineExamWiseQueConfigBL;
    private List<QuestionDetails> mlstQuestionDetails;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to set the values to coltrols.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moOnlineExamWiseQueConfigBL = new OnlineExamWiseQueConfigBL(miSchoolId, miAcademicYearId);
            if (!IsPostBack)
            {
                ReadQueryString();
                SetJavascriptAttributes();
                FillQuestionListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bind the data to question list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwQuestionDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iQuestionId = Convert.ToInt32(lstvwQuestionDetails.DataKeys[e.Item.DisplayIndex]["QuestionId"]);

                QuestionDetails oQuestionDetails = e.Item.DataItem as QuestionDetails;

                LinkButton lnkClearFields = e.Item.FindControl("lnkClearFields") as LinkButton;
                lnkClearFields.Attributes.Add("onclick", "if(!ClearAnswerFields('" + lnkClearFields.ClientID + "'));");

                if (oQuestionDetails.AnswerTypeId == 2 && oQuestionDetails.AttachmentPath != string.Empty)
                {
                    Image imgQuestionAttachment = e.Item.FindControl("imgQuestionAttachment") as Image;
                    imgQuestionAttachment.ImageUrl = "../Uploads/OnlineExamImages/" + oQuestionDetails.AttachmentPath;            
                    HtmlTableRow tr = e.Item.FindControl("trQuestionAttachment") as HtmlTableRow;
                    tr.Visible = true;
                }
                else if (oQuestionDetails.AnswerTypeId == 3)
                {
                    HtmlTableRow tr1 = e.Item.FindControl("trAnswerDetails") as HtmlTableRow;
                    tr1.Visible = false;
                }
                FillAnswerListView(e, iQuestionId);

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bind answers to selcific question.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAnswerDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListView lstAnswerDetails = sender as ListView;
                int iAnswerId = Convert.ToInt32(lstAnswerDetails.DataKeys[e.Item.DisplayIndex]["AnswerId"]);
                int iUserSelectedAnswer = Convert.ToInt32(lstAnswerDetails.DataKeys[e.Item.DisplayIndex]["UserSelectedAnswer"]);
                RadioButton rdoCorrectAnswer = e.Item.FindControl("rdoCorrectAnswer") as RadioButton;
                rdoCorrectAnswer.CssClass = "Answer" + hidQuestionId.Value;
                rdoCorrectAnswer.Attributes.Add("onclick", "if(!CheckSelected('" + rdoCorrectAnswer.ClientID + "', '" + e.Item.DisplayIndex + "'));");

                AnswerDetails oAnswerDetails = e.Item.DataItem as AnswerDetails;

                int iAnswerTypeId = mlstQuestionDetails.Where(qd => qd.QuestionId == oAnswerDetails.QuestionID).Select(qd => qd.AnswerTypeId).FirstOrDefault();

                Label lblAnswer = e.Item.FindControl("lblAnswer") as Label;
                Image imgAttachment = e.Item.FindControl("imgAttachment") as Image;
               

             
                if (iAnswerTypeId == 2)
                {
                    
                    imgAttachment.ImageUrl = "../Uploads/OnlineExamImages/" + oAnswerDetails.AttachmentPath;
                    imgAttachment.Visible = true;
                    lblAnswer.Visible = false;
                   
                   
                }
                else if (iAnswerTypeId == 1)
                {
                   
                    imgAttachment.Visible = false;
                    lblAnswer.Visible = true;                    
                }
                else if (iAnswerTypeId == 3)
                {
                  
                    imgAttachment.Visible = false;
                    lblAnswer.Visible = false;
                    rdoCorrectAnswer.Visible = false;
                   
                    if (oAnswerDetails.DescriptionFileName != string.Empty)
                    {
                       
                        btnView.Visible = true;                        
                        hidDescriptionFilePath.Value = oAnswerDetails.DescriptionFileName;
                        string sNewFileName = S_FOLDER_PATH + oAnswerDetails.DescriptionFileName;
                        btnView.Attributes.Add("onclick", "OpenWindow('" + sNewFileName + "'); return false;");
                       
                    }
                }
               
                if (iAnswerId == iUserSelectedAnswer)               
                    rdoCorrectAnswer.Checked = true;
              
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save the records in to database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            moOnlineExamWiseQueConfigBL = new OnlineExamWiseQueConfigBL(miSchoolId, miAcademicYearId);
            SaveDetails();
            FillQuestionListView();
            base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
            btnSubmit.Enabled = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit the student Exam.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            moOnlineExamWiseQueConfigBL = new OnlineExamWiseQueConfigBL(miSchoolId, miAcademicYearId);
            int iStandardId = Session[Constants.S_SESSION_STUDENT_STANDERED_ID].ToInt();
            moOnlineExamWiseQueConfigBL.SubmitStudentOnlineExam(iStandardId, hidStdDivionId.Value.ToInt(), hidSubjectId.Value.ToInt(), hidExamId.Value.ToInt(), hidStudentId.Value.ToInt());
            base.DisplayMessage(S_SUBMIT_MESSAGE, false, tdMessage);
            FillQuestionListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event is used to tick event of timer control.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        try
        {
           
            if (btnSave.Enabled)
            {
                Timer1.Enabled = false;

                btnSave.Enabled = false;
                btnSave_Click(sender, e);
                
                btnSubmit_Click(sender, e);
                btnSubmit.Enabled = false;

                base.DisplayMessage("Online Exam time out. All answers are auto saved and submitted!!!", false, tdMessage);

                btnClear.Enabled = false;
            }


            //DateTime dt1 = hidExamEndTime.Value.ToDateTime();
            //DateTime dt2 = DateTime.Now.ToDateTime();
            //TimeSpan timespan = dt1.Subtract(dt2);                       
            //lblExamTime.Text = "Time Left = " + timespan.Hours + " : " + timespan.Minutes + " : " + timespan.Seconds;

            //if (timespan.Hours == 0 && timespan.Minutes == 0 && timespan.Seconds == 0)
            //{
            //    Timer1.Enabled = false;
                
            //    btnSave_Click(sender, e);
            //    btnSave.Enabled = false;

            //    btnSubmit_Click(sender, e);
            //    btnSubmit.Enabled = false;

            //    btnClear.Enabled = false;                
            //}
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear all the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ClearSelection();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }    

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to save the details.
    /// </summary>
    private void SaveDetails()
    {
        List<StudentQuestionAnswerDetails> lstStudentQuestionAnswerDetails = new List<StudentQuestionAnswerDetails>();
        int iTotalMarks = Constants.I_ZERO;
        int iMarkScored = Constants.I_ZERO;

        foreach (ListViewItem item in lstvwQuestionDetails.Items)
        {            
            int iQuestionId = lstvwQuestionDetails.DataKeys[item.DisplayIndex]["QuestionId"].ToInt();
            int iMarks = lstvwQuestionDetails.DataKeys[item.DisplayIndex]["Marks"].ToInt();
            int iAnswerypeId = lstvwQuestionDetails.DataKeys[item.DisplayIndex]["AnswerTypeId"].ToInt();
            System.Web.UI.HtmlControls.HtmlTableRow oHtmlTableRow = item.FindControl("trAnswerDetails") as System.Web.UI.HtmlControls.HtmlTableRow;
            System.Web.UI.HtmlControls.HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdAnswerDetails") as System.Web.UI.HtmlControls.HtmlTableCell;
            ListView lstvwAnswerDetails = oHtmlTableCell.FindControl("lstvwAnswerDetails") as ListView;

            foreach (ListViewItem Answers in lstvwAnswerDetails.Items)
            {
                int iAnswerId = lstvwAnswerDetails.DataKeys[Answers.DisplayIndex]["AnswerId"].ToInt();
                bool bIsCorrectAnswer = lstvwAnswerDetails.DataKeys[Answers.DisplayIndex]["IsCorrectAnswer"].ToBool();
                RadioButton rdoCorrectAnswer = Answers.FindControl("rdoCorrectAnswer") as RadioButton;

                if (iAnswerypeId == 3)
                {
                    
                    string sFileName = string.Empty;
                    if (fuDescriptionAnswer.HasFile)
                    {
                        string sFolderName = base.BasePath + S_UPLOAD_FILE_FOLDER_PATH;
                        string sServerFilePath = sFolderName + fuDescriptionAnswer.FileName;
                        sFileName = fuDescriptionAnswer.FileName;
                        if (File.Exists(sServerFilePath))
                        {
                            sFileName = CommonUtility.GetFileNameForRenaming(fuDescriptionAnswer.FileName);
                            sServerFilePath = sFolderName + sFileName;
                        }
                        fuDescriptionAnswer.SaveAs(sServerFilePath);
                    }
                    else if (hidDescriptionFilePath.Value != "")
                        sFileName = hidDescriptionFilePath.Value;
                    

                    StudentQuestionAnswerDetails oStudentQuestionAnswerDetails = new StudentQuestionAnswerDetails();
                    oStudentQuestionAnswerDetails.QuestionId = iQuestionId;
                    oStudentQuestionAnswerDetails.AnswerId = iAnswerId;
                    oStudentQuestionAnswerDetails.DescriptionFileName = sFileName;

                    lstStudentQuestionAnswerDetails.Add(oStudentQuestionAnswerDetails);
                }
                else
                {                   
                    if (rdoCorrectAnswer.Checked)
                    {
                        StudentQuestionAnswerDetails oStudentQuestionAnswerDetails = new StudentQuestionAnswerDetails();

                        oStudentQuestionAnswerDetails.QuestionId = iQuestionId;
                        oStudentQuestionAnswerDetails.AnswerId = iAnswerId;

                        if (bIsCorrectAnswer)
                            iMarkScored = iMarkScored + iMarks;

                        lstStudentQuestionAnswerDetails.Add(oStudentQuestionAnswerDetails);
                        
                    }
                }
            }

            iTotalMarks = iTotalMarks + iMarks;
        }
        

        int iStandardId = Session[Constants.S_SESSION_STUDENT_STANDERED_ID].ToInt();
        moOnlineExamWiseQueConfigBL.SaveStudentQuestionAnswerDetails(iStandardId, hidStdDivionId.Value.ToInt(), hidSubjectId.Value.ToInt(), hidExamId.Value.ToInt(), hidStudentId.Value.ToInt(), base.GenerateXml(lstStudentQuestionAnswerDetails), hidStudentId.Value.ToInt(), iMarkScored, iTotalMarks);
    }

    /// <summary>
    /// This method is used to fill the list view of question details.
    /// </summary>
    private void FillQuestionListView()
    {     
        int iStandardId = Session[Constants.S_SESSION_STUDENT_STANDERED_ID].ToInt();
        mlstQuestionDetails = moOnlineExamWiseQueConfigBL.GetQuestionsForOnlineExam(iStandardId, hidStdDivionId.Value.ToInt(), hidSubjectId.Value.ToInt(), hidExamId.Value.ToInt(), hidStudentId.Value.ToInt());

        lblExam.Text = moOnlineExamWiseQueConfigBL.OnlineExamConfiguration.Exam;
        lblSubject.Text = moOnlineExamWiseQueConfigBL.OnlineExamConfiguration.Subject;

        if (moOnlineExamWiseQueConfigBL.OnlineExamConfiguration.StartDateAndTime.Date != moOnlineExamWiseQueConfigBL.OnlineExamConfiguration.EndDateAndTime.Date)
            lblDateTime.Text = moOnlineExamWiseQueConfigBL.OnlineExamConfiguration.StartDateAndTime.ToString(Constants.S_DATE_FORMAT + " " + "hh:mm tt") + " to " + moOnlineExamWiseQueConfigBL.OnlineExamConfiguration.EndDateAndTime.ToString(Constants.S_DATE_FORMAT + " " + "hh:mm tt");
        else
            lblDateTime.Text = moOnlineExamWiseQueConfigBL.OnlineExamConfiguration.StartDateAndTime.ToString(Constants.S_DATE_FORMAT + " " + "hh:mm tt") + " to " + moOnlineExamWiseQueConfigBL.OnlineExamConfiguration.EndDateAndTime.ToString("hh:mm tt");


        if (mlstQuestionDetails.Count > Constants.I_ZERO)
        {
            lstvwQuestionDetails.DataSource = mlstQuestionDetails;
            lstvwQuestionDetails.DataBind();

            if (mlstQuestionDetails.Any(aa => aa.IsExamSaved))
                btnSubmit.Enabled = true;

            if (mlstQuestionDetails.Any(ss => ss.IsExamSubmited))
            {
                btnSubmit.Enabled = false;
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                lstvwQuestionDetails.Enabled = false;
            }

            if (btnSave.Enabled)
            {
                var diff = moOnlineExamWiseQueConfigBL.OnlineExamConfiguration.EndDateAndTime.Subtract(DateTime.Now);
                if (diff.TotalMilliseconds > 0)
                {
                    Timer1.Interval = diff.TotalMilliseconds.ToInt();
                    Timer1.Enabled = true;
                }
                else
                    Timer1.Enabled = false;
            }
            else
                Timer1.Enabled = false;

            if (mlstQuestionDetails.Any(ss => ss.AnswerTypeId == 3))
            {
                trDescription.Visible = true;
              hidAnswerTypeId.Value = "3";
              
            }
         
        }      
    }

    /// <summary>
    /// This method is used to fill answer list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="iQuestionId"></param>
    private void FillAnswerListView(ListViewItemEventArgs e, int iQuestionId)
    {
        hidQuestionId.Value = iQuestionId.ToString();
        List<AnswerDetails> lstAnswerDetails = moOnlineExamWiseQueConfigBL.AnswerDetails.Where(ad => ad.QuestionID == iQuestionId).ToList();
        if (lstAnswerDetails.Count > Constants.I_ZERO)
        {
            System.Web.UI.HtmlControls.HtmlTableRow oHtmlTableRow = e.Item.FindControl("trAnswerDetails") as System.Web.UI.HtmlControls.HtmlTableRow;
            System.Web.UI.HtmlControls.HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdAnswerDetails") as System.Web.UI.HtmlControls.HtmlTableCell;
            ListView lstvwAnswerDetails = oHtmlTableCell.FindControl("lstvwAnswerDetails") as ListView;
            lstvwAnswerDetails.DataSource = lstAnswerDetails;
            lstvwAnswerDetails.DataBind();
        }
      
    }

    /// <summary>
    /// This method is used to Read query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["StandardDivisionId"] != null)
            hidStdDivionId.Value = QueryString["StandardDivisionId"];

        if (QueryString["ExamId"] != null)
            hidExamId.Value = QueryString["ExamId"];

        if (QueryString["SubjectId"] != null)
            hidSubjectId.Value = QueryString["SubjectId"];

        if (QueryString["StartTime"] != null)
            hidExamStartTime.Value = QueryString["StartTime"];

        if (QueryString["EndTime"] != null)
            hidExamEndTime.Value = QueryString["EndTime"];

        if(QueryString["StudentId"] != null && QueryString["StudentId"].ToString() != string.Empty)
            hidStudentId.Value = QueryString["StudentId"].ToString();
        else if(moUserRole == Constants.UserRoles.Student)
            hidStudentId.Value = Session[Constants.S_SESSION_STUDENT_ID].ToString();

        //hidStudentId.Value = "3229";
    }

    /// <summary>
    /// This method is used to clear all controls.
    /// </summary>
    private void ClearSelection()
    {
        foreach (ListViewItem item in lstvwQuestionDetails.Items)
        {   
            System.Web.UI.HtmlControls.HtmlTableRow oHtmlTableRow = item.FindControl("trAnswerDetails") as System.Web.UI.HtmlControls.HtmlTableRow;
            System.Web.UI.HtmlControls.HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdAnswerDetails") as System.Web.UI.HtmlControls.HtmlTableCell;
            ListView lstvwAnswerDetails = oHtmlTableCell.FindControl("lstvwAnswerDetails") as ListView;

            foreach (ListViewItem Answers in lstvwAnswerDetails.Items)
            {
                RadioButton rdoCorrectAnswer = Answers.FindControl("rdoCorrectAnswer") as RadioButton;

                rdoCorrectAnswer.Checked = false;
            }
        }
    }

    /// <summary>
    /// This method is used to set java script attributes to controls.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnClear, btnSubmit });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnBack.PostBackUrl = "OnlineExamDetailsUI.aspx?"+CommonUtility.EncryptQuerystring("ExamId="+hidExamId.Value);
        hidAreYouSureYouWantDeleteEvent.Value = Resources.LocalizedResources.AreYouSureYouWantDeleteEvent;
    }

    #endregion
}