/*File Name - UpdateRFIDUI.aspx.cs
 * Created Date - 08-Jul-2024
 * Created By - Rutuja
 * Description - This class is used to update RFID of student.
 */
using System;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using BusinessLogic.TransportBL;
using Utility;
using System.Configuration;

public partial class RFIDDetailsUI : SchoolBase
{
    #region Constants

    private const string S_COMMAND_SELECT = "SelectDetails";
    private const string S_SAVE_MSG = "RFID updated successfully !!!";

    #endregion

    #region Datamembers

    private RFIDDetailsBL moRFIDDetailsBL;

    #endregion

    #region Events
    /// <summary>
    /// This event is used to display details at page load.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moRFIDDetailsBL = new RFIDDetailsBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {    
                SetDefaultButton(btnSave);
                SetDefaultValues();
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
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwUpdateRFIDDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle update action.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUpdateRFIDDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                int iSchoolwiseStudentId = Convert.ToInt32(lstvwUpdateRFIDDetails.DataKeys[iRowId]["SchoolWiseStudentId"]);
                hidSchoolwiseStudentId.Value = iSchoolwiseStudentId.ToString();
                hidUserId.Value = lstvwUpdateRFIDDetails.DataKeys[iRowId]["UserId"].ToString();

                if (e.CommandName == S_COMMAND_SELECT)
                {
                    Label lblName = oCurrentItem.FindControl("lblName1") as Label;
                    lblStudentNameData.Text = lblName.Text;

                    Label lblRFID = oCurrentItem.FindControl("lblRFID") as Label;
                    txtRFID.Text = lblRFID.Text;

                    txtRFID.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set paging details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUpdateRFIDDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwUpdateRFIDDetails.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwUpdateRFIDDetails, DtPgCount);
            }
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save RFID.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                moRFIDDetailsBL.Save(hidSchoolwiseStudentId.Value.ToInt(), txtRFID.Text);

                if (ConfigurationManager.AppSettings["TransportExternalDBName"] != null && ConfigurationManager.AppSettings["TransportExternalDBName"].ToString() != string.Empty)
                {
                    string sDBName = ConfigurationManager.AppSettings["reportdatabasename"].ToString();
                    string sTransportDBName = ConfigurationManager.AppSettings["TransportExternalDBName"].ToString();
                    TransferTransportDetailsBL oTransferTransportDetailsBL = new TransferTransportDetailsBL(miSchoolId, sDBName, sTransportDBName);
                    oTransferTransportDetailsBL.UpdateRFIDDetails(hidUserId.Value.ToInt());
                }

                lblUpdate.Text = S_SAVE_MSG;
                ResetFields();
                FillStudentDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ResetFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search record.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillStudentDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This evet is used to validate RFID duplication.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DuplicateRFID_Validate(object sender, ServerValidateEventArgs e)
    {
        string sMessage = moRFIDDetailsBL.ValidateRFID(hidSchoolwiseStudentId.Value.ToInt(), txtRFID.Text.Trim());
        if (sMessage != string.Empty)
        {
            ((CustomValidator)sender).ErrorMessage = sMessage;
            e.IsValid = false;
        }
        else
            e.IsValid = true;
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to fill listview.
    /// </summary>
    private void FillStudentDetails()
    {
        lstvwUpdateRFIDDetails.DataSourceID = objdsRFIDDetails.ID;
        lstvwUpdateRFIDDetails.DataBind();
    }

    /// <summary>
    /// This method is used to reset fields.
    /// </summary>
    private void ResetFields()
    {
        txtRFID.Text = string.Empty;        
        lblStudentNameData.Text = string.Empty;
        hidSchoolwiseStudentId.Value = Constants.S_ZERO;
        hidUserId.Value = Constants.S_ZERO;
    }

    private void SetDefaultValues()
    {
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }
    #endregion
}