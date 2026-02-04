using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SchoolEntities.OnlineExam;
using BusinessLogic;
using Utility;
using System.Web.UI.HtmlControls;
using SchoolEntities;
public partial class OnlineExamResultUI : SchoolBase
{
    PublishOnlineExamBL moPublishOnlineExamBL;
    protected void Page_Load(object sender, EventArgs e)
    {
        moPublishOnlineExamBL = new PublishOnlineExamBL(miSchoolId, miAcademicYearId,miUserId);
        if (!IsPostBack)
        {
            FillResult();
            SetJavascriptAttributes();
        }
    }

    private void SetJavascriptAttributes()
    {
        btnBack.PostBackUrl = "PublishOnlineExamUI.aspx?" + Request.QueryString;
    }

    private void FillResult()
    {
        List<StudentInfo> lstStudentInfo = moPublishOnlineExamBL.GetAllStudentsForClass(QueryString["StdDivId"].ToInt(), QueryString["ExamId"].ToInt(), QueryString["SubjectId"].ToInt());
        lstvwStudent.DataSource = lstStudentInfo;
        lstvwStudent.DataBind();
    }
    protected void lstvwStudent_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            StudentInfo oStudentInfo = e.Item.DataItem as StudentInfo;

            HtmlTableRow tr = e.Item.FindControl("trAnswerDetails") as HtmlTableRow;
            HtmlTableCell td = tr.FindControl("tdAnswerDetails") as HtmlTableCell;
            ListView oListView = td.FindControl("lstvwAnswerDetails") as ListView;

            List<OnlineExamResult> lstOnlineExamResult = moPublishOnlineExamBL.ExamResults.Where(er => er.StudentId == oStudentInfo.StudentId).ToList();
            oListView.DataSource = lstOnlineExamResult;
            oListView.DataBind();
        }
    }

    protected void lstvwAnswerDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            OnlineExamResult oOnlineExamResult = e.Item.DataItem as OnlineExamResult;

            Label lblAnswer = e.Item.FindControl("lblAnswer") as Label;
            Image imgAttachment = e.Item.FindControl("imgAttachment") as Image;
            Image imgQuestionAttachment = e.Item.FindControl("imgQuestionAttachment") as Image;

            if (oOnlineExamResult.QuestionAttachmentPath != string.Empty)
            {
                imgQuestionAttachment.Visible = true;
                imgQuestionAttachment.ImageUrl = "../Uploads/OnlineExamImages/" + oOnlineExamResult.QuestionAttachmentPath;
            }

            if (oOnlineExamResult.AnswerTypeId == 1)
            {
                if (oOnlineExamResult.IsCorrectAnswer)
                    lblAnswer.Style.Add("Color", "Green");
                else
                    lblAnswer.Style.Add("Color", "Red");

                lblAnswer.Visible = true;
                imgAttachment.Visible = false;
            }
            else
            {
                imgAttachment.ImageUrl = "../Uploads/OnlineExamImages/" + oOnlineExamResult.AnswernAttachmentPath;
                lblAnswer.Visible = false;
                imgAttachment.Visible = true;
            }
        }
    }
}