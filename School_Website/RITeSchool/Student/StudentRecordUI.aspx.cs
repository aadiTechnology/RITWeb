/* File Name - StudentRecordUI.aspx.cs
 * Created By - Sachin
 * Created Date - 4-Jun-2018
 * Description - This class is used to save /submit student record.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class StudentRecordUI : SchoolBase
{
    #region Constant(s)

    private const string S_SUBMIT_MSG = "Student record submitted successfully !!!";
    private const string S_SAVE_MSG = "Student record saved successfully !!!";
    private const string S_MARK_AS_READ_MSG = "Student record(s) are marked as read successfully !!!";
    private const string S_COMMENT_SUBMIT_MSG = "Comment is submitted successfully !!!";
    private const string S_TIME_FORMAT = "hh:mm tt";
 
    #endregion

    #region Data Member(s)

    private StudentRecordBL moStudentRecordBL;
    private StudentDataCollction moStudentDataCollction;

    #endregion

    #region Property(s)

    private bool IsReadMode
    {
        get
        {
            return hidIsReadMode.Value == Constants.S_ONE;
        }
    }

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to load student details.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            moStudentRecordBL = new StudentRecordBL(miSchoolId, miAcademicYearId, miUserId);

            if (Page.Request.Params.Get("__EVENTTARGET") != null)
            {
                if (btnSave.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                     btnSubmit.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                    FillStudentRecords();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display student record.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStudentRecordBL = new StudentRecordBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {   
                ReadQuerystring();
                SetDefaultValues();
                FillStudentRecords();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit student record.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
      {
        try
        {
            moStudentRecordBL.Submit(hidStudentId.Value.ToInt(), 0, false);
            lblMessage.Text = S_SUBMIT_MSG;
            FillStudentRecords();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to mark details as read.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRead_Click(object sender, EventArgs e)
    {
        
        try
        {
            moStudentRecordBL.MarkAsRead(hidStudentId.Value.ToInt());
            lblMessage.Text = S_MARK_AS_READ_MSG;
            FillStudentRecords();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit comment.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmitComment_Click(object sender, EventArgs e)
    {
        try
        {
            moStudentRecordBL.Submit(hidStudentId.Value.ToInt(), 0, true);
            lblMessage.Text = S_COMMENT_SUBMIT_MSG;
            FillStudentRecords();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save student record.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            List<KeyValue> lstKeys = new List<KeyValue>();
            foreach (HtmlTableRow tr in tblSections.Rows)
            {
                if (tr.ID != null && tr.ID.Contains("tr"))
                {
                    int iSectionId = tr.ID.Substring(tr.ID.IndexOf("_") + 1).ToInt();
                    HtmlTableCell td = tr.FindControl("tdFortblParameters_" + iSectionId) as HtmlTableCell;
                    if (td != null)
                    {
                        HtmlTable tblParameters = td.FindControl("tblParameters_" + iSectionId) as HtmlTable;
                        if (tblParameters != null)
                        {

                            foreach (HtmlTableRow trParameter in tblParameters.Rows)
                            {
                                if (trParameter != null && trParameter.ID != null)
                                {
                                    int iParameterId = trParameter.ID.Substring(trParameter.ID.IndexOf("_") + 1).ToInt();
                                    HtmlTableCell tdParameter = trParameter.FindControl("tdParameter_" + iParameterId) as HtmlTableCell;
                                    if (tdParameter != null)
                                    {
                                        KeyValue oKeyValue = new KeyValue();
                                        oKeyValue.Key = iParameterId;

                                        TextBox oTextBox = tdParameter.FindControl("txtParameter_" + iParameterId) as TextBox;
                                        if (oTextBox != null)
                                            oKeyValue.Value = oTextBox.Text.Trim();
                                        else
                                        {
                                            RadioButton optYes = tdParameter.FindControl("optYes_" + iParameterId) as RadioButton;
                                            if (optYes.Checked)
                                                oKeyValue.Value = Constants.S_ONE;
                                            else
                                                oKeyValue.Value = Constants.S_ZERO;
                                        }

                                        lstKeys.Add(oKeyValue);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (lstKeys.Count > 0)
            {
                string sData = base.GenerateXml(lstKeys);
                moStudentRecordBL.Save(hidStudentId.Value.ToInt(), sData, Convert.ToDateTime(txtDate.Text + " " + DateTime.Now.ToString(S_TIME_FORMAT)));
                lblMessage.Text = S_SAVE_MSG;
                FillStudentRecords();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)
    
    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        string sStdDivId = QueryString["StdDivId"].ToString();
        string sShowOnlySavedRecord = QueryString["ShowOnlySavedRecord"].ToString();
        txtDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);

        btnBack.PostBackUrl = "StudentRecordStatusUI.aspx?" + CommonUtility.EncryptQuerystring("SchoolwiseStudentId=" + hidStudentId.Value + "&StdDivId=" + sStdDivId + "&Filter=" + hidFilter.Value + "&ShowOnlySavedRecord=" + sShowOnlySavedRecord + "&ShowOnlyRiseAndShine=" + hidFilterIsRiseAndShinde.Value);
        ValSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnAddComment.Attributes.Add("onclick", "ShowPopup(" + hidStudentId.Value + "," + 0 + "," + hidIsReadMode.Value + "," + hidIsPrincipal.Value + "," + hidIsCounsellor.Value + "," + hidIsClassTeacher.Value + "," + sStdDivId + "," + sShowOnlySavedRecord + "); return false;");
    }

    /// <summary>
    /// This method is used to read querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        hidStudentId.Value = QueryString["SchoolwiseStudentId"].ToString();
        hidIsReadMode.Value = QueryString["IsReadMode"].ToString();
        hidIsPrincipal.Value = QueryString["IsPrincipal"].ToString();
        hidIsCounsellor.Value = QueryString["IsCounsellor"].ToString();
        hidFilter.Value = QueryString["Filter"].ToString();
        if (QueryString["ShowOnlyRiseAndShine"] != null)
            hidFilterIsRiseAndShinde.Value = QueryString["ShowOnlyRiseAndShine"].ToString();
        if (QueryString["IsSubjectTeacher"] != null)
            hidIsSubjectTeacher.Value = QueryString["IsSubjectTeacher"].ToString();
        if (QueryString["IsClassTeacher"] != null && QueryString["IsClassTeacher"] != string.Empty && QueryString["IsClassTeacher"] != "-1" && QueryString["IsClassTeacher"] != "0")
            hidIsClassTeacher.Value = Constants.S_ONE;
        else
            hidIsClassTeacher.Value = Constants.S_ZERO;
    }

    /// <summary>
    /// This method is used to fill student details.
    /// </summary>
    private void FillStudentRecords()
    {
        int iStudentId = 0;

        if (!IsPostBack)
            iStudentId = hidStudentId.Value.ToInt();
        else
            iStudentId = Convert.ToInt32(Request.Params[hidStudentId.ClientID.Replace("_", "$")]);

        moStudentDataCollction = moStudentRecordBL.GetAllStudentRecords(iStudentId, IsReadMode);
        SetStudentBasicDetails();
        AddSiblingDetails();
        FillSections();
        FilComments();
        SetButtonState();
    }

    /// <summary>
    /// This method is used to set button state.
    /// </summary>
    private void SetButtonState()
    {
        var oComment = moStudentDataCollction.StudentRecordComments.Where(cmnt => cmnt.IsDefaultComment).FirstOrDefault();
        var oIsSubmited = moStudentDataCollction.StudentRecordComments.OrderByDescending(ss => ss.Id).FirstOrDefault();              
        
        if (!IsReadMode)
        {
            btnRead.Visible = false;
            btnSave.Visible = true;
            btnSubmit.Visible = true;
            btnSubmitComment.Visible = true;
            btnAddComment.Visible = true;

            if (oComment != null)
            {
                if (!oComment.IsSubmitted)
                    btnSubmit.Enabled = true;
                else
                {
                    btnSave.Enabled = false;
                    btnSubmit.Enabled = false;
                }

                if (moStudentDataCollction.StudentRecordComments.Any(assd => !assd.IsSubmitted))
                    btnAddComment.Enabled = false;                     
                else
                    btnAddComment.Enabled = true;                

                var oSubmitComment = moStudentDataCollction.StudentRecordComments.Where(cmnt => !cmnt.IsDefaultComment && !cmnt.IsSubmitted).FirstOrDefault();
                if (oSubmitComment == null)
                    btnSubmitComment.Enabled = false;
                else
                    btnSubmitComment.Enabled = true;                
               
                btnRead.Visible = true;
                btnRead.Enabled = false;

                var oSubmitStatus = moStudentDataCollction.StudentRecordComments.Where(cmnt => !cmnt.IsSubmitted).FirstOrDefault();                

                if ((hidIsClassTeacher.Value == Constants.S_ONE) && (moStudentDataCollction.StudentRecordComments.Any(cmnts => !cmnts.IsCommentReadByClassTeacher)))
                {                       
                    btnRead.Enabled = true;          
                }
                else if (hidIsCounsellor.Value == Constants.S_ONE || hidIsPrincipal.Value == Constants.S_ONE)
                {
                    if (oSubmitStatus == null && ((hidIsPrincipal.Value == Constants.S_ONE && moStudentDataCollction.StudentRecordComments.Any(cmnt => !cmnt.IsCommentReadByPrincipal)) ||
                    (hidIsCounsellor.Value == Constants.S_ONE && moStudentDataCollction.StudentRecordComments.Any(cmnt => !cmnt.IsCommentReadByConsellor))))
                    {
                        btnRead.Enabled = true;
                    }
                }
            }
        }
        else
        {
            btnSave.Visible = false;
            btnSubmit.Visible = false;
         
            var oSubmitComment = moStudentDataCollction.StudentRecordComments.Where(cmnt => !cmnt.IsDefaultComment && !cmnt.IsSubmitted).FirstOrDefault();
            if (oSubmitComment == null)
            {
                btnSubmitComment.Enabled = false;
                btnAddComment.Enabled = true;
            }
            else
            {
                btnSubmitComment.Enabled = true;
                btnAddComment.Enabled = false;
            }

            if ((hidIsPrincipal.Value == Constants.S_ONE && moStudentDataCollction.StudentRecordComments.Any(cmnt => !cmnt.IsCommentReadByPrincipal)) ||
                (hidIsCounsellor.Value == Constants.S_ONE && moStudentDataCollction.StudentRecordComments.Any(cmnt => !cmnt.IsCommentReadByConsellor))
                )
            {
                btnRead.Visible = true;
                btnRead.Enabled = true;
            }
            else
            {
                btnRead.Enabled = false;
                if (hidIsPrincipal.Value == Constants.S_ZERO && hidIsClassTeacher.Value == Constants.S_ONE && moStudentDataCollction.StudentRecordComments.Any(cmnt => !cmnt.IsCommentReadByClassTeacher))
                {
                    btnRead.Visible = true;
                    btnRead.Enabled = true;
                }
            }
        }
    }

    /// <summary>
    /// This method is used to fill comments.
    /// </summary>
    private void FilComments()
    {   
        tblComments.Rows.Clear();
        moStudentDataCollction.StudentRecordComments.OrderBy(cm => cm.Date).ToList().ForEach(
            comment =>
            {
                HtmlTable oHtmlTable = new HtmlTable();
                oHtmlTable.Width = "100%";

                HtmlTableRow tr1 = new HtmlTableRow();
                base.AddCell(tr1, "Date : " + comment.Date.ToString(Constants.S_DATE_FORMAT) + " Time : " + comment.Date.ToString(S_TIME_FORMAT), "clsLable", "Left", 1, "background-color: #006179; color: White;");



                Literal ltrl = new Literal();
                ltrl.Text = "Read By Principal : ";

                Literal obj = new Literal();
                obj.Text = "Read By Counsellor : ";

                Literal aObj = new Literal();
                aObj.Text = "Read By Class Teacher : ";

                if (comment.LoginUserDesignation == 1 || comment.LoginUserDesignation == 2 || comment.LoginUserDesignation == 3)
                {
                    if (miUserId == comment.InsertedById)
                    {
                        //aObj.Text = aObj.Text + "-";
                        if (comment.LoginUserDesignation == 1 || comment.LoginUserDesignation == 2)
                        {
                            if (comment.LoginUserDesignation == 2)
                                obj.Text = obj.Text + "Yes";
                            else
                                obj.Text = obj.Text + "No";
                            //ltrl.Text = ltrl.Text + "-";
                        }
                        else
                        {
                            if (comment.IsCommentReadByConsellor)
                                obj.Text = obj.Text + "Yes";
                            else
                                obj.Text = obj.Text + "No";
                        }

                        if (comment.IsCommentReadByPrincipal)
                            ltrl.Text = ltrl.Text + "Yes";
                        else
                            ltrl.Text = ltrl.Text + "No";
                        
                        if (comment.IsCommentReadByClassTeacher)
                            aObj.Text = aObj.Text + "Yes";
                        else
                            aObj.Text = aObj.Text + "No";
                    }
                    else
                    {
                        if (comment.IsCommentReadByPrincipal)
                            ltrl.Text = ltrl.Text + "Yes";
                        else
                            ltrl.Text = ltrl.Text + "No";

                        if (comment.IsCommentReadByConsellor)
                            obj.Text = obj.Text + "Yes";
                        else
                            obj.Text = obj.Text + "No";

                        if (comment.IsCommentReadByClassTeacher)
                            aObj.Text = aObj.Text + "Yes";
                        else
                            aObj.Text = aObj.Text + "No";
                    }
                }
                else
                {
                    if (comment.IsCommentReadByPrincipal)
                        ltrl.Text = ltrl.Text + "Yes";
                    else
                        ltrl.Text = ltrl.Text + "No";

                    if (comment.IsCommentReadByConsellor)
                        obj.Text = obj.Text + "Yes";
                    else
                        obj.Text = obj.Text + "No";

                    if (comment.IsCommentReadByClassTeacher)
                        aObj.Text = aObj.Text + "Yes";
                    else
                        aObj.Text = aObj.Text + "No";
                }

                Control ctrl = new Control();
                ctrl.Controls.Add(ltrl);
                base.AddCell(tr1, string.Empty, "clsLable", "Left", 1, "background-color: #006179; color: White;", ctrl);

                Control oControl = new Control();
                oControl.Controls.Add(obj);
                base.AddCell(tr1, string.Empty, "clsLable", "Left", 1, "background-color: #006179; color: White;", oControl);

                Control aoControl = new Control();
                aoControl.Controls.Add(aObj);
                base.AddCell(tr1, string.Empty, "clsLable", "Left", 1, "background-color: #006179; color: White;", aoControl);

                ImageButton btnEdit = new ImageButton();
                btnEdit.AlternateText = "Edit";
                btnEdit.ToolTip = "Edit";
                btnEdit.ImageUrl = "../images/IconGrid_Edit.GIF";
                btnEdit.Attributes.Add("onclick", "ShowPopup(" + hidStudentId.Value + "," + comment.Id + "," + hidIsReadMode.Value + "," + hidIsPrincipal.Value + "," + hidIsCounsellor.Value + "," + hidIsClassTeacher.Value + "," + QueryString["StdDivId"].ToString() + "," + QueryString["ShowOnlySavedRecord"].ToString() + "," + QueryString["Filter"].ToString() + "); return false;");

                if (comment.IsDefaultComment || comment.IsSubmitted)
                    btnEdit.Visible = false;
                else
                    btnEdit.Visible = true;

                base.AddCell(tr1, string.Empty, "clsLable", "Center", 1, "background-color: #006179; color: White;", btnEdit);
                oHtmlTable.Rows.Add(tr1);

                if (comment.UserName != string.Empty)
                {
                    HtmlTableRow trBorder = new HtmlTableRow();
                    base.AddCell(trBorder, " ", "clsLable", "left", 6, "border-top:1px solid; border-size:thin; border-color:#006179;");
                    oHtmlTable.Rows.Add(trBorder);

                    HtmlTableRow trAddCommentName = new HtmlTableRow();
                    base.AddCell(trAddCommentName, "<b>Added By :</b> " + comment.UserName, "clsLable", "left", 4, "text-align:justify; padding-right:5px;");
                    oHtmlTable.Rows.Add(trAddCommentName);
                }

                HtmlTableRow trComment = new HtmlTableRow();
                base.AddCell(trComment, "<b>Comment : </b>" + comment.Comment, "clsLable", "left", 4, "text-align:justify; padding-right:5px;");                               
                oHtmlTable.Rows.Add(trComment);

                if (comment.LectureName != string.Empty)
                {
                    HtmlTableRow trBorder = new HtmlTableRow();
                    base.AddCell(trBorder, " ", "clsLable", "left", 5, "border-top:1px solid; border-size:thin; border-color:#006179;");
                    oHtmlTable.Rows.Add(trBorder);

                    HtmlTableRow trLectureName = new HtmlTableRow();
                    base.AddCell(trLectureName, "<b>Lecture Name :</b> " + comment.LectureName, "clsLable", "left", 4, "text-align:justify; padding-right:5px;");
                    oHtmlTable.Rows.Add(trLectureName);
                }
                 

                HtmlTableRow tr = new HtmlTableRow();
                base.AddCell(tr, string.Empty, string.Empty, "left", 1, "border-style:ridge;border-size:thin;border-color:#006179;padding-left:0px;", oHtmlTable);
                tblComments.Rows.Add(tr);

                AddBreak(tblComments);
            }
            );

        if (moStudentDataCollction.StudentRecordComments.Count == 0)
        {
            HtmlTableRow tr = new HtmlTableRow();
            base.AddCell(tr, "No Comment Found.", string.Empty, "Center", 1, "border-style:ridge;border-size:thin;border-color:#006179;padding-left:0px;");
            tblComments.Rows.Add(tr);
        }
    }

    /// <summary>
    /// This method is used to fill sections.
    /// </summary>
    private void FillSections()
    {
        int iSectionCounter = 1;
        tblSections.Rows.Clear();

        moStudentDataCollction.StudentRecordSections.OrderBy(st => st.SortOrder).ToList().ForEach
            (
                section =>
                {
                    HtmlTableRow tr = new HtmlTableRow();
                    tr.ID = "tr_" + section.Id;

                    AddParameters(section.Id,section.Name);

                    AddBreak(tblSections);
                    iSectionCounter++;
                }
            );
    }

    /// <summary>
    /// This method is used to add parameters.
    /// </summary>
    /// <param name="aiSectionId"></param>
    private void AddParameters(int aiSectionId, string asSectionName)
    {
        var oComment = moStudentDataCollction.StudentRecordComments.Where(cmnt => cmnt.IsDefaultComment).FirstOrDefault();

        if (oComment == null)
        {
            cDate.Enabled = true;
            txtDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        }
        else
        {
            cDate.Enabled = false;
            txtDate.Text = oComment.Date.ToString(Constants.S_DATE_FORMAT);
            if (!oComment.IsSubmitted)
                cDate.Enabled = true;
        }
        
        bool bIsSubmited = false;
        if (oComment != null && oComment.IsSubmitted)
            bIsSubmited = true;

        HtmlTable tblParameters = new HtmlTable();
        tblParameters.Width = "100%";
        tblParameters.ID = "tblParameters_" + aiSectionId; ;

        AddBreak(tblParameters);

        int iSrNo = 1;
        moStudentDataCollction.StudentRecordParameters.Where(st => st.SectionId == aiSectionId).OrderBy(st => st.SortOrder).ToList().ForEach
            (
                param =>
                {
                    HtmlTableRow trParam = new HtmlTableRow();
                    HtmlTableCell tdParam = new HtmlTableCell();
                    base.AddCell(trParam, iSrNo.ToString() + ".", "", "Center", 1, "text-align:center;font-weight:700;widtgh:100px;");
                    base.AddCell(trParam, param.Name, "clsLabel", "Center", 1, "text-align:left;font-weight:700;padding-left:0px;");

                    tblParameters.Rows.Add(trParam);

                    HtmlTableRow trParamField = new HtmlTableRow();
                    trParamField.ID = "trParameter_" + param.Id;
                    var oValue = moStudentDataCollction.StudentRecords.Where(rcd => rcd.ParameterId == param.Id).FirstOrDefault();

                    if (param.ControlId == Constants.InputControls.Textbox.ToInt())
                        AddTextField(param, trParamField, (oValue != null ? oValue.Answer : string.Empty), bIsSubmited);
                    else if (param.ControlId == Constants.InputControls.RadioButton.ToInt())
                        AddOptionFields(param, trParamField, (oValue != null ? oValue.Answer : Constants.S_ONE), bIsSubmited);

                    tblParameters.Rows.Add(trParamField);
                    AddBreak(tblParameters);

                    iSrNo++;
                }
            );

        iSrNo = 1;

        eWorld.UI.CollapsablePanel obj = GetCollapsablePanel(aiSectionId, asSectionName);
        obj.Controls.Add(tblParameters);
    
        HtmlTableRow trTable1 = new HtmlTableRow();
        trTable1.ID = "tr_" + aiSectionId;
        base.AddCell(trTable1, string.Empty, string.Empty, "left", 2, "", obj, "tdFortblParameters_" + aiSectionId);
        tblSections.Rows.Add(trTable1);
    }

    private static eWorld.UI.CollapsablePanel GetCollapsablePanel(int aiSectionId, string asSectionName)
    {

        eWorld.UI.CollapsablePanel obj = new eWorld.UI.CollapsablePanel();
        obj.TitleText = asSectionName;
        obj.TitleStyle.CssClass = "CollapsTitle";
        obj.ID = "colapse_" + aiSectionId;

        obj.AllowSliding = true;
        obj.ExpandImageUrl = "../images/node_open.gif";
        obj.CollapseImageUrl = "../images/node_close.gif";
        obj.CollapserAlign = eWorld.UI.HorizontalAlignment.Left;
        obj.TitleStyle.Height = Unit.Pixel(25);
        obj.Collapsed = false;
        obj.SlideSpeed = 25;
        obj.CollapsedTitleStyle.CssClass = "CollapsedTitle";
        obj.BorderStyle = BorderStyle.Solid;
        obj.BorderColor = Color.FromName("#006179");
        obj.BorderWidth = Unit.Pixel(2);
        obj.TitleStyle.BackColor = Color.FromName("#003d55");
        obj.CollapsedTitleStyle.BackColor = Color.FromName("#006179");
        obj.TitleStyle.ForeColor = Color.White;
        obj.CollapsedTitleStyle.ForeColor = Color.White;
        
        return obj;
    }

    /// <summary>
    /// This method is used to set mode.
    /// </summary>
    /// <param name="asFlag"></param>
    private void SetMode(bool asFlag)
    {
        btnSave.Visible = asFlag;
        btnSubmit.Visible = asFlag;
        btnAddComment.Visible = asFlag;
        btnRead.Visible = !asFlag;
    }

    /// <summary>
    /// This method is used to student basic details.
    /// </summary>
    private void SetStudentBasicDetails()
    {
        spnStudentName.InnerText = moStudentDataCollction.StudentBasicInformation.StudentName;
        spnDOB.InnerText = moStudentDataCollction.StudentBasicInformation.DOB.ToString(Constants.S_DATE_FORMAT);
        spnFatherName.InnerText = moStudentDataCollction.StudentBasicInformation.FatherName;
        spnFatherOccuption.InnerText = moStudentDataCollction.StudentBasicInformation.FatherOccupation;
        spnMotherName.InnerText = moStudentDataCollction.StudentBasicInformation.MotherName;
        spnMotherOccuption.InnerText = moStudentDataCollction.StudentBasicInformation.MotherOccupation;
    }

    /// <summary>
    /// This method is used to show sibling details.
    /// </summary>
    private void AddSiblingDetails()
    {
        HtmlTableRow tr1 = new HtmlTableRow();
        HtmlTableCell td1 = new HtmlTableCell();
        
        tblSibling.Rows.Clear();

        base.AddCell(tr1, "Name", "clsLable", "Center", 1, "width:250px;background-color: #006179; color: White;");
        base.AddCell(tr1, "Sex", "clsLable", "Center", 1, "width:100px;background-color: #006179; color: White;");
        base.AddCell(tr1, "Age", "clsLable", "Center", 1, "width:100px;background-color: #006179; color: White;");
        base.AddCell(tr1, "Grade", "clsLable", "Center", 1, "width:100px;background-color: #006179; color: White;");
        tblSibling.Rows.Add(tr1);

        moStudentDataCollction.StudentRecordSiblings.ForEach
            (
                sibling =>
                {
                    HtmlTableRow tr = new HtmlTableRow();
                    HtmlTableCell td = new HtmlTableCell();
                    base.AddCell(tr, sibling.SiblingName, "clsLable");
                    base.AddCell(tr, sibling.Sex.ToString(), string.Empty);
                    base.AddCell(tr, sibling.Age.ToString(), string.Empty);
                    base.AddCell(tr, sibling.Standard, string.Empty);

                    tblSibling.Rows.Add(tr);
                }
            );

        if (moStudentDataCollction.StudentRecordSiblings.Count == 0)
        {
            trSibling.Visible = false;
            trSiblingHeader.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to display option fields.
    /// </summary>
    /// <param name="aoParam"></param>
    /// <param name="atrParamField"></param>
    /// <param name="asAnswer"></param>
    /// <param name="abIsSubmitted"></param>
    private void AddOptionFields(StudentRecordParameter aoParam, HtmlTableRow atrParamField, string asAnswer, bool abIsSubmitted)
    {
        RadioButton optYes = new RadioButton();
        optYes.Text = "Yes";
        optYes.GroupName = "Answer";
        optYes.Width = Unit.Pixel(50);
        optYes.ID = "optYes_" + aoParam.Id;

        RadioButton optNo = new RadioButton();
        optNo.Text = "No";
        optNo.GroupName = "Answer";
        optNo.Width = Unit.Pixel(50);
        optNo.ID = "optNo_" + aoParam.Id;

        if (asAnswer == Constants.S_ZERO)
            optNo.Checked = true;
        else
            optYes.Checked = true;

        if (IsReadMode || abIsSubmitted)
        {
            optYes.Enabled = false;
            optNo.Enabled = false;
        }

        Control ctr = new Control();
        ctr.Controls.Add(optYes);
        ctr.Controls.Add(optNo);
        base.AddCell(atrParamField, string.Empty, "", "left", 1, "padding-left:0px;width:30px;");
        base.AddCell(atrParamField, string.Empty, "", "left", 1, "padding-left:0px;", ctr, "tdParameter_" + aoParam.Id);
    }

    /// <summary>
    /// This method is used to show text fields.
    /// </summary>
    /// <param name="aParam"></param>
    /// <param name="atrParamField"></param>
    /// <param name="asAnswer"></param>
    /// <param name="abIsSubmitted"></param>
    private void AddTextField(StudentRecordParameter aParam, HtmlTableRow atrParamField, string asAnswer, bool abIsSubmitted)
    {
        TextBox oTextBox = new TextBox();
        oTextBox.Width = Unit.Percentage(98);
        oTextBox.TextMode = TextBoxMode.MultiLine;
        oTextBox.Height = Unit.Pixel(120);
        oTextBox.CssClass = "LrgTxtBox";
        oTextBox.ID = "txtParameter_" + aParam.Id;
        oTextBox.Style.Add("background-color", "#fff3e6");

        oTextBox.Text = asAnswer;

        if (IsReadMode || abIsSubmitted)
            oTextBox.Enabled = false;

        base.AddCell(atrParamField, string.Empty, "", "left", 1, "padding-left:0px;width:30px;", null);
        base.AddCell(atrParamField, string.Empty, "", "left", 1, "padding-left:0px;", oTextBox, "tdParameter_" + aParam.Id);
    }

    /// <summary>
    /// This method is used to add some space.
    /// </summary>
    /// <param name="aoTable"></param>
    private void AddBreak(HtmlTable aoTable)
    {
        HtmlTableRow trBreak = new HtmlTableRow();
        HtmlTableCell tdBreak = new HtmlTableCell();
        base.AddCell(trBreak, string.Empty, string.Empty, "Center", 2, "height:20px;");
        aoTable.Rows.Add(trBreak);
    }

    #endregion

    #region Public Method(s)

    [WebMethod]
    public static string GetQueryString(string asStudId, string asCommentId, string asIsReadMode, string asIsPrincipal, string asIsCounsellor, string asIsClassTeacher, string asStdDivId, string asFilter, string asShowOnlySavedRecord)
    {
        return CommonUtility.EncryptQuerystring("SchoolwiseStudentId=" + asStudId + "&CommentId=" + asCommentId + "&IsReadMode=" + asIsReadMode + "&IsPrincipal=" + asIsPrincipal + "&IsCounsellor=" +
            asIsCounsellor + "&IsClassTeacher=" + asIsClassTeacher + "&StdDivId=" + asStdDivId + "&Filter=" + asFilter + "&ShowOnlySavedRecord=" + asShowOnlySavedRecord);
    }

    #endregion

    public class KeyValue
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}