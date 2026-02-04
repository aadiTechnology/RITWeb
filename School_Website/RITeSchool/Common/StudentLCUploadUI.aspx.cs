// Class Name       :- StudentLCUploadUI
// Purpose          :- This class is used to manage StudentLCUploadUI details.
// Date Of creation :- 28/3/2019
// Author Name      :- Sachin Wagh

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using SchoolEntities;
using System.Data;
using System.Reflection;
using LCUploadEntities;
using System.Collections;
using System.Text;
using System.IO;

/// <summary>
/// This class is used to upload LC
/// </summary>
public partial class StudentLCUploadUI : SchoolBase
{
    #region Constants

    private const string S_DELETE_MESSAGE = "Student LC attachment(s) Removed successfully!!!";
    private const string S_SAVE_MESSAGE = "Student LC attachment(s) updated successfully!!!";
    private const string S_FOLDER_PATH = @"../DOWNLOADS/LCSample/";

    const int I_FILE_SIZE_LIMIT = 256000;//nearly 250 kb

    List<string> mlstStudentLCDetails;

    #endregion

    #region DataMember

    StudentLCUploadBL moStudentLCUploadBL;    

    #endregion

    #region Event's

    /// <summary>
    /// This method is used to set default page controls and java script attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStudentLCUploadBL = new StudentLCUploadBL(miSchoolId,miAcademicYearId,miUserId);
            if (!IsPostBack)
            {
                SetVisibility(false);
                SetJavaScriptAttributes();                
                FillStandardCombo();                
                FillDivisionCombobox();                
                FillStudentListview();
                cmbStandard.Focus();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used for Search Student deatails
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {   
            DtPgCount.SetPageProperties(0, Constants.I_GRID_PAGE_COUNT, false);
            FillStudentListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to bind data to listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentLCDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton oImg = (ImageButton)e.Item.FindControl("ibtnIsStudentLC");
                HiddenField hidPhotoUploadStatus = (HiddenField)e.Item.FindControl("hidPhotoUploadStatus");
                int iFileUploadStatus = Convert.ToInt32(lstvwStudentLCDetails.DataKeys[e.Item.DisplayIndex]["LCUploadStatus"]);

                StudentLCDetails oStudentLCDetails = e.Item.DataItem as StudentLCDetails;
                CheckBox ochkRemoveLc = (CheckBox)e.Item.FindControl("chkRemoveLC");
                HiddenField hidlc = (HiddenField)e.Item.FindControl("hidlc");
                oImg.ImageAlign = ImageAlign.Middle;
                if (oStudentLCDetails.LCFilePath != string.Empty) 
                {
                    oImg.Visible = true;
                    oImg.ImageUrl = "../images/iconGridSml_ViewGE.gif";
                    hidlc.Value =  S_FOLDER_PATH + oStudentLCDetails.LCFilePath;
                    oImg.Attributes.Add("Onclick", "openfile("+e.Item.DisplayIndex+")");
                    ochkRemoveLc.Visible = true;
                }
                else
                {
                    oImg.Visible = false;
                }

                if (iFileUploadStatus == Constants.I_ONE)
                    hidPhotoUploadStatus.Value = Constants.S_YES;
                else
                    hidPhotoUploadStatus.Value = Constants.S_NO;
            }
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
    protected void lstvwStudentLCDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStudentLCDetails.Items.Count > 0)
            {
                SetConfirmationMessage();
                SetVisibility(true);
                ControlUtility.FillListViewPagerFooter(lstvwStudentLCDetails, DtPgCount);
                               
                DataPager oDataPager = lstvwStudentLCDetails.FindControl("DtPgDropDown") as DataPager;
                int iCurrentPage = (oDataPager.StartRowIndex / oDataPager.PageSize) + 1;
                hidPageNo.Value = iCurrentPage.ToString();
            }
            else
            {
                SetVisibility(false);
                DtPgCount.Visible = false;                
            }
            hidCount.Value = lstvwStudentLCDetails.Items.Count.ToString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to set page count.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStudentLCDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to bind divisions for selected class 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {   
            FillDivisionCombobox();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill list view
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DtPgCount.SetPageProperties(0, Constants.I_GRID_PAGE_COUNT, false);
            FillStudentListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used is Upload LC 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            mlstStudentLCDetails = new List<string>();
            List<StudentLCDetails> lstStudentLCDetails = Populate();
            if (lstStudentLCDetails.Count > Constants.I_ZERO)
            {
                moStudentLCUploadBL.UploadStudentLC(base.GenerateXml(lstStudentLCDetails));

                lblUpdateSucess.Visible = true;
                lblUpdateSucess.Text = S_SAVE_MESSAGE;
                FillStudentListview();
            }            
        }
        catch (ApplicationException ex)
        {
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to Remove LC 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRemoveLC_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder obj = new StringBuilder();
            foreach (ListViewDataItem oListViewDataItem in lstvwStudentLCDetails.Items)
            {
                CheckBox oRemoveLC = oListViewDataItem.FindControl("chkRemoveLC") as CheckBox;
                if (oRemoveLC.Checked)
                    obj.Append("," + lstvwStudentLCDetails.DataKeys[oListViewDataItem.DisplayIndex]["StudentId"].ToString());
            }

            string sIds = string.Empty;
            if (obj.Length > 0)
                sIds = obj.ToString().Substring(1);

            if (sIds != string.Empty)
            {
                moStudentLCUploadBL.DeleteLCFiles(sIds);

                lblUpdateSucess.Visible = true;
                lblUpdateSucess.Text = S_DELETE_MESSAGE;
                FillStudentListview();
            }
        }
        catch (ApplicationException ex)
        {
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to check box change event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkUserWithLC_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            DtPgCount.SetPageProperties(0, Constants.I_GRID_PAGE_COUNT, false);
            FillStudentListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region private Methods

    /// <summary>
    /// This method is used to set confirmation messaege on change of page.
    /// </summary>
    private void SetConfirmationMessage()
    {
        DataPager oDataPager = lstvwStudentLCDetails.FindControl("DtPgDropDown") as DataPager;
        DropDownList ddlCount = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
        ddlCount.Attributes.Add("onchange", "if(!MessageAboutUpload('" + ddlCount.ClientID + "')){return false;}");
    }

    /// <summary>
    /// This method is used to fill standard's combo.
    /// </summary>
    private void FillStandardCombo()
    {   
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbStandard,
                                       Constants.S_STANDARD_ID_FIELD, Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT);
        //Add item into division combobox.        
        cmbDivision.Items.Add(new ListItem(Constants.S_SELECT_ALL, Constants.I_ZERO.ToString()));
    }

