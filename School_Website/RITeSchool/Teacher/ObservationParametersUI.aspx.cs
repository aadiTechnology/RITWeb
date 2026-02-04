using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Teacher;
using Utility;

public partial class ObservationParametersUI : SchoolBase
{
    #region Constant(s)

    private const string S_SUBMIT = "Submit";
    private const string S_UN_SUBMIT = "Un Submit";

    #endregion

    #region Data Member(s)

    private ObservationParametersBL moObservationParametersBL;

    #endregion

    #region Event(s)

    protected void Page_Load(object sender, EventArgs e)
    {
        moObservationParametersBL = new ObservationParametersBL(miSchoolId, miUserId, miAcademicYearId);
        try
        {
            if (!IsPostBack)
            {
                FillStandardCombo();
                SetJavaScriptAttribute();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillSkillDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();

            lblMessage.Text = "Observation Parameter saved successfully !!!";
            lblMessage.ForeColor = System.Drawing.Color.Blue;

            ClearFields();
            FillObParameterListview();

            bool bIsConfigured = QueryString[Constants.S_IS_CONFIGURED] == Constants.S_YES;
            if (!bIsConfigured)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.ObservationParameters));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            moObservationParametersBL.Submit(cmbStandard.SelectedValue.ToInt(), cmbSkill.SelectedValue.ToInt(), true);
            lblMessage.Text = "Observation Parameter submitted successfully !!!";
            lblMessage.ForeColor = System.Drawing.Color.Blue;
            FillObParameterListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
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
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnUnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            moObservationParametersBL.Submit(cmbStandard.SelectedValue.ToInt(), cmbSkill.SelectedValue.ToInt(), false);
            lblMessage.Text = "Observation Parameter un-submitted successfully !!!";
            lblMessage.ForeColor = System.Drawing.Color.Blue;
            FillObParameterListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwobParameter_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iParamterId = lstvwobParameter.DataKeys[e.Item.DisplayIndex]["Id"].ToInt();
                if (e.CommandName == "UpdateCommand")
                {
                    ObservationParameters oObservationParameters = moObservationParametersBL.Get(cmbSkill.SelectedValue.ToInt(), iParamterId);
                    if (oObservationParameters.Id != 0)
                    {
                        hidParameterId.Value = oObservationParameters.Id.ToString();
                        txtParameter.Text = oObservationParameters.Parameter;
                        txtSortOrder.Text = oObservationParameters.SortOrder.ToString();
                    }

                    btnSave.Text = Constants.ButtonText.Update.ToString();
                }
                else if (e.CommandName == "RemoveCommand")
                {
                    moObservationParametersBL.Delete(iParamterId);
                    lblMessage.Text = "Observation Parameter deleted successfully !!!";
                    lblMessage.ForeColor = System.Drawing.Color.Blue;
                    ClearFields();
                    FillObParameterListview();
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

    protected void lstvwobParameter_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            ObservationParameters oObservationParameters = e.Item.DataItem as ObservationParameters;

            ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
            btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

            ImageButton btnEdit = e.Item.FindControl("btnEdit") as ImageButton;

            ImageButton btnIsSubmitted = e.Item.FindControl("btnIsSubmitted") as ImageButton;
            btnIsSubmitted.Visible = oObservationParameters.IsSubmitted;

            if (oObservationParameters.IsSubmitted)
            {
                btnIsSubmitted.Visible = true;
                btnEdit.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                btnIsSubmitted.Visible = false;
                btnEdit.Visible = true;
                btnDelete.Visible = true;
            }

        }
    }

    protected void lstvwobParameter_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;

            base.RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
            FillObParameterListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    protected void cmbSkill_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            FillObParameterListview();
            if (cmbSkill.SelectedValue != Constants.S_ZERO)
                btnSave.Enabled = true;
            else
                btnSave.Enabled = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    private void ClearFields()
    {
        txtParameter.Text = string.Empty;
        txtSortOrder.Text = string.Empty;
        hidParameterId.Value = Constants.S_ZERO;
        btnSave.Text = Constants.ButtonText.Save.ToString();
    }

    public bool abIsErrorMessage { get; set; }

    private void FillStandardCombo()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandard = oStandardCollectionBL.GetAssociatedStandards();
        ListSource.FillDropDownList(oDtStandard, cmbStandard, "standard_name", "standard_id", Constants.S_SELECT);

        cmbSkill.Items.Clear();
        cmbSkill.Items.Add(new ListItem { Value = Constants.S_ZERO, Text = Constants.S_SELECT });
    }

    private void FillSkillDetails()
    {
        List<ObservationParameters> lstSkills = moObservationParametersBL.GetSkills(miSchoolId, cmbStandard.SelectedValue.ToInt(), miAcademicYearId);
        ListSource.FillDropDownList(lstSkills, cmbSkill, "SkillName", "SkillId", Constants.S_SELECT);

        lstvwobParameter.DataSource = null;
        lstvwobParameter.DataBind();
    }

    //private List<ObservationParameters> Sort(List<ObservationParameters> alstParameters)
    //{
    //    if (hidSortExpression.Value == string.Empty || hidSortExpression.Value == "Parameter")
    //    {
    //        if (hidSortDirection.Value == Constants.S_ASCENDING)
    //            alstParameters = alstParameters.OrderBy(prm => prm.Parameter).ToList();
    //        else
    //            alstParameters = alstParameters.OrderByDescending(prm => prm.Parameter).ToList();
    //    }
    //    else if (hidSortExpression.Value == "SortOrder")
    //    {
    //        if (hidSortDirection.Value == Constants.S_ASCENDING)
    //            alstParameters = alstParameters.OrderBy(prm => prm.SortOrder).ToList();
    //        else
    //            alstParameters = alstParameters.OrderByDescending(prm => prm.SortOrder).ToList();
    //    }

    //    return alstParameters;
    //}

    private void SetJavaScriptAttribute()
    {
        btnCancel.Attributes.Add("onclick", "ClearFields();");
        base.ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave, btnBack, btnSubmit, btnUnSubmit });
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.ObservationRelated));

        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidParameterId.Value = Constants.S_ZERO;
        btnSave.Attributes.Add("onclick", "ClearMessage()");
        cmbStandard.Focus();
    }

    private void FillObParameterListview()
    {
        int iSkillId = Convert.ToInt32(cmbSkill.SelectedValue);
        List<ObservationParameters> lstParameters = moObservationParametersBL.GetAll(iSkillId);

        //lstParameters = Sort(lstParameters);

        lstvwobParameter.DataSource = lstParameters;
        lstvwobParameter.DataBind();

        btnSubmit.Enabled = lstParameters.FindAll(prm => !prm.IsSubmitted).Any();
        btnUnSubmit.Enabled = lstParameters.FindAll(prm => prm.IsSubmitted).Any();
    }

    private void Save()
    {
        ObservationParameters oObservationParameters = new ObservationParameters
        {
            Id = hidParameterId.Value.ToInt(),
            Parameter = txtParameter.Text.Trim(),
            SortOrder = txtSortOrder.Text.ToInt(),
            StandardId = cmbStandard.SelectedValue.ToInt(),
            SkillId = cmbSkill.SelectedValue.ToInt(),

        };
        moObservationParametersBL.Save(oObservationParameters);
    } 

    #endregion
}
