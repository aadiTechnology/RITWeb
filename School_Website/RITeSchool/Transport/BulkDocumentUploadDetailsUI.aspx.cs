/*File Name - BulkDocumentUploadDetailsUI.aspx.cs
 * Created Date - 23-Jul-2024
 * Created By - Rutuja
 * Description - This class is used to display bulk document details.
 */
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Reflection;
using BusinessLogic;
using BusinessLogic.Exceptions;
using TransportEntities;
using Utility;
using System.IO;
using BusinessLogic.TransportBL;
using System.Web.UI.HtmlControls;
using CrystalDecisions.Shared;

public partial class BulkDocumentUploadDetailsUI : SchoolBase
{
    #region Constants
    
    private const string S_DELETE_MSG = "Document Details deleted successfully !!!";
    private const string S_SAVE_MSG = "Document Details saved successfully !!!";
    private const string S_COMMAND_DELETE = "DeleteDocumentDetails";
    private const string S_DOCUMENT_FOLDER_LOCATION = "\\DOWNLOADS\\TransportModule\\VehicleDocuments\\";
    private const int EXPIRY_WARNING_DAYS = 30;
    
    #endregion

    #region Data Member(s)

    private BulkDocumentDetailsBL moBulkDocumentDetailsBL;
    
    #endregion

    #region Event(s)
    
