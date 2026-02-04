using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class ApprovalLevelConfigurationUI : SchoolBase
{

    #region Events

    /// <summary>
    /// This method is used to override and handle page load event and inialize the page controls
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                ReadQueryString();
                InializeScreen();
                SetJavaScriptAtrribute();
            }
        }
        catch(Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This method is used to handle Add next approval level drop down list for configuration
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddLevel_Click(object sender, EventArgs e)
    {
        try
        {
            int iCurrentLevel = Convert.ToInt32(hidCurrentLevel.Value);
            int iDesignationID = 0;
            switch (iCurrentLevel)
            {
                case 1:
                    iDesignationID = Convert.ToInt32(ddlFirstApprovalLevel.SelectedValue);
                    BindDesignationDropDownList(ddlSecondApprovalLevel, iDesignationID, 0);
                    tblSecondLevel.Visible = true;
                    break;
                case 2:
                    iDesignationID = Convert.ToInt32(ddlSecondApprovalLevel.SelectedValue);
                    BindDesignationDropDownList(ddlThirdApprovalLevel, iDesignationID, 0);
                    tblThirdLevel.Visible = true;
                    break;
                case 3:
                    iDesignationID = Convert.ToInt32(ddlThirdApprovalLevel.SelectedValue);
                    BindDesignationDropDownList(ddlFourthApprovalLevel, iDesignationID, 0);
                    tblFourthLevel.Visible = true;
                    break;
                case 4:
                    iDesignationID = Convert.ToInt32(ddlFourthApprovalLevel.SelectedValue);
                    BindDesignationDropDownList(ddlFifthApprovalLevel, iDesignationID, 0);
                    tblFifthLevel.Visible = true;
                    btnAddLevel.Visible = false;
                    break;
            }
            tblActionCntrls.Visible = true;
            btnRemoveLevel.Visible = true;

            hidCurrentLevel.Value = Convert.ToString(++iCurrentLevel);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle Creator's dropdrwn list's selected index changed 
    /// and to fill first level fo designation drop down for configuration
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCreatorDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ShowHideAllApprovalLevels(false);
            int iDesignationID = Convert.ToInt32(ddlCreatorDesignation.SelectedValue);
            hidSelectedDesignationIds.Value = "0";
            hidCurrentLevel.Value = "0";
            BindDesignationDropDownList(ddlFirstApprovalLevel, iDesignationID, 0);
            if (iDesignationID == 0)
            {
                tblFirstLevel.Visible = false;
                btnAdd.Visible = false;
            }
            else
            {
                tblFirstLevel.Visible = true;
                tblActionCntrls.Visible = true;
            }
            btnRemoveLevel.Visible = false;
            btnAddLevel.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle first appproval levels dropdrwn list's selected index changed 
    /// and to fill second level fo designation drop down for configuration
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlFirstApprovalLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ShowHideAllApprovalLevels(false);
            hidCurrentLevel.Value = "1";
            btnAdd.Visible = true;
            btnRemoveLevel.Visible = false;
            tblFirstLevel.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle second appproval levels dropdrwn list's selected index changed 
    /// and to fill third level fo designation drop down for configuration
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlSecondApprovalLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            tblThirdLevel.Visible = false;
            tblFourthLevel.Visible = false;
            tblFifthLevel.Visible = false;
            hidCurrentLevel.Value = "2";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle third appproval levels dropdrwn list's selected index changed 
    /// and to fill fourth level fo designation drop down for configuration
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlThirdApprovalLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            tblFourthLevel.Visible = false;
            tblFifthLevel.Visible = false;
            hidCurrentLevel.Value = "3";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle fourth appproval levels dropdrwn list's selected index changed 
    /// and to fill fifth level fo designation drop down for configuration
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlFourthApprovalLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            tblFifthLevel.Visible = false;
            hidCurrentLevel.Value = "4";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle fifth appproval levels dropdrwn list's selected index changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlFifthApprovalLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btnAddLevel.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle back button event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage) this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Inventory)));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This method is used to handle add event and add approval level configuration
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            ApprovalLevelConfigurationBL oApprovalLevelConfigurationBL;

            if (((Button)sender).CommandName == "AddLevel")
                oApprovalLevelConfigurationBL = new ApprovalLevelConfigurationBL();
            else
            {
                int iApprovalLevelId = Convert.ToInt32(((Button)sender).CommandArgument);
                oApprovalLevelConfigurationBL = new ApprovalLevelConfigurationBL(iApprovalLevelId);
            }

            oApprovalLevelConfigurationBL.RequisitionByDesignationID = Convert.ToInt32(ddlCreatorDesignation.SelectedValue);
            oApprovalLevelConfigurationBL.FirstDesignationID = Convert.ToInt32(ddlFirstApprovalLevel.SelectedValue);
            if (ddlSecondApprovalLevel.Visible)
                oApprovalLevelConfigurationBL.SecondDesignationID = Convert.ToInt32(ddlSecondApprovalLevel.SelectedValue);
            else
                oApprovalLevelConfigurationBL.SecondDesignationID = 0;
            if (ddlThirdApprovalLevel.Visible)
                oApprovalLevelConfigurationBL.ThirdDesignationID = Convert.ToInt32(ddlThirdApprovalLevel.SelectedValue);
            else
                oApprovalLevelConfigurationBL.ThirdDesignationID = 0;

            if (ddlFourthApprovalLevel.Visible)
                oApprovalLevelConfigurationBL.FourthDesignationID = Convert.ToInt32(ddlFourthApprovalLevel.SelectedValue);
            else
                oApprovalLevelConfigurationBL.FourthDesignationID = 0;

            if (ddlFifthApprovalLevel.Visible)
                oApprovalLevelConfigurationBL.fifthDesignationID = Convert.ToInt32(ddlFifthApprovalLevel.SelectedValue);
            else
                oApprovalLevelConfigurationBL.fifthDesignationID = 0;

            oApprovalLevelConfigurationBL.School_Id = miSchoolId;

            if (((Button)sender).CommandName == "AddLevel")
            {
                oApprovalLevelConfigurationBL.Insert_Date = DateTime.Now;
                oApprovalLevelConfigurationBL.Inserted_By_Id = miUserId;
                oApprovalLevelConfigurationBL.InsertApprovalLevelConfiguration();
            }
            else
            {
                oApprovalLevelConfigurationBL.Update_Date = DateTime.Now;
                oApprovalLevelConfigurationBL.Updated_By_Id = miUserId;
                oApprovalLevelConfigurationBL.UpdateApprovalLevelConfiguration();
            }
            ShowHideAllApprovalLevels(false);
            BindApprovalLevelList();
            ddlCreatorDesignation.SelectedIndex = 0;
            ddlCreatorDesignation.Enabled = true;
            btnAdd.Text = "Add";
            btnAdd.CommandName = "AddLevel";
            btnAdd.CommandArgument = string.Empty;
            if (hidIsConfig.Value != Constants.C_YES.ToString())
            {
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.ApprovalLevelConfig));
            }
            btnRemoveLevel.Visible = false;
            btnAddLevel.Visible = false;
            BindDesignationDropDownList(ddlCreatorDesignation, 0, -999);
        }
        
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This method is used to handle cancel event and cancel the add or update action
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ResetAddNewConfiguration();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    ///  This method is used to handle remove level event and removes last level
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRemoveLevel_Click(object sender, EventArgs e)
    {
        try
        {
            int iCurrentLevel = Convert.ToInt32(hidCurrentLevel.Value);
            btnAddLevel.Visible = true;
            switch (iCurrentLevel)
            {
                case 1:
                    btnAddLevel.Visible = false;
                    break;
                case 2:
                    tblSecondLevel.Visible = false;
                    btnRemoveLevel.Visible = false;
                    break;
                case 3:
                    tblThirdLevel.Visible = false;
                    break;
                case 4:
                    tblFourthLevel.Visible = false;
                    break;
                case 5:
                    tblFifthLevel.Visible = false;
                    break;
            }
            tblActionCntrls.Visible = true;
            hidCurrentLevel.Value = Convert.ToString(--iCurrentLevel);
            SetInProcessDesignations();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void SetInProcessDesignations()
    {
        StringBuilder oSB = new StringBuilder("0");
        if (ddlCreatorDesignation.Visible && ddlCreatorDesignation.SelectedIndex > 0)
            oSB.Append(", " + ddlCreatorDesignation.SelectedValue);
        if (ddlFirstApprovalLevel.Visible && ddlFirstApprovalLevel.SelectedIndex > 0)
            oSB.Append(", " + ddlFirstApprovalLevel.SelectedValue);
        if (ddlSecondApprovalLevel.Visible && ddlSecondApprovalLevel.SelectedIndex > 0)
            oSB.Append(", " + ddlSecondApprovalLevel.SelectedValue);
        if (ddlThirdApprovalLevel.Visible && ddlThirdApprovalLevel.SelectedIndex > 0)
            oSB.Append(", " + ddlThirdApprovalLevel.SelectedValue);
        if (ddlFourthApprovalLevel.Visible && ddlFourthApprovalLevel.SelectedIndex > 0)
            oSB.Append(", " + ddlFourthApprovalLevel.SelectedValue);
        if (ddlFifthApprovalLevel.Visible && ddlFifthApprovalLevel.SelectedIndex > 0)
            oSB.Append(", " + ddlFifthApprovalLevel.SelectedValue);
        hidSelectedDesignationIds.Value = oSB.ToString();
    }

    /// <summary>
    ///  This method is used to handle
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveFinalApprovers_Click(object sender, EventArgs e)
    {
        try
        {
            string sFinalApproversXML = GenerateFinalApproversXML();
            ApprovalLevelConfigurationCollectionBL.UpdateFinalApproverDesignation(sFinalApproversXML);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #region Grid event
    /// <summary>
    ///  This method is used to handle list view's item command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwApprovalLevel_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            btnAdd.Visible = true;
            int iApprovalLevelId = Convert.ToInt32(((ImageButton) (e.CommandSource)).CommandArgument);
            ApprovalLevelConfigurationBL oApprovalLevelConfigurationBL =new ApprovalLevelConfigurationBL(iApprovalLevelId);
            oApprovalLevelConfigurationBL.IsPendingApproval(oApprovalLevelConfigurationBL.RequisitionByDesignationID);
            if (e.CommandName == "Remove")
            {

                oApprovalLevelConfigurationBL.DeleteApprovalLevelConfiguration(iApprovalLevelId);
                BindApprovalLevelList();
                ResetAddNewConfiguration();
                if (lstvwApprovalLevel.Items.Count == 0)
                    DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.ApprovalLevelConfig));
            }
            else if (e.CommandName == "EditLevel")
            {
                ClearAllCombos();
                BindEditModeValues(e);

            }
        }
        catch (SqlException ex)
        {
            ResetAddNewConfiguration();
            lblErr.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
      
    }

    private void ClearAllCombos()
    {
        ddlCreatorDesignation.Items.Clear();
        ddlFirstApprovalLevel.Items.Clear();
        ddlSecondApprovalLevel.Items.Clear();
        ddlThirdApprovalLevel.Items.Clear();
        ddlFourthApprovalLevel.Items.Clear();
        ddlFifthApprovalLevel.Items.Clear();
    }
    #endregion Grid event

    #endregion Events

    #region Private Methods

    /// <summary>
    /// This method is used to reset  add new configuration controls
    /// </summary>
    private void ResetAddNewConfiguration()
    {
        ShowHideAllApprovalLevels(false);
        btnAdd.Text = "Add";
        btnAdd.CommandName = "AddLevel";
        btnAdd.CommandArgument = string.Empty;
        ddlCreatorDesignation.Enabled = true;
        hidSelectedDesignationIds.Value = "0";
        BindDesignationDropDownListForCancel(ddlCreatorDesignation, 0, -999);
        btnRemoveLevel.Visible = false;
        btnAddLevel.Visible = false;
    }

    /// <summary>
    /// This method is used to initialise screen details.
    /// </summary>
    private void InializeScreen()
    {
        ApprovalValidationSummary.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        BindReqCreatorsDesignationList();
        BindApprovalLevelList();
        BindFinalApproverCheckList();
    }

    /// <summary>
    /// This method is used to bind final approver's listview.
    /// </summary>
    private void BindFinalApproverCheckList()
    {
        SchoolUserBL oSchoolUserBL = new SchoolUserBL();
        DataTable oDtDesignations = oSchoolUserBL.GetAllDesgnation(miSchoolId, "0", 0);
        lstvwFinalApprover.DataSource = oDtDesignations;
        lstvwFinalApprover.DataBind();
    }

    /// <summary>
    /// This method is used to bind approver level configuration listview.
    /// </summary>
    private void BindApprovalLevelList()
    {
        DataTable odtApprovalLevelConfiguration = ApprovalLevelConfigurationCollectionBL.FetchApprovalLevelConfigurationDetails(miSchoolId);
        lstvwApprovalLevel.DataSource = odtApprovalLevelConfiguration;
        lstvwApprovalLevel.DataBind();
    }

    /// <summary>
    /// This method is used to bind list of requisition creator's designations
    /// This dropdown list will contain a designation for which approval level is not configured.
    /// </summary>
    private void BindReqCreatorsDesignationList()
    {
        BindDesignationDropDownList(ddlCreatorDesignation, 0, -999);
    }

    /// <summary>
    /// This method is used to bind list of  designations drop down.
    /// This drop down will containg all designation except its all previous levels does selected.
    /// </summary>
    /// <param name="oListView"></param>
    /// <param name="aiDesignationID"></param>
    private void BindDesignationDropDownList(DropDownList oDropDownList, int aiDesignationID, int aiRequisitionByDesignationID)
    {
        SetInProcessDesignations();
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillDesignationCombobox(ref oDropDownList, Constants.UserRoles.None);
    }

    /// <summary>
    /// This method is used to bind list of  designations drop down.
    /// This drop down will containg all designation except its all previous levels does selected.
    /// </summary>
    /// <param name="oListView"></param>
    /// <param name="aiDesignationID"></param>
    private void BindDesignationDropDownListForCancel(DropDownList oDropDownList, int aiDesignationID, int aiRequisitionByDesignationID)
    {
        SchoolUserBL oSchoolUserBL = new SchoolUserBL();
        DataTable oDtDesignations = oSchoolUserBL.GetAllDesgnation(miSchoolId, hidSelectedDesignationIds.Value, aiRequisitionByDesignationID);
        ControlUtility.FillDropDownList(oDtDesignations, ref oDropDownList,"Designation_Id","Designation_Name",Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to show or hide approval levels
    /// </summary>
    /// <param name="bVisible"></param>
    private void ShowHideAllApprovalLevels(bool bVisible)
    {
        tblFirstLevel.Visible = bVisible;
        tblSecondLevel.Visible = bVisible;
        tblThirdLevel.Visible = bVisible;
        tblFourthLevel.Visible = bVisible;
        tblFifthLevel.Visible = bVisible;
    }
  

    /// <summary>
    /// This method is used to set edit mode of screen
    /// </summary>
    /// <param name="e"></param>
    private void BindEditModeValues(ListViewCommandEventArgs e)
    {
        ShowHideAllApprovalLevels(true);
        btnAdd.Text = "Update";
        btnAdd.CommandName = "UpdateLevel";
        btnAdd.CommandArgument = ((ImageButton)(e.CommandSource)).CommandArgument;
        tblActionCntrls.Visible = true;
        hidSelectedDesignationIds.Value = "0";
        HiddenField oHiddenField = (HiddenField)e.Item.FindControl("hidCreator");
        BindDesignationDropDownList(ddlCreatorDesignation, 0, Convert.ToInt32(oHiddenField.Value));
        ddlCreatorDesignation.SelectedValue = oHiddenField.Value;
        ddlCreatorDesignation.Enabled = false;
        btnRemoveLevel.Visible = false;
        oHiddenField = (HiddenField)e.Item.FindControl("hidFirstAppover");
        int iCurrentLevel = 0;
        if (oHiddenField.Value != string.Empty)
        {
            BindDesignationDropDownList(ddlFirstApprovalLevel, Convert.ToInt32(ddlCreatorDesignation.SelectedValue), 0);
            ddlFirstApprovalLevel.SelectedValue = oHiddenField.Value;
            btnAddLevel.Visible = true;
            iCurrentLevel = 1;
        }
        else
            tblFirstLevel.Visible = false;

        oHiddenField = (HiddenField)e.Item.FindControl("hidSecondAppover");
        if (oHiddenField.Value != string.Empty && oHiddenField.Value != "0")
        {
            BindDesignationDropDownList(ddlSecondApprovalLevel, Convert.ToInt32(ddlFirstApprovalLevel.SelectedValue), 0);
            ddlSecondApprovalLevel.SelectedValue = oHiddenField.Value;
            btnRemoveLevel.Visible = true;
            iCurrentLevel = 2;
        }
        else
            tblSecondLevel.Visible = false;

        oHiddenField = (HiddenField)e.Item.FindControl("hidThirdAppover");
        if (oHiddenField.Value != string.Empty && oHiddenField.Value != "0")
        {
            BindDesignationDropDownList(ddlThirdApprovalLevel, Convert.ToInt32(ddlSecondApprovalLevel.SelectedValue), 0);
            ddlThirdApprovalLevel.SelectedValue = oHiddenField.Value;
            iCurrentLevel = 3;
        }
        else
            tblThirdLevel.Visible = false;

        oHiddenField = (HiddenField)e.Item.FindControl("hidFourthAppover");
        if (oHiddenField.Value != string.Empty && oHiddenField.Value != "0")
        {
            BindDesignationDropDownList(ddlFourthApprovalLevel, Convert.ToInt32(ddlThirdApprovalLevel.SelectedValue), 0);
            ddlFourthApprovalLevel.SelectedValue = oHiddenField.Value;
            iCurrentLevel = 4;
        }
        else
            tblFourthLevel.Visible = false;

        oHiddenField = (HiddenField)e.Item.FindControl("hidFifthAppover");
        if (oHiddenField.Value != string.Empty && oHiddenField.Value != "0")
        {
            BindDesignationDropDownList(ddlFifthApprovalLevel, Convert.ToInt32(ddlFourthApprovalLevel.SelectedValue), 0);
            ddlFifthApprovalLevel.SelectedValue = oHiddenField.Value;
            btnAddLevel.Visible = false;
            iCurrentLevel = 5;
        }
        else
            tblFifthLevel.Visible = false;
        hidCurrentLevel.Value = iCurrentLevel.ToString();
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>

    private void SetJavaScriptAtrribute()
    {
        ApplyMouseHoverEffect(new List<Button> {btnCancel, btnAdd,btnBack,btnAddLevel,btnRemoveLevel,btnSaveFinalApprovers});
        btnAdd.Attributes["onclick"] = "javascript:SetError();";
        btnAddLevel.Attributes["onclick"] = "javascript:SetError();";
        btnRemoveLevel.Attributes["onclick"] = "javascript:SetError();";
     
    }

    /// <summary>
    /// Generate XML for the RollNos order.
    /// </summary>
    /// <returns></returns>
    private string GenerateFinalApproversXML()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("FinalApproversCollection");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "FinalApproversCollection", "");

        // Loop through all the grid rows.
        foreach (ListViewDataItem oListViewDataItem in lstvwFinalApprover.Items)
        {
            CheckBox oCheckBox = (CheckBox)oListViewDataItem.FindControl("chkCanFinalApprove");

            // Create root xml element.
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "FinalApprovers", "");

            string sAtrrName = "Designation_Id";
            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = lstvwFinalApprover.DataKeys[oListViewDataItem.DisplayIndex][0].ToString();
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "IsFinalApproval";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = oCheckBox.Checked ? Constants.I_ONE.ToString() : Constants.I_ZERO.ToString();
            oXmlNode.Attributes.Append(attr);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);
        }
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void ReadQueryString()
    {
        if(QueryString["Is_Configured"] != null)
            hidIsConfig.Value = QueryString["Is_Configured"];
    }

    #endregion Private Methods

}
