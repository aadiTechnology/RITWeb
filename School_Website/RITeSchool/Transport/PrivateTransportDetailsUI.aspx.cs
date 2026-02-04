using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;


public partial class PrivateTransportDetailsUI : SchoolBase
{
    #region "EVENTS"

    /// <summary>
    /// This event is used to fill standard combobox,fill traveler's list and set default values.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                FillStandardCombo();
                SetDefaultValues();
                SetJavascriptAttributes();
            }
            ResetLables();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill division combo as per standard selected in standard combo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
            if (iStandardId != 0)
                FillDivisionCombobox(iStandardId);
            else
            {
                ddlDivision.Items.Clear();
                ListItem olstDivision = new ListItem();
                olstDivision.Value = "0";
                olstDivision.Text = "-- All --";
                ddlDivision.Items.Add(olstDivision);
            }
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
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwTravelersDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save transport details for traveler.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SavePrivateTransportDetails();
            lblUpdateSucess.Visible = true;
            lblUpdateSucess.Text = "Transport details saved successfully !!!";
            FillTravelersList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to clear for controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFormControls();
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used enable/disable delete button in list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTravelersDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem) e.Item;
            int iRowId = oCurrentItem.DisplayIndex;
            int iPrivateTransportDetailsId =
                Convert.ToInt32(lstvwTravelersDetails.DataKeys[iRowId]["PrivateTransportDetailsId"]);
            ImageButton obtnDelete = (ImageButton) e.Item.FindControl("imgBtnDelete");
            if (iPrivateTransportDetailsId == 0)
            {
                obtnDelete.Visible = false;
            }
            else
            {
                obtnDelete.Enabled = true;
                obtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill footer property of travelers listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTravelersDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwTravelersDetails.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwTravelersDetails, DtPgCount);
            }
            else
                DtPgCount.Visible = false;
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save or delete the travelers details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTravelersDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName != Constants.S_COMMAND_SORT)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem) e.Item;
                int iRowId = oCurrentItem.DisplayIndex;
                int iPrivateTransportDetailsId =
                    Convert.ToInt32(lstvwTravelersDetails.DataKeys[iRowId]["PrivateTransportDetailsId"]);
                hidPrivateTransportDetailsId.Value = Convert.ToString(iPrivateTransportDetailsId);
                int iUserId = Convert.ToInt32(lstvwTravelersDetails.DataKeys[iRowId]["UserId"]);
                hidUserId.Value = Convert.ToString(iUserId);
                string sUserName = Convert.ToString(lstvwTravelersDetails.DataKeys[iRowId]["UserName"]);
                hidUserName.Value = sUserName;
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                    SetControlsForUpdate(iPrivateTransportDetailsId);
                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                    DeletePrivateTransportDetails(iPrivateTransportDetailsId, sUserName);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region "PRIVATE METHODS"

    /// <summary>
    /// This method is used to set default values for validation summary header, cancel button and default mode.
    /// </summary>
    private void SetDefaultValues()
    {
        btnCancel.Text = "Back";
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidMode.Value = Constants.S_NEW_MODE;
    }
    private void ResetLables()
    {
        if (txtTransportStaff2.Text != "" && txtMobile2.Text != "")
            txtMobile2.Enabled = true;
        else
            txtMobile2.Enabled = false;
    }

    /// <summary>
    /// This method is used fill travelers list.
    /// </summary>
    private void FillTravelersList()
    {
        lstvwTravelersDetails.DataSourceID = ObjDSTravelersDetails.ID;
        lstvwTravelersDetails.DataBind();
    }

    /// <summary>
    /// This method is used to fill standard's combo.
    /// </summary>
    private void FillStandardCombo()
    {
        YearWIseStudentsBL oYearWiseSTudentInfoBL = new YearWIseStudentsBL();
        
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDSStandardCollection, ref ddlStandard,Constants.S_STANDARD_ID_FIELD,Constants.S_STANDARD_NAME_FIELD,"-- All --");

        //Add item into division combobox.
        ListItem olstDivision = new ListItem();
        olstDivision.Value = "0";
        olstDivision.Text = "-- All --";
        ddlDivision.Items.Add(olstDivision);
    }

    /// <summary>
    /// This method is used to fill division's combo.    
    /// </summary>
    /// <param name="aiStandardId"></param>
    private void FillDivisionCombobox(int aiStandardId)
    {
        
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(aiStandardId);
        ControlUtility.FillDropDownList(oDSStandardCollection, ref ddlDivision,Constants.S_DIVISION_ID_FIELD,Constants.S_DIVISION_NAME_FIELD,string.Empty);
    }

    /// <summary>
    /// This method is used to save transport details.
    /// </summary>
    private void SavePrivateTransportDetails()
    {
        PrivateTransportDetailsBL oPrivateTransportDetailsBL = PopulateBL();
        if (hidPrivateTransportDetailsId.Value == "0" && hidMode.Value != Constants.S_EDIT_MODE)
            oPrivateTransportDetailsBL.Insert();
        else
        {
            oPrivateTransportDetailsBL.PrivateTransportDetailsId = Convert.ToInt32(hidPrivateTransportDetailsId.Value);
            oPrivateTransportDetailsBL.Update();
        }
        hidMode.Value = Constants.S_EDIT_MODE;
    }

    /// <summary>
    /// This method creates and returns PrivateTransportDetailsBL object.
    /// </summary>
    /// <returns></returns>
    private PrivateTransportDetailsBL PopulateBL()
    {
        PrivateTransportDetailsBL oPrivateTransportDetailsBL = new PrivateTransportDetailsBL();
        oPrivateTransportDetailsBL.UserId = Convert.ToInt32(hidUserId.Value);
        oPrivateTransportDetailsBL.StopName = txtStopName.Text;
        oPrivateTransportDetailsBL.VehicleNumber = txtVehicleNumber.Text;
        oPrivateTransportDetailsBL.VehicleType = txtVehicleType.Text;
        oPrivateTransportDetailsBL.TransportStaff1 = txtTransportStaff1.Text;
        oPrivateTransportDetailsBL.TransportStaff2 = txtTransportStaff2.Text;
        oPrivateTransportDetailsBL.MobileNo1 = txtMobile1.Text;
        if (txtTransportStaff2.Text == "")
            txtMobile2.Text = string.Empty;
        oPrivateTransportDetailsBL.MobileNo2 = txtMobile2.Text;
        oPrivateTransportDetailsBL.SchoolId = miSchoolId;
        oPrivateTransportDetailsBL.AcademicYearId = miAcademicYearId;
        oPrivateTransportDetailsBL.InsertedById = miUserId;
        return oPrivateTransportDetailsBL;
    }

    /// <summary>
    /// This method is used to clear form controls.
    /// </summary>
    private void ClearFormControls()
    {
        hidUserId.Value = string.Empty;
        hidPrivateTransportDetailsId.Value = string.Empty;
        lblStudentName.Text = string.Empty;
        txtStopName.Text = string.Empty;
        txtVehicleNumber.Text = string.Empty;
        txtVehicleType.Text = string.Empty;
        txtTransportStaff1.Text = string.Empty;
        txtTransportStaff2.Text = string.Empty;
        txtMobile1.Text = string.Empty;
        txtMobile2.Text = string.Empty;
        mltvwContainer.ActiveViewIndex = 0;
        hidMode.Value = Constants.S_NEW_MODE;
    }

    /// <summary>
    /// This method is used to delete transport details for traveler.
    /// </summary>
    /// <param name="iPrivateTransportDetailsId"></param>
    /// <param name="sUserName"></param>
    private void DeletePrivateTransportDetails(int iPrivateTransportDetailsId, string sUserName)
    {
        lblDeleteMsg.Text = "";
        PrivateTransportDetailsBL.Delete(iPrivateTransportDetailsId);
        lblDeleteMsg.Visible = true;
        lblDeleteMsg.Text = "Transport details deleted for " + sUserName + ".";
        FillTravelersList();
    }

    /// <summary>
    /// This method used to set controls for update or add the travelers transport details.
    /// </summary>
    /// <param name="aiPrivateTransportDetailsId"></param>
    private void SetControlsForUpdate(int aiPrivateTransportDetailsId)
    {

        mltvwContainer.ActiveViewIndex = 1;
        lblStudentName.Text = hidUserName.Value;
        if (aiPrivateTransportDetailsId != 0)
        {
            PrivateTransportDetailsBL oPrivateTransportDetailsBL = new PrivateTransportDetailsBL(aiPrivateTransportDetailsId, miSchoolId,miAcademicYearId);
            txtStopName.Text = oPrivateTransportDetailsBL.StopName;
            txtVehicleNumber.Text = oPrivateTransportDetailsBL.VehicleNumber;
            txtVehicleType.Text = oPrivateTransportDetailsBL.VehicleType;
            txtTransportStaff1.Text = oPrivateTransportDetailsBL.TransportStaff1;
            txtTransportStaff2.Text = oPrivateTransportDetailsBL.TransportStaff2;
            txtMobile1.Text = oPrivateTransportDetailsBL.MobileNo1;
            txtMobile2.Text = oPrivateTransportDetailsBL.MobileNo2;
            hidMode.Value = Constants.S_EDIT_MODE;
        }
        ResetLables();
    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwTravelersDetails.SortDirection.ToString() == "Ascending" || lstvwTravelersDetails.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwTravelersDetails.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwTravelersDetails.SortExpression.ToString();
        else
            hidSortExpression.Value = "UserName";
        HtmlTableRow oHtmlTableHeaderRow = lstvwTravelersDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> {btnCancel, btnSave,btnSearch});
        // This Method is used to set default button.
        SetDefaultButton(btnSearch);
    }
    
    private void AddSortImage(HtmlTableRow aoHtmlTableRow, string asSortExpression, string asSortDirection)
    {
        if (asSortExpression.Trim().Equals(""))
            return;
        // Create the sorting image based on the sort direction.
        Image sortImage = new Image();
        sortImage.ID = "sortImage";
        if (asSortDirection == "asc")
        {
            sortImage.ImageUrl = "~/RITeSchool/images/up.gif";
            sortImage.AlternateText = "Ascending Order";
        }
        else if (asSortDirection == "desc")
        {
            sortImage.ImageUrl = "~/RITeSchool/images/down.gif";
            sortImage.AlternateText = "Descending Order";
        }
        // Iterate through the Columns collection to determine the index
        // of the column being sorted.
        foreach (HtmlTableCell oHtmlTableCell in aoHtmlTableRow.Cells)
        {
            asSortExpression = asSortExpression.Replace(" ", "").Replace("asc", "").Replace("desc", "");

            // Iterate through the cells collection to determine the index
            // of the cell being sorted.
            foreach (Control oControl in oHtmlTableCell.Controls)
            {
                LinkButton oLinkButton = oControl as LinkButton;
                if (oLinkButton != null && oLinkButton.CommandArgument == asSortExpression)
                {
                    Image oImage = (Image)oHtmlTableCell.FindControl("sortImage");
                    if (oImage == null)
                    {
                        // Add the image to the appropriate header cell.
                        if (sortImage.ImageUrl != "")
                        {
                            oHtmlTableCell.Controls.Add(sortImage);
                            break;
                        }
                    }
                }
            }
        }
    }        

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillTravelersList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion
}
