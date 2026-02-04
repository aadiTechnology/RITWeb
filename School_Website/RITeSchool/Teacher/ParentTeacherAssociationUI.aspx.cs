/* File Name = ParentTeacherAssociation.aspx.cs
 * Created Date - 
 * Modified Date  -24 Dec 2010
 * Created by - Sachin
 * Class Description - This class is defined to manage Parent eacher Association details.
 * Modified By:Rohini  
 * Date:14 Jan 2011
 * Decsription: Teacher can not edit the details who does not have edit access.
 */

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AssociationEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Web;

public partial class ParentTeacherAssociationUI : SchoolBase
{
    #region "Constants"

    private const int I_PAGE_SIZE = 5;
    private const string S_SAVE = "Save";
    private const string S_UPDATE = "Update";
    private const string S_PRE_PRIMARY = "Pre-Primary";
    private const string S_PRIMARY_AND_SECONDARY = "Primary and Secondary";
    private const string S_UNDERSCORE = "-";
    private const int I_ADMIN_USER_ROLE_ID = 1;
    private const int I_TEACHER_USER_ROLE_ID = 2;
    private const int I_STUDENT_USER_ROLE_ID = 3;
    private const string S_NEW_MODE = "NEW_MODE";
    private const string S_EDIT_MODE = "EDIT_MODE";
    private const int I_DESIGNATION_ID = 185; ////Designation = Representative
    private const string S_SAVE_MSG = "Staff details saved successfully !!!";
    private const string S_UPDATE_MSG = "Staff details updated successfully !!!";
    private const string S_PARENT_SAVE_MSG = "Parent details saved successfully !!!";
    private const string S_PARENT_UPDATE_MSG = "Parent details updated successfully !!!";

    #endregion

    #region Property(s)
    
    private int SchoolCommitteeId
    {
        get { return Convert.ToInt32(QueryString["SchoolCommitteeId"]); }
    } 

    #endregion

    #region "Events"

