using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using Utility;
using System.Web.UI.HtmlControls;
using BusinessLogic;
using PayrollEntities;
using System.Data;

public partial class UserShiftAssociation : SchoolBase
{

    #region "CONSTANTS"

        const string S_DEFAULT_SORT_EXP = "Name";
        const string S_SAVE_MESSAGE = "User Shift Details are saved Successfully !!!";

    #endregion

    #region "Events"

        /// <summary>
        /// This event is used to add sort image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                AddSortImage();
            }
            catch (Exception ex)
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }

    /// <summary>
    /// This event is used to load all the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetDefaultValues();
                fillShifts();
                FillStaffGroups();
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to click on show button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnshow_Click(object sender, EventArgs e)
    {
        try
        {
            lstvwStaffGroupUsers.DataSourceID = GrdDSobj.ID;
            lstvwStaffGroupUsers.DataBind();
            
            btnSaveShifts.Visible = lstvwStaffGroupUsers.Items.Count > 0;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to fill gridview after changing page index.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStaffGroupUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
           
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save shift Details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveShifts_Click(object sender, EventArgs e)
    {
        try
        {
            UserShiftAssociationBL oShiftDetailsBL = new UserShiftAssociationBL();
            List<UserShiftAssociationDetails> lstUserShiftAssociationDetails = new List<UserShiftAssociationDetails>();
            

            foreach (ListViewDataItem ldi in lstvwStaffGroupUsers.Items)
            {
                UserShiftAssociationDetails oUserShiftAssociationDetails = new UserShiftAssociationDetails();
                if (ldi.ItemType == ListViewItemType.DataItem)
                {
                    CheckBox chkIsSelected = (CheckBox)(ldi.FindControl("ChkSelect"));                   

                    int UserID = Convert.ToInt32(lstvwStaffGroupUsers.DataKeys[ldi.DisplayIndex]["UserId"]);
                    int shiftId = Convert.ToInt32(lstvwStaffGroupUsers.DataKeys[ldi.DisplayIndex]["ShiftId"]);                   

                    if (chkIsSelected.Checked && shiftId == Constants.I_ZERO)
                    {
                        oUserShiftAssociationDetails.UserId = UserID;
                        oUserShiftAssociationDetails.Action = Constants.I_ONE;

                        lstUserShiftAssociationDetails.Add(oUserShiftAssociationDetails);
                    }

                    if (!chkIsSelected.Checked && shiftId != Constants.I_ZERO)
                    {
                        oUserShiftAssociationDetails.UserId = UserID;
                        oUserShiftAssociationDetails.Action = Constants.I_ZERO;

                        lstUserShiftAssociationDetails.Add(oUserShiftAssociationDetails);
                    }                    
                }
            }
            if (lstUserShiftAssociationDetails.Count > Constants.I_ZERO)
            {
                oShiftDetailsBL.InsertUserShiftAssociationDetailsForUser(base.GenerateXml(lstUserShiftAssociationDetails), miSchoolId, miAcademicYearId, cmbShifts.SelectedValue.ToInt(), miUserId);
                lblUpdateSucess.Text = S_SAVE_MESSAGE;
            }

            lstvwStaffGroupUsers.DataSourceID = GrdDSobj.ID;
            lstvwStaffGroupUsers.DataBind();
            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view page wise travelers.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStaffGroupUsers);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to list view data bound.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStaffGroupUsers_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStaffGroupUsers.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwStaffGroupUsers, DtPgCount);
                SetConfirmationMessage();

                DataPager oDataPager = lstvwStaffGroupUsers.FindControl("DtPgDropDown") as DataPager;
                if (oDataPager != null)
                {
                    DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
                    if (ddlCnt != null)
                        hidPageNo.Value = (ddlCnt.SelectedIndex + 1).ToString();
                }

            }
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwStaffGroupUsers_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CheckBox chkUser = e.Item.FindControl("ChkSelect") as CheckBox;
                int iShiftId = Convert.ToInt32(lstvwStaffGroupUsers.DataKeys[e.Item.DisplayIndex]["ShiftId"]);

                if (iShiftId != 0 && iShiftId != Constants.I_ZERO)
                {
                    chkUser.Checked = true;
                }
                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region "Private Methods"

    /// <summary>
    /// This method is used to Fill Staff Group combo box.
    /// </summary>
    private void FillStaffGroups()
    {
        StaffGroupsBL oStaffGroupsBL = new StaffGroupsBL();
        List<StaffGroupsEntity> staffGroups = oStaffGroupsBL.GetAllStaffGroups(miSchoolId);
        ListSource.FillDropDownList(staffGroups, cmbStaffGroup, "staffGroupsName", "staffGroupsId", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This event is used tofill Shift Combobox.
    /// </summary>
    private void fillShifts()
    {
        ShiftDetailsBL oShiftDetailsBL = new ShiftDetailsBL();
        List<SchoolShifts> lstShifts = oShiftDetailsBL.GetAllShifts(miSchoolId, miAcademicYearId);
        ListSource.FillDropDownList(lstShifts, cmbShifts, "ShiftName", "ShiftId",Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill list view data.
    /// </summary>
    private void FillStaffGroupUsersGrid()
    {
        if (cmbShifts.SelectedValue.ToInt() != 0)
        {
            HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwStaffGroupUsers.FindControl("trHeader");
            if (oHtmlTableRow != null)
            {
                CheckBox oCheckBox = (CheckBox)oHtmlTableRow.FindControl("ChkSelectAll");

                foreach (ListViewDataItem oCurrentItem in lstvwStaffGroupUsers.Items)
                {
                    CheckBox oCheckBoxIAltItemShift = (CheckBox)oCurrentItem.FindControl("ChkSelect");
                    CheckBox oCheckBoxItemShift = (CheckBox)oCurrentItem.FindControl("ChkSelect");
                   
                    int? shiftid = Convert.ToInt32(lstvwStaffGroupUsers.DataKeys[oCurrentItem.DisplayIndex]["ShiftId"]);
                    if (shiftid != 0 && shiftid == cmbShifts.SelectedValue.ToInt())
                    {
                        oCheckBoxIAltItemShift.Checked = true;
                        oCheckBoxItemShift.Checked = true;
                    }
                    else
                    {
                        oCheckBoxIAltItemShift.Checked = false;
                        oCheckBoxItemShift.Checked = false;
                    }
                }
                oCheckBox.Focus();
                btnSaveShifts.Visible = true;
            }
        }
    }

    /// <summary>
    /// This method is used to set JavaScript attributes
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSaveShifts, btnBack });
        btnSaveShifts.Attributes["onclick"] = "ResetUpdateLbl()";
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related));
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidSortDirection.Value = SortDirection.Ascending.ToString();
    }

    /// <summary>
    /// This method is used to set confirmation messaege on change of page.
    /// </summary>
    private void SetConfirmationMessage()
    {
        DataPager oDataPager = lstvwStaffGroupUsers.FindControl("DtPgDropDown") as DataPager;
        if (oDataPager != null)
        {
            DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
            if (ddlCnt != null)
            {
                ddlCnt.Attributes.Add("onchange", "if(!MessageAboutDate('" + ddlCnt.ClientID + "')){return false;}");
            }
        }
    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwStaffGroupUsers.SortDirection.ToString() == "Ascending" || lstvwStaffGroupUsers.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwStaffGroupUsers.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwStaffGroupUsers.SortExpression.ToString();
        else
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;

        HtmlTableRow oHtmlTableHeaderRow = lstvwStaffGroupUsers.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    #endregion    
}