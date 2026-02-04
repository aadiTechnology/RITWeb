/*File Name - PerformanceParametersUI.aspx.cs
 * Created By - Sachin
 * Created Date - 17 Sept 2013
 * Description - This class is used to manage performance parameters.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using StaffPerformanceEntity;
using Utility;
using SchoolEntities;

public partial class PerformanceParametersUI : SchoolBase
{
    #region Constant(s)
    
    private const string S_SUBMIT = "Submit";
    private const string S_UN_SUBMIT = "Un Submit"; 

    #endregion

    #region Data Member(s)

    private PerformanceParameterBL moPerformanceParameterBL;

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
                hidSortExpression.Value = "SortOrder";

            if (hidSortDirection.Value == string.Empty)
                hidSortDirection.Value = Constants.S_ASCENDING;

            base.AddSortImage(lstvwParameter, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill up year, skill combo boxes and fill parameter list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!CheckPreCondition())
            {
                moPerformanceParameterBL = new PerformanceParameterBL(miSchoolId, miUserId);
                if (!IsPostBack)
                {
                    SetJavaScriptAttribute();
                    FillYearCombobox();
                    FillSkillCombobox();
                    GetFormTypeDetails();
                    FillParameterListview();                    
                }
            }
            else
                SetJavaScriptAttribute();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save parameter details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();

            bool bIsConfigured = QueryString[Constants.S_IS_CONFIGURED] != Constants.S_YES;
            if (bIsConfigured)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.PerformanceParameter));

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
    /// This event is used to fill up parameter list view according to selected year.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
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
    /// This event is used to fill up parameter list view according to selected skill.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbSkill_SelectedIndexChanged(object sender, EventArgs e)
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
                    cmbFormType.SelectedValue = lstvwParameter.DataKeys[e.Item.DisplayIndex]["AppraisalFormTypeId"].ToString();
                    btnSave.Text = Constants.ButtonText.Update.ToString();
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moPerformanceParameterBL.Delete(iParameterId, Constants.SchoolConfigurations.PerformanceParameter.ToInt());
                    if (iParameterId == hidParameterId.Value.ToInt())
                        ClearFields();
                    DisplayMessage(Constants.ItemState.deleted, false);
                    FillParameterListview();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel current operation and clean fields.
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
    /// This event is used to set attributes on delete button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwParameter_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                PerformanceParameter oPerformanceParameter = e.Item.DataItem as PerformanceParameter;
                ImageButton btnIsSubmitted = e.Item.FindControl("btnIsSubmitted") as ImageButton;
                btnIsSubmitted.Visible = oPerformanceParameter.IsSubmitted;
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                ImageButton btnEdit = e.Item.FindControl("btnEdit") as ImageButton;
                if (btnIsSubmitted.Visible == true)
                {
                    btnDelete.Visible = false;
                    btnEdit.Visible = false;     
                }
                else
                {
                    btnDelete.Visible = true;
                    btnEdit.Visible = true;
                }
               
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle sorting.
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
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit parameters.
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
    /// This event is used to un submit parameters.
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

 protected void cmbFormType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbFormType.SelectedValue == Constants.S_ONE)
               FillParameterListview();
            else if (cmbFormType.SelectedValue == Constants.S_TWO)
                FillParameterListview();    
           
                
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)
    
    /// <summary>
    /// This method is used to submit parameters.
    /// </summary>
    /// <param name="abSubmit"></param>
    private void SubmitParameters(bool abSubmit)
    {
        int iYear = Convert.ToInt32(cmbYear.SelectedValue);
        int iSkillId = Convert.ToInt32(cmbSkill.SelectedValue);
        moPerformanceParameterBL.Submit(iYear, iSkillId, abSubmit);

        FillParameterListview();
    }

    /// <summary>
    /// This method is used to fill up year combo box.
    /// </summary>
    private void FillYearCombobox()
    {
        List<AcademicYear> lstYears = SchoolWiseAcademicYearMasterBL.GetAllYears(miSchoolId);
        ListSource.FillDropDownList(lstYears,cmbYear, "Year","Id",string.Empty);
        cmbYear.SelectedValue = miAcademicYearId.ToString();
    }

    /// <summary>
    /// This method is used to fill up skill combo box.
    /// </summary>
    private void FillSkillCombobox()
    {
        PerformanceSkillBL oPerformanceSkillBL = new PerformanceSkillBL(miSchoolId, miUserId);
        List<PerformanceSkill> lstSkills = oPerformanceSkillBL.GetAll();
        lstSkills = lstSkills.Where(skl => skl.SchoolId  == miSchoolId).ToList();
        ListSource.FillDropDownList(lstSkills, cmbSkill, "SkillName", "SkillId", string.Empty);
        
    }

    /// <summary>
    /// This method is used to fill up form type combo.
    /// </summary>
    private void GetFormTypeDetails()
    {
        PerformanceSkillBL oPerformanceSkillBL = new PerformanceSkillBL(miSchoolId, miUserId);
        List<FormType> lstFormTypeDetails = oPerformanceSkillBL.GetFormTypeDetails();
        ListSource.FillDropDownList(lstFormTypeDetails, cmbFormType, "Name", "Id", string.Empty);
        ListSource.FillDropDownList(lstFormTypeDetails, cmbFilterFormType, "Name", "Id", Constants.S_ALL);
    }

    ///// <summary>
    ///// This method is used to fill filter combo box Form Type.
    ///// </summary>
    //private void FillcmbFilterFormTypeCombo()
    //{
    //    PerformanceSkillBL oPerformanceSkillBL = new PerformanceSkillBL(miSchoolId, miUserId);
    //    List<FormType> lstFormTypeDetails = oPerformanceSkillBL.GetFormTypeDetails();
    //    ListSource.FillDropDownList(lstFormTypeDetails, cmbFilterFormType, "Name", "Id", string.Empty);
    //}

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttribute()
    {
        btnCancel.Attributes.Add("onclick", "ClearFields();");
        base.ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave, btnBack, btnSubmit, btnUnSubmit, btnBackUp });
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.StaffPerformanceRelated));
        btnBackUp.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.StaffPerformanceRelated));
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidParameterId.Value = Constants.S_ZERO;
        btnSave.Attributes.Add("onclick", "ClearMessage()");
        cmbYear.Focus();
    }

    /// <summary>
    /// This method is used to save parameter details.
    /// </summary>
    private void Save()
    {
        PerformanceParameter oPerformanceParameter = new PerformanceParameter
        {
            Id = hidParameterId.Value.ToInt(),
            Title = txtParameter.Text.Trim(),
            SortOrder = txtSortOrder.Text.ToInt(),
            Year = cmbYear.SelectedValue.ToInt(),
            SkillId = cmbSkill.SelectedValue.ToInt(),
            AppraisalFormTypeId = cmbFormType.SelectedValue.ToInt()
        };
        moPerformanceParameterBL.Save(oPerformanceParameter);
    }

    /// <summary>
    /// This method is used to fill parameter list view.
    /// </summary>
    private void FillParameterListview()
    {
        int iYear = Convert.ToInt32(cmbYear.SelectedValue);
        int iSkillId = Convert.ToInt32(cmbSkill.SelectedValue);
        List<PerformanceParameter> lstParameters = moPerformanceParameterBL.GetAll(iYear, iSkillId, cmbFormType.SelectedValue.ToInt());

        lstParameters = Sort(lstParameters);

        lstvwParameter.DataSource = lstParameters;
        lstvwParameter.DataBind();

        btnSubmit.Enabled = lstParameters.FindAll(prm => !prm.IsSubmitted).Any();
        btnUnSubmit.Enabled = lstParameters.FindAll(prm => prm.IsSubmitted).Any();               
    }

    /// <summary>
    /// This method is used to sort parameter details.
    /// </summary>
    /// <param name="alstParameters"></param>
    /// <returns></returns>
    private List<PerformanceParameter> Sort(List<PerformanceParameter> alstParameters)
    {
        if (hidSortExpression.Value == string.Empty || hidSortExpression.Value == "Title")
        {
            if (hidSortDirection.Value == Constants.S_ASCENDING)
                alstParameters = alstParameters.OrderBy(prm => prm.Title).ToList();
            else
                alstParameters = alstParameters.OrderByDescending(prm => prm.Title).ToList();
        }
        else if (hidSortExpression.Value == "SortOrder")
        {
            if (hidSortDirection.Value == Constants.S_ASCENDING)
                alstParameters = alstParameters.OrderBy(prm => prm.SortOrder).ToList();
            else
                alstParameters = alstParameters.OrderByDescending(prm => prm.SortOrder).ToList();
        }

        return alstParameters;
    }

    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        txtParameter.Text = string.Empty;
        txtSortOrder.Text = string.Empty;
        hidParameterId.Value = Constants.S_ZERO;
        btnSave.Text = Constants.ButtonText.Save.ToString();
    }

    /// <summary>
    /// This method is used to display message.
    /// </summary>
    /// <param name="aoItemState"></param>
    /// <param name="abIsErrorMessage"></param>
    private void DisplayMessage(Constants.ItemState aoItemState, bool abIsErrorMessage)
    {
        string sMessage = "Performance parameter " + aoItemState.ToString() + " successfully!!!";
        DisplayMessage(sMessage, abIsErrorMessage, tdMessage);
    }

    /// <summary>
    /// This method is used to display message.
    /// </summary>
    /// <param name="asButtonName"></param>
    /// <param name="abIsErrorMessage"></param>
    private void DisplayMessage(string asButtonName, bool abIsErrorMessage)
    {
        string sMessage = "Performance parameter " + asButtonName + " successfully!!!";
        DisplayMessage(sMessage, abIsErrorMessage, tdMessage);
    }

    /// <summary>
    /// This method is used to check pre-condition to configure association.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.PerformanceParameter);

        if (!sLinks.Equals(string.Empty))
        {
            trPreCondition.Visible = true;
            divErr.InnerHtml = sLinks;
            trControls.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.Visible = false;
            trControls.Visible = true;
            trPreCondition.Visible = false;
        }

        return bReturn;
    }

    #endregion
    protected void cmbFilterFormType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbFilterFormType.SelectedValue == Constants.S_ZERO)
                FillParameterListview();
            else if (cmbFilterFormType.SelectedValue == Constants.S_ONE)
                FillParameterListview();
            else
                FillParameterListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }
   
}