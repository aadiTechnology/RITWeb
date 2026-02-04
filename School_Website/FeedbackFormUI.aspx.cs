/* File Name - FeedbackFormUI.aspx.cs
 * Created Date - 11-Apr-2017
 * Created By - Sachin
 * Description - This class is used to submit feedback details.
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class FeedbackFormUI : SchoolBase
{
    #region Data Member(s)

    private ParentFeedbackBL moParentFeedbackBL;
    private int miRowIndex = 0;
    private int miOldParentQuestionId = 0;

    #endregion

    #region Event(s)
    
    /// <summary>
    /// This event is used to display feedback questions.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moParentFeedbackBL = new ParentFeedbackBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {
                ReadQueryString();
                FillFeedbackDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set conftrols visibility.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwParameters_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ParentFeedbackQuestion oParentFeedbackQuestion = e.Item.DataItem as ParentFeedbackQuestion;
                DropDownList cmbGrade = e.Item.FindControl("cmbGrade") as DropDownList;
                TextBox txtDescription = e.Item.FindControl("txtDescription") as TextBox;
                Label lblTitle = e.Item.FindControl("lblTitle") as Label;
                Label lblSrNo = e.Item.FindControl("lblSrNo") as Label;
                CheckBox chkOption = e.Item.FindControl("chkOption") as CheckBox;

                if (oParentFeedbackQuestion.ControlId == 1)
                {
                    ListSource.FillDropDownList(moParentFeedbackBL.ParentFeedbackGrades, cmbGrade, "Name", "Id", Constants.S_SELECT);
                    cmbGrade.Visible = true;
                }
                else if (oParentFeedbackQuestion.ControlId == 2)
                    txtDescription.Visible = true;
                else if (oParentFeedbackQuestion.ControlId == 3)
                {
                    chkOption.Visible = true;
                    chkOption.Attributes.Add("onclick","SetFieldState("+e.Item.DisplayIndex+")");
                }

                if (oParentFeedbackQuestion.ParentQuestionId != miOldParentQuestionId)
                {
                    miOldParentQuestionId = oParentFeedbackQuestion.ParentQuestionId;
                    miRowIndex = 1;
                }
                else
                    miRowIndex++;

                HtmlTableRow Tr2 = e.Item.FindControl("Tr2") as HtmlTableRow;

                if (oParentFeedbackQuestion.ParentQuestionId != 0)
                {
                    lblSrNo.Text = miRowIndex.ToString();
                    if (Tr2 != null)
                        Tr2.Attributes.Add("class", "ProgressReportParameter");
                }
                else
                {
                    lblTitle.Font.Bold = true;
                    if (Tr2 != null)
                        Tr2.Attributes.Add("class", "ProgressReportRow");
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (ListViewDataItem oItem in lstvwParameters.Items)
            {
                if (oItem.ItemType == ListViewItemType.DataItem)
                {
                    DropDownList cmbGrade = oItem.FindControl("cmbGrade") as DropDownList;
                    TextBox txtDescription = oItem.FindControl("txtDescription") as TextBox;
                    CheckBox chkOption = oItem.FindControl("chkOption") as CheckBox;

                    if (cmbGrade != null && cmbGrade.Visible)
                        cmbGrade.ClearSelection();

                    if (txtDescription != null && txtDescription.Visible)
                        txtDescription.Text = string.Empty;

                    if (chkOption != null && chkOption.Visible)
                        chkOption.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear session.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/eSchoolLogin.aspx");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit feedback details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            List<ParentFeedbackDetails> lstFeedbackDetails = new List<ParentFeedbackDetails>();
            foreach (ListViewDataItem oItem in lstvwParameters.Items)
            {
                if (oItem.ItemType == ListViewItemType.DataItem)
                {
                    DropDownList cmbGrade = oItem.FindControl("cmbGrade") as DropDownList;
                    TextBox txtDescription = oItem.FindControl("txtDescription") as TextBox;
                    CheckBox chkOption = oItem.FindControl("chkOption") as CheckBox;

                    int iParentQuestionId = lstvwParameters.DataKeys[oItem.DisplayIndex]["ParentQuestionId"].ToInt();

                    if (iParentQuestionId != 0)
                    {
                        ParentFeedbackDetails oParentFeedbackDetails = new ParentFeedbackDetails();
                        oParentFeedbackDetails.QuestionId = lstvwParameters.DataKeys[oItem.DisplayIndex]["Id"].ToInt();

                        if (cmbGrade != null && cmbGrade.Visible)
                            oParentFeedbackDetails.GradeId = cmbGrade.SelectedValue.ToInt();

                        if (txtDescription != null && txtDescription.Visible)
                            oParentFeedbackDetails.Remark = txtDescription.Text.Trim();

                        if (chkOption != null && chkOption.Visible)
                            oParentFeedbackDetails.GradeId = (chkOption.Checked ? 1 : 0);

                        lstFeedbackDetails.Add(oParentFeedbackDetails);
                    }
                }
            }

            if (lstFeedbackDetails.Count > 0)
            {
                moParentFeedbackBL.Save(miUserId, base.GenerateXml(lstFeedbackDetails), 1);
                if (hidIsFromTerms.Value == Constants.S_ONE)
                    Response.Redirect("~/RITeSchool/Common/StudentChangePassword.aspx");
                else
                    Response.Redirect("~/RITeSchool/Common/ControlPanel.aspx");
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
    /// This method is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["FromTermsOfUse"] != null)
            hidIsFromTerms.Value = QueryString["FromTermsOfUse"].ToString();
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// This method is used to fill feedback details.
    /// </summary>
    private void FillFeedbackDetails()
    {
        List<ParentFeedbackQuestion> lstQuestions = moParentFeedbackBL.GetAll(miUserId, 1);
        lstvwParameters.DataSource = lstQuestions;
        lstvwParameters.DataBind();
    } 

    #endregion
}