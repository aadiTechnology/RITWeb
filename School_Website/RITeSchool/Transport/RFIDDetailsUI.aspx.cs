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
using BusinessLogic;
using Utility;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Data.SqlClient;

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
                FillStandardDropDown();
                FillDivisionDropDown();
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

                UpdateRFIDToTransportDB();

                lblUpdate.Text = S_SAVE_MSG;
                ResetFields();
                FillStudentDetails(true);
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
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnShow.Text.ToUpper() == "SHOW")
            {
               SetFieldState(false);
               FillStudentDetails(false);

            }
            else
            {
                SetFieldState(true);
                FillStudentDetails(true);
                ResetFields();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
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
    /// <summary>
    /// These event is used to fill division dropdown on selection of Standard
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cmbDivision.Items.Clear();
            cmbDivision.Items.Add(new ListItem(Constants.S_SELECT, Constants.I_ZERO.ToString()));

            if (Convert.ToInt32(cmbStandard.SelectedValue) == Constants.I_ZERO)
            {
                FillStudentDetails(true);
            }
            else
            {
                FillDivisionDropDown();
                FillStudentDetails(true);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// These method is used to Import Details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnImport_Click(object sender, EventArgs e)
    {
        string sServerFilePath = string.Empty;
        string sFileName;
        try
        {
            sFileName = CommonUtility.GetFileNameForRenaming(fuRFIDImport.FileName);
            //string sFolderName = Server.MapPath("~") + "\\RITeSchool\\Uploads\\";
            string sFolderName = base.BasePath + "\\RITeSchool\\Uploads\\RFID Imports\\";
            sServerFilePath = sFolderName + sFileName;
            fuRFIDImport.SaveAs(sServerFilePath);

            string sErrorMessage = string.Empty;
            sErrorMessage = UploadFile(sServerFilePath);
        
            if (sErrorMessage.Equals(""))
            {
                UpdateRFIDToTransportDB();
                lblUpdate.CssClass = "ClsHilightTextB";
                lblUpdate.Text = Resources.LocalizedResources.MsgFileUpload;
                lblUpdate.Visible = true;                
            }
            else
            {
                lblUpdate.Text = sErrorMessage;
                lblUpdate.Visible = true;
                lblUpdate.ForeColor = System.Drawing.Color.Red;
            }
        }
        catch (BusinessLogic.Exceptions.DuplicateRegisterNumberExceptions ex)
        {
            catchException(ex);
        }
       
       catch (Exception ex)
        {
            lblUpdate.Text = ex.Message;
            lblUpdate.CssClass = "ClsLabel";
            lblUpdate.Visible = true;
            lblUpdate.ForeColor = System.Drawing.Color.Red;
        }
        
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method fills combobox with Divisions
    /// </summary>
    /// <param name="aiStandardId"></param>
    private void FillDivisionDropDown()
    {
        int aiStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(aiStandardId);
        ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbDivision,
                                       Constants.S_DIVISION_ID_FIELD,
                                       Constants.S_DIVISION_NAME_FIELD,
                                       Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to fill up Standard combo box.
    /// </summary>
    private void FillStandardDropDown()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandard = oStandardCollectionBL.GetAssociatedStandards();
        ListSource.FillDropDownList(oDtStandard, cmbStandard, "standard_name", "standard_id", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to fill listview.
    /// </summary>
    private void FillStudentDetails(bool abReset)
    {
        hidIsResetCall.Value = (abReset ? Constants.S_ONE : Constants.S_ZERO);        

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
        lnkDownloadTemplate.Attributes.Add("onclick", "window.open('../DOWNLOADS/StudentRFIDDetails.xlsx','_self'); return false;");

        hidValFileUpload.Value = Resources.LocalizedResources.ValFileUpload;
        hidValFileUploadType.Value = Resources.LocalizedResources.ValFileUploadType;
    }

    private void SetFieldState(bool abEnable)
    {
        cmbStandard.Enabled = abEnable;
        cmbDivision.Enabled = abEnable;

        btnShow.Text = abEnable ? "SHOW" : "CHANGE INPUT";

        if (!abEnable)
        {
            txtSearch.Enabled = false;
        }
        else
        {
            txtSearch.Enabled = true;
        }
    }

    /// <summary>
    /// This method is used to set error message.
    /// </summary>
    /// <param name="ex"></param>
    private void catchException(Exception ex)
    {
        lblUpdate.Text = ex.Message;
        lblUpdate.CssClass = "ClsLabel";
        lblUpdate.Visible = true;
        lblUpdate.ForeColor = System.Drawing.Color.Red;
    }


    /// <summary>
    /// This method is used to upload file.
    /// </summary>
    /// <param name="sServerFilePath"></param>
    /// <returns></returns>
    private string UploadFile(string sServerFilePath)
    {
        string sSourceFileName = fuRFIDImport.PostedFile.FileName;

        Constants.UploadFileType oUploadFileType = Constants.UploadFileType.RFID;
        int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);

        FileUploadUtilityBL oFileUploadUtility = new FileUploadUtilityBL(sSourceFileName, sServerFilePath, oUploadFileType);
        oFileUploadUtility.UserId = miUserId;
        oFileUploadUtility.SchoolId = miSchoolId;
        oFileUploadUtility.StandardId = iStandardId;
        oFileUploadUtility.AcademicYearId = miAcademicYearId;

        return oFileUploadUtility.UploadFile();
    }

    /// <summary>
    /// This method is used to update transport database for RFID details.
    /// </summary>
    private void UpdateRFIDToTransportDB()
    {
        if (ConfigurationManager.AppSettings["TransportExternalDBName"] != null && ConfigurationManager.AppSettings["TransportExternalDBName"].ToString() != string.Empty)
        {
            string sDBName = ConfigurationManager.AppSettings["reportdatabasename"].ToString();
            string sTransportDBName = ConfigurationManager.AppSettings["TransportExternalDBName"].ToString();
            TransferTransportDetailsBL oTransferTransportDetailsBL = new TransferTransportDetailsBL(miSchoolId, sDBName, sTransportDBName);
            oTransferTransportDetailsBL.UpdateRFIDDetails(hidUserId.Value.ToInt());
        }
    }

    #endregion
}