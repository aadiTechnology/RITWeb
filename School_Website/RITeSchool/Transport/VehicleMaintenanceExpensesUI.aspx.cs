// File Name  : VehicleDetailsUI.aspx.cs
// Created By : Deepak
// Date       : 7 July 2010
//Description :This class is used to add, eidt, delete vehicle details and also assocaite satff for vehicle. 

using System;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using SchoolEntities.Transport;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Web.UI.HtmlControls;
using System.Xml;
using CrystalDecisions.Shared;
using SchoolAutoSearchService.Service;
using System.Linq;
using BusinessLogic.Exceptions;
using System.IO;
using System.Xml.Serialization;

public partial class VehicleMaintenanceExpensesUI : SchoolBase
{
    #region 
    private const string S_TEXT_SAVE = "Save";
    private const string S_TEXT_UPDATE = "Update";
    private const string S_SAVE_MSG = "Vehicle Maintenance Expenses saved successfully !!!";
    private const string S_UPDATE_MSG = "Vehicle Maintenance Expenses updated successfully !!!";
    private const string S_DELETE_MSG = "Vehicle Maintenance Expenses deleted successfully !!!";
    private const string S_CONTROL_PANNEL_PATH = "../Common/ControlPanel.aspx";
    private const string S_COMMAND_DELETE_VEHICLE_MAINTENANCE = "DeleteVehicleMaintenance";
    private const string S_COMMAND_UPDATE_VEHICLE_MAINTENANCE = "UpdateVehicleMaintenance";
    private const string S_COMMAND_ADD_USED_PARTS = "AddParts";
    private const string S_COMMAND_DELETE_USED_PARTS = "DeleteParts";
    private const string S_DEFAULT_SORT_EXP = "";
    private const string S_BILL_FOLDER_LOCATION = "\\RITeSchool\\DOWNLOADS\\TransportModule\\BILL\\";
    
    #endregion

    #region Data Member(s)

    private VehicleMaintenanceExpensesBL oVehicleMaintenanceExpensesBL = new VehicleMaintenanceExpensesBL();

