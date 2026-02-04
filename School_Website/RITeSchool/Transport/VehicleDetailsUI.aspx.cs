// File Name  : VehicleDetailsUI.aspx.cs
// Created By : Deepak
// Date       : 7 July 2010
//Description :This class is used to add, eidt, delete vehicle details and also assocaite satff for vehicle. 

using BusinessLogic;
using BusinessLogic.Exceptions;
using BusinessLogic.TransportBL;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using SchoolEntities.Transport;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using Utility;

public partial class VehicleDetailsUI : SchoolBase
{

    #region "CONSTANTS"

    private const string S_DATAKEY_TRANSPORT_STAFF_ID = "TransportStaffId";
    private const string S_DEFAULT_SORT_EXP = "VehicleNumber";

    #endregion

    #region "EVENTS"

    /// <summary>
    /// This event is used check precondition, set javascript attributes, set default values for sorting and error message header,
    /// fill existing satff list view and fill vehicle-satff association listview.
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
                    FillPurchaseOrDetailsCombo();
                    FillTransportStaffList();
                    FillVehicleStaffAssoCiation();
                    SetDefaultValues();
                }
            }
            txtVehicleType.Focus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used save vehicle details depending upon number is duplicated or not 
    /// and at least one satff member should be asssigned to vehicle.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!VehicleDetailsBL.IsDuplicateVehicleNumber(Convert.ToInt32(hidVehicleId.Value), txtVehicleNumber.Text, txtVehicleType.Text,miSchoolId,miAcademicYearId))
            {
                SaveVehicleStaffDetails();
                FillTransportStaffList();
                FillVehicleStaffAssoCiation();
                if (hidVehicleId.Value == Constants.S_ZERO)
                    lblUpdateSucess.Text = "Vehicle details saved successfully !!!";
                else
                    lblUpdateSucess.Text = "Vehicle details updated successfully !!!";
                ClearFields();
                lblUpdateSucess.Visible = true;                
                if (QueryString[Constants.S_IS_CONFIGURED] != Constants.S_YES)
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.VehicleDetails));
            }
            else
            {
                AddSortImage();
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Vehicle Number already exists.";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view page wise vehicle satff association list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwVehicleStaffAsso);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill footer property of vehicle satff association listview and set sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehicleStaffAsso_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwVehicleStaffAsso.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwVehicleStaffAsso, DtPgCount);

                if (moSchool != Constants.SchoolId.SNS)
                {
                    HtmlTableCell th = lstvwVehicleStaffAsso.FindControl("thSync") as HtmlTableCell;
                    if (th != null)
                        th.Visible = false;
                }
            }
            if (IsPostBack)
                AddSortImage();
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to add confirmation message while deleting vehicle details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehicleStaffAsso_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
            }

            Label lblName = e.Item.FindControl("lblName") as Label;

            LinkButton oLinkButton = e.Item.FindControl("lnkUploadDocument") as LinkButton;

            int iVehicleId = Convert.ToInt32(lstvwVehicleStaffAsso.DataKeys[e.Item.DisplayIndex]["VehicleId"]);
            string sQueryString = "VehicleId=" + iVehicleId + "&VehicleNUmber=" + lblName.Text;
            sQueryString = CommonUtility.EncryptQuerystring(sQueryString); 
            oLinkButton.Attributes.Add("onclick", "OpenPopup('" + sQueryString + "'); return false;");

            if (moSchool != Constants.SchoolId.SNS)
            {
                HtmlTableCell tdSync = e.Item.FindControl("tdSync") as HtmlTableCell;
                if (tdSync != null)
                    tdSync.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used to delete, update vehicle details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehicleStaffAsso_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName !=Constants.S_COMMAND_SORT)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iListIndex = oCurrentItem.DisplayIndex;
                int iVehicleId = Convert.ToInt32(lstvwVehicleStaffAsso.DataKeys[iListIndex]["VehicleId"]);
                
                if (e.CommandName == Constants.S_COMMAND_REMOVE)

                    DeleteTransportStaffDetails(iVehicleId);
                else if (e.CommandName == Constants.S_COMMAND_UPDATE)
                    FillControlsForStaffUpdate(iVehicleId);
                else if (e.CommandName == "SYNC")
                {
                    VehicleDetailsBL oVehicleDetailsBL = new VehicleDetailsBL();
                    string sVehicleNo = lstvwVehicleStaffAsso.DataKeys[e.Item.DisplayIndex]["VehicleNumber"].ToString();
                    VehicleMasterToSync oVehicleMasterToSync = oVehicleDetailsBL.GetVehicleDetails(miSchoolId, sVehicleNo);

                    var jsonString = JsonConvert.SerializeObject(oVehicleMasterToSync);

                    try
                    {
                        ASCIIEncoding encoding = new ASCIIEncoding();
                        byte[] ArrMessage = encoding.GetBytes(jsonString);

                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                        HttpWebRequest oRequest = (HttpWebRequest)WebRequest.Create("https://v1.dhundhoo.com/vendor/journeys/refresh?apiKey=32a15db6-a3eb-4629-83db-0e9c41a7d373");
                        oRequest.Method = "POST";
                        oRequest.ContentType = "application/json";
                        oRequest.ContentLength = ArrMessage.Length;
                        Stream oRequestStream = oRequest.GetRequestStream();
                        oRequestStream.Write(ArrMessage, 0, ArrMessage.Length);
                        WebResponse oWebResponse = oRequest.GetResponse();
                        Stream oResponseMessage = oWebResponse.GetResponseStream();
                        using (StreamReader oStreamReader = new StreamReader(oResponseMessage))
                        {
                            var Result = oStreamReader.ReadToEnd();
                            TrackingURLDetails oTrackingURLDetails = JsonConvert.DeserializeObject<TrackingURLDetails>(Result);

                            if (oTrackingURLDetails.thing_id != string.Empty && oTrackingURLDetails.tracking_url != string.Empty)
                            {
                                //oVehicleDetailsBL.UpdateTrackingURL(miSchoolId, miAcademicYearId, miUserId, iVehicleId, oTrackingURLDetails.tracking_url);
                                lblUpdateSucess.Text = "Vehicle is synced successfully !!!";
                                lblUpdateSucess.Visible = true;
                            }
                            else
                            {
                                lblErrorMsg.Text = "Tracking URL is not received.";
                                lblErrorMsg.Visible = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblErrorMsg.Text = "Failed to sync vehicle.";
                        lblErrorMsg.Visible = true;
                        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
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
    /// This event used to check/uncheck satff members depending upon vehicle-satff association.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTransportStaff_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                CheckBox oChkSelect = e.Item.FindControl("ChkSelect") as CheckBox;
                if (Convert.ToInt32(lstvwTransportStaff.DataKeys[iRowId]["VehicleStaffId"].ToString()) > 0)
                    oChkSelect.Checked = true;
                else
                    oChkSelect.Checked = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is selected index change event of dropdownlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPurchaseorhire_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlPurchaseorhire.SelectedValue == Constants.S_ZERO)
            {
                txtCalPopup.Enabled = false;
                txtCalPopup.Text = string.Empty;
            }
            else
                txtCalPopup.Enabled = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
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
            ClearFields();
            FillTransportStaffList();
            FillVehicleStaffAssoCiation();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search vehicle.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillVehicleStaffAssoCiation();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort the listview of vehicle staff association.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwVehicleStaffAsso_Sorting(object sender, ListViewSortEventArgs e)
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

    #endregion

    #region "PRIVATE METHODS"

    /// <summary>
    /// This method gets all transport satff members and fill transport satff listview.
    /// </summary>
    private void FillTransportStaffList()
    {
        DataTable oDTTransportStaff = VehicleDetailsBL.GetAllStaffMembers(miSchoolId,miAcademicYearId);
        if (oDTTransportStaff != null && oDTTransportStaff.Rows.Count > 0)
        {
            divContainer.Visible = true;
            lstvwTransportStaff.DataSource = oDTTransportStaff;
            lstvwTransportStaff.DataBind();
        }
        else
            divContainer.Visible = false;
    }

    /// <summary>
    /// This method is used set datasource vehicle-satff association listview.
    /// </summary>
    private void FillVehicleStaffAssoCiation()
    {
        lstvwVehicleStaffAsso.DataSourceID = ObjDSVehicleStaffDetails.ID;
        lstvwVehicleStaffAsso.DataBind();
    }

    /// <summary>
    /// This method is used to set default values for sorting and error message header.
    /// </summary>
    private void SetDefaultValues()
    {

        hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidSortDirection.Value = Constants.S_ASCENDING;
        HtmlTableRow oHtmlTableHeaderRow = lstvwVehicleStaffAsso.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> {btnCancel, btnSave,btnBack});
        btnSave.Attributes["onclick"] = "ResetUpdateLbl()";
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Transport_Releted));
    }

    /// <summary>
    /// This Method is used to clear form fields.
    /// </summary>
    private void ClearFields()
    {
        txtCapacity.Text = string.Empty;
        txtManufacturer.Text = string.Empty;
        txtVehicleNumber.Text = string.Empty;
        txtVehicleType.Text = string.Empty;
        hidVehicleId.Value = "0";
        CheckBox oChkHeader = (CheckBox)lstvwTransportStaff.FindControl("ChkSelectAll");
        oChkHeader.Checked = false;
        txtEngineNo.Text = string.Empty;
        txtChassisNo.Text = string.Empty;
        rbtPetrol.Checked = true;
        rbtDiesel.Checked = false;
        ddlPurchaseorhire.SelectedValue = Constants.S_ZERO;
        txtCalPopup.Enabled = false;
        txtCalPopup.Text = string.Empty;
        txtTrackingURL.Text = string.Empty;
        txtOffcialMobNo.Text = string.Empty;
        txtRFID.Text = string.Empty;
    }

    /// <summary>
    /// This method used to save vehicle details with at least one satff member associated with vehicle.
    /// </summary>
    /// <returns></returns>
    private void SaveVehicleStaffDetails()
    {
        VehicleDetailsBL oVehicleDetailsBL = PopulateVehicleBL();
        string StaffXML = GetVehicleStaffAssociationXML();
        oVehicleDetailsBL.VehicleId = Convert.ToInt32(hidVehicleId.Value);
        oVehicleDetailsBL.Save(StaffXML);

        if (ConfigurationManager.AppSettings["TransportExternalDBName"] != null && ConfigurationManager.AppSettings["TransportExternalDBName"].ToString() != string.Empty)
        {
            string sDBName = ConfigurationManager.AppSettings["reportdatabasename"].ToString();
            string sTransportDBName = ConfigurationManager.AppSettings["TransportExternalDBName"].ToString();
            TransferTransportDetailsBL oTransferTransportDetailsBL = new TransferTransportDetailsBL(miSchoolId, sDBName, sTransportDBName);
            oTransferTransportDetailsBL.UpdateAttendantRFIDDetails(oVehicleDetailsBL.VehicleNumber, oVehicleDetailsBL.RFID);
        }
    }
    /// <summary>
    /// This method create XML of selected Transport satff members.
    /// </summary>
    /// <returns></returns>
    private string GetVehicleStaffAssociationXML()
    {
        CheckBox oChkIsStaffSelected;
        const string S_ELEMENT = "element";
        string sAttribute;
        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement oRoot = oDoc.CreateElement("VehicleStaff");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "VehicleStaff", "");
        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount <= lstvwTransportStaff.Items.Count - 1; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwTransportStaff.Items[iRowCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            int iTransportStaffId = Convert.ToInt32(lstvwTransportStaff.DataKeys[iRowId][S_DATAKEY_TRANSPORT_STAFF_ID]);
            int iVehicleStaffId = Convert.ToInt32(lstvwTransportStaff.DataKeys[iRowId]["VehicleStaffId"]);
            oChkIsStaffSelected = (CheckBox)oCurrentItem.FindControl("ChkSelect");

            if ((oChkIsStaffSelected.Checked == true && iVehicleStaffId == 0) || iVehicleStaffId > 0)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "VehicleStaff", "");
                sAttribute = "TransportStaffId";
                XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = iTransportStaffId.ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "VehicleStaffId";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = iVehicleStaffId.ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Is_Deleted";
                oAttr = oDoc.CreateAttribute(sAttribute);
                if (oChkIsStaffSelected.Checked)
                    oAttr.Value = "0";
                else
                    oAttr.Value = "1";
                oXmlNode.Attributes.Append(oAttr);
                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
        }
        // Add the root node to document element.         
        oRoot.AppendChild(oXmlRootNode);
        return oRoot.InnerXml;
    }


    /// <summary>
    /// This method create VehicleDetailsBL object, set its properties and returns VehicleDetailsBL object.
    /// </summary>
    /// <returns></returns>
    private VehicleDetailsBL PopulateVehicleBL()
    {
        VehicleDetailsBL oVehicleDetailsBL = new VehicleDetailsBL();
        oVehicleDetailsBL.VehicleNumber = txtVehicleNumber.Text.Trim();
        oVehicleDetailsBL.VehicleType = txtVehicleType.Text.Trim();
        oVehicleDetailsBL.ManufacturerName = txtManufacturer.Text.Trim();
        oVehicleDetailsBL.VehicleCapacity = Convert.ToInt32(txtCapacity.Text);
        oVehicleDetailsBL.InsertedById = miUserId;
        oVehicleDetailsBL.SchoolId = miSchoolId;
        oVehicleDetailsBL.Academic_Year_Id = miAcademicYearId;
        oVehicleDetailsBL.EngineNumber = txtEngineNo.Text.Trim();
        oVehicleDetailsBL.ChassisNumber = txtChassisNo.Text.Trim();
        oVehicleDetailsBL.FuelType = (rbtPetrol.Checked == true) ? 1 : 2;
        oVehicleDetailsBL.PurchaseOrHire = ddlPurchaseorhire.SelectedValue.ToInt();
        oVehicleDetailsBL.TrackingURL = txtTrackingURL.Text.Trim();
        oVehicleDetailsBL.OfficialMobileNo = txtOffcialMobNo.Text.Trim();
        oVehicleDetailsBL.RFID = txtRFID.Text.Trim();

        if (txtCalPopup.Text != string.Empty)            
            oVehicleDetailsBL.PurchaseDate = txtCalPopup.Text.ToDateTime();
        return oVehicleDetailsBL;
    }

    /// <summary>
    /// This method is used to set controls to update vehicle details.
    /// </summary>
    /// <param name="iVehicleId"></param>
    /// <param name="iSchoolID"></param>
    /// <param name="iAcademicYearId"></param>
    private void FillControlsForStaffUpdate(int aiVehicleId )
    {
        DataSet oDSVehicleStaffDetails = VehicleDetailsBL.GetVehicleStaffForUpdate(aiVehicleId, miSchoolId, miAcademicYearId);
        if (oDSVehicleStaffDetails != null && oDSVehicleStaffDetails.Tables.Count > 0)
        {
            lstvwTransportStaff.DataSource = oDSVehicleStaffDetails.Tables[0];
            lstvwTransportStaff.DataBind();
            txtCapacity.Text = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["VehicleCapacity"]);
            txtVehicleNumber.Text = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["VehicleNumber"]);
            txtManufacturer.Text = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["ManufacturerName"]);
            txtVehicleType.Text = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["VehicleType"]);
            hidVehicleId.Value = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["VehicleId"]);
            txtEngineNo.Text = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["EngineNumber"]).Trim();
            txtChassisNo.Text = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["ChassisNumber"]).Trim();
            txtTrackingURL.Text = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["TrackingURL"]).Trim();
            txtOffcialMobNo.Text = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["OfficialMobileNo"]).Trim();
            txtRFID.Text = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["AttendantRFID"]).Trim();
            string FuelTypeId = Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["FuelType"]);
            if (FuelTypeId == "2")
            {
                rbtDiesel.Checked = true;
                rbtPetrol.Checked = false;
            }
            else
            {
                rbtPetrol.Checked = true;
                rbtDiesel.Checked = false;
            }
            ddlPurchaseorhire.SelectedValue = (Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["PurchaseOrHire"]) == string.Empty) ? Constants.S_ZERO : Convert.ToString(oDSVehicleStaffDetails.Tables[1].Rows[0]["PurchaseOrHire"]);

            if (oDSVehicleStaffDetails.Tables[1].Rows[0]["PurchaseDate"].ToString() != string.Empty)
            {
                txtCalPopup.Enabled = true;
                CalPopup.DateValue = Convert.ToDateTime(oDSVehicleStaffDetails.Tables[1].Rows[0]["PurchaseDate"]);
                txtCalPopup.Text = CalPopup.DateValue.ToString("dd-MMM-yyyy");
            }
            else
            {
                if (ddlPurchaseorhire.SelectedValue == Constants.S_ZERO)
                {
                    txtCalPopup.Enabled = false;
                    txtCalPopup.Text = string.Empty;
                }
                else
                    txtCalPopup.Enabled = true;
            }

            CheckBox oChkHeader = (CheckBox)lstvwTransportStaff.FindControl("ChkSelectAll");
            oChkHeader.Checked = false;
            AddSortImage();
        }

    }

    /// <summary>
    /// This method is used to delete exisiting vehicle details as well as it checks dependancy of vehicle with Route-Shift-Timing Details.
    /// And also checks if at least one vehicle's details has been configured or not.
    /// </summary>
    /// <param name="iVehicleId"></param>
    /// <param name="iSchoolID"></param>
    /// <param name="iAcademicYearId"></param>
    private void DeleteTransportStaffDetails(int aiVehicleId )
    {
        VehicleDetailsBL oVehicleDetailsBL = new VehicleDetailsBL();
        int iRowCount = 0;
        DataTable oDTMsg = oVehicleDetailsBL.DeleteVehicleDetails(aiVehicleId, miSchoolId, miAcademicYearId, out iRowCount);
        if (oDTMsg != null && oDTMsg.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(oDTMsg.Rows[0]["Msg"])))
        {
            AddSortImage();
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Vehicle " + Convert.ToString(oDTMsg.Rows[0]["Msg"]) + " can not be deleted since associated with Route-Shift-Timing Details.";
            ClearFields();
        }
        else
        {
            if (iRowCount == 0)
                // This method is used to delete vehicle configuration details.
                DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.VehicleDetails));
            lblUpdateSucess.Text = "Vehicle details deleted successfully !!!";
            lblUpdateSucess.Visible = true;
            ClearFields();
            FillVehicleStaffAssoCiation();
            FillTransportStaffList();
        }

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
        if (lstvwVehicleStaffAsso.SortDirection.ToString() == "Ascending" || lstvwVehicleStaffAsso.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwVehicleStaffAsso.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwVehicleStaffAsso.SortExpression.ToString();
        else
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        HtmlTableRow oHtmlTableHeaderRow = lstvwVehicleStaffAsso.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to check pre-condition to configure association.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.VehicleDetails);
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
    /// This method used hide form controls.
    /// </summary>
    private void HideControls()
    {
        tblVehicleDetails.Visible = false;
        tblStaff.Visible = false;
        trSave.Visible = false;
        trDataPager.Visible = false;
        lstvwVehicleStaffAsso.Visible = false;
    }

    /// <summary>
    /// This method is used to fill PurchaseOrDetails dropdownlist.
    /// </summary>
    private void FillPurchaseOrDetailsCombo()
    {
        VehicleDetailsBL oVehicleDeatailsBL = new VehicleDetailsBL();
        List<VehiclePurchaseOrHireDetails> lstVehiclePurchaseOrHireDetails = oVehicleDeatailsBL.GetAllVehicleDetails();
        ListSource.FillDropDownList(lstVehiclePurchaseOrHireDetails, ddlPurchaseorhire, "VehicleName", "VehicleId",Constants.S_SELECT);
        if (ddlPurchaseorhire.SelectedValue == Constants.S_ZERO)
            txtCalPopup.Enabled = false;
        else
            txtCalPopup.Enabled = true;
    }
    
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string sVehicleDetails = string.Empty;

            sVehicleDetails = "(usp_ExportVehicleDetails.SchoolId}=" + miSchoolId + " AND  usp_ExportVehicleDetails.AcademicYearId} =" + miAcademicYearId  + ")" + "@ ";
            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.ExportVehicleDetails, sVehicleDetails, ExportFormatType.Excel);
            oReportDisplay.DisplayReport();
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
}