    /// <summary>
    /// This method is used to fill division's combo.    
    /// </summary>    
    private void FillDivisionCombobox()
    {
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(cmbStandard.SelectedValue.ToInt());
        ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbDivision,
                                       Constants.S_DIVISION_ID_FIELD, Constants.S_DIVISION_NAME_FIELD,
                                       Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill student listview.
    /// </summary>
    private void FillStudentListview()
    {  
        lstvwStudentLCDetails.DataSourceID = lstvwDsObj.ID;
        lstvwStudentLCDetails.DataBind();
    }

    /// <summary>
    /// This method is used to set visibility according to action.
    /// </summary>
    /// <param name="abAction"></param>
    private void SetVisibility(bool abAction)
    {   
        btnUpload.Visible = abAction;
        btnRemoveLC.Enabled = abAction;
    }
     
    /// <summary>
    /// This method is used to upload the file to the server.
    /// DeleteFiles();
    /// </summary>
    private string SaveFileOnServer(FileUpload FileUploadPhoto, int iRowId)
    {   
        string asFileName = FileUploadPhoto.FileName;
        string sFolderName = Server.MapPath("..") + S_FOLDER_PATH;
        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;
        if (!File.Exists(sServerFilePath))
        {
            mlstStudentLCDetails.Add(sServerFilePath);            
            string sErrorMsg = ValidateFile(FileUploadPhoto, iRowId);
            if (sErrorMsg.Equals(string.Empty))
            {
                sFileName = CommonUtility.GetFileNameForRenaming(asFileName);
                sServerFilePath = sFolderName + sFileName;
            }
            else
            {
                for (int iCount = 0; iCount < mlstStudentLCDetails.Count; iCount++)
                    File.Delete(mlstStudentLCDetails[iCount].ToString());

                throw new ApplicationException(sErrorMsg);
            }
            FileUploadPhoto.SaveAs(sServerFilePath);
        }        
        return sFileName;
    }

    /// <summary>
    ///This method is used to validate size, height and width of uploaded files.
    /// </summary>
    private string ValidateFile(FileUpload FileUploadPhoto, int iRowId)
    {
        string sReturnErrorMsg = String.Empty;        

        int iFileSize = FileUploadPhoto.PostedFile.ContentLength;

        if (iFileSize > I_FILE_SIZE_LIMIT)
        {
            sReturnErrorMsg = "Size of file is too large at row number " + (iRowId + 1).ToString() + ".";            
        }
        return sReturnErrorMsg;
    }

    /// <summary>
    /// This method is used to get collection of user photo details to upload.
    /// </summary>
    /// <returns></returns>
    private List<StudentLCDetails> Populate()
    {   
        List<StudentLCDetails> oStudentLCUploadDetails = new List<StudentLCDetails>();
        foreach (ListViewDataItem oListViewDataItem in lstvwStudentLCDetails.Items)
        {
            FileUpload oFileUpload = oListViewDataItem.FindControl("FileUploadLC") as FileUpload;            

            if (oFileUpload.HasFile)
            {
                StudentLCDetails oStudentLCDetails = new StudentLCDetails();
                oStudentLCDetails.StudentId = Convert.ToInt32(lstvwStudentLCDetails.DataKeys[oListViewDataItem.DisplayIndex]["StudentId"]);
                oStudentLCDetails.LCFilePath = SaveFileOnServer(oFileUpload, oListViewDataItem.DisplayIndex);                
                oStudentLCUploadDetails.Add(oStudentLCDetails);
            }
        }
        return oStudentLCUploadDetails;
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSearch, btnUpload, btnRemoveLC });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnRemoveLC.Attributes.Add("onclick", "if(!CheckCheckBoxisChecked()) {return false}");
        btnUpload.Attributes.Add("onclick", "if(!CheckFileIsUploaded()) {return false}");
        hidFirstFxFollowingErrors.Value = Resources.LocalizedResources.PleaseFixFollowingError;
    }
   
    #endregion    
}