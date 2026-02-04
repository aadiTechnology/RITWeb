/* Class Name - TransportOverrideDetailsUI
 * Created By - Vishakha
 * Created On - 07-July-2023
 * Description - This class is used to display transport Override details.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using BusinessLogic.TransportBL;
using SchoolEntities.Transport;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Configuration;

public partial class TransportOverrideDetailsUI : SchoolBase
{
    #region Const
    
    private const string S_COMMAND_DELETE = "DeleteTransportOverrideDetails";
    private const string S_COMMAND_UPDATE = "UpdateTransportOverrideDetails";
    private const string S_DELETE_MSG = "Transport override details deleted succeessfully !!!"; 

    #endregion

    #region Data member(s)

    private TransportOverrideDetailsBL moTransportOverrideDetailsBL;

    #endregion

    #region Event(s)

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moTransportOverrideDetailsBL = new TransportOverrideDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                FillTransportOverrideDetails();
                SetDefaultValues();
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
    protected void lstvwTransportOverrideDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                int iId = Convert.ToInt32(lstvwTransportOverrideDetails.DataKeys[iRowId]["Id"]);

                if (e.CommandName == S_COMMAND_DELETE)
                {
                    Delete(iId);                   
                    FillTransportOverrideDetails();
                }
                else if (e.CommandName == S_COMMAND_UPDATE)
                {
                    MasterPage oMaster = this.Master as MasterPage;
                    oMaster.RedirectToNextPage("OverrideDetailsUI.aspx?" + CommonUtility.EncryptQuerystring("Id=" + iId));
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
    protected void lstvwTransportOverrideDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                OverrideDetails  oOverrideDetails = e.Item.DataItem as OverrideDetails;
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;

                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");

                Label lblStartDate = e.Item.FindControl("lblStartDate") as Label;
                lblStartDate.Text = lblStartDate.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT);

                Label lblEndDate = e.Item.FindControl("lblEndDate") as Label;
                lblEndDate.Text = lblEndDate.Text.ToDateTime().ToString(Constants.S_DATE_FORMAT);

                if (DateTime.Now.Date.IsBetween(oOverrideDetails.StartDate, oOverrideDetails.EndDate))
                {
                    HtmlTableRow tr = lblStartDate.Parent.Parent as HtmlTableRow;
                    tr.Style.Add("color", "Navy");
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
    protected void lstvwTransportOverrideDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwTransportOverrideDetails.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwTransportOverrideDetails, DtPgCount);
            }
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
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwTransportOverrideDetails);
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
            FillTransportOverrideDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ResetFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This event is used to fill listview.
    /// </summary>
    private void FillTransportOverrideDetails()
    {
        lstvwTransportOverrideDetails.DataSourceID = objOverrideDetails.ID;
        lstvwTransportOverrideDetails.DataBind();
    }

    /// <summary>
    /// This method is used to delete transport override details.
    /// </summary>
    /// <param name="aiId"></param>
    private void Delete(int aiId)
    {
        moTransportOverrideDetailsBL.Delete(aiId);
        lblUpdate.Text = S_DELETE_MSG;
        UpdateJourneyOverrides();
    }

    private void UpdateJourneyOverrides()
    {
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
        txtStudentName.Text = string.Empty;
        txtOverrideName.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        base.SetDefaultButton(btnSearch);
    }

    #endregion    
}