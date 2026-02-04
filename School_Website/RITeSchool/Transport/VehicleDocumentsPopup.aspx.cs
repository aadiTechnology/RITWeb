/*
 * File Name - VehicleDocumentPopup.aspx.cs
 * Created By - Vishakha
 * Created Date - 3-November-2022
 * Descrption - This class is used to upload and delete Vehicle documents.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using TransportEntities;
using Utility;

public partial class VehicleDocumentsPopup : SchoolBase
{
    #region Constants

    private const string S_VEHICLE_DOCUMENT_FOLDER_LOCATION = "\\DOWNLOADS\\TransportModule\\VehicleDocuments\\";
    private const int I_FILE_SIZE_LIMIT = 5242880; // nearly 5 mb
    private const string S_COMMAND_UPDATE = "UpdateVehicleDocumentDetails";
    private const string S_COMMAND_DELETE = "DeleteVehicleDocumentDetails";
    private const string S_DELETE_MSG = "Vehicle Document Details deleted successfully !!!";
    private const string S_SAVED_MSG = "Vehicle Document Details saved successfully !!!";

    #endregion

    #region Data Member(s)

    private VehicleDocumentBL moVehicleDocumentBL;

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
                hidSortExpression.Value = "DocumentName";
                hidSortDirection.Value = Constants.S_DESCENDING;
            }

            AddSortImage(lstvwDocuments, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set default values, fill documents in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moVehicleDocumentBL = new VehicleDocumentBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                FillDocuments();
                ReadQueryString();
                SetDefaultValues();
                FillDocumentsListView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill pager footer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwDocuments_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwDocuments.Items.Count > 0)
                ControlUtility.FillListViewPagerFooter(lstvwDocuments, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
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
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwDocuments);
            //DataPager oDataPager = lstvwDocuments.FindControl("DtPgDropDown") as DataPager;
            //if (oDataPager != null)
            //{
            //    DropDownList ddlCnt = oDataPager.Controls[0].FindControl("ddlCnt") as DropDownList;
            //    if (ddlCnt != null)
            //        hidPageNo.Value = ddlCnt.SelectedValue;
            //}
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set attribute on listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwDocuments_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                ImageButton imgBtnDelete = oCurrentItem.FindControl("imgBtnDelete") as ImageButton;
                imgBtnDelete.Attributes.Add("Onclick", "if(!ConfirmDelete()) return false;");

                GetVehicleDocumentDetails oVehicleDocument = oCurrentItem.DataItem as GetVehicleDocumentDetails;

                ImageButton btnView = oCurrentItem.FindControl("btnView") as ImageButton;
                string sPath = "../downloads/TransportModule/VehicleDocuments/" + oVehicleDocument.FileName;
                btnView.Attributes.Add("Onclick", "OpenFile('" + sPath + "'); return false;");

                Label lblStartDate = oCurrentItem.FindControl("lblStartDate") as Label;
                lblStartDate.Text = oVehicleDocument.StartDate.ToString(Constants.S_DATE_FORMAT);

                Label lblEndDate = oCurrentItem.FindControl("lblEndDate") as Label;
                if (oVehicleDocument.EndDate != DateTime.MinValue)
                    lblEndDate.Text = oVehicleDocument.EndDate.ToString(Constants.S_DATE_FORMAT);
                else
                    lblEndDate.Text = "-";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to save Vehicle document.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                string asFileName;
                if (SaveFileToServer(out asFileName))
                {
                    VehicleDocumentDetails oVehicleDocumentDetails = Populate(asFileName);
                    moVehicleDocumentBL.SaveDocument(oVehicleDocumentDetails);
                    lblMessage.Text = S_SAVED_MSG;
                    ClearFields();
                    FillDocumentsListView();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to update,remove document.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwDocuments_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iId = Convert.ToInt32(lstvwDocuments.DataKeys[e.Item.DisplayIndex]["Id"]);

                if (e.CommandName == S_COMMAND_UPDATE)
                {
                    imgbtnView.Visible = true;
                    GetVehicleDocumentDetails oGetVehicleDocumentDetails = moVehicleDocumentBL.Get(iId);
                    ddlDocuments.SelectedValue = oGetVehicleDocumentDetails.DocumentId.ToString();
                    txtStartDate.Text = oGetVehicleDocumentDetails.StartDate.ToString(Constants.S_DATE_FORMAT);

                    if (oGetVehicleDocumentDetails.EndDate != DateTime.MinValue)
                        txtEndDate.Text = oGetVehicleDocumentDetails.EndDate.ToString(Constants.S_DATE_FORMAT);

                    txtPolicyNo.Text = oGetVehicleDocumentDetails.PolicyNo.ToString();
                    txtAmount.Text = oGetVehicleDocumentDetails.Amount.ToString();
                    txtTitle.Text = oGetVehicleDocumentDetails.Title.ToString();
                    txtDescription.Text = oGetVehicleDocumentDetails.Description.ToString();
                    hidId.Value = iId.ToString();

                    string sPath = "../downloads/TransportModule/VehicleDocuments/" + oGetVehicleDocumentDetails.FileName;
                    imgbtnView.Attributes.Add("Onclick", "OpenFile('" + sPath + "'); return false;");

                    hidFileUpload.Value = oGetVehicleDocumentDetails.FileName;
                }
                else if (e.CommandName == S_COMMAND_DELETE)
                {
                    DeleteDocument(iId);
                    ClearFields();

                    string sFileName = Convert.ToString(lstvwDocuments.DataKeys[e.Item.DisplayIndex]["FileName"]);
                    string sServerFilePath = Server.MapPath("..") + S_VEHICLE_DOCUMENT_FOLDER_LOCATION + "\\" + sFileName;

                    if (File.Exists(sServerFilePath))
                        File.Delete(sServerFilePath);

                    FillDocumentsListView();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill document dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillDocumentsListView();

            if (ddlDocuments.SelectedItem.Text != "Insurance")
            {
                txtAmount.Text = string.Empty;
                txtPolicyNo.Text = string.Empty;
                trAmount.Visible = false;
                trPolicy.Visible = false;

                reqValPolicyNo.Enabled = false;
                reqValAmount.Enabled = false;
            }
            else
            {
                trAmount.Visible = true;
                trPolicy.Visible = true;
                reqValPolicyNo.Enabled = true;
                reqValAmount.Enabled = true;
            }

            if (ddlDocuments.SelectedItem.Text == "Invoice" || ddlDocuments.SelectedItem.Text == "RC Book")
            {
                trEndDate.Visible = false;
                reqValEndDate.Enabled = false;
            }
            else
            {
                trEndDate.Visible = true;
                reqValEndDate.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cancel saving.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void DocumentDate_Validate(object obj, ServerValidateEventArgs e)
    {
        bool bIsValid = moVehicleDocumentBL.Validate(ddlDocuments.SelectedValue.ToInt(), hidVehicleId.Value.ToInt(), txtStartDate.Text, txtEndDate.Text, hidId.Value.ToInt(), string.Empty, string.Empty, 1);

        if (!bIsValid)
        {
            if (ddlDocuments.SelectedItem.Text == "Invoice" || ddlDocuments.SelectedItem.Text == "RC Book")
                ((CustomValidator)obj).ErrorMessage = "Start Date should not be duplicate for selected document.";
            else
                ((CustomValidator)obj).ErrorMessage = "Start Date and End Date should not be duplicate for selected document.";
        }

        e.IsValid = bIsValid;
    }

    protected void DocumentTitle_Validate(object obj, ServerValidateEventArgs e)
    {
        bool bIsValid = moVehicleDocumentBL.Validate(ddlDocuments.SelectedValue.ToInt(), hidVehicleId.Value.ToInt(), string.Empty, string.Empty, hidId.Value.ToInt(), txtTitle.Text.Trim(), string.Empty, 2);

        e.IsValid = bIsValid;
    }

    protected void DocumentInsuranceDetails_Validate(object obj, ServerValidateEventArgs e)
    {
        if (ddlDocuments.SelectedItem.Text == "Insurance")
        {
            bool bIsValid = moVehicleDocumentBL.Validate(ddlDocuments.SelectedValue.ToInt(), hidVehicleId.Value.ToInt(), string.Empty, string.Empty, hidId.Value.ToInt(), string.Empty, txtPolicyNo.Text, 3);

            e.IsValid = bIsValid;
        }
        else
            e.IsValid = true;
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to set default values to fields.
    /// </summary>
    private void SetDefaultValues()
    {
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ApplyMouseHoverEffect(new List<Button> { BtnSave, btnClose });
        BtnSave.Attributes.Add("onclick", "ResetMessage();");
        imgbtnView.Visible = false;
        ddlDocuments.SelectedValue = "1";
        txtStartDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        flDocument.Focus();
    }

    /// <summary>
    /// This method is used to validate file size.
    /// </summary>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private bool SaveFileToServer(out string asFileName)
    {
        if (flDocument.HasFile)
        {
            if (flDocument.FileContent.Length > I_FILE_SIZE_LIMIT)
            {
                asFileName = flDocument.FileName;
                return false;
            }

            string sFileName = flDocument.FileName;
            string sRenamedFileName = sFileName;
            string sFolderName = Server.MapPath("..") + S_VEHICLE_DOCUMENT_FOLDER_LOCATION;
            string sServerFilePath = sFolderName + sFileName;
            asFileName = sFileName;

            if (File.Exists(sServerFilePath))
            {
                sRenamedFileName = CommonUtility.GetFileNameForRenaming(sFileName);
                asFileName = sRenamedFileName;
            }

            sServerFilePath = sFolderName + sRenamedFileName;
            flDocument.SaveAs(sServerFilePath);
        }
        else
            asFileName = hidFileUpload.Value;
        return true;
    }

    /// <summary>
    /// This method is used to fill document dropdown.
    /// </summary>
    private void FillDocuments()
    {
        List<Documents> lstDocuments = moVehicleDocumentBL.GetDocumentList();
        ListSource.FillDropDownList(lstDocuments, ddlDocuments, "DocumentName", "DocumentId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to delete documents.
    /// </summary>
    private void DeleteDocument(int iId)
    {
        moVehicleDocumentBL.DeleteDocument(iId);
        lblMessage.Text = S_DELETE_MSG;
    }

    /// <summary>
    /// This method is used to fill document listview.
    /// </summary>
    /// <param name="aiVehicleId"></param>
    /// <param name="aiDocumentId"></param>
    private void FillDocumentsListView()
    {
        lstvwDocuments.DataSourceID = objdsDocumentDetails.ID;
        lstvwDocuments.DataBind();
    }

    /// <summary>
    /// This method is used t clear fields.
    /// </summary>
    private void ClearFields()
    {
        hidId.Value = "0";
        txtTitle.Text = string.Empty;
        txtDescription.Text = string.Empty;
        txtAmount.Text = string.Empty;
        txtPolicyNo.Text = string.Empty;
        hidFileUpload.Value = string.Empty;
        imgbtnView.Visible = false;
        txtStartDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
    }

    /// <summary>
    /// This method is used to read querystring.
    /// </summary>
    private void ReadQueryString()
    {
        hidVehicleId.Value = Convert.ToString(QueryString["VehicleId"]);
        lblVehicleNumber.Text = QueryString["VehicleNumber"].ToString();
    }

    /// <summary>
    /// This method is used to populate fields.
    /// </summary>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private VehicleDocumentDetails Populate(string asFileName)
    {
        VehicleDocumentDetails oVehicleDocumentDetails = new VehicleDocumentDetails();

        oVehicleDocumentDetails.VehicleId = Convert.ToInt32(QueryString["VehicleId"]);
        oVehicleDocumentDetails.DocumentId = ddlDocuments.SelectedValue.ToInt();
        oVehicleDocumentDetails.Title = txtTitle.Text.Trim();
        oVehicleDocumentDetails.Description = txtDescription.Text.Trim();
        oVehicleDocumentDetails.StartDate = txtStartDate.Text.ToDateTime();
        oVehicleDocumentDetails.Id = hidId.Value.ToInt();
        oVehicleDocumentDetails.FileName = asFileName;

        if (txtEndDate.Text != string.Empty)
            oVehicleDocumentDetails.EndDate = txtEndDate.Text.ToDateTime();
        else
            oVehicleDocumentDetails.EndDate = DateTime.MinValue;

        if (ddlDocuments.SelectedItem.Text == "Insurance")
        {
            oVehicleDocumentDetails.Amount = txtAmount.Text.ToInt();
            oVehicleDocumentDetails.PolicyNo = txtPolicyNo.Text;
        }
        else
        {
            oVehicleDocumentDetails.Amount = 0;
            oVehicleDocumentDetails.PolicyNo = "";
        }
        return oVehicleDocumentDetails;
    }

    #endregion
}