    /// <summary>
    /// This event is used to set default values, fill documents in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
       try
       {
           moBulkDocumentDetailsBL = new BulkDocumentDetailsBL(miSchoolId, miAcademicYearId, miUserId);
           if (!IsPostBack)
           {   
               SetDefaultValues();
               FillDocuments();               
           }
       }
       catch (Exception ex)
       {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
       }
    }

    /// <summary>
    /// This event is used to set attribute on listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBulkDocumentDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                GetBulkDocumentDetails oBulkDocumentDetails = e.Item.DataItem as GetBulkDocumentDetails;
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;

                HiddenField hidDocFile = (HiddenField)oCurrentItem.FindControl("hidDocFile");
                ImageButton imgbtnView = (ImageButton)e.Item.FindControl("imgbtnView");
                ImageButton imgbtnDelete = (ImageButton)e.Item.FindControl("imgbtnDelete");
                TextBox txtTitle = e.Item.FindControl("txtTitle") as TextBox;
                TextBox txtStartDate = e.Item.FindControl("txtStartDate") as TextBox;
                TextBox txtEndDate = e.Item.FindControl("txtEndDate") as TextBox;
                TextBox txtDescription = e.Item.FindControl("txtDescription") as TextBox;
                string sFilePath = lstvwBulkDocumentDetails.DataKeys[oCurrentItem.DisplayIndex]["FileName"].ToString();
                DropDownList cmbAction = e.Item.FindControl("cmbAction") as DropDownList;
                Label lblRowNo = e.Item.FindControl("lblRowNo") as Label;
                Label lblVehicleNumber = e.Item.FindControl("lblVehicleNumber") as Label;

                lblRowNo.Text = (e.Item.DisplayIndex + 1).ToString();

                if (oBulkDocumentDetails.Id != 0)
                {
                    if (sFilePath != string.Empty)
                        imgbtnView.Visible = true;

                    imgbtnDelete.Visible = true;
                }
                else
                {
                    imgbtnView.Visible = false;
                    imgbtnDelete.Visible = false;
                    cmbAction.SelectedValue = "1";
                    cmbAction.Enabled = false;
                }

                if (oBulkDocumentDetails.Title == "-")
                    txtTitle.Text = string.Empty;

                if (oBulkDocumentDetails.Description == "-")
                    txtDescription.Text = string.Empty;

                if (oBulkDocumentDetails.StartDate != DateTime.MinValue)
                    txtStartDate.Text = oBulkDocumentDetails.StartDate.ToString(Constants.S_DATE_FORMAT);
                else
                    txtStartDate.Text = string.Empty;

                if (oBulkDocumentDetails.EndDate != DateTime.MinValue)
                    txtEndDate.Text = oBulkDocumentDetails.EndDate.ToString(Constants.S_DATE_FORMAT);
                else
                    txtEndDate.Text = string.Empty;

                if (sFilePath.TrimAll() != string.Empty && sFilePath != "-")
                {
                    hidDocFile.Value = sFilePath;
                    imgbtnView.Visible = true;
                    imgbtnView.Attributes.Add("Onclick", "OpenDocument('../DOWNLOADS/TransportModule/VehicleDocuments" + '/' + sFilePath + "');return false;");
                    imgbtnDelete.Visible = true;
                    imgbtnDelete.Attributes.Add("Onclick", "if(!ConfirmDelete()) return false;");
                }

                HtmlTableCell tdAmount = e.Item.FindControl("tdAmount") as HtmlTableCell;
                if (tdAmount != null)
                {
                    if (ddlDocuments.SelectedValue == DocumentType.Insurance.ToInt().ToString())
                        tdAmount.Visible = true;
                    else
                        tdAmount.Visible = false;
                }

                HtmlTableCell tdPolicyNo = e.Item.FindControl("tdPolicyNo") as HtmlTableCell;
                if (tdPolicyNo != null)
                {
                    if (ddlDocuments.SelectedValue == DocumentType.Insurance.ToInt().ToString())
                        tdPolicyNo.Visible = true;
                    else
                        tdPolicyNo.Visible = false;
                }

                HtmlTableCell tdEndDate = e.Item.FindControl("tdEndDate") as HtmlTableCell;
                if (tdEndDate != null)
                {
                    if (ddlDocuments.SelectedValue == DocumentType.Invoice.ToInt().ToString() || ddlDocuments.SelectedValue == DocumentType.RCBook.ToInt().ToString())
                        tdEndDate.Visible = false;
                    else
                        tdEndDate.Visible = true;
                }
                if (oBulkDocumentDetails.EndDate != DateTime.MinValue && lblVehicleNumber != null)
                {
                    if (oBulkDocumentDetails.EndDate.Date < DateTime.Today.Date)
                    {
                        lblVehicleNumber.Style["color"] = "red";
                        lblVehicleNumber.Style["font-weight"] = "bold";
                    }
                    else if (oBulkDocumentDetails.EndDate.Date >= DateTime.Today.Date && oBulkDocumentDetails.EndDate.Date <= DateTime.Today.AddDays(EXPIRY_WARNING_DAYS).Date)
                    {
                        lblVehicleNumber.Style["color"] = "navy";
                        lblVehicleNumber.Style["font-weight"] = "bold";
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
    /// This event is used to fill pager footer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBulkDocumentDetails_DataBound(object sender, EventArgs e)
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
    /// This event is used to upload,remove document.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwBulkDocumentDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iId = Convert.ToInt32(lstvwBulkDocumentDetails.DataKeys[e.Item.DisplayIndex]["Id"]);
                
                if (e.CommandName == S_COMMAND_DELETE)
                {
                    DeleteDocumentDetails(iId);
                    FillListview();
                }                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save document details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if(Page.IsValid)
                Save();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            FillListview();

            hidDocumentId.Value = ddlDocuments.SelectedValue;
            hidVehicleNo.Value = txtSearch.Text.Trim();
            hidShowAll.Value = (chkShowAll.Checked ? "1" : "0");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string sFilter = "(usp_GetVehicleDocumentDetailsForReport.Id}=" + hidDocumentId.Value + " AND usp_GetVehicleDocumentDetailsForReport.SchoolId}=" + miSchoolId + " AND usp_GetVehicleDocumentDetailsForReport.AcademicYearId}=" + miAcademicYearId + " AND usp_GetVehicleDocumentDetailsForReport.Filter}=" + hidVehicleNo.Value + " AND usp_GetVehicleDocumentDetailsForReport.ShowAll}=" + hidShowAll.Value + ")@";
            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.VehicleDocumentDetails, sFilter, ExportFormatType.Excel);
            oReportDisplay.DisplayReport();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void DateOverlapping_Validate(object sender, ServerValidateEventArgs e)
    {
        List<BulkDocumentDetails> lstBulkDocumentDetails = new List<BulkDocumentDetails>();
        foreach (ListViewDataItem item in lstvwBulkDocumentDetails.Items)
        {
            CheckBox chkSelect = item.FindControl("chkSelect") as CheckBox;            
            TextBox txtStartDate = item.FindControl("txtStartDate") as TextBox;
            TextBox txtEndDate = item.FindControl("txtEndDate") as TextBox;
            DropDownList cmbAction = item.FindControl("cmbAction") as DropDownList;
            
            if (chkSelect.Checked)
            {
                BulkDocumentDetails oBulkDocumentDetails = new BulkDocumentDetails
                {
                    Id = (cmbAction.SelectedValue == "1" ? 0 : lstvwBulkDocumentDetails.DataKeys[item.DisplayIndex]["Id"].ToInt()),
                    VehicleId = lstvwBulkDocumentDetails.DataKeys[item.DisplayIndex]["VehicleId"].ToInt(),
                    StartDate = txtStartDate.Text.ToDateTime()                    
                };

                if (ddlDocuments.SelectedValue.ToInt() == DocumentType.Invoice.ToInt() || ddlDocuments.SelectedValue.ToInt() == DocumentType.RCBook.ToInt())
                    oBulkDocumentDetails.EndDate = DateTime.MinValue;
                else
                    oBulkDocumentDetails.EndDate = txtEndDate.Text.ToDateTime();

                lstBulkDocumentDetails.Add(oBulkDocumentDetails);
            }
        }

        string sDates = base.GenerateXml(lstBulkDocumentDetails);
        string sMessage = moBulkDocumentDetailsBL.Validate(ddlDocuments.SelectedValue.ToInt(), sDates);

        if (sMessage != string.Empty)
        {
            ((CustomValidator)sender).ErrorMessage = sMessage;
            e.IsValid = false;
        }
        else
            e.IsValid = true;
    }

    #endregion

    #region Method(s)
    
    /// <summary>
    /// This method is used to save document details.
    /// </summary>
    private void Save()
    {
        List<BulkDocumentDetails> lstBulkDocumentDetails = Populate();
        string sXML = base.GenerateXml(lstBulkDocumentDetails);
        moBulkDocumentDetailsBL.Save(ddlDocuments.SelectedValue.ToInt(), sXML);
        lblUpdateSuccess.Text = S_SAVE_MSG;
        FillListview();
    }

    /// <summary>
    /// This method is used to populate document details.
    /// </summary>
    /// <param name="iId"></param>
    /// <returns></returns>
    private List<BulkDocumentDetails> Populate()
    {
        List<BulkDocumentDetails> lstBulkDocumentDetails = new List<BulkDocumentDetails>();

        foreach (ListViewDataItem item in lstvwBulkDocumentDetails.Items)
        {   
            CheckBox chkSelect = item.FindControl("chkSelect") as CheckBox;            
            TextBox txtTitle = item.FindControl("txtTitle") as TextBox;
            TextBox txtStartDate = item.FindControl("txtStartDate") as TextBox;
            TextBox txtEndDate = item.FindControl("txtEndDate") as TextBox;
            TextBox txtAmount = item.FindControl("txtAmount") as TextBox;
            TextBox txtPolicyNo = item.FindControl("txtPolicyNo") as TextBox;
            TextBox txtDescription = item.FindControl("txtDescription") as TextBox;
            FileUpload flDocument = item.FindControl("flDocument") as FileUpload;
            HiddenField hidDocFile = item.FindControl("hidDocFile") as HiddenField;
            DropDownList cmbAction = item.FindControl("cmbAction") as DropDownList;
            
            if (chkSelect.Checked)
            {
                BulkDocumentDetails oBulkDocumentDetails = new BulkDocumentDetails
                                        {
                                            Id = (cmbAction.SelectedValue == "1"?0: lstvwBulkDocumentDetails.DataKeys[item.DisplayIndex]["Id"].ToInt()),
                                            VehicleId = lstvwBulkDocumentDetails.DataKeys[item.DisplayIndex]["VehicleId"].ToInt(),
                                            Title = txtTitle.Text.Trim(),
                                            StartDate = txtStartDate.Text.ToDateTime(),                                            
                                            Description = txtDescription.Text.Trim(),                                            
                                            FileName = hidDocFile.Value.ToString(),
                                            ActionId = cmbAction.SelectedValue.ToInt()
                                        };

                string sFileName;
                if (flDocument.HasFile)
                {
                    sFileName = SaveFileOnServer(flDocument);
                    oBulkDocumentDetails.FileName = sFileName;                 
                }
                else
                    oBulkDocumentDetails.FileName = hidDocFile.Value;

                if (ddlDocuments.SelectedValue == DocumentType.Insurance.ToInt().ToString())
                {
                    oBulkDocumentDetails.Amount = txtAmount.Text.ToInt();
                    oBulkDocumentDetails.PolicyNo = txtPolicyNo.Text.ToString();
                }
                else
                {
                    oBulkDocumentDetails.Amount = 0; ;
                    oBulkDocumentDetails.PolicyNo = string.Empty;
                }

                if (ddlDocuments.SelectedValue != DocumentType.Invoice.ToInt().ToString() && ddlDocuments.SelectedValue != DocumentType.RCBook.ToInt().ToString())
                    oBulkDocumentDetails.EndDate = txtEndDate.Text.ToDateTime();
                else
                    oBulkDocumentDetails.EndDate = DateTime.MinValue;

                lstBulkDocumentDetails.Add(oBulkDocumentDetails);
            }
        }
        return lstBulkDocumentDetails;
    }

    /// <summary>
    /// This method is used to delete document details from listview.
    /// </summary>
    /// <param name="iId"></param>

    private void DeleteDocumentDetails(int iId)
    {
        moBulkDocumentDetailsBL.DeleteBulkDocument(iId);
        lblUpdateSuccess.Text = S_DELETE_MSG;
    }

    /// <summary>
    /// This method is used to upload the file to the server.
    /// </summary>
    /// <param name="FileUploadPhoto"></param>
    /// <param name="iRowId"></param>
    /// <returns></returns>
    private string SaveFileOnServer(FileUpload aFileUpload)
    {
        string asFileName = aFileUpload.FileName;
        string sFolderName = Server.MapPath("..") + S_DOCUMENT_FOLDER_LOCATION;
        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;

        if (File.Exists(sServerFilePath))
        {
            sFileName = CommonUtility.GetFileNameForRenaming(asFileName);
            sServerFilePath = sFolderName + sFileName;
        }
        aFileUpload.SaveAs(sServerFilePath);

        return sFileName;
    }

    /// <summary>
    /// This method is used to fill document dropdown.
    /// </summary>
    private void FillDocuments()
    {
        VehicleDocumentBL moVehicleDocumentBL = new VehicleDocumentBL();
        List<Documents> lstDocuments = moVehicleDocumentBL.GetDocumentList();
        ListSource.FillDropDownList(lstDocuments, ddlDocuments, "DocumentName", "DocumentId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill document listview.
    /// </summary>
    /// <param name="aiId"></param>
    private void FillListview()
    {
        List<GetBulkDocumentDetails> lstBulkDocumentDetails = moBulkDocumentDetailsBL.GetDocumentsDetails(ddlDocuments.SelectedValue.ToInt(), txtSearch.Text.Trim(), chkShowAll.Checked);
        lstvwBulkDocumentDetails.DataSource = lstBulkDocumentDetails;
        lstvwBulkDocumentDetails.DataBind();
        
		if (lstBulkDocumentDetails != null && lstBulkDocumentDetails.Count > 0)
        {
            LegendTable.Visible = true;
        }
        else
        {
            LegendTable.Visible = false;
        }

        if (ddlDocuments.SelectedValue == DocumentType.Invoice.ToInt().ToString() || ddlDocuments.SelectedValue == DocumentType.RCBook.ToInt().ToString())
            custValEndDate.Enabled = false;
        else
            custValEndDate.Enabled = true;

        if (ddlDocuments.SelectedValue == DocumentType.Insurance.ToInt().ToString())
        {
            custValAmount.Enabled = true;
            custValPolicyNo.Enabled = true;
        }
        else
        {
            custValAmount.Enabled = false;
            custValPolicyNo.Enabled = false;
        }

        if (lstBulkDocumentDetails.Count > 0)
        {
            btnSave.Visible = true;
            btnExport.Visible = true;
            trNote.Visible = true;
            HtmlTableCell thPolicyNo = lstvwBulkDocumentDetails.FindControl("thPolicyNo") as HtmlTableCell;
            if (thPolicyNo != null)
            {
                if (ddlDocuments.SelectedValue == DocumentType.Insurance.ToInt().ToString())
                    thPolicyNo.Visible = true;
                else
                    thPolicyNo.Visible = false;
            }

            HtmlTableCell thAmount = lstvwBulkDocumentDetails.FindControl("thAmount") as HtmlTableCell;
            if (thAmount != null)
            {
                if (ddlDocuments.SelectedValue == DocumentType.Insurance.ToInt().ToString())
                    thAmount.Visible = true;
                else
                    thAmount.Visible = false;
            }

            HtmlTableCell thEndDate = lstvwBulkDocumentDetails.FindControl("thEndDate") as HtmlTableCell;
            if (thEndDate != null)
            {
                if (ddlDocuments.SelectedValue == DocumentType.Invoice.ToInt().ToString() || ddlDocuments.SelectedValue == DocumentType.RCBook.ToInt().ToString())
                    thEndDate.Visible = false;
                else
                    thEndDate.Visible = true;
            }

            HtmlTableCell thEndDateRow = lstvwBulkDocumentDetails.FindControl("thEndDateRow") as HtmlTableCell;
            if (thEndDateRow != null)
            {
                if (ddlDocuments.SelectedValue == DocumentType.Invoice.ToInt().ToString() || ddlDocuments.SelectedValue == DocumentType.RCBook.ToInt().ToString())
                    thEndDateRow.Visible = false;
                else
                    thEndDateRow.Visible = true;
            }

        }
        else
        {
            btnSave.Visible = false;
            btnExport.Visible = false;
            trNote.Visible = false;
        }
    }

    private void SetDefaultValues()
    {
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSumFilter.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnSave.Attributes.Add("onclick", "ResetMessage();");
        base.SetDefaultButton(null);
        LegendTable.Visible = false;
    }

    protected void DocumentDate_Validate(object obj, ServerValidateEventArgs e)
    {
        //foreach (ListViewDataItem item in lstvwBulkDocumentDetails.Items)
        //{
        //    TextBox txtTitle = item.FindControl("txtTitle") as TextBox;
        //    Label lblVehicleNumber = item.FindControl("lblVehicleNumber") as Label;
        //    TextBox txtStartDate = item.FindControl("txtStartDate") as TextBox;
        //    TextBox txtEndDate = item.FindControl("txtEndDate") as TextBox;
        //    bool bIsValid = moBulkDocumentDetailsBL.Validate(ddlDocuments.SelectedValue.ToInt(), lblVehicleNumber.Text.ToString(), txtStartDate.Text, txtEndDate.Text, hidId.Value.ToInt(), string.Empty, 1);

        //    if (!bIsValid)
        //    {
        //        if (ddlDocuments.SelectedItem.Text == "Invoice" || ddlDocuments.SelectedItem.Text == "RC Book")
        //            ((CustomValidator)obj).ErrorMessage = "Start Date should not be duplicate for selected document.";
        //        else
        //            ((CustomValidator)obj).ErrorMessage = "Start Date and End Date should not be duplicate for selected document.";
        //    }

        //    e.IsValid = bIsValid;
        //}
    }

    protected void DocumentTitle_Validate(object obj, ServerValidateEventArgs e)
    {
        //foreach (ListViewDataItem item in lstvwBulkDocumentDetails.Items)
        //{
        //    TextBox txtTitle = item.FindControl("txtTitle") as TextBox;
        //    Label lblVehicleNumber = item.FindControl("lblVehicleNumber") as Label;
        //    bool bIsValid = moBulkDocumentDetailsBL.Validate(ddlDocuments.SelectedValue.ToInt(), lblVehicleNumber.Text.ToString(), string.Empty, string.Empty, hidId.Value.ToInt(), txtTitle.Text.Trim(), 2);
        //    e.IsValid = bIsValid;
        //}
    }

    #endregion

    private enum DocumentType
    {
        Invoice = 1,        
        Insurance = 3,
        RCBook = 4
    }
}