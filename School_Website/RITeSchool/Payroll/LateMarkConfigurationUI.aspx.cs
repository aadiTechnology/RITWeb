/* File Name :- LateMarkConfigurationUI.aspx.cs
 * Created By :- Vinod
 * Created Date :- 14-Nov-2009
 * Class Description :- This class is used to configure Late Mark Configuration. 
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class LateMarkConfigurationUI : SchoolBase
{
    #region Constant

    private const int S_LATE_MARK_CONFIG_TABLE_INDEX = 0;
    private const int S_STAFF_LEAVE_SORT_ORDER_TABLE_INDEX = 1;
    private const string S_PAID_LEAVE_MESSAGE = "Please configure at least one paid leave.";
    
    #endregion

    #region Data MEmber(s)

    private int miRowCount = 0;
    private LateMarkConfigurationBL moLateMarkConfigurationBL;

    #endregion

    #region Event

    /// <summary>
    /// This event is used to fill LateMarkConfiguration & StaffLeaveSortOrder ListView and set attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moLateMarkConfigurationBL = new LateMarkConfigurationBL(miSchoolId, miUserId);
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                if (CheckPreCondition())
                {
                    FillLateMarkConfig();
                    valSummary.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill LateMarkConfiguration ListView Row data.
    /// </summary>
    protected void lstvwLateMarkConfiguration_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            Label lblRowNo = oCurrentItem.FindControl("lblRowNo") as Label;
            lblRowNo.Text = ((oCurrentItem.DisplayIndex)+1).ToString();
            DataRowView oDataRowView = (DataRowView)oCurrentItem.DataItem;
            CheckBox chkSelect = oCurrentItem.FindControl("ChkSelect") as CheckBox;
            if (!oDataRowView.Row[3].ToString().Equals("0"))
                chkSelect.Checked = true;

            chkSelect.Attributes["onclick"] = "ResetFields(" + oCurrentItem.DisplayIndex + ")";

            DropDownList cmbSortOrder = oCurrentItem.FindControl("cmbSortOrder") as DropDownList;
            cmbSortOrder.Items.Add(new ListItem(Constants.S_SELECT, Constants.S_ZERO));

            for (int iRowNo = 1; iRowNo <= miRowCount; iRowNo++)
                cmbSortOrder.Items.Add(iRowNo.ToString());               

            cmbSortOrder.SelectedValue = oDataRowView.Row["SortOrder"].ToString();

            TextBox otxtConsideredLeaves = oCurrentItem.FindControl("txtConsideredLeaves") as TextBox;
            otxtConsideredLeaves.Attributes.Add("onkeyup", "OnGridKeyUpNumber(this,1,false,event);");
            TextBox otxtLateMarkCount = oCurrentItem.FindControl("txtLateMarkCount") as TextBox; 
            otxtLateMarkCount.Attributes.Add("onkeyup", "OnGridKeyUpNumber(this,0,false,event);");
            hidConsideredLeaves.Value = otxtConsideredLeaves.Text;
            otxtConsideredLeaves.Attributes.Add("onfocus", "GetValue(this)");
            otxtConsideredLeaves.Attributes.Add("onblur", "Validate(this,'" + oDataRowView.Row["ConsideredLeaves"].ToString() + "');extractNumber(this,2,false);");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill StaffLeaveSortOrder List View row data.
    /// </summary>
    protected void lstvwLeaveSortOrder_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            DataRowView oDataRowView = (DataRowView)oCurrentItem.DataItem;

            DropDownList ocmbLeaveSortOrder = oCurrentItem.FindControl("cmbStaffLeaveSortOrder") as DropDownList;
            ocmbLeaveSortOrder.Items.Add(new ListItem(Constants.S_SELECT, "9999"));
            for (int iRowNo = 1; iRowNo <= miRowCount; iRowNo++)
                ocmbLeaveSortOrder.Items.Add(iRowNo.ToString());
            ocmbLeaveSortOrder.SelectedValue = oDataRowView.Row["SortOrder"].ToString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Save LateMarkConfiguration & StaffLeaveSortOrder data and configuration .
    /// </summary>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            moLateMarkConfigurationBL.Save(GenerateLateMarkConfigurationXML(), GenerateStaffLeavesSortOrderXML());
            if (QueryString["Is_Configured"] != Constants.S_YES)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.LateMarkConfiguration));
            
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related)));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    public void FillLateMarkConfig()
    {   
        DataSet oDSLateMarkConfiguration = moLateMarkConfigurationBL.GetAll();

        if (oDSLateMarkConfiguration.Tables[S_LATE_MARK_CONFIG_TABLE_INDEX].IsNonEmpty())
        {
            miRowCount = oDSLateMarkConfiguration.Tables[S_LATE_MARK_CONFIG_TABLE_INDEX].Rows.Count;
            hidLateNarkConfigSaveCount.Value = miRowCount.ToString();
            lstvwLateMarkConfiguration.DataSource = oDSLateMarkConfiguration.Tables[S_LATE_MARK_CONFIG_TABLE_INDEX];
            lstvwLateMarkConfiguration.DataBind();
        }

        if (oDSLateMarkConfiguration.Tables[S_STAFF_LEAVE_SORT_ORDER_TABLE_INDEX].IsNonEmpty())
        {
            miRowCount = oDSLateMarkConfiguration.Tables[S_STAFF_LEAVE_SORT_ORDER_TABLE_INDEX].Rows.Count;
            // Save no of record in StaffLeaveSortOrder Table
            hidLeaveSortOrderSaveCount.Value = miRowCount.ToString();
            lstvwLeaveSortOrder.DataSource = oDSLateMarkConfiguration.Tables[S_STAFF_LEAVE_SORT_ORDER_TABLE_INDEX];
            lstvwLeaveSortOrder.DataBind();
        }
        else
        {
            lblErrorMessage.Text = S_PAID_LEAVE_MESSAGE;
            btnSave.Visible = false;
        }

        ListViewDataItem oListViewDataItem = (ListViewDataItem)lstvwLateMarkConfiguration.Items[0];
        CheckBox oCheckBox = oListViewDataItem.FindControl("ChkSelect") as CheckBox;
        oCheckBox.Focus();
    }

    /// <summary>
    /// This event is used to set Java script Attribute.
    /// </summary>
    protected void SetJavascriptAttributes()
    {
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related));
        btnSave.Attributes.Add("onclick", "if(!CheckSelectedGroups(this)) return false;");
        btnCancel.Attributes["onclick"] = "javascript:DisableButtons(this)";
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel });
    }

    /// <summary>
    /// This method is used to generate xml of LateMarkConfiguration details.
    /// </summary>
    /// <returns></returns>
    private string GenerateLateMarkConfigurationXML()
    {
        const string S_ELEMENT = "element";
        string sAttribute;
        XmlDocument oXmlDocument = new XmlDocument();

        // Create a root level element.
        XmlElement rootElement = oXmlDocument.CreateElement("LateMarkConfig");
        XmlNode oXmlRootNode = oXmlDocument.CreateNode(S_ELEMENT, "LateMarkConfig", string.Empty);

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount < lstvwLateMarkConfiguration.Items.Count; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwLateMarkConfiguration.Items[iRowCount];
            int iLateMarkConfigurationId = Convert.ToInt32(lstvwLateMarkConfiguration.DataKeys[iRowCount]["LateMarkConfigurationId"]);

            CheckBox oCheckBox = oCurrentItem.FindControl("ChkSelect") as CheckBox;
            XmlNode oXmlNode = oXmlDocument.CreateNode(S_ELEMENT, "LateMarkConfig", string.Empty);

            sAttribute = "LateMarkConfigurationId";
            XmlAttribute oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);

            oXmlAttribute.Value = iLateMarkConfigurationId.ToString();
            oXmlNode.Attributes.Append(oXmlAttribute);

            sAttribute = "LateMarkCount";
            oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
            TextBox otxtLateMarkCount = (TextBox)oCurrentItem.FindControl("txtLateMarkCount");
            oXmlAttribute.Value = otxtLateMarkCount.Text.Trim();
            oXmlNode.Attributes.Append(oXmlAttribute);

            sAttribute = "ConsideredLeaves";
            oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
            TextBox txtConsideredLeaves = (TextBox)oCurrentItem.FindControl("txtConsideredLeaves");
            oXmlAttribute.Value = txtConsideredLeaves.Text.Trim();
            oXmlNode.Attributes.Append(oXmlAttribute);

            sAttribute = "SortOrder";
            oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
            DropDownList ocmbSortOrder = (DropDownList)oCurrentItem.FindControl("cmbSortOrder");
            oXmlAttribute.Value = ocmbSortOrder.SelectedValue;
            oXmlNode.Attributes.Append(oXmlAttribute);

            sAttribute = "Is_Deleted";
            oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
            if (oCheckBox.Checked)
                oXmlAttribute.Value = Constants.S_NO;
            else
                oXmlAttribute.Value = Constants.S_YES;
            oXmlNode.Attributes.Append(oXmlAttribute);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);
        }

        rootElement.AppendChild(oXmlRootNode);
        return rootElement.InnerXml;
    }

    /// <summary>
    /// This method is used to generate xml of StaffLeavesSortOrder details.
    /// </summary>
    /// <returns></returns>
    private string GenerateStaffLeavesSortOrderXML()
    {
        const string S_ELEMENT = "element";
        string sAttribute;
        XmlDocument oXmlDocument = new XmlDocument();

        // Create a root level element.
        XmlElement rootElement = oXmlDocument.CreateElement("StaffLeaveSortOrder");
        XmlNode oXmlRootNode = oXmlDocument.CreateNode(S_ELEMENT, "StaffLeaveSortOrder", string.Empty);

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount < lstvwLeaveSortOrder.Items.Count; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwLeaveSortOrder.Items[iRowCount];
            int iStaffLeaveSortOrderId = Convert.ToInt32(lstvwLeaveSortOrder.DataKeys[iRowCount]["StaffLeaveSortOrderId"]);

            XmlNode oXmlNode = oXmlDocument.CreateNode(S_ELEMENT, "StaffLeaveSortOrder", string.Empty);

            sAttribute = "StaffLeaveSortOrderId";
            XmlAttribute oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
            oXmlAttribute.Value = iStaffLeaveSortOrderId.ToString();
            oXmlNode.Attributes.Append(oXmlAttribute);

            sAttribute = "LeaveId";
            oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
            Label olblStaffLeaves = oCurrentItem.FindControl("lblStaffLeaves") as Label;
            string icmbSortOrder = lstvwLeaveSortOrder.DataKeys[iRowCount]["LeaveId"].ToString();
            oXmlAttribute.Value = icmbSortOrder;
            oXmlNode.Attributes.Append(oXmlAttribute);

            sAttribute = "SortOrder";
            oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
            DropDownList ocmbStaffLeaveSortOrder = (DropDownList)oCurrentItem.FindControl("cmbStaffLeaveSortOrder");
            oXmlAttribute.Value = ocmbStaffLeaveSortOrder.SelectedValue;
            oXmlNode.Attributes.Append(oXmlAttribute);

            sAttribute = "Is_Deleted";
            oXmlAttribute = oXmlDocument.CreateAttribute(sAttribute);
            if (ocmbStaffLeaveSortOrder.SelectedValue == "9999")
                oXmlAttribute.Value = Constants.S_YES;
            else
                oXmlAttribute.Value = Constants.S_NO;
            oXmlNode.Attributes.Append(oXmlAttribute);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);
        }

        rootElement.AppendChild(oXmlRootNode);
        return rootElement.InnerXml;
    }

    /// <summary>
    /// This method checks the pre conditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.LateMarkConfiguration);

        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            VisibleOrHideControls();
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to set visible or hide properties of controls.
    /// </summary>
    private void VisibleOrHideControls()
    {
        tdLateMarkConfig.Visible = false;
        tdlstvwLeaveSortOrder.Visible = false;
        btnCancel.Visible = true;
        btnCancel.Text = "Back";
        btnSave.Visible = false;
    }

    #endregion
}