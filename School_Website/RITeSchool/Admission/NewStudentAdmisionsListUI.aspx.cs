// File Name   : NewStudentAdmisionsListUI.aspx.cs
// Created By  : Amit
// Date        : 26/11/2009
// Description : This class is used to display list of new admission students 

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using SchoolAutoSearchService.Service;
using System.Linq;
using SchoolEntities;
using System.Threading;
using System.Web;
using CrystalDecisions.Shared;
using System.Collections;
using System.Configuration;
public partial class NewStudentAdmisionsListUI : ExportDataTable
{
    #region " Constants "
    private const string S_DEFAULT_SORT_EXP = "Form_Number";
    private const string S_DEFAULT_ENQ_SORT_EXP = "Enquiry_No";
    private const string S_COMMAND_DELETE_ENQUIRY_DETAILS = "DeleteCommand";
    private const string S_COMMAND_DELETE_ADMISSION_DETAILS = "DeleteCommand";
    private const string S_DELETE_MSG = "Student Enquiry Details Deleted Successfully !!!";
    string S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = "N";    
    #endregion "Constants "

    #region " Constants "

    StudentAdmissionsBL moStudentAdmissionsBL;
    Hashtable moManualMobileNo = new Hashtable();

    #endregion

    #region " Events "
    /// <summary>
    /// This Event is handled to Add a Sort Image to the Tables
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(Object sender, EventArgs e)
    {
        try
        {
            if (cmbStatus.SelectedIndex != 4)
                SetSortImage(S_DEFAULT_SORT_EXP);
            else
                SetSortEnqImage(S_DEFAULT_ENQ_SORT_EXP);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill details of admission form submiting students.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStudentAdmissionsBL = new StudentAdmissionsBL();
            if (ViewState["AcademicYearDetails"] != null)
            {
                DataTable dtAcademicDetails = (DataTable)ViewState["AcademicYearDetails"];

                DataRow[] dtrow = dtAcademicDetails.Select("AcademicYearId=" + ddlAcademicYEar.SelectedValue);
                if (dtrow.Length > Constants.I_ZERO)
                {
                    if (dtrow[0]["IsCurrentYear"].ToBool() == true)
                        S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = Constants.S_YES;
                    else
                        S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = Constants.S_NO;
                }
            }
            else
                S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = Settings.ShowAdmissionForCurrentYear ? Constants.S_YES : Constants.S_NO;
          
            if (!IsPostBack)
            {
                FillAcademicYearCombo();
                LoadPageControls();
                
            if (QueryString["AcademicYearId"] != null && QueryString["StatusId"] != null)
                {
                    ddlAcademicYEar.SelectedValue = QueryString["AcademicYearId"].ToString();
                    cmbStatus.SelectedValue = QueryString["StatusId"].ToString();
                    cmbStatus_SelectedIndexChanged(cmbStatus, null);
                }
                else if (QueryString["AcademicYearId"] != null)
                {
                    ddlAcademicYEar.SelectedValue = QueryString["AcademicYearId"].ToString();
                    ddlAcademicYEar_SelectedIndexChanged(ddlAcademicYEar, null);
                }
            }
            
            else DtPgCount.Visible = false;

            base.SetDefaultButton(btnShow);
            
            if (SchoolBase.Settings.IsEnableEnquiry)
            {
                if (miSchoolId != Constants.SchoolId.SNS.ToInt() && miSchoolId != Constants.SchoolId.PIONEER.ToInt())
                    btnAdd.Visible = false;
              
                  btnAddEnquiry.Visible = true;
               
                if (miSchoolId == Constants.SchoolId.SPS.ToInt() || miSchoolId == Constants.SchoolId.SVP.ToInt() || miSchoolId == Constants.SchoolId.SVNP.ToInt())
                {
                    btnAddEnquiry.Text = "Add Registration";
                    trAdmissionFor.Visible = true;
                    hidAdmissionFor.Value = cmbAdmissionFor.SelectedValue;
                }
                else
                {
                    trAdmissionFor.Visible = false;
                    hidAdmissionFor.Value = Constants.S_ZERO;
                }
            }
            HideEnquiryFormColumn();
            HideRegistrationFormColumn();
            HideDeleteColumn();
          SetJavascriptAttributes();
        }
       catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill all the controls on page load.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LoadPageControls()
    {
        lblEnqError.Text = GetErrorMessage();

        lblEnqError.Visible = true;
        GetNewAcadamicYearID();
        FillAllPageControls();
        SetDefaultProperties();
        GetScreenConfigDetails();
        SetDefaultState();
        DtPgCount.Visible = true;

        if (miSchoolId == Constants.SchoolId.SPS.ToInt() || miSchoolId == Constants.SchoolId.SVP.ToInt() || miSchoolId == Constants.SchoolId.SVNP.ToInt())
        {
            cmbStatus.SelectedValue = "4";
            cmbStatus.Enabled = false;
            cmbStatus_SelectedIndexChanged(cmbStatus, null);
        }        
    }

    private void SetDefaultState()
    {
        lstviewEnquiryDetails.DataSourceID = null;
        lstviewEnquiryDetails.Visible = false;
        DataPager1.Visible = false;
        DtPgCount.Visible = false;
        btnExportEnq.Visible = true;
        btnExport.Visible = true;
    }

    private void SetJavascriptAttributes()
    {
        btnConfirm.Attributes.Add("onclick", "if(!VerifyAtleastOneCheckBox()){return false;}");
        btnSave.Attributes.Add("onclick", "if(!Confirmed()){return false;}");
        DtPgCount.Visible = true;      
		trSubmissionStatus.Visible = true;		
    }

    /// <summary>
    /// This event is used to show student list as per applied filter criteria.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmbStatus.SelectedIndex != 4)
            {
                FillListView();
            }
            else
                FillEnquiryListView();
               
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            lstviewEnquiryDetails.DataSource = null;
            lblEnqError.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save confirmed student details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if(miSchoolId != Constants.SchoolId.SPS.ToInt() & miSchoolId != Constants.SchoolId.SVP.ToInt() && miSchoolId != Constants.SchoolId.SVNP.ToInt())
                SaveStudentDetails(lstvwStudentDetails);
            else
                SaveStudentDetails(lstviewEnquiryDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to change academic year combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAcademicYEar_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadPageControls();
            FillListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add button event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtAcademicDetails = new DataTable();
            bool bIsCurrentYearAdmission = false;
            string sQueryString = string.Empty;
            if (ViewState["AcademicYearDetails"] != null)
            {
                dtAcademicDetails =(DataTable) ViewState["AcademicYearDetails"];

                DataRow[] dtrow = dtAcademicDetails.Select("AcademicYearId=" + ddlAcademicYEar.SelectedValue);
                if (dtrow.Length > Constants.I_ZERO)
                {
                    if (dtrow[0]["IsCurrentYear"].ToBool() == true)
                        bIsCurrentYearAdmission = true;
                }
                sQueryString = "IsCurrentYearAdmission=" + bIsCurrentYearAdmission + "&AcademicYearId=" + ddlAcademicYEar.SelectedValue;
            }

            MasterPage oMasterPage = (MasterPage)this.Master;

            if (miSchoolId != Constants.SchoolId.SNS.ToInt())
                oMasterPage.RedirectToNextPage("AdmissionFormStudentDetails.aspx?" + CommonUtility.EncryptQuerystring(sQueryString));
            else
                oMasterPage.RedirectToNextPage("StudentRegistrationDetails.aspx?" + CommonUtility.EncryptQuerystring(sQueryString));            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnAddEnquiry_Click(object sender, EventArgs e)
    {
        try
        {
           MasterPage oMaster = this.Master as MasterPage;

           if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            {
                string sRegistQueryString = CommonUtility.EncryptQuerystring( "AcademicYearId=" + ddlAcademicYEar.SelectedValue + "&StatusId=4&IsEnquiry=1" );

                oMaster.RedirectToNextPage("../Admission/StudentRegistrationDetails.aspx?" + sRegistQueryString);
            }
            else
            {
                string sQueryString = CommonUtility.EncryptQuerystring( "AcademicYearId=" + ddlAcademicYEar.SelectedValue + "&StatusId=4" );
                 oMaster.RedirectToNextPage("../Admission/EnquiryForm.aspx?" + sQueryString);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

 #endregion " Events "

    #region " Listview Events "

    /// <summary>
    /// This event is used to set list view footer and sorting image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStudentDetails.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwStudentDetails, DtPgCount);
                HtmlControl oThReceipt = lstvwStudentDetails.FindControl("thReceipt") as HtmlControl;
                oThReceipt.Visible = Settings.EnableAdmissionFormFee;
                btnExport.Visible = true;
                DtPgCount.Visible = true;
                btnExportEnq.Visible = false;
                ChangeListHeaderText();
            }
            else
            {
                DtPgCount.Visible = false;
                btnConfirm.Enabled = false;
                btnExport.Visible = false;
            }
            HideRegistrationFormColumn();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to set list view footer and sorting image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstviewEnquiryDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstviewEnquiryDetails.Items.Count > 0)
            {   
                ChangeListHeaderText();

                ControlUtility.FillListViewPagerFooter(lstviewEnquiryDetails, DataPager1);
                DataPager1.Visible = true;
               
            }
            else
            {
                DataPager1.Visible = false;
                btnExport.Visible = false;
            }
            
            // Set thEnquiryForm visibility after data binding
            HideEnquiryFormColumn();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
   
    /// <summary>
    /// This event is used to add java script to hyper link to open admission form.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                HyperLink olnkbtnForm = e.Item.FindControl("lnkbtnForm") as HyperLink;
                CheckBox ocheckBox = e.Item.FindControl("chkIsConfirm") as CheckBox;
                HyperLink olnkReceipt = e.Item.FindControl("lnkReceipt") as HyperLink;
                HyperLink lnkConfirmationForm = e.Item.FindControl("lnkConfirmationForm") as HyperLink;

                string sAdmissionId = Convert.ToString(lstvwStudentDetails.DataKeys[iRowId]["Student_Admission_Id"]);
                string sReceiptNo = ((System.Data.DataRowView)oCurrentItem.DataItem).Row["Receipt_Number"].ToString();
                string sFormNo = ((System.Data.DataRowView)oCurrentItem.DataItem).Row["Form_Number"].ToString();
                string sAcademicYearId = ((System.Data.DataRowView)oCurrentItem.DataItem).Row["Acedemic_Year_Id"].ToString();
                string sSelectedInLottery = Convert.ToString(lstvwStudentDetails.DataKeys[iRowId]["SelectedInLottery"]);
                bool bIsLotteryConfirmed = Convert.ToBoolean(lstvwStudentDetails.DataKeys[iRowId]["IsLotteryConfirmed"]);
                bool bCanConfirmDirectly = Convert.ToBoolean(lstvwStudentDetails.DataKeys[iRowId]["CanConfirmDirectly"]);
                bool bIsConfirmed = Convert.ToBoolean(lstvwStudentDetails.DataKeys[iRowId]["IsConfirmed"]);
                string sQuerystringForFrom = string.Empty;
                HtmlTableCell td = e.Item.FindControl("tdConfirmationForm") as HtmlTableCell;                
                if (ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.SPS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.SVP.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.SVNP.ToInt())
                {
                    if(td != null)
                        td.Visible = false;
                    lnkConfirmationForm.Visible = false;
                    
                    bool bIsCurrentYearAdmission = false;
                    if (ViewState["AcademicYearDetails"] != null)
                    {
                        DataTable dtAcademicDetails = (DataTable)ViewState["AcademicYearDetails"];

                        DataRow[] dtrow = dtAcademicDetails.Select("AcademicYearId=" + ddlAcademicYEar.SelectedValue);
                        if (dtrow.Length > Constants.I_ZERO)
                        {
                            if (dtrow[0]["IsCurrentYear"].ToBool() == true)
                                bIsCurrentYearAdmission = true;
                        }
                    }

                    sQuerystringForFrom = "iAdmissionId=" + sAdmissionId + "&IsCurrentYearAdmission=" + bIsCurrentYearAdmission + "&IsTeachersCopy=1";

                    if(moSchool == Constants.SchoolId.PPSH)
                        olnkbtnForm.NavigateUrl = string.Format("javascript:openNewReport('{0}');", olnkbtnForm.NavigateUrl + "?" + CommonUtility.EncryptQuerystring(sQuerystringForFrom));
                    else
                        olnkbtnForm.NavigateUrl = string.Format("javascript:openReport('{0}');", olnkbtnForm.NavigateUrl + "?" + CommonUtility.EncryptQuerystring(sQuerystringForFrom));
                }
                else
                {
                    sQuerystringForFrom = "StudentAdmissionId=" + sAdmissionId;
                    olnkbtnForm.Visible = false;
                    olnkbtnForm.NavigateUrl = string.Format("javascript:openReport('{0}');", olnkbtnForm.NavigateUrl + "?" + CommonUtility.EncryptQuerystring(sQuerystringForFrom));

                    if (bIsConfirmed)
                    {
                        lnkConfirmationForm.Visible = false;
                        string sQuryString = "StudentAdmissionId=" + sAdmissionId + "&IsConfirmationForm=1";
                        lnkConfirmationForm.NavigateUrl = string.Format("javascript:openReport('{0}');", lnkConfirmationForm.NavigateUrl + "?" + CommonUtility.EncryptQuerystring(sQuryString));
                    }
                    else
                    {
                        lnkConfirmationForm.Visible = false;
                    }
                }

                if (sReceiptNo != Constants.S_ZERO)
                {
                    string sQueryStringForReceipt = "iAdmissionId=" + sAdmissionId + "&ReceiptNo=" + sReceiptNo + "&FormNo=" + sFormNo + "&AcademicYear=" + sAcademicYearId;
                    olnkReceipt.NavigateUrl = olnkReceipt.NavigateUrl + "?" + CommonUtility.EncryptQuerystring(sQueryStringForReceipt);
                    olnkReceipt.Attributes.Add("onclick", "window.open('" + olnkReceipt.NavigateUrl + "' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=860,height=650'); return false;");
                }
                else
                    olnkReceipt.Text = string.Empty;

                if (Convert.ToInt32(ddlStandard.SelectedValue) == 0)
                    ocheckBox.Enabled = false;
                else
                    ocheckBox.Enabled = true;

                if (bCanConfirmDirectly == false && (sSelectedInLottery != "M"))
                {
                    ocheckBox.Visible = false;
                    btnConfirm.Enabled = false;
                }
                else if (bCanConfirmDirectly == true && sSelectedInLottery != "M" && bIsLotteryConfirmed == true)
                {
                    ocheckBox.Visible = false;
                    btnConfirm.Enabled = true;
                }

                if(!bCanConfirmDirectly)
                    ocheckBox.Visible = false;

                HtmlControl oTdReceipt = oCurrentItem.FindControl("tdReceipt") as HtmlControl;
                if (!oTdReceipt.IsNull())
                    oTdReceipt.Visible = Settings.EnableAdmissionFormFee;

                string sStatusQueryString = CommonUtility.EncryptQuerystring("StudentAdmissionId=" + sAdmissionId);
                HiddenField hidQueryString = oCurrentItem.FindControl("hidQueryString") as HiddenField;
                hidQueryString.Value = sStatusQueryString;

                LinkButton lnkStatus = oCurrentItem.FindControl("lnkStatus") as LinkButton;
                lnkStatus.Attributes.Add("onclick", "OpenStatusPopup(" + oCurrentItem.DisplayIndex + "); return false;");
                if (rbUnSuccessful.Checked == true)
                    ocheckBox.Visible = false;

                ImageButton imgEdit = e.Item.FindControl("btnEdit") as ImageButton;
                if (!bIsConfirmed)
                {
                    string sQueryString = string.Empty;
                    sQueryString = "FormNumber=" + sFormNo + "&StudetAdmissionId=" + sAdmissionId + "&AcademicYearId=" + ddlAcademicYEar.SelectedValue + "&IsEditMode=1";
                    if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                    {
                        string sStandardName = Convert.ToString(lstvwStudentDetails.DataKeys[iRowId]["Standard_Name"]);
                        sQueryString = string.Empty;
                        sQueryString = "StudetAdmissionId=" + sAdmissionId + "&AcademicYearId=" + ddlAcademicYEar.SelectedValue + "&StatusId=" + cmbStatus.SelectedValue
                            + "&StandardName=" + sStandardName + "&IsEnquiry=0";
                        imgEdit.PostBackUrl = "StudentRegistrationDetails.aspx?" + CommonUtility.EncryptQuerystring(sQueryString);
                   }
                    else
                        imgEdit.PostBackUrl = "AdmissionFormStudentDetails.aspx?" + CommonUtility.EncryptQuerystring(sQueryString);
                }
                else
                {
                    imgEdit.Visible = false;
                }
                                
                HtmlTableCell tdRegForm = e.Item.FindControl("tdRegForm") as HtmlTableCell;
                if (tdRegForm != null)
                {
                    if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                    {
                        tdRegForm.Visible = true;
                        string sQuerystringForEnquiryFrom = "iEnquiryId=0" + "&AdmissionId=" + sAdmissionId + "&IsTeachersCopy=1";
                        HyperLink olnkbtnRegForm = e.Item.FindControl("lnkbtnRegForm") as HyperLink;
                        olnkbtnRegForm.NavigateUrl = string.Format("javascript:openNewReport('{0}');", olnkbtnRegForm.NavigateUrl + "?" + CommonUtility.EncryptQuerystring(sQuerystringForEnquiryFrom));
                    }
                    else
                        tdRegForm.Visible = false;
                }

                ImageButton imgDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                if (imgDelete != null)
                {
                    bool allowDelete = Settings.EnableDeleteButtonforStudentRegistration;
                    imgDelete.OnClientClick = "if (!ConfirmDelete()) { return false; }";
                    imgDelete.Visible = allowDelete;
                }
             }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event is called while row in list view is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == S_COMMAND_DELETE_ADMISSION_DETAILS)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                int iId = lstvwStudentDetails.DataKeys[iRowId]["Student_Admission_Id"].ToInt();

                StudentAdmissionsBL.DeleteStudentRegistrationform(iId, miSchoolId,miUserId);
                lblUpdateSuccess.Text = S_DELETE_MSG;

                FillListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event is called while row in list view is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstviewEnquiryDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            int iRowId = oCurrentItem.DisplayIndex;

            int iId = lstviewEnquiryDetails.DataKeys[iRowId]["Id"].ToInt();
            int iStatusId = lstviewEnquiryDetails.DataKeys[iRowId]["StatusId"].ToInt();
            string sRegistrationNo = lstviewEnquiryDetails.DataKeys[iRowId]["Enquiry_No"].ToString();
            

            if (e.CommandName == S_COMMAND_DELETE_ENQUIRY_DETAILS)
            {
                DeleteStudentEnquiryDetails(iId, oCurrentItem,miUserId);
                FillEnquiryListView();
            }
            else if (e.CommandName == "Paid")
            { 
                if (iStatusId == 4)
                {
                    string sMobileNumbers = string.Empty;                    
                    moStudentAdmissionsBL.UpdateEnquiryStatus(miSchoolId, miAcademicYearId, iId, miUserId, out sMobileNumbers);                    
                    FillEnquiryListView();

                    foreach (string sMobileNo in sMobileNumbers.Trim().Split(','))
                        if (sMobileNo.Trim() != string.Empty && !moManualMobileNo.ContainsKey(sMobileNo.Trim()))
                            moManualMobileNo[sMobileNo.Trim()] = sMobileNo.Trim();

                    SendRegistrationSMS(sRegistrationNo, sMobileNumbers);
                }                
            }
        }       
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this method is used to delete student enquiry details record from enquiry listview.
    /// </summary>
    private void DeleteStudentEnquiryDetails(int iId, ListViewDataItem oCurrentItem, int aiUpdatedById)
    {
        StudentAdmissionsBL.DeleteStudentEnquiryDetails(iId, miSchoolId, aiUpdatedById);
        lblUpdateSuccess.Text = S_DELETE_MSG;
    }
      
