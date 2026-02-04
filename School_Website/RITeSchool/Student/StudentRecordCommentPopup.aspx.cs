/* File Name - StudentRecordCommentPopup.aspx.cs
 * Created By - Sachin
 * Created Date - 4-Jun-2018
 * Description - This class is used to manupulate comment.
 */
using System;
using System.Reflection;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class StudentRecordCommentPopup : SchoolBase
{
    #region Constants

    private const string S_TIME_FORMAT = "hh:mm tt";

    #endregion

    #region Data Member(s)

    private StudentRecordBL moStudentRecordBL; 

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to show comment details if it is in update mode.
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
                if (!IsPostBack)
                {
                    SetJavascriptAttributes();
                    SetFields();
                    if (QueryString["CommentId"].ToInt() == 0)
                        btnDelete.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save comment and close popup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save(false);
            ClosePopup();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete comment.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int iSchoolwiseStudentId = QueryString["SchoolwiseStudentId"].ToInt();
            int iCommentId = QueryString["CommentId"].ToInt();
            moStudentRecordBL.DeleteComment(iSchoolwiseStudentId, iCommentId);
            ClosePopup();
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
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Submit();
            ClosePopup();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save and submit comment.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveAndSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Save(true);
            ClosePopup();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to submit comment.
    /// </summary>
    private void Submit()
    {
        moStudentRecordBL.Submit(QueryString["SchoolwiseStudentId"].ToInt(), hidCommentId.Value.ToInt(), false);
    }

    /// <summary>
    /// This method is used to save comment.
    /// </summary>
    /// <param name="abAllowSubmit"></param>
    private void Save(bool abAllowSubmit)
    {
        int iSchoolwiseStudentId = QueryString["SchoolwiseStudentId"].ToInt();
        int iCommentId = QueryString["CommentId"].ToInt();

        StudentRecordComment oStudentRecordCommnet = new StudentRecordComment
        {
            Date = Convert.ToDateTime(txtDate.Text + " " + txtTime.Text.ToString()),
            Comment = txtComment.Text.Trim(),
            LectureName = txtLectureName.Text.Trim()
        };

        string sStdDivId = QueryString["StdDivId"].ToString();
        moStudentRecordBL.SaveComment(iSchoolwiseStudentId, iCommentId, oStudentRecordCommnet, abAllowSubmit, sStdDivId);
    }

    /// <summary>
    /// This method is used to close popup.
    /// </summary>
    private void ClosePopup()
    {
        string sQuerystring = "SchoolwiseStudentId=" + QueryString["SchoolwiseStudentId"].ToString() + "&IsReadMode=" + QueryString["IsReadMode"].ToString() + "&IsPrincipal=" + QueryString["IsPrincipal"].ToString() + "&IsCounsellor=" + QueryString["IsCounsellor"].ToString() + "&IsClassTeacher=" + QueryString["IsClassTeacher"].ToString() +
            "&StdDivId=" + QueryString["StdDivId"].ToString() + "&Filter=" + QueryString["Filter"].ToString() + "&ShowOnlySavedRecord=" + QueryString["ShowOnlySavedRecord"].ToString();
        sQuerystring = CommonUtility.EncryptQuerystring(sQuerystring);
        sQuerystring = string.Format("'?{0}'", sQuerystring);
        Response.Write(string.Format("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+{0};window.close();window.opener.focus(); </Script>", sQuerystring));
    }

    /// <summary>
    /// This method is used to set fields.
    /// </summary>
    private void SetFields()
    {
        int iSchoolwiseStudentId = QueryString["SchoolwiseStudentId"].ToInt();
        int iCommentId = QueryString["CommentId"].ToInt();
        hidCurrentTime.Value = DateTime.Now.ToString();
        if (iCommentId != 0)
        {
            StudentRecordComment oStudentRecordCommnet = moStudentRecordBL.GetCommentDetails(iSchoolwiseStudentId, iCommentId);
            txtDate.Text = oStudentRecordCommnet.Date.ToString(Constants.S_DATE_FORMAT);
            txtTime.Text = oStudentRecordCommnet.Date.ToString(S_TIME_FORMAT);
            txtComment.Text = oStudentRecordCommnet.Comment;
            txtLectureName.Text = oStudentRecordCommnet.LectureName;            
            btnSubmit.Enabled = true;
        }
        else
        {
            txtTime.Text = DateTime.Now.ToString(S_TIME_FORMAT);
            txtDate.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);
        }

        hidSelectedDateTime.Value = txtDate.Text + " " + txtTime.Text;
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        txtDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        ValSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
        btnSave.Attributes.Add("önclick", "ConfirmSubmit();");
        hidCommentId.Value = QueryString["CommentId"].ToString();
    } 

    #endregion
}