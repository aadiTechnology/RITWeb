using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class UserSurveyDetailsUI : SchoolBase
{
    #region Constant(s)

    private const string S_SAVE_MESSAGE = "Feedback details saved successfully !!!";
    private const string S_SUBMIT_MESSAGE = "Feedback details submitted successfully !!!"; 

    #endregion

    #region Data Member(s)

    private UserSurveyBL moUserSurveyBL;
    private List<UserSurveyDetails> mlstUsers;

    #endregion

    #region Property(s)

    public bool AllowDataEditing
    {
        get { return moUserSurveyBL.ButtonStates.EnableSaveButton; }
    }

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill feedback details.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            moUserSurveyBL = new UserSurveyBL(miSchoolId, miAcademicYearId, miUserId);

            if (Page.Request.Params.Get("__EVENTTARGET") != null)
            {
                if (btnSave.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                     btnSubmit.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                    FillSurveyQuestionDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display survey details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetJavascriptAttributes();
            FillSurveyQuestionDetails();
        }
    }

    /// <summary>
    /// This event is used to save survey details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Save();
        base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
        FillSurveyQuestionDetails();
    }

    /// <summary>
    /// This event is used to submit survey details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {   
        Submit();
        base.DisplayMessage(S_SUBMIT_MESSAGE, false, tdMessage);
        FillSurveyQuestionDetails();
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to submit survey details.
    /// </summary>
    private void Submit()
    {
        int iUserId = hidUserId.Value.ToInt();
        int iSurveyId = hidSurveyId.Value.ToInt();
        moUserSurveyBL.Submit(iUserId, iSurveyId);
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnBack, btnSave, btnSubmit });
        btnSubmit.Attributes.Add("onclick", "if(!ConfirmSubmit()) return false;");
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidUserId.Value = QueryString["UserId"].ToString();
        hidSurveyId.Value = QueryString["SurveyId"].ToString();

        hidUserRoleId.Value = QueryString["UserRoleId"].ToString();
        hidFilter.Value = QueryString["Filter"].ToString();

        btnBack.PostBackUrl = "SurveyUserListUI.aspx?" + CommonUtility.EncryptQuerystring("SurveyId=" + hidSurveyId.Value + "&UserId=" + hidUserId.Value + "&Filter=" + hidFilter.Value + "&UserRoleId="+hidUserRoleId.Value);
    }

    /// <summary>
    /// This method is used to fill questions.
    /// </summary>
    private void FillSurveyQuestionDetails()
    {
        int iUserId = hidUserId.Value.ToInt();
        int iSurveyId = hidSurveyId.Value.ToInt();

        if (IsPostBack)
        {
            iUserId = Convert.ToInt32(Request.Params[hidUserId.ClientID.Replace("_", "$")]);
            iSurveyId = Convert.ToInt32(Request.Params[hidSurveyId.ClientID.Replace("_", "$")]);
        }
        else
        {
            iUserId = hidUserId.Value.ToInt();
            iSurveyId = hidSurveyId.Value.ToInt();
        }

        mlstUsers = moUserSurveyBL.GetUserSurveyDetails(iSurveyId, iUserId);

        tblQuestions.Rows.Clear();

        FillSchoolDetails();
        AddBlankRow();
        FillSurveyHeader();
        AddLine();
        FillStudentInfo();
        AddLine();
        FillSurveyQuestions();
        SetButtonState();
    }

    /// <summary>
    /// This method is used to set button state.
    /// </summary>
    private void SetButtonState()
    {
        btnSave.Enabled = moUserSurveyBL.ButtonStates.EnableSaveButton;
        btnSubmit.Enabled = moUserSurveyBL.ButtonStates.EnableSubmitButton;
    }

    /// <summary>
    /// This method is used to fill survey questions.
    /// </summary>
    private void FillSurveyQuestions()
    {
        int iSrNo = 1;
        moUserSurveyBL.SurveyQuestions.Where(qt => qt.ParentQuestionId == 0).OrderBy(qt => qt.SortOrder).ToList().ForEach
            (
                question =>
                {
                    HtmlTableRow trQuestion = new HtmlTableRow();
                    base.AddCell(trQuestion, "Q-" + iSrNo, "ClsSurveyHeader", "left", 1, "font-weight:bold");
                    base.AddCell(trQuestion, question.Title, "ClsSurveyHeader", "left", 2, "font-weight:bold");
                    tblQuestions.Rows.Add(trQuestion);

                    FillAnswers(iSrNo, question.SurveyGroupId, question.AllowFreeText, question.Id);

                    moUserSurveyBL.SurveyQuestions.Where(qt => qt.ParentQuestionId == question.Id).OrderBy(qt => qt.SortOrder).ToList().ForEach
                        (
                            chd =>
                            {
                                HtmlTableRow trChileQuestion = new HtmlTableRow();
                                base.AddCell(trChileQuestion, string.Empty, "ClsSurveyCell", "left", 1, "font-weight:bold");
                                base.AddCell(trChileQuestion, chd.Title, "ClsSurveyChildHeader", "left", 2, "font-weight:bold");
                                tblQuestions.Rows.Add(trChileQuestion);

                                FillAnswers(iSrNo, chd.SurveyGroupId, question.AllowFreeText, chd.Id);
                            }

                        );

                    AddBlankRow();
                    iSrNo++;
                }

            );
    }

    /// <summary>
    /// This method is used to set school details.
    /// </summary>
    private void FillSchoolDetails()
    {
        HtmlTableRow trSchoolDetails = new HtmlTableRow();
        base.AddCell(trSchoolDetails, moUserSurveyBL.UserDetails.SchoolName, "ClsSurveySchoolHead", "center", 3, "font-weight:bold");
        tblQuestions.Rows.Add(trSchoolDetails);

        trSchoolDetails = new HtmlTableRow();
        base.AddCell(trSchoolDetails, moUserSurveyBL.UserDetails.SchoolAddress, "ClsSurveyCell", "center", 3, "font-weight:bold");
        tblQuestions.Rows.Add(trSchoolDetails);
    }

    /// <summary>
    /// This method is used to set survey headers.
    /// </summary>
    private void FillSurveyHeader()
    {
        moUserSurveyBL.SurveyHeaders.OrderBy(sh => sh.SortOrder).ToList()
            .ForEach
            (
                sh =>
                {
                    HtmlTableRow trHeader = new HtmlTableRow();
                    base.AddCell(trHeader, sh.Header, string.Empty, "left", 3, "text-align:Justified;font-family:Tahoma;line-height:22px");
                    tblQuestions.Rows.Add(trHeader);
                }

            );
    }

    /// <summary>
    /// This method is used to add blank row.
    /// </summary>
    private void AddBlankRow()
    {
        HtmlTableRow trHeader = new HtmlTableRow();
        base.AddCell(trHeader, string.Empty, string.Empty, "center", 3, "height:10px");
        tblQuestions.Rows.Add(trHeader);
    }

    /// <summary>
    /// This method is used to add line.
    /// </summary>
    private void AddLine()
    {
        HtmlTableRow trHeader = new HtmlTableRow();
        base.AddCell(trHeader, "<hr />", string.Empty, "center", 3);
        tblQuestions.Rows.Add(trHeader);
    }

    /// <summary>
    /// This method is used to fill student info.
    /// </summary>
    private void FillStudentInfo()
    {
        HtmlTableRow trStudent = new HtmlTableRow();

        HtmlTable oHtmlTable = new HtmlTable();
        oHtmlTable.Width = "100%";

        HtmlTableRow tr = new HtmlTableRow();
        base.AddCell(tr, "Student Name", "clsLabel clsBorderLight", "left", 1, "font-weight:bold;width:100px");
        base.AddCell(tr, ": " + moUserSurveyBL.UserDetails.UserName, "clsLabel", "left", 1, "width:50%");
        base.AddCell(tr, "Class", "clsLabel clsBorderLight", "left", 1, "font-weight:bold;width:100px");
        base.AddCell(tr, ": " + moUserSurveyBL.UserDetails.ClassName, "clsLabel", "left", 1);
        oHtmlTable.Rows.Add(tr);

        tr = new HtmlTableRow();
        base.AddCell(tr, "Parent Name", "clsLabel clsBorderLight", "left", 1, "font-weight:bold;width:100px");
        base.AddCell(tr, ": " + moUserSurveyBL.UserDetails.ParentName, "clsLabel", "left", 1, "width:50%");
        base.AddCell(tr, "Contact No.", "clsLabel clsBorderLight", "left", 1, "font-weight:bold;width:100px");
        base.AddCell(tr, ": " + moUserSurveyBL.UserDetails.MobileNumber, "clsLabel", "left", 1);
        oHtmlTable.Rows.Add(tr);

        tr = new HtmlTableRow();
        base.AddCell(tr, "Email Id", "clsLabel clsBorderLight", "left", 1, "font-weight:bold;width:100px");
        base.AddCell(tr, ": " + moUserSurveyBL.UserDetails.EmailAddress, "clsLabel", "left", 3);
        oHtmlTable.Rows.Add(tr);

        base.AddCell(trStudent, string.Empty, string.Empty, "left", 3, string.Empty, oHtmlTable);
        tblQuestions.Rows.Add(trStudent);
    }

    /// <summary>
    /// This method is used to display answers.
    /// </summary>
    /// <param name="iSrNo"></param>
    /// <param name="aiSurveyGroupId"></param>
    /// <param name="abAllowFreeText"></param>
    /// <param name="aiQuestionId"></param>
    private void FillAnswers(int iSrNo, int aiSurveyGroupId, bool abAllowFreeText, int aiQuestionId)
    {
        moUserSurveyBL.SurveyAnswers.Where(sa => sa.SurveyGroupId == aiSurveyGroupId).ToList().ForEach
            (
                answer =>
                {
                    HtmlTableRow trAnswer = new HtmlTableRow();
                    trAnswer.ID = "tr_" + aiQuestionId + "_" + answer.Id;

                    base.AddCell(trAnswer, string.Empty, "ClsSurveyCell", "left", 1);

                    if (answer.InputControlId == Constants.InputControls.Checkbox.ToInt())
                    {
                        CheckBox oCheckBox = new CheckBox();
                        oCheckBox.ID = "chk_" + aiQuestionId + "_" + answer.Id;

                        if (mlstUsers.Any(usr => usr.AnswerId == answer.Id && usr.QuestionId == aiQuestionId))
                            oCheckBox.Checked = true;
                        else
                            oCheckBox.Checked = false;

                        oCheckBox.Enabled = AllowDataEditing;

                        base.AddCell(trAnswer, string.Empty, "ClsSurveyCell", "center", 1, "width:100px", oCheckBox);
                        base.AddCell(trAnswer, answer.Answer, "ClsSurveyCell", "left", 1);
                    }
                    else if (answer.InputControlId == Constants.InputControls.Textbox.ToInt())
                    {
                        if (!abAllowFreeText)
                        {
                            TextBox oTextBox = AddTextbox(aiQuestionId, answer);
                            base.AddCell(trAnswer, string.Empty, "ClsSurveyCell", "center", 1, "width:100px", oTextBox);
                            base.AddCell(trAnswer, answer.Answer, "ClsSurveyCell", "left", 1);
                        }
                        else
                        {
                            TextBox oTextBox = new TextBox();
                            oTextBox.ID = "txt_" + aiQuestionId + "_" + answer.Id;
                            oTextBox.Attributes.Add("class", "exLrgTextbox");
                            oTextBox.Width = Unit.Percentage(100);
                            oTextBox.Height = Unit.Pixel(50);
                            oTextBox.TextMode = TextBoxMode.MultiLine;

                            if (mlstUsers.Any(usr => usr.AnswerId == answer.Id && usr.QuestionId == aiQuestionId))
                                oTextBox.Text = mlstUsers.Where(usr => usr.AnswerId == answer.Id && usr.QuestionId == aiQuestionId).FirstOrDefault().FreeTextValue;
                            else
                                oTextBox.Text = string.Empty;

                            oTextBox.Enabled = AllowDataEditing;

                            base.AddCell(trAnswer, string.Empty, "ClsSurveyCell", "center", 2, string.Empty, oTextBox);
                        }
                    }
                    else if (answer.InputControlId == Constants.InputControls.RadioButton.ToInt())
                    {
                        RadioButton oRadioButton = new RadioButton();
                        oRadioButton.GroupName = "Group" + iSrNo;
                        oRadioButton.ID = "opt_" + aiQuestionId + "_" + answer.Id;

                        if (mlstUsers.Any(usr => usr.AnswerId == answer.Id && usr.QuestionId == aiQuestionId))
                            oRadioButton.Checked = true;
                        else
                            oRadioButton.Checked = false;

                        oRadioButton.Enabled = AllowDataEditing;

                        base.AddCell(trAnswer, string.Empty, "ClsSurveyCell", "center", 1, "width:100px", oRadioButton);
                        base.AddCell(trAnswer, answer.Answer, "ClsSurveyCell", "left", 1);
                    }
                    else if (answer.InputControlId == Constants.InputControls.CheckboxAndTextbox.ToInt())
                    {
                        CheckBox oCheckBox = new CheckBox();
                        oCheckBox.ID = "chk_" + aiQuestionId + "_" + answer.Id;

                        if (mlstUsers.Any(usr => usr.AnswerId == answer.Id && usr.QuestionId == aiQuestionId))
                            oCheckBox.Checked = true;
                        else
                            oCheckBox.Checked = false;

                        oCheckBox.Enabled = AllowDataEditing;

                        base.AddCell(trAnswer, string.Empty, "ClsSurveyCell", "center", 1, "width:100px", oCheckBox);

                        TextBox oTextBox = new TextBox();
                        oTextBox.ID = "txt_" + aiQuestionId + "_" + answer.Id;
                        oTextBox.Attributes.Add("class", "smlTextbox");
                        oTextBox.Width = Unit.Percentage(100);
                        oTextBox.MaxLength = 300;

                        if (mlstUsers.Any(usr => usr.AnswerId == answer.Id && usr.QuestionId == aiQuestionId))
                            oTextBox.Text = mlstUsers.Where(usr => usr.AnswerId == answer.Id && usr.QuestionId == aiQuestionId).FirstOrDefault().FreeTextValue;
                        else
                            oTextBox.Text = string.Empty;

                        oTextBox.Enabled = AllowDataEditing;

                        base.AddCell(trAnswer, answer.Answer + " ", "ClsSurveyCell", "left", 1, string.Empty, oTextBox);
                    }

                    tblQuestions.Rows.Add(trAnswer);
                }

            );
    }

    /// <summary>
    /// This method is used to add textbox.
    /// </summary>
    /// <param name="aiQuestionId"></param>
    /// <param name="aoAnswer"></param>
    /// <returns></returns>
    private TextBox AddTextbox(int aiQuestionId, SurveyAnswer aoAnswer)
    {
        TextBox oTextBox = new TextBox();
        oTextBox.ID = "txt_" + aiQuestionId + "_" + aoAnswer.Id;
        oTextBox.Attributes.Add("class", "smlTextbox");
        oTextBox.Attributes.Add("onkeypress", "return blockNonNumbers (this, event, true, false);");
        oTextBox.Attributes.Add("onblur", "extractNumber(this,2,false)");
        oTextBox.Attributes.Add("onkeyup", "extractNumber(this,2,false)");
        oTextBox.Style.Add("text-align", "right");
        oTextBox.Style.Add("padding-right", "5px");
        oTextBox.Attributes.Add("onpaste", "event.returnValue=false");
        oTextBox.Attributes.Add("ondrop", "event.returnValue=false");
        oTextBox.Width = Unit.Pixel(50);
        oTextBox.MaxLength = 3;

        if (mlstUsers.Any(usr => usr.AnswerId == aoAnswer.Id && usr.QuestionId == aiQuestionId))
            oTextBox.Text = mlstUsers.Where(usr => usr.AnswerId == aoAnswer.Id && usr.QuestionId == aiQuestionId).FirstOrDefault().FreeTextValue;
        else
            oTextBox.Text = string.Empty;

        oTextBox.Enabled = AllowDataEditing;
        return oTextBox;
    }

    /// <summary>
    /// This method is used to save survey details.
    /// </summary>
    private void Save()
    {
        int iUserId = hidUserId.Value.ToInt();
        int iSurveyId = hidSurveyId.Value.ToInt();
        string sXml = Populate();
        moUserSurveyBL.Save(iUserId, iSurveyId, sXml);
    }

    /// <summary>
    /// This method is used to populate survey details.
    /// </summary>
    /// <returns></returns>
    private string Populate()
    {
        List<UserSurveyDetails> lstUsers = new List<UserSurveyDetails>();
        foreach (HtmlTableRow tr in tblQuestions.Rows)
        {
            UserSurveyDetails oUserSurveyDetails = new UserSurveyDetails();
            if (tr.ID != null)
            {
                string sSuffix = tr.ID.Substring(3);
                int iQuestionId = sSuffix.Substring(0, sSuffix.IndexOf('_')).ToInt();
                int iAnswerId = sSuffix.Substring(sSuffix.IndexOf('_') + 1).ToInt();

                oUserSurveyDetails.QuestionId = iQuestionId;
                oUserSurveyDetails.AnswerId = 0;
                oUserSurveyDetails.FreeTextValue = string.Empty;

                CheckBox chk = tr.FindControl("chk_" + sSuffix) as CheckBox;
                if (chk != null)
                {
                    if (chk.Checked)
                        oUserSurveyDetails.AnswerId = iAnswerId;
                }

                TextBox txt = tr.FindControl("txt_" + sSuffix) as TextBox;
                if (txt != null)
                {
                    txt.Text = txt.Text.Trim();
                    if (chk != null)
                    {
                        if (chk.Checked || txt.Text != string.Empty)
                        {
                            oUserSurveyDetails.AnswerId = iAnswerId;
                            oUserSurveyDetails.FreeTextValue = txt.Text;
                        }
                    }
                    else
                    {
                        if (txt.Text != string.Empty)
                        {
                            oUserSurveyDetails.AnswerId = iAnswerId;
                            oUserSurveyDetails.FreeTextValue = txt.Text;
                        }
                    }
                }

                RadioButton opt = tr.FindControl("opt_" + sSuffix) as RadioButton;
                if (opt != null)
                {
                    if (opt.Checked)
                        oUserSurveyDetails.AnswerId = iAnswerId;
                }

                if (oUserSurveyDetails.AnswerId != 0)
                    lstUsers.Add(oUserSurveyDetails);
            }
        }

        return base.GenerateXml(lstUsers);
    }
    #endregion
}