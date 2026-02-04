using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BusinessLogic;
using Utility;
using System.Xml;
using BookEntities;
using BusinessLogic.Exceptions;

public partial class LibraryVendorUI : SchoolBase
{
    #region "Data Members"
    const string S_DEFAULT_SORT_EXP = "Vendor_Name";
    const string S_COMMAND_REMOVE = "RemoveVendor";
    const string S_COMMAND_UPDATE = "UpdateVendor";
    #endregion "Data Members"

    #region "Events"

    /// <summary>
    /// This event is used to set default control values and to set javascript attributes for buttons.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {			
            if (!IsPostBack)
            {
                SetDefaultControls();
                valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                FillVendorDetails();
                SetJavaScriptAttributres();
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save library vendor details as well as its configuration details. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            LibraryVendors oLibraryVendor = PopulateLibraryVendor();
            LibraryVendorBL oLibraryVendorBL = new LibraryVendorBL();
            oLibraryVendorBL.LibraryVendor = oLibraryVendor;
            lblErrorMsg.Text = string.Empty;
            if (oLibraryVendorBL.IsVendorDuplicate() == 0)
            {
                if (hidMode.Value != "Update")
                {
                    oLibraryVendorBL.InsertLibraryVendorBL();
                    lblUpdateSucess.Text = "Vendor details saved successfully!!";
                }
                else
                {
                    int iVendorId = Convert.ToInt32(hidVendorId.Value);
                    oLibraryVendorBL.UpdateLibraryVendorBL(iVendorId);
                    lblUpdateSucess.Text = "Vendor details updated successfully!!";
                }
            }
            else
                lblErrorMsg.Text = "Vendor Name already exists.";
            
			if (QueryString[0] == Constants.S_NO)
				SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.LibraryVendors));
            FillVendorDetails();
            lblCheckDependency.Text = string.Empty;
            lblUpdateSucess.Visible = true;
            SetDefaultControls();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

	/// <summary>
    /// This event is used to cancle saving.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            SetDefaultControls();
            AddSortImage();
            lblErrorMsg.Text = string.Empty;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view page wise library vendor list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwLibraryVendor);
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    #endregion "Events"

    #region "Listview Events"

    /// <summary>
    /// This event is used to add confirmation message while deleting existing library vendor's record.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwLibraryVendor_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                DataRowView oDataRowView = oCurrentItem.DataItem as DataRowView;
                ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to edit and delete the existing library vendor's details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwLibraryVendor_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName != "Sort")
            {
                lblErrorMsg.Text = string.Empty;
                lblUpdateSucess.Text = string.Empty;
                lblCheckDependency.Text = string.Empty;
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iListIndex = oCurrentItem.DisplayIndex;
                int iVendorId = Convert.ToInt32(lstvwLibraryVendor.DataKeys[iListIndex]["VendorId"]);
                string sVendorName = lstvwLibraryVendor.DataKeys[iListIndex]["VendorName"].ToString();
                hidVendorId.Value = iVendorId.ToString();
                hidVendorName.Value = sVendorName;
             
                if (e.CommandName == S_COMMAND_REMOVE)
                {
                    SetDefaultControls();
                    DeleteLibraryVendorDetails(iVendorId, sVendorName);
                }
                else if (e.CommandName == S_COMMAND_UPDATE)
                    LoadLibraryVendorDetails(iVendorId);
                FillVendorDetails();
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort the listview of library vendor's by Name,Address and Mobile No.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwLibraryVendor_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill footer property and add sort image for existing library vendor listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwLibraryVendor_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwLibraryVendor.Items.Count > 0)
            {
                lstvwLibraryVendor.Items.Clear();
                ControlUtility.FillListViewPagerFooter(lstvwLibraryVendor, DtPgCount);
                if (IsPostBack)
                    AddSortImage();
            }
            else
            {
                DtPgCount.Visible = false;
                 DeleteStopNameConfigDetails();
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    #endregion "Listview Events"

    #region "Private Methods"

    /// <summary>
    /// This method is used to set javascript attributes for buttons.
    /// </summary>
    private void SetJavaScriptAttributres()
    {
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Library_Related));
		ApplyMouseHoverEffect(new List<Button>() { btnBack, btnCancel, btnSave});
        AddSortImage();
    }

    /// <summary>
    /// This method is used set datasource to existing library vendor listView.
    /// </summary>
    private void FillVendorDetails()
    {
        LibraryVendorBL oLibraryVendorBL = new LibraryVendorBL();
        lstvwLibraryVendor.DataSourceID = ObjDSLibraryVendor.ID;
        lstvwLibraryVendor.DataBind();
    }

    /// <summary>
    /// This method is used to populate object of LibraryVendor class.
    /// </summary>
    /// <returns></returns>
    private LibraryVendors PopulateLibraryVendor()
    {
        LibraryVendors oLibraryVendor = new LibraryVendors();
        if (hidMode.Value != "Update")
            oLibraryVendor.VendorId = 0;
        else
            oLibraryVendor.VendorId = Convert.ToInt32(hidVendorId.Value);
        oLibraryVendor.VendorName = txtVendorName.Text;
        oLibraryVendor.Address = txtAddress.Text;
        oLibraryVendor.MobileNumber = txtMobileNo.Text;
        oLibraryVendor.SchoolId = miSchoolId;
        oLibraryVendor.UserId = miUserId;
        oLibraryVendor.InsertDate = System.DateTime.Now.ToString();
        oLibraryVendor.UpdateDate = System.DateTime.Now.ToString();
        return oLibraryVendor;
    }

    /// <summary>
    /// This method is used to set default control values.
    /// </summary>
    private void SetDefaultControls()
    {
        txtVendorName.Focus();
        btnSave.Text = "Save";
        hidMode.Value = "Save";
        lblCheckDependency.Text = string.Empty;
        txtVendorName.Text = string.Empty;
        txtAddress.Text = string.Empty;
        txtMobileNo.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwLibraryVendor.SortDirection.ToString() == "Ascending" || lstvwLibraryVendor.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwLibraryVendor.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwLibraryVendor.SortExpression.ToString();
        else
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        HtmlTableRow oHtmlTableHeaderRow = lstvwLibraryVendor.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to set controls to update library vendor details.
    /// </summary>
    /// <param name="iVendorId"></param>
    private void LoadLibraryVendorDetails(int iVendorId)
    {
        LibraryVendorBL oLibraryVendorBL = new LibraryVendorBL(miSchoolId, iVendorId);
        txtVendorName.Text = oLibraryVendorBL.LibraryVendor.VendorName;
        txtMobileNo.Text = oLibraryVendorBL.LibraryVendor.MobileNumber;
        txtAddress.Text = oLibraryVendorBL.LibraryVendor.Address;
        btnSave.Text = "Update";
        hidMode.Value = "Update";
    }

    /// <summary>
    /// This method is used to delete exisiting library vendor's details as well as it checks dependancy of library vendor with book.
    /// And also checks if at least one library vendor's details has been configured or not.
    /// </summary>
    /// <param name="iVendorId"></param>
    private void DeleteLibraryVendorDetails(int iVendorId, string sVendorName)
    {
        LibraryVendorBL oLibraryVendorBL = new LibraryVendorBL();
        int iVendorCount = oLibraryVendorBL.CountAssociatedLibraryVendorBL(iVendorId);
        if (iVendorCount == 0)
        {
            oLibraryVendorBL.DeleteLibraryVendorBL(iVendorId);
            lblCheckDependency.Text = string.Empty;
        }
        else
            lblCheckDependency.Text = "Vendor '" + sVendorName + "' cannot be deleted since associated with a book.";
        int iTotalStopCount = oLibraryVendorBL.CountTotalLibraryVendorBL(miSchoolId, "", 0, 0);
        if (iTotalStopCount == 0)
            DeleteStopNameConfigDetails();
    }

    /// <summary>
    /// This method is used set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

	/// <summary>
    /// This method is used delete Stop config details.
    /// </summary>
    private void DeleteStopNameConfigDetails()
    {
        int iAcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
        ConfigurationSchoolMasterBL oConfiguration = new ConfigurationSchoolMasterBL();
        oConfiguration.OriginalConfigId = Convert.ToInt32(Constants.SchoolConfigurations.LibraryVendors);
        oConfiguration.SchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
        oConfiguration.AcademicYearId = iAcademicYearId;
        oConfiguration.IsConfigure = Constants.C_YES;
        oConfiguration.InsertedById = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
        oConfiguration.UpdateById = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
        oConfiguration.FinancialYearId = miFinancialYearId;
        oConfiguration.DeleteConfigurationSchoolMaster();
    }
    #endregion "Private Methods"
}
