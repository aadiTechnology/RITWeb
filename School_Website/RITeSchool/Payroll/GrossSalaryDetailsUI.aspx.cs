/*
 * File Name - GrossSalaryDetailsUI.aspx.cs
 * Created Date - 4 April 2014
 * Created By - Sachin
 * Description - This class is used to manage association of user and payment category.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class GrossSalaryDetailsUI : SchoolBase
{
    #region Data Member(S)
    
    private UserPaymentCategoryAssoBL moUserPaymentCategoryAssoBL; 
    private List<PaymentCategory> mlstCategories = new List<PaymentCategory>();

    #endregion

    #region Constant(s)
    
    private const string S_PAYMENT_CATEGORIES = "PaymentCategories"; 

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
                hidSortExpression.Value = "UserName";
                hidSortDirection.Value = Constants.S_ASCENDING;
            }

            AddSortImage(lstvwAssociation, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to  fill user list view, set default values and fill staff group combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moUserPaymentCategoryAssoBL = new UserPaymentCategoryAssoBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                SetDefaultValues();
                FillStaffGroupCombobox();
                ReadAllCategories();
                FillUsers();
            }

            RefreshValue();
            trMessage.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill user list view according to selected staff group.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStaffgroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ViewState[S_PAYMENT_CATEGORIES] != null)
                mlstCategories = ViewState[S_PAYMENT_CATEGORIES] as List<PaymentCategory>;

            DataPager oDtPgDropDown = lstvwAssociation.FindControl("DtPgDropDown") as DataPager;
            if (oDtPgDropDown != null)
            {
                DropDownList oddlCnt = (oDtPgDropDown.Controls[0].FindControl("ddlCnt")) as DropDownList;
                if (oddlCnt != null)
                {
                    if (!oddlCnt.SelectedValue.IsNullOrEmpty())
                    {
                        if (oddlCnt.Items.Count >= 1)
                            oddlCnt.SelectedIndex = 0;
                        cmbPageCnt_SelectedIndexChanged(oddlCnt, null);
                    }
                }
            }
            lstvwAssociation.DataBind();

            ResetHeaderFields();
            hidSelectedRows.Value = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save association.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            List<UserPaymentCategoryAssociation> lstAssociations = Populate();
            if (lstAssociations.Count > 0)
            {
                string sXml = base.GenerateXml(lstAssociations);
                moUserPaymentCategoryAssoBL.Save(sXml);
            }

            //lblMessage.Text = Resources.LocalizedResources.msgUserPaymentCategorySave;

            base.DisplayMessage(Resources.LocalizedResources.msgUserPaymentCategorySave, false, tdMessage);

            trMessage.Visible = true;
            FillUsers();
            hidSelectedRows.Value = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle pager setting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAssociation_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwAssociation.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooterWithCulture(lstvwAssociation, DtPgCount, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);
            else
                DtPgCount.Visible = false;

            SetConfirmationMessage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set attributes on list view controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAssociation_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                UserPaymentCategoryAssociation oUserPaymentCategoryAssociation = e.Item.DataItem as UserPaymentCategoryAssociation;

                CheckBox ChkSelect = e.Item.FindControl("ChkSelect") as CheckBox;
                
                if (oUserPaymentCategoryAssociation.Id != 0)
                    ChkSelect.Checked = true;

                Label lblRowNo = e.Item.FindControl("lblRowNo") as Label;
                DropDownList cmbCategory = e.Item.FindControl("cmbCategory") as DropDownList;
                ListSource.FillDropDownList(mlstCategories, cmbCategory, "Name", "Id", Constants.S_SELECT);
                cmbCategory.Attributes.Add("onchange", "UpdateRowSelection(this,0," + lblRowNo.Text.Trim() + ")");

                ChkSelect.Attributes.Add("onclick", "DisableRespectiveField(this," + e.Item.DisplayIndex + "," + lblRowNo.Text.Trim() + ")");

                ListItem oListItem = cmbCategory.Items.FindByValue(oUserPaymentCategoryAssociation.CategoryId.ToString());
                if (oListItem != null)
                    oListItem.Selected = true;

                TextBox txtAmount = e.Item.FindControl("txtAmount") as TextBox;                
                txtAmount.Attributes.Add("onchange", "UpdateRowSelection(this,1," + lblRowNo.Text.Trim() + ")");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
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
            if (ViewState[S_PAYMENT_CATEGORIES] != null)
                mlstCategories = ViewState[S_PAYMENT_CATEGORIES] as List<PaymentCategory>;

            ReadPageNo();
            ResetHeaderFields();
            hidSelectedRows.Value = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to update category combo boxes for updated list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void hidUpdateGrid_ValueChanged(object sender, EventArgs e)
    {
        try
        {
            PaymentCategoryBL oPaymentCategoryBL = new PaymentCategoryBL(miSchoolId, miUserId);
            List<PaymentCategory> lstCategories = oPaymentCategoryBL.GetAll().OrderBy(ct => ct.Name).ToList();
            ViewState[S_PAYMENT_CATEGORIES] = lstCategories;

            foreach (var oItem in lstvwAssociation.Items)
            {
                if (oItem.ItemType == ListViewItemType.DataItem)
                {
                    DropDownList cmbCategory = oItem.FindControl("cmbCategory") as DropDownList;
                    int iCategoryId = Convert.ToInt32(cmbCategory.SelectedValue);
                    ListSource.FillDropDownList(lstCategories, cmbCategory, "Name", "Id", Constants.S_SELECT);

                    ListItem oListItem = cmbCategory.Items.FindByValue(iCategoryId.ToString());
                    if (oListItem != null)
                        oListItem.Selected = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to populate association object.
    /// </summary>
    /// <returns></returns>
    private List<UserPaymentCategoryAssociation> Populate()
    {
        List<UserPaymentCategoryAssociation> lstAssociations = new List<UserPaymentCategoryAssociation>();
        foreach (var oItem in lstvwAssociation.Items)
        {
            if (oItem.ItemType == ListViewItemType.DataItem)
            {
                int iAssoId = Convert.ToInt32(lstvwAssociation.DataKeys[oItem.DisplayIndex]["Id"]);
                int iUserId = Convert.ToInt32(lstvwAssociation.DataKeys[oItem.DisplayIndex]["UserId"]);

                CheckBox chkSelect = oItem.FindControl("ChkSelect") as CheckBox;
                DropDownList cmbCategory = oItem.FindControl("cmbCategory") as DropDownList;
                TextBox txtAmount = oItem.FindControl("txtAmount") as TextBox;
                Label lblRowNo = oItem.FindControl("lblRowNo") as Label;
                string[] sarrSelectedRows = hidSelectedRows.Value.Split(',');

                if ((chkSelect.Checked || iAssoId != 0) && sarrSelectedRows.Contains(lblRowNo.Text.Trim()))
                {
                    lstAssociations.Add
                        (
                            new UserPaymentCategoryAssociation
                            {
                                Id = iAssoId,
                                UserId = iUserId,
                                CategoryId = Convert.ToInt32(cmbCategory.SelectedValue),
                                Amount = Convert.ToInt32(txtAmount.Text),
                                IsDeleted = !chkSelect.Checked
                            });
                }
            }
        }

        return lstAssociations;
    }

    /// <summary>
    /// This method is used to reset header fields.
    /// </summary>
    private void ResetHeaderFields()
    {
        HtmlTableRow oHtmlTableRow = lstvwAssociation.FindControl("trHeaderContol") as HtmlTableRow;
        if (oHtmlTableRow != null)
        {
            DropDownList oDropDownList = oHtmlTableRow.FindControl("cmbAllStaffGroups") as DropDownList;
            if (oDropDownList != null)
                oDropDownList.ClearSelection();

            TextBox txtAllAmount = oHtmlTableRow.FindControl("txtAllAmount") as TextBox;
            if (txtAllAmount != null)
                txtAllAmount.Text = Constants.S_ZERO;
        }
    }

    /// <summary>
    /// This method is used to read all categories.
    /// </summary>
    private void ReadAllCategories()
    {
        PaymentCategoryBL oPaymentCategoryBL = new PaymentCategoryBL(miSchoolId, miUserId);
        List<PaymentCategory> lstCategories = oPaymentCategoryBL.GetAll().OrderBy(ct => ct.Name).ToList();
        ViewState[S_PAYMENT_CATEGORIES] = lstCategories;
    }

    /// <summary>
    /// This method is used to fill user list view.
    /// </summary>
    private void FillUsers()
    {
        if (ViewState[S_PAYMENT_CATEGORIES] != null)
            mlstCategories = ViewState[S_PAYMENT_CATEGORIES] as List<PaymentCategory>;

        lstvwAssociation.DataSourceID = "objdsPayments";
        lstvwAssociation.DataBind();

        FillHeaderFields();
    }

    /// <summary>
    /// This method is used to fill header controls.
    /// </summary>
    private void FillHeaderFields()
    {
        HtmlTableRow oHtmlTableRow = lstvwAssociation.FindControl("trHeaderContol") as HtmlTableRow;
        if (oHtmlTableRow != null)
        {
            DropDownList oDropDownList = oHtmlTableRow.FindControl("cmbAllStaffGroups") as DropDownList;
            if (oDropDownList != null)
                ListSource.FillDropDownList(mlstCategories, oDropDownList, "Name", "Id", Constants.S_SELECT);

            TextBox txtAllAmount = oHtmlTableRow.FindControl("txtAllAmount") as TextBox;
            if (txtAllAmount != null)
                txtAllAmount.Text = Constants.S_ZERO;
        }
    }
   
    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        ApplyMouseHoverEffect(new List<Button> { BtnSave, btnBack });        
        lnkPaymentCategories.Attributes.Add("onclick", "OpenPopup(); return false;");
        BtnSave.Attributes.Add("onclick","ResetFields()");
        hidSortDirection.Value = Constants.S_ASCENDING;
        hidSortExpression.Value = "UserName";
        cmbStaffgroups.Focus();
    }

    /// <summary>
    /// This method is used to fill staff group combo box.
    /// </summary>
    private void FillStaffGroupCombobox()
    {
        StaffGroupsBL oStaffGroupsBL = new StaffGroupsBL();
        List<StaffGroupsEntity> lstStaffGroups = oStaffGroupsBL.GetAllStaffGroups(miSchoolId);
        ListSource.FillDropDownList(lstStaffGroups, cmbStaffgroups, "StaffGroupsName", "StaffGroupsId", Constants.S_ALL);
    }

    /// <summary>
    /// This method is used to set confirmation message on change of page.
    /// </summary>
    private void SetConfirmationMessage()
    {
        DataPager oDataPager = lstvwAssociation.FindControl("DtPgDropDown") as DataPager;
        if (oDataPager != null)
        {
            DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
            if (ddlCnt != null)
                ddlCnt.Attributes.Add("onchange", "if(!PageChangeMessage('" + ddlCnt.ClientID + "')){return false;}");
        }
    }

    /// <summary>
    /// This method is used to read page no.
    /// </summary>
    private void ReadPageNo()
    {
        ControlUtility.SetDataPagerAccordingToPageNo(lstvwAssociation);
        DataPager oDataPager = lstvwAssociation.FindControl("DtPgDropDown") as DataPager;
        if (oDataPager != null)
        {
            DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
            if (ddlCnt != null)
                hidPageNo.Value = ddlCnt.SelectedValue;
        }
    }

    /// <summary>
    /// This method is used to refresh values according to culture.
    /// </summary>
    private void RefreshValue()
    {
        valSum.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidvalAssociationAmount.Value = Resources.LocalizedResources.valAssociationAmount;
        hidvalAssociationCategory.Value = Resources.LocalizedResources.valAssociationCategory;
        hidvalAssociation.Value = Resources.LocalizedResources.valAssociation;
        hidmsgUnsaveMessage.Value = Resources.LocalizedResources.msgUnsaveMessage;
    }
         

    #endregion
}