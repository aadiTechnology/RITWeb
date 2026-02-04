// File Name  : RouteDetailsUI.aspx.cs
// Created By : Deepak
//Modified By :Pravin
// Date       : 9 July 2010
//Description :This class is used to add, eidt, delete route details and also assocaite stops for route. 

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class RouteDetailsUI : SchoolBase
{
    #region "CONSTANTS"

    private const string S_DATAKEY_STOP_ID = "miStopId";
    private const string S_DEFAULT_SORT_EXP = "RouteName";
    private const string S_FOLDER_LOCATION = "RITeSchool\\DOWNLOADS\\Route Map Picture\\";
    private const string S_FOLDER_PATH = @"../DOWNLOADS/Route Map Picture/";
    private const string S_FILE_NOT_FOUND = "File does not exists.";
    private const int I_FILE_SIZE_LIMIT = 1048576;  // File limit is 1 MB
    private const string S_FILE_SIZE_ERROR = "Size of file is too large.";
    private RouteDetailsBL moRouteDetailsBL;

    #endregion

    #region "EVENTS"

    /// <summary>
    /// This event is used check precondition, set javascript attributes, set default values for sorting and error message header,
    /// fill existing stop list view and fill route-stop association listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moRouteDetailsBL = new RouteDetailsBL(miSchoolId, miAcademicYearId, miUserId);            
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                if (CheckPreCondition())
                {
                    MandatoryNonMandatoryFields();
                    hidFilePath.Value = Constants.S_ZERO;
                    FillStopsList();
                    FillRouteStopAssociation();
                    SetDefaultValues();
                }
            }
            txtRouteName.Focus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used save route details depending upon route name is duplicated or not 
    /// and at least one stop should be asssigned to route.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Text = "";
            if (!RouteDetailsBL.IsDuplicateRouteName(Convert.ToInt32(hidRouteId.Value), txtRouteName.Text,miSchoolId,miAcademicYearId))
            {
                SaveRouteStopDetails();
                if (lblErrorMsg.Text == string.Empty)
                {
                    FillStopsList();
                    FillRouteStopAssociation();
                    ClearFields();
                    lblUpdateSucess.Visible = true;
                    lblUpdateSucess.Text = "Route details saved successfully !!!";
                    // This Method is used to decrypt query string.
                    if (QueryString[Constants.S_IS_CONFIGURED] != Constants.S_YES)
                        SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.RouteDetails));
                }
            }
            else
            {
                AddSortImage();
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Route Name already exists.";                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view page wise route-stop association.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwRouteStopAsso);

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill footer property of route-stop association listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwRouteStopAsso_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwRouteStopAsso.Items.Count > 0)
                ControlUtility.FillListViewPagerFooter(lstvwRouteStopAsso, DtPgCount);
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
    /// This event is used to add confirmation message while deleting route details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwRouteStopAsso_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {            
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
                ImageButton oimgbtnView = e.Item.FindControl("imgbtnView") as ImageButton;
                DataCommunicator.RouteDetailsDC.RouteDetails olRouteDetailsBL = e.Item.DataItem as DataCommunicator.RouteDetailsDC.RouteDetails;
                string sLink = olRouteDetailsBL.msLinkUrl.ToString();
                if (string.IsNullOrEmpty(sLink))
                {
                    oimgbtnView.Visible = false;
                }
                else
                {
                    string sNewFileName = S_FOLDER_PATH + sLink;
                    oimgbtnView.Attributes.Add("onclick", "OpenWindow('" + sNewFileName + "'); return false;");
                    oimgbtnView.Visible = true;
                }            
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used to delete, update route details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwRouteStopAsso_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName != Constants.S_COMMAND_SORT)
            {
                ListViewDataItem ocurrentItem = (ListViewDataItem)e.Item;
                int iListIndex = ocurrentItem.DisplayIndex;
                int iRouteId = Convert.ToInt32(lstvwRouteStopAsso.DataKeys[iListIndex]["miRouteId"]);
                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                    DeleteRouteDetails(iRouteId);
                else if (e.CommandName == Constants.S_COMMAND_UPDATE)
                    FillControlsForRouteUpdate(iRouteId);
            }
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
            FillStopsList();
            FillRouteStopAssociation();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sort the listview of route-stop association.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwRouteStopAsso_Sorting(object sender, ListViewSortEventArgs e)
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
    /// This method is used to check file size and then check correct file to specified location
    /// </summary>
    private string UploadNoticeFile(out string asFileName)
    {
        asFileName = string.Empty;
        if(fileUploadItems.FileName.TrimAll() != string.Empty)
        {
        hidFilePath.Value = fileUploadItems.FileName.ToString();
        }
        if (hidFilePath.Value != string.Empty)
        {
            string sReturnErrorMsg = string.Empty;
            string sServerPath = Server.MapPath("~");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";
            if (hidRouteId.Value == Constants.S_ZERO)
            {               
                if (fileUploadItems.HasFile)
                {
                    if (fileUploadItems.PostedFile.ContentLength <= I_FILE_SIZE_LIMIT)
                    {
                        string sLinkName = CommonUtility.GetFileNameForRenaming(fileUploadItems.FileName.ToString());
                        string sLinkPath = sServerPath + S_FOLDER_LOCATION + sLinkName;
                        fileUploadItems.SaveAs(sLinkPath);
                        asFileName = sLinkName;
                    }
                    else
                    {
                        sReturnErrorMsg = S_FILE_SIZE_ERROR;
                    }
                }
            }
            else
            {
                string sLinkName;
                if (fileUploadItems.HasFile)
                {
                    if (fileUploadItems.PostedFile.ContentLength <= I_FILE_SIZE_LIMIT)
                    {
                        sLinkName = CommonUtility.GetFileNameForRenaming(hidFilePath.Value);
                        string sLinkPath = sServerPath + S_FOLDER_LOCATION + sLinkName;
                        fileUploadItems.SaveAs(sLinkPath);
                        asFileName = sLinkName;
                    }
                    else
                    {
                        sReturnErrorMsg = S_FILE_SIZE_ERROR;                        
                    }
                }
                else if (hidFilePath.Value != Constants.S_ZERO)
                {
                    sLinkName = hidFilePath.Value;
                    string sLinkPath = sServerPath + S_FOLDER_LOCATION + sLinkName;
                    asFileName = sLinkName;
                }                
            }
            return sReturnErrorMsg;
        }
        return string.Empty;
    }

    /// <summary>
    /// This method gets all stops and fill stops listview.
    /// </summary>
    private void FillStopsList()
    {
        RouteDetailsBL oRouteDetailsBL = new RouteDetailsBL();
        oRouteDetailsBL.RouteDetails = RouteDetailsBL.GetAllStops(miSchoolId,miAcademicYearId);
        lstvwStops.DataSource = oRouteDetailsBL.RouteDetails;
        lstvwStops.DataBind();
    }

    /// <summary>
    /// This method is used set datasource route-stop association listview.
    /// </summary>
    private void FillRouteStopAssociation()
    {
        lstvwRouteStopAsso.DataSourceID = ObjDSRouteStopDetails.ID;
        lstvwRouteStopAsso.DataBind();
    }

    /// <summary>
    /// This method used to save route details with at least one stop associated with route.
    /// </summary>
    /// <returns></returns>
    private void SaveRouteStopDetails()
    {
        RouteDetailsBL oRouteDetailsBL = PopulateRouteBL();
        List<RouteDetailsBL> olstRouteDetailsBL=new List<RouteDetailsBL>();
        olstRouteDetailsBL.Add(oRouteDetailsBL);
        string StopXML = GetRouteStopAssociationXML();
        oRouteDetailsBL.RouteId = Convert.ToInt32(hidRouteId.Value);
        string sMessage=oRouteDetailsBL.Save(StopXML);
        if (sMessage != string.Empty)
        {
            sMessage = sMessage.Substring(1, sMessage.Length - 1);
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Stop(s) '" + sMessage + "' can not be deleted since associated with user.";
        }
    }

    /// <summary>
    /// This method create XML of selected stops.
    /// </summary>
    /// <returns></returns>
    private string GetRouteStopAssociationXML()
    {
        CheckBox oChkIsStopSelected;
        const string S_ELEMENT = "element";
        string sAttribute;
        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement oRoot = oDoc.CreateElement("RouteStop");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "RouteStop", "");
        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount <= lstvwStops.Items.Count - 1; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwStops.Items[iRowCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            int iStopId = Convert.ToInt32(lstvwStops.DataKeys[iRowId][S_DATAKEY_STOP_ID]);
            int iRouteStopId = Convert.ToInt32(lstvwStops.DataKeys[iRowId]["miRouteStopId"]);
            oChkIsStopSelected = (CheckBox)oCurrentItem.FindControl("ChkSelect");
            if ((oChkIsStopSelected.Checked && iRouteStopId == 0) || iRouteStopId > 0)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "RouteStop", "");
                sAttribute = "StopId";
                XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = iStopId.ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "RouteStopId";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = iRouteStopId.ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Is_Deleted";
                oAttr = oDoc.CreateAttribute(sAttribute);
                if (oChkIsStopSelected.Checked)
                    oAttr.Value = Constants.S_ZERO;
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
    /// This method create RouteDetailsBL object, set its properties and returns RouteDetailsBL object.
    /// </summary>
    /// <returns></returns>
    private RouteDetailsBL PopulateRouteBL()
    {
        RouteDetailsBL oRouteDetailsBL = new RouteDetailsBL();
        oRouteDetailsBL.RouteName = txtRouteName.Text.Trim();
        oRouteDetailsBL.InsertedById = miUserId;
        oRouteDetailsBL.SchoolId = miSchoolId;
        oRouteDetailsBL.Academic_Year_Id = miAcademicYearId;
        oRouteDetailsBL.RouteNo = txtRouteNo.Text;        
        oRouteDetailsBL.JourneyTypeId = 0;
        oRouteDetailsBL.StartTime = string.Empty;
        oRouteDetailsBL.EndTime = string.Empty;

        string sLinkName;
        string sFileUploadErr = UploadNoticeFile(out sLinkName);
        if (string.IsNullOrEmpty(sFileUploadErr))
        {        
            oRouteDetailsBL.LinkUrl = sLinkName;
        }
        else
        {
            lblError.Text = sFileUploadErr;
        }        
        return oRouteDetailsBL;
    }

    /// <summary>
    /// This method is used to set controls to update route details.
    /// </summary>
    /// <param name="aiRouteId"></param>
    /// <param name="aiSchoolID"></param>
    /// <param name="aiAcademicYearId"></param>
    private void FillControlsForRouteUpdate(int aiRouteId)
    {
        DataSet oDSRouteStopDetails = RouteDetailsBL.GetRouteStopsForUpdate(aiRouteId, miSchoolId, miAcademicYearId);
        if (oDSRouteStopDetails != null && oDSRouteStopDetails.Tables.Count > 0)
        {
            txtRouteName.Text = Convert.ToString(oDSRouteStopDetails.Tables[1].Rows[0]["RouteName"]);
            hidRouteId.Value = Convert.ToString(oDSRouteStopDetails.Tables[1].Rows[0]["RouteId"]);
            CheckBox oChkHeader = (CheckBox)lstvwStops.FindControl("ChkSelectAll");
            oChkHeader.Checked = false;
            hidFilePath.Value = Convert.ToString(oDSRouteStopDetails.Tables[1].Rows[0]["LinkUrl"]);         
            if(fileUploadItems.FileName!=string.Empty && fileUploadItems.FileName != null)
                hidFilePath.Value = fileUploadItems.FileName;
            txtRouteNo.Text = Convert.ToString(oDSRouteStopDetails.Tables[1].Rows[0]["RouteNo"]);
              
            if (oDSRouteStopDetails.Tables[0].IsNonEmpty())
            {
                foreach (ListViewDataItem olstItem in lstvwStops.Items)
                {
                    if (olstItem.ItemType == ListViewItemType.DataItem)
                    {
                        CheckBox ChkSelect = olstItem.FindControl("ChkSelect") as CheckBox;
                        DataRow[] dr = oDSRouteStopDetails.Tables[0].Select("miStopId=" + lstvwStops.DataKeys[olstItem.DisplayIndex]["miStopId"]);
                        if (dr.Length > 0)
                            ChkSelect.Checked = true;
                        else
                            ChkSelect.Checked = false;
                    }
                }
            }
                
                AddSortImage();
        }
    }

    /// <summary>
    /// This method is used to delete exisiting route details as well as it checks dependancy of route with Route-Shift-Timing Details.
    /// And also checks if at least one route's details has been configured or not.
    /// </summary>
    /// <param name="aiRouteId"></param>
    /// <param name="aiSchoolID"></param>
    /// <param name="aiAcademicYearId"></param>
    private void DeleteRouteDetails(int aiRouteId)
    {
        RouteDetailsBL oRouteDetailsBL = new RouteDetailsBL();
        int iRowCount = 0;
        DataTable oDTMsg = oRouteDetailsBL.DeleteRouteDetails(aiRouteId, miSchoolId, miAcademicYearId, out iRowCount);
        if (oDTMsg != null && oDTMsg.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(oDTMsg.Rows[0]["Msg"])))
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Route " + Convert.ToString(oDTMsg.Rows[0]["Msg"]) + " can not be deleted since associated with Route-Shift-Timing Details.";
        }
        else
        {
            if (iRowCount == 0)
                DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.RouteDetails));
            ClearFields();
            FillRouteStopAssociation();
            FillStopsList();
        }
    }

    /// <summary>
    /// This method is used to clear form fields.
    /// </summary>
    private void ClearFields()
    {
        txtRouteName.Text = string.Empty;
        hidRouteId.Value = Constants.S_ZERO;
        hidFilePath.Value = Constants.S_ZERO;
        lblErrorMsg.Text = string.Empty;
        CheckBox oChkHeader = (CheckBox)lstvwStops.FindControl("ChkSelectAll");
        oChkHeader.Checked = false;
        txtRouteNo.Text = string.Empty;        
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
    /// This method is used to set default values for sorting and error message's header.
    /// </summary>
    private void SetDefaultValues()
    {
        hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidSortDirection.Value = Constants.S_ASCENDING;
        HtmlTableRow oHtmlTableHeaderRow = lstvwRouteStopAsso.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
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
    /// This method is used to check pre-condition to configure association.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.RouteDetails);
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
        tblRouteDetails.Visible = false;
        tblStop.Visible = false;
        trSave.Visible = false;
        trDataPager.Visible = false;
        lstvwRouteStopAsso.Visible = false;
    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwRouteStopAsso.SortDirection.ToString() == "Ascending" || lstvwRouteStopAsso.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwRouteStopAsso.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwRouteStopAsso.SortExpression.ToString();
        else
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        HtmlTableRow oHtmlTableHeaderRow = lstvwRouteStopAsso.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    private void MandatoryNonMandatoryFields()
    {
        if (miSchoolId != Constants.SchoolId.SNS.ToInt())
        {            
            ReqRouteNo.Enabled = false;         
            sRouteNo.Visible = false;            
        }
        else
        {            
            ReqRouteNo.Enabled = true;         
            sRouteNo.Visible = true;            
        }
    }
    
    #endregion

}
