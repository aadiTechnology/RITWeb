// File Name  : RouteDetailsUI.aspx.cs
// Created By : Deepak
// Date       : 13 July 2010
//Description :This class is used to add, eidt, delete route details and also assocaite stops for route. 

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Transport;
using Utility;
using BusinessLogic.TransportBL;
using System.Configuration;

public partial class RouteShiftTimingUI : SchoolBase
{

    #region Property(s)

    private bool IsOverrideMode
    {
        get
        {
            return hidCategoryId.Value == Constants.S_ONE;
        }
    } 

    #endregion

    #region "EVENTS"

    /// <summary>
    /// This event used to set javascript attributes,and depending upon precondition; 
    /// fill route,shift vehicles comboboxes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                if (CheckPreCondition())
                {
                    ReadQueryString();
                    SetCancelButtonState();
                    FillControlsForEditMode();
                    RenameLabel();
                    FillComboBoxes();
                    FillStopTimingListView();
                    valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                }
            }
            cmbRouteName.Focus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to save route-shift timings.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                SaveRouteShiftVehicleTiming();
                if (QueryString[Constants.S_IS_CONFIGURED] != Constants.S_YES && !IsOverrideMode)
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.RouteShiftTimmingDetails));

                if (IsOverrideMode)
                {
                    string sQueryString = "Name=" + hidName.Value + "&VehicleNo=" + hidVehicleNo.Value + "&RouteName=" + hidRouteName.Value + "&JourneyName=" + hidJourneyName.Value;
                    MasterPage oMaster = this.Master as MasterPage;
                    oMaster.RedirectToNextPage("TransportOverrideConfigDetailsUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString));
                }
                else
                {
                    FillStopTimingListView();
                    lblUpdateSucess.Visible = true;
                    lblUpdateSucess.Text = "Route timing updated successfully !!! ";
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used to fill stop-timngs list depending on selected values in combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbRouteName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillStopTimingListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This evemt is used to fill journeys.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlJourneyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillShiftDropDown();

            int ShiftId = 0;
            if (ddlJourneyType.SelectedValue == Constants.S_ONE && hidPickupShiftId.Value != Constants.S_ZERO)
                ShiftId = hidPickupShiftId.Value.ToInt();
            else if (ddlJourneyType.SelectedValue == Constants.S_TWO && hidDropShiftId.Value != Constants.S_ZERO)
                ShiftId = hidDropShiftId.Value.ToInt();

            if (ShiftId != 0)
            {
                ListItem oShift = cmbShift.Items.FindByValue(ShiftId.ToString());
                if (oShift != null)
                {
                    oShift.Selected = true;
                    cmbShift_SelectedIndexChanged(cmbShift, null);
                }
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used to fill stop-timngs list depending on selected values in combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbShift_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillStopTimingListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used to fill stop-timngs list depending on selected values in combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbVehicle_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillRoutesJourneyDetails();
        } 
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete route-shift timings if it is not associated to travelers.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string sRouteShiftTimingIDs = GetRouteShiftVehicleIDs();
            int iRowCount = 0;
            char Message=' ';
            RouteShiftTimingDetailsBL.DeleteRouteShiftVehicleTiming(sRouteShiftTimingIDs, out iRowCount,out Message);
            if (Message=='N')
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Route-Shift-Timing Details can not be deleted since associated with traveler.";
            }
            else
            {
                if (iRowCount == 0)
                    DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.RouteShiftTimmingDetails));
                FillStopTimingListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle pick up / drop case.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStopsTimeDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (moSchool == Constants.SchoolId.SNS)
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    if (ddlJourneyType.SelectedValue == Constants.S_ONE)
                    {
                        HtmlTableCell tdDropTime = e.Item.FindControl("tdDropTime") as HtmlTableCell;
                        if (tdDropTime != null)
                            tdDropTime.Visible = false;

                        HtmlTableCell tdPickupTime = e.Item.FindControl("tdPickupTime") as HtmlTableCell;
                        if (tdPickupTime != null)
                            tdPickupTime.Visible = true;
                    }
                    else if (ddlJourneyType.SelectedValue == Constants.S_TWO)
                    {
                        HtmlTableCell tdPickupTime = e.Item.FindControl("tdPickupTime") as HtmlTableCell;
                        if (tdPickupTime != null)
                            tdPickupTime.Visible = false;

                        HtmlTableCell tdDropTime = e.Item.FindControl("tdDropTime") as HtmlTableCell;
                        if (tdDropTime != null)
                            tdDropTime.Visible = true;
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
    /// This event is used to validate name and dates.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void NameAndDates_Validate(object sender, ServerValidateEventArgs e)
    {
        try
        {
            RouteShiftTimingDetailsBL obj = new RouteShiftTimingDetailsBL();
            string sMessage = obj.ValidateNameAndDates(miSchoolId, miAcademicYearId, hidId.Value.ToInt(), txtName.Text.Trim(), txtStartDate.Text.ToDateTime(), txtEndDate.Text.ToDateTime(), cmbRouteName.SelectedValue.ToInt(), cmbVehicle.SelectedValue.ToInt(), cmbShift.SelectedValue.ToInt());

            if (sMessage == string.Empty)
                e.IsValid = true;
            else
            {
                CustomValidator cv = sender as CustomValidator;
                cv.ErrorMessage = sMessage;
                e.IsValid = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is sued to dipslay fields as per selection of type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbTypes.SelectedValue == "-1")
            {
                trDates.Visible = true;
                trWeekdays.Visible = false;
            }
            else
            {
                trDates.Visible = false;
                trWeekdays.Visible = true;
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
    /// This Method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> {btnCancel, btnSave,btnDelete});
        btnSave.Attributes["onclick"] = "ResetUpdateLbl()";
        btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");        
    }
    
    /// <summary>
    /// This method is sued to set cancel button state.
    /// </summary>
    private void SetCancelButtonState()
    {
        string sQueryString = "Name=" + hidName.Value + "&VehicleNo=" + hidVehicleNo.Value + "&RouteName=" + hidRouteName.Value + "&JourneyName=" + hidJourneyName.Value;
        if (QueryString["CategoryId"] != null && QueryString["CategoryId"].ToString() != string.Empty)
            btnCancel.PostBackUrl = "TransportOverrideConfigDetailsUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString);
        else
            btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Transport_Releted));
    } 

    /// <summary>
    /// This method is used to fill stop-timings list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>   
    private void FillStopTimingListView(int iRouteId, int iTransportShiftId, int iVehicleId)
    {
        lstvwStopsTimeDetails.DataSource = RouteShiftTimingDetailsBL.GetRouteShiftTimingDetails(miSchoolId,miAcademicYearId,iRouteId, iTransportShiftId, iVehicleId, (hidCategoryId.Value == Constants.S_ONE?true : false), hidId.Value.ToInt());
        lstvwStopsTimeDetails.DataBind();
        if (lstvwStopsTimeDetails.Items.Count > 0)
        {
            hidRowCount.Value = Convert.ToString(lstvwStopsTimeDetails.Items.Count);
            if (Convert.ToInt32(lstvwStopsTimeDetails.DataKeys[0]["miRouteTimingDetailsId"]) != 0 && !IsOverrideMode)
                btnDelete.Visible = true;
            else
                btnDelete.Visible = false;

            if (moSchool == Constants.SchoolId.SNS)
            { 
                if (ddlJourneyType.SelectedValue == Constants.S_ONE)
                {
                    HtmlTableCell thDropTime = lstvwStopsTimeDetails.FindControl("thDropTime") as HtmlTableCell;
                    if (thDropTime != null)
                        thDropTime.Visible = false;

                    HtmlTableCell thPickUpTime = lstvwStopsTimeDetails.FindControl("thPickUpTime") as HtmlTableCell;
                    if (thPickUpTime != null)
                        thPickUpTime.Visible = true;
                }
                else if (ddlJourneyType.SelectedValue == Constants.S_TWO)
                {
                    HtmlTableCell thPickUpTime = lstvwStopsTimeDetails.FindControl("thPickUpTime") as HtmlTableCell;
                    if (thPickUpTime != null)
                        thPickUpTime.Visible = false;

                    HtmlTableCell thDropTime = lstvwStopsTimeDetails.FindControl("thDropTime") as HtmlTableCell;
                    if (thDropTime != null)
                        thDropTime.Visible = true;
                }
            }
        }
    }

    /// <summary>
    /// This method is used to fill route, shift, vehicle comboboxes.
    /// </summary>
    /// <returns></returns>
    private void FillComboBoxes()
    {
        DataSet oDSRouteShiftVehicle = RouteShiftTimingDetailsBL.GetRoutesShiftsVehicles(miSchoolId, miAcademicYearId);
        if (oDSRouteShiftVehicle != null && oDSRouteShiftVehicle.Tables.Count > 0)
        {
            ControlUtility.FillDropDownList(oDSRouteShiftVehicle.Tables[0], ref cmbRouteName, "RouteId", "RouteName", Constants.S_SELECT);
            ControlUtility.FillDropDownList(oDSRouteShiftVehicle.Tables[1], ref cmbShift, "TransportShiftId", "TransportShiftName", Constants.S_SELECT);
            ControlUtility.FillDropDownList(oDSRouteShiftVehicle.Tables[2], ref cmbVehicle, "VehicleId", "VehicleNumber", Constants.S_SELECT);
        }
    }

    /// <summary>
    /// This method is used to fill journey dropdown.
    /// </summary>
    private void FillShiftDropDown()
    {
        DataSet DSShift = RouteShiftTimingDetailsBL.GetShiftValues(miSchoolId, miAcademicYearId, ddlJourneyType.SelectedValue.ToInt());
        if (DSShift != null && DSShift.Tables.Count > 0)
        {
            ControlUtility.FillDropDownList(DSShift.Tables[0], ref cmbShift, "TransportShiftId", "TransportShiftName", Constants.S_SELECT);
        }
    }

    /// <summary>
    /// This method is used to save route-shift-timing details.
    /// </summary>
    /// <returns></returns>
    private void SaveRouteShiftVehicleTiming()
    {
        RouteShiftTimingDetailsBL oRouteShiftTimingDetailsBL = PopulateRouteShiftTimingDetailsBL();
        oRouteShiftTimingDetailsBL.SaveRouteShiftVehicleTiming();

        if (ConfigurationManager.AppSettings["TransportExternalDBName"] != null && ConfigurationManager.AppSettings["TransportExternalDBName"].ToString() != string.Empty)
        {
            string sDBName = ConfigurationManager.AppSettings["reportdatabasename"].ToString();
            string sTransportDBName = ConfigurationManager.AppSettings["TransportExternalDBName"].ToString();
            if (oRouteShiftTimingDetailsBL.CategoryId == 1)
            {
                TransferTransportDetailsBL oTransferTransportDetailsBL = new TransferTransportDetailsBL(miSchoolId, sDBName, sTransportDBName);
                oTransferTransportDetailsBL.UpdateJourneyOverrideDetails();
            }
            else
            {
                TransferTransportDetailsBL oTransferTransportDetailsBL = new TransferTransportDetailsBL(miSchoolId, sDBName, sTransportDBName);
                oTransferTransportDetailsBL.UpdateJourneyDetails();
            }
        }
    }

    /// <summary>
    /// This method create and returns RouteShiftTimingDetailsBL object.
    /// </summary>
    /// <returns></returns>
    private RouteShiftTimingDetailsBL PopulateRouteShiftTimingDetailsBL()
    {
        RouteShiftTimingDetailsBL oRouteShiftTimingDetailsBL = new RouteShiftTimingDetailsBL();
        oRouteShiftTimingDetailsBL.TransportShiftId = Convert.ToInt32(cmbShift.SelectedValue);
        oRouteShiftTimingDetailsBL.VehicleId = Convert.ToInt32(cmbVehicle.SelectedValue);
        oRouteShiftTimingDetailsBL.InsertedById = miUserId;
        oRouteShiftTimingDetailsBL.RouteShiftTimingDetailsXML = GetRouteShiftTimingXML();
        oRouteShiftTimingDetailsBL.Id = hidId.Value.ToInt();
        oRouteShiftTimingDetailsBL.Name = txtName.Text.Trim();
        oRouteShiftTimingDetailsBL.CategoryId = hidCategoryId.Value.ToInt();
        oRouteShiftTimingDetailsBL.TypeId = cmbTypes.SelectedValue.ToInt();

        if (cmbTypes.SelectedValue == "-1" && trOverrideDetails.Visible == true)
        {
            oRouteShiftTimingDetailsBL.StartDate = txtStartDate.Text.ToDateTime();
            oRouteShiftTimingDetailsBL.EndDate = txtEndDate.Text.ToDateTime();
            oRouteShiftTimingDetailsBL.Weekdays = string.Empty;
        }
        else
        {
            oRouteShiftTimingDetailsBL.StartDate = DateTime.MinValue;
            oRouteShiftTimingDetailsBL.EndDate = DateTime.MinValue;
            oRouteShiftTimingDetailsBL.Weekdays = GetSelectedWeekdays();
        }

        return oRouteShiftTimingDetailsBL;
    }

    /// <summary>
    /// This method is used to return selected weekdays.
    /// </summary>
    /// <returns></returns>
    private string GetSelectedWeekdays()
    {
        StringBuilder sb = new StringBuilder();
        for (int iIndex = 0; iIndex< chkWeekdays.Items.Count;iIndex++)
        {
            if (chkWeekdays.Items[iIndex].Selected)
                sb.Append("," + chkWeekdays.Items[iIndex].Value);
        }

        if (sb.Length > 0)
            return sb.ToString().Substring(1);
        else
            return string.Empty;
    }
        
    /// <summary>
    /// This method is used to generate route-shift-timing xml.
    /// </summary>
    /// <returns></returns>
    private string GetRouteShiftTimingXML()
    {
        const string S_ELEMENT = "element";
        string sAttribute;
        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement oRoot = oDoc.CreateElement("RouteShiftTiming");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "RouteShiftTiming", "");
        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount <= lstvwStopsTimeDetails.Items.Count - 1; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwStopsTimeDetails.Items[iRowCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            int iRouteStopId = Convert.ToInt32(lstvwStopsTimeDetails.DataKeys[iRowId]["miRouteStopId"]);
            int iRouteShiftVehicleDetailsId = Convert.ToInt32(lstvwStopsTimeDetails.DataKeys[iRowId]["miRouteShiftVehicleDetailsId"]);
            int iRouteTimingDetailsId = Convert.ToInt32(lstvwStopsTimeDetails.DataKeys[iRowId]["miRouteTimingDetailsId"]);
            TextBox oTxtSortOrdre = (TextBox)oCurrentItem.FindControl("txtOrder");
            oTxtSortOrdre.Attributes.Add("onkeyup", "OnGridKeyUpNumber(this,0,false,event);");
            TextBox oTxtPickupTime = (TextBox)oCurrentItem.FindControl("txtPickupTime");
            oTxtPickupTime.Attributes.Add("onkeyup", "OnGridKeyUpNumber(this,0,false,event);");
            TextBox oTxtDropTime = (TextBox)oCurrentItem.FindControl("txtDropTime");
            oTxtDropTime.Attributes.Add("onkeyup", "OnGridKeyUpNumber(this,0,false,event);");
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "RouteShiftTiming", "");

            sAttribute = "RouteStopId";
            XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = iRouteStopId.ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "RouteShiftVehicleDetailsId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = iRouteShiftVehicleDetailsId.ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "RouteTimingDetailsId";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = iRouteTimingDetailsId.ToString();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "SortOrder";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = oTxtSortOrdre.Text.Trim();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "PickUpTime";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = oTxtPickupTime.Text.Trim();
            oXmlNode.Attributes.Append(oAttr);

            sAttribute = "DropTime";
            oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = oTxtDropTime.Text.Trim();
            oXmlNode.Attributes.Append(oAttr);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);
        }
        // Add the root node to document element.         
        oRoot.AppendChild(oXmlRootNode);
        // return the string generated.
        return oRoot.InnerXml;
    }

    /// <summary>
    /// This method is used to generate route-shift-vehicle xml.
    /// </summary>
    /// <returns></returns>
    private string GetRouteShiftVehicleIDs()
    {
        const string S_ELEMENT = "element";
        string sAttribute;
        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement oRoot = oDoc.CreateElement("RouteShiftVehicle");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "RouteShiftVehicle", "");
        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount <= lstvwStopsTimeDetails.Items.Count - 1; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwStopsTimeDetails.Items[iRowCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            int iRouteShiftVehicleDetailsId = Convert.ToInt32(lstvwStopsTimeDetails.DataKeys[iRowId]["miRouteShiftVehicleDetailsId"]);
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "RouteShiftVehicle", "");

            sAttribute = "RouteShiftVehicleDetailsId";
            XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
            oAttr.Value = iRouteShiftVehicleDetailsId.ToString();
            oXmlNode.Attributes.Append(oAttr);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);
        }
        // Add the root node to document element.         
        oRoot.AppendChild(oXmlRootNode);
        // return the string generated.
        return oRoot.InnerXml;
    }

    /// <summary>
    /// This method is used to fill stop-timing details.
    /// </summary>
    private void FillStopTimingListView()
    {
        if (cmbRouteName.SelectedValue != Constants.S_ZERO && cmbShift.SelectedValue != Constants.S_ZERO && cmbVehicle.SelectedValue != Constants.S_ZERO)
        {
            ShowHideControls(true);
            btnCancel.Text = "Cancel";
            FillStopTimingListView(Convert.ToInt32(cmbRouteName.SelectedValue), Convert.ToInt32(cmbShift.SelectedValue), Convert.ToInt32(cmbVehicle.SelectedValue));
        }
        else
        {
            ShowHideControls(false);

            btnDelete.Visible = false;
            btnCancel.Text = "Back";
        }
    }

    /// <summary>
    /// This method is used to show/hide fields.
    /// </summary>
    /// <param name="abAction"></param>
    private void ShowHideControls(bool abAction)
    {
        divContainer.Visible = abAction;
        btnSave.Visible = abAction;
    }
    
    /// <summary>
    /// This method is used to check pre-condition to configure association.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.RouteShiftTimmingDetails);
        if (!sLinks.Equals(String.Empty))
        {
            divErr.InnerHtml = sLinks;
            HideControls();
        }
        else
        {
            divErr.Visible = false;
            bReturn = true;
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to hide the form controls.
    /// </summary>
    /// <returns></returns>
    private void HideControls()
    {
        tblRouteTimingDetails.Visible = false;
        tblStopTimeDetails.Visible = false;
        btnSave.Visible = false;
        btnDelete.Visible = false;
        btnCancel.Text = "Back";
    }

    /// <summary>
    /// This method is used to set hourney text.
    /// </summary>
    private void RenameLabel()
    {
        if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            lblJourney.Text = "Journey : ";
    }

    /// <summary>
    /// This method is used to fill Vehicle related Details
    /// </summary>
    private void FillRoutesJourneyDetails()
    {
        ddlJourneyType.ClearSelection();
        cmbRouteName.ClearSelection();
        cmbShift.ClearSelection();
        hidPickupShiftId.Value = Constants.S_ZERO;
        hidDropShiftId.Value = Constants.S_ZERO;
        RouteShiftTimingDetailsBL oRouteShiftTimingDetailsBL = new RouteShiftTimingDetailsBL();
        DataTable oDT = oRouteShiftTimingDetailsBL.GetRoutesJourneyDetails(miSchoolId, miAcademicYearId, cmbVehicle.SelectedValue.ToInt());

        if (oDT.Rows.Count > 0 && oDT.Rows[0][0] != DBNull.Value)
        {
            hidPickupShiftId.Value = oDT.Rows[0]["TransportShiftId"].ToString();
            hidDropShiftId.Value = oDT.Rows[0]["DropTransportShiftId"].ToString();
            
            ListItem oListItem = cmbRouteName.Items.FindByValue(oDT.Rows[0]["RouteId"].ToString());
            if (oListItem != null)
            {
                oListItem.Selected = true;
                cmbRouteName_SelectedIndexChanged(cmbRouteName, null);

                if (ddlJourneyType.SelectedValue == Constants.S_ZERO)
                    ddlJourneyType.SelectedValue = Constants.S_ONE;

                FillShiftDropDown();

                ListItem oShift = cmbShift.Items.FindByValue(oDT.Rows[0]["TransportShiftId"].ToString());
                if (oShift != null)
                {
                    oShift.Selected = true;
                    cmbShift_SelectedIndexChanged(cmbShift, null);
                }
            }
        }
    }

    /// <summary>
    /// This method is used to fill override related fields in edit mode.
    /// </summary>
    private void FillControlsForEditMode()
    {
        if (hidId.Value != Constants.S_ZERO)
        {
            RouteShiftTimingDetailsBL oRouteShiftTimingDetailsBL = new RouteShiftTimingDetailsBL();
            RouteShiftTimingOverrideDetails oRouteShiftTimingOverrideDetails = oRouteShiftTimingDetailsBL.Get(hidId.Value.ToInt());

            txtName.Text = oRouteShiftTimingOverrideDetails.Name;
            cmbTypes.SelectedValue = oRouteShiftTimingOverrideDetails.TypeId.ToString();

            if (oRouteShiftTimingOverrideDetails.TypeId == -1)
            {
                trDates.Visible = true;
                trWeekdays.Visible = false;
                txtStartDate.Text = oRouteShiftTimingOverrideDetails.StartDate.ToString(Constants.S_DATE_FORMAT);
                txtEndDate.Text = oRouteShiftTimingOverrideDetails.EndDate.ToString(Constants.S_DATE_FORMAT);
            }
            else
            {
                trWeekdays.Visible = true;
                trDates.Visible = false;
                txtStartDate.Text = string.Empty;
                txtEndDate.Text = string.Empty;

                if (oRouteShiftTimingOverrideDetails.WeekdayIds != string.Empty)
                {
                    int iSelectedCount = 0;
                    List<string> lstWeekDays = oRouteShiftTimingOverrideDetails.WeekdayIds.Split(',').ToList();
                    for (int iIndex = 0; iIndex < chkWeekdays.Items.Count; iIndex++)
                    {
                        if (lstWeekDays.Contains(chkWeekdays.Items[iIndex].Value))
                        {
                            chkWeekdays.Items[iIndex].Selected = true;
                            iSelectedCount++;
                        }
                        else
                            chkWeekdays.Items[iIndex].Selected = false;
                    }
                }
            }

            cmbVehicle.SelectedValue = oRouteShiftTimingOverrideDetails.VehicleId.ToString();
            cmbVehicle_SelectedIndexChanged(cmbVehicle, null);
            cmbRouteName.SelectedValue = oRouteShiftTimingOverrideDetails.RouteId.ToString();
            cmbRouteName_SelectedIndexChanged(cmbRouteName, null);
            ddlJourneyType.SelectedValue = oRouteShiftTimingOverrideDetails.JourneyTypeId.ToString();
            ddlJourneyType_SelectedIndexChanged(ddlJourneyType, null);
            cmbShift.SelectedValue = oRouteShiftTimingOverrideDetails.JourneyId.ToString();
            cmbShift_SelectedIndexChanged(cmbShift, null);
        }
    }

    /// <summary>
    /// This method is used to read querystring values and set to it hidden fields.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["CategoryId"] != null && QueryString["CategoryId"].ToString() != string.Empty)
        {
            hidCategoryId.Value = QueryString["CategoryId"].ToString();

            if (QueryString["Id"] != null && QueryString["Id"].ToString() != string.Empty)
                hidId.Value = QueryString["Id"].ToString();
            else
                hidId.Value = Constants.S_ZERO;

            if (QueryString["RouteName"] != null && QueryString["RouteName"].ToString() != string.Empty)
                hidRouteName.Value = QueryString["RouteName"].ToString();

            if (QueryString["VehicleNo"] != null && QueryString["VehicleNo"].ToString() != string.Empty)
                hidVehicleNo.Value = QueryString["VehicleNo"].ToString();

            if (QueryString["JourneyName"] != null && QueryString["JourneyName"].ToString() != string.Empty)
                hidJourneyName.Value = QueryString["JourneyName"].ToString();

            if (QueryString["Name"] != null && QueryString["Name"].ToString() != string.Empty)
                hidName.Value = QueryString["Name"].ToString();

            trOverrideDetails.Visible = true;
            trOverrideDetailsHeader.Visible = true;
            trHR.Visible = true;            
            reqValName.Enabled = true;            
            cstValWeekdays.Enabled = true;
            cstValDates.Enabled = true;
            cstValEndDate.Enabled = true;

            FillWeekdays();
        }
        else
        {
            hidId.Value = Constants.S_ZERO;
            hidCategoryId.Value = Constants.S_TWO;
        }
    }

    /// <summary>
    /// This method is used to fill weekday checkboxlist.
    /// </summary>
    private void FillWeekdays()
    {
        WeekDaysMasterBL oWeekDaysMasterBL = new WeekDaysMasterBL();
        DataTable oDTWeekdays = oWeekDaysMasterBL.GetConfiguredWeekDays(miSchoolId, miAcademicYearId);
        oDTWeekdays.Columns.Add("Name");

        //for (int iIndex = 0; iIndex < oDTWeekdays.Rows.Count; iIndex++)
        //{
        //    oDTWeekdays.Rows[iIndex]["Name"] = oDTWeekdays.Rows[iIndex]["Weekday_Name"].ToString().Substring(0, 3);
        //}

        ListSource.FillCheckBoxList(oDTWeekdays, chkWeekdays, "Weekday_Name", "Original_Weekdays_Id");
    }

    #endregion   
}
