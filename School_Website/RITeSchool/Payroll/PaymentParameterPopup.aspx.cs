using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using BusinessLogic;
using Utility;
using PayrollEntities;
using System.Data.SqlClient;
/// <summary>
/// This class is used to add/edit/delete payment parameters.
/// </summary>
public partial class PaymentParameterPopup : SchoolBase
{
    #region Data Member(s)

    private PaymentParameterBL moPaymentParameterBL;

    #endregion    

    #region Event(s)

    /// <summary>
    /// This event is used to initialize all the parameters on page load.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moPaymentParameterBL = new PaymentParameterBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {
                SetCulture();
                SetJavascriptAttributes();
                RefreshValue();
                FillParameters();
            }

            SetCulture();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle the command events like Edit,Delete of parameter.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwParameters_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var iParameterId = e.CommandArgument;
                var oCurrentItem = e.Item as ListViewDataItem;                

                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moPaymentParameterBL.Delete(iParameterId.ToInt());
                    lblMessage.Text = hidParameterDeleted.Value;
                    ClearFields();
                    FillParameters();
                }
                else if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    hidParameterId.Value = iParameterId.ToString();                
                    Label lblPaymentParameter = oCurrentItem.FindControl("lblParameter") as Label;
                    txtParameter.Text = lblPaymentParameter.Text;
                    btnSave.Text = Resources.LocalizedResources.Update;
                }
            }
        }
        catch(SqlException ex)
        {
            if (ex.Message == Constants.S_ONE)
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = hidRICheck.Value;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save/update the payment parameter.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            moPaymentParameterBL.Save(hidParameterId.Value.ToInt(), txtParameter.Text.Trim());

            if (!hidParameterId.Value.IsNullOrEmpty() && hidParameterId.Value.ToInt() == 0)
                lblMessage.Text = hidParameterSaved.Value;
            else
                lblMessage.Text = hidParameterUpdated.Value;                

            ClearFields();
            FillParameters();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel the changes/operation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
            lblErrorMessage.Text = string.Empty;
            lblMessage.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to set the cultre for the page.
    /// </summary>
    private void SetCulture()
    {
        if (!IsPostBack)
        {
            if (Session[Constants.S_SESSION_LANGUAGE] != null)
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
        }

        if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
        {
            hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
            RefreshValue();
        }
    }

    /// <summary>
    /// This function is used to initialize controls to their default values.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnClose,btnCancel,btnSave });
        base.SetDefaultButton(btnSave);
        hidParameterId.Value = Constants.S_ZERO;
        btnClose.Attributes.Add("onclick", "CloseWindow()");
        btnSave.Text = Resources.LocalizedResources.Save;
    }

    /// <summary>
    /// This method is used to clear the field after operation.
    /// </summary>
    private void ClearFields()
    {
        hidParameterId.Value = Constants.S_ZERO;
        txtParameter.Text = string.Empty;
        btnSave.Text = Resources.LocalizedResources.Save;
    }

    /// <summary>
    /// This method is used to fill the payment parameters in listview.
    /// </summary>
    private void FillParameters()
    {
        List<PaymentParameter> lstPaymentParameters = moPaymentParameterBL.GetAll(0);
        lstPaymentParameters = lstPaymentParameters.OrderBy(param => param.Parameter).ToList();
        lstvwParameters.DataSource = lstPaymentParameters;
        lstvwParameters.DataBind();
    }

    /// <summary>
    /// This method is used to refresh the values of hidden field.
    /// </summary>
    private void RefreshValue()
    {
        hidAlert.Value = Resources.LocalizedResources.AlertDeleterecord;
        hidParameterEmpty.Value = Resources.LocalizedResources.ParameterEmpty;
        hidParameterDeleted.Value = Resources.LocalizedResources.ParameterDeleted;
        hidParameterUpdated.Value = Resources.LocalizedResources.ParameterUpdated;
        hidParameterSaved.Value = Resources.LocalizedResources.ParameterSaved;
        hidRICheck.Value = Resources.LocalizedResources.ParameterRICheck;
        hidAlreadyExists.Value = Resources.LocalizedResources.ParameterAlreadyExists;
        valSumError.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
    }

    #endregion
}