using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

/// <summary>
/// This class is used to set full attendance of users.
/// </summary>
public partial class UsersAttendancePopup : SchoolBase
{
    #region Data Member

    List<StaffLeaveDetails> moStaffLeaveDetails;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill staff group combobox and fill staff list view.
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
                SetJavascriptAttributes();
                FillStaffGroupCombobox();
                FillUserListview();
                cmbStaffGroup.Focus();
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill staff list view according to selected staff group.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStaffGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillUserListview();
            hidStaffGroup.Value = cmbStaffGroup.SelectedValue;
            lblMessage.Visible = false;

            if (lstvwUsers.Items.Count > 0)
            {
                HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwUsers.FindControl("trHeader");
                if (oHtmlTableRow != null)
                {
                    CheckBox oCheckBox = (CheckBox)oHtmlTableRow.FindControl("ChkSelectAll");
                    if (oCheckBox != null)
                    {
                        oCheckBox.Checked = false;
                        oCheckBox.Focus();
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
    /// This event is used to display leaves of respective user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwUsers_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            int iStaffAttendanceId = Convert.ToInt32(lstvwUsers.DataKeys[oCurrentItem.DisplayIndex]["StaffAttendanceId"]);

            StringBuilder oStaffLeaves = new StringBuilder();
            var oLeaves = moStaffLeaveDetails.Where(leave => leave.StaffAttendanceId == iStaffAttendanceId);
            if (oLeaves != null)
                oLeaves.OrderBy(leave => leave.OriginalLeaveId).ToList().ForEach(leave => oStaffLeaves.Append(", " + leave.ShortName + " (" + leave.Days + ")"));

            if (oStaffLeaves.ToString().StartsWith(", "))
            {
                Label lblUsedLeaves = oCurrentItem.FindControl("lblUsedLeaves") as Label;
                lblUsedLeaves.Text = oStaffLeaves.ToString().Substring(2);
            }
        }
        catch (Exception ex)
        {
          ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save staff attendance.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            new StaffAttendanceBL
            {
                StaffAttendance = new StaffAttendance
                {
                    SchoolId = miSchoolId,
                    AcademicYearId = miAcademicYearId,
                    InsertedById = miUserId,
                    MonthId = Convert.ToInt32(hidMonthId.Value),
                    Year = Convert.ToInt32(hidYear.Value),
                    UserIdsXML = GenerateUserIdsXml()
                }
            }
            .SaveStaffAttendance();
            FillUserListview();
            lblMessage.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to close popup and refresh parent screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            ClosePopup();
        }
        catch (Exception ex)
        {
          ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnClose });
        btnSave.Attributes.Add("onclick", "if(!CheckSelectedGroups()) return false;");
    }

    /// <summary>
    /// This method is used to close popup.
    /// </summary>
    private void ClosePopup()
    {
        string sQuerystring = String.Format("MonthId={0}&Year={1}&StaffGroupId={2}&Filter={3}", hidMonthId.Value, hidYear.Value, hidStaffGroup.Value, hidFilter.Value);
        sQuerystring = CommonUtility.EncryptQuerystring(sQuerystring);
        sQuerystring = String.Format("'?{0}'", sQuerystring);
        Response.Write(String.Format("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+{0};window.close();window.opener.focus(); </Script>", sQuerystring));
    }

    /// <summary>
    /// This method is used to decrypt query string.
    /// </summary>
    private void ReadQueryString()
    {
        hidMonthId.Value = QueryString["MonthId"];
        hidYear.Value = QueryString["Year"];
        hidStaffGroup.Value = QueryString["StaffGroupId"];
        if (QueryString["IsStaticOutput"] == Constants.S_YES)
        {
            btnSave.Visible = false;
            hidIsStaticOutput.Value = "Y";
        }
        else
            hidIsStaticOutput.Value = "N";
        if (QueryString["Filter"] != null)
            hidFilter.Value = QueryString["Filter"];

        lblSalaryMonth.Text = new DateTime(Convert.ToInt32(hidYear.Value), Convert.ToInt32(hidMonthId.Value), 1).ToString("MMMM") + " - " + hidYear.Value;        
    }

    /// <summary>
    /// This method is used to fill staff group combobox.
    /// </summary>
    private void FillStaffGroupCombobox()
    {   
        StaffGroupsBL oStaffGroupsBL = new StaffGroupsBL();        
        List<StaffGroupsEntity> staffGroups = oStaffGroupsBL.GetAllStaffGroups(miSchoolId);
        ListSource.FillDropDownList(staffGroups, cmbStaffGroup, "staffGroupsName", "staffGroupsId", Constants.S_ALL);
        if (cmbStaffGroup.Items.Count > 0)
            cmbStaffGroup.SelectedValue = hidStaffGroup.Value;
    }

    /// <summary>
    /// This method is used to fill user listview.
    /// </summary>
    private void FillUserListview()
    {
        if (cmbStaffGroup.Items.Count > 0)
        {
            StaffAttendanceBL oStaffGroupsBL = new StaffAttendanceBL
             {
                 StaffAttendance = new StaffAttendance
                 {
                     SchoolId = miSchoolId,
                     AcademicYearId = miAcademicYearId,
                     StaffGroupsId = Convert.ToInt32(cmbStaffGroup.SelectedValue),
                     MonthId = Convert.ToInt32(hidMonthId.Value),
                     Year = Convert.ToInt32(hidYear.Value)
                 }
             };
            
            oStaffGroupsBL.GetStaffGroupUsers();
            moStaffLeaveDetails = oStaffGroupsBL.StaffLeaveDetails;
            List<StaffAttendance> olstStaffAttendances = oStaffGroupsBL.StaffAttendances;

            if (olstStaffAttendances.Count > 0)
            {
                lstvwUsers.DataSource = olstStaffAttendances;                
                DisplayControls(true);
            }
            else
            {
                lstvwUsers.DataSource = null;                
                DisplayControls(false);
            }
            lstvwUsers.DataBind();
        }
    }

    /// <summary>
    /// This method is used to display controls if list view contains at least one record.
    /// </summary>
    /// <param name="abAction"></param>
    private void DisplayControls(bool abAction)
    {
        divContainer.Visible = abAction;
        btnSave.Visible = abAction;
        if (hidIsStaticOutput.Value == "Y")
            btnSave.Visible = false;
        trNoRecordFound.Visible = !abAction;
        tblNote.Visible = abAction;
    }

    /// <summary>
    /// This method is used to generate user ids xml to save.
    /// </summary>
    /// <returns></returns>
    private string GenerateUserIdsXml()
    {
        XmlDocument oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement("UserIds");
        XmlNode rootNode = oDoc.CreateNode("element", "UserIds", string.Empty);

        foreach (ListViewDataItem oCurrentItem in lstvwUsers.Items)
        {
            CheckBox chkSelect = oCurrentItem.FindControl("chkSelect") as CheckBox;
            if (chkSelect.Checked)
            {
                XmlNode node = oDoc.CreateNode("element", "UserIds", string.Empty);

                XmlAttribute attr = oDoc.CreateAttribute("UserId");
                attr.Value = lstvwUsers.DataKeys[oCurrentItem.DisplayIndex]["UserId"].ToString();
                node.Attributes.Append(attr);
                rootNode.AppendChild(node);
            }
        }
        root.AppendChild(rootNode);
        return root.InnerXml;
    }
    #endregion
}