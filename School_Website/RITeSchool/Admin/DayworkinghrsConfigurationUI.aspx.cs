using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using SchoolEntities;
using System.Data;
using System.Xml.Serialization;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Linq;

public partial class DayworkinghrsConfigurationUI : SchoolBase
{
    #region Constants

    private const string S_SAVE_MESSAGE = "Full hour and Half hour Details saved successfully !!!";

    #endregion

    #region DataMember

    private WorkinghoursBL moWorkinghrsBL;

    #endregion

    #region enums

    public enum WeekdayConfiguration
    {
        Sunday = 1,
        Monday = 2,
        Tuesday = 3,
        Wednesday = 4,
        Thursday = 5,
        Friday = 6,
        Saturday = 7
    }

    #endregion

    #region Event(s)

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moWorkinghrsBL = new WorkinghoursBL(miSchoolId, miAcademicYearId);
            if (!IsPostBack)
            {
                FillStanderdCombo();
                SetJavaScriptAttributes();
                FillList();

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save the Working hours details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            btnSave.Attributes.Add("onclick", "if(!CheckValidations()){return false;}");
            Save();
            base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used the bound data to the list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwWorkinghrs_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                int iDivisionId = Convert.ToInt32(lstvwWorkinghrs.DataKeys[e.Item.DisplayIndex]["DivisionId"]);

                TextBox txtfullhrsMonday = e.Item.FindControl("txtfullhrsMonday") as TextBox;
                TextBox txtHalfhrsMonday = e.Item.FindControl("txtHalfhrsMonday") as TextBox;
                TextBox txtfullhrsTuesday = e.Item.FindControl("txtfullhrsTuesday") as TextBox;
                TextBox txtHalfhrsTuesday = e.Item.FindControl("txtHalfhrsTuesday") as TextBox;
                TextBox txtfullhrsWednesday = e.Item.FindControl("txtfullhrsWednesday") as TextBox;
                TextBox txtHalfhrsWednesday = e.Item.FindControl("txtHalfhrsWednesday") as TextBox;
                TextBox txtfullhrsThursday = e.Item.FindControl("txtfullhrsThursday") as TextBox;
                TextBox txtHalfhrsThursday = e.Item.FindControl("txtHalfhrsThursday") as TextBox;
                TextBox txtfullhrsFriday = e.Item.FindControl("txtfullhrsFriday") as TextBox;
                TextBox txtHalfhrsFriday = e.Item.FindControl("txtHalfhrsFriday") as TextBox;
                TextBox txtfullhrsSaturday = e.Item.FindControl("txtfullhrsSaturday") as TextBox;
                TextBox txtHalfhrsSaturday = e.Item.FindControl("txtHalfhrsSaturday") as TextBox;
                TextBox txtfullhrsSunday = e.Item.FindControl("txtfullhrsSunday") as TextBox;
                TextBox txtHalfhrsSunday = e.Item.FindControl("txtHalfhrsSunday") as TextBox;

                if (moWorkinghrsBL.WorkinghoursDetails.Count > Constants.I_ZERO)
                {
                    WorkinghrsDetails oWorkinghrsDetails;

                    if (moWorkinghrsBL.WorkinghoursDetails.Where(div => div.DivisionId == iDivisionId).Any())
                    {
                        oWorkinghrsDetails = SetFieldValue(txtfullhrsSunday, txtHalfhrsSunday, iDivisionId, WeekdayConfiguration.Sunday);
                        oWorkinghrsDetails = SetFieldValue(txtfullhrsMonday, txtHalfhrsMonday, iDivisionId, WeekdayConfiguration.Monday);
                        oWorkinghrsDetails = SetFieldValue(txtfullhrsTuesday, txtHalfhrsTuesday, iDivisionId, WeekdayConfiguration.Tuesday);
                        oWorkinghrsDetails = SetFieldValue(txtfullhrsWednesday, txtHalfhrsWednesday, iDivisionId, WeekdayConfiguration.Wednesday);
                        oWorkinghrsDetails = SetFieldValue(txtfullhrsThursday, txtHalfhrsThursday, iDivisionId, WeekdayConfiguration.Thursday);
                        oWorkinghrsDetails = SetFieldValue(txtfullhrsFriday, txtHalfhrsFriday, iDivisionId, WeekdayConfiguration.Friday);
                        oWorkinghrsDetails = SetFieldValue(txtfullhrsSaturday, txtHalfhrsSaturday, iDivisionId, WeekdayConfiguration.Saturday);
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
    /// This method is used for Combobox selected  index change event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandardId_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillList();
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This method returs all the value.
    /// </summary>

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used for the set fields Value to Textbox;
    /// </summary>
    /// <param name="iDivisionId"></param>
    /// <param name="oWeekdayConfiguration"></param>
    /// <param name="txtfullhrsMonday"></param>
    /// <param name="txtHalfhrsMonday"></param>
    private WorkinghrsDetails SetFieldValue(TextBox txtfullhrsMonday, TextBox txtHalfhrsMonday, int iDivisionId, WeekdayConfiguration oWeekdayConfiguration)
    {
        WorkinghrsDetails oWorkinghrsDetails;
        List<WorkinghrsDetails> lstHours = moWorkinghrsBL.WorkinghoursDetails.Where(div => div.DivisionId == iDivisionId).ToList();
        oWorkinghrsDetails = lstHours.Where(WeekDay => WeekDay.WeekdayNumber == oWeekdayConfiguration.ToInt()).FirstOrDefault();
        if (oWorkinghrsDetails != null)
        {
            txtfullhrsMonday.Text = oWorkinghrsDetails.FullHours.ToString();
            txtHalfhrsMonday.Text = oWorkinghrsDetails.HalfHours.ToString();
        }
        return oWorkinghrsDetails;
    }

    /// <summary>
    /// This method is used for the Save details of working hours.
    /// </summary>
    private void Save()
    {
        WorkinghoursBL moWorkinghrsBL = new WorkinghoursBL(miSchoolId, miAcademicYearId);
        List<WorkinghrsDetails> lstWorkinghrsDetails = PopulateHoursDetails();
        moWorkinghrsBL.InsertWorkingHrsDetails(cmbStandardId.SelectedValue.ToInt(), GetHrsDetailXML(lstWorkinghrsDetails), miUserId);
    }

    /// <summary>
    /// This method Populate the  Details From the list.
    /// </summary>
    /// <returns></returns>
    private List<WorkinghrsDetails> PopulateHoursDetails()
    {
        List<WorkinghrsDetails> lstWorkinghrsDetails = new List<WorkinghrsDetails>();
        WorkinghrsDetails oWorkinghrsDetails = null;
        TextBox txtfullhours = null;
        TextBox txtHalfhours = null;

        for (int iRowCount = 0; iRowCount < lstvwWorkinghrs.Items.Count; iRowCount++)
        {

            int iDivisionId = Convert.ToInt32(lstvwWorkinghrs.DataKeys[iRowCount]["DivisionId"]);

            ListViewDataItem oCurrentItem = lstvwWorkinghrs.Items[iRowCount] as ListViewDataItem;
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

            oWorkinghrsDetails = new WorkinghrsDetails();
            txtfullhours = lstvwWorkinghrs.Items[iRowCount].FindControl("txtfullhrsMonday") as TextBox;
            txtHalfhours = lstvwWorkinghrs.Items[iRowCount].FindControl("txtHalfhrsMonday") as TextBox;
            oWorkinghrsDetails.DivisionId = iDivisionId;
            oWorkinghrsDetails.FullHours = Convert.ToDecimal(txtfullhours.Text);
            oWorkinghrsDetails.HalfHours = Convert.ToDecimal(txtHalfhours.Text);
            oWorkinghrsDetails.WeekdayNumber = WeekdayConfiguration.Monday.ToInt();
            lstWorkinghrsDetails.Add(oWorkinghrsDetails);

            oWorkinghrsDetails = new WorkinghrsDetails();
            txtfullhours = lstvwWorkinghrs.Items[iRowCount].FindControl("txtfullhrsTuesday") as TextBox;
            txtHalfhours = lstvwWorkinghrs.Items[iRowCount].FindControl("txtHalfhrsTuesday") as TextBox;
            oWorkinghrsDetails.DivisionId = iDivisionId;
            oWorkinghrsDetails.FullHours = Convert.ToDecimal(txtfullhours.Text);
            oWorkinghrsDetails.HalfHours = Convert.ToDecimal(txtHalfhours.Text);
            oWorkinghrsDetails.WeekdayNumber = WeekdayConfiguration.Tuesday.ToInt();
            lstWorkinghrsDetails.Add(oWorkinghrsDetails);

            oWorkinghrsDetails = new WorkinghrsDetails();
            txtfullhours = lstvwWorkinghrs.Items[iRowCount].FindControl("txtfullhrsWednesday") as TextBox;
            txtHalfhours = lstvwWorkinghrs.Items[iRowCount].FindControl("txtHalfhrsWednesday") as TextBox;
            oWorkinghrsDetails.DivisionId = iDivisionId;
            oWorkinghrsDetails.FullHours = Convert.ToDecimal(txtfullhours.Text);
            oWorkinghrsDetails.HalfHours = Convert.ToDecimal(txtHalfhours.Text);
            oWorkinghrsDetails.WeekdayNumber = WeekdayConfiguration.Wednesday.ToInt();
            lstWorkinghrsDetails.Add(oWorkinghrsDetails);


            oWorkinghrsDetails = new WorkinghrsDetails();
            txtfullhours = lstvwWorkinghrs.Items[iRowCount].FindControl("txtfullhrsThursday") as TextBox;
            txtHalfhours = lstvwWorkinghrs.Items[iRowCount].FindControl("txtHalfhrsThursday") as TextBox;
            oWorkinghrsDetails.DivisionId = iDivisionId;
            oWorkinghrsDetails.FullHours = Convert.ToDecimal(txtfullhours.Text);
            oWorkinghrsDetails.HalfHours = Convert.ToDecimal(txtHalfhours.Text);
            oWorkinghrsDetails.WeekdayNumber = WeekdayConfiguration.Thursday.ToInt();
            lstWorkinghrsDetails.Add(oWorkinghrsDetails);


            oWorkinghrsDetails = new WorkinghrsDetails();
            txtfullhours = lstvwWorkinghrs.Items[iRowCount].FindControl("txtfullhrsFriday") as TextBox;
            txtHalfhours = lstvwWorkinghrs.Items[iRowCount].FindControl("txtHalfhrsFriday") as TextBox;
            oWorkinghrsDetails.DivisionId = iDivisionId;
            oWorkinghrsDetails.FullHours = Convert.ToDecimal(txtfullhours.Text);
            oWorkinghrsDetails.HalfHours = Convert.ToDecimal(txtHalfhours.Text);
            oWorkinghrsDetails.WeekdayNumber = WeekdayConfiguration.Friday.ToInt();
            lstWorkinghrsDetails.Add(oWorkinghrsDetails);


            oWorkinghrsDetails = new WorkinghrsDetails();
            txtfullhours = lstvwWorkinghrs.Items[iRowCount].FindControl("txtfullhrsSaturday") as TextBox;
            txtHalfhours = lstvwWorkinghrs.Items[iRowCount].FindControl("txtHalfhrsSaturday") as TextBox;
            oWorkinghrsDetails.DivisionId = iDivisionId;
            oWorkinghrsDetails.FullHours = Convert.ToDecimal(txtfullhours.Text);
            oWorkinghrsDetails.HalfHours = Convert.ToDecimal(txtHalfhours.Text);
            oWorkinghrsDetails.WeekdayNumber = WeekdayConfiguration.Saturday.ToInt();
            lstWorkinghrsDetails.Add(oWorkinghrsDetails);

            oWorkinghrsDetails = new WorkinghrsDetails();
            txtfullhours = lstvwWorkinghrs.Items[iRowCount].FindControl("txtfullhrsSunday") as TextBox;
            txtHalfhours = lstvwWorkinghrs.Items[iRowCount].FindControl("txtHalfhrsSunday") as TextBox;
            oWorkinghrsDetails.DivisionId = iDivisionId;
            oWorkinghrsDetails.FullHours = Convert.ToDecimal(txtfullhours.Text);
            oWorkinghrsDetails.HalfHours = Convert.ToDecimal(txtHalfhours.Text);
            oWorkinghrsDetails.WeekdayNumber = WeekdayConfiguration.Sunday.ToInt();
            lstWorkinghrsDetails.Add(oWorkinghrsDetails);
        }
        return lstWorkinghrsDetails;
    }

    /// <summary>
    /// This method is used for the generate the XML .
    /// </summary>
    /// <param name="lstHrsDetails"></param>
    /// <returns></returns>
    private string GetHrsDetailXML(List<WorkinghrsDetails> lstHrsDetails)
    {
        StringWriter sw = new StringWriter();
        new XmlSerializer(lstHrsDetails.GetType()).Serialize(sw, lstHrsDetails);
        string sXML = sw.ToString();
        sXML = sXML.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
        return sXML;
    }

    /// <summary>
    /// This Method is used Fill Standard DropDown List.
    /// </summary>
    private void FillStanderdCombo()
    {
        var oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        cmbStandardId.Bind(oDtStandardCollection, Constants.S_STANDARD_ID_FIELD, Constants.S_STANDARD_NAME_FIELD, Constants.S_SELECT);
    }

    /// <summary>
    /// This Method is used Fill  ListView.
    /// </summary>
    private void FillList()
    {
        List<WorkinghrsDetails> oWorkinghrsDetails = moWorkinghrsBL.GetAllDivisionsForStandard(cmbStandardId.SelectedValue.ToInt());
        lstvwWorkinghrs.DataSource = oWorkinghrsDetails;
        lstvwWorkinghrs.DataBind();
        if (cmbStandardId.SelectedValue == Constants.S_ZERO)
        {
            btnSave.Enabled = false;
            LegendTable.Visible = false;
        }
        else
        {
            btnSave.Enabled = true;
            LegendTable.Visible = true;
        }
    }

    /// <summary>
    /// This Method is used to set java script attributes to control.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        btnSave.Attributes.Add("onclick", "if(!CheckValidations()){return false;}");
        btnSave.Enabled = false;
    }

    /// <summary>
    /// This method is used to clear all controls as per the standard combobox selected index
    /// </summary>
    private void ClearFields()
    {
        TextBox txtfullhours = null;
        TextBox txtHalfhours = null;

        HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwWorkinghrs.FindControl("trheader2");

        txtfullhours = (TextBox)oHtmlTableRow.FindControl("txtfullhrsMonday");
        txtfullhours.Text = string.Empty;
        txtHalfhours = (TextBox)oHtmlTableRow.FindControl("txtHalfhrsMonday");
        txtHalfhours.Text = string.Empty;

        txtfullhours = (TextBox)oHtmlTableRow.FindControl("txtfullhrsTuesday");
        txtfullhours.Text = string.Empty;
        txtHalfhours = (TextBox)oHtmlTableRow.FindControl("txtHalfhrsTuesday");
        txtHalfhours.Text = string.Empty;

        txtfullhours = (TextBox)oHtmlTableRow.FindControl("txtfullhrsWednesday");
        txtfullhours.Text = string.Empty;
        txtHalfhours = (TextBox)oHtmlTableRow.FindControl("txtHalfhrsWednesday");
        txtHalfhours.Text = string.Empty;


        txtfullhours = (TextBox)oHtmlTableRow.FindControl("txtfullhrsThursday");
        txtfullhours.Text = string.Empty;
        txtHalfhours = (TextBox)oHtmlTableRow.FindControl("txtHalfhrsThursday");
        txtHalfhours.Text = string.Empty;

        txtfullhours = (TextBox)oHtmlTableRow.FindControl("txtfullhrsFriday");
        txtfullhours.Text = string.Empty;
        txtHalfhours = (TextBox)oHtmlTableRow.FindControl("txtHalfhrsFriday");
        txtHalfhours.Text = string.Empty;

        txtfullhours = (TextBox)oHtmlTableRow.FindControl("txtfullhrsSaturday");
        txtfullhours.Text = string.Empty;
        txtHalfhours = (TextBox)oHtmlTableRow.FindControl("txtHalfhrsSaturday");
        txtHalfhours.Text = string.Empty;

        txtfullhours = (TextBox)oHtmlTableRow.FindControl("txtfullhrsSunday");
        txtfullhours.Text = string.Empty;
        txtHalfhours = (TextBox)oHtmlTableRow.FindControl("txtHalfhrsSunday");
        txtHalfhours.Text = string.Empty;
        lblMessage.Text = string.Empty;
    }

    #endregion
}
