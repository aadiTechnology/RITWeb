using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using BusinessLogic;
using Utility;
using System.Data;
using PhotoUploadEntities;
using System.IO;
using System.Collections;
using SchoolEntities.Admin;
using PayrollEntities;
using System.Web.UI.HtmlControls;

public partial class UploadUserDocumentsUI : SchoolBase
{
    #region Constant(s)

    const int I_PAGE_SIZE = 10;
    private const string S_DELETE_MESSAGE = "Document details deleted successfully !!!";    
    private const string S_SAVE_MESSAGE = "Document details saved successfully !!!";
    protected int miUserRoleID;
    #endregion

    #region Data Members

    ArrayList oArrlstSave = null;    

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
                hidSortExpression.Value = "PaymentDate";
                hidSortDirection.Value = Constants.S_DESCENDING;
            }

            AddSortImage(lstvwUserDocuments, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwPayments_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if (hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;
            base.RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This event is used to fill user list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UploadUserDocumentBL oUploadUserDocumentBL = new UploadUserDocumentBL();
            if (!IsPostBack)
            {
                FillDocumentType();//
              
                SetVisibility(true);
                FillUserRoleCombo();
                FillClasses();
                SetFieldValuesLeftStudent();
                FillUserListView();
                SetJavaScriptAttributes();
                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    private void SetFieldValues()
    {
        UploadUserDocumentBL oUploadUserDocumentBL = new UploadUserDocumentBL(miSchoolId, miAcademicYearId);
        DataTable dt = oUploadUserDocumentBL.GetUserWisePanNo(miSchoolId, miAcademicYearId, cmbUser.SelectedValue.ToString());
        if (dt.Rows.Count > 0)
        {
            lblpanNo.Text = dt.Rows[0]["PanNo"].ToString();
            lblEmpNo.Text = dt.Rows[0]["EmployeeNo"].ToString();
        }
    }
    /// <summary>
    /// This event is used to change user role combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbUserRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
         
            FillUsers(); // 

            if (optDocumentWise.Checked)
            {
                if (cmbUserRole.SelectedValue == Constants.UserRoles.Student.ToInt().ToString())
                {
                    trleftStudent.Visible = true;
                    trnewclass.Visible = true;
                 }
                else
                {
                    chkLeftStudent.Checked = false;
                    trleftStudent.Visible = false;
                    trnewclass.Visible = false;
                }
            }
            if (optDocumentWise.Checked)
                FillUserListView();

            else
            {
                lstvwUserDocuments.DataSourceID = null;
                lstvwUserDocuments.DataBind();

                if (cmbUserRole.SelectedValue == Constants.UserRoles.Student.ToInt().ToString())
                {
                    cmbClass.Enabled = true;
                    trnewclass.Visible = false;
                }
                else
                {
                    cmbClass.ClearSelection();
                    cmbClass.Enabled = false;
                    trnewclass.Visible = false;
                }

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void chkLeftStudent_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillUserListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change user  combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetFieldValues();
            FillUserListView();


            if (optUserWise.Checked)
            {
                HtmlTableRow tr = lstvwUserDocuments.FindControl("trHeader") as HtmlTableRow;
                HtmlTableCell thPanNo = tr.FindControl("thpanno") as HtmlTableCell;
                if (thPanNo != null)
                    thPanNo.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search the user by his name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillUserListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This button is used to save the document details in table.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
           
                oArrlstSave = new ArrayList();
                UploadUserDocumentBL oUploadUserDocumentBL = new UploadUserDocumentBL(miSchoolId, miAcademicYearId);
                List<UserRolewiseDocumentDetails> lstUserRolewiseDocumentDetails = Populate();
                string sUserDocument = base.GenerateXml(lstUserRolewiseDocumentDetails);
                oUploadUserDocumentBL.Save(cmbDocumentType.SelectedValue.ToInt(), cmbUserRole.SelectedValue.ToInt(), sUserDocument, miUserId);
                FillUserListView();

                DataPager oDataPager = lstvwUserDocuments.FindControl("DtPgDropDown") as DataPager;
                if (oDataPager.Visible)
                {
                    DropDownList ddlCount = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
                    DtPgCount.SetPageProperties((Convert.ToInt32(ddlCount.SelectedIndex) * I_PAGE_SIZE), I_PAGE_SIZE, false);
                }
                base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
               
           
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set details in list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUserDocuments_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                ImageButton imgView = (ImageButton)oCurrentItem.FindControl("imgView");
                ImageButton btnDelete = (ImageButton)oCurrentItem.FindControl("btnDelete");
                string sFilePath = lstvwUserDocuments.DataKeys[oCurrentItem.DisplayIndex]["DocumentFilePath"].ToString();
                HiddenField hidDocFile = (HiddenField)oCurrentItem.FindControl("hidDocFile");
                if (sFilePath.TrimAll() != string.Empty)
                {
                    hidDocFile.Value = sFilePath;
                    if (optDocumentWise.Checked)
                    {
                        if (cmbDocumentType.SelectedItem.Text == "Leaving Certificate")
                            sFilePath = "Leaving Certificate/" + sFilePath;
                        else
                            sFilePath = "FormNo16/" + sFilePath;
                    }
                    else
                    {
                        Label Label1 = e.Item.FindControl("Label1") as Label;
                        string sFLName = HttpUtility.HtmlDecode(Label1.Text);
                        if (sFLName == "Leaving Certificate")
                            sFilePath = "Leaving Certificate/" + sFilePath;
                        else
                            sFilePath = "FormNo16/" + sFilePath;
                    }
                                        
                    imgView.Visible = true;
                    imgView.Attributes.Add("Onclick", "OpenDocument('" + sFilePath + "');return false;");
                    btnDelete.Visible = true;
                    btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
                }
                else
                {
                    imgView.Visible = false;
                    btnDelete.Visible = false;
                }

                HtmlTableCell tdSelect = e.Item.FindControl("tdSelect") as HtmlTableCell;
                HtmlTableCell tdEmpNo = e.Item.FindControl("tdEmpNo") as HtmlTableCell;

                if (cmbUserRole.SelectedValue == "3" || optUserWise.Checked)
                {
                    if (tdSelect != null)
                        tdSelect.Visible = false;

                    if (tdEmpNo != null)
                        tdEmpNo.Visible = false;
                }
                else if (optDocumentWise.Checked)
                {
                    if (tdSelect != null)
                        tdSelect.Visible = true;

                    if (tdEmpNo != null)
                        tdEmpNo.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
   

    /// <summary>
    /// This event is used to list view command event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUserDocuments_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {

                int iDocumentId = Convert.ToInt32(lstvwUserDocuments.DataKeys[e.Item.DisplayIndex]["DocumentId"]);
                int iUserId = 0;
                if (optDocumentWise.Checked == true)
                    iUserId = Convert.ToInt32(lstvwUserDocuments.DataKeys[e.Item.DisplayIndex]["UserId"]);
                else
                    iUserId = cmbUser.SelectedValue.ToInt();  //
                int iDocumentTypeId = Convert.ToInt32(lstvwUserDocuments.DataKeys[e.Item.DisplayIndex]["DocumentTypeId"]);
                UploadUserDocumentBL oUploadUserDocumentBL = new UploadUserDocumentBL(miSchoolId, miAcademicYearId);
              
                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    oUploadUserDocumentBL.Delete(iDocumentId, iDocumentTypeId, iUserId, miUserId);
                    FillUserListView();
                    base.DisplayMessage(S_DELETE_MESSAGE, false, tdMessage);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
  
    /// <summary>
    /// This event is used for Databound.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUserDocuments_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwUserDocuments.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstvwUserDocuments, DtPgCount);
            else
                DtPgCount.Visible = false;

            if (lstvwUserDocuments.Items.Count > Constants.I_ZERO)
            {
                hidCount.Value = lstvwUserDocuments.Items.Count.ToString();
                btnUpload.Visible = true;
            }
            else
                btnUpload.Visible = false;
            

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used set paging for list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwUserDocuments);
            DataPager oDataPager = lstvwUserDocuments.FindControl("DtPgDropDown") as DataPager;
            if (oDataPager != null)
            {
                DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
                if (ddlCnt != null)
                    hidPageNo.Value = ddlCnt.SelectedValue;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillUsers();

            lstvwUserDocuments.DataSourceID = null;
            lstvwUserDocuments.DataBind();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbnewclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillUserListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region "Private Method(s)"

    /// <summary>
    /// This method is used to set visibility according to action.
    /// </summary>
    /// <param name="abAction"></param>
    private void SetVisibility(bool abAction)
    {       
        trPhotoPager.Visible = abAction;
    }   

    /// <summary>
    /// This method fills combobox with standards.
    /// </summary>   
    private void FillUserRoleCombo()
    {
        UserRolewisePhotoUploadBL oUserRolewisePhotoUploadBL = new UserRolewisePhotoUploadBL(miSchoolId, miAcademicYearId);
        DataTable oDtUserRoleCollection = oUserRolewisePhotoUploadBL.GetUserRoleDetail();

        //DataRow[] oDataRows = oDtUserRoleCollection.Select("UserRoleId=" + Constants.UserRoles.Student.ToInt());
        //if (oDataRows.Length > 0)
        //{
        //    oDataRows[0].Delete();
        //    oDtUserRoleCollection.AcceptChanges();
        //}

        DataTable dt = oDtUserRoleCollection.Select("UserRoleId IN (1,2,3,6,7)").CopyToDataTable();

        ControlUtility.FillDropDownList(dt, ref cmbUserRole,
                                        "UserRoleId", "UserRoleName", Constants.S_SELECT);
        cmbUserRole.SelectedValue = Constants.I_ONE.ToString(); 
        
    }


    /// <summary>
    /// This method is used to fill User .
    /// </summary>
    private void FillUsers() //
    {
        int miuserrole = cmbUserRole.SelectedValue.ToInt();
        UploadUserDocumentBL oUserRolewisePhotoUploadBL = new UploadUserDocumentBL(miSchoolId, miAcademicYearId);
        
        DataTable oDT = oUserRolewisePhotoUploadBL.GetUsers(miSchoolId, miAcademicYearId, miuserrole, cmbClass.SelectedValue.ToInt());
        ControlUtility.FillDropDownList(oDT, ref cmbUser,
                                        "UserId", "UserName", Constants.S_SELECT);

        //DataTable oDT1 = oUserRolewisePhotoUploadBL.GetUsers(miSchoolId, miAcademicYearId, miuserrole, cmbnewclass.SelectedValue.ToInt());
        //ControlUtility.FillDropDownList(oDT, ref cmbUser,
        //                                "UserId", "UserName", Constants.S_SELECT);

    }

    /// <summary>
    /// This method is used to fill Document Type .
    /// </summary>
    /// 
    private void FillDocumentType()
    {
        UploadUserDocumentBL oUserRolewisePhotoUploadBL = new UploadUserDocumentBL(miSchoolId, miAcademicYearId);
        DataTable oDT = oUserRolewisePhotoUploadBL.GetDocumentTypes(miSchoolId);
        ControlUtility.FillDropDownList(oDT, ref cmbDocumentType,
                                        "Id", "Name", string.Empty);
    }

    protected void optDocumentWise_CheckedChanged(object sender, EventArgs e)
    {
        truser.Visible = false;
        trClass.Visible = false;
        trnewclass.Visible = true;
        DocumentWiseLstvw.Visible = true;
        trusername.Visible = true;
        trsearch.Visible = true;
        documenttype.Visible = true;
        trnote.Visible = true;
        trpanno.Visible = false;
        trEmpNo.Visible = false;
        trleftStudent.Visible = false;
        cmbUser.ClearSelection(); //
        cmbUserRole.ClearSelection(); //
        FillUserListView();
        chkLeftStudent.Checked =false;
    }

    protected void optUserWise_CheckedChanged(object sender, EventArgs e)
    {
        DocumentWiseLstvw.Visible = true;
        truser.Visible = true;
        trClass.Visible = true;
        trnewclass.Visible = false;
        cmbClass.Enabled = false;

        trusername.Visible = false;
        trsearch.Visible = false;
        documenttype.Visible = false;
        trnote.Visible = true;
        trpanno.Visible = true;
        trEmpNo.Visible = true;
        trleftStudent.Visible = false;
        FillUsers(); 
      
        cmbUserRole.ClearSelection(); //
        cmbDocumentType.ClearSelection(); //
        FillUserListView();

       
    }
    /// <summary>
    /// This method is used to fill User details in listview.
    /// </summary>
    private void FillUserListView()
    {
        lstvwUserDocuments.DataSourceID = lstvwDsObj.ID;
        lstvwUserDocuments.DataBind();

        HtmlTableRow tr = lstvwUserDocuments.FindControl("trHeader") as HtmlTableRow;
        if (tr != null)
        {
            if (cmbUserRole.SelectedValue.ToString() == "3" || optUserWise.Checked)
            {
                HtmlTableCell thPanNo = tr.FindControl("thpanno") as HtmlTableCell;
                if (thPanNo != null)
                    thPanNo.Visible = false;

                HtmlTableCell thEmpNo = tr.FindControl("thEmpNo") as HtmlTableCell;
                if (thEmpNo != null)
                    thEmpNo.Visible = false;
            }
            else if(optDocumentWise.Checked)
            {
                HtmlTableCell thPanNo = tr.FindControl("thpanno") as HtmlTableCell;
                if (thPanNo != null)
                    thPanNo.Visible = true;

                HtmlTableCell thEmpNo = tr.FindControl("thEmpNo") as HtmlTableCell;
                if (thEmpNo != null)
                    thEmpNo.Visible = true;
            }
        }
    }
    

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSearch, btnUpload });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidSchoolId.Value = miSchoolId.ToString();
        hidAcademicYearId.Value = miAcademicYearId.ToString();
        btnUpload.Attributes.Add("onclick", "ResetMessage()");
    }

    /// <summary>
    /// This method is used to populate user details.
    /// </summary>
    /// <returns></returns>
    private List<UserRolewiseDocumentDetails> Populate()
    {
        int iRowId = 0;
        string sFileName;
        List<UserRolewiseDocumentDetails> lstUserRolewiseDocumentDetails = new List<UserRolewiseDocumentDetails>();

        foreach (ListViewDataItem oListViewDataItem in lstvwUserDocuments.Items)
        {
            UserRolewiseDocumentDetails oUserRolewiseDocumentDetails = new UserRolewiseDocumentDetails();
            FileUpload oFileUpload = oListViewDataItem.FindControl("FileUploadDoc") as FileUpload;
            HiddenField hidDocFile = oListViewDataItem.FindControl("hidDocFile") as HiddenField;
            int iDocumentTypeId = 0;  // 
            iRowId = Convert.ToInt32(oListViewDataItem.DisplayIndex);
            int iUserId = 0;  //
            if (optDocumentWise.Checked == true) //
            {
                iUserId = Convert.ToInt32(lstvwUserDocuments.DataKeys[iRowId]["UserId"]);
                iDocumentTypeId = cmbDocumentType.SelectedValue.ToInt(); //
            }
            else            //
            {
                iUserId = cmbUser.SelectedValue.ToInt();
                iDocumentTypeId = Convert.ToInt32(lstvwUserDocuments.DataKeys[iRowId]["DocumentTypeId"]);  //
            }
            if (oFileUpload.HasFile)
            {   
                sFileName = SaveFileOnServer(oFileUpload, iRowId);
                oUserRolewiseDocumentDetails.UserId = iUserId;                
                oUserRolewiseDocumentDetails.DocumentFilePath = sFileName;
                oUserRolewiseDocumentDetails.DocumentTypeId = iDocumentTypeId;  //
                lstUserRolewiseDocumentDetails.Add(oUserRolewiseDocumentDetails);
            }
            else if (hidDocFile.Value != string.Empty)
            {
                oUserRolewiseDocumentDetails.UserId = iUserId;
                oUserRolewiseDocumentDetails.DocumentFilePath = hidDocFile.Value;
                oUserRolewiseDocumentDetails.DocumentTypeId = iDocumentTypeId;  //
                lstUserRolewiseDocumentDetails.Add(oUserRolewiseDocumentDetails);
            }
        }

        return lstUserRolewiseDocumentDetails;
    }

   
    /// <summary>
    /// This method is used to upload the file to the server.
    /// DeleteFiles();
    /// </summary>
    private string SaveFileOnServer(FileUpload FileUploadPhoto, int iRowId)
    {
        string asFileName = FileUploadPhoto.FileName;
        
        string sFolderName;
        if (cmbDocumentType.SelectedItem.Text == "Leaving Certificate")
           sFolderName  = Server.MapPath("..") + Constants.S_UPLOAD_LEAVING_CERTIFICATE_FOLDER_PATH;
        else
            sFolderName = Server.MapPath("..") + Constants.S_UPLOAD_FORM16_FOLDER_PATH;

        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;
        if (File.Exists(sServerFilePath))
        {
            sFileName = CommonUtility.GetFileNameForRenaming(asFileName);
            sServerFilePath = sFolderName + sFileName;
            oArrlstSave.Add(sServerFilePath);
        }
        FileUploadPhoto.SaveAs(sServerFilePath);
        
        return sFileName;
    }

    private void FillClasses()
    {
        StandardDivisionCollectionBL oStandardDivisionCollectionBL = new StandardDivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oClass = oStandardDivisionCollectionBL.GetAssociatedStandardsDivisions();
        ControlUtility.FillDropDownList(oClass, ref cmbClass, Constants.S_STANDARD_DIVISION_ID_FIELD, Constants.S_STANDARD_DIVISION_NAME_FIELD, Constants.S_SELECT_ALL);
        ControlUtility.FillDropDownList(oClass, ref cmbnewclass, Constants.S_STANDARD_DIVISION_ID_FIELD, Constants.S_STANDARD_DIVISION_NAME_FIELD, Constants.S_SELECT_ALL);
		
    }

    private void SetFieldValuesLeftStudent()
    {
        chkLeftStudent.Checked = true;
    }

    #endregion   
    
}