    Decimal calculatedAmt;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            FillMaintenanceTypeDropdown();
            SetDefaultValues();
            FillVehicleNumbers();
            FirstListViewRow();
            FillLstVWVehicleMaintenanceDetails();
            SetJavaScriptAttributes();
        }
    }

    protected void txtQuantity_TextChanged(object sender, EventArgs e)
    {
        SetCalculatedAmount(sender);
    }

    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        SetCalculatedAmount(sender);
    }

    protected void txtAmounts_TextChanged(object sender, EventArgs e)
    {
        SetCalculatedTotalAmount();
    }

    protected void txtLabour_TextChanged(object sender, EventArgs e)
    {
        SetCalculatedTotalAmount();
    }

    /// <summary>
    /// This event is called to save the Vehicle Maintenance Details and Vehicle Maintenance Parts Used Details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveVehicleMaintenanceExpensesDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is called while cancelling update and save operation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ResetFields();
            FillVehicleNumbers();
            AddSortImage();
            FillLstVWVehicleMaintenanceDetails();
            FirstListViewRow();
            btnSave.Text = S_TEXT_SAVE;
            lblUpdateSuccess.Text = "";
            ddlMaintenanceType.ClearSelection();
            btnFile.Visible = false;
            DeleteIcon.Visible = false;            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle update & delete commands raised from the Existing Vehicle Maintenance Parts Used ListView.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwPartsUsed_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;

                if (e.CommandName == S_COMMAND_ADD_USED_PARTS)
                {
                    int rowIndex = 0;

                    if (ViewState["CurrentTable"] != null)
                    {
                         DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                         DataRow drCurrentRow = null;
                         if (dtCurrentTable.Rows.Count > 0)
                         {
                             for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                             {
                                 TextBox TextPartsUsed = (TextBox)lstvwPartsUsed.Items[rowIndex].FindControl("txtPartsUsed");
                                 TextBox TextQuantity = (TextBox)lstvwPartsUsed.Items[rowIndex].FindControl("txtQuantity");
                                 TextBox TextRate = (TextBox)lstvwPartsUsed.Items[rowIndex].FindControl("txtRate");
                                 TextBox TextAmounts = (TextBox)lstvwPartsUsed.Items[rowIndex].FindControl("txtAmounts");

                                 drCurrentRow = dtCurrentTable.NewRow();
                                 drCurrentRow["RowNumber"] = i + 1;
                                 
                                 dtCurrentTable.Rows[i - 1]["ColPartsUsed"] = TextPartsUsed.Text;
                                 dtCurrentTable.Rows[i - 1]["ColQty"] = TextQuantity.Text;
                                 dtCurrentTable.Rows[i - 1]["ColRate"] = TextRate.Text;
                                 dtCurrentTable.Rows[i - 1]["ColAmt"] = TextAmounts.Text;
                                 rowIndex++;
                             }
                             dtCurrentTable.Rows.Add(drCurrentRow);
                             ViewState["CurrentTable"] = dtCurrentTable;

                             lstvwPartsUsed.DataSource = dtCurrentTable;
                             lstvwPartsUsed.DataBind();
                         }
                    }
                    else
                    {
                        Response.Write("ViewState is null");
                    }
                    SetPreviousData();
                }
                else if (e.CommandName == S_COMMAND_DELETE_USED_PARTS)
                {
                    SetRowData();
                    if (ViewState["CurrentTable"] != null)
                    {
                        DataTable dt = (DataTable)ViewState["CurrentTable"];

                        int rowIndex = iRowId;
                        if (dt.Rows.Count > 1)
                        {
                            dt.Rows.Remove(dt.Rows[rowIndex]);
                            ViewState["CurrentTable"] = dt;
                            lstvwPartsUsed.DataSource = dt;
                            lstvwPartsUsed.DataBind();

                            for (int i = 0; i < lstvwPartsUsed.Items.Count - 1; i++)
                            {
                                TextBox TextPartsUsed = (TextBox)lstvwPartsUsed.Items[i].FindControl("txtPartsUsed");
                                TextPartsUsed.Text = Convert.ToString(i + 1);
                            }
                            SetPreviousData();
                        }
                        SetCalculatedTotalAmount();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle update & delete commands raised from the Existing Vehicle Maintenance Expenses Details ListView.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehicleMaintenanceDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                int iVehicleMaintenanceExpensesId = Convert.ToInt32(lstvwVehicleMaintenanceDetails.DataKeys[iRowId]["VehicleMaintenanceExpensesId"]);

                if (e.CommandName == S_COMMAND_UPDATE_VEHICLE_MAINTENANCE)
                {
                    SetControlsForEditMode(oCurrentItem);
                }
                else if (e.CommandName == S_COMMAND_DELETE_VEHICLE_MAINTENANCE)
                {
                    DeleteVehicleMaintenanceExpenses(iVehicleMaintenanceExpensesId);
                    FirstListViewRow();
                    ResetFields();
                    FillLstVWVehicleMaintenanceDetails();
                    FillVehicleNumbers();

                    string sBillFileName = Convert.ToString(lstvwVehicleMaintenanceDetails.DataKeys[e.Item.DisplayIndex]["BillFileName"]);
                    string sServerFilePath = Server.MapPath("..") + S_BILL_FOLDER_LOCATION + "\\" + sBillFileName;

                    if (File.Exists(sServerFilePath))
                        File.Delete(sServerFilePath);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the visibility of columns in the ListView.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehicleMaintenanceDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                if (oCurrentItem != null)
                    SetVisibilityOfColumns(oCurrentItem);

                int iRowId = oCurrentItem.DisplayIndex;
                ImageButton imgBtnView = oCurrentItem.FindControl("imgBtnView") as ImageButton;               
                string sBillFileName = Convert.ToString(lstvwVehicleMaintenanceDetails.DataKeys[iRowId]["BillFileName"]);
                string sPath = "../DOWNLOADS/TransportModule/Bill/" + sBillFileName;
                if (sBillFileName != string.Empty)
                {
                    imgBtnView.Visible = true;
                    imgBtnView.Attributes.Add("onclick", "window.open('" + sPath
                                              + "' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600'); return false;");
                }
                else
                    imgBtnView.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Initializes the DataPager control of the ListView and adds a Sort Image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehicleMaintenanceDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwVehicleMaintenanceDetails.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwVehicleMaintenanceDetails, DtPgCount);
                AddSortImage();
            }
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort data according to selection.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehicleMaintenanceDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view pagewise Maintenance Expenses.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwVehicleMaintenanceDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to used to search record.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillLstVWVehicleMaintenanceDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Method Deletes perticular Maintenance Expenses.
    /// </summary>
    /// <param name="aiNoticeId"></param>
    private void DeleteVehicleMaintenanceExpenses(int iVehicleMaintenanceExpensesId)
    {
        VehicleMaintenanceExpensesBL.Delete(miSchoolId, iVehicleMaintenanceExpensesId, miUserId, miAcademicYearId);
        lblUpdateSuccess.Text = S_DELETE_MSG;
    }

    #region

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        txtMaintenanceDate.Text = DateTime.Today.ToString(Constants.S_DATE_FORMAT);
        txtBillDate.Text = DateTime.Today.ToString(Constants.S_DATE_FORMAT);
        HtmlTableRow oHtmlTableHeaderRow = lstvwVehicleMaintenanceDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
        
        base.SetDefaultButton(btnSearch);
        
        //AddSortImage();
    }

    /// <summary>
    /// This method is used to fill all the Vehicle Maintenance Expenses Details in the ListView.
    /// </summary>
    private void FillLstVWVehicleMaintenanceDetails()
    {
        lstvwVehicleMaintenanceDetails.DataSourceID = ObjDSVehicleDetails.ID;
        lstvwVehicleMaintenanceDetails.DataBind();
    }

    /// <summary>
    ///  This method is used to fill all the Vehicle Maintenance Expenses Parts Used Details in the ListView.
    /// </summary>
    private void FillLstvwVehicleMaintenancePartsUsedDetails()
    {
        DataSet dsVehiclePartsUsed = oVehicleMaintenanceExpensesBL.GetAllVehicleExpensesPartsUsedDetails(Convert.ToInt32(hidVehicleMaintenanceExpensesId.Value));

        if (!(dsVehiclePartsUsed.Tables[0].IsNonEmpty()))
            FirstListViewRow();
        else
        {
            lstvwPartsUsed.DataSource = dsVehiclePartsUsed.Tables[0];
            lstvwPartsUsed.DataBind();

            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("ColPartsUsed", typeof(string)));
            dt.Columns.Add(new DataColumn("ColQty", typeof(string)));
            dt.Columns.Add(new DataColumn("ColRate", typeof(string)));
            dt.Columns.Add(new DataColumn("ColAmt", typeof(string)));

            if (dsVehiclePartsUsed.Tables[0].Rows.Count > 0)
                for (int i = 1; i <= dsVehiclePartsUsed.Tables[0].Rows.Count; i++)
                {
                    TextBox oTextBoxPartsUsed = lstvwPartsUsed.Items[i-1].FindControl("txtPartsUsed") as TextBox;
                    TextBox oTextBoxQty = lstvwPartsUsed.Items[i-1].FindControl("txtQuantity") as TextBox;
                    TextBox oTextBoxRate = lstvwPartsUsed.Items[i-1].FindControl("txtRate") as TextBox;
                    TextBox oTextBoxAmounts = lstvwPartsUsed.Items[i-1].FindControl("txtAmounts") as TextBox;

                    oTextBoxPartsUsed.Text = dsVehiclePartsUsed.Tables[0].Rows[i-1]["PartsUsed"].ToString();
                    oTextBoxQty.Text = dsVehiclePartsUsed.Tables[0].Rows[i-1]["Quantity"].ToString();
                    oTextBoxRate.Text = dsVehiclePartsUsed.Tables[0].Rows[i-1]["Rate"].ToString();
                    oTextBoxAmounts.Text = dsVehiclePartsUsed.Tables[0].Rows[i-1]["Amount"].ToString();
                    
                    dr = dt.NewRow();
                    dr["RowNumber"] = i;
                    dr["ColPartsUsed"] = oTextBoxPartsUsed.Text;
                    dr["ColQty"] = oTextBoxQty.Text;
                    dr["ColRate"] = oTextBoxRate.Text;
                    dr["ColAmt"] = oTextBoxAmounts.Text;
                    dt.Rows.Add(dr);
                    ViewState["CurrentTable"] = dt;
                }
        }
    }

    /// <summary>
    /// This method is used to create one empty row with four textbox in the Vehicle Maintenance Expenses Parts Used Listview.
    /// </summary>
    private void FirstListViewRow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("ColPartsUsed", typeof(string)));
        dt.Columns.Add(new DataColumn("ColQty", typeof(string)));
        dt.Columns.Add(new DataColumn("ColRate", typeof(string)));
        dt.Columns.Add(new DataColumn("ColAmt", typeof(string)));
        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["ColPartsUsed"] = string.Empty;
        dr["ColQty"] = string.Empty;
        dr["ColRate"] = string.Empty;
        dr["ColAmt"] = string.Empty;
        dt.Rows.Add(dr);

        ViewState["CurrentTable"] = dt;

        lstvwPartsUsed.DataSource = dt;
        lstvwPartsUsed.DataBind();
    }

    /// <summary>
    /// This method is used to set the previous data to the Vehicle Maintenance Expenses Parts Used Listview.
    /// </summary>
    private void SetPreviousData()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox TextPartsUsed = (TextBox)lstvwPartsUsed.Items[i].FindControl("txtPartsUsed");
                    TextBox TextQuantity = (TextBox)lstvwPartsUsed.Items[i].FindControl("txtQuantity");
                    TextBox TextRate = (TextBox)lstvwPartsUsed.Items[i].FindControl("txtRate");
                    TextBox TextAmounts = (TextBox)lstvwPartsUsed.Items[i].FindControl("txtAmounts");

                    TextPartsUsed.Text = dt.Rows[i]["ColPartsUsed"].ToString();
                    TextQuantity.Text = dt.Rows[i]["ColQty"].ToString();
                    TextRate.Text = dt.Rows[i]["ColRate"].ToString();
                    TextAmounts.Text = dt.Rows[i]["ColAmt"].ToString();
                    rowIndex++;
                }
            }
        }
    }

    /// <summary>
    /// This method is used to set the row data to the Vehicle Maintenance Expenses Parts Used Listview while deleting the row from listview.
    /// </summary>
    private void SetRowData()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    TextBox TextPartsUsed = (TextBox)lstvwPartsUsed.Items[rowIndex].FindControl("txtPartsUsed");
                    TextBox TextQuantity = (TextBox)lstvwPartsUsed.Items[rowIndex].FindControl("txtQuantity");
                    TextBox TextRate = (TextBox)lstvwPartsUsed.Items[rowIndex].FindControl("txtRate");
                    TextBox TextAmounts = (TextBox)lstvwPartsUsed.Items[rowIndex].FindControl("txtAmounts");

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RowNumber"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["ColPartsUsed"] = TextPartsUsed.Text;
                    dtCurrentTable.Rows[i - 1]["ColQty"] = TextQuantity.Text;
                    dtCurrentTable.Rows[i - 1]["ColRate"] = TextRate.Text;
                    dtCurrentTable.Rows[i - 1]["ColAmt"] = TextAmounts.Text;
                    rowIndex++;
                }

                ViewState["CurrentTable"] = dtCurrentTable;
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }
    }

    /// <summary>
    /// This method is used to reset fields.
    /// </summary>
    private void ResetFields()
    {
        ddlVehicleNo.ClearSelection();
        ddlVehicleNo.Focus();
        txtMaintenanceDate.Text = DateTime.Today.ToString(Constants.S_DATE_FORMAT);
        txtMeterReading.Text = string.Empty;
        txtBillNo.Text = string.Empty;
        txtBillDate.Text = DateTime.Today.ToString(Constants.S_DATE_FORMAT);
        txtWorkshopName.Text = string.Empty;
        txtWorkDetails.Text = string.Empty;
        txtLabour.Text = string.Empty;
        txtTotalAmount.Text = string.Empty;
        txtExpiryDate.Text = string.Empty;
        ddlMaintenanceType.ClearSelection();
        btnSave.Text = Constants.ButtonText.Save.ToString();
        btnFile.Visible = false;
        DeleteIcon.Visible = false;        
    }

    /// <summary>
    /// This method is used to populate Vehicle Maintenace Expenses Details to save.
    /// </summary>
    /// <param name="iVehicleMaintenanceExpensesId"></param>
    /// <returns></returns>
    private VehicleMaintenanceExpenses PopulateVehicleMaintenanceDetails(int iVehicleMaintenanceExpensesId, string asFileName)
    {
        VehicleMaintenanceExpenses oVehicleMaintenanceExpenses = new VehicleMaintenanceExpenses
        {
            VehicleMaintenanceExpensesId = iVehicleMaintenanceExpensesId,
            MeterReading = txtMeterReading.Text != string.Empty ? Convert.ToDecimal(txtMeterReading.Text) : Constants.I_ZERO,
            MaintenanceDate = txtMaintenanceDate.Text,
            BillDate = txtBillDate.Text,
            VehicleId = ddlVehicleNo.SelectedValue.ToInt(),
            BillNumber = txtBillNo.Text,
            WorkshopName = txtWorkshopName.Text,
            WorkDetails = txtWorkDetails.Text,
            SchoolId = miSchoolId,
            AcademicYearId = miAcademicYearId,
            InsertedById = miUserId,
            Labour = txtLabour.Text != string.Empty ? Convert.ToDecimal(txtLabour.Text) : Constants.I_ZERO,
            TotalAmount = txtTotalAmount.Text != string.Empty ? Convert.ToDecimal(txtTotalAmount.Text) : Constants.I_ZERO,
            ExpiryDate = txtExpiryDate.Text,
            MaintenanceTypeId = ddlMaintenanceType.SelectedValue.ToInt(),
            BillFileName = asFileName
        };
        return oVehicleMaintenanceExpenses;
    }

    /// <summary>
    /// This method is used to populate Vehicle Maintenace Expenses Parts Used Details to save.
    /// </summary>
    /// <param name="iVehicleMaintenanceExpensesId"></param>
    /// <returns></returns>
    private List<VehicleMaintenancePartsUsed> PopulateVehiclePartsDetails(int iVehicleMaintenanceExpensesId)
    {
        List<VehicleMaintenancePartsUsed> oVehicleMaintenancePartsUsed = new List<VehicleMaintenancePartsUsed>();

        foreach (ListViewDataItem item in lstvwPartsUsed.Items)
        {
            TextBox oTextBoxPartsUsed = item.FindControl("txtPartsUsed") as TextBox;
            TextBox oTextBoxQty = item.FindControl("txtQuantity") as TextBox;
            TextBox oTextBoxRate = item.FindControl("txtRate") as TextBox;
            TextBox oTextBoxAmounts = item.FindControl("txtAmounts") as TextBox;

            if (oTextBoxPartsUsed.Text != string.Empty)
            {
                oVehicleMaintenancePartsUsed.Add(new VehicleMaintenancePartsUsed()
                    {
                        VehicleMaintenanceExpensesId = iVehicleMaintenanceExpensesId,
                        PartsUsed = oTextBoxPartsUsed.Text != string.Empty ? oTextBoxPartsUsed.Text : string.Empty,
                        Quantity = oTextBoxQty.Text != string.Empty ? Convert.ToDecimal(oTextBoxQty.Text) : Constants.I_ZERO,
                        Rate = oTextBoxRate.Text != string.Empty ? Convert.ToDecimal(oTextBoxRate.Text) : Constants.I_ZERO,
                        Amount = oTextBoxAmounts.Text != string.Empty ? Convert.ToDecimal(oTextBoxAmounts.Text) : Constants.I_ZERO,
                        InsertedById = miUserId,
                    });
            }
        }

        return oVehicleMaintenancePartsUsed;
    }

    /// <summary>
    /// This method is used to fill Vehicle Number in the "Vehicle No." Dropdownlist.
    /// </summary>
    private void FillVehicleNumbers()
    {
        List<VehicleMaintenanceExpenses> lstVehicleDetails = VehicleMaintenanceExpensesBL.GetVehicleNumbers(miAcademicYearId);

        if (lstVehicleDetails != null)
        {
            ListSource.FillDropDownList(lstVehicleDetails, ddlVehicleNo, "VehicleNumber", "VehicleId", Constants.S_SELECT);
        }
    }

    /// <summary>
    /// This methos is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        new Button[] { btnCancel, btnSave }.ApplyEffect();

        if (moUserRole == Constants.UserRoles.Admin)
        {
            btnBack.Visible = true;
            btnBack.PostBackUrl = Constants.S_PAGE_CONTROL_PANEL;
        }
    }

    /// <summary>
    /// This method is used to Save the Vehicle Maintenance Expenses Details and Vehicle Maintenance Expenses Parts Used Details.
    /// </summary>
    private void SaveVehicleMaintenanceExpensesDetails()
    {
        int iVehicleMaintenanceExpensesId = 0;
        if (hidVehicleMaintenanceExpensesId.Value != string.Empty)
        {
            iVehicleMaintenanceExpensesId = Convert.ToInt32(hidVehicleMaintenanceExpensesId.Value);
        }

        string sFileName = CheckIsFileUploaded();

        VehicleMaintenanceExpenses oVehicleMaintenanceExpenses = PopulateVehicleMaintenanceDetails(iVehicleMaintenanceExpensesId, sFileName);
        List<VehicleMaintenancePartsUsed> oVehicleMaintenancePartsUsed = PopulateVehiclePartsDetails(iVehicleMaintenanceExpensesId);

        if (oVehicleMaintenanceExpenses != null)
        {
            string sXml = CommonUtility.GenerateXml(oVehicleMaintenanceExpenses);

            string sXmlVehiclePartsUsed = GenerateXml(oVehicleMaintenancePartsUsed);

            VehicleMaintenanceExpensesBL.SaveUpdateVehicleMaintenanceExpenses(sXml, sXmlVehiclePartsUsed);
            if (btnSave.Text == S_TEXT_SAVE)
                lblUpdateSuccess.Text = S_SAVE_MSG;
            else
            {
                lblUpdateSuccess.Text = S_UPDATE_MSG;
                btnSave.Text = S_TEXT_SAVE;
            }
            hidVehicleMaintenanceExpensesId.Value = "";
        }
        else
            AddSortImage();

        FillVehicleNumbers();
        FillLstVWVehicleMaintenanceDetails();
        FirstListViewRow();
        ResetFields();
    }

    /// This method is used to check Is file Uploaded or not.
    /// </summary>
    private string CheckIsFileUploaded()
    {
        string sFileName = string.Empty;
        if (FlBill.FileName != string.Empty)
        {
            string sServerPath = Server.MapPath("~");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";

            string sLinkName = FlBill.FileName;

            if (File.Exists(sServerPath + S_BILL_FOLDER_LOCATION + sLinkName))
                sLinkName = CommonUtility.GetFileNameForRenaming(FlBill.FileName);

            string sLinkPath = sServerPath + S_BILL_FOLDER_LOCATION + sLinkName;

            if (FlBill.HasFile)
            {
                FlBill.SaveAs(sLinkPath);
                sFileName = sLinkName;
            }
        }
        else
            sFileName = hidFileUpload.Value;

        return sFileName;
    }

    /// <summary>
    /// This method is used to set values to controls in edit mode.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    private void SetControlsForEditMode(ListViewDataItem oCurrentItem)
    {
        btnSave.Text = S_TEXT_UPDATE;
        Label olblMaintenanceDt = oCurrentItem.FindControl("lblMaintenanceDt") as Label;
        Label olblMeterReading = oCurrentItem.FindControl("lblMeterReading") as Label;
        Label olblBillNo = oCurrentItem.FindControl("lblBillNo") as Label;
        Label olblBillDt = oCurrentItem.FindControl("lblBillDt") as Label;
        Label olblWorkshopName = oCurrentItem.FindControl("lblWorkshopName") as Label;
        Label olblLabour = oCurrentItem.FindControl("lblLabour") as Label;
        Label olblTotalAmount = oCurrentItem.FindControl("lblTotalAmount") as Label;

        int iRowId = oCurrentItem.DisplayIndex;
        int iVehicleId = Convert.ToInt32(lstvwVehicleMaintenanceDetails.DataKeys[iRowId]["VehicleId"]);
        int VehicleMaintenanceExpensesId = Convert.ToInt32(lstvwVehicleMaintenanceDetails.DataKeys[iRowId]["VehicleMaintenanceExpensesId"]);
        string sWorkDetails = Convert.ToString(lstvwVehicleMaintenanceDetails.DataKeys[iRowId]["WorkDetails"]);
        
        hidVehicleMaintenanceExpensesId.Value = VehicleMaintenanceExpensesId.ToString();
        txtMaintenanceDate.Text = Convert.ToDateTime(olblMaintenanceDt.Text).ToString("dd-MMM-yyyy");
        ddlVehicleNo.SelectedValue = iVehicleId.ToString();
        txtMeterReading.Text = olblMeterReading.Text;
        txtBillNo.Text = olblBillNo.Text;
        txtBillDate.Text = Convert.ToDateTime(olblBillDt.Text).ToString("dd-MMM-yyyy");
        txtWorkshopName.Text = olblWorkshopName.Text;
        txtWorkDetails.Text = sWorkDetails;
        txtLabour.Text = olblLabour.Text;
        txtTotalAmount.Text = olblTotalAmount.Text;

        if (lstvwVehicleMaintenanceDetails.DataKeys[iRowId]["ExpiryDate"] != DBNull.Value && lstvwVehicleMaintenanceDetails.DataKeys[iRowId]["ExpiryDate"].ToDateTime().ToString(Constants.S_DATE_FORMAT) != "01-Jan-1900")
            txtExpiryDate.Text = lstvwVehicleMaintenanceDetails.DataKeys[iRowId]["ExpiryDate"].ToDateTime().ToString(Constants.S_DATE_FORMAT);
        else
            txtExpiryDate.Text = string.Empty;

        ddlMaintenanceType.SelectedValue = Convert.ToString(lstvwVehicleMaintenanceDetails.DataKeys[iRowId]["MaintenanceTypeId"]);

        string sBillFileName = Convert.ToString(lstvwVehicleMaintenanceDetails.DataKeys[iRowId]["BillFileName"]);
        string sPath = "../downloads/TransportModule/Bill/" + sBillFileName;
        btnFile.Attributes.Add("Onclick", "OpenFile('" + sPath + "'); return false;");

        hidFileUpload.Value = sBillFileName;

        DeleteIcon.Attributes.Add("Onclick", "if(!Confirm()) {return false;}");

        if (sBillFileName != string.Empty)
        {
            btnFile.Visible = true;
            DeleteIcon.Visible = true;
        }
        else
        {
            btnFile.Visible = false;
            DeleteIcon.Visible = false;
        }

        FillLstvwVehicleMaintenancePartsUsedDetails();
        AddSortImage();
    }

    /// <summary>
    /// This method is used to set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwVehicleMaintenanceDetails.SortDirection.ToString() == "Ascending" || lstvwVehicleMaintenanceDetails.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwVehicleMaintenanceDetails.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwVehicleMaintenanceDetails.SortExpression.ToString();
        HtmlTableRow oHtmlTableHeaderRow = lstvwVehicleMaintenanceDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to Set Visibility of Columns in Vehicle Maintenance Expenses Details listview.
    /// </summary>
    /// <param name="oCurrentItem"></param>
    private void SetVisibilityOfColumns(ListViewDataItem oCurrentItem)
    {
        ImageButton imgBtnDelete = oCurrentItem.FindControl("imgBtnDelete") as ImageButton;

        imgBtnDelete.Attributes.Add("Onclick", "if(!ConfirmDelete()) {return false;}");
         ResetFields();
    }

    /// <summary>
    /// This function is used to generate an XML for inserting/updating records of the Vehicle Maintenance Parts Used Listview
    /// </summary>
    /// <param name="alsVehiclePartsUsed"></param>
    /// <returns></returns>
    private string GenerateXML(List<VehicleMaintenancePartsUsed> alsVehiclePartsUsed)
    {
        StringWriter sw = new StringWriter();
        new XmlSerializer(alsVehiclePartsUsed.GetType()).Serialize(sw, alsVehiclePartsUsed);
        string sXML = sw.ToString();
        sXML = sXML.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", String.Empty);
        return sXML;
    }

  /// <summary>
  /// This function is used to Get Calculated Amount from Rate and Quantity
  /// </summary>
  /// <param name="Qty"></param>
  /// <param name="Rate"></param>
  /// <returns></returns>
    private decimal GetCalculatedAmount(string Qty, string Rate)
    {
        return Qty != string.Empty && Rate != string.Empty ? Convert.ToDecimal(Qty) * Convert.ToDecimal(Rate) : Constants.I_ZERO;
    }

    /// <summary>
    /// This function is used to Set calculated Amount to Textbox column in ListView 
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
    private void SetCalculatedAmount(object sender)
     {
        TextBox txtQty = (TextBox)((TextBox)sender).Parent.Parent.Parent.FindControl("txtQuantity");
        TextBox txtRate = (TextBox)((TextBox)sender).Parent.Parent.Parent.FindControl("txtRate");
        TextBox txtAmt = (TextBox)((TextBox)sender).Parent.Parent.Parent.FindControl("txtAmounts");

        txtAmt.Text = Convert.ToString(GetCalculatedAmount(txtQty.Text, txtRate.Text));
        SetCalculatedTotalAmount();
    }
    /// <summary>
    /// This function is used to Set calculated TotalAmount to textbox
    /// </summary>s
    private void SetCalculatedTotalAmount()
    {
        foreach (ListViewDataItem item in lstvwPartsUsed.Items)
        {
            TextBox oTextBoxAmounts = item.FindControl("txtAmounts") as TextBox;

            calculatedAmt += oTextBoxAmounts.Text != string.Empty ? Convert.ToDecimal(oTextBoxAmounts.Text) : Constants.I_ZERO;
        }

        txtTotalAmount.Text = calculatedAmt != 0 ? (txtLabour.Text != string.Empty ? Convert.ToDecimal(txtLabour.Text) + calculatedAmt : calculatedAmt).ToString() : (txtLabour.Text != string.Empty ? txtLabour.Text : Convert.ToString(Constants.I_ZERO));
    }

    /// <summary>
    /// This method is used to fill maintenance type dropdown.
    /// </summary>
    private void FillMaintenanceTypeDropdown()
    {
        List<Maintanance> lstMaintenance = oVehicleMaintenanceExpensesBL.GetMaintenanceTypeList();
        ListSource.FillDropDownList(lstMaintenance, ddlMaintenanceType, "MaintenanceType", "MaintenanceTypeId", Constants.S_SELECT);
        ListSource.FillDropDownList(lstMaintenance, cmbMaintenanceType, "MaintenanceType", "MaintenanceTypeId", Constants.S_ALL);
    }

    #endregion
}