///File Name  : BlackListedStudentUI.aspx.cs
//// Created By : Rutuja
//// Date       : 18 sep 2023
//// Description : This class is used to display blacklisted students list.

using System;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Admin;
using Utility;

public partial class BlackListedStudentUI : SchoolBase
{
    #region Constants

    private const string S_COMMAND_REMOVE = "REMOVE";
    private const string S_COMMAND_ADD = "ADD";
    private const string S_COMMAND_UPDATE = "UPDATESTUDENT";
    
    #endregion

    #region Data Member

    private StudentBL moStudentBL;

    #endregion

    #region Events

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
                hidSortExpression.Value = "SchoolLeft_Date";
                hidSortDirection.Value = Constants.S_DESCENDING;
            }

            AddSortImage(lstvwBlackListedStudents, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }

    }
    
    /// <summary>
    /// This event is used to display details at page load.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetDefaultValues();
                FillBlackListedStudent();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillBlackListedStudent();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to select blacklisted student details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBlackListedStudents_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iId = Convert.ToInt32(lstvwBlackListedStudents.DataKeys[e.Item.DisplayIndex]["Id"]);
                hidId.Value = iId.ToString();

                int iSchoolwiseStudentId = Convert.ToInt32(lstvwBlackListedStudents.DataKeys[e.Item.DisplayIndex]["SchoolwiseStudentId"]);
                string sComment = (e.Item.FindControl("txtRemark") as TextBox).Text.Trim();

                if (e.CommandName == S_COMMAND_REMOVE)
                {
                    UpdateBlackListStudent(iId, 3, iSchoolwiseStudentId, sComment);
                    lblUpdate.Text = "Student removed successfully!!!";
                }
                else if (e.CommandName == S_COMMAND_ADD)
                {
                    UpdateBlackListStudent(iId, 2, iSchoolwiseStudentId, sComment);
                    lblUpdate.Text = "Student added successfully!!!";
                }
                else if (e.CommandName == S_COMMAND_UPDATE)
                {
                    UpdateBlackListStudent(iId, 1, iSchoolwiseStudentId, sComment);
                    lblUpdate.Text = "Comment updated successfully!!!";
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to select page no.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwBlackListedStudents);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill pager footer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBlackListedStudents_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwBlackListedStudents.Items.Count > 0)
                ControlUtility.FillListViewPagerFooter(lstvwBlackListedStudents, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    ///  This event is used to sort variables.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBlackListedStudents_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;
            base.RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill black listed students.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBlackListedStudents_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                BlackListedStudent oBlackListedStudent = e.Item.DataItem as BlackListedStudent;
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                LinkButton lnkRemove = oCurrentItem.FindControl("lnkRemove") as LinkButton;
                lnkRemove.Attributes.Add("Onclick", "if(!ConfirmDelete()) {return false;}");

                LinkButton lnkAdd = oCurrentItem.FindControl("lnkAdd") as LinkButton;
                LinkButton lnkUpdate = oCurrentItem.FindControl("lnkUpdate") as LinkButton;

                if (oBlackListedStudent.Id == 0)
                {
                    lnkUpdate.Visible = false;
                    lnkRemove.Visible = false;
                    lnkAdd.Visible = true;
                }
                else
                {
                    lnkUpdate.Visible = true;
                    lnkRemove.Visible = true;
                    lnkAdd.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillBlackListedStudent();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to delete student from black list.
    /// </summary>
    /// <param name="aiId"></param>
    private void UpdateBlackListStudent(int aiId, int aiActionId, int aiSchoolwiseStudentId, string asComment)
    {
        StudentBL moStudentBL = new StudentBL();
        moStudentBL.UpdateBlackListStudent(miSchoolId, aiId, miUserId, aiActionId, aiSchoolwiseStudentId, asComment);
        FillBlackListedStudent();
    }

    /// <summary>
    ///  This method is used to set sort variables.
    /// </summary> 
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to fll listview of blacklisted student.
    /// </summary>
    private void FillBlackListedStudent()
    {
        lstvwBlackListedStudents.DataSourceID = objdsStudentList.ID;
        lstvwBlackListedStudents.DataBind();
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        BtnBack.PostBackUrl = "AllStudentsUI.aspx";

        if (QueryString["IsFromStudentScreen"] != null && QueryString["IsFromStudentScreen"].ToString() == Constants.S_YES)
            BtnBack.Visible = true;
    }

    #endregion
}