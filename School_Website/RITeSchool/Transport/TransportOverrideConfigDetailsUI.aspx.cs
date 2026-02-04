/* Class Name - TransportOverrideConfigDetailsUI
 * Created By - Vishakha
 * Created On - 06-Sep-2023
 * Description - This class is used to display transport Override configuration details.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using Utility;
using BusinessLogic.TransportBL;
using SchoolEntities.Transport;
using System.Text;
using System.Configuration;

public partial class TransportOverrideConfigDetailsUI : SchoolBase
{
    #region Constant(s)
    
    private const string S_COMMAND_DELETE = "DeleteCommand";
    private const string S_COMMAND_UPDATE = "UpdateCommand";
    private const string S_DELETE_MSG = "Transport configuration override details deleted successfully!!!";

    #endregion

    #region Data member(s)

    private TransportOverrideConfigDetailsBL moTransportOverrideConfigDetailsBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to show sorting image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty)
            {
                hidSortExpression.Value = "StartDate";
                hidSortDirection.Value = Constants.S_DESCENDING;
            }

            AddSortImage(lstvwTransportOverrideConfigDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to show override details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moTransportOverrideConfigDetailsBL = new TransportOverrideConfigDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                SetQueryString();
                FillOverrideDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to edit, delete.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTransportOverrideConfigDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                int iId = Convert.ToInt32(lstvwTransportOverrideConfigDetails.DataKeys[iRowId]["Id"]);

                if (e.CommandName == S_COMMAND_DELETE)
                {
                    Delete(iId);
                    ResetFields();
                    FillOverrideDetails();
                }
                else if (e.CommandName == S_COMMAND_UPDATE)
                {
                    string S_PAGE = "~/Transport/RouteShiftTimingUI.aspx";
                    string sQueryString = "Id=" + iId + "&CategoryId=1&RouteName=" + txtRouteNo.Text + "&VehicleNo=" + txtVehicleNo.Text + "&JourneyName=" + txtJourney.Text + "&Name=" + txtName.Text;
                    string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQueryString);
                    string sRedirectUrl = S_PAGE + "?" + sEncrypt;
                    MasterPage oMasterPage = (MasterPage)this.Master;
                    oMasterPage.RedirectToNextPage(sRedirectUrl);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for listview related operations
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTransportOverrideConfigDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                TransportOverrideConfigDetails oTransportOverrideConfigDetails = e.Item.DataItem as TransportOverrideConfigDetails;
                
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

                Label lblStartDate = e.Item.FindControl("lblStartDate") as Label;

                if (oTransportOverrideConfigDetails.StartDate != DateTime.MinValue)
                    lblStartDate.Text = lblStartDate.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                else
                    lblStartDate.Text = "-";

                Label lblEndDate = e.Item.FindControl("lblEndDate") as Label;

                if (oTransportOverrideConfigDetails.EndDate != DateTime.MinValue)
                    lblEndDate.Text = lblEndDate.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT);
                else
                    lblEndDate.Text = "-";

                ImageButton imgBtnCopy = e.Item.FindControl("imgBtnCopy") as ImageButton;

                if (oTransportOverrideConfigDetails.WeekdayIds != string.Empty)
                {
                    Label lblWeekdays = e.Item.FindControl("lblWeekdays") as Label;

                    StringBuilder sb = new StringBuilder();
                    List<string> lstIds = oTransportOverrideConfigDetails.WeekdayIds.Split(',').ToList();
                    foreach (string sId in lstIds)
                    {
                        sb.Append(", " + ((DayOfWeek)sId.ToInt()).ToString().Substring(0, 3));
                    }

                    lblWeekdays.Text = sb.ToString().Substring(2);
                    imgBtnCopy.Visible = false;
                }
                else
                {
                    imgBtnCopy.Visible = true;

                    string sQueryString = CommonUtility.EncryptQuerystring("Id=" + oTransportOverrideConfigDetails.Id);

                    imgBtnCopy.Attributes.Add("onclick", "OpenPopup('" + sQueryString + "'); return false;");
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill pager footer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTransportOverrideConfigDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwTransportOverrideConfigDetails.Items.Count > 0)
                ControlUtility.FillListViewPagerFooter(lstvwTransportOverrideConfigDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to select page no.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwTransportOverrideConfigDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search details in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillOverrideDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTransportOverrideConfigDetails_Sorting(object sender, ListViewSortEventArgs e)
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
    /// This event is used to add new override entry.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string sQueryString = "CategoryId=1" + "&RouteName=" + txtRouteNo.Text + "&VehicleNo=" + txtVehicleNo.Text + "&JourneyName=" + txtJourney.Text + "&Name=" + txtName.Text;
            string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("../Transport/RouteShiftTimingUI.aspx?" + sEncrypt);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is sued to set querystring.
    /// </summary>
    private void SetQueryString()
    {
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Transport_Releted));
        
        if(QueryString["RouteName"] != null)
            txtRouteNo.Text = QueryString["RouteName"];

        if(QueryString["VehicleNo"] != null)
            txtVehicleNo.Text = QueryString["VehicleNo"];

        if(QueryString["JourneyName"]!= null)
            txtJourney.Text = QueryString["JourneyName"];

        if(QueryString["Name"]!= null)
            txtName.Text = QueryString["Name"];
        
        if (txtRouteNo.Text != string.Empty || txtVehicleNo.Text != string.Empty || txtJourney.Text != string.Empty || txtName.Text != string.Empty)
            btnSearch_Click(btnSearch, null);
    }
    
    /// <summary>
    /// This event is used to fill listview.
    /// </summary>
    private void FillOverrideDetails()
    {
        lstvwTransportOverrideConfigDetails.DataSourceID = objOverrideConfigDetails.ID;
        lstvwTransportOverrideConfigDetails.DataBind();
    }

    /// <summary>
    /// This method is used to delete transport override details.
    /// </summary>
    /// <param name="aiId"></param>
    private void Delete(int aiId)
    {
        moTransportOverrideConfigDetailsBL.Delete(aiId);
        lblUpdate.Text = S_DELETE_MSG;

        if (ConfigurationManager.AppSettings["TransportExternalDBName"] != null && ConfigurationManager.AppSettings["TransportExternalDBName"].ToString() != string.Empty)
        {
            string sDBName = ConfigurationManager.AppSettings["reportdatabasename"].ToString();
            string sTransportDBName = ConfigurationManager.AppSettings["TransportExternalDBName"].ToString();
            TransferTransportDetailsBL oTransferTransportDetailsBL = new TransferTransportDetailsBL(miSchoolId, sDBName, sTransportDBName);
            oTransferTransportDetailsBL.UpdateJourneyOverrideDetails();
        }
    }

    /// <summary>
    /// This event is used to reset fields.
    /// </summary>
    private void ResetFields()
    {
        txtRouteNo.Text = string.Empty;
        txtVehicleNo.Text = string.Empty;
        txtJourney.Text = string.Empty;
        txtName.Text = string.Empty;
    }

    #endregion
}