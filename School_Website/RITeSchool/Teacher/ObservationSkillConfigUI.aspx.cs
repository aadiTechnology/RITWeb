using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Teacher;
using Utility;

public partial class ObservationSkillConfigUI : SchoolBase
{
    #region Data Member(s)
    
    private ObservationSkillConfigBL moObservationSkillConfigBL; 

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

            base.AddSortImage(lstvwobSkillConfig, hidSortExpression.Value, hidSortDirection.Value);
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
            moObservationSkillConfigBL = new ObservationSkillConfigBL(miSchoolId, miUserId, miAcademicYearId);
            if (!IsPostBack)
            {
                SetJavaScriptAttribute();
                FillStandardCombo();
                fillSubjectCombo();
                FillListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            fillSubjectCombo();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwobSkillConfig_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;

            base.RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
            FillListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this method used to save Skill Details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();

            bool bIsConfigured = QueryString[Constants.S_IS_CONFIGURED] == Constants.S_YES;
            if (!bIsConfigured)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.ObservationSkill));
            DisplayMessage(hidId.Value == Constants.S_ZERO ? Constants.ItemState.saved : Constants.ItemState.updated, false);
            ClearFields();
            FillListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this method is used to Edit,delete Skill Details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwobSkillConfig_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iSkillId = lstvwobSkillConfig.DataKeys[e.Item.DisplayIndex]["Id"].ToInt();

                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    Label lblName = e.Item.FindControl("lblName") as Label;
                    Label lblSortOrder = e.Item.FindControl("lblSortOrder") as Label;

                    txtSkill.Text = HttpUtility.HtmlDecode(lblName.Text);
                    txtSortOrder.Text = HttpUtility.HtmlDecode(lblSortOrder.Text);
                    hidId.Value = iSkillId.ToString();
                    btnSave.Text = Constants.ButtonText.Update.ToString();
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moObservationSkillConfigBL.Delete(iSkillId, Constants.SchoolConfigurations.ObservationSkill.ToInt());
                    if (iSkillId == hidId.Value.ToInt())
                        ClearFields();
                    DisplayMessage(Constants.ItemState.deleted, false);
                    FillListView();
                }
            }
        }
        catch (SqlException ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwobSkillConfig_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {                
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                ImageButton btnEdit = e.Item.FindControl("btnEdit") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void btnBack_Click(object sender, System.EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.ObservationRelated)));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavaScriptAttribute()
    {
        btnCancel.Attributes.Add("onclick", "ClearFields();");
        base.ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave, btnSearch, btnBack });
        valSave.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidId.Value = Constants.S_ZERO;
        btnSave.Attributes.Add("onclick", "ClearMessage()");
    }

    /// <summary>
    /// This method is used to fill up Standard combo box.
    /// </summary>
    private void FillStandardCombo()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandard = oStandardCollectionBL.GetAssociatedStandards();
        ListSource.FillDropDownList(oDtStandard, cmbStandard, "standard_name", "standard_id", Constants.S_SELECT);
    }

    /// <summary>
    /// This mtehod is used to fill subject combo.
    /// </summary>
    private void fillSubjectCombo()
    {

        List<ObservationSkillConfig> lstSubjects = moObservationSkillConfigBL.GetAllSubjects(miSchoolId, miAcademicYearId, cmbStandard.SelectedValue.ToInt());
        lstSubjects = lstSubjects.OrderBy(sub => sub.SortOrder).ToList();
        ListSource.FillDropDownList(lstSubjects, cmbSubject, "SubjectName", "SubjectId", Constants.S_SELECT);
    }

    /// <summary>
    /// this method is used to fill ListView
    /// </summary>
    public void FillListView()
    {
        int iStandardId = cmbStandard.SelectedValue.ToInt();
        int iSubjectId = cmbSubject.SelectedValue.ToInt();
        List<ObservationSkillConfig> lstObservationSkillConfig = moObservationSkillConfigBL.GetAll(iStandardId, iSubjectId, txtSearch.Text.Trim());
        lstObservationSkillConfig = Sort(lstObservationSkillConfig);
        lstvwobSkillConfig.DataSource = lstObservationSkillConfig;
        lstvwobSkillConfig.DataBind();
    }

    /// <summary>
    /// This method is used to sort Observation Skill details.
    /// </summary>
    /// <param name="alstSkill"></param>
    /// <returns></returns>
    private List<ObservationSkillConfig> Sort(List<ObservationSkillConfig> alstSkill)
    {

        if (hidSortExpression.Value == string.Empty || hidSortExpression.Value == "Name")
        {
            if (hidSortDirection.Value == Constants.S_ASCENDING)
                alstSkill = alstSkill.OrderBy(prm => prm.Name).ToList();
            else
                alstSkill = alstSkill.OrderByDescending(prm => prm.Name).ToList();
        }
        if (hidSortExpression.Value == "SortOrder")
        {
            if (hidSortDirection.Value == Constants.S_ASCENDING)
                alstSkill = alstSkill.OrderBy(prm => prm.SortOrder).ToList();
            else
                alstSkill = alstSkill.OrderByDescending(prm => prm.SortOrder).ToList();
        }

        return alstSkill;
    }

    /// <summary>
    /// This method isused to save observation skills.
    /// </summary>
    private void Save()
    {
        ObservationSkillConfig oObservationSkillConfig = new ObservationSkillConfig
        {
            Id = hidId.Value.ToInt(),
            Skill = txtSkill.Text.Trim(),
            SortOrder = txtSortOrder.Text.ToInt(),
            StandardId = cmbStandard.SelectedValue.ToInt(),
            SubjectId = cmbSubject.SelectedValue.ToInt(),
        };
        moObservationSkillConfigBL.Save(oObservationSkillConfig);
    }

    /// <summary>
    /// This method is used to display message.
    /// </summary>
    /// <param name="aoItemState"></param>
    /// <param name="abIsErrorMessage"></param>
    private void DisplayMessage(Constants.ItemState aoItemState, bool abIsErrorMessage)
    {
        string sMessage = "Observation Skill " + aoItemState.ToString() + " successfully!!!";
        DisplayMessage(sMessage, abIsErrorMessage, tdMessage);
    }

    /// <summary>
    /// this method is used to Clear Fields
    /// </summary>
    private void ClearFields()
    { 
        txtSkill.Text = string.Empty;
        txtSortOrder.Text = string.Empty;
        btnSave.Text = Constants.ButtonText.Save.ToString();
    }
 
    #endregion
}