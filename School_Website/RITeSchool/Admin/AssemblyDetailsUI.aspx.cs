/* File Name :- AssemblyDetailsUI.aspx.cs
 * Created Date :- 13-Feb-2016
 * Class Description :- This class is used to Assembly Details. 
 * Created By :- Dnyaneshwar Shinde.
 */

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
using System.IO;
using MasterEntities;
using System.Data;
using System.Web;

public partial class AssemblyDetailsUI : SchoolBase
{
    #region Constant(s)

    private const string S_SAVE_MESSAGE = "Assembly details saved successfully !!!";
    private const string S_SUBMIT_MESSAGE = "Assembly details submitted successfully !!!";
    private const string S_PUBLISH_MESSAGE = "Assembly details published successfully !!!";
    private const string S_UNPUBLISH_MESSAGE = "Assembly details Unpublished successfully !!!";
    private const string S_ANSWERS = "ViewStateAnswers";
    private const string S_UPLOAD_ASSEMBLY_PATH = "\\DOWNLOADS\\Assembly\\";
    private const int I_FILE_SIZE = 1024;

    #endregion

    #region Data Member(s)

    private AssemblyDetailsBL moAssemblyDetailsBL;
    private List<AssemblyDetails> mlstAssemblyDetails;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill Assembly details.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            moAssemblyDetailsBL = new AssemblyDetailsBL(miSchoolId, miAcademicYearId, miUserId);

            if (Page.Request.Params.Get("__EVENTTARGET") != null)
            {
                if (btnSave.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                     btnSubmit.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                    FillAssemblyQuestionDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set controls on page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ReadQueryString();
                SetJavascriptAttributes();
                FillAssemblyQuestionDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to call Save method.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveAssembly();
            base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
            FillAssemblyQuestionDetails();
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message, true, tdMessage);
        }
    }

    /// <summary>
    /// This event is used to call Submit Method.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dtDate = Convert.ToDateTime(txtDate.Text);
            int iAssemblyId = Convert.ToInt32(hidAssemblyId.Value);
            moAssemblyDetailsBL.Submit(dtDate, true, iAssemblyId);
            base.DisplayMessage(S_SUBMIT_MESSAGE, false, tdMessage);
            FillAssemblyQuestionDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to publish Assembly Details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPublish_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dtDate = Convert.ToDateTime(txtDate.Text);
            int iAssemblyId = Convert.ToInt32(hidAssemblyId.Value);
            bool bIsPublish = btnPublish.Text == Resources.LocalizedResources.Publish ? true : false;

            moAssemblyDetailsBL.Publish(dtDate, bIsPublish, iAssemblyId);
            FillAssemblyQuestionDetails();
            if (bIsPublish)
                base.DisplayMessage(S_PUBLISH_MESSAGE, false, tdMessage);
            else
                base.DisplayMessage(S_UNPUBLISH_MESSAGE, false, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnBack, btnSave, btnSubmit, btnPublish });
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnSubmit.Attributes.Add("onclick", "if(!ConfirmSubmit()) return false;");
        txtDate.Attributes.Add("onchange", "if(!ChangeDate()) return false;");
        if (txtDate.Text == string.Empty)
            txtDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        lblDay.Text = Convert.ToString(Convert.ToDateTime(txtDate.Text).DayOfWeek);
    }

    /// <summary>
    /// This method is used to fill Questons & Controls.
    /// </summary>
    private void FillAssemblyQuestionDetails()
    {
        DateTime dtAssemblyDate;
        if (IsPostBack)
            dtAssemblyDate = Convert.ToDateTime(Request.Params[txtDate.ClientID.Replace("_", "$")]);
        else
            dtAssemblyDate = Convert.ToDateTime(txtDate.Text);
        mlstAssemblyDetails = moAssemblyDetailsBL.GetAllAssemblyDetails(dtAssemblyDate);
        ViewState[S_ANSWERS] = moAssemblyDetailsBL.AssemblyAnswers;
        if (mlstAssemblyDetails.Count != Constants.I_ZERO)
            hidAssemblyId.Value = Convert.ToString(mlstAssemblyDetails[0].AssemblyId);
        else
            hidAssemblyId.Value = Constants.S_ZERO;
        tblQuestions.Rows.Clear();
        FillAssemblyQuestions();
        SetButtonState();
    }

    /// <summary>
    /// This method is used to fill Assembly questions.
    /// </summary>
    private void FillAssemblyQuestions()
    {
        HtmlTableRow trHeader = new HtmlTableRow();
        base.AddCell(trHeader, string.Empty, "ClsAssemblyTopHeader", "left", 1, "font-weight:bold");
        base.AddCell(trHeader, "Assembly Activity", "ClsAssemblyTopHeader", "left", 1, "font-weight:bold");
        base.AddCell(trHeader, "Status", "ClsAssemblyTopHeader", "left", 1, "font-weight:bold");
        tblQuestions.Rows.Add(trHeader);
        moAssemblyDetailsBL.AssemblyQuestions.Where(qt => qt.ParentQuestionId == 0).OrderBy(qt => qt.SortOrder).ToList().ForEach
            (
                question =>
                {
                    HtmlTableRow trQuestion = new HtmlTableRow();
                    base.AddCell(trQuestion, question.Name, "ClsAssemblyHeader", "left", 3, "font-weight:bold");
                    tblQuestions.Rows.Add(trQuestion);

                    int iSrNo = 1;
                    moAssemblyDetailsBL.AssemblyQuestions.Where(qt => qt.ParentQuestionId == question.Id).OrderBy(qt => qt.SortOrder).ToList().ForEach
                        (
                            chd =>
                            {
                                HtmlTableRow trChileQuestion = new HtmlTableRow();
                                base.AddCell(trChileQuestion, "" + iSrNo + ".", "ClsAssemblyCell", "left", 1, "font-weight:bold;width:10px");
                                base.AddCell(trChileQuestion, chd.Name, "ClsAssemblyCell", "left", 1, "font-weight:bold;width:25%");

                                trChileQuestion.ID = "tr_" + chd.Id + "_" + chd.GroupId;

                                FillAnswers(chd.GroupId, chd.Id, trChileQuestion);
                                tblQuestions.Rows.Add(trChileQuestion);
                                iSrNo++;
                            }

                        );
                    AddBlankRow();
                }
            );
    }

    /// <summary>
    /// This method is used to display answers.
    /// </summary>
    /// <param name="aiGroupId"></param>
    /// <param name="aiQuestionId"></param>
    /// <param name="trAnswer"></param>
    private void FillAnswers(int aiGroupId, int aiQuestionId, HtmlTableRow atrAnswer)
    {
        List<Control> lstControls = new List<Control>();
        RadioButton optButton = null;
        bool bIsFound = false;
        var s = moAssemblyDetailsBL.AssemblyAnswers.Where(sa => sa.AnswerGroupId == aiGroupId).ToList();
        moAssemblyDetailsBL.AssemblyAnswers.Where(sa => sa.AnswerGroupId == aiGroupId).ToList().ForEach
            (
                answer =>
                {
                    if (answer.InputControlId == Constants.InputControls.Textbox.ToInt() || answer.InputControlId == Constants.InputControls.MultilineTextbox.ToInt())
                    {
                        TextBox oTextBox = new TextBox();
                        oTextBox.ID = "txt_" + aiQuestionId + "_" + answer.Id;
                        oTextBox.Attributes.Add("class", "exLrgTextbox");
                        oTextBox.Width = Unit.Percentage(100);
                        if (answer.InputControlId == Constants.InputControls.MultilineTextbox.ToInt())
                        {
                            oTextBox.Height = Unit.Pixel(60);
                            oTextBox.TextMode = TextBoxMode.MultiLine;
                        }

                        if (mlstAssemblyDetails.Any(usr => usr.AnswerId == answer.Id && usr.QuestionId == aiQuestionId))
                            oTextBox.Text = mlstAssemblyDetails.Where(usr => usr.AnswerId == answer.Id && usr.QuestionId == aiQuestionId).FirstOrDefault().FreeTextValue;
                        else
                            oTextBox.Text = string.Empty;

                        lstControls.Add(oTextBox);
                    }
                    else if (answer.InputControlId == Constants.InputControls.RadioButton.ToInt())
                    {
                        RadioButton oRadioButton = new RadioButton();
                        oRadioButton.GroupName = "Group" + aiQuestionId;
                        oRadioButton.ID = "opt_" + aiQuestionId + "_" + answer.Id;
                        oRadioButton.Text = answer.Answer;

                        if (mlstAssemblyDetails.Any(usr => usr.AnswerId == answer.Id && usr.QuestionId == aiQuestionId))
                        {
                            oRadioButton.Checked = true;
                            bIsFound = true;
                        }
                        else
                            oRadioButton.Checked = false;

                        if (optButton == null)
                            optButton = oRadioButton;

                        lstControls.Add(oRadioButton);
                    }

                    else if (answer.InputControlId == Constants.InputControls.CheckBoxList.ToInt())
                    {
                        CheckBoxList oCheckBoxList = new CheckBoxList();
                        oCheckBoxList.ID = "chk_" + aiQuestionId + "_" + answer.Id;
                        if (oCheckBoxList.ID != null)
                        {
                            ListSource.FillCheckBoxList(moAssemblyDetailsBL.StandardDetails, oCheckBoxList, "StandardName", "StandardId");
                            oCheckBoxList.RepeatColumns = 15;

                            foreach (var lstStandards in mlstAssemblyDetails)
                            {
                                if (lstStandards.FreeTextValue != null && lstStandards.QuestionId == aiQuestionId)
                                {
                                    string sStandard = lstStandards.FreeTextValue;
                                    string[] sStandardValue = sStandard.Split(',');

                                    int iCount = oCheckBoxList.Items.Count;
                                    for (int iIndex = 0; iIndex < iCount; iIndex++)
                                    {
                                        if (sStandardValue.Contains(oCheckBoxList.Items[iIndex].Value))
                                            oCheckBoxList.Items[iIndex].Selected = true;
                                    }
                                }
                            }

                            lstControls.Add(oCheckBoxList);
                        }
                    }

                    else if (answer.InputControlId == Constants.InputControls.FileUploadControl.ToInt())
                    {
                        FileUpload oFileUpload = new FileUpload();
                        oFileUpload.ID = "File_" + aiQuestionId + "_" + answer.Id;
                        if (oFileUpload.ID != null)
                        {                            
                            foreach (var lstFileUpload in mlstAssemblyDetails)
                            {
                                if (lstFileUpload.AssemblyPhoto != null && lstFileUpload.QuestionId == aiQuestionId)
                                {
                                    hidPhotoFilePath.Value = lstFileUpload.PhotoFilePath;
                                    Byte[] ImageBinaryData = GetByteArrayFromFileField(oFileUpload);
                                    DateTime dtDate = Convert.ToDateTime(txtDate.Text.ToString());
                                    //List<PhotoMaster> lstPhotos = AssemblyDetailsBL.GetAssemblyBinaryPhoto(miSchoolId, miAcademicYearId, dtDate);
                                    //SetImageData(lstPhotos, miUserId);                                    
                                    //imgAssemblyPhoto.Src = "../School_WebSite/images/Diwali.png";
                                    //imgAssemblyPhoto.Visible = true;
                                    //Image imgAssembly = new Image();
                                    if (lstFileUpload.PhotoFilePath != string.Empty)
                                    {
                                        ImageButton imgAssembly = new ImageButton();
                                        imgAssembly.ImageUrl = "~/RITeSchool/images/iconGridSml_ViewGE.gif";                                        
                                        imgAssembly.ID = "Img";
                                        imgAssembly.Visible = true;                                        
                                        imgAssembly.Attributes.Add("onclick", "OpenFile('" + lstFileUpload.PhotoFilePath + "')");
                                        //imgAssembly.OnClientClick = "~/RITeSchool/DOWNLOADS/Assembly/ " + lstFileUpload.PhotoFilePath;
                                        imgAssembly.ToolTip = "View File";
                                        lstControls.Add(imgAssembly);                                    
                                    }
                                    hidFileUpload.Value = Convert.ToString(ImageBinaryData);
                                }
                            }
                            lstControls.Add(oFileUpload); 
                        }
                    }
                }

            );

        if (optButton != null && !bIsFound)
            optButton.Checked = true;

        base.AddCells(atrAnswer, string.Empty, "ClsAssemblyCell", "left", 1, "width:100px", lstControls);
        tblQuestions.Rows.Add(atrAnswer);
    }

    /// <summary>
    /// This method is used to set button state.
    /// </summary>
    private void SetButtonState()
    {
        btnSave.Enabled = true;
        btnSubmit.Enabled = true;
        btnPublish.Visible = false;
        btnPublish.Text = Resources.LocalizedResources.Publish;
        if (moAssemblyDetailsBL.ButtonStates.EnablePublishButton)
        {
            btnPublish.Text = Resources.LocalizedResources.Unpublish;
            btnSave.Enabled = false;
            btnSubmit.Enabled = false;
        }
        else
        {
            if (moAssemblyDetailsBL.ButtonStates.EnableSubmitButton)
            {
                btnSave.Enabled = false;
                btnSubmit.Enabled = false;
            }
            else
            {
                if (!moAssemblyDetailsBL.ButtonStates.EnableSaveButton)
                {
                    btnSubmit.Enabled = false;
                    btnPublish.Enabled = false;
                }
            }
        }
        if (moAssemblyDetailsBL.ButtonStates.IsApprover)
        {
            btnPublish.Visible = true;
            if (moAssemblyDetailsBL.ButtonStates.EnableSaveButton && moAssemblyDetailsBL.ButtonStates.EnableSubmitButton)
                btnPublish.Enabled = true;
            else
                btnPublish.Enabled = false;
        }
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
    /// This method is used to save Assembly Details.
    /// </summary>
    private void SaveAssembly()
    {
        string sXml = Populate();
        DateTime dtDate = Convert.ToDateTime(txtDate.Text);
        int iAssemblyId = Convert.ToInt32(hidAssemblyId.Value);
        moAssemblyDetailsBL.Save(sXml, dtDate, iAssemblyId);
    }

    /// <summary>
    /// This method is used to populate values for save.
    /// </summary>
    private string Populate()
    {
        List<AssemblyAnswers> lstAnswers = new List<AssemblyAnswers>();
        if (ViewState[S_ANSWERS] != null)
            lstAnswers = ViewState[S_ANSWERS] as List<AssemblyAnswers>;

        List<AssemblyDetails> lstAssemblyDetails = new List<AssemblyDetails>();
        foreach (HtmlTableRow tr in tblQuestions.Rows)
        {
            AssemblyDetails oAssemblyDetails = new AssemblyDetails();
            if (tr.ID != null)
            {
                string sSuffix = tr.ID.Substring(3);
                int iQuestionId = sSuffix.Substring(0, sSuffix.IndexOf('_')).ToInt();
                int iGroupId = sSuffix.Substring(sSuffix.IndexOf('_') + 1).ToInt();

                oAssemblyDetails.QuestionId = iQuestionId;
                oAssemblyDetails.AnswerId = 0;
                oAssemblyDetails.FreeTextValue = string.Empty;

                var oAnswers = lstAnswers.Where(obj => obj.AnswerGroupId == iGroupId).ToList();

                TextBox txt = tr.FindControl("txt_" + iQuestionId + "_" + oAnswers.FirstOrDefault().Id) as TextBox;
                if (txt != null)
                {
                    txt.Text = txt.Text.Trim();

                    if (txt.Text != string.Empty)
                    {
                        oAssemblyDetails.AnswerId = oAnswers.FirstOrDefault().Id;
                        oAssemblyDetails.FreeTextValue = txt.Text;
                    }
                }

                foreach (var Ans in oAnswers)
                {
                    RadioButton opt = tr.FindControl("opt_" + iQuestionId + "_" + Ans.Id) as RadioButton;
                    if (opt != null)
                    {
                        if (opt.Checked)
                            oAssemblyDetails.AnswerId = Ans.Id;
                    }
                }

                foreach (var Ans in oAnswers)
                {
                    CheckBoxList chk = tr.FindControl("chk_" + iQuestionId + "_" + Ans.Id) as CheckBoxList;
                    if (chk != null)
                    {
                        int iCount = chk.Items.Count;
                        string sStandards = string.Empty;
                        oAssemblyDetails.AnswerId = Ans.Id;
                        for (int iIndex = 0; iIndex < iCount; iIndex++)
                        {
                            if (chk.Items[iIndex].Selected == true)
                            {
                                sStandards = sStandards + "," + chk.Items[iIndex].Value;
                            }
                        }
                        if (sStandards != string.Empty)
                        {
                            sStandards = sStandards.Substring(1);
                        }
                        oAssemblyDetails.FreeTextValue = sStandards;

                    }
                }

                foreach (var Ans in oAnswers)
                {
                    FileUpload file = tr.FindControl("file_" + iQuestionId + "_" + Ans.Id) as FileUpload;
                    if (file != null)
                    {
                        oAssemblyDetails.AnswerId = oAnswers.FirstOrDefault().Id;
                        if (file.HasFile == true)
                        {
                            string sFileName = file.PostedFile.FileName;
                            string sFileExtention = System.IO.Path.GetExtension(sFileName);
                            string sFileMimeType = file.PostedFile.ContentType;
                            int iFileLengthinKb = file.PostedFile.ContentLength / I_FILE_SIZE;

                            string[] matchExtension = { ".jpg", ".png", ".bmp", ".jpeg" };
                            string[] matchMimeType = { "image/jpeg", "image/png", "image/bmp", "image/jpeg" };

                            if (matchExtension.Contains(sFileExtention) && matchMimeType.Contains(sFileMimeType))
                            {
                                if (iFileLengthinKb <= I_FILE_SIZE)
                                {
                                    string sPhotoName = string.Empty;
                                    sPhotoName = SaveFileOnServer(file);
                                    oAssemblyDetails.PhotoFilePath = sPhotoName;
                                    Byte[] ImageBinaryData = GetByteArrayFromFileField(file);
                                    oAssemblyDetails.AssemblyPhoto = Convert.ToString(ImageBinaryData);
                                }
                                else
                                    throw new System.ApplicationException("File size should not be greater than 1 MB.");
                            }
                            else
                                throw new System.ApplicationException("File type should be between .jpg, .jpeg, .png and .bmp.");
                        }
                        else
                        {
                            oAssemblyDetails.PhotoFilePath = hidPhotoFilePath.Value;
                            oAssemblyDetails.AssemblyPhoto = hidFileUpload.Value;
                        }
                    }
                }
                if (oAssemblyDetails.AnswerId != 0)
                    lstAssemblyDetails.Add(oAssemblyDetails);
            }
        }
        return base.GenerateXml(lstAssemblyDetails);
    }

    /// <summary>
    /// This method is used to read query String.
    /// </summary>
    private void ReadQueryString()
    {
        hidAssemblyId.Value = QueryString["AssemblyId"];
        txtDate.Text = QueryString["Date"];
    }

    /// <summary>
    /// This method is used to save File on Server.
    /// </summary>
    /// <param name="aFile"></param>
    private string SaveFileOnServer(FileUpload aFile)
    {
        string sFolderName = Server.MapPath("..") + S_UPLOAD_ASSEMBLY_PATH;
        string asFileName = aFile.FileName;
        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;
        if (File.Exists(sServerFilePath))
        {
            sFileName = CommonUtility.GetFileNameForRenaming(asFileName);
            sServerFilePath = sFolderName + sFileName;
        }
        aFile.SaveAs(sServerFilePath);
        return sFileName;
    }

    /// <summary>
    /// This method is used to set image data.
    /// </summary>
    /// <param name="alstPhotos"></param>
    /// <param name="aiUserId"></param>
    private void SetImageData(List<PhotoMaster> alstPhotos, int aiUserId)
    {
        if (alstPhotos.Where(usr => usr.UserId == miUserId).Any())
        {
            byte[] imageByteArray = alstPhotos.Where(usr => usr.UserId == miUserId).Select(bytes => bytes.TotalBytes).FirstOrDefault();
            if (imageByteArray != null && imageByteArray.Length > 0)
            {
                DateTime dtDate = Convert.ToDateTime(txtDate.Text.ToString());
                DataTable dt = new DataTable();
                dt.Columns.Add("UserId");
                dt.Columns.Add("TotalBytes");
                foreach (var lstPhoto in alstPhotos)
                {
                    dt.Rows.Add(lstPhoto.UserId, lstPhoto.TotalBytes);
                }
                //Response.Clear();
                //Response.ContentType = "image/jpg";
                //Response.BinaryWrite(imageByteArray);
                //Response.End();
                string S = (string)dt.Rows[0]["TotalBytes"];
                byte[] data = System.Text.Encoding.ASCII.GetBytes(S);
                Byte[] bytes = data;
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = dt.Rows[0]["UserId"].ToString();
                Response.AddHeader("content-disposition", "attachment;filename=" + dt.Rows[0]["TotalBytes"].ToString());
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
        }
    }

    #endregion
}