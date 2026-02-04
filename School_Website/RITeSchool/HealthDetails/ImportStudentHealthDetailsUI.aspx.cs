using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using SchoolEntities;
using System.Text;
using System.Data.SqlClient;

public partial class ImportStudentHealthDetailsUI : SchoolBase
{
    #region DataMember

    private HealthDetailsBL moHealthDetailsBL;

    #endregion

    #region Event's    

    /// <summary>
    /// This event is used to load all the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moHealthDetailsBL = new HealthDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                FillStandardCombobox();                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used to Change the standards.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ResetPager();
            FillDivisionCombobox();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used to bound the data to listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentList_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Label lblMonthlyIncome = e.Item.FindControl("lblMonthlyIncome") as Label;
                Label lblFatherAadhar = e.Item.FindControl("lblFatherAadhar") as Label;
                Label lblMotherAadhar = e.Item.FindControl("lblMotherAadhar") as Label;

                if (lblMonthlyIncome.Text == Constants.S_ZERO || lblMonthlyIncome.Text == "0.00")
                    lblMonthlyIncome.Text = string.Empty;

                if (lblFatherAadhar.Text == string.Empty)
                    lblFatherAadhar.Text = "-";

                if (lblMotherAadhar.Text == string.Empty)
                    lblMotherAadhar.Text = "-";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }    

    /// <summary>
    /// This Event is used to Data bound.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentList_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStudentList.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwStudentList, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display listview record according to value in page combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStudentList);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is Used to Import students health details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnImportStudent_Click(object sender, EventArgs e)
    {
        string sServerFilePath = string.Empty;
        string sFileName;
        try
        {
            sFileName = CommonUtility.GetFileNameForRenaming(FUStudentHealth.FileName);
            //string sFolderName = Server.MapPath("~") + "\\RITeSchool\\Uploads\\";
            string sFolderName = base.BasePath + "\\RITeSchool\\Uploads\\";
            sServerFilePath = sFolderName + sFileName;
            FUStudentHealth.SaveAs(sServerFilePath);

            string sErrorMessage = string.Empty;
            sErrorMessage = UploadFile(sServerFilePath);

            if (sErrorMessage.Equals(""))
            {
                lblHead.CssClass = "ClsHilightTextB";
                lblHead.Text = Resources.LocalizedResources.MsgFileUpload;
                lblHead.Visible = true;
                FillStudentDetails();
            }
            else
            {
                lblHead.Text = sErrorMessage;
                lblHead.Visible = true;
            }
        }
        catch (BusinessLogic.Exceptions.DuplicateRegisterNumberExceptions ex)
        {
            catchException(ex);
        }
        catch (SqlException ex)
        {
            lblHead.Text = ex.Message;
            lblHead.CssClass = "ClsLabel";
            lblHead.Visible = true;
            lblHead.ForeColor = System.Drawing.Color.Red;
        }
        catch (Exception ex)
        {
            lblHead.Text = ex.Message;
            lblHead.CssClass = "ClsLabel";
            lblHead.Visible = true;
            lblHead.ForeColor = System.Drawing.Color.Red;
        }       
        try
        {
            if (System.IO.File.Exists(sServerFilePath))
                System.IO.File.Delete(sServerFilePath);
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
            ResetPager();
            FillStudentDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to reset pager and fill student list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ResetPager();
            FillStudentDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method's

    /// <summary>
    /// This method fills combobox with standards.
    /// /// </summary>
    private void FillStandardCombobox()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandardsForHealth();
        ControlUtility.FillDropDownList(oDtStandardCollection, ref cmbStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT_ALL);

        cmbDivision.Items.Add(new ListItem { Text=Constants.S_SELECT_ALL, Value = Constants.S_ZERO});
    }

    /// <summary>
    /// This method fills combobox with Divisions
    /// </summary>
    /// <param name="aiStandardId"></param>
    private void FillDivisionCombobox()
    {
        if (cmbStandard.SelectedValue.ToInt() == 0)
        {
            cmbDivision.Items.Clear();
            cmbDivision.Items.Add(new ListItem { Text = Constants.S_SELECT_ALL, Value = Constants.S_ZERO });
        }
        else
        {
            DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
            DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(cmbStandard.SelectedValue.ToInt());
            ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbDivision,
                                           Constants.S_DIVISION_ID_FIELD,
                                           Constants.S_DIVISION_NAME_FIELD,
                                          Constants.S_SELECT_ALL);
        }
    }

    /// <summary>
    /// Fill Students List view.
    /// </summary>
    private void FillStudentDetails()
    {
        lstvwStudentList.DataSourceID = lstvwDSobj.ID;
        lstvwStudentList.DataBind();
    }

    /// <summary>
    /// This method is used to upload file.
    /// </summary>
    /// <param name="sServerFilePath"></param>
    /// <returns></returns>
    private string UploadFile(string sServerFilePath)
    {
        string sSourceFileName = FUStudentHealth.PostedFile.FileName;

        Constants.UploadFileType oUploadFileType = Constants.UploadFileType.StudentHealth;
        int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);

        FileUploadUtilityBL oFileUploadUtility = new FileUploadUtilityBL(sSourceFileName, sServerFilePath, oUploadFileType);
        oFileUploadUtility.UserId = miUserId;
        oFileUploadUtility.SchoolId = miSchoolId;
        oFileUploadUtility.StandardId = iStandardId;        
        oFileUploadUtility.AcademicYearId = miAcademicYearId;

        return oFileUploadUtility.UploadFile();
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        lnkDownloadTemplate.Attributes.Add("onclick", "window.open('../DOWNLOADS/StudentHealthDetails.xls','_self'); return false;");
        ApplyMouseHoverEffect(new List<Button> { btnImportStudent, imgbtnBack });
        valErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidValFileUpload.Value = Resources.LocalizedResources.ValFileUpload;
        hidValFileUploadType.Value = Resources.LocalizedResources.ValFileUploadType;
    }

    /// <summary>
    /// This method is used to set error message.
    /// </summary>
    /// <param name="ex"></param>
    private void catchException(Exception ex)
    {
        lblHead.Text = ex.Message;
        lblHead.CssClass = "ClsLabel";
        lblHead.Visible = true;
        lblHead.ForeColor = System.Drawing.Color.Red;
    }

    /// <summary>
    /// This method is used to reset pager.
    /// </summary>
    private void ResetPager()
    {
        DataPager dtPager = lstvwStudentList.FindControl("DtPgDropDown") as DataPager;
        if (dtPager != null)
            dtPager.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, true);
    }

    #endregion
}