    /// <summary>
    /// This event is used to set default properties to controls on the page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {   
            if (!IsPostBack)
            {
                UpdateSitemapEntry();
                hidUserHasEditAccess.Value = CommonUtility.IsUserHasEditAccess(SchoolCommitteeId == Constants.SchoolCommittees.PTA.ToInt() ? Constants.SchoolConfigurations.ParentTeacherAssociation : Constants.SchoolConfigurations.TransportCommittee).ToString();
                FillDesignationCombobox();
                FillTeacherParentDetails();
                txtSearchByName.Focus();
                SetJavaScriptAttribute();
                CheckUserRole();
                trNorecordFoundSearch.Visible = false;
                FillSections();
                SetScreenWidth();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void FillSections()
    {
        ParentTeacherAssociationDetailsBL oParentTeacherAssociationDetailsBL = new ParentTeacherAssociationDetailsBL();
        List<SectionDetails> lstSectionDetails = oParentTeacherAssociationDetailsBL.GetSections(miSchoolId);
        ListSource.FillDropDownList(lstSectionDetails, cmbSection,"Name","Id", string.Empty);
    }

    /// <summary>   
    /// This event is used to search details of Parent or Teacher as per criteria given.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            trPager1.Visible = false;
            DtPgCount.SetPageProperties(0, I_PAGE_SIZE, false);
            FillSearchListView();
            ClearTeacherAllControls();
            ClearParentAllControls();
            EnableDisableTeacherControls(false);
            EnableDisableParentControls(false);
            btnSaveTeacherDetails.Text = S_SAVE;
            btnSaveParentDetails.Text = S_SAVE;
            if (optTeacher.Checked)
            {
                HtmlTableRow oPTableRow = lstvwSearchByCategory.FindControl("trHeader") as HtmlTableRow;
                if (oPTableRow != null)
                {
                    HtmlTableCell oTblStudentNameCell = oPTableRow.FindControl("tdStudentName") as HtmlTableCell;
                    HtmlTableCell oTblClassNameCell = oPTableRow.FindControl("tdClassName") as HtmlTableCell;
                    if (oTblStudentNameCell != null) oTblStudentNameCell.Visible = false;
                    if (oTblClassNameCell != null) oTblClassNameCell.Visible = false;
                }
            }
            else
            {
                HtmlTableRow oPTableRow = lstvwSearchByCategory.FindControl("trHeader") as HtmlTableRow;
                if (oPTableRow != null)
                {
                    HtmlTableCell oTblDesignationCell = oPTableRow.FindControl("tdDesignation") as HtmlTableCell;
                    if (oTblDesignationCell != null) oTblDesignationCell.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to edit the selected record.
    /// </summary>
    /// <param name="sender"></param>   
    /// <param name="e"></param>
    protected void lnkSelect_Click(object sender, EventArgs e)
    {
        try
        {
            if (optTeacher.Checked)
            {
                EnableDisableTeacherControls(true);
                FillTeacherDetails();
            }
            else
            {
                EnableDisableParentControls(true);
                FillParentDetails();
            }
            lblErrorMsg.Text = string.Empty;
            lblSuccessMsg.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear Search ListView and reset DataPager value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optParent_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lstvwSearchByCategory.DataSource = null;
            lstvwSearchByCategory.DataBind();
            lstvwSearchByCategory.Items.Clear();
            if (lstvwSearchByCategory.Items.Count > Constants.I_ZERO)
                FillListViewPagerFooter(lstvwSearchByCategory, DtPgCount);
            divSearch.Visible = false;
            trPager1.Visible = false;
            ClearTeacherAllControls();
            ClearParentAllControls();
            EnableDisableTeacherControls(false);
            EnableDisableParentControls(false);
            trNorecordFoundSearch.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used toclear Search ListView and reset DataPager value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optTeacher_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lstvwSearchByCategory.DataSource = null;
            lstvwSearchByCategory.DataBind();
            lstvwSearchByCategory.Items.Clear();
            if (lstvwSearchByCategory.Items.Count > Constants.I_ZERO)
                FillListViewPagerFooter(lstvwSearchByCategory, DtPgCount);
            divSearch.Visible = false;
            trPager1.Visible = false;
            ClearTeacherAllControls();
            ClearParentAllControls();
            EnableDisableTeacherControls(false);
            EnableDisableParentControls(false);
            trNorecordFoundSearch.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSearchByCategory_SelectedIndexChanged(object sender, EventArgs e)
    { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSearchByCategory_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
    { }

    /// <summary>
    /// This event is used to save Teacher details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveTeacherDetails_Click(object sender, EventArgs e)
    {
        try
        {
            SaveTeacherDetails();
            lblErrorMsg.Visible = false;
            lblSuccessMsg.Visible = true;
            lblSuccessMsg.Text = string.Empty;
            if (btnSaveTeacherDetails.Text == S_SAVE)
                lblSuccessMsg.Text = S_SAVE_MSG;
            else
                lblSuccessMsg.Text = S_UPDATE_MSG;

            FillTeacherDetails();
            if (lstvwSearchByCategory.Items.Count > Constants.I_ZERO)
                FillSearchListView();
            ClearTeacherAllControls();
            EnableDisableTeacherControls(false);
            btnSaveTeacherDetails.Text = S_SAVE;
        }
        catch (SqlException sqlEx)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = string.Empty;
            lblErrorMsg.Text = sqlEx.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save Parent details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveParentDetails_Click(object sender, EventArgs e)
    {
        try
        {
            SaveParentDetails();
            lblSuccessMsg.Visible = true;
            if (btnSaveParentDetails.Text == S_SAVE)
                lblSuccessMsg.Text = S_PARENT_SAVE_MSG;
            else
                lblSuccessMsg.Text = S_PARENT_UPDATE_MSG;
            FillParentDetails();
            if (lstvwSearchByCategory.Items.Count > Constants.I_ZERO)
                FillSearchListView();
            ClearParentAllControls();
            EnableDisableParentControls(false);
            btnSaveParentDetails.Text = S_SAVE;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view record on selected value of page .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbSearchListViewPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetDataPagerAccordingToPageNo(lstvwSearchByCategory);
            FillSearchListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view record on selected value of page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmblstvwTeacherDetailsPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetDataPagerAccordingToPageNo(lstvwTeacherDetails);
            FillTeacherDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view record on selected value of page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmblstvwParentDetailsPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetDataPagerAccordingToPageNo(lstvwParentDetails);
            FillParentDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill data rowwise in search ListView.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSearchByCategory_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                if (optTeacher.Checked)
                {
                    HtmlTableCell oTblStudNameCell = e.Item.FindControl("tdStudName") as HtmlTableCell;
                    HtmlTableCell oTblClsNameCell = e.Item.FindControl("tdClsName") as HtmlTableCell;
                    if (oTblStudNameCell != null) oTblStudNameCell.Visible = false;
                    if (oTblClsNameCell != null) oTblClsNameCell.Visible = false;
                }
                else
                {
                    HtmlTableCell oTblDesigCell = e.Item.FindControl("tdDesigName") as HtmlTableCell;
                    oTblDesigCell.Visible = false;
                }
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                HiddenField hidUId = oCurrentItem.FindControl("hidUId") as HiddenField;
                hidUId.Value = ((ParentTeacherAssociationDetails)oCurrentItem.DataItem).Id.ToString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill data rowwise in Teacher ListView.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTeacherDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;

                ////When User Role Id is Student hide "Edit" and "Delete" column from ListView.
               
                if (moUserRole != Constants.UserRoles.Admin &&
                    ((moUserRole == Constants.UserRoles.Student 
                    || (hidUserHasEditAccess.Value == Constants.S_NO))))
                {
                    HtmlTableCell oTblEditCell = e.Item.FindControl("tdTimgEdit") as HtmlTableCell;
                    HtmlTableCell oTblDeleteCell = e.Item.FindControl("tdTimgDelete") as HtmlTableCell;
                    HtmlTableCell oTblRelatedSectionCell = e.Item.FindControl("tdRelatedSection") as HtmlTableCell;
                    oTblEditCell.Visible = false;
                    oTblDeleteCell.Visible = false;
                    oTblRelatedSectionCell.Visible = false;
                }
                Label lblRelatedSection = oCurrentItem.FindControl("lblRelatedSection") as Label;
             
                //lblRelatedSection.Text = lblRelatedSection.Text == Constants.I_ONE.ToString() ? S_PRE_PRIMARY : (lblRelatedSection.Text == Constants.I_TWO.ToString() ? S_PRIMARY_AND_SECONDARY : Constants.S_ALL);


                HiddenField hidDesigId = oCurrentItem.FindControl("hidDesigId") as HiddenField;
                HiddenField hidUserId = oCurrentItem.FindControl("hidUserId") as HiddenField;
                HiddenField hidSectionId = oCurrentItem.FindControl("hidSectionId") as HiddenField;
                hidDesigId.Value = ((ParentTeacherAssociationDetails)oCurrentItem.DataItem).DesignationId.ToString();
                hidUserId.Value = ((ParentTeacherAssociationDetails)oCurrentItem.DataItem).TeacherId.ToString();
                hidSectionId.Value = ((ParentTeacherAssociationDetails)oCurrentItem.DataItem).RelatedSection.ToString();
              
                // Highlight the members which are related to current satndard division (Onli members whho having designation "Representative").  
                if (moUserRole == Constants.UserRoles.Student && hidSectionId.Value != Constants.I_ZERO.ToString() && SchoolCommitteeId == Constants.SchoolCommittees.PTA.ToInt())
                {
                    HtmlTableRow oHtmlTableHeaderRow = e.Item.FindControl("trTeacher") as HtmlTableRow;
                    oHtmlTableHeaderRow.Style.Add("background-color", "#FFCCCC");
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill data rowwise in Parent ListView.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwParentDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                Label lblRowIndex = oCurrentItem.FindControl("lblRowIndex") as Label;
                lblRowIndex.Text = (oCurrentItem.DisplayIndex + 1).ToString();
                Label lblContactNumber = oCurrentItem.FindControl("lblContactNumber") as Label;
                string sMobileNo1 = lstvwParentDetails.DataKeys[iRowId]["MobileNumber1"].ToString();
                string sMobileNo2 = lstvwParentDetails.DataKeys[iRowId]["MobileNumber2"].ToString();
                HiddenField hidDesigId = oCurrentItem.FindControl("hidDesigId") as HiddenField;
                HiddenField hidUserId = oCurrentItem.FindControl("hidUserId") as HiddenField;
                hidDesigId.Value = ((ParentTeacherAssociationDetails)oCurrentItem.DataItem).DesignationId.ToString();
                hidUserId.Value = ((ParentTeacherAssociationDetails)oCurrentItem.DataItem).StudentId.ToString();
                string sContctNo = ((ParentTeacherAssociationDetails)oCurrentItem.DataItem).ContactNo.ToString();
                bool iIsMobileNo1 = Convert.ToBoolean(lstvwParentDetails.DataKeys[iRowId]["IsMobileNo1"].ToString());
                bool iIsMobileNo2 = Convert.ToBoolean(lstvwParentDetails.DataKeys[iRowId]["IsMobileNo2"].ToString());

                string sContactNo = string.Empty;
                if (iIsMobileNo1)
                    sContactNo = sMobileNo1 + ", ";
                if (iIsMobileNo2 && sMobileNo2 != string.Empty)
                    sContactNo += sMobileNo2 + ", ";
                //// Split Contact no string and add " "(Space) after each comma.
                string[] sCntactArray = { };
                if (sContctNo != string.Empty)
                {
                    sCntactArray = sContctNo.Split(',');
                    sContctNo = string.Join(", ", sCntactArray.ToArray()).Trim();
                }
                if (sContctNo != string.Empty)
                    sContactNo += sContctNo;
                if (sContactNo.EndsWith(", "))
                    sContactNo = sContactNo.Substring(0, sContactNo.Length - 2);
                lblContactNumber.Text = sContactNo;
                ////When User Role Id is Student hide "Edit" and "Delete" column from ListView.
                if (moUserRole != Constants.UserRoles.Admin &&
                    (moUserRole == Constants.UserRoles.Student 
                    || hidUserHasEditAccess.Value == Constants.S_NO))
                {
                    HtmlTableCell oTblEditCell = e.Item.FindControl("tdPimgEdit") as HtmlTableCell;
                    HtmlTableCell oTblDeleteCell = e.Item.FindControl("tdPimgDelete") as HtmlTableCell;
                    HtmlTableCell oTblDeletetdConsideredAsParentCell = e.Item.FindControl("tdConsideredAsParent") as HtmlTableCell;
                    if (oTblEditCell != null) oTblEditCell.Visible = false;
                    if (oTblDeleteCell != null) oTblDeleteCell.Visible = false;
                    if (oTblDeletetdConsideredAsParentCell != null) oTblDeletetdConsideredAsParentCell.Visible = false;

                    // Highlight the members which are related to current satndard division (Only members who having designation "Representative").  
                    if (hidDesigId.Value == I_DESIGNATION_ID.ToString())
                    {

                        HtmlTableRow oHtmlTableHeaderRow = e.Item.FindControl("trParent") as HtmlTableRow;
                        if (moUserRole != Constants.UserRoles.Teacher && moUserRole != Constants.UserRoles.Student)
                            trLegend.Visible = false;
                        else
							if (moUserRole == Constants.UserRoles.Teacher) 
								trLegend.Visible = false;
							
							else                        
								if (oHtmlTableHeaderRow != null) 
									oHtmlTableHeaderRow.Style.Add("background-color", "#FFCCCC");   

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
    ///  This event is used to edit and update the Teacher or Parent details as per ther selection.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSearchByCategory_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ClearParentAllControls();
            ClearTeacherAllControls();
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            if (oCurrentItem != null)
            {
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                int olblID;

                Label olblName = oCurrentItem.FindControl("lblName") as Label;
                Label olblDesignation = oCurrentItem.FindControl("lblDesignation") as Label;
                HiddenField hidFatherName = oCurrentItem.FindControl("hidFatherName") as HiddenField;
                olblID = Convert.ToInt32(lstvwSearchByCategory.DataKeys[iRowId]["Id"].ToString());

                if (optTeacher.Checked)
                {                
                    cmbTDesignation.Focus();
                    EnableDisableTeacherControls(true);
                    int iDesignationId = Convert.ToInt32(lstvwSearchByCategory.DataKeys[iRowId]["DesignationId"].ToString());
                    hidTeacherId.Value = olblID.ToString();
                    txtTeacherName.Text = olblName.Text;					
                    cmbSection.SelectedValue = Constants.S_ZERO;
                }
                else
                {                
                    optFatherAsParent.Focus();
                    EnableDisableParentControls(true);
                    Label olblMotherName = oCurrentItem.FindControl("lblMotherName") as Label;
                    hidStudentId.Value = olblID.ToString();
                    if (olblName.Text != olblMotherName.Text)
                        txtFatherName.Text = olblName.Text;
                    else
                        txtFatherName.Text = hidFatherName.Value;
                    txtMotherName.Text = olblMotherName.Text;
                    if (olblDesignation.Text == S_UNDERSCORE)
                        cmbPDesignation.SelectedIndex = Constants.I_ZERO;
                    else
                        cmbPDesignation.SelectedItem.Text = olblDesignation.Text;
                    string sMobileNumber1 = lstvwSearchByCategory.DataKeys[iRowId]["MobileNumber1"].ToString();
                    string sMobileNumber2 = lstvwSearchByCategory.DataKeys[iRowId]["MobileNumber2"].ToString();

                    txtMobileNumber1.Text = sMobileNumber1 != string.Empty ? sMobileNumber1 : string.Empty;
                    txtMobileNumber2.Text = sMobileNumber2 != string.Empty ? sMobileNumber2 : string.Empty;
                }
                hidTempUserId.Value = olblID.ToString();
            }
            hidMode.Value = S_NEW_MODE;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This event is used to edit and update the Teacher details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTeacherDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

            lblErrorMsg.Text = string.Empty;
            lblSuccessMsg.Text = string.Empty;

            if (e.CommandName == "TEACHER_EDIT")
            {
                btnSaveTeacherDetails.Text = S_UPDATE;
                cmbTDesignation.Focus();
                EnableDisableTeacherControls(true);
                int iDesignationId = Convert.ToInt32(lstvwTeacherDetails.DataKeys[iRowId]["DesignationId"].ToString());
                Label oLblName = e.Item.FindControl("lblName") as Label;
                HiddenField oHidRelatedSection = e.Item.FindControl("hidSectionId") as HiddenField;
                Label oLblTeacherId = e.Item.FindControl("lblTeacherID") as Label;
                hidTeacherId.Value = oLblTeacherId.Text;
                txtTeacherName.Text = oLblName.Text;
                cmbTDesignation.SelectedValue = iDesignationId.ToString();
                cmbSection.SelectedValue = oHidRelatedSection.Value;
                hidTempUserId.Value = oLblTeacherId.Text;
                hidMode.Value = S_EDIT_MODE;
                hidSelectedRow.Value = iRowId.ToString();
                hidTeacherAssociationId.Value = lstvwTeacherDetails.DataKeys[iRowId]["TeacherAssociationDetailsId"].ToString();
            }
            else if (e.CommandName == "TEACHER_DELETE")
            {
                ClearTeacherAllControls();
                EnableDisableTeacherControls(false);
                int iTeacherAssociationId = Convert.ToInt32(e.CommandArgument);
                DeleteTeacherDetails(iTeacherAssociationId);
                FillTeacherDetails();
                btnSaveTeacherDetails.Text = S_SAVE;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This event is used to edit and update the Parent details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwParentDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            lblErrorMsg.Text = string.Empty;
            lblSuccessMsg.Text = string.Empty;

            if (e.CommandName == "PARENT_EDIT")
            {
                string sNumber = string.Empty;
                btnSaveParentDetails.Text = S_UPDATE;
                optFatherAsParent.Focus();
                EnableDisableParentControls(true);
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                Label oLblResidenceArea = e.Item.FindControl("lblResidenceArea") as Label;
                Label oLblContactTiming = e.Item.FindControl("lblContactTiming") as Label;
                Label oLblConsideredAsParent = e.Item.FindControl("lblConsideredAsParent") as Label;
                Label oLblContactNo = e.Item.FindControl("lblContactNumber") as Label;
                hidStudentId.Value = lstvwParentDetails.DataKeys[iRowId]["StudentId"].ToString();
                bool iIsMobileNo1 = Convert.ToBoolean(lstvwParentDetails.DataKeys[iRowId]["IsMobileNo1"].ToString());
                bool iIsMobileNo2 = Convert.ToBoolean(lstvwParentDetails.DataKeys[iRowId]["IsMobileNo2"].ToString());
                string sMobileNo1 = lstvwParentDetails.DataKeys[iRowId]["MobileNumber1"].ToString();
                string sMobileNo2 = lstvwParentDetails.DataKeys[iRowId]["MobileNumber2"].ToString();
                List<string> sArrContact = oLblContactNo.Text.Split(',').ToList();

                if (iIsMobileNo1 && iIsMobileNo2)
                {
                    sArrContact.RemoveAt(1);
                    sArrContact.RemoveAt(0);
                }
                else if (iIsMobileNo1 || iIsMobileNo2)
                    sArrContact.RemoveAt(0);

                txtMobileNumber1.Text = sMobileNo1;
                txtMobileNumber2.Text = sMobileNo2;
                chkMobileNumber1.Checked = iIsMobileNo1 == true ? true : false;
                chkMobileNumber2.Checked = iIsMobileNo2 == true ? true : false;
                if (oLblConsideredAsParent.Text == "Mother")
                    EnavleParentRadioButton(true);
                else
                    EnavleParentRadioButton(false);

                txtFatherName.Text = lstvwParentDetails.DataKeys[iRowId]["FatherName"].ToString();
                txtMotherName.Text = lstvwParentDetails.DataKeys[iRowId]["MotherName"].ToString();
                txtResidenceArea.Text = oLblResidenceArea.Text;
                txtContactTiming.Text = oLblContactTiming.Text;                
                cmbPDesignation.SelectedValue = lstvwParentDetails.DataKeys[iRowId]["DesignationId"].ToString();
                hidSelectedRow.Value = iRowId.ToString();
                hidMode.Value = S_EDIT_MODE;
                hidParentAssociationDetailsId.Value = lstvwParentDetails.DataKeys[iRowId]["ParentAssociationDetailsId"].ToString();                
            }
            else if (e.CommandName == "PARENT_DELETE")
            {
                ClearParentAllControls();
                EnableDisableParentControls(false);
                int iParentAssociationId = Convert.ToInt32(e.CommandArgument);
                DeleteParentDetails(iParentAssociationId);
                FillParentDetails();
                btnSaveParentDetails.Text = S_SAVE;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnTCancel_Click(object sender, EventArgs e)
    {
    }

    protected void btnPCancel_Click(object sender, EventArgs e)
    {
    }

    #endregion

    #region "Private Methods"

    /// <summary>
    /// This method is used to check user role.
    /// </summary>
    private void CheckUserRole()
    {
       if (moUserRole != Constants.UserRoles.Admin &&
           ((moUserRole == Constants.UserRoles.Student ||
           hidUserHasEditAccess.Value == Constants.S_NO)))
		   {
			   if (moUserRole == Constants.UserRoles.Teacher)
			   {
				   SetControlVisibility(false);
					trLegend.Visible = false;
			   }
			   else
					SetControlVisibility(false);				
		   }
       else
       {
           EnableDisableTeacherControls(false);
           EnableDisableParentControls(false);
           divSearch.Visible = false;
           trLegend.Visible = false;
       }
    }

    /// <summary>
    /// This method is used to Fill Search ListView(Parent or Teacher Details) as per filter selection.
    /// </summary>
    private void FillSearchListView()
    {
        const int I_FILTER_BY_TEACHER = 1;
        const int I_FILTER_BY_PARENT = 2;

        ParentTeacherAssociationDetailsBL oParentTeacherAssociationDetailsBL = new ParentTeacherAssociationDetailsBL();
        if (optTeacher.Checked)
            oParentTeacherAssociationDetailsBL.GetParentOrTeacherDetails(txtSearchByName.Text.Trim(), I_FILTER_BY_TEACHER, miSchoolId, miAcademicYearId);
        else
            oParentTeacherAssociationDetailsBL.GetParentOrTeacherDetails(txtSearchByName.Text.Trim(), I_FILTER_BY_PARENT, miSchoolId, miAcademicYearId);

        if (oParentTeacherAssociationDetailsBL.ParentOrTeacherAssociationDetailsList.Count > Constants.I_ZERO)
        {
            divSearch.Visible = true;
            lstvwSearchByCategory.DataSource = oParentTeacherAssociationDetailsBL.ParentOrTeacherAssociationDetailsList;
            lstvwSearchByCategory.DataBind();
            if (oParentTeacherAssociationDetailsBL.ParentOrTeacherAssociationDetailsList.Count > Constants.I_ZERO)
                FillListViewPagerFooter(lstvwSearchByCategory, DtPgCount);
            SetDataPagerVisibility(oParentTeacherAssociationDetailsBL.ParentOrTeacherAssociationDetailsList);
            lstvwSearchByCategory.Visible = true;
            trNorecordFoundSearch.Visible = false;
        }
        else
        {
            divSearch.Visible = false;
            trNorecordFoundSearch.Visible = true;
            lstvwSearchByCategory.DataSource = null;
            lstvwSearchByCategory.DataBind();
            lstvwSearchByCategory.Visible = false;
            trPager1.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to Fill Teacher, Parent Designaton and Standard Combobox.
    /// </summary>
    private void FillDesignationCombobox()
    {
        ParentTeacherAssociationDetailsBL oParentTeacherAssociationDetailsBL = new ParentTeacherAssociationDetailsBL();
        oParentTeacherAssociationDetailsBL.ParentTeacherAssociationDetails = PopulateParentTeacher();
        oParentTeacherAssociationDetailsBL.FillDesignationCombobox();
        List<ParentTeacherAssociationDetails> lstTeacherDesignationDetails = oParentTeacherAssociationDetailsBL.DesignationDetails.Where(Teacher => Teacher.UserRoleId == Constants.S_ONE.ToInt() || Teacher.UserRoleId == Constants.I_ZERO || Teacher.UserRoleId == Constants.UserRoles.Supervisor.ToInt()).ToList();
        List<ParentTeacherAssociationDetails> lstParentDesignationDetails = oParentTeacherAssociationDetailsBL.DesignationDetails.Where(Parent => Parent.UserRoleId == Constants.S_TWO.ToInt() || Parent.UserRoleId == Constants.I_ZERO).ToList();
        cmbTDesignation.Items.Clear();
		ListSource.FillDropDownList(lstTeacherDesignationDetails, cmbTDesignation, "DesignationName", "DesignationId", Constants.S_SELECT);
        cmbPDesignation.Items.Clear();
        ListSource.FillDropDownList(lstParentDesignationDetails, cmbPDesignation, "DesignationName", "DesignationId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to Fill Teacher and Parent details.
    /// </summary>
    private void FillTeacherParentDetails()
    {
        ParentTeacherAssociationDetailsBL oParentTeacherAssociationDetailsBL = new ParentTeacherAssociationDetailsBL();
        ParentTeacherAssociationDetails oParentTeacherAssociationDetails = PopulateSchoolInfo();
        oParentTeacherAssociationDetailsBL.ParentTeacherAssociationDetails = oParentTeacherAssociationDetails;        
        oParentTeacherAssociationDetailsBL.GetAll();

        // To find Orginal standard id as per user login.
        if (moUserRole == Constants.UserRoles.Student)
        {
            if (oParentTeacherAssociationDetailsBL.StandardDetails.Count > Constants.I_ZERO)
            {
                int iStandardId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_ID].ToString());
                int sStdId = (from OriginalStdId in oParentTeacherAssociationDetailsBL.StandardDetails
                              where OriginalStdId.StandardId == iStandardId
                              select OriginalStdId.OriginalStandardId).FirstOrDefault();
                hidOriginalStandardId.Value = sStdId.ToString();
            }
        }
        if (oParentTeacherAssociationDetailsBL.TeacherAssociationDetailsList.Count > Constants.I_ZERO)
        {
            lstvwTeacherDetails.DataSource = oParentTeacherAssociationDetailsBL.TeacherAssociationDetailsList;
            lstvwTeacherDetails.DataBind();
            hidTeacherRowCount.Value = lstvwTeacherDetails.Items.Count().ToString();
            trLgndDisplayOrNot.Visible = true;
            trNoRecordMsg.Visible = false;
        }
        else
        {
            lstvwTeacherDetails.DataSource = null;
            lstvwTeacherDetails.Visible = false;
            trLgndDisplayOrNot.Visible = false;
            trNoRecordMsg.Visible = true;
            trPager1.Visible = false;
        }
        if (oParentTeacherAssociationDetailsBL.ParentAssociationDetailsList.Count > Constants.I_ZERO)
        {
            lstvwParentDetails.DataSource = oParentTeacherAssociationDetailsBL.ParentAssociationDetailsList;
            lstvwParentDetails.DataBind();
            hidParentRowCount.Value = lstvwParentDetails.Items.Count().ToString();
            trLgndDisplayOrNot.Visible = true;
            SetMessageVisibility(true);
        }
        else
        {
            lstvwParentDetails.DataSource = null;
            if (!trNoRecordMsg.Visible)
               trLgndDisplayOrNot.Visible =true;
            else 
               trLgndDisplayOrNot.Visible =false;
            SetMessageVisibility(false);
        }
    }

    /// <summary>
    /// This method is used to Fill Teacher details.
    /// </summary>
    private void FillTeacherDetails()
    {
        ParentTeacherAssociationDetailsBL oParentTeacherAssociationDetailsBL = new ParentTeacherAssociationDetailsBL();
        oParentTeacherAssociationDetailsBL.ParentTeacherAssociationDetails = PopulateSchoolInfo();
        oParentTeacherAssociationDetailsBL.GetTeacherDetails();

        if (oParentTeacherAssociationDetailsBL.TeacherAssociationDetailsList.Count > Constants.I_ZERO)
        {
            lstvwTeacherDetails.DataSource = oParentTeacherAssociationDetailsBL.TeacherAssociationDetailsList;
            lstvwTeacherDetails.DataBind();
            hidTeacherRowCount.Value = lstvwTeacherDetails.Items.Count().ToString();
            lstvwTeacherDetails.Visible = true;
            trNoRecordMsg.Visible = false;
        }
        else
        {
            hidTeacherRowCount.Value = Constants.I_ZERO.ToString();
            lstvwTeacherDetails.DataSource = null;
            lstvwTeacherDetails.Visible = false;
            trNoRecordMsg.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to Fill Parent details.
    /// </summary>
    private void FillParentDetails()
    {
        ParentTeacherAssociationDetailsBL oParentTeacherAssociationDetailsBL = new ParentTeacherAssociationDetailsBL();
        oParentTeacherAssociationDetailsBL.ParentTeacherAssociationDetails = PopulateSchoolInfo();
        oParentTeacherAssociationDetailsBL.GetParentDetails();

        if (oParentTeacherAssociationDetailsBL.ParentAssociationDetailsList.Count > Constants.I_ZERO)
        {
            lstvwParentDetails.DataSource = oParentTeacherAssociationDetailsBL.ParentAssociationDetailsList;
            lstvwParentDetails.DataBind();
            hidParentRowCount.Value = lstvwParentDetails.Items.Count().ToString();
          ;
            SetMessageVisibility(true);
        }
        else
        {
            hidParentRowCount.Value = Constants.I_ZERO.ToString();
            lstvwParentDetails.DataSource = null;
            SetMessageVisibility(false);
        }
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavaScriptAttribute()
    {
        valPSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valTSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ApplyMouseHoverEffect(new List<Button> { btnPCancel, btnSaveParentDetails, btnSaveTeacherDetails, btnSearch, btnTCancel });
        btnSaveParentDetails.Attributes.Add("onclick", "if(!ValidateParent()) {return false;}");
        //btnTCancel.Attributes.Add("onclick", "if(!ClearTeacherControls()){return false;}");
        btnTCancel.Attributes.Add("onclick", "ClearTeacherControls()");
        //btnPCancel.Attributes.Add("onclick", "if(!ClearParentControls()){return false;}");
        btnPCancel.Attributes.Add("onclick", "ClearParentControls()");
        btnSearch.Attributes.Add("onclick", "if(!ClearErrorMsg()){return false;}");
    }

    /// <summary>
    /// This method is used fill the datapager dropdown list in the list view.
    /// Pager control name should be same as defined here.
    /// e.g. DtPgDropDown is the datapager name which contains the drop down list.
    /// Same for drop down list in the pager control as well as label
    /// </summary>
    public void FillListViewPagerFooter(ListView aolstListView, DataPager aoPgCntDataPager)
    {
        DataPager oDataPager = aolstListView.FindControl("DtPgDropDown") as DataPager;
        HtmlTableRow oTrDataPager = aolstListView.FindControl("trDataPager") as HtmlTableRow;
        oTrDataPager.Visible = false;
        aoPgCntDataPager.Visible = false;
        int iCurrentPage = (oDataPager.StartRowIndex / oDataPager.PageSize) + 1;
        int iTotalPages = oDataPager.TotalRowCount / oDataPager.PageSize;
        if (iTotalPages * oDataPager.PageSize < oDataPager.TotalRowCount)
            iTotalPages += 1;
        if (iTotalPages > 1)
        {
            oTrDataPager.Visible = true;
            aoPgCntDataPager.Visible = true;

            ////Populate the DropDownList if needed
            DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
            if (ddlCnt.Items.Count == 0)
            {
               ////Add a list item for each page
                for (int i = 1; i <= iTotalPages; i++)
                    ddlCnt.Items.Add(i.ToString());

                ////Set the DDL to the appropriate page value
                ddlCnt.Items.FindByValue(iCurrentPage.ToString()).Selected = true;
                Label oLblCurrentPageLabel = oDataPager.Controls[0].FindControl("CurrentPageLabel") as Label;
                oLblCurrentPageLabel.Font.Bold = true;
                oLblCurrentPageLabel.Text = "Page " + iCurrentPage + " of " + iTotalPages;
            }
        }
    }

    /// <summary>
    /// This method is used to set list view according selected page from the pager dropdownlist.
    /// Pager control name should be same as defined here.
    /// e.g. DtPgDropDown is the datapager name which contains the drop down list.
    /// Same for drop down list in the pager control as well as label
    /// </summary>
    public void SetDataPagerAccordingToPageNo(ListView aolstListView)
    {
        DataPager oDtPgDropDown = aolstListView.FindControl("DtPgDropDown") as DataPager;
        DropDownList oDdlCnt = oDtPgDropDown.Controls[0].FindControl("ddlCnt") as DropDownList;
        int iRowIndex = (Convert.ToInt32(oDdlCnt.SelectedValue) - 1) * oDtPgDropDown.PageSize;
        oDtPgDropDown.SetPageProperties(iRowIndex, oDtPgDropDown.PageSize, true);
        int icurrentPage = (oDtPgDropDown.StartRowIndex / oDtPgDropDown.PageSize) + 1;
        int itotalPages = oDtPgDropDown.TotalRowCount / oDtPgDropDown.PageSize;
        Label oLblCurrentPageLabel = oDtPgDropDown.Controls[0].FindControl("CurrentPageLabel") as Label;
        oLblCurrentPageLabel.Text = "Page " + icurrentPage + " of " + itotalPages;
    }

    /// <summary>
    /// This method is used to delete Teacher details.
    /// </summary>
    /// <param name="aiTeacherAssociationId"></param>
    public void DeleteTeacherDetails(int aiTeacherAssociationId)
    {
        ParentTeacherAssociationDetailsBL oParentTeacherAssociationDetailsBL = new ParentTeacherAssociationDetailsBL();
        oParentTeacherAssociationDetailsBL.ParentTeacherAssociationDetails.TeacherAssociationDetailsId = aiTeacherAssociationId;
        oParentTeacherAssociationDetailsBL.DeleteTeacherDetails();
    }

    /// <summary>
    /// This method is used to Delete Parent details.
    /// </summary>
    /// <param name="aiParentAssociationId"></param>
    public void DeleteParentDetails(int aiParentAssociationId)
    {
        ParentTeacherAssociationDetailsBL oParentTeacherAssociationDetailsBL = new ParentTeacherAssociationDetailsBL();
        oParentTeacherAssociationDetailsBL.ParentTeacherAssociationDetails.ParentAssociationDetailsId = aiParentAssociationId;
        oParentTeacherAssociationDetailsBL.DeleteParentDetails();
    }

    /// <summary>
    /// This method is used to clear all teacher controls.
    /// </summary>
    public void ClearTeacherAllControls()
    {
        txtTeacherName.Text = string.Empty;
        cmbTDesignation.SelectedIndex = Constants.I_ZERO;
        cmbSection.SelectedValue = Constants.I_ZERO.ToString();
        lblErrorMsg.Text = string.Empty;
        btnSaveTeacherDetails.Text = S_SAVE;
    }

    /// <summary>
    /// This method is used to clear all parent controls.
    /// </summary>
    public void ClearParentAllControls()
    {
        txtFatherName.Text = string.Empty;
        txtMotherName.Text = string.Empty;
        txtResidenceArea.Text = string.Empty;
        txtContactTiming.Text = string.Empty;
        optFatherAsParent.Checked = true;
        optMotherAsParent.Checked = false;
        cmbPDesignation.SelectedIndex = Constants.I_ZERO;
        lblErrorMsg.Text = string.Empty;
        chkMobileNumber1.Checked = false;
        chkMobileNumber2.Checked = false;
        txtMobileNumber1.Text = string.Empty;
        txtMobileNumber2.Text = string.Empty;
        btnSaveParentDetails.Text = S_SAVE;
    }

    /// <summary>
    /// This method is used to Enable Disable Teacher controls.
    /// </summary>
    /// <param name="abFlag"></param>
    public void EnableDisableTeacherControls(bool abFlag)
    {
        txtTeacherName.Enabled = abFlag;
        cmbTDesignation.Enabled = abFlag;
        cmbSection.Enabled = abFlag;
        btnSaveTeacherDetails.Enabled = abFlag;
        btnTCancel.Enabled = abFlag;
    }

    /// <summary>
    /// This method is used to Enable Disable Parent controls.
    /// </summary>
    /// <param name="abFlag"></param>
    public void EnableDisableParentControls(bool abFlag)
    {
        txtFatherName.Enabled = abFlag;
        txtMotherName.Enabled = abFlag;
        txtResidenceArea.Enabled = abFlag;
        txtContactTiming.Enabled = abFlag;
        cmbPDesignation.Enabled = abFlag;
        optFatherAsParent.Enabled = abFlag;
        optMotherAsParent.Enabled = abFlag;
        btnSaveParentDetails.Enabled = abFlag;
        btnPCancel.Enabled = abFlag;
        chkMobileNumber1.Enabled = abFlag;
        chkMobileNumber2.Enabled = abFlag;
        txtMobileNumber1.Text = string.Empty;
        txtMobileNumber2.Text = string.Empty;
        txtMobileNumber1.Enabled = abFlag;
        txtMobileNumber2.Enabled = abFlag;
    }

    /// <summary>
    /// This method is used to set control visibility when USER ROLE is Student.
    /// </summary>
    /// <param name="abFlag"></param>
    public void SetControlVisibility(bool abFlag)
    {
        trSearchControlsAndLstvw.Visible = abFlag;
        trTeacherEditControls.Visible = abFlag;
        trParentEditControls.Visible = abFlag;
        MandatoryMark.Visible = abFlag;
        if (moUserRole != Constants.UserRoles.Teacher && moUserRole != Constants.UserRoles.Student)
            trLegend.Visible = false;
        else
            trLegend.Visible = true;

        if (SchoolCommitteeId != Constants.SchoolCommittees.PTA.ToInt())
            trLegend.Visible = false;

        HtmlTableRow oTTableRow = lstvwTeacherDetails.FindControl("trTHeader") as HtmlTableRow;
        if (oTTableRow != null)
        {
            HtmlTableCell oTblEditCell = oTTableRow.FindControl("TeacherEdit") as HtmlTableCell;
            HtmlTableCell oTblDeleteCell = oTTableRow.FindControl("TeacherDelete") as HtmlTableCell;
            HtmlTableCell oTblDeleteSectionCell = oTTableRow.FindControl("thSection") as HtmlTableCell;
            if (oTblEditCell != null) oTblEditCell.Visible = false;
            if (oTblDeleteCell != null) oTblDeleteCell.Visible = false;
            if (oTblDeleteSectionCell != null) oTblDeleteSectionCell.Visible = false;
        }
        HtmlTableRow oPTableRow = lstvwParentDetails.FindControl("trHeader") as HtmlTableRow;
        if (oPTableRow != null)
        {
            HtmlTableCell oTblEditCell = oPTableRow.FindControl("ParentEdit") as HtmlTableCell;
            HtmlTableCell oTblDeleteCell = oPTableRow.FindControl("ParentDelete") as HtmlTableCell;
            HtmlTableCell oTblDeleteConsideredAsParentCell = oPTableRow.FindControl("thConsideredAsParent") as HtmlTableCell;
            if (oTblEditCell != null) oTblEditCell.Visible = false;
            if (oTblDeleteCell != null) oTblDeleteCell.Visible = false;
            if (oTblDeleteConsideredAsParentCell != null) oTblDeleteConsideredAsParentCell.Visible = false;
        }
        if (lstvwTeacherDetails.Items.Count == 0)
        {
            lstvwTeacherDetails.DataSource = null;
            trNoRecordMsg.Visible = true;
        }
        if (lstvwParentDetails.Items.Count == 0)
        {
            lstvwParentDetails.DataSource = null;
            divContainer.Visible = false;
            trNoRecordParentMsg.Visible = true;
        }
        SetScreenWidth();
    }

    /// <summary>
    /// This method is used to set label visibility.
    /// </summary>
    /// <param name="oDataTable"></param>
    public void SetDataPagerVisibility(List<ParentTeacherAssociationDetails> alstParentTeacherAssoDetails)
    {
        if (alstParentTeacherAssoDetails.Count > I_PAGE_SIZE)
            trPager1.Visible = true;
        else
            trPager1.Visible = false;
    }

    /// <summary>
    /// This method is used to Save Teacher details.
    /// </summary>
    public void SaveTeacherDetails()
    {
        ParentTeacherAssociationDetailsBL oParentTeacherAssociationDetailsBL = new ParentTeacherAssociationDetailsBL();
        oParentTeacherAssociationDetailsBL.ParentTeacherAssociationDetails = PopulateTeacher();
        oParentTeacherAssociationDetailsBL.SaveTeacherDetails(hidMode.Value);
    }

    /// <summary>
    /// This method is used to save Parent details.
    /// </summary>
    public void SaveParentDetails()
    {
        ParentTeacherAssociationDetailsBL oParentTeacherAssociationDetailsBL = new ParentTeacherAssociationDetailsBL();
        oParentTeacherAssociationDetailsBL.ParentTeacherAssociationDetails = PopulateSchoolInfo();
        oParentTeacherAssociationDetailsBL.SaveParentDetails(GenerateXml(PopulateParent()), hidMode.Value);
    }

    /// <summary>
    /// This method is used to populate School Details.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private ParentTeacherAssociationDetails PopulateSchoolInfo()
    {
        ParentTeacherAssociationDetails oParentTeacherAssociationDetails = new ParentTeacherAssociationDetails();
        oParentTeacherAssociationDetails.SchoolId = miSchoolId;
        oParentTeacherAssociationDetails.AcademicYearId = miAcademicYearId;
        oParentTeacherAssociationDetails.InsertedById = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID].ToString());
        oParentTeacherAssociationDetails.SchoolCommitteeId = SchoolCommitteeId;
        return oParentTeacherAssociationDetails;
    }

    /// <summary>
    /// This method is used to populate Teacher Details.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private ParentTeacherAssociationDetails PopulateTeacher()
    {
        ParentTeacherAssociationDetails oParentTeacherAssociationDetails = new ParentTeacherAssociationDetails();
        oParentTeacherAssociationDetails.DesignationId = Convert.ToInt32(cmbTDesignation.SelectedItem.Value.ToString());
        if (hidMode.Value == S_NEW_MODE)
            oParentTeacherAssociationDetails.TeacherAssociationDetailsId = Constants.I_ZERO;
        else
            oParentTeacherAssociationDetails.TeacherAssociationDetailsId = Convert.ToInt32(hidTeacherAssociationId.Value);
        oParentTeacherAssociationDetails.RelatedSection = Convert.ToInt32(cmbSection.SelectedValue);
        oParentTeacherAssociationDetails.TeacherId = Convert.ToInt32(hidTeacherId.Value);
        oParentTeacherAssociationDetails.InsertedById = miUserId;
        oParentTeacherAssociationDetails.SchoolId = miSchoolId;
        oParentTeacherAssociationDetails.AcademicYearId = miAcademicYearId;
        oParentTeacherAssociationDetails.SchoolCommitteeId = SchoolCommitteeId;
        return oParentTeacherAssociationDetails;
    }

    /// <summary>
    /// This method is used to populate Parent Details.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private List<ParentTeacherAssociationDetails> PopulateParent()
    {
        ParentTeacherAssociationDetails oParentTeacherAssociationDetails = new ParentTeacherAssociationDetails();
        List<ParentTeacherAssociationDetails> lstParentTeacherAssociationDetails = new List<ParentTeacherAssociationDetails>();

        oParentTeacherAssociationDetails.FatherName = txtFatherName.Text;
        oParentTeacherAssociationDetails.MotherName = txtMotherName.Text;
        oParentTeacherAssociationDetails.StudentId = Convert.ToInt32(hidStudentId.Value);
        oParentTeacherAssociationDetails.ResidenceArea = txtResidenceArea.Text;
        oParentTeacherAssociationDetails.ContactTiming = txtContactTiming.Text;
        string sConsiderAsParent;
        if (optFatherAsParent.Checked)
            sConsiderAsParent = optFatherAsParent.Text;
        else
            sConsiderAsParent = optMotherAsParent.Text;
        oParentTeacherAssociationDetails.ConsideredAsParent = sConsiderAsParent;
        oParentTeacherAssociationDetails.Section = string.Empty;
        oParentTeacherAssociationDetails.DesignationId = Convert.ToInt32(cmbPDesignation.SelectedItem.Value);
        oParentTeacherAssociationDetails.FromStandardId = Constants.I_ZERO;
        oParentTeacherAssociationDetails.ToStandardId = Constants.I_ZERO;
        oParentTeacherAssociationDetails.ContactNo = string.Empty;
        oParentTeacherAssociationDetails.MobileNumber1 = txtMobileNumber1.Text;
        oParentTeacherAssociationDetails.MobileNumber2 = txtMobileNumber2.Text;
        oParentTeacherAssociationDetails.IsMobileNo1 = chkMobileNumber1.Checked;
        oParentTeacherAssociationDetails.IsMobileNo2 = chkMobileNumber2.Checked;
        oParentTeacherAssociationDetails.Is_Deleted = Constants.C_NO;
        oParentTeacherAssociationDetails.InsertedById = miUserId;
        oParentTeacherAssociationDetails.SchoolId = miSchoolId;
        oParentTeacherAssociationDetails.AcademicYearId = miAcademicYearId;
        if (hidMode.Value == S_NEW_MODE)
            oParentTeacherAssociationDetails.ParentAssociationDetailsId = Constants.I_ZERO;
        else
            oParentTeacherAssociationDetails.ParentAssociationDetailsId = Convert.ToInt32(hidParentAssociationDetailsId.Value);
        lstParentTeacherAssociationDetails.Add(oParentTeacherAssociationDetails);

        return lstParentTeacherAssociationDetails;
    }

    /// <summary>
    /// This method is used to populate Parent Teacher Details to fill combobox.
    /// </summary>
    /// <returns></returns>
    private ParentTeacherAssociationDetails PopulateParentTeacher()
    {
        ParentTeacherAssociationDetails oParentTeacherAssociationDetails = new ParentTeacherAssociationDetails();
        oParentTeacherAssociationDetails.TeacherUserRoleId = I_TEACHER_USER_ROLE_ID;
        oParentTeacherAssociationDetails.StudentUserRoleId = I_STUDENT_USER_ROLE_ID;
        oParentTeacherAssociationDetails.AdminUserRoleId = I_ADMIN_USER_ROLE_ID;
        oParentTeacherAssociationDetails.SchoolId = miSchoolId;
        oParentTeacherAssociationDetails.AcademicYearId = miAcademicYearId;
        oParentTeacherAssociationDetails.SchoolCommitteeId = SchoolCommitteeId;
        return oParentTeacherAssociationDetails;
    }

    /// <summary>
    /// This method is used to set screen width.
    /// </summary>
    private void SetScreenWidth()
    {
        if (Session[Constants.S_SESSION_SCREEN_WIDTH] != null)
        {
            string str = Session[Constants.S_SESSION_SCREEN_WIDTH].ToString().Replace("px !important", string.Empty);
            int iWidth = Convert.ToInt32(str) - 265;            
            if (lstvwParentDetails.Items.Count < 5)
                divContainer.Style.Add("height", Convert.ToString(200) + "px !important");
            iWidth = iWidth / 100 * 80;
            divContainer.Style.Add("width", iWidth.ToString() + "px !important");
            divSearch.Style.Add("width", iWidth.ToString() + "px !important");
            pnlTeacherDetails.Style.Add("width", iWidth.ToString() + "px !important");
        }
        else
            divContainer.Style.Add("width", Convert.ToString(1024) + "px !important");
    }

    /// <summary>
    /// This method is used to enable the radio button.
    /// </summary>
    /// <param name="abFlag"></param>
    private void EnavleParentRadioButton(bool abFlag)
    {
        optMotherAsParent.Checked = abFlag;
        optFatherAsParent.Checked = !abFlag;
    }

    /// <summary>
    /// This method is used to set message visibility.
    /// </summary>
    /// <param name="abFlag"></param>
    private void SetMessageVisibility(bool abFlag)
    {
        divContainer.Visible = abFlag;
        trNoRecordParentMsg.Visible = !abFlag;
    }

    /// <summary>
    /// This method is used to update sitemap entry.
    /// </summary>
    private void UpdateSitemapEntry()
    {
        string sText = (SchoolCommitteeId == Convert.ToInt32(Constants.SchoolCommittees.PTA) ? "Parent Teacher Association" : "Transport Committee");
        //MasterPage oMasterPage = (MasterPage)this.Master;
        //oMasterPage.SetCurrentNodeText(sText, moUserRole.ToInt(), miSchoolId);

        MasterPage oMasterPage = (MasterPage)this.Master;
        oMasterPage.NodeTitle = sText;
        oMasterPage.SetCurrentNodeText(sText, moUserRole.ToInt(), miSchoolId);
    }
 
    #endregion    
}