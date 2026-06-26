// Class Name       :- TravelerTransportDetailsUI
// Purpose          :- This class is used to manage travelers transport  details.
// Date Of creation :- 20 july 2010
// Author Name      :- Deepak

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Xml;
using SchoolEntities.Transport;
using BusinessLogic;
using Utility;
using System.Threading;
public partial class TravelerTransportDetailsUI : ExportDataTable
{

    #region "CONSTANTS"

    private const string S_ROUTE_TABLE = "Route";
    private const string S_STOP_TABLE = "Stop";
    private const string S_SHIFT_TABLE = "Shift";
    private const string S_VEHICLE_TABLE = "Vehicle";

    #endregion

    bool ShowJourney
    {
        get
        {
            return moSchool == Constants.SchoolId.SNS;
        }
    }

    #region "EVENTS"

    /// <summary>
    /// This event is used to fill user role, routes, stops and shifts comboboxes, set default of page 
    /// and fill travelers list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                if (CheckPreCondition())
                {
                    RenameLabel();
                    FillComboBoxes();
                    ReadQueryString();
                    SetDefaultView();                    
                    FillTravelersList();
                    SetJavaScripAttribute();                    
                }
            }      
           
            SetQueryString();            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

   
    /// <summary>
    /// This event is used to view page wise travelers details.
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
    /// This event is used to capture event selected index of Stop combobox. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStop_SelectedIndexChanged(object sender, EventArgs e)
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
    
    /// <summary>
    /// This event is used to capture event selected index of Shift Combobox. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbShift_SelectedIndexChanged(object sender, EventArgs e)
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

    /// <summary>
    /// This event is used to fill route, stop, shift, vehicle combos of list and also set combo's values to saved values.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTravelersDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = oCurrentItem.DisplayIndex;
                var imgbtnEdit = e.Item.FindControl("imgBtnEdit") as ImageButton;
                var lblName = e.Item.FindControl("lblName") as Label;
                int iUserId = lstvwTravelersDetails.DataKeys[iRowId]["UserId"].ToInt();
                bool bIsHistoryExists = lstvwTravelersDetails.DataKeys[iRowId]["IsHistoryExists"].ToBool();

                string sQueryString = String.Format("UserId={0}&UserName={1}&UserRoleId={2}&StdId={3}&DivId={4}&SearchText={5}&IncludeNotAssociated={6}&RouteId={7}&StopId={8}&ShiftId={9}",
                                                    iUserId, lblName.Text, cmbUserRole.SelectedValue, ddlStandard.SelectedValue, ddlDivision.SelectedValue, txtSearch.Text, chkIncludeAll.Checked,
                                                    cmbRoute.SelectedValue,cmbStop.SelectedValue,cmbShift.SelectedValue);
                imgbtnEdit.Attributes.Add("onclick", "if(!OpenPopup( '../Transport/RouteStopAssignmentPopup.aspx?" + CommonUtility.EncryptQuerystring(sQueryString) + "' )) return false;");

                var btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                ImageButton btnView = e.Item.FindControl("btnView") as ImageButton;
                if (chkIncludeAll.Checked)                
                    btnDelete.Visible = false;                                    
                else
                {
                    string sQryStr = string.Format("UserId={0}&StdId={1}&DivId={2}&SearchText={3}&UserRoleId={4}&RouteId={5}&StopId={6}&ShiftId={7}&IncludeNotAssociated={8}&UserName={9}", iUserId, ddlStandard.SelectedValue, ddlDivision.SelectedValue, txtSearch.Text.TrimAll(), cmbUserRole.SelectedValue, cmbRoute.SelectedValue, cmbStop.SelectedValue, cmbShift.SelectedValue, chkIncludeAll.Checked,lblName.Text);
                    string sEncryptedQry = CommonUtility.EncryptQuerystring(sQryStr);
                   btnDelete.Attributes.Add( "onclick",  "return OpenLeftPopup('" + sEncryptedQry + "');");
                }

                if (bIsHistoryExists)
                {
                    btnView.Visible = true;
                    string sQueryStr = string.Format("UserId={0}&StdId={1}&DivId={2}&SearchText={3}&UserRoleId={4}&RouteId={5}&StopId={6}&ShiftId{7}&IncludeNotAssociated{8}", iUserId, ddlStandard.SelectedValue, ddlDivision.SelectedValue, txtSearch.Text.TrimAll(), cmbUserRole.SelectedValue, cmbRoute.SelectedValue, cmbStop.SelectedValue, cmbShift.SelectedValue, chkIncludeAll.Checked);
                    btnView.PostBackUrl = "../Transport/StudentTransportHistoryUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryStr);
                }
                else
                    btnView.Visible = false;
                
                Label lblMobile2 = e.Item.FindControl("lblMobile2") as Label;
                
                if(lblMobile2 != null && moSchool == Constants.SchoolId.SNS)
                    lblMobile2.Text = lstvwTravelersDetails.DataKeys[e.Item.DisplayIndex]["ClassName"].ToString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill footer property of traveler details listview, add sort image and show/hide save and sms buttons.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTravelersDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            hidRowCount.Value = lstvwTravelersDetails.Items.Count.ToString();
            if (IsPostBack)
                AddSortImage();
            
            if (lstvwTravelersDetails.Items.Count > 0)
            {
                FillPageNoCombo(lstvwTravelersDetails, DtPgCount);
                DataPager oDataPager = lstvwTravelersDetails.FindControl("DtPgDropDown") as DataPager;
                int iCurrentPage = (oDataPager.StartRowIndex / oDataPager.PageSize) + 1;
                //ControlUtility.FillListViewPagerFooter(lstvwTravelersDetails, DtPgCount);
                hidPageNo.Value = iCurrentPage.ToString();         
                if (!chkIncludeAll.Checked)
                    btnSms.Visible = true;

                if (moSchool == Constants.SchoolId.SNS)
                {
                    HtmlTableCell thMobieNo = lstvwTravelersDetails.FindControl("thMobieNo") as HtmlTableCell;
                    if (thMobieNo != null)
                    {
                        Label lblMobile2 = thMobieNo.FindControl("lblMobile2") as Label;
                        if (lblMobile2 != null)
                            lblMobile2.Text = "Class";
                    }
                }
            }
            else            
                DtPgCount.Visible = false;                           
            
            if(chkIncludeAll.Checked || lstvwTravelersDetails.Items.Count == 0)
                btnSms.Visible = false;

            if (lstvwTravelersDetails.Items.Count > 0 && cmbUserRole.SelectedValue != Constants.S_ZERO && cmbUserRole.SelectedValue.ToInt() == Constants.UserRoles.Student.ToInt())
                btnExport.Visible = true;
            else
                btnExport.Visible = false;

            if (ShowJourney)
            {
                HtmlTableCell thShift = lstvwTravelersDetails.FindControl("thShift") as HtmlTableCell;
                if (thShift != null)
                {
                    Label lblShift = thShift.FindControl("lblShift") as Label;
                    if (lblShift != null)
                    {
                        lblShift.Text = "Journey";
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
    /// This event is used to sort the listview of travelers details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTravelersDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            SetSortVariables();
            hidSortExpression.Value = e.SortExpression;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set stop, shift combo of filters as per route selected in route combo of filter.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int iStopId = cmbStop.SelectedValue.ToInt();
            DataSet oDSRouteDetails =
                TravelerTransportDetailsBL.GetStopShiftVehicleForRoute(miSchoolId,miAcademicYearId,Convert.ToInt32(cmbRoute.SelectedValue));
            ControlUtility.FillDropDownList(oDSRouteDetails.Tables[0], ref cmbStop, "StopId", "StopName",Constants.S_SELECT);
            ControlUtility.FillDropDownList(oDSRouteDetails.Tables[1], ref cmbShift, "TransportShiftId","TransportShiftName", Constants.S_SELECT);
            
            if (cmbRoute.SelectedValue != "0")
                EnableStopShiftCombo(true);
            else
                EnableStopShiftCombo(false);

            if (iStopId != 0)
            {
                ListItem oListItem = cmbStop.Items.FindByValue(iStopId.ToString());
                if (oListItem != null)
                    oListItem.Selected = true;
            }

            SetQueryString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used to set default view for each user rile selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbUserRole_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        try
        {
            SetDefaultView();
            SetJavaScripAttribute();
            SetQueryString();            
            btnSms.Visible = false;
            if(cmbUserRole.SelectedValue.ToInt() != Constants.UserRoles.Student.ToInt())
                btnExport.Visible = false; ;
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
            SetQueryString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
        
    /// <summary>
    /// This event is 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// This even is used to export transport details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            TravelerTransportDetailsBL oTravelerTransportDetailsBL = new TravelerTransportDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            DataTable dt = oTravelerTransportDetailsBL.GetTransportDetailsToExport(cmbRoute.SelectedValue.ToInt(), ddlStandard.SelectedValue.ToInt(), ddlDivision.SelectedValue.ToInt(), txtSearch.Text.Trim(),
                cmbStop.SelectedValue.ToInt(), cmbShift.SelectedValue.ToInt(), chkIncludeAll.Checked);
            ExportToExcel("TransportDetails.xls", dt);
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region "PRIVATE METHODS"

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
            hidSortExpression.Value = "DesignationId";
        HtmlTableRow oHtmlTableHeaderRow = lstvwTravelersDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to fill listview's route, stop, shift, vehicle combos.
    /// </summary>
    /// <param name="ddlRouteName"></param>
    /// <param name="ddlStopName"></param>
    /// <param name="ddlShiftName"></param>
    /// <param name="ddlVehicle"></param>
    private void FillListControls(ref DropDownList ddlRouteName, ref DropDownList ddlStopName, ref DropDownList ddlShiftName, ref DropDownList ddlVehicle)
    {
        DataTable oDTRoute = (DataTable)ViewState[S_ROUTE_TABLE];
        DataTable oDTStop = (DataTable)ViewState[S_STOP_TABLE];
        DataTable oDTShift = (DataTable)ViewState[S_SHIFT_TABLE];
        DataTable oDTVehicle = (DataTable)ViewState[S_VEHICLE_TABLE];
        ControlUtility.FillDropDownList(oDTRoute, ref ddlRouteName, "RouteId", "RouteName", Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDTStop, ref ddlStopName, "StopId", "StopName", Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDTShift, ref ddlShiftName, "TransportShiftId", "TransportShiftName", Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDTVehicle, ref ddlVehicle, "VehicleId", "VehicleNumber", Constants.S_SELECT);
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
    /// This method is used to fill traveler details list.
    /// </summary>
    private void FillTravelersList()
    {  
        lstvwTravelersDetails.DataSourceID = ObjDSTravelersDetails.ID;
        lstvwTravelersDetails.DataBind();

        //int iPageSize = Constants.I_GRID_PAGE_COUNT;
        //if (cmbShift.SelectedValue != Constants.S_ZERO)
        //    iPageSize = 100;

        ResetPagerIndex(50);
    }

    /// <summary>
    /// This method is used to reset pager index.
    /// </summary>
    private void ResetPagerIndex(int aiPageSize)
    {
        DataPager dtPager = lstvwTravelersDetails.FindControl("DtPgDropDown") as DataPager;
        if (dtPager != null)
            dtPager.SetPageProperties(Constants.I_ZERO, aiPageSize, true);
    }

    /// <summary>
    /// This method is used to set default view of page.
    /// </summary>
    private void SetDefaultView()
    {
        if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
        {
            ResetFilters();
            EnableFilters(false);
        }
        if (cmbUserRole.SelectedValue != "0")
        {
            SetDefaultFilter(true);
            if (Convert.ToInt32(cmbUserRole.SelectedValue) == Convert.ToInt32(Constants.UserRoles.Student))
            {
                ShowStdDivCombo(true);
                lblSearch.Text = "Name / Reg. No. : ";
                FillStandardCombo();
                if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING && !IsPostBack && !hidStdId.Value.IsNullOrEmpty())
                {
                    ddlStandard.SelectedValue = hidStdId.Value;
                    FillDivisionCombobox(ddlStandard.SelectedValue.ToInt());
                    ddlDivision.SelectedValue = hidDivId.Value;
                }
            }
            else
            {
                ClearStdDivCombo();
                lblSearch.Text = "Name : ";
                ShowStdDivCombo(false);
            }
            ShowSearchControls(true);
            SetControlsVisibility(true);
        }
        else
        {
            txtSearch.Text = string.Empty;
            ShowSearchControls(false);
            ClearStdDivCombo();
            ShowStdDivCombo(false);
            SetControlsVisibility(false);
        }
        //DtPgCount.Visible = DtPgCount.TotalRowCount > Constants.I_GRID_PAGE_COUNT;  
    }

    private void ShowSearchControls(bool abAction)
    {
        tdSearch.Visible = abAction;
        tdlblSearch.Visible = abAction;
    }

    /// <summary>
    /// This method is used to set visibilty of page controls.
    /// </summary>
    /// <param name="abAction"></param>
    private void SetControlsVisibility(bool abAction)
    {
        //btnSms.Visible = abAction;
        DtPgCount.Visible = abAction;
        tblTransportDetails.Visible = abAction;
    }

    /// <summary>
    /// This method is used to clear standard and division combos.
    /// </summary>
    private void ClearStdDivCombo()
    {
        ddlStandard.Items.Clear();
        ddlDivision.Items.Clear();
    }

    /// <summary>
    /// This method sets default filter of route.
    /// </summary>
    /// <param name="abAction"></param>
    private void SetDefaultFilter(bool abAction)
    {
        cmbRoute.Enabled = abAction;
    }

    /// <summary>
    /// This method is used show/hide standard and dividion combos.s
    /// </summary>
    /// <param name="abAction"></param>
    private void ShowStdDivCombo(bool abAction)
    {
        lblStandard.Visible = abAction;
        lblDivision.Visible = abAction;
        ddlStandard.Visible = abAction;
        ddlDivision.Visible = abAction;
    }

    /// <summary>
    /// This method is used to enable/disable route,shift and stop combos of filter.
    /// </summary>
    /// <param name="abAction"></param>
    private void EnableFilters(bool abAction)
    {
        cmbRoute.Enabled = abAction;
        cmbShift.Enabled = abAction;
        cmbStop.Enabled = abAction;
    }

    /// <summary>
    /// This method is used to reset route,shift and stop combos of filter.
    /// </summary>
    private void ResetFilters()
    {
        cmbRoute.SelectedValue = "0";
        cmbShift.SelectedValue = "0";
        cmbStop.SelectedValue = "0";
    }

    /// <summary>
    /// This function is used to read the query string and set appropriate values after closing assignment popup.
    /// </summary>
    private void ReadQueryString()
    {
        if (Request.QueryString.ToString() == Constants.S_EMPTY_STRING)
            return;

        if (!QueryString["UserRoleId"].IsNull())
            cmbUserRole.SelectedValue = QueryString["UserRoleId"].ToString();

        if (!QueryString["StdId"].IsNull() && QueryString["StdId"].ToString() != string.Empty && QueryString["StdId"].ToString() != Constants.S_ZERO)
            hidStdId.Value = QueryString["StdId"];

        if (!QueryString["DivId"].IsNull() && QueryString["DivId"].ToString() != string.Empty)
            hidDivId.Value = QueryString["DivId"];

        if (!QueryString["SearchText"].IsNull() && QueryString["SearchText"].ToString() != string.Empty)
            txtSearch.Text = QueryString["SearchText"].ToString();

        if (!QueryString["IncludeNotAssociated"].IsNull())
            chkIncludeAll.Checked = QueryString["IncludeNotAssociated"].ToBool();

        if (!QueryString["RouteId"].IsNull() && QueryString["RouteId"].ToString() != string.Empty && QueryString["RouteId"].ToString() != Constants.S_ZERO)
        {
            cmbRoute.SelectedValue = QueryString["RouteId"].ToString();
            cmbRoute_SelectedIndexChanged(null, null);
        }

        if (!QueryString["StopId"].IsNull() && QueryString["StopId"].ToString() != string.Empty)
            cmbStop.SelectedValue = QueryString["StopId"].ToString();

        if (!QueryString["ShiftId"].IsNull() && QueryString["ShiftId"].ToString() != string.Empty)
            cmbShift.SelectedValue = QueryString["ShiftId"].ToString();
    }

    /// <summary>
    /// This method is used to fill user role, route,shift and stop combos of filter.
    /// </summary>
    private void FillComboBoxes()
    {
        DataSet oDSComboData = TravelerTransportDetailsBL.GetUserRoleRouteStopShift(miSchoolId,miAcademicYearId);
        if (oDSComboData != null && oDSComboData.Tables.Count > 0)
        {
            ControlUtility.FillDropDownList(oDSComboData.Tables[0], ref cmbUserRole, "User_Role_Id", "User_Role_Name", Constants.S_SELECT);
            ControlUtility.FillDropDownList(oDSComboData.Tables[1], ref cmbRoute, "RouteId", "RouteName", Constants.S_SELECT);
            ControlUtility.FillDropDownList(oDSComboData.Tables[2], ref cmbStop, "StopId", "StopName", Constants.S_SELECT);
            ControlUtility.FillDropDownList(oDSComboData.Tables[3], ref cmbShift, "TransportShiftId", "TransportShiftName", Constants.S_SELECT);

            ViewState.Add(S_ROUTE_TABLE, oDSComboData.Tables[1]);
            ViewState.Add(S_STOP_TABLE, oDSComboData.Tables[2]);
            ViewState.Add(S_SHIFT_TABLE, oDSComboData.Tables[3]);
            ViewState.Add(S_VEHICLE_TABLE, oDSComboData.Tables[4]);
        }
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

    private void SetDefaultComboBoxItem(DropDownList cmb)
    {
        cmb.Items.Clear();
        cmb.Items.Add(new ListItem(Constants.S_SELECT, Constants.I_ZERO.ToString()));
        cmb.Enabled = false;
    }

    private void FillVehicleComboInList(DropDownList ddlStopName, DropDownList ddlShiftName, DropDownList ddlRouteName, DropDownList ddlVehicle)
    {
        if (ddlStopName.SelectedValue != "0" && ddlRouteName.SelectedValue != "0" && ddlShiftName.SelectedValue != "0")
        {
            DataTable oDSRouteDetails = TravelerTransportDetailsBL.GetVehicleForRouteShiftStop(miSchoolId,
                                                                                              miAcademicYearId,
                                                                                              Convert.ToInt32(ddlRouteName.SelectedValue), Convert.ToInt32(ddlStopName.SelectedValue),
                                                                                               Convert.ToInt32(ddlShiftName.SelectedValue));

            ControlUtility.FillDropDownList(oDSRouteDetails, ref ddlVehicle, "VehicleId", "VehicleNumber", Constants.S_SELECT);
            ddlVehicle.Enabled = true;
        }
        else
        {
            ddlVehicle.Items.Clear();
            ListItem olstDivision = new ListItem();
            olstDivision.Value = "0";
            olstDivision.Text = "-- Select --";
            ddlVehicle.Items.Add(olstDivision);
            ddlVehicle.Enabled = false;
        }
        AddSortImage();
    }    

    /// <summary>
    /// This method is used to check pre-condition to configure association.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.TransportManagment);
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
    /// This method used to hide form controls.
    /// </summary>
    private void HideControls()
    {
        trMandetory.Visible = false;
        trFilters.Visible = false;
        trlstvwTransport.Visible = false;
        DtPgCount.Visible = false;
        //btnSms.Visible = false;
    }
    
    /// <summary>
    /// This method is used to enable/disable shift and stop combos.
    /// </summary>
    /// <param name="abAction"></param>
    private void EnableStopShiftCombo(bool abAction)
    {
        cmbShift.Enabled = abAction;
        cmbStop.Enabled = abAction;
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScripAttribute()
    {
        // This method is used to set default properties to controls. 
        SetDefaultButton(btnSearch);
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ApplyMouseHoverEffect(new List<Button> {btnSms, btnSearch});        
    }

    /// <summary>
    /// This function is used to set query string to send sms.
    /// </summary>
    private void SetQueryString()
    {        
        string sQueryString = "UserRoleId=" + cmbUserRole.SelectedValue + "&RouteId=" + cmbRoute.SelectedValue + "&StopId=" + cmbStop.SelectedValue +
                                    "&TransportShiftId=" + cmbShift.SelectedValue + "&StandardId=" + ddlStandard.SelectedValue + "&DivisionId=" + ddlDivision.SelectedValue;
        sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
        hidQueryString.Value = sQueryString;
        btnSms.Attributes.Add("onclick", "if(!OpenSendSMSPopup('" + sQueryString + "')) return false;"); 
    }

    /// <summary>
    /// This method is used fill the datapager dropdown list in the list view.
    /// Pager control name should be same as defined here.
    /// e.g. DtPgDropDown is the datapager name which contains the drop down list.
    /// Same for drop down list in the pager control as well as label
    /// </summary>
    public static void FillPageNoCombo(ListView oListView, DataPager oPgCntDataPager)
    {
        DataPager oDataPager = oListView.FindControl("DtPgDropDown") as DataPager;
        HtmlTableRow otblDataPager = oListView.FindControl("trDataPager") as HtmlTableRow;
        otblDataPager.Visible = false;
        oPgCntDataPager.Visible = false;
        int iCurrentPage = (oDataPager.StartRowIndex / oDataPager.PageSize) + 1;
        int iTotalPages = oDataPager.TotalRowCount / oDataPager.PageSize;
        if (iTotalPages * oDataPager.PageSize < oDataPager.TotalRowCount)
            iTotalPages += 1;

        if (iTotalPages > 1)
        {
            otblDataPager.Visible = true;
            oPgCntDataPager.Visible = true;
            //Populate the DropDownList if needed
            DropDownList ddlCount = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
            //ddlCount.Attributes.Add("onchange", "if(!MessageAboutUpload('" + ddlCount.ClientID + "')){return false;}");

            if (ddlCount.Items.Count == 0)
            {
                //Add a list item for each page
                for (int iddlCount = 1; iddlCount <= iTotalPages; iddlCount++)
                    ddlCount.Items.Add(iddlCount.ToString());

                //Set the DDL to the appropriate page value
                ddlCount.Items.FindByValue(iCurrentPage.ToString()).Selected = true;
                Label oLabel = (oDataPager.Controls[0].FindControl("CurrentPageLabel")) as Label;
                oLabel.Font.Bold = true;
                oLabel.Text = "Page " + iCurrentPage + " of " + iTotalPages;
            }
        }
    }

    private void RenameLabel()
    {
        if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            spnShiftHeader.InnerText = "Journey";
    }

    #endregion
}