    /// <summary>
    /// This event is used add sorting to list view columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetSortVariables();
            hidSortExpression.Value = e.SortExpression;
            DataPager1.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used add sorting to list view columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstviewEnquiryDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetEnqSortVariables();
            HidEnqSortExprsn.Value = e.SortExpression;
            DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set paging of list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //trStud.Visible = true; //
            //trenq.Visible = false; //
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStudentDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set paging of list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlEnqCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //trenq.Visible = true;  //
            //trStud.Visible = false;  //
            ControlUtility.SetDataPagerAccordingToPageNo(lstviewEnquiryDetails);

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlStandard.SelectedValue) == 0)
                btnConfirm.Enabled = false;

            if ((Convert.ToInt32(ddlStandard.SelectedValue) == 0 && cmbStatus.SelectedIndex == 4) || miSchoolId == Constants.SchoolId.SPS.ToInt() || miSchoolId == Constants.SchoolId.SVP.ToInt() || miSchoolId == Constants.SchoolId.SVNP.ToInt())
            {
                if (lstviewEnquiryDetails.Items.Count > 0)
                {
                    ControlUtility.FillListViewPagerFooter(lstviewEnquiryDetails, DataPager1);
                    FillEnquiryListView();
                    lblEnqError.Visible = false;
                    DtPgCount.Visible = false;

                    if (miSchoolId == Constants.SchoolId.SPS.ToInt() || miSchoolId == Constants.SchoolId.SVP.ToInt() || miSchoolId == Constants.SchoolId.SVNP.ToInt())
                    {
                        rdoFinal.Checked = true;
                        FillStandardPopupCombo(Convert.ToInt32(ddlStandard.SelectedValue));
                    }
                }
            }

            else
            {
                rdoFinal.Checked = true;
                FillStandardPopupCombo(Convert.ToInt32(ddlStandard.SelectedValue));

                if (miSchoolId != Constants.SchoolId.SPS.ToInt() || miSchoolId != Constants.SchoolId.SVP.ToInt() || miSchoolId != Constants.SchoolId.SVNP.ToInt())
                    btnConfirm.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event i used to update status of respective row.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void hidStudentAdmissionId_ValueChanged(object sender, EventArgs e)
    {
        try
        {
            AdmissionDetails oAdmissionDetails;
            int iStudentAdmissionId = Convert.ToInt32(hidStudentAdmissionId.Value);
            StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL(miSchoolId, miAcademicYearId, miUserId);
            oStudentAdmissionsBL.GetAllComments(iStudentAdmissionId, out oAdmissionDetails);

            ListItem oListItem = cmbStatus.Items.FindByValue(oAdmissionDetails.StatusId.ToString());
            if (oListItem != null)
            {
                foreach (ListViewDataItem oItem in lstvwStudentDetails.Items)
                {
                    int iAdmissinId = lstvwStudentDetails.DataKeys[oItem.DisplayIndex]["Student_Admission_Id"].ToInt();
                    if (iAdmissinId == iStudentAdmissionId)
                    {
                        LinkButton lnkStatus = oItem.FindControl("lnkStatus") as LinkButton;
                        if (lnkStatus != null)
                            lnkStatus.Text = oListItem.Text;
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to generate report.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string sStudentName = txtStudentName.Text;
            string sSortExpression = Convert.ToString(hidSortExpression.Value);
            int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
            int iAdmissionTypeId = Convert.ToInt32(ddlAdmissionType.SelectedValue);
            int iStatusId = Convert.ToInt32(cmbStatus.SelectedValue);
            string asAdmissionStartDate = Convert.ToString(txtAdmissionStartDate.Text);
            string asAdmissionEndDate = Convert.ToString(txtAdmissionEndDate.Text);
            StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL();
            //DataTable dtStudentAdmission = oStudentAdmissionsBL.GetAllNewStudentDetails(miSchoolId, hidNextAcademiYearId.Value.ToInt(), iStandardId, iAdmissionTypeId, sStudentName, iStatusId, 0, hidAdmissionFor.Value.ToInt(), rbSuccessful.Checked, asAdmissionStartDate, asAdmissionEndDate, sSortExpression, 20000);

            DataTable dtStudentAdmission = oStudentAdmissionsBL.GetAllNewStudentDetails(miSchoolId, hidNextAcademiYearId.Value.ToInt(), iStandardId, iAdmissionTypeId, sStudentName, iStatusId, sSortExpression, 0, 20000, rbSuccessful.Checked, hidAdmissionFor.Value.ToInt(), asAdmissionStartDate, asAdmissionEndDate);

           HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Report-StudentAdmission.XLS");
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><BR><BR>");

            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:15px; font-family:Calibri; background:white;'>");
            HttpContext.Current.Response.Write("<TR>");

            AddHeader("Standard Name", "text-align:center; font-weight:bold; font-size:17px;");
            AddHeader("Admission Type", "text-align:center; font-weight:bold; font-size:17px;");
            AddHeader("Form Number", "text-align:center; font-weight:bold; font-size:17px;");
            AddHeader("Student Name", "text-align:left; font-weight:bold; font-size:17px;");
            AddHeader("Mobile Number", "text-align:center; font-weight:bold; font-size:17px;");
            AddHeader("Is Confirm", "text-align:center; font-weight:bold; font-size:17px;");
            AddHeader("Date Of Birth", "text-align:center; font-weight:bold; font-size:17px;");           
            AddHeader("Last School Name", "text-align:center; font-weight:bold; font-size:17px;");
            AddHeader("Last Completed Standard", "text-align:center; font-weight:bold; font-size:17px;");
            AddHeader("Guardian/Parent Name", "text-align:center; font-weight:bold; font-size:17px;");
            AddHeader("Address", "text-align:left; font-weight:bold; font-size:17px;");
            AddHeader("Email ID", "text-align:left; font-weight:bold; font-size:17px;");
            AddHeader("Living Location", "text-align:left; font-weight:bold; font-size:17px;");
            if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
            {
                AddHeader("Preference", "text-align:left; font-weight:bold; font-size:17px;");
                AddHeader("Mobile Number1", "text-align:center; font-weight:bold; font-size:17px;");
                AddHeader("Mobile Number2", "text-align:center; font-weight:bold; font-size:17px;");                
            }
            if (miSchoolId == Constants.SchoolId.PPS.ToInt())
            {
                AddHeader("Sibling Student Name", "text-align:center; font-weight:bold; font-size:17px;");
                AddHeader("Sibling Student Standard", "text-align:center; font-weight:bold; font-size:17px;");
                AddHeader("Sibling Student Division", "text-align:center; font-weight:bold; font-size:17px;");                
            }
			
			AddHeader("Previous School UDISE No", "text-align:center; font-weight:bold; font-size:17px;");

            if(moSchool != Constants.SchoolId.PPSN)
                AddHeader("Previous School Saral Id", "text-align:center; font-weight:bold; font-size:17px;");

            if (miSchoolId == Constants.SchoolId.PPS.ToInt())
            {
                AddHeader("Admission Category", "text-align:center; font-weight:bold; font-size:17px;");
                AddHeader("Preference batch", "text-align:center; font-weight:bold; font-size:17px;");
            }

            if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
                AddHeader("Sibling Name", "text-align:center; font-weight:bold; font-size:17px");
            if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            {
               AddHeader("Sibling Name", "text-align:left; font-weight:bold; font-size:17px;");
               AddHeader("Sibling Age", "text-align:center; font-weight:bold; font-size:17px;");
               AddHeader("Sibling Institution", "text-align:left; font-weight:bold; font-size:17px;");
               AddHeader("Sibling Standard", "text-align:center; font-weight:bold; font-size:17px;");
               AddHeader("Sibling Name 2", "text-align:left; font-weight:bold; font-size:17px;");
               AddHeader("Sibling Age 2", "text-align:center; font-weight:bold; font-size:17px;");
               AddHeader("Sibling Institution 2", "text-align:left; font-weight:bold; font-size:17px;");
               AddHeader("Sibling Standard 2", "text-align:center; font-weight:bold; font-size:17px;");
               AddHeader("Mother Name", "text-align:left; font-weight:bold; font-size:17px;");
               AddHeader("Mother Educational Qualification", "text-align:left; font-weight:bold; font-size:17px;");
               AddHeader("Mother Occupation", "text-align:left; font-weight:bold; font-size:17px;");
               AddHeader("Mother OfficeAddress ", "text-align:left; font-weight:bold; font-size:17px;");
               AddHeader("Father Name", "text-align:left; font-weight:bold; font-size:17px;");
               AddHeader("Father Educational Qualification", "text-align:left; font-weight:bold; font-size:17px;");
               AddHeader("Father Occupation", "text-align:left; font-weight:bold; font-size:17px;");
               AddHeader("Father OfficeAddress ", "text-align:left; font-weight:bold; font-size:17px;");
              }

            AddHeader("PEN No.", "text-align:left; font-weight:bold; font-size:17px;");
            AddHeader("AAPAR ID", "text-align:left; font-weight:bold; font-size:17px;");
            AddHeader("Aadhar Card Number", "text-align:center; font-weight:bold; font-size:17px;");
            AddHeader("Name As Per Aadhar Card", "text-align:left; font-weight:bold; font-size:17px;");

            HttpContext.Current.Response.Write("</TR>");

            foreach (DataRow row in dtStudentAdmission.Rows)
            {
                HttpContext.Current.Response.Write("<TR>");

                AddTableRows(row["Standard_Name"].ToString(), "text-align:left");
                AddTableRows((Convert.ToInt32(row["Receipt_Number"]) == 0 ? "Manual Admission" : "Online Admission"), "text-align:left");
                AddTableRows(row["Form_Number"].ToString(), "text-align:left");
                AddTableRows(row["StudentName"].ToString(), "text-align:left");
                AddTableRows(row["MobileNumber"].ToString(), "text-align:center");
                AddTableRows(Convert.ToInt32(row["IsConfirmed"]) == 0 ? "No" : "Yes", "text-align:center");
                AddTableRows(row["DOB"].ToDateTime().ToString(Constants.S_DATE_FORMAT), "text-align:center");
                AddTableRows(row["LastSchoolName"].ToString(), "text-align:left");
                AddTableRows(row["LastCompletedStd"].ToString(), "text-align:left");
                AddTableRows(row["GuardianName"].ToString(), "text-align:left");
                AddTableRows(row["Address"].ToString(), "text-align:left");
                AddTableRows(row["EmailAddress"].ToString(), "text-align:left");
                AddTableRows(row["LivingLocationName"].ToString(), "text-align:left");
                if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
                {
                    AddTableRows(row["ResidenceType"].ToString(), "text-align:left");
                    AddTableRows(row["MobileNumber1"].ToString(), "text-align:center");
                    AddTableRows(row["MobileNumber2"].ToString(), "text-align:center");                    
                }
                if (miSchoolId == Constants.SchoolId.PPS.ToInt())
                {
                    AddTableRows(row["SiblingStudentName"].ToString(), "text-align:left");
                    AddTableRows(row["SiblingStudentStandard"].ToString(), "text-align:left");
                    AddTableRows(row["SiblingStudentDivision"].ToString(), "text-align:left");                    
                }
				
				AddTableRows(row["LastSchoolUDISENo"].ToString(), "text-align:left");
                
                if (moSchool != Constants.SchoolId.PPSN)
                    AddTableRows(row["PreviousSchoolSaralId"].ToString(), "text-align:left");

                if (miSchoolId == Constants.SchoolId.PPS.ToInt())
                {
                    AddTableRows(row["LivingLocationName"].ToString(), "text-align:left");
                    AddTableRows(row["PreferenceBatch"].ToString(), "text-align:left");
                }

                if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
                {
                    string sSiblingName = "-";
                    if (row["SiblingStudentName"] != DBNull.Value && row["SiblingStudentName"].ToString() != string.Empty)
                    {
                        if (row["Class"] != DBNull.Value && row["Class"].ToString() != string.Empty)
                            sSiblingName = row["SiblingStudentName"].ToString() + "(" + row["Class"].ToString()+")";
                        else
                            sSiblingName = row["SiblingStudentName"].ToString();
                    }

                    AddTableRows(sSiblingName);
                }

                if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                {
                    AddTableRows(row["Name1"] == DBNull.Value || row["Name1"].ToString() == "0" ? "" : row["Name1"].ToString(), "text-align:left");
                    AddTableRows(row["Age1"] == DBNull.Value  || row["Age1"].ToString() == "0" ? "" : row["Age1"].ToString(), "text-align:center");
                    AddTableRows(row["Institution1"] == DBNull.Value   || row["Institution1"].ToString() == "0" ? "" : row["Institution1"].ToString(), "text-align:left");
                    AddTableRows(row["StandardName1"] == DBNull.Value || row["StandardName1"].ToString() == "0" ? "" : row["StandardName1"].ToString(), "text-align:center");
                    AddTableRows(row["Name2"] == DBNull.Value || row["Name2"].ToString() == "0" ? "" : row["Name2"].ToString(), "text-align:left");
                    AddTableRows(row["Age2"] == DBNull.Value || row["Age2"].ToString() == "0" ? "" : row["Age2"].ToString(), "text-align:center");
                    AddTableRows(row["Institution2"] == DBNull.Value  || row["Institution2"].ToString() == "0" ? "" : row["Institution2"].ToString(), "text-align:left");
                    AddTableRows(row["StandardName2"] == DBNull.Value  || row["StandardName2"].ToString() == "0" ? "" : row["StandardName2"].ToString(), "text-align:center");
                    AddTableRows(row["Mother_Name"].ToString(), "text-align:left");
                    AddTableRows(row["M_Educational_Qualification"].ToString(), "text-align:left");
                    AddTableRows(row["M_Occupation"].ToString(), "text-align:left");
                    AddTableRows(row["M_OfficeAddress"].ToString(), "text-align:left");
                    AddTableRows(row["Father_Name"].ToString(), "text-align:left");
                    AddTableRows(row["F_Educational_Qualification"].ToString(), "text-align:left");
                    AddTableRows(row["F_Occupation"].ToString(), "text-align:left");
                    AddTableRows(row["F_OfficeAddress"].ToString(), "text-align:left");
               }

                AddTableRows(row["PenNo"].ToString(), "mso-number-format:\"\\@\";");
                AddTableRows(row["ApaarId"].ToString(), "mso-number-format:\"\\@\";");
                AddTableRows(row["AadharCardNo"].ToString(), "mso-number-format:\"\\@\"; text-align:center;");
                AddTableRows(row["NameAsPerAadharCard"].ToString(), "text-align:left");

                HttpContext.Current.Response.Write("</TR>");
            }

            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End(); 
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to generate report.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExportEnq_Click(object sender, EventArgs e)
    {
        try
        {
            string sStandardName, sEnquiryNumber, sStudentNames, sStatus, sAddress, sPreviousStandard, sWhatsupMother, sWhatsupFather, sMotherQualification, sFatherQualification, sFirstName, sMiddleName, sLastName, sDateofBirth, sMotherMobileNo, sFatherEmailAddress,  sLandmark, sdate;  ////
            string sMobileNumber, sGender;
            string sStudentName = txtStudentName.Text;
            int iAdmissionTypeId = Convert.ToInt32(ddlAdmissionType.SelectedValue);
            int iAdmissionStatusId = Convert.ToInt32(cmbStatus.SelectedValue);
            int iLocationid = Convert.ToInt32(cmbSchoolLocation.SelectedValue);
            string sSortExpression = null;
            int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
            string asStartDate =  Convert.ToString( txtEnquiryStartDate.Text); ////
            string asEndDate = Convert.ToString(txtEnquiryEndDate.Text); ////
            
            SchoolEnquiryBL oSchoolEnquiryBL = new SchoolEnquiryBL();
            DataTable dtStudentEnquiry = oSchoolEnquiryBL.GetAllStuEnquiryDetails(miSchoolId, ddlAcademicYEar.SelectedValue.ToInt(), iLocationid,iStandardId, iAdmissionTypeId, sStudentName, sSortExpression, 0, 20000, iAdmissionStatusId, Constants.I_ZERO, asStartDate, asEndDate);
         
            DataTable dtStudentenquiryReport = new DataTable();
            
            dtStudentenquiryReport.Columns.Add("Standard Name", typeof(string));
            dtStudentenquiryReport.Columns.Add("Enquiry No", typeof(string));
            dtStudentenquiryReport.Columns.Add("Student Name", typeof(string));

            dtStudentenquiryReport.Columns.Add("Last Name", typeof(string));
            dtStudentenquiryReport.Columns.Add("First Name", typeof(string));
            dtStudentenquiryReport.Columns.Add("Middle Name", typeof(string));
            
            dtStudentenquiryReport.Columns.Add("Gender", typeof(string));
            dtStudentenquiryReport.Columns.Add("Date of Birth", typeof(string));

            dtStudentenquiryReport.Columns.Add("Father Last Name", typeof(string));
            dtStudentenquiryReport.Columns.Add("Father First Name", typeof(string));
            dtStudentenquiryReport.Columns.Add("Father Middle Name", typeof(string));
            dtStudentenquiryReport.Columns.Add("Father Qualification", typeof(string));
            dtStudentenquiryReport.Columns.Add("Father WhatsApp No.", typeof(string));
            dtStudentenquiryReport.Columns.Add("Father Mobile Number", typeof(string));
            dtStudentenquiryReport.Columns.Add("Father Email Address", typeof(string));

            dtStudentenquiryReport.Columns.Add("Mother Last Name", typeof(string));
            dtStudentenquiryReport.Columns.Add("Mother First Name", typeof(string));
            dtStudentenquiryReport.Columns.Add("Mother Middle Name", typeof(string));
            dtStudentenquiryReport.Columns.Add("Mother Qualification", typeof(string));
            dtStudentenquiryReport.Columns.Add("Mother WhatsApp No.", typeof(string));
            dtStudentenquiryReport.Columns.Add("Mother Mobile Number", typeof(string));
            dtStudentenquiryReport.Columns.Add("Mother Email Address", typeof(string));

            dtStudentenquiryReport.Columns.Add("Address", typeof(string));
            dtStudentenquiryReport.Columns.Add("Landmark", typeof(string));
            dtStudentenquiryReport.Columns.Add("Previous Standard", typeof(string));
            dtStudentenquiryReport.Columns.Add("Previous School Name", typeof(string));
            
            dtStudentenquiryReport.Columns.Add("Source", typeof(string));
            dtStudentenquiryReport.Columns.Add("Status", typeof(string));
            dtStudentenquiryReport.Columns.Add("Date", typeof(string));  //////

            string sFatherLastName, sFatherFirstName, sFatherMiddleName, sMotherLastName, sMotherFirstName, sMotherMiddleName, sMotherEmail, sPreviousSchoolName, sSource;

            foreach (DataRow row in dtStudentEnquiry.Rows)
            {   
                sStandardName = row["Standard_Name"].ToString();
                sEnquiryNumber = Convert.ToString(row["Enquiry_No"]);
                sStudentNames = row["StudentName"].ToString();

                sLastName = Convert.ToString(row["LastName"]);  
                sFirstName = Convert.ToString(row["FirstName"]); 
                sMiddleName = Convert.ToString(row["MiddleName"]); 
                
                sGender = Convert.ToString(row["Gender"]);
                sDateofBirth = Convert.ToString(row["DOB"].ToDateTime().ToString(Constants.S_DATE_FORMAT));

                sFatherLastName = Convert.ToString(row["Father_Last_Name"]);
                sFatherFirstName = Convert.ToString(row["Father_Fst_Name"]);
                sFatherMiddleName = Convert.ToString(row["Father_Middle_Name"]);
                sFatherQualification = Convert.ToString(row["FatherQualification"]);
                sWhatsupFather = Convert.ToString(row["WhatsupFather"]);
                sMobileNumber = Convert.ToString(row["MobileNumber"]);
                sFatherEmailAddress = Convert.ToString(row["FatherEmail"]);

                sMotherLastName = Convert.ToString(row["Mother_Last_Name"]);
                sMotherFirstName = Convert.ToString(row["Mother_Fst_Name"]);
                sMotherMiddleName = Convert.ToString(row["Mother_Middle_Name"]);
                sMotherQualification = Convert.ToString(row["MotherQualification"]);
                sWhatsupMother = Convert.ToString(row["WhatsupMother"]);
                sMotherMobileNo = Convert.ToString(row["MotherMobileNo"]);
                sMotherEmail = Convert.ToString(row["MotherEmailAddress"]);

                sAddress = Convert.ToString(row["Address"]);
                sLandmark = Convert.ToString(row["Landmark"]);
                sPreviousStandard = Convert.ToString(row["PreviousStandard"]);
                sPreviousSchoolName = Convert.ToString(row["Current_School_Name"]);

                sSource = Convert.ToString(row["Source"]);
                sStatus = Convert.ToString(row["Status"]);
                sdate = Convert.ToString(row["date"].ToDateTime().ToString(Constants.S_DATE_FORMAT));  //////

                dtStudentenquiryReport.Rows.Add(
                    sStandardName,
                    sEnquiryNumber,
                    sStudentNames,
                    sLastName,
                    sFirstName,
                    sMiddleName,
                    sGender,
                    sDateofBirth,
                    sFatherLastName,
                    sFatherFirstName,
                    sFatherMiddleName,
                    sFatherQualification,
                    sWhatsupFather,
                    sMobileNumber,
                    sFatherEmailAddress,
                    sMotherLastName,
                    sMotherFirstName,
                    sMotherMiddleName,
                    sMotherQualification,
                    sWhatsupMother,
                    sMotherMobileNo,
                    sMotherEmail,
                    sAddress,
                    sLandmark,
                    sPreviousStandard,
                    sPreviousSchoolName,
                    sSource,
                    sStatus
                    , sdate
                    );

                //dtStudentenquiryReport.Rows.Add(sStandardName, sEnquiryNumber, sStudentNames, sMobileNumber,sMotherMobileNo, sStatus, sAddress, sArea, sWhatsupFather, sWhatsupMother, sFatherQualification, sMotherQualification, sPreviousStandard,sFirstName,sMiddleName,sLastName,sDateofBirth,sFatherEmailAddress,sLandmark);
                  
            }
            ExportToExcel("StudentEnquiry.XLS", dtStudentenquiryReport);
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This empty event is used to fill up student admission list view on change of status combo box and manage ajax call.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbStatus.SelectedIndex == 4)
        {
            enquiryFilter.Visible = true;  ////
            AdmissionDateTR.Visible = false;
            FillEnquiryListView();            
            btnAdd.Visible = false;
            btnExportEnq.Visible = true;
            btnExport.Visible = false;
            DtPgCount.Visible = false;
            btnConfirm.Enabled = false;
			txtEnquiryEndDate.Text = string.Empty;
            txtEnquiryStartDate.Text = string.Empty;
            trSubmissionStatus.Visible = false;
            
            if (lstviewEnquiryDetails.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstviewEnquiryDetails, DataPager1);
                DataPager1.Visible = true;
            }
            else
                DataPager1.Visible = false;

            lstvwStudentDetails.Visible = false;
            lstviewEnquiryDetails.Visible = true;
            
            // Ensure thEnquiryForm visibility is set after ListView is visible and data-bound
            HideEnquiryFormColumn();
        }
        else
        {
            enquiryFilter.Visible = false;  ////
            AdmissionDateTR.Visible = true;
            lstvwStudentDetails.Visible = true;
            lstviewEnquiryDetails.Visible = false;
            DataPager1.Visible = false;
            DtPgCount.Visible = true;
            btnExportEnq.Visible = false;
            txtAdmissionStartDate.Text = string.Empty;
            txtAdmissionEndDate.Text = string.Empty;
            trSubmissionStatus.Visible = true;
            if (miSchoolId == Constants.SchoolId.SNS.ToInt() || miSchoolId == Constants.SchoolId.PIONEER.ToInt())
                btnAdd.Visible = true;
            else
                btnAdd.Visible = (!SchoolBase.Settings.IsEnableEnquiry);
           }
    }

    protected void cmbAdmissionFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbStatus.SelectedIndex == 4)
            {
                FillEnquiryListView();
                btnAdd.Visible = false;
                btnExportEnq.Visible = true;
                btnExport.Visible = false;
                DtPgCount.Visible = false;
                btnConfirm.Enabled = false;
               if (lstviewEnquiryDetails.Items.Count > 0)
                {
                    ControlUtility.FillListViewPagerFooter(lstviewEnquiryDetails, DataPager1);
                    DataPager1.Visible = true;                    
                    btnConfirm.Enabled = true;                    
                }
                lstvwStudentDetails.Visible = false;
                lstviewEnquiryDetails.Visible = true;
            }
            else
                btnConfirm.Enabled = false;
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstviewEnquiryDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                int iEnqId = Convert.ToInt32(lstviewEnquiryDetails.DataKeys[iRowId]["Id"]);
                int iStatusId = Convert.ToInt32(lstviewEnquiryDetails.DataKeys[iRowId]["StatusId"]);
                bool bIsConfirmed = Convert.ToBoolean(lstviewEnquiryDetails.DataKeys[iRowId]["IsConfirmed"]);
                string sQuerystringForFrom = "EnquiryId=" + iEnqId + "&IsEnquiry=1";
                HyperLink olnkbtnForm = e.Item.FindControl("lnkbtnAdmsn") as HyperLink;
                HyperLink olnkbtnEnquiryForm = e.Item.FindControl("lnkbtnEnquiryForm") as HyperLink;
             
                LinkButton lnkbtnPay = e.Item.FindControl("lnkbtnPay") as LinkButton;
                CheckBox chkIsConfirm = e.Item.FindControl("chkIsConfirm") as CheckBox;
                Image imgConfirm = e.Item.FindControl("imgConfirm") as Image;
                Label lblDash = e.Item.FindControl("lblDash") as Label;
                ImageButton imgBtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                HyperLink lnkEditEnquiryDetails = e.Item.FindControl("lnkEditEnquiryDetails") as HyperLink;
                string Mobile = Convert.ToString(lstviewEnquiryDetails.DataKeys[iRowId]["MobileNumber"]);
               
                chkIsConfirm.Visible = false;
                imgConfirm.Visible = false;
                lblDash.Visible = false;

                HtmlTableCell tdEnquiryForm = e.Item.FindControl("tdEnquiryForm") as HtmlTableCell;
                if (tdEnquiryForm != null)
                {
                    tdEnquiryForm.Visible = (miSchoolId == Constants.SchoolId.SNS.ToInt());
                }
                if (SchoolBase.Settings.IsAaryanSchool)
                {
                    HtmlTableCell tdSelect = e.Item.FindControl("tdSelect") as HtmlTableCell;
                    if (tdSelect != null)
                        tdSelect.Visible = false;
                }
                if (miSchoolId != Constants.SchoolId.SPS.ToInt() && miSchoolId != Constants.SchoolId.SVP.ToInt() && miSchoolId != Constants.SchoolId.SVNP.ToInt() && miSchoolId != Constants.SchoolId.SNS.ToInt())
                {
                    olnkbtnForm.Visible = true;
                    lnkbtnPay.Visible = false;
                    olnkbtnForm.NavigateUrl = string.Format("AdmissionFormStudentDetails.aspx?" + CommonUtility.EncryptQuerystring(sQuerystringForFrom));
                }
                else if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                {
                    olnkbtnForm.Visible = true;
                    lnkbtnPay.Visible = false;
                    string sStandardName = Convert.ToString(lstviewEnquiryDetails.DataKeys[iRowId]["Standard_Name"]);
                    string sQueryString = "EnquiryId=" + iEnqId + "&AcademicYearId=" + ddlAcademicYEar.SelectedValue + "&StatusId=" + cmbStatus.SelectedValue 
                        + "&StandardName=" + sStandardName + "&IsEnquiry=0";
                   
                    olnkbtnForm.NavigateUrl = string.Format("StudentRegistrationDetails.aspx?" + CommonUtility.EncryptQuerystring(sQueryString));

                   olnkbtnEnquiryForm.Visible = true;
                   string sQuerystringForEnquiryFrom = "iEnquiryId=" + iEnqId + "&AdmissionId=0"+ "&IsTeachersCopy=1";
                   olnkbtnEnquiryForm.NavigateUrl =  string.Format("javascript:openNewReport('{0}');", olnkbtnEnquiryForm.NavigateUrl + "?" + CommonUtility.EncryptQuerystring(sQuerystringForEnquiryFrom));
                }
                else
                {
                   // tdEnquiryForm.Visible = false;
                    olnkbtnForm.Visible = false;
                    lnkbtnPay.Visible = true;
                    olnkbtnEnquiryForm.Visible = false;
                    if (iStatusId == 4)
                    {
                        lnkbtnPay.Text = "Pay";
                        lblDash.Visible = true;
                    }
                    else if (iStatusId == 3)
                    {
                        chkIsConfirm.Visible = true;
                        chkIsConfirm.Enabled = false;

                        if (ddlStandard.SelectedValue != Constants.S_ZERO)
                            chkIsConfirm.Enabled = true;

                        if (bIsConfirmed)
                        {
                            chkIsConfirm.Visible = false;
                            imgConfirm.Visible = true;
                            imgBtnDelete.Enabled = false;
                            lnkEditEnquiryDetails.Enabled = false;
                        }
                        lnkbtnPay.Text = "Receipt";
                        HiddenField hidQueryString = e.Item.FindControl("hidQueryString") as HiddenField;
                        string sQueryString = string.Empty;
                        sQueryString = "EnquiryId=" + iEnqId + "&IsFromEnquiryList=1";
                        hidQueryString.Value = CommonUtility.EncryptQuerystring(sQueryString);

                        lnkbtnPay.Attributes.Add("onclick", "OpenReceiptPopup(" + oCurrentItem.DisplayIndex + "); return false;");
                    }
                }
                
                if (oCurrentItem != null)
                    SetVisibilityOfColumns(oCurrentItem);
                     OpenEnquiryDetails(oCurrentItem);
                //btnExportEnq.Visible = true;
                   
                  string sStatusQueryString = CommonUtility.EncryptQuerystring("EnquiryId=" + iEnqId + "&MobileNumber=" + Mobile + "&NextAcademiYearId=" + ddlAcademicYEar.SelectedValue);
                  HiddenField hidQueryString1 = oCurrentItem.FindControl("hidQueryString") as HiddenField;
                  hidQueryString1.Value = sStatusQueryString;
                  LinkButton lnkStatus = oCurrentItem.FindControl("lnkStatuss") as LinkButton;
                  lnkStatus.Attributes.Add("onclick", "OpenEnquiryStatusPopup(" + oCurrentItem.DisplayIndex + "); return false;"); 
            }
        }        
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Listview Events "

    #region " Private Methods "

    /// <summary>
    /// This method is used to get new academic year id.
    /// </summary>
    private void GetNewAcadamicYearID()
    {
        // Table Indices
        //const int S_TBL_NEW_ACADAMIC_YEAR = 0;
        //SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
        //DataSet oDSNextAcdemic = oSchoolWiseAcademicYearMasterBL.GetNextConfiguredAcademicYear(miSchoolId, S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR);
        //if (oDSNextAcdemic != null && oDSNextAcdemic.Tables[S_TBL_NEW_ACADAMIC_YEAR].Rows.Count > 0)
        //{
        //    if (oDSNextAcdemic.Tables[S_TBL_NEW_ACADAMIC_YEAR].Rows[0]["Academic_Year_Id"] != DBNull.Value)
        //        hidNextAcademiYearId.Value = oDSNextAcdemic.Tables[S_TBL_NEW_ACADAMIC_YEAR].Rows[0]["Academic_Year_Id"].ToString();
        //    else
        //        hidNextAcademiYearId.Value = "0";
        //}
        //else
        //    hidNextAcademiYearId.Value = "0";

        hidNextAcademiYearId.Value = ddlAcademicYEar.SelectedValue;
    }

    /// <summary>
    /// This method is used to fill all combo on page.
    /// </summary>
    private void FillAllPageControls()
    {   
        FillStandardCombo();
        FillStatusCombo();
        FillAdmissionTypeCombo();
        FillAdmissionForCombo();
        FillSchoolLocations();
    }

    /// <summary>
    /// This method is used to fill school locations.
    /// </summary>
    private void FillSchoolLocations()
    {
        if (moSchool == Constants.SchoolId.DPIS)
        {
            trLocation.Visible = true;
            SchoolEnquiryBL oSchoolEnquiryBL = new SchoolEnquiryBL();
            DataTable dtLocation = oSchoolEnquiryBL.GetSchoolLocations();
            cmbSchoolLocation.Bind(dtLocation, "Id", "Name", Constants.S_ALL);
        }
        else
        {
            trLocation.Visible = false;
            cmbSchoolLocation.Items.Add(new ListItem { Text = Constants.S_ALL, Value = Constants.S_ZERO });
        }
    }

    /// <summary>
    /// This method is used to fill up status combo box.
    /// </summary>
    private void FillStatusCombo()
    {
        StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL();
        List<AdmissionStatus> lstStatuses = oStudentAdmissionsBL.GetAllAdmissionStatuses();
        ListSource.FillDropDownList(lstStatuses, cmbStatus, "Name", "Id", Constants.S_ALL + " (Registration)");
    }

    private void FillAdmissionForCombo()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, ddlAcademicYEar.SelectedValue.ToInt());
        DataTable dtAdmissionForm = oStandardCollectionBL.GetAdmissionForCategories();
        ControlUtility.FillDropDownList(dtAdmissionForm, ref cmbAdmissionFor, "Id", "AdmissionFor", string.Empty);
        cmbAdmissionFor.SelectedValue = Constants.S_ONE;
    }

    /// <summary>
    /// This method is used to fill admission types in combo.
    /// </summary>
    private void FillAdmissionTypeCombo()
    {
        const string S_DDL_VALUE_ONLINE = "Online Admission";
        const string S_DDL_VALUE_MANUAL = "Manual Admission";

        ddlAdmissionType.AppendDataBoundItems = true;
        ddlAdmissionType.Items.Clear();
        ddlAdmissionType.Items.Add(new ListItem(Constants.S_SELECT_ALL, "0"));
        ddlAdmissionType.Items.Add(new ListItem(S_DDL_VALUE_ONLINE, "1"));
        ddlAdmissionType.Items.Add(new ListItem(S_DDL_VALUE_MANUAL, "2"));
    }

    /// <summary>
    /// This method is used to fill all combo with all standard of school.
    /// </summary>
    private void FillStandardCombo()
    {
        int iAcademicYearID = Convert.ToInt32(hidNextAcademiYearId.Value);
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, iAcademicYearID);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDtStandardCollection, ref ddlStandard, Constants.S_STANDARD_ID_FIELD, Constants.S_STANDARD_NAME_FIELD, Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to fill the academic year combobox.
    /// </summary>
    private void FillAcademicYearCombo()
    {
        StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL();
        DataTable dtAcademic = oStudentAdmissionsBL.GetAcademicYearsForNewAdmission(miSchoolId);
        ControlUtility.FillDropDownList(dtAcademic, ref ddlAcademicYEar, "AcademicYearId", "AcademicYear", Constants.S_SELECT);
        ViewState["AcademicYearDetails"] = dtAcademic;

        DataRow[] drArr = dtAcademic.Select("IsCurrentYear=1 AND AcademicYearId=" + miAcademicYearId + " AND SettingKeyStatus=1");
        if (drArr.Length > 0)
            ddlAcademicYEar.SelectedValue = drArr[0]["AcademicYearId"].ToString();
        else
        {
            DataRow[] drArr1 = dtAcademic.Select("IsCurrentYear=0");
            if(drArr1.Length > 0)
                ddlAcademicYEar.SelectedValue = drArr1[0]["AcademicYearId"].ToString();
        }
    }


    private void FillStandardPopupCombo(int aiStandardId)
    {
        int iAcademicYearID = Convert.ToInt32(hidNextAcademiYearId.Value);
        DivisionCollectionBL oDivisionMasterBL = new DivisionCollectionBL(miSchoolId, iAcademicYearID);
        
        int aiAdmissionTypeId = Constants.I_ZERO;
        if (miSchoolId == Constants.SchoolId.SPS.ToInt() || miSchoolId == Constants.SchoolId.SVP.ToInt() || miSchoolId == Constants.SchoolId.SVNP.ToInt())
            aiAdmissionTypeId = cmbAdmissionFor.SelectedValue.ToInt();

        DataTable oDtDivisionCollection = oDivisionMasterBL.GetAllDivisionsForStandardForAdmissionConfirmation(aiStandardId, aiAdmissionTypeId);
        ControlUtility.FillDropDownList(oDtDivisionCollection, ref cmbStandardNamePopup, Constants.S_DIVISION_ID_FIELD, Constants.S_DIVISION_NAME_FIELD, string.Empty);
    }


    /// <summary>
    /// This method is used to set default properties of controls. 
    /// </summary>
    private void SetDefaultProperties()
    {
        ApplyMouseHoverEffect(new List<Button> { btnShow, btnAdd, btnClear, btnConfirm, btnSave, btnClose });

        const string S_DEFAULT_SORT_EXP = "Form_Number";
        hidSortDirection.Value = Constants.S_DESCENDING;
        HidenqSortDirection.Value = Constants.S_DESCENDING;
        hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] = null;
        btnClear.Attributes.Add("onclick", "return ClearContriols();");  ///////
        HtmlForm form1 = this.Master.FindControl("form1") as HtmlForm;
        form1.DefaultButton = btnShow.UniqueID;
        btnConfirm.Enabled = ddlStandard.SelectedIndex != 0;
    }

    /// <summary>
    /// This method is used set sorting direction.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used set sorting direction.
    /// </summary>
    private void SetEnqSortVariables()
    {
        if (HidenqSortDirection.Value == Constants.S_DESCENDING)
            HidenqSortDirection.Value = Constants.S_ASCENDING;
        else
            HidenqSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set sorting image in list view column header.
    /// </summary>
    private void SetSortImage(string asSortExpression)
    {
        //if (lstvwStudentDetails.SortDirection.ToString() == "Ascending" || lstvwStudentDetails.SortDirection.ToString() == "")
        //    hidSortDirection.Value = Constants.S_ASCENDING;
        //else
        //    hidSortDirection.Value = Constants.S_DESCENDING;
              
        if (lstvwStudentDetails.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwStudentDetails.SortExpression.ToString();
        else
            hidSortExpression.Value = asSortExpression;
        HtmlTableRow oHtmlTableHeaderRow = lstvwStudentDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to set sorting image in list view column header.
    /// </summary>
    private void SetSortEnqImage(string asSortExpression)
    {
        //if (lstviewEnquiryDetails.SortDirection.ToString() == "Ascending" || lstvwStudentDetails.SortDirection.ToString() == "")
        //    HidenqSortDirection.Value = Constants.S_ASCENDING;
        //else
        //    HidenqSortDirection.Value = Constants.S_DESCENDING;
        if (lstviewEnquiryDetails.SortExpression != string.Empty)
            HidEnqSortExprsn.Value = lstviewEnquiryDetails.SortExpression.ToString();
        else
            HidEnqSortExprsn.Value = asSortExpression;

        HtmlTableRow oHtmlTableHeaderRow = lstviewEnquiryDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, HidEnqSortExprsn.Value, HidenqSortDirection.Value);
    }

    /// <summary>
    /// This method is used to check weather studentui is configured or not. If student is confirm then set is_configured flag to 'Y' for studentui.
    /// </summary>
    private void GetScreenConfigDetails()
    {
        int iScreenLevel = Convert.ToInt32(Constants.ScreenLevel.Configuration);
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable oDSUserDetails = oMasterDataCollectionBL.GetConfigurationDetails(miSchoolId, hidNextAcademiYearId.Value.ToInt(), miFinancialYearId, Constants.SchoolConfigMenuId.Other_User_Related.ToInt(), iScreenLevel, miUserId, moUserRole.ToInt());
        if (oDSUserDetails.Rows.Count > 0 && oDSUserDetails.Rows[0]["Is_Configure"] != null)
            hidIsConfigured.Value = oDSUserDetails.Rows[0]["Is_Configure"].ToString();
    }

    /// <summary>
    /// This method is used to fill student list.
    /// </summary>
    private void FillListView()
    {
        DataPager pager = lstvwStudentDetails.FindControl("DtPgDropDown") as DataPager;
        if (pager != null)
            pager.SetPageProperties(0, pager.PageSize, true);
        DtPgCount.Visible = true;
        lstvwStudentDetails.Visible = true;
        lstvwStudentDetails.DataSourceID = lstvwObjDS.ID;
        lstvwStudentDetails.DataBind();
    }

    /// <summary>
    /// This method is used to fill student list.
    /// </summary>
    private void FillEnquiryListView()
    {
        if (miSchoolId == Constants.SchoolId.SPS.ToInt() || miSchoolId == Constants.SchoolId.SVP.ToInt() || miSchoolId == Constants.SchoolId.SVNP.ToInt())
        {
            hidAdmissionFor.Value = cmbAdmissionFor.SelectedValue;
            lstvwStudentDetails.Visible = false;
        }
        else
            hidAdmissionFor.Value = Constants.S_ZERO;

        DataPager pager = lstviewEnquiryDetails.FindControl("DtPgDropDown") as DataPager;
        if (pager != null)
            pager.SetPageProperties(0, pager.PageSize, true);
        DtPgCount.Visible = false;
        lstviewEnquiryDetails.DataSourceID = ObjectDataSource1.ID;
        lblEnqError.Visible = false;
    }

    /// <summary>
    /// This method is used to save student details.
    /// </summary>
    private void SaveStudentDetails(ListView aoListView)
    {
        if (CheckPreConditionForStandard())
        {
            StudentCollectionBL oStudentCollectionBL = new StudentCollectionBL();
            int iAcademicYearId = Convert.ToInt32(hidNextAcademiYearId.Value);
            int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
            int iDivisionId = Convert.ToInt32(cmbStandardNamePopup.SelectedValue);
            int iUserRoleId = Convert.ToInt32(Constants.UserRoles.Student);
            string sStudentDetails = GenerateXml(iUserRoleId, aoListView);
            DataTable oDataTable = oStudentCollectionBL.InsertMultipleStudents(miSchoolId, iAcademicYearId, miUserId, iStandardId, iDivisionId, sStudentDetails, iUserRoleId, S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR,  Settings.AutoCalculateEnrolmentNo);

            if (miSchoolId == Constants.SchoolId.SVNP.ToInt())
                oDataTable = GetUserEnrolmentNumber(oDataTable);
           
            SendSMS(oDataTable);
            RefreshStudentCache(oDataTable);
            FillListView();
            if (hidIsConfigured.Value == Constants.S_NO)
            {
                this.SaveConfigurationDetails(Constants.SchoolConfigurations.Student.ToInt());
                hidIsConfigured.Value = Constants.S_YES;
            }
        }
    }

    /// This Method is used to Update Students User Name & Password For SVNP school only.
    /// </summary>
    /// <param name="oDataTable"></param>
    /// <returns></returns>
    private DataTable GetUserEnrolmentNumber(DataTable oDataTable)
    {
        SchoolUserBL oSchoolUserBL = new SchoolUserBL();
        string sEnrolmentNo = string.Empty;
        int iUserId = Constants.I_ZERO;
        string sPassword = string.Empty;
        string UpdatedPass = string.Empty;
        string sUserLogin = string.Empty;
        List<UserLoginDetails> lstUserLoginDetails = new List<UserLoginDetails>();

        for (int iRowCount = 0; iRowCount <= oDataTable.Rows.Count - 1; iRowCount++)
        {
            UserLoginDetails oUserLoginDetails = new UserLoginDetails();

            sEnrolmentNo = oDataTable.Rows[iRowCount]["Enrolment_Number"].ToString();
            iUserId = oDataTable.Rows[iRowCount]["UserId"].ToInt();
            sUserLogin = oDataTable.Rows[iRowCount]["UserLogin"].ToString();
            sPassword = CommonUtility.GetDecryptedPassword(sUserLogin, oDataTable.Rows[iRowCount]["UserPassword"].ToString());
            UpdatedPass = Utility.CommonUtility.GetEncryptedPassword(sEnrolmentNo, sPassword);
            oDataTable.Rows[iRowCount]["UserLogin"] = sEnrolmentNo;
            oDataTable.Rows[iRowCount]["UserPassword"] = UpdatedPass;

            oUserLoginDetails.UserId = iUserId;
            oUserLoginDetails.UserLogin = sEnrolmentNo;
            oUserLoginDetails.Password = UpdatedPass;

            lstUserLoginDetails.Add(oUserLoginDetails);
        }

        oSchoolUserBL.UpdateStudentLoginDetails(lstUserLoginDetails);

        return oDataTable;
    }

    /// <summary>
    /// This method is used to send SMS.
    /// </summary>
    /// <param name="oDataTable"></param>
    private void SendSMS(DataTable oDataTable)
    {
        string sAdmissionConfirmSMS = string.Empty; 
		string sTemplateRegistrationId = string.Empty;
        string sSmsSubject = string.Empty;
		
        if (oDataTable != null && oDataTable.Rows.Count > 0 && oDataTable.Rows[0][0] != DBNull.Value)
        {
            int iRowCount = oDataTable.Rows.Count;
            int iSMSType = 0;
            int iSmsId = Constants.I_ZERO;
            
            if(rdoFinal.Checked)
                iSmsId = Convert.ToInt32(Constants.SMSTemplate.AdmissionConfirmationSMS);
            else if(rdoProvisional.Checked)
                iSmsId = Convert.ToInt32(Constants.SMSTemplate.AdmissionProvisionalConfirmationSMS);

            DataTable oDTTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
            if (oDTTemplate.Rows.Count != 0)
            {
                if (oDTTemplate.Rows[0][2] != DBNull.Value)
                {
                    sAdmissionConfirmSMS = Convert.ToString(oDTTemplate.Rows[0][2]);
					
					if (oDTTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                        sTemplateRegistrationId = oDTTemplate.Rows[0]["TemplateRegistrationId"].ToString();
					
                    sSmsSubject = Convert.ToString(oDTTemplate.Rows[0][1]);
                }

                if (oDTTemplate.Rows[0][3] != DBNull.Value)
                    iSMSType = oDTTemplate.Rows[0][3].ToInt();
            }

            SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
            string sSMSSenderName = oSchoolBL.SMSSenderName;
            foreach (DataRow oDR in oDataTable.Rows)
            {
                int iUserId = Convert.ToInt32(oDR["UserId"]);
                string sMobileNo = Convert.ToString(oDR["MobileNo"]);
                string sDisplayText = Convert.ToString(oDR["DisplayText"]);
                SMS oSMS = new SMS();
                oSMS.Sender = sSMSSenderName;
                oSMS.SMSText = sAdmissionConfirmSMS;
				oSMS.TemplateRegistrationId = sTemplateRegistrationId;
                oSMS.School_Name = oSchoolBL.SchoolName + "::" + sSmsSubject;
                oSMS.DisplayText = sDisplayText;
                oSMS.SMSType = iSMSType;
                oSMS.SchoolID = miSchoolId;
                oSMS.AcademicYearID = Convert.ToInt32(hidNextAcademiYearId.Value);
                oSMS.To.Add(iUserId, sMobileNo);
                oSMS.Send();
            }
        }
    }

    /// <summary>
    /// This Method is USed to Send Registartion related SMS to parent.
    /// </summary>
    /// <param name="sRegistrationNo"></param>
    /// <param name="sDisplayText"></param>
    private void SendRegistrationSMS(string sRegistrationNo, string sDisplayText)
    {
        int iSMSCount = moManualMobileNo.Count;
        SMS oSMS = new SMS();
        oSMS.Sender = "SPSSCH";
        oSMS.SMSCount = iSMSCount;
        oSMS.DisplayText = sDisplayText;
        oSMS.SMSText = "Dear Parent,\nPlease visit on sanjeevanpublicschool.org for your ward admissions. \nKindly refer your registration number for continuing admission -\n" + sRegistrationNo;
        oSMS.ToManualNumbers = moManualMobileNo;
        oSMS.IsScheduled = false;
        oSMS.ScheduledDate = DateTime.Now;
        oSMS.IsUnicodeSMS = false;
        oSMS.Send();
    }

    /// <summary>
    /// This method is used to generate xml.
    /// </summary>
    /// <returns></returns>
    private string GenerateXml(int aiUserRoleId, ListView aoListView)
    {
        Random oRandomNo = new Random((int)DateTime.Now.Ticks);
        int iItemCount = aoListView.Items.Count;
        int iLoginId = StudentBL.GetNextLoginId(miSchoolId, aiUserRoleId);
        iLoginId++;
        const string S_ELEMENT = "element";
        string sAttribute;
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StudentDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentDetails", "");

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount < iItemCount; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)aoListView.Items[iRowCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;

            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentDetails", "");

            CheckBox chkSelect = (CheckBox)oCurrentItem.FindControl("chkIsConfirm");

            if (chkSelect.Checked)
            {
                string sFormNumber = string.Empty;
                if (miSchoolId != Constants.SchoolId.SPS.ToInt() && miSchoolId != Constants.SchoolId.SVP.ToInt() && miSchoolId != Constants.SchoolId.SVNP.ToInt())
                    sFormNumber = aoListView.DataKeys[iRowCount]["Form_Number"].ToString();
                else
                    sFormNumber = aoListView.DataKeys[iRowCount]["Enquiry_No"].ToString();

                sAttribute = "Form_Number";
                XmlAttribute attr = oDoc.CreateAttribute(sAttribute);
                attr.Value = sFormNumber;
                oXmlNode.Attributes.Append(attr);

                sAttribute = "User_Login";
                attr = oDoc.CreateAttribute(sAttribute);
                attr.Value = iLoginId.ToString();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "User_Password";
                attr = oDoc.CreateAttribute(sAttribute);
                string sPassword = Utility.CommonUtility.GetEncryptedPassword(iLoginId.ToString(), oRandomNo.Next(100000, 999999).ToString());
                attr.Value = sPassword;
                oXmlNode.Attributes.Append(attr);

                iLoginId++;

                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
        }

        // Add the root node to document element.
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to check pre-condition to configure association.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreConditionForStandard()
    {
        bool bReturn = false;
        string sLinks = null;
        int iAcademicYearId = Convert.ToInt32(hidNextAcademiYearId.Value);
        int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
        DataTable oDataTable = StudentAdmissionsBL.GetPreConditionMsg(miSchoolId, iAcademicYearId, iStandardId);
        sLinks = FormatData(oDataTable);
        if (!sLinks.Equals(string.Empty))
        {
            trPrecondition.Visible = true;
            divErr.InnerHtml = sLinks;
            divErr.Visible = true;
        }
        else
        {
            divErr.Visible = false;
            trPrecondition.Visible = false;
            bReturn = true;
        }

        return bReturn;
    }

    /// <summary>
    /// This method is used to format data.
    /// </summary>
    /// <param name="aoDataTable"></param>
    /// <returns></returns>
    private string FormatData(DataTable aoDataTable)
    {
        string sReturn = string.Empty;
        char cIsCurrentAcademicYear = 'N';
        if (miAcademicYearId.ToString() == hidNextAcademiYearId.Value)
            cIsCurrentAcademicYear = 'Y';
        string sHeaderMessage = "Please configure following details in mid year.";
        int iRowCount = aoDataTable.Rows.Count;
        if (cIsCurrentAcademicYear == 'Y')
            sHeaderMessage = "Please configure following details.";
        if (iRowCount > 0)
        {
            sReturn = "<table class=\"LblNoRecord\"><tr><td class=\"ClsConfigText\">" + sHeaderMessage + "</td></tr>";
            for (int i = 0; i < aoDataTable.Rows.Count; i++)
            {
                if (cIsCurrentAcademicYear == 'Y')
                    sReturn = sReturn + "<tr><td><a class=\"ClsConfigLink\" href=" + aoDataTable.Rows[i]["NavigateURL"].ToString() + ">" + aoDataTable.Rows[i]["Configure_Name"] + "</a></td></tr>";
                else
                    sReturn = sReturn + "<tr><td><a class=\"ClsConfigLink\" href='' Enabled='false' onclick ='return false;'>" + aoDataTable.Rows[i]["Configure_Name"] + "</a></td></tr>";
            }

            sReturn = sReturn + "</table>";
        }

        return sReturn;
    }

    /// <summary>
    /// This method is used to 
    /// </summary>
    /// <param name="aiOriginalConfigId"></param>
    public void SaveConfigurationDetails(int aiOriginalConfigId)
    {
        ConfigurationSchoolMasterBL oConfigurationSchoolMasterBL = PopulateSchoolDeatails(aiOriginalConfigId);
        oConfigurationSchoolMasterBL.InsertConfigurationSchoolMaster();
    }

    /// <summary>
    ///		This method is used to initailze configuration details.
    /// </summary>
    /// <param name="aiOriginalConfigId"></param>
    private ConfigurationSchoolMasterBL PopulateSchoolDeatails(int aiOriginalConfigId)
    {
        return new ConfigurationSchoolMasterBL
        {
            OriginalConfigId = aiOriginalConfigId,
            SchoolId = miSchoolId,
            AcademicYearId = hidNextAcademiYearId.Value.ToInt(),
            IsConfigure = Constants.C_YES,
            InsertedById = miUserId,
            UpdateById = miUserId,
            FinancialYearId = miFinancialYearId
        };
    }

    /// <summary>
    /// This method is used to refresh student cache.
    /// </summary>
    private void RefreshStudentCache(DataTable aoDataTable)
    {
        var oDatarows = from dr in aoDataTable.AsEnumerable()
                        select Convert.ToInt32(dr["StudentId"]);
        List<int> lstStudentIds = new List<int>();
        if (oDatarows.Any())
            lstStudentIds = oDatarows.ToList();

        AutoSearchService oAutoSearchService = new AutoSearchService();
        oAutoSearchService.RefreshStudentCache(miSchoolId, miAcademicYearId, lstStudentIds, Constants.Action.Insert);
    }

    /// <summary>
    /// This method is used to get error message.
    /// </summary>
    private string GetErrorMessage()
    {
        String currurl = HttpContext.Current.Request.RawUrl;
        string errormessage = null;
        int index = currurl.IndexOf('=');
        //if (index >= 0)
        //{
        //    errormessage = "Admission is not open for that Selected Standard.";
        //}
        //else
            errormessage = string.Empty;

        return errormessage;
     }

    /// <summary>
    /// This method is used to hide or show filename column in listview.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    private void SetVisibilityOfColumns(ListViewDataItem oCurrentItem)
    {
        ImageButton imgBtnDelete = oCurrentItem.FindControl("imgBtnDelete") as ImageButton;
        imgBtnDelete.Attributes.Add("Onclick", "if(!ConfirmDelete()) {return false;}");
    }

    /// <summary>
    /// this method is use to navigate to the enquiry form screen.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    private void OpenEnquiryDetails(ListViewDataItem oCurrentItem)
    {
        int iRowId = oCurrentItem.DisplayIndex;

        int iId = lstviewEnquiryDetails.DataKeys[iRowId]["Id"].ToInt();

        string sQueryString = string.Format("Id={0}&AcademicYearId={1}&StatusId=4", iId,ddlAcademicYEar.SelectedValue);
        string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);

        HyperLink lnkEditEnquiryDetails = oCurrentItem.FindControl("lnkEditEnquiryDetails") as HyperLink;
           if (miSchoolId == Constants.SchoolId.SNS.ToInt())
          {
              string sStandardName = Convert.ToString(lstviewEnquiryDetails.DataKeys[iRowId]["Standard_Name"]);

              string sRegistQueryString = string.Format("EnquiryId={0}&AcademicYearId={1}&StatusId=4&StandardName={2}&IsEnquiry=1",  iId, ddlAcademicYEar.SelectedValue, sStandardName);
              lnkEditEnquiryDetails.NavigateUrl = "StudentRegistrationDetails.aspx?" + CommonUtility.EncryptQuerystring(sRegistQueryString);
          }
        else
         {
            
            lnkEditEnquiryDetails.NavigateUrl = string.Format("EnquiryForm.aspx?{0}", sEncrypt);
         }
    }

    /// <summary>
    /// This method is used for Adding the row Header.
    /// </summary>
    private void AddHeader(string asText, string asStyle = "")
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle = "style='" + asStyle + "'";
        HttpContext.Current.Response.Write("<Td colspan='" + "' " + sStyle + ">");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(asText);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
    }

    /// <summary>
    /// 	This method is used for Adding the rows in to Table.
    /// </summary>
    private void AddTableRows(string sRowHeader, string asStyle = "")
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle = "style='" + asStyle + "'";
        HttpContext.Current.Response.Write("<TD " + sStyle + ">");
        HttpContext.Current.Response.Write(sRowHeader.ToString());
        HttpContext.Current.Response.Write("</TD>");
    }

    /// <summary>
    /// This method is used to to change Header text for SPS school.
    /// </summary>
    private void ChangeListHeaderText()
    {
        HtmlTableRow tr = lstviewEnquiryDetails.FindControl("trHeader") as HtmlTableRow;
        HtmlTableRow trStudentDetails = lstvwStudentDetails.FindControl("trHeader") as HtmlTableRow;

        if(SchoolBase.Settings.IsAaryanSchool)
        {
          HtmlTableCell thIsConfirmed = tr.FindControl("thIsConfirmed") as HtmlTableCell;
          if (thIsConfirmed != null)
              thIsConfirmed.Visible = false;
        }

        if (miSchoolId == Constants.SchoolId.SPS.ToInt() || miSchoolId == Constants.SchoolId.SVP.ToInt() || miSchoolId == Constants.SchoolId.SVNP.ToInt())
        {
            if (trStudentDetails != null)
            {
                HtmlTableCell thConfirmastion = trStudentDetails.FindControl("trConfirmastion") as HtmlTableCell;

                if (thConfirmastion != null)
                    thConfirmastion.Visible = true;
            }

            if (tr != null)
            {
                HtmlTableCell thEnquiryName = tr.FindControl("thEnquiry") as HtmlTableCell;
                if (thEnquiryName != null)
                {
                    LinkButton lnkFormNo = thEnquiryName.FindControl("lnkFormNo") as LinkButton;
                    if (lnkFormNo != null)
                        lnkFormNo.Text = "Registration No";
                }
            }
        }
        else
        {
            if (trStudentDetails != null)
            {
                HtmlTableCell thConfirmastion = trStudentDetails.FindControl("thConfirmationForm") as HtmlTableCell;

                if (thConfirmastion != null)
                    thConfirmastion.Visible = false;
            }
        }
    }
    /// <summary>
    /// these method is used to hide delete column
    /// </summary>
    private void HideDeleteColumn()
    {
        HtmlTableCell thDelete = lstvwStudentDetails.FindControl("thDelete") as HtmlTableCell;
        if (thDelete != null )
        {
            if (SchoolBase.Settings.EnableDeleteButtonforStudentRegistration)
                thDelete.Visible = true;
            else
                thDelete.Visible = false;
        }
        
    }

    /// <summary>
    /// these method is used to hide enquiry form column.
    /// </summary>
  private void HideEnquiryFormColumn()
  {
    // Find the header row first, then find thEnquiryForm within it
    HtmlTableRow trHeader = lstviewEnquiryDetails.FindControl("trHeader") as HtmlTableRow;
    if (trHeader != null)
    {
        HtmlTableCell thEnquiryForm = trHeader.FindControl("thEnquiryForm") as HtmlTableCell;
        if (thEnquiryForm != null)
        {
            thEnquiryForm.Visible = (miSchoolId == Constants.SchoolId.SNS.ToInt());
        }
     }    
  }
    /// <summary>
    /// these method is used to hide registration form column.
    /// </summary>
  private void HideRegistrationFormColumn()
   {

      HtmlTableRow trHeader = lstvwStudentDetails.FindControl("trHeader") as HtmlTableRow;
      if (trHeader != null)
      {
          HtmlTableCell thRegForm = trHeader.FindControl("thRegForm") as HtmlTableCell;
          if (thRegForm != null)
          {
              thRegForm.Visible = (miSchoolId == Constants.SchoolId.SNS.ToInt());
          }
      }
   }
 }
#endregion " Private Methods "   
    


