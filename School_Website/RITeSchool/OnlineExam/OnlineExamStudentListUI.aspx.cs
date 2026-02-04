using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using BusinessLogic.Exceptions;
using Utility;
using BusinessLogic;
using System.Reflection;
using System.Web.UI.HtmlControls;
using SchoolEntities;
using SchoolEntities.OnlineExam;
using System.IO;

public partial class OnlineExamStudentListUI : SchoolBase
{
    #region Constant(s)

    OnlineExamWiseQueConfigBL moOnlineExamWiseQueConfigBL; 
    const string S_COMMAND_DETAILS = "SelectCommand";
    private const string S_FOLDER_PATH = @"../Uploads/OnlineExamImages/";
    private const string S_SAVE_MESSAGE = "Student mark details saved successfully!!!";
    int miUserid;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill the Controls.
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
                ReadyQueryString();
                FillListview();
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
  
    /// <summary>
    /// This event is used to Save data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            moOnlineExamWiseQueConfigBL = new OnlineExamWiseQueConfigBL(miSchoolId, miAcademicYearId);
            SaveDetails(sender);
            base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);

            foreach (ListViewItem item in lstVwStudentList.Items)
            {
                HtmlTableRow oHtmlTableRow = item.FindControl("trtxtQty") as HtmlTableRow;
                HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdtxtQty") as HtmlTableCell;
                ListView lstvwQuestionDetails = oHtmlTableCell.FindControl("lstVwQuestionDetails") as ListView;

                HtmlTableRow oHtmlAttachment = item.FindControl("trDescriptionAttachment") as HtmlTableRow;
                HtmlTableRow oHtmlBtnSave = item.FindControl("trbtnSave") as HtmlTableRow;

                oHtmlAttachment.Visible = false;
                oHtmlBtnSave.Visible = false;
                lstvwQuestionDetails.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bind data to student list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstVwStudentList_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListView lstVwStudentList = sender as ListView;
                LinkButton lnkDescription = e.Item.FindControl("lnkDescription") as LinkButton;
                Button BtnSave = e.Item.FindControl("BtnSave") as Button;

                string sFileName = Convert.ToString(lstVwStudentList.DataKeys[e.Item.DisplayIndex]["DescriptionFileName"]);
                string sNewFileName = S_FOLDER_PATH + sFileName;
                lnkDescription.Attributes.Add("onclick", "OpenWindow('" + sNewFileName + "'); return false;");

                if (hidIsPublished.Value == "Y")
                    BtnSave.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bind data to inner list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstVwQuestionDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {   
                TextBox txtMarks = e.Item.FindControl("txtMarks") as TextBox;

                if (txtMarks.Text == Constants.S_ZERO)
                    txtMarks.Text = string.Empty;

                if (hidIsPublished.Value == "Y")
                {
                    txtMarks.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This event is used to list view command.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstVwStudentList_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            moOnlineExamWiseQueConfigBL = new OnlineExamWiseQueConfigBL(miSchoolId, miAcademicYearId);
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            int iStudentid = Convert.ToInt32(lstVwStudentList.DataKeys[iRowId]["YearWise_Student_Id"]);
            int aiQuestionId = Convert.ToInt32(lstVwStudentList.DataKeys[iRowId]["QuestionId"]);

            foreach (ListViewItem list in lstVwStudentList.Items)
            {
                HtmlTableRow oHtmlTable = list.FindControl("trtxtQty") as HtmlTableRow;
                HtmlTableRow oHtmlDescription = list.FindControl("trDescriptionAttachment") as HtmlTableRow;
                HtmlTableRow oHtmlSaveBtn = list.FindControl("trbtnSave") as HtmlTableRow;

                if (oHtmlTable != null)
                {
                    oHtmlTable.Visible = false;
                    oHtmlDescription.Visible = false;
                    oHtmlSaveBtn.Visible = false;
                }
            }


            HtmlTableRow oHtmlTableRow = e.Item.FindControl("trtxtQty") as HtmlTableRow;
            HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdtxtQty") as HtmlTableCell;
            ListView olstVwQuestionDetails = oHtmlTableCell.FindControl("lstVwQuestionDetails") as ListView;

            HtmlTableRow oHtmlAttachment = e.Item.FindControl("trDescriptionAttachment") as HtmlTableRow;
            HtmlTableRow oHtmlBtnSave = e.Item.FindControl("trbtnSave") as HtmlTableRow;

            oHtmlTableRow.Visible = false;
            oHtmlAttachment.Visible = false;
            oHtmlBtnSave.Visible = false;

            if (e.CommandName == S_COMMAND_DETAILS)
            {
                DataTable dt = moOnlineExamWiseQueConfigBL.GetAllStudentQuestionList(iStudentid, hidSubjectId.Value.ToInt());
                if (dt.Rows.Count > Constants.I_ZERO)
                {
                    oHtmlTableRow.Visible = true;
                    oHtmlTableCell.Visible = true;
                    olstVwQuestionDetails.Visible = true;

                    olstVwQuestionDetails.DataSource = dt;
                    olstVwQuestionDetails.DataBind();                    

                    oHtmlAttachment.Visible = true;
                    oHtmlBtnSave.Visible = true;
                }
                else
                {
                    oHtmlAttachment.Visible = false;
                    oHtmlBtnSave.Visible = false;
                }                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to save details.
    /// </summary>
    protected void SaveDetails(object sender)
    {
        List<DescriptionAnswerDetails> lstDescriptionAnswerDetails = new List<DescriptionAnswerDetails>();
        HtmlTableCell oHtmlTableCell = (sender as Button).Parent as HtmlTableCell;
        ListView lstvwQuestionDetails = oHtmlTableCell.FindControl("lstVwQuestionDetails") as ListView;
        foreach (ListViewItem Questiondetails in lstvwQuestionDetails.Items)
        {
            DescriptionAnswerDetails oDescriptionAnswerDetails = new DescriptionAnswerDetails();
            int iQuestionid = Convert.ToInt32(lstvwQuestionDetails.DataKeys[Questiondetails.DisplayIndex]["QuestionId"]);
            int iAnswerId = Convert.ToInt32(lstvwQuestionDetails.DataKeys[Questiondetails.DisplayIndex]["AnswerId"]);
            int iQuestionAnswerId = Convert.ToInt32(lstvwQuestionDetails.DataKeys[Questiondetails.DisplayIndex]["QuestionAnswerId"]);
            TextBox txtMarks = Questiondetails.FindControl("txtMarks") as TextBox;
            oDescriptionAnswerDetails.QuestionId = iQuestionid;
            oDescriptionAnswerDetails.AnswerId = iAnswerId;
            oDescriptionAnswerDetails.QuestionAnswerId = iQuestionAnswerId;
            oDescriptionAnswerDetails.Marks = txtMarks.Text.Trim().ToInt();

            lstDescriptionAnswerDetails.Add(oDescriptionAnswerDetails);
        }
        moOnlineExamWiseQueConfigBL.SaveStudentsQuestionMarks(base.GenerateXml(lstDescriptionAnswerDetails), miUserid);
    }

    /// <summary>
    /// This method is used to fill student list view.
    /// </summary>
    private void FillListview()
    {   
        DataTable dt = moOnlineExamWiseQueConfigBL.GetAllStudentList(hidStdDivId.Value.ToInt(), hidSubjectId.Value.ToInt());
        lstVwStudentList.DataSource = dt;
        lstVwStudentList.DataBind();
    }    

    /// <summary>
    /// This method is used to read Query string.
    /// </summary>
    private void ReadyQueryString()
    {
        if (QueryString["StdDivId"] != null)
            hidStdDivId.Value = QueryString["StdDivId"];

        if (QueryString["SubjectId"] != null)
            hidSubjectId.Value = QueryString["SubjectId"];

        if (QueryString["IsPublished"] != null)
            hidIsPublished.Value = QueryString["IsPublished"];
    }

    /// <summary>
    /// This method is used to set javascript attributes to controls.
    /// </summary>
    private void SetJavascriptAttributes()
    {   
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnBack.PostBackUrl = "PublishOnlineExamUI.aspx?" + Request.QueryString;
    }

    #endregion    
}