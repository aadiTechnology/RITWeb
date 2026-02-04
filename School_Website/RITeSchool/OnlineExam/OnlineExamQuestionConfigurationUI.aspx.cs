using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using SchoolEntities.Admin;
using Utility;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

public partial class OnlineExamQuestionConfigurationUI :SchoolBase
{
    #region Constant(s)

    const string S_DEFAULT_SORT_EXP = "Question";
    const string S_UPLOAD_FILE_FOLDER_PATH = "\\RITeSchool\\Uploads\\OnlineExamImages\\";
    private const string S_FOLDER_PATH = @"../Uploads/OnlineExamImages/";

    #endregion

    #region Data Member(s)

    private List<StandardDivisions> mlstStandardDivisions;
    private OnlineExamQuestionConfigurationBL moOnlineExamQuestionConfigurationBL = null;

    #endregion

    //#region Event(s)

    ///// <summary>
    ///// This event is used to add sort image.
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty)
            {
                hidSortExpression.Value = "Question";
                hidSortDirection.Value = Constants.S_ASCENDING;
            }

            AddSortImage(lstvwConfigure, hidSortExpression.Value, hidSortDirection.Value);
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
            moOnlineExamQuestionConfigurationBL = new OnlineExamQuestionConfigurationBL(miSchoolId, miAcademicYearId, miUserId);

            if (!IsPostBack)
            {
                FillStandard();
                FillSubjectCombo();
                //FillStandardDivisionLstBox();
                SetJavascriptAttributes();
                SetButtonState();
                ReadQueryString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    private void ReadQueryString()
    {
        if (QueryString["Is_Configured"] != null)
            hidIsConfigured.Value = QueryString["Is_Configured"].ToString();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            if (btnSave.Text == "Save")
                DisplayMessage("Question details saved successfully!!!",false);
            else
                DisplayMessage("Question details updated successfully!!!", false);

            if (hidIsConfigured.Value != Constants.S_YES)
                base.SaveConfigDetails(Constants.SchoolConfigurations.OnlineExamQuestionConfiguration.ToInt());

            hidIsConfigured.Value = Constants.S_YES;
            ClearFields();
            FillOnlineExamQuestion();
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

    ////protected void btnCopy_Click(object sender, EventArgs e)
    ////{
    ////    try
    ////    {
    ////        string Sids = GetClassesForExam();
    ////        moOnlineExamQuestionConfigurationBL.CopySubjectConfiguration(ddlDivision.SelectedValue.ToInt(), ddlSubject.SelectedValue.ToInt(), Sids);
    ////        lblUpdateMessage.Text = "Copy Exam Question Configuration Successfull !!!";

    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
    ////    }
    ////}

    ////protected void cmbAnswerType_SelectedIndexChanged(object sender, EventArgs e)
    ////{
    ////    try
    ////    {
    ////        if (cmbAnswerType.SelectedValue == "1")
    ////        {
    ////            trAns1.Visible = true;
    ////            trAns2.Visible = true;
    ////            trAns3.Visible = true;
    ////            trAns4.Visible = true;
    ////        }
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
    ////    }
    ////}

    ///// <summary>
    ///// This event used to delete, update vehicle details.
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    protected void llstvwConfigure_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ClearFields();
            if (e.CommandName != Constants.S_COMMAND_SORT)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iListIndex = oCurrentItem.DisplayIndex;
                int iId = Convert.ToInt32(lstvwConfigure.DataKeys[iListIndex]["Id"]);

                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moOnlineExamQuestionConfigurationBL.Delete(iId.ToInt());
                    //lblUpdateMessage.Text = "Question details deleted successfully !!!";
                    DisplayMessage("Question details deleted successfully!!!", false);
                    FillOnlineExamQuestion();
                    ClearFields();
                }
                else if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    FillControls(iId);
                    ddlStandard.Enabled = false;
                    ddlDivision.Enabled = false;
                    ddlSubject.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    ///// <summary>
    ///// This is used to bind confirmation event to delete button.
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    protected void lstvwConfigure_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                OnlineExamQuestionConfig oOnlineExamQuestionConfig = e.Item.DataItem as OnlineExamQuestionConfig;
                ImageButton imgBtnEdit = e.Item.FindControl("imgBtnEdit") as ImageButton;
                ImageButton oimgbtnDelete = e.Item.FindControl("imgbtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmRemove()) {return false;}");
                oimgbtnDelete.Visible = !oOnlineExamQuestionConfig.IsSubmitted;
                imgBtnEdit.Visible = !oOnlineExamQuestionConfig.IsSubmitted;

                Label lblCorrectAnswer = e.Item.FindControl("lblCorrectAnswer") as Label;
                Image imgImage = e.Item.FindControl("imgImage") as Image;
                if (oOnlineExamQuestionConfig.AnswerTypeId == 2)
                {
                    imgImage.ImageUrl = "../Uploads/OnlineExamImages/" + oOnlineExamQuestionConfig.AnswerFilePath;
                    imgImage.Visible = true;
                    lblCorrectAnswer.Visible = false;
                }
                else
                {
                    imgImage.Visible = false;
                    lblCorrectAnswer.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    ///// <summary>
    ///// This event is used to initialize the DataPager control of the ListView.
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    protected void lstvwConfigure_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwConfigure.Items.Count > 0)
            {
                DtPgCount.Visible = true;
                ControlUtility.FillListViewPagerFooter(lstvwConfigure, DtPgCount);
            }
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    ///// <summary>
    ///// This event is used to update the ListView pager controls.
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwConfigure);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    ////protected void lstvwVideoStandardDivision_ItemDataBound(object sender, ListViewItemEventArgs e)
    ////{
    ////    try
    ////    {
    ////        ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
    ////        int iRowId = oCurrentItem.DisplayIndex;
    ////        if (e.Item.ItemType == ListViewItemType.DataItem)
    ////        {
    ////            CheckBox chkVdoStandard = oCurrentItem.FindControl("chkVdoStandard") as CheckBox;
    ////            CheckBoxList chkVideoStandardDivLst = oCurrentItem.FindControl("chkvideoStandardDivLst") as CheckBoxList;
    ////            int iStandardId = lstvwVideoStandardDivision.DataKeys[iRowId]["StandardId"].ToInt();
    ////            var oList = mlstStandardDivisions.Where(sd => sd.StandardId == iStandardId).OrderBy(sd => sd.OriginalStandardId).ThenBy(sd => sd.StandardDivisionId).Select(sd => new { sd.StandardDivisionId, sd.DivisionName }).ToList();
    ////            ListSource.FillCheckBoxList(oList, chkVideoStandardDivLst, "DivisionName", "StandardDivisionId");

    ////            chkVdoStandard.Attributes.Add("onclick", "CheckAllForVideo(this,'" + iRowId + "')");
    ////            chkVideoStandardDivLst.Attributes.Add("onclick", "CheckStdForVideo('" + iRowId + "')");
    ////        }
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
    ////    }
    ////}

    ///// <summary>
    ///// This event is used to fill division dropdownlist.
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Filldivision();
            FillSubjectCombo();
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillSubjectCombo();
            FillOnlineExamQuestion();
            ClearFields();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    ///// <summary>
    ///// This event is used to sort the listview of vehicle staff association.
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    protected void lstvwConfigure_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillOnlineExamQuestion();
            ClearFields();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }   

    /// <summary>
    /// This event is used to delete Question Image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            moOnlineExamQuestionConfigurationBL = new OnlineExamQuestionConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            moOnlineExamQuestionConfigurationBL.DeleteQuestionAnswerImage(hidQuestionId.Value.ToInt(), 0);
            hidQuestionFilePath.Value = string.Empty;
            btnView.Visible = false;
            imgDelete.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete Answer1 Image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnImgAnsDelete1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            moOnlineExamQuestionConfigurationBL = new OnlineExamQuestionConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            moOnlineExamQuestionConfigurationBL.DeleteQuestionAnswerImage(hidQuestionId.Value.ToInt(), hidAnswerId1.Value.ToInt());
            hidAnswerFilePath1.Value = string.Empty;
            btnImgAnsr1.Visible = false;
            btnImgAnsDelete1.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete Answer2 Image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnImgAnsDelete2_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            moOnlineExamQuestionConfigurationBL = new OnlineExamQuestionConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            moOnlineExamQuestionConfigurationBL.DeleteQuestionAnswerImage(hidQuestionId.Value.ToInt(), hidAnswerId2.Value.ToInt());
            hidAnswerFilePath2.Value = string.Empty;
            btnImgAnsr2.Visible = false;
            btnImgAnsDelete2.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete Answer3 Image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnImgAnsDelete3_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            moOnlineExamQuestionConfigurationBL = new OnlineExamQuestionConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            moOnlineExamQuestionConfigurationBL.DeleteQuestionAnswerImage(hidQuestionId.Value.ToInt(), hidAnswerId3.Value.ToInt());
            hidAnswerFilePath3.Value = string.Empty;
            btnImgAnsr3.Visible = false;
            btnImgAnsDelete3.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete Answer4 Image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnImgAnsDelete4_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            moOnlineExamQuestionConfigurationBL = new OnlineExamQuestionConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            moOnlineExamQuestionConfigurationBL.DeleteQuestionAnswerImage(hidQuestionId.Value.ToInt(), hidAnswerId4.Value.ToInt());
            hidAnswerFilePath4.Value = string.Empty;
            btnImgAnsr4.Visible = false;
            btnImgAnsDelete4.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    //#endregion

    //#region Method(s)

    //private void SetAnswerControlVisibility(bool bFlag)
    //{
    //    tdFUAnswer1.Visible = !bFlag;
    //    tdFUAnswer2.Visible = !bFlag;
    //    tdFUAnswer3.Visible = !bFlag;
    //    tdFUAnswer4.Visible = !bFlag;
    //    tdTxtAnswer1.Visible = bFlag;
    //    tdTxtAnswer2.Visible = bFlag;
    //    tdTxtAnswer3.Visible = bFlag;
    //    tdTxtAnswer4.Visible = bFlag;
    //}

    private void SetButtonState()
    {
        ButtonStateDetails oButtonStateDetails = moOnlineExamQuestionConfigurationBL.GetButtonState(ddlStandard.SelectedValue.ToInt(), ddlDivision.SelectedValue.ToInt(), ddlSubject.SelectedValue.ToInt());
        btnSubmit.Enabled = oButtonStateDetails.EnableSubmitButtton;
        btnUnsubmit.Enabled = oButtonStateDetails.EnableUnSubmitButtton;
    }

    private void Save()
    {
        string AnswerXML = PopulateOnlineExamDetails();
        OnlineExamQuestionConfig oOnlineExamQuestConfig = Populate();
        moOnlineExamQuestionConfigurationBL.Save(AnswerXML, oOnlineExamQuestConfig, ddlStandard.SelectedValue.ToInt());

        if (cmbAnswerType.SelectedValue == Constants.S_ONE)
        {
            DeleteFile(hidAnswerFilePath1.Value);
            DeleteFile(hidAnswerFilePath2.Value);
            DeleteFile(hidAnswerFilePath3.Value);
            DeleteFile(hidAnswerFilePath4.Value);
        }
    }

    private void DeleteFile(string asFileName)
    {
        if (asFileName != string.Empty)
        {
            string sfolderPath = base.BasePath + "\\RITeSchool\\Uploads\\ONlineExamImages\\" + asFileName;
            if (File.Exists(sfolderPath))
                File.Delete(sfolderPath);
        }
    }

    private OnlineExamQuestionConfig Populate()
    {
        string sServerFilePath = string.Empty;
        OnlineExamQuestionConfig oExamQueConfig = new OnlineExamQuestionConfig();
        string sFileName = string.Empty;

        if (fuQuestion.HasFile)
        {
            sFileName = SaveFileOnServer(fuQuestion.FileName, fuQuestion, hidQuestionFilePath);
            sServerFilePath = sFileName;
        }
        else
        {
            sServerFilePath = hidQuestionFilePath.Value;
        }

        oExamQueConfig.Question = txtQuestion.Text.Trim();
        oExamQueConfig.SubjectId = ddlSubject.SelectedValue.ToInt();
        oExamQueConfig.StandardDivisionId = ddlDivision.SelectedValue.ToInt();
        oExamQueConfig.OutOfMarks = txtOutOfMarks.Text.ToInt();
        oExamQueConfig.AnswerTypeId = cmbAnswerType.SelectedValue.ToInt();
        oExamQueConfig.QuestionId = Convert.ToInt32(hidQuestionId.Value);
        oExamQueConfig.QuestionFilePath = sServerFilePath;
        return oExamQueConfig;
    }

    private void ClearFields()
    {
        txtQuestion.Text = string.Empty;
        txtOutOfMarks.Text = string.Empty;
        hidQuestionId.Value = Constants.S_ZERO;
        btnSave.Text = Resources.LocalizedResources.Save;

        txtAns1.Text = string.Empty;
        txtAns2.Text = string.Empty;
        txtAns3.Text = string.Empty;
        txtAns4.Text = string.Empty;
        rdOption1.Checked = false;
        rdOption2.Checked = false;
        rdOption3.Checked = false;
        rdOption4.Checked = false;
        cmbAnswerType.ClearSelection();

        ddlStandard.Enabled = true;
        ddlDivision.Enabled = true;
        ddlSubject.Enabled = true;

        ddlStandard.Enabled = true;
        ddlDivision.Enabled = true;
        ddlSubject.Enabled = true;

        btnView.Visible = false;
        btnImgAnsr1.Visible = false;
        btnImgAnsr2.Visible = false;
        btnImgAnsr3.Visible = false;
        btnImgAnsr4.Visible = false;

        imgDelete.Visible = false;
        btnImgAnsDelete1.Visible = false;
        btnImgAnsDelete2.Visible = false;
        btnImgAnsDelete3.Visible = false;
        btnImgAnsDelete4.Visible = false;
        //SetAnswerControlVisibility(true);
    }

    private void FillOnlineExamQuestion()
    {
        lstvwConfigure.DataSourceID = ObjDSQuestionDetails.ID;
        lstvwConfigure.DataBind();
        SetButtonState();
    }

    private string PopulateOnlineExamDetails()
    {
        List<OnlineExamAnswer> lstOnlineExamAnswer = new List<OnlineExamAnswer>();

        string sServerFilePath1 = string.Empty;
        string sServerFilePath2 = string.Empty;
        string sServerFilePath3 = string.Empty;
        string sServerFilePath4 = string.Empty;
        string sFileName = string.Empty;
        string sFolderName = string.Empty;

        string sFreeInput1 = string.Empty, sFreeInput2 = string.Empty, sFreeInput3 = string.Empty, sFreeInput4 = string.Empty;
        if (cmbAnswerType.SelectedValue == "2")
        {
            if (fuAnswer1.HasFile)
            {
                sFileName = SaveFileOnServer(fuAnswer1.FileName, fuAnswer1, hidAnswerFilePath1);
                sServerFilePath1 = sFileName;
            }
            else if (hidAnswerFilePath1.Value != string.Empty)
            {
                sServerFilePath1 = hidAnswerFilePath1.Value;
            }

            if (fuAnswer2.HasFile)
            {
                sFileName = SaveFileOnServer(fuAnswer2.FileName, fuAnswer2, hidAnswerFilePath2);
                sServerFilePath2 = sFileName;
            }
            else if (hidAnswerFilePath2.Value != string.Empty)
            {
                sServerFilePath2 = hidAnswerFilePath2.Value;
            }

            if (fuAnswer3.HasFile)
            {
                sFileName = SaveFileOnServer(fuAnswer3.FileName, fuAnswer3, hidAnswerFilePath3);
                sServerFilePath3 = sFileName;
            }
            else if (hidAnswerFilePath3.Value != string.Empty)
            {
                sServerFilePath3 = hidAnswerFilePath3.Value;
            }

            if (fuAnswer4.HasFile)
            {
                sFileName = SaveFileOnServer(fuAnswer4.FileName, fuAnswer4, hidAnswerFilePath4);
                sServerFilePath4 = sFileName;
            }
            else if (hidAnswerFilePath4.Value != string.Empty)
            {
                sServerFilePath4 = hidAnswerFilePath4.Value;
            }
        }
        else if (cmbAnswerType.SelectedValue == "1")
        {
            sFreeInput1 = txtAns1.Text.Trim();
            sFreeInput2 = txtAns2.Text.Trim();
            sFreeInput3 = txtAns3.Text.Trim();
            sFreeInput4 = txtAns4.Text.Trim();
        }
        else
        {
            OnlineExamAnswer oOnlineExamAnswer = new OnlineExamAnswer();
            {
                oOnlineExamAnswer.Answer = string.Empty;
                oOnlineExamAnswer.IsCorrectAnswer = false;
                oOnlineExamAnswer.DisplayOrder = 1;
                oOnlineExamAnswer.AnswerFilePath = string.Empty;
                lstOnlineExamAnswer.Add(oOnlineExamAnswer);
            }
        }

        if (cmbAnswerType.SelectedValue != "3")
        {
            OnlineExamAnswer oOnlineExamAnswer1 = new OnlineExamAnswer();
            {
                oOnlineExamAnswer1.Answer = sFreeInput1;
                oOnlineExamAnswer1.IsCorrectAnswer = rdOption1.Checked;
                oOnlineExamAnswer1.DisplayOrder = 1;
                oOnlineExamAnswer1.AnswerFilePath = sServerFilePath1;
                lstOnlineExamAnswer.Add(oOnlineExamAnswer1);
            }

            OnlineExamAnswer oOnlineExamAnswer2 = new OnlineExamAnswer();
            {
                oOnlineExamAnswer2.Answer = sFreeInput2;
                oOnlineExamAnswer2.IsCorrectAnswer = rdOption2.Checked;
                oOnlineExamAnswer2.DisplayOrder = 2;
                oOnlineExamAnswer2.AnswerFilePath = sServerFilePath2;
                lstOnlineExamAnswer.Add(oOnlineExamAnswer2);
            }

            OnlineExamAnswer oOnlineExamAnswer3 = new OnlineExamAnswer();
            {
                oOnlineExamAnswer3.Answer = sFreeInput3;
                oOnlineExamAnswer3.IsCorrectAnswer = rdOption3.Checked;
                oOnlineExamAnswer3.DisplayOrder = 3;
                oOnlineExamAnswer3.AnswerFilePath = sServerFilePath3;
                lstOnlineExamAnswer.Add(oOnlineExamAnswer3);
            }

            OnlineExamAnswer oOnlineExamAnswer4 = new OnlineExamAnswer();
            {
                oOnlineExamAnswer4.Answer = sFreeInput4;
                oOnlineExamAnswer4.IsCorrectAnswer = rdOption4.Checked;
                oOnlineExamAnswer4.DisplayOrder = 4;
                oOnlineExamAnswer4.AnswerFilePath = sServerFilePath4;
                lstOnlineExamAnswer.Add(oOnlineExamAnswer4);
            }
        }

        return base.GenerateXml(lstOnlineExamAnswer);
    }

    private void FillControls(int aiId)
    {
        List<OnlineExamQuestionConfig> lstExamConfig = moOnlineExamQuestionConfigurationBL.Get(aiId);

        if (lstExamConfig != null && lstExamConfig.Count > 0)
        {
            hidIsEditMode.Value = Constants.S_YES;
            txtQuestion.Text = lstExamConfig[0].Question.ToString();
            ddlSubject.SelectedValue = lstExamConfig[0].SubjectId.ToString();
            txtOutOfMarks.Text = lstExamConfig[0].OutOfMarks.ToString();
            if (lstExamConfig[0].AnswerTypeId.ToString() != Constants.S_ZERO)
            {
                cmbAnswerType.SelectedValue = lstExamConfig[0].AnswerTypeId.ToString();

                bool bFlag = false;
                if (cmbAnswerType.SelectedValue == Constants.S_ONE)
                    bFlag = true;
                else if (cmbAnswerType.SelectedValue == Constants.S_TWO)
                    bFlag = false;

                //SetAnswerControlVisibility(bFlag);
            }

            if (lstExamConfig[0].QuestionFilePath.ToString() != string.Empty)
            {
                btnView.Visible = true;
                imgDelete.Visible = true;
                hidQuestionFilePath.Value = lstExamConfig[0].QuestionFilePath.ToString();
                string sNewFileName = S_FOLDER_PATH + lstExamConfig[0].QuestionFilePath.ToString();
                btnView.Attributes.Add("onclick", "OpenWindow('" + sNewFileName + "'); return false;");
            }
                        
            OnlineExamQuestionConfig oFirstAnswer = lstExamConfig.Where(ld => ld.DisplayOrder == 1).FirstOrDefault();
            if (oFirstAnswer != null)
            {
                txtAns1.Text = oFirstAnswer.Answer;
                rdOption1.Checked = oFirstAnswer.IsCorrectAnswer;
                hidAnswerId1.Value = oFirstAnswer.Id.ToString();

                if (oFirstAnswer.AnswerFilePath != null && oFirstAnswer.AnswerFilePath != string.Empty)
                {
                    btnImgAnsr1.Visible = true;
                    btnImgAnsDelete1.Visible = true;
                    hidAnswerFilePath1.Value = oFirstAnswer.AnswerFilePath;
                    string sFileName = S_FOLDER_PATH + oFirstAnswer.AnswerFilePath;
                    btnImgAnsr1.Attributes.Add("onclick", "OpenWindow('" + sFileName + "'); return false;");
                }
            }
            OnlineExamQuestionConfig oSecondAnswer = lstExamConfig.Where(ld => ld.DisplayOrder == 2).FirstOrDefault();
            if (oSecondAnswer != null)
            {
                txtAns2.Text = oSecondAnswer.Answer;
                rdOption2.Checked = oSecondAnswer.IsCorrectAnswer;
                hidAnswerId2.Value = oSecondAnswer.Id.ToString();

                if (oSecondAnswer.AnswerFilePath != null && oSecondAnswer.AnswerFilePath != string.Empty)
                {
                    btnImgAnsr2.Visible = true;
                    btnImgAnsDelete2.Visible = true;
                    hidAnswerFilePath2.Value = oSecondAnswer.AnswerFilePath;
                    string sFileName = S_FOLDER_PATH + oSecondAnswer.AnswerFilePath;
                    btnImgAnsr2.Attributes.Add("onclick", "OpenWindow('" + sFileName + "'); return false;");
                }
            }

            OnlineExamQuestionConfig oThirdAnswer = lstExamConfig.Where(ld => ld.DisplayOrder == 3).FirstOrDefault();
            if (oThirdAnswer != null)
            {
                txtAns3.Text = oThirdAnswer.Answer;
                rdOption3.Checked = oThirdAnswer.IsCorrectAnswer;
                hidAnswerId3.Value = oThirdAnswer.Id.ToString();

                if (oThirdAnswer.AnswerFilePath != null && oThirdAnswer.AnswerFilePath != string.Empty)
                {
                    btnImgAnsr3.Visible = true;
                    btnImgAnsDelete3.Visible = true;
                    hidAnswerFilePath3.Value = oThirdAnswer.AnswerFilePath;
                    string sFileName = S_FOLDER_PATH + oThirdAnswer.AnswerFilePath;
                    btnImgAnsr3.Attributes.Add("onclick", "OpenWindow('" + sFileName + "'); return false;");
                }
            }
            OnlineExamQuestionConfig oForthAnswer = lstExamConfig.Where(ld => ld.DisplayOrder == 4).FirstOrDefault();
            if (oForthAnswer != null)
            {
                txtAns4.Text = oForthAnswer.Answer;
                rdOption4.Checked = oForthAnswer.IsCorrectAnswer;
                hidAnswerId4.Value = oForthAnswer.Id.ToString();

                if (oForthAnswer.AnswerFilePath != null && oForthAnswer.AnswerFilePath != string.Empty)
                {
                    btnImgAnsr4.Visible = true;
                    btnImgAnsDelete4.Visible = true;
                    hidAnswerFilePath4.Value = oForthAnswer.AnswerFilePath;
                    string sFileName = S_FOLDER_PATH + oForthAnswer.AnswerFilePath;
                    btnImgAnsr4.Attributes.Add("onclick", "OpenWindow('" + sFileName + "'); return false;");
                }
            }

            hidQuestionId.Value = aiId.ToString();
            btnSave.Text = Resources.LocalizedResources.Update;

            ddlStandard.Enabled = false;
            ddlDivision.Enabled = false;
            ddlSubject.Enabled = false;
        }
    }

    ///// <summary>
    ///// This procedure is used to initialize default fields.
    ///// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel });
        valSumTaskDetails.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        btnSave.Attributes.Add("onclick", "ClearMessageText()");
        SetDefaultValues();
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.OnlineExamRelated));
        hidAreYouSureYouWantDeleteEvent.Value = Resources.LocalizedResources.AreYouSureYouWantDeleteEvent;
        cmbAnswerType.Attributes.Add("onchange", "VisibleHideControls()");
        lnkMathFormula.Attributes.Add("onclick", "if(!OpenFormulaScreen()) return false;");
    }

    ///// <summary>
    ///// This method is used to set sort variables.
    ///// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    private void SetDefaultValues()
    {
        //hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        valSumTaskDetails.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidSortDirection.Value = Constants.S_ASCENDING;
    }

    private void FillDefaultDivisionValue()
    {
        ListItem olstDivision = new ListItem();
        olstDivision.Text = Constants.S_ALL;
        olstDivision.Value = "0";
        ddlDivision.Items.Add(olstDivision);
    }

    private void FillStandard()
    {
        DataTable oDT = moOnlineExamQuestionConfigurationBL.GetAllStandards();
        ControlUtility.FillDropDownList(oDT, ref ddlStandard, Constants.S_STANDARD_ID_FIELD, Constants.S_STANDARD_NAME_FIELD, Constants.S_SELECT);
        FillDefaultDivisionValue();
    }

    private void Filldivision()
    {
        DataTable oDtStandardCollection = moOnlineExamQuestionConfigurationBL.GetAssociatedStandards(ddlStandard.SelectedValue.ToInt());
        ControlUtility.FillDropDownList(oDtStandardCollection, ref ddlDivision, Constants.S_STANDARD_DIVISION_ID_FIELD, Constants.S_DIVISION_NAME_FIELD, Constants.S_ALL);
    }

    ///// <summary>
    ///// This method  is used to fill Class combobox.
    ///// </summary>
    private void FillSubjectCombo()
    {
        OnlineExamConfigurationBL aoOnlineExamConfigurationBL = new OnlineExamConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
        List<YearWiseSubjectsDetails> lstSubjects = aoOnlineExamConfigurationBL.GetAllYearwiseSubjects(ddlStandard.SelectedValue.ToInt(), ddlDivision.SelectedValue.ToInt());
        ListSource.FillDropDownList(lstSubjects, ddlSubject, "SubjectName", "SubjectId", Constants.S_SELECT);

        //List<YearWiseSubjectsDetails> lstSubjects = moOnlineExamQuestionConfigurationBL.GetAllYearwiseSubjects();
        //ListSource.FillDropDownList(lstSubjects, ddlSubject, "SubjectName", "SubjectId", Constants.S_SELECT);
    } 

    //#endregion 
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SubmitQuestions(true);
        lblUpdateMessage.Text = "Question details submitted successfully !!!";
    }
    protected void btnUnsubmit_Click(object sender, EventArgs e)
    {
        SubmitQuestions(false);
        lblUpdateMessage.Text = "Question details unsubmitted successfully !!!";
    }

    private void SubmitQuestions(bool abIsSubmit)
    {
        moOnlineExamQuestionConfigurationBL.Submit(ddlStandard.SelectedValue.ToInt(), ddlDivision.SelectedValue.ToInt(), ddlSubject.SelectedValue.ToInt(), abIsSubmit);
        FillOnlineExamQuestion();
    }

    private string SaveFileOnServer(string asFileName, FileUpload FileUpload, HiddenField hidFilePath)
    {
        // Upload the file to the server.
        string sFolderName = base.BasePath + S_UPLOAD_FILE_FOLDER_PATH;
        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;
        if (File.Exists(sServerFilePath))
        {
            sFileName = CommonUtility.GetFileNameForRenaming(asFileName);
            sServerFilePath = sFolderName + sFileName;
        }

        FileUpload.SaveAs(sServerFilePath);    
        // delete exesting logo
        string sFileToDelete = base.BasePath + hidFilePath.Value;
        if (File.Exists(sFileToDelete))        
            File.Delete(sFileToDelete);
        
        return sFileName;

    }    
}