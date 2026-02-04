/* Class Name :- AllowedLeavePopup.aspx.cs
 * Created By :- Deepak
 * Created Date :- 5-Jan-2009
 * Description :- This class is used to save year-wise leaves configuration of staff.
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using PayrollEntities;
using Utility;

public partial class AllowedYearwiseLeavesPopup : SchoolBase
{
    #region "Constant"

    private const string S_USRES_LEAVES_YEARWISE_CONFIGURATION_ID = "UserLeavesYearwiseConfigurationId";
    private const string S_WARNING_MESSAGE = "These values are not yet saved.";
    private const string S_User_Name = "Name";
    private const string S_Staff_Groups_Id = "StaffGroupsId";
    private const string S_Staff_Groups_Name = "StaffGroupsName";

    private const int I_LEAVES_CONFIGURATION_MESSAGE_TABLE_INDEX = 0;
    private const int I_YEARWISE_LEAVES_CONFIGURATION_TABLE_INDEX = 1;
    private const int I_RECORD_COUNT_TABLE_INDEX = 2;
    private const int I_USERS_DETAILS_TABLE_INDEX = 3;
    private const int I_DEFAULT_COUNT_TABLE_INDEX = 4;

    private const string MONTH_DETAILs = "MonthDetails";

    #endregion

    #region Data Member(s)

    private bool mbIsPageInit = false;
    private List<BasicLeaveDetails> mlstBasicLeaveDetails;
    private UserLeavesYearwiseConfigurationBL moUserLeavesYearwiseConfigurationBL;

    #endregion

    #region "Events"

    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);            
            moUserLeavesYearwiseConfigurationBL = new UserLeavesYearwiseConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            mbIsPageInit = true;
            string sIsSaveClick = Convert.ToString(Request.Params[hidIsSaveButtonClick.ClientID.Replace("_", "$")]);
            if (sIsSaveClick == Constants.S_YES)
                FillLeavesListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to decrypt query string,
    /// set java script attributes of controls,set default values of controls,
    /// and fills list view of users leaves.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                mbIsPageInit = false;
                ReadQuerystring();
                SetJavascriptAttributes();
                FillYearComboBox();
                InitializeFields();
                FillLeavesListView();
            }
            hidIsSaveButtonClick.Value = Constants.S_NO;
        }
        catch (NoRecordFoundExceptions)
        {
            DisplayBasicLeavePresondition(true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set rounded values in leaves textboxes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwLeave_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;

                TextBox txtLeaveBalance = (TextBox)oCurrentItem.FindControl("txtLeaveBalance");
                txtLeaveBalance.Text = oDataRowView["LeaveBalance"].ToString();

                Label lblLeaveName = (Label)oCurrentItem.FindControl("lblLeaveName");
                lblLeaveName.Text = oDataRowView["LeaveName"].ToString();

                var oMonths = mlstBasicLeaveDetails.Select(mn => new { mn.Month.MonthAbbreviation, mn.Month.MonthId, mn.BasicLeaves, mn.LeaveId }).Distinct();
                oMonths.Where(lv => lv.LeaveId == Convert.ToInt32(oDataRowView["LeaveId"])).ToList().ForEach(
                     month =>
                     {
                         TextBox txtMonth = e.Item.FindControl("txt" + month.MonthAbbreviation) as TextBox;
                         if (txtMonth != null)
                         {
                             txtMonth.Text = month.BasicLeaves.ToString();
                             txtMonth.Attributes.Add("onfocus", "GetValue(this)");
                             txtMonth.Attributes.Add("onblur", "Validate(this,'" + hidIsLeapYear.Value + "');extractNumber(this,2,false);");
                         }

                         HiddenField hidMonth = e.Item.FindControl("hid" + month.MonthAbbreviation) as HiddenField;
                         if (hidMonth != null)
                         {
                             var oLeaveDetails = mlstBasicLeaveDetails.Where(bld => bld.Month.MonthId == month.MonthId && bld.LeaveId == Convert.ToInt32(oDataRowView["LeaveId"])).FirstOrDefault();
                             hidMonth.Value = oLeaveDetails.BasicLeaveConfigId + "_" + oLeaveDetails.Id;
                         }
                     });

                txtLeaveBalance.Attributes.Add("onfocus", "GetValue(this)");
                txtLeaveBalance.Attributes.Add("onblur", "Validate(this,'" + hidIsLeapYear.Value + "');extractNumber(this,2,true);");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save users year wise leaves configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveYearwiseLeaves(false);
            ClosePopupWindow();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used to set leave's list view as per selected year's leave configuration
    /// for user and set controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divBasicLeave.InnerHtml = string.Empty;
            hidIsLeapYear.Value = Convert.ToString(DateTime.IsLeapYear(Convert.ToInt32(cmbYear.SelectedValue)));
            mbIsPageInit = false;
            FillLeavesListView();
        }
        catch (NoRecordFoundExceptions)
        {
            DisplayBasicLeavePresondition(true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region "Private Methods"

    /// <summary>
    /// This method used to make disable or enable controls.
    /// </summary>
    /// <param name="abAction"></param>
    private void HideDisplayControls(bool abAction)
    {
        lblHeaderUserName.Visible = !abAction;
        btnSave.Visible = !abAction;
        tdUserName.Visible = !abAction;        
        lblUserName.Visible = !abAction;
        divContainer.Visible = !abAction;
    }

    /// <summary>
    /// This method is used to fill Year's combo.
    /// </summary>
    private void FillYearComboBox()
    {
        //List<string> lstYears = SchoolWiseAcademicYearMasterBL.GetYearsForAnnualPalanner(miSchoolId);
        List<LeaveYear> lstYears = moUserLeavesYearwiseConfigurationBL.GetLeaveYears();
        ListSource.FillDropDownList(lstYears, cmbYear, "Year", "Id", string.Empty);

        var oYear = lstYears.Where(yr => yr.StartDate.Date <= DateTime.Now.Date && DateTime.Now.Date <= yr.EndDate.Date).FirstOrDefault();
        if (oYear != null)
            cmbYear.SelectedValue = oYear.Id.ToString();

        //cmbYear.Items.Clear();
        //lstYears = lstYears.OrderByDescending(yr => yr).ToList();
        //lstYears.ForEach(yr => cmbYear.Items.Add(new ListItem { Value = yr, Text = yr }));
    }

    /// <summary>
    ///  This method set java script attributes of save and cancel button.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel });        
        btnCancel.Attributes.Add("onclick", "window.close();");
        btnSave.Attributes.Add("onclick", "SetInitStatus()");
        cmbYear.Attributes.Add("onclick", "SetInitStatus()");        
    }

    /// <summary>
    /// This method is used to initialize validation summary header,Year combo,
    /// radio button and user names label with default values.
    /// </summary>
    private void InitializeFields()
    {
        cmbYear.Focus();
        //cmbYear.SelectedValue = Convert.ToString(DateTime.Now.Year);
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// This method is used to get users year wise leave configuration and fill the leaves list view.
    /// </summary>
    private void FillLeavesListView()
    {
        int iYear;
        if (cmbYear.SelectedValue == string.Empty)
            iYear = Convert.ToInt32(Request.Params[cmbYear.ClientID.Replace("_", "$")]);
        else
            iYear = Convert.ToInt32(cmbYear.SelectedValue);

        int iUserId = 0;
        if (hidUserId.Value == string.Empty)
            iUserId = Convert.ToInt32(Request.Params[hidUserId.ClientID.Replace("_", "$")]);
        else
            iUserId = Convert.ToInt32(hidUserId.Value);

        DataSet oDSAllowedLeaves = moUserLeavesYearwiseConfigurationBL.GetAllowedLeaves(iUserId, iYear);
        if (oDSAllowedLeaves != null && oDSAllowedLeaves.Tables.Count > 0)
        {
            DataTable oDTLeaveConfig = oDSAllowedLeaves.Tables[I_LEAVES_CONFIGURATION_MESSAGE_TABLE_INDEX];
            if (oDTLeaveConfig.IsNonEmpty())
            {
                HideDisplayControls(false);
                trYear.Visible = true;
                divErr.Visible = false;
            }
            else
            {
                HideDisplayControls(true);
                trYear.Visible = false;
                divErr.Visible = true;
            }

            DataTable oDTUserDetails = oDSAllowedLeaves.Tables[I_USERS_DETAILS_TABLE_INDEX];
            if (oDTUserDetails.IsNonEmpty())
            {
                hidUserName.Value = oDTUserDetails.Rows[0][S_User_Name].ToString();
                hidStaffGroupId.Value = oDTUserDetails.Rows[0][S_Staff_Groups_Id].ToString();
                hidStaffGroupName.Value = oDTUserDetails.Rows[0][S_Staff_Groups_Name].ToString();
                lblUserName.Text = hidUserName.Value;
            }

            DataTable oDTRecordCount = oDSAllowedLeaves.Tables[I_RECORD_COUNT_TABLE_INDEX];
            if (oDTRecordCount != null && oDTRecordCount.Rows.Count > 0 && oDTRecordCount.Rows[0][0] != DBNull.Value)
                hidRecordCount.Value = oDTRecordCount.Rows[0][0].ToString();

            DataTable oDTDefaultCount = oDSAllowedLeaves.Tables[I_DEFAULT_COUNT_TABLE_INDEX];
            if (oDTDefaultCount != null && oDTDefaultCount.Rows.Count > 0 && oDTDefaultCount.Rows[0][0] != DBNull.Value)
                hidDisplayMessage.Value = oDTDefaultCount.Rows[0][0].ToString();

            BindListViewTemplate();

            lstvwLeave.DataSource = oDSAllowedLeaves.Tables[I_YEARWISE_LEAVES_CONFIGURATION_TABLE_INDEX];
            lstvwLeave.DataBind();
        }

        DisplayWarningMessage();
    }

    /// <summary>
    /// This method is used to show default values warning message.
    /// </summary>
    private void DisplayWarningMessage()
    {
        if (lstvwLeave.Items.Count > 0)
        {
            if (Convert.ToInt32(lstvwLeave.DataKeys[0][S_USRES_LEAVES_YEARWISE_CONFIGURATION_ID]) == 0 && hidDisplayMessage.Value != Constants.S_NO)
                lblWarningMessage.Text = S_WARNING_MESSAGE;
            else
                lblWarningMessage.Text = string.Empty;                
        }
    }

    /// <summary>
    /// This method is used to decrypt query string.
    /// </summary>
    /// <returns></returns>
    private void ReadQuerystring()
    {
        hidUserId.Value = QueryString["UserId"];
        hidUserRoleId.Value = QueryString["UserRoleId"];
        hidFilter.Value = QueryString["Filter"];
        if (QueryString["IsLocked"] == Constants.S_YES)
            btnSave.Enabled = false;
    }

    /// <summary>
    /// This method is used to close the current popup.
    /// </summary>
    private void ClosePopupWindow()
    {
        string sQueryString = "UserRoleId=" + hidUserRoleId.Value +
                              "&Is_Configured=" + hidIsConfigured.Value +
                              "&Filter=" + hidFilter.Value;
        sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
        sQueryString = "?" + sQueryString;
        hidQueryString.Value = sQueryString;        
    }

    /// <summary>
    /// This method  is used to save users year wise leaves configuration.
    /// </summary>
    private void SaveYearwiseLeaves(bool abApplytoAll)
    {
        int iStaffGroupId = Convert.ToInt32(hidStaffGroupId.Value);
        UserLeaveConfiguration oUserLeaveConfiguration = PopulateBL();
        moUserLeavesYearwiseConfigurationBL.Save(abApplytoAll, iStaffGroupId, Convert.ToChar(hidApplyToAllUsersOfStaffGroup.Value), oUserLeaveConfiguration);
    }

    /// <summary>
    /// This method creates UserLeavesYearwiseConfigurationBL object set its properties and return it.
    /// </summary>
    /// <returns></returns>
    private UserLeaveConfiguration PopulateBL()
    {
        return new UserLeaveConfiguration
        {   
            Year = Convert.ToInt32(cmbYear.SelectedValue),
            UserId = Convert.ToInt32(hidUserId.Value),
            InsertedById = miUserId,
            Is_Deleted = Constants.C_NO,
            AllowedLeaveXML = GenerateXML(),
            BasicLeaveXml = GenerateBasicLeaveXml()
        };
    }

    private string GenerateBasicLeaveXml()
    {
        var lstMonthMaster = ViewState[MONTH_DETAILs] as List<MonthMaster>;
        var oMonths = lstMonthMaster.Select(mn => new { mn.MonthId, mn.MonthAbbreviation }).Distinct();
        List<BasicLeaveDetails> lstBasicLeaveDetails = new List<BasicLeaveDetails>();
        foreach (ListViewDataItem oCurrentItem in lstvwLeave.Items)
        {
            oMonths.ToList().ForEach(
                month =>
                {
                    HiddenField hidMonth = oCurrentItem.FindControl("hid" + month.MonthAbbreviation) as HiddenField;
                    TextBox txtMonth = oCurrentItem.FindControl("txt" + month.MonthAbbreviation) as TextBox;
                    if (txtMonth.Text.Trim() == string.Empty)
                        txtMonth.Text = "0";
                    if (hidMonth.Value.Trim() == string.Empty)
                        hidMonth.Value = "0_0";

                    string[] Ids = hidMonth.Value.Split('_');
                    BasicLeaveDetails oBasicLeaveDetails = new BasicLeaveDetails
                    {
                        BasicLeaveConfigId = Convert.ToInt32(Ids[0]),
                        Id = Convert.ToInt32(Ids[1]),
                        BasicLeaves = Convert.ToDecimal(txtMonth.Text),
                        MonthId = month.MonthId,
                        LeaveId = Convert.ToInt32(lstvwLeave.DataKeys[oCurrentItem.DisplayIndex]["LeaveId"])
                    };
                    lstBasicLeaveDetails.Add(oBasicLeaveDetails);
                });
        }

        return GenerateXml(lstBasicLeaveDetails);
    }

    /// <summary>
    /// his method prepares XML document for saving users year wise leaves configuration.
    /// </summary>
    /// <returns></returns>
    private string GenerateXML()
    {
        const string S_ELEMENT = "element";

        int iRowId;
        int iLeaveId;
        int iUserLeavesYearwiseConfigurationId;

        XmlDocument oDoc = new XmlDocument();
        XmlElement oRoot = oDoc.CreateElement("UsersLeaveConfig");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "UsersLeaveConfig", string.Empty);

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount <= lstvwLeave.Items.Count - 1; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwLeave.Items[iRowCount];

            iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            iLeaveId = Convert.ToInt32(lstvwLeave.DataKeys[iRowId]["LeaveId"]);
            iUserLeavesYearwiseConfigurationId = Convert.ToInt32(lstvwLeave.DataKeys[iRowId][S_USRES_LEAVES_YEARWISE_CONFIGURATION_ID]);
            TextBox txtLeaveBalance = (TextBox)oCurrentItem.FindControl("txtLeaveBalance");
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "UsersLeaveConfig", string.Empty);
            oXmlNode.Attributes.Append(CreateAttribute(oDoc, S_USRES_LEAVES_YEARWISE_CONFIGURATION_ID, iUserLeavesYearwiseConfigurationId.ToString()));
            oXmlNode.Attributes.Append(CreateAttribute(oDoc, "LeaveId", iLeaveId.ToString()));
            oXmlNode.Attributes.Append(CreateAttribute(oDoc, "LeaveBalance", txtLeaveBalance.Text));

            oXmlRootNode.AppendChild(oXmlNode);
        }
        // Add the root node to document element.         
        oRoot.AppendChild(oXmlRootNode);
        // return the string generated.
        return oRoot.InnerXml;
    }

    /// <summary>
    /// This method is used to create xml attributes.
    /// </summary>
    /// <param name="oDoc"></param>
    /// <param name="asAttribteName"></param>
    /// <param name="asAtrributeValue"></param>
    /// <returns></returns>
    private XmlAttribute CreateAttribute(XmlDocument oDoc, string asAttribteName, string asAtrributeValue)
    {
        XmlAttribute attr = oDoc.CreateAttribute(asAttribteName);
        attr.Value = asAtrributeValue;
        return attr;
    }

    /// <summary>
    /// This method is used to create list view template dynamically.
    /// </summary>
    /// <param name="aTeacherId"></param>
    /// <param name="aiStudentId"></param>
    /// <param name="aiTermId"></param>
    private void BindListViewTemplate()
    {
        int iUserId = 0;
        if (hidUserId.Value == string.Empty)
            iUserId = Convert.ToInt32(Request.Params[hidUserId.ClientID.Replace("_", "$")]);
        else
            iUserId = Convert.ToInt32(hidUserId.Value);

        int iYear = 0;
        if (cmbYear.SelectedValue == string.Empty)
            iYear = Convert.ToInt32(Request.Params[cmbYear.ClientID.Replace("_", "$")]);
        else
            iYear = Convert.ToInt32(cmbYear.SelectedValue);

        int iStaffGroupId = 0;
        if (hidStaffGroupId.Value == string.Empty)
            iStaffGroupId = Convert.ToInt32(Request.Params[hidStaffGroupId.ClientID.Replace("_", "$")]);
        else
            iStaffGroupId = Convert.ToInt32(hidStaffGroupId.Value);

        int iLeaveSeperatorDay = Settings.LeaveSeperaterDay;
        mlstBasicLeaveDetails = moUserLeavesYearwiseConfigurationBL.GetUsersBasicLeaves(iUserId, iStaffGroupId, iLeaveSeperatorDay);
        List<MonthMaster> lstMonths = mlstBasicLeaveDetails.Select(month => new MonthMaster { MonthId = month.Month.MonthId, MonthAbbreviation = month.Month.MonthAbbreviation }).Distinct().ToList();
        ViewState[MONTH_DETAILs] = lstMonths;
        if (mlstBasicLeaveDetails.Count > 0)
        {
            lstvwLeave.LayoutTemplate = new BAsicLeaveMonthConfigTemplate(ListViewItemType.EmptyItem, mlstBasicLeaveDetails, false);
            lstvwLeave.ItemTemplate = new BAsicLeaveMonthConfigTemplate(ListViewItemType.DataItem, mlstBasicLeaveDetails, false);
            lstvwLeave.AlternatingItemTemplate = new BAsicLeaveMonthConfigTemplate(ListViewItemType.DataItem, mlstBasicLeaveDetails, true);
        }
        else
        {
            lstvwLeave.LayoutTemplate = new BAsicLeaveMonthConfigTemplate(ListViewItemType.EmptyItem, mlstBasicLeaveDetails, false);
            if (!mbIsPageInit)
                throw new NoRecordFoundExceptions("Please configure Basic Leave(s) for selected year.");
        }
    }

    /// <summary>
    /// This method is used to display precondition message for basic leaves.
    /// </summary>
    /// <param name="abAction"></param>
    private void DisplayBasicLeavePresondition(bool abAction)
    {
        if (abAction)
        {
            Dictionary<string, string> oDictionary = new Dictionary<string, string>();
            oDictionary.Add("Basic Leave(s) for User", "BasicLeaveConfigPopup.aspx?" + CommonUtility.EncryptQuerystring("StaffGroupId=" + hidStaffGroupId.Value));
            divBasicLeave.InnerHtml = GetPreconditionMessage(oDictionary);
        }

        divBasicLeave.Visible = abAction;
        trBasicLEaveMsg.Visible = abAction;

        lblHeaderUserName.Visible = abAction;
        lblUserName.Visible = abAction;
        tdUserName.Visible = abAction;

        btnSave.Visible = !abAction;        
        divContainer.Visible = !abAction;
    }

    /// <summary>
    /// This method is used to generate precondition message.
    /// </summary>
    /// <param name="aoMessages"></param>
    /// <returns></returns>
    private string GetPreconditionMessage(Dictionary<string, string> aoMessages)
    {
        string sMessage = "<table class='LblNoRecord' width='1000%'  cellpadding='0' cellspacing='0'><tr><td class='ClsConfigText'>Please configure following details for School :</td>";

        foreach (KeyValuePair<string, string> kvp in aoMessages)
            sMessage += "</tr><tr><td><a class='ClsConfigLink' href=" + kvp.Value + ">" + kvp.Key + "</a></td></tr>";

        sMessage += "</table>";
        return sMessage;
    }

    #endregion

    #region Template Class

    public class BAsicLeaveMonthConfigTemplate : ITemplate
    {
        private ListViewItemType moLstvwItemType;
        private List<BasicLeaveDetails> mlstBasicLeaveDetails;
        private bool mbIsAlterNateRow = false;

        public BAsicLeaveMonthConfigTemplate(ListViewItemType aoLstItemType, List<BasicLeaveDetails> alstBasicLeaveDetails, bool abIsAlterNate)
        {
            moLstvwItemType = aoLstItemType;
            mlstBasicLeaveDetails = alstBasicLeaveDetails;
            mbIsAlterNateRow = abIsAlterNate;
        }

        /// <summary>
        /// This method is used to create template structure and bind data to list view.
        /// </summary>
        /// <param name="aoContainer"></param>
        public void InstantiateIn(Control aoContainer)
        {
            var oMonths = mlstBasicLeaveDetails.Select(month => new { month.Month.MonthId, month.Month.MonthAbbreviation, month.BasicLeaveConfigId }).Distinct();
            if (moLstvwItemType == ListViewItemType.DataItem)
            {
                Literal ltrlDataItemTr = new Literal();
                Literal ltrlDataItemTd = new Literal();
                Literal ltrlDataItemName = new Literal();
                Literal ltrlDataItemTdClose = new Literal();
                Literal ltrlDataItemTrClose = new Literal();

                ltrlDataItemTr.Text = mbIsAlterNateRow == false ? "<tr class='ClsGridRow'>" : "<tr class='ClsGridAltRow'>";
                aoContainer.Controls.Add(ltrlDataItemTr);
                aoContainer.Controls.Add(ltrlDataItemTrClose);

                Literal ltrlDataItemTdName = new Literal();
                Label lblName = new Label();
                Literal ltrlDataItemTdNameClose = new Literal();

                ltrlDataItemTdName.Text = "<td style='padding-left:8px' width='100px' align='left'>";
                lblName.ID = "lblLeaveName";
                lblName.Width = Unit.Pixel(200);
                ltrlDataItemTdNameClose.Text = "</td>";
                ltrlDataItemTrClose.Text = "</tr>";

                aoContainer.Controls.Add(ltrlDataItemTdName);
                aoContainer.Controls.Add(lblName);
                aoContainer.Controls.Add(ltrlDataItemTdNameClose);
                aoContainer.Controls.Add(ltrlDataItemTrClose);

                oMonths.ToList().ForEach(month =>
                {
                    Literal ltrltd = new Literal();
                    Literal ltrtdClose = new Literal();
                    ltrltd.Text = "<td align = 'right' width='100px'>";
                    ltrtdClose.Text = "</td>";

                    TextBox txtMonth = new TextBox();
                    txtMonth.ID = "txt" + month.MonthAbbreviation;
                    txtMonth.Width = Unit.Pixel(70);
                    txtMonth.Attributes.Add("onkeypress", "return blockNonNumbers (this, event, true, false)");
                    txtMonth.Attributes.Add("onblur", "extractNumber(this,1,false)");
                    txtMonth.Attributes.Add("ondrop", "event.returnValue=false");
                    txtMonth.Attributes.Add("onkeyup", "extractNumber(this,1,false)");
                    txtMonth.Attributes.Add("onpaste", "event.returnValue=false");

                    txtMonth.MaxLength = 6;
                    txtMonth.Style.Add("text-align", "right");
                    txtMonth.Style.Add("padding-right", "5px");
                    txtMonth.CssClass = "SmlTxtBox";

                    HiddenField hidMonth = new HiddenField();
                    hidMonth.ID = "hid" + month.MonthAbbreviation;

                    aoContainer.Controls.Add(ltrltd);
                    aoContainer.Controls.Add(txtMonth);
                    aoContainer.Controls.Add(hidMonth);
                    aoContainer.Controls.Add(ltrtdClose);
                });

                Literal ltrltd1 = new Literal();
                Literal ltrtdClose1 = new Literal();
                ltrltd1.Text = "<td align = 'right' width='100px'>";
                ltrtdClose1.Text = "</td>";

                TextBox txtLeaveBalance = new TextBox();
                txtLeaveBalance.ID = "txtLeaveBalance";
                txtLeaveBalance.Width = Unit.Pixel(70);
                txtLeaveBalance.Attributes.Add("onkeypress", "return blockNonNumbers (this, event, true, false)");
                txtLeaveBalance.Attributes.Add("onblur", "extractNumber(this,1,false)");
                txtLeaveBalance.Attributes.Add("ondrop", "event.returnValue=false");
                txtLeaveBalance.Attributes.Add("onkeyup", "extractNumber(this,1,false)");
                txtLeaveBalance.Attributes.Add("onpaste", "event.returnValue=false");

                txtLeaveBalance.MaxLength = 6;
                txtLeaveBalance.Style.Add("text-align", "right");
                txtLeaveBalance.Style.Add("padding-right", "5px");
                txtLeaveBalance.CssClass = "SmlTxtBox";

                aoContainer.Controls.Add(ltrltd1);
                aoContainer.Controls.Add(txtLeaveBalance);
                aoContainer.Controls.Add(ltrtdClose1);

                aoContainer.Controls.Add(ltrlDataItemTrClose);
            }
            else
            {
                Literal ltrlHeadertbl = new Literal();
                ltrlHeadertbl.Text = "<table cellpadding='0' cellspacing='1' style='color: #333333' class='GridBorder' align='center'>";
                ltrlHeadertbl.Text += "<tr class='ClsGridHeader'><th align='left' style='padding-left:8px'>Leave Name</th>";

                Literal ltrthClose = new Literal();
                ltrthClose.Text = "</th>";

                Literal ltrlHeadertrClose = new Literal();
                ltrlHeadertrClose.Text = "</tr>";

                aoContainer.Controls.Add(ltrlHeadertbl);

                oMonths.ToList().ForEach(month =>
                {
                    Literal ltrlthHeader = new Literal();
                    ltrlthHeader.Text = "<th align='right' style='padding-right:5px'>" + month.MonthAbbreviation + "</th>";
                    aoContainer.Controls.Add(ltrlthHeader);
                });

                Literal ltrlthHeader1 = new Literal();
                ltrlthHeader1.Text = "<th align='right' style='padding-right:5px'>Balance</th>";
                aoContainer.Controls.Add(ltrlthHeader1);

                aoContainer.Controls.Add(ltrlHeadertrClose);

                Literal ltrlItemPlaceHolder = new Literal();
                ltrlItemPlaceHolder.ID = "itemPlaceholder";
                Literal ltrlHeadertblClose = new Literal();
                ltrlHeadertblClose.Text = "</table>";

                aoContainer.Controls.Add(ltrlItemPlaceHolder);
                aoContainer.Controls.Add(ltrlHeadertblClose);
            }
        }
    }

    #endregion
}
