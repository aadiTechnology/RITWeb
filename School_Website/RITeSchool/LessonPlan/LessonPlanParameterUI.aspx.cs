/*File Name - LessonPlanParameter.aspx.cs
 * Created By - Sanket Bhujbal
 * Created Date - 17 June 2015
 * Description - This class is used to manage performance parameters.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LessonPlanEntities;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using BusinessLogic.LessionPlan;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
public partial class LessonPlanParameterUI : SchoolBase
{
    #region Data Members(s)

    private LessonPlanParameterBL moLessonPlanParameterBL;

    #endregion

    #region Event(s)

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
                hidSortExpression.Value = "SortOrder";
                hidSortDirection.Value = Constants.S_ASCENDING;
            }

            AddSortImage(lstvwParameter, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This event is used to fill ComboBox and parameter list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!CheckPreCondition())
            {
                moLessonPlanParameterBL = new LessonPlanParameterBL(miSchoolId, miUserId, miAcademicYearId);
                if (!IsPostBack)
                {
                    SetJavaScriptAttribute();
                    GetCategories();
                    GetSubjectCategories();
                    FillParentLessonPlan();
                    FillParameterListview();
                  
                }
            }
            else
            {
            
                base.ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave, btnBack, btnSubmit, btnUnSubmit });
                btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.LessonPlanRelated));
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save parameter
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            FillParentLessonPlan();

            if (QueryString[Constants.S_IS_CONFIGURED] != Constants.S_YES)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.LessonPlanParameter));

            DisplayMessage(hidParameterId.Value == Constants.S_ZERO ? Constants.ItemState.saved : Constants.ItemState.updated, false);

            ClearFields();
            FillParameterListview();

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill up parameter list view according to selected Lesson Plan Category.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillParameterListview();
            FillParentLessonPlan();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    protected void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillParameterListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to edit / delete selected records.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwParameter_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iParameterId = lstvwParameter.DataKeys[e.Item.DisplayIndex]["Id"].ToInt();

                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    Label lblTitle = e.Item.FindControl("lblTitle") as Label;
                    Label lblSortOrder = e.Item.FindControl("lblSortOrder") as Label;

                    txtParameter.Text = HttpUtility.HtmlDecode(lblTitle.Text);
                    txtSortOrder.Text = HttpUtility.HtmlDecode(lblSortOrder.Text);
                    hidParameterId.Value = iParameterId.ToString();

                    cmbCategory.SelectedValue = lstvwParameter.DataKeys[e.Item.DisplayIndex]["LessonPlanCategoryId"].ToString();
                    cmdAppliedtosubject.SelectedValue = lstvwParameter.DataKeys[e.Item.DisplayIndex]["SubjectCategoryId"].ToString();
                    ddlParentlessonPlanId.SelectedValue = lstvwParameter.DataKeys[e.Item.DisplayIndex]["ParentParameterId"].ToString();
                    btnSave.Text = Constants.ButtonText.Update.ToString();
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moLessonPlanParameterBL.Delete(iParameterId, Constants.SchoolConfigurations.LessonPlanParameter.ToInt());
                    if (iParameterId == hidParameterId.Value.ToInt())
                        ClearFields();
                    DisplayMessage(Constants.ItemState.deleted, false);
                    FillParameterListview();
                }
            }
        }
        catch (SqlException se)
        {
            base.DisplayMessage(se.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
   }
    /// <summary>
    /// This event is used to set attributes on delete button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    protected void lstvwParameter_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {   
                LessonPlanParameters oPerformanceParameter = e.Item.DataItem as LessonPlanParameters;
                ImageButton btnIsSubmitted = e.Item.FindControl("btnIsSubmitted") as ImageButton;
                btnIsSubmitted.Visible = oPerformanceParameter.IsSubmitted;

                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

                ImageButton btnEdit = e.Item.FindControl("btnEdit") as ImageButton;

                int IParentParameter = lstvwParameter.DataKeys[e.Item.DisplayIndex]["ParentParameterId"].ToInt();
                Label lblParentParameter = e.Item.FindControl("lblParentParameter") as Label;
                if (IParentParameter == 0)
                    lblParentParameter.Text = "-";

                if (oPerformanceParameter.IsSubmitted)
                {
                    btnDelete.Visible = false;
                    btnEdit.Visible = false;
                }
                else
                {
                    btnDelete.Visible = true;
                    btnEdit.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear the fields
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit parameters
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            SubmitParameters(true);
            DisplayMessage("submitted", false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to un submit parameters
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            SubmitParameters(false);
            DisplayMessage("unsubmitted", false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This method is used to manage sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwParameter_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;
            base.RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
            FillParameterListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill listview based on selected section.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbSection_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            FillParameterListview();
            FillParentLessonPlan();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to display message
    /// </summary>
    /// <param name="aoItemState"></param>
    /// <param name="abIsErrorMessage"></param>
    private void DisplayMessage(Constants.ItemState aoItemState, bool abIsErrorMessage)
    {
        string sMessage = "Lesson Plan parameter " + aoItemState.ToString() + " successfully!!!";
        DisplayMessage(sMessage, abIsErrorMessage, tdMessage);
    }

    /// <summary>
    /// This method is set java script attributes
    /// </summary>
    private void SetJavaScriptAttribute()
    {
        btnCancel.Attributes.Add("onclick", "ClearFields();");
        base.ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave, btnBack, btnSubmit, btnUnSubmit });
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.LessonPlanRelated));
        //btnBackUp.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.LessonPlanRelated));
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidParameterId.Value = Constants.S_ZERO;
        btnSave.Attributes.Add("onclick", "ClearMessage()");
       cmbCategory.Focus();
    }
    /// <summary>
    /// This method is used to fill Lesson Plan parameter list view.
    /// </summary>
   private void FillParameterListview()
   {
       List<LessonPlanParameters> lstParameters = moLessonPlanParameterBL.GetAll(cmbCategory.SelectedValue.ToInt(),cmbSection.SelectedValue.ToInt());
       if (hidSortExpression.Value == "SortOrder" || hidSortExpression.Value == string.Empty)
       {
           if (hidSortDirection.Value == Constants.S_ASCENDING || hidSortDirection.Value == string.Empty)
               lstParameters = lstParameters.OrderBy(pt => pt.SortOrder).ToList();
           else
               lstParameters = lstParameters.OrderByDescending(pt => pt.SortOrder).ToList();
       }
       else
       {
           if (hidSortDirection.Value == Constants.S_ASCENDING || hidSortDirection.Value == string.Empty)
               lstParameters = lstParameters.OrderBy(pt => pt.Title).ToList();
           else
               lstParameters = lstParameters.OrderByDescending(pt => pt.Title).ToList();
       }
       lstvwParameter.DataSource = lstParameters;
       lstvwParameter.DataBind();
       SetSubmitBussonState(lstParameters);
   }
  /// <summary>
    /// This method is used to set submit/unsubmit button state.
    /// </summary>
    /// <param name="alstParameters"></param>
    private void SetSubmitBussonState(List<LessonPlanParameters> alstParameters)
    {
        if (alstParameters.Count > 0)
        {
            if (alstParameters.FindAll(prm => !prm.IsSubmitted).Any() == true)
            {
                btnSubmit.Enabled = true;
                btnUnSubmit.Enabled = false;
            }
            else
            {
                btnSubmit.Enabled = false;
                btnUnSubmit.Enabled = true;
            }
        }
        else
        {
            btnSubmit.Enabled = false;
            btnUnSubmit.Enabled = false;
        }
    }

    /// <summary>
    /// This method is used to save parameter details.
    /// </summary>
    private void Save()
    {
        LessonPlanParameters oLessonPlanParameterBL = new LessonPlanParameters
        {
            Id = hidParameterId.Value.ToInt(),
            Title = txtParameter.Text.Trim(),
            SortOrder = txtSortOrder.Text.ToInt(),
            LessonPlanCategoryId = cmbCategory.SelectedValue.ToInt(),
            LessonPlanSectionId = cmbSection.SelectedValue.ToInt(),
            SubjectCategoryId=cmdAppliedtosubject.SelectedValue.ToInt(),
            ParentParameterId=ddlParentlessonPlanId.SelectedValue.ToInt(),
        };
        moLessonPlanParameterBL.Save(oLessonPlanParameterBL);
    }

    /// <summary>
    /// This method is used to clear fields
    /// </summary>
    private void ClearFields()
    {
        txtParameter.Text = string.Empty;
        txtSortOrder.Text = string.Empty;
        ddlParentlessonPlanId.ClearSelection();
        hidParameterId.Value = Constants.S_ZERO;
        btnSave.Text = Constants.ButtonText.Save.ToString();
    }

    /// <summary>
    /// This method is used to fill ComboBox
    /// </summary>
    private void GetCategories()
    {
        List<LessonPlanCategory> lstCatgory = moLessonPlanParameterBL.GetCategories();
        ListSource.FillDropDownList(lstCatgory, cmbCategory, "Name", "Id", string.Empty);
    }
    private void FillParentLessonPlan()
    {
         moLessonPlanParameterBL = new LessonPlanParameterBL(miSchoolId, miUserId, miAcademicYearId);
         DataTable dt = moLessonPlanParameterBL.GetParentLessonPlan(cmbCategory.SelectedValue.ToInt());
         ListSource.FillDropDownList(dt, ddlParentlessonPlanId, "Title", "Id", Constants.S_SELECT);
   }
    /// <summary>
    /// This method is used to fill ComboBox
    /// </summary>
    private void GetSubjectCategories()
    {
        List<LessonSubjectCategories> lstSubjectCategories = moLessonPlanParameterBL.GetSubjectCategories();
        ListSource.FillDropDownList(lstSubjectCategories, cmdAppliedtosubject, "Name", "Id", string.Empty);
    }
    /// <summary>
    /// This method is used to check pre-condition to configure association.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        //bool bReturn = false;
        //string sLinks = ReferenceBL.GetPreConditionMsg(Utility.Constants.SchoolConfigurations.PerformanceParameter);

        //if (!sLinks.Equals(string.Empty))
        //{
        //    trPreCondition.Visible = true;
        //    divErr.InnerHtml = sLinks;
        //    trControls.Visible = false;
        //    bReturn = true;
        //}
        //else
        //{
        //    divErr.Visible = false;
        //    trControls.Visible = true;
        //    trPreCondition.Visible = false;
        //}

        //return bReturn;

        return false;
    }

    /// <summary>
    /// This method is used to display message.
    /// </summary>
    /// <param name="asButtonName"></param>
    /// <param name="abIsErrorMessage"></param>
    private void DisplayMessage(string asButtonName, bool abIsErrorMessage)
    {
        string sMessage = "Lesson Plan parameter " + asButtonName + " successfully!!!";
        DisplayMessage(sMessage, abIsErrorMessage, tdMessage);
    }

    /// <summary>
    /// This method is used to submit parameters.
    /// </summary>
    /// <param name="abSubmit"></param>
    private void SubmitParameters(bool abSubmit)
    {
        int iLessonPlanCategoryId = Convert.ToInt32(cmbCategory.SelectedValue);
        moLessonPlanParameterBL.Submit(iLessonPlanCategoryId, abSubmit);
        FillParameterListview();
    }

    #endregion
}
