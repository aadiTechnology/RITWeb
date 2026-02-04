using System;
using System.Web.UI.WebControls;
using Utility;

public partial class ClearanceListFiltersUI : System.Web.UI.UserControl
{
    #region "Public Members"

    public event EventHandler OnClearanceFiltersChanged;

    #endregion

    #region "Properties"

    public RadioButton RegNoRadioButton
    {
        get { return optRegNo; }
    }
    public RadioButton PaymentDateRadioButton
    {
        get { return optPaymentDate; }
    }
    public RadioButton ClearanceDateRadioButton
    {
        get { return optClearanceDate; }
    }
    public Label PaymentDateLable
    {
        get { return lblPaymenDate; }
    }

    public string StudentNameOrRegNo
    {
        get
        {
            return this.txtRegNo.Text;
        }
        set
        {
            this.txtRegNo.Text = value;
        }
    }
    public string PaymentStartDate
    {
        get
        {
            return this.txtPaymentStartDate.Text;
        }
        set
        {
            this.txtPaymentStartDate.Text = value;
        }
    }
    public string PaymentEndDate
    {
        get
        {
            return this.txtPaymentEndDate.Text;
        }
        set
        {
            this.txtPaymentEndDate.Text = value;
        }
    }
    public string ClearanceEndDate
    {
        get
        {
            return this.txtClearanceEndDate.Text;
        }
        set
        {
            this.txtClearanceEndDate.Text = value;
        }
    }
    public string ClearanceStartDate
    {
        get
        {
            return this.txtClearanceStartDate.Text;
        }
        set
        {
            this.txtClearanceStartDate.Text = value;
        }
    }

    public bool IncludeAll
    {
        get
        {
            return chkIncludeAll.Checked;
        }
        set
        {
            chkIncludeAll.Checked = value;
        }
    }
    public bool RegNoChecked
    {
        get
        {
            return optRegNo.Checked;
        }
        set
        {
            optRegNo.Checked = value;
        }
    }
    public bool PaymentDateChecked
    {
        get
        {
            return optPaymentDate.Checked;
        }
        set
        {
            optPaymentDate.Checked = value;
        }
    }
    public bool ClearanceDateChecked
    {
        get
        {
            return optClearanceDate.Checked;
        }
        set
        {
            optClearanceDate.Checked = value;
        }
    }



    #endregion

    #region Events

    /// <summary>
    /// This event is used to set filter of Registration number.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optRegNo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            optRegNoChecked();
            OnClearanceFiltersChanged(sender, e);
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used to set filter based on Payment Date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optPaymentDate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            optPaymentDateChecked();
            OnClearanceFiltersChanged(sender, e);
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    /// <summary>
    /// This event is used to set filter based on Clearance Date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optClearanceDate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            optClearanceDateChecked();
            OnClearanceFiltersChanged(sender, e);

        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog
            (ex.Message + Constants.S_TRACE + ex.StackTrace,
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + System.Reflection.MethodBase.GetCurrentMethod().Name,
            Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]));
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method is used to clear texts.
    /// </summary>
    private void ClearTextboxes()
    {
        txtRegNo.Text = string.Empty;
        txtPaymentStartDate.Text = string.Empty;
        txtPaymentEndDate.Text = string.Empty;
        txtClearanceStartDate.Text = string.Empty;
        txtClearanceEndDate.Text = string.Empty;
    }
    /// <summary>
    /// This method is used set controls when RegNo radio button checked.
    /// </summary>
    public void optRegNoChecked()
    {
        txtRegNo.Focus();
        ClearTextboxes();
        txtRegNo.Enabled = true;
        txtPaymentStartDate.Enabled = false;
        txtPaymentEndDate.Enabled = false;
        txtClearanceStartDate.Enabled = false;
        txtClearanceEndDate.Enabled = false;
        chkIncludeAll.Checked = false;
    }
    /// <summary>
    /// This method is used set controls when PaymentDate radio button checked.
    /// </summary>
    private void optPaymentDateChecked()
    {
        ClearTextboxes();
        txtRegNo.Enabled = false;
        txtPaymentStartDate.Enabled = true;
        txtPaymentEndDate.Enabled = true;
        txtClearanceStartDate.Enabled = false;
        txtClearanceEndDate.Enabled = false;
        chkIncludeAll.Checked = false;
    }
    /// <summary>
    /// This method is used set controls when ClearanceDate radio button checked.
    /// </summary>
    private void optClearanceDateChecked()
    {
        ClearTextboxes();
        txtRegNo.Enabled = false;
        txtPaymentStartDate.Enabled = false;
        txtPaymentEndDate.Enabled = false;
        txtClearanceStartDate.Enabled = true;
        txtClearanceEndDate.Enabled = true;
        chkIncludeAll.Checked = true;
    }
    /// <summary>
    /// This method exposes the functionality of disabling the textboxes uncheck the check box on this user control.
    /// </summary>
    public void DisableAll()
    {
        ClearTextboxes();
        txtRegNo.Enabled = false;
        txtPaymentStartDate.Enabled = false;
        txtPaymentEndDate.Enabled = false;
        txtClearanceStartDate.Enabled = false;
        txtClearanceEndDate.Enabled = false;
        chkIncludeAll.Checked = false;

    }
    /// <summary>
    /// This method exposes the functionality of enabling/disabling the radio buttons and the check box on this user control.
    /// </summary>
    public void EnableDisableControls(bool abflag)
    {

        optRegNo.Enabled = abflag;
        optPaymentDate.Enabled = abflag;
        optClearanceDate.Enabled = abflag;
        chkIncludeAll.Enabled = abflag;
    }

    /// <summary>
    /// This method exposes the functionality of enabling/disabling the textboxes based upon radio buttons checked.
    /// </summary>
    public void EnableDisableControlChecked(bool abFlag)
    {
        if (optRegNo.Checked)
            txtRegNo.Enabled = abFlag;
        else if (optPaymentDate.Checked)
        {
            txtPaymentStartDate.Enabled = abFlag;
            txtPaymentEndDate.Enabled = abFlag;
        }
        else if (optClearanceDate.Checked)
        {
            txtClearanceStartDate.Enabled = abFlag;
            txtClearanceEndDate.Enabled = abFlag;
        }
    }

    #endregion

}
