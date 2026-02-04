/* File Name :- DatewiseClassHalfDayConfigurationUI.aspx.cs
 * Created Date :- 29-Nov-2016
 * Class Description :- This class is used to manage Class wise half day Configuration details.
 * Created By :- Dnyaneshwar Shinde.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using SchoolEntities;
using BusinessLogic;
using Utility;
using System.Text;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

public partial class DatewiseClassHalfDayConfigurationUI : SchoolBase
{
    #region Constant(s)

    private const string S_DELETE_MESSAGE = "Half Day Configuration details deleted successfully !!!";
    private const string S_UPDATE_MESSAGE = "Half Day Configuration details updated successfully !!!";
    private const string S_SAVE_MESSAGE = "Half Day Configuration details Saved successfully !!!";
    private const string S_UPDATE_TEXT = "Update";
    private const string S_SAVE_TEXT = "Save";
    private const string S_DATE_FORMATE = "1900-01-01 00:00:00.000";
    private const string S_SORT_ROW = "SortRow";

    #endregion

    #region DataMember

    private SchoolWorkinDetailsBL moSchoolWorkinDetailsBL;    

    #endregion

    #region Event's

    /// <summary>
    /// Thos event is used to add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>    
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        try
        {
            if (hidSortExpression.Value == string.Empty)
            {
                hidSortExpression.Value = "Date";
                hidSortDirection.Value = Constants.S_DESCENDING;
            }
            base.AddSortImage(lstViewHalfDayStandardDivDetails, hidSortExpression.Value, hidSortDirection.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the OnInit Controls.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            moSchoolWorkinDetailsBL = new SchoolWorkinDetailsBL(miSchoolId, miAcademicYearId, miUserId);            
            if(btnSave.ClientID != null)
                FillStandardDetails(0);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the page Load Events.
    /// </summary>   
    /// <param name="e"></param>
    /// <param name="sender"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            moSchoolWorkinDetailsBL = new SchoolWorkinDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                FillStandardDetails(1);
                SetJavascriptAttributes();
                FillDatewiseHalfDayDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the values in Standard Division Listwiew.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="sender"></param>
    protected void lstViewStdDivDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Label lblStandardName = e.Item.FindControl("lblStandard") as Label;
                string sStandardName = Convert.ToString(lstViewStdDivDetails.DataKeys[e.Item.DisplayIndex]["StandardName"]);
                int iStandardId = Convert.ToInt32(lstViewStdDivDetails.DataKeys[e.Item.DisplayIndex]["StandardId"]);
                if (sStandardName != null)
                    lblStandardName.Text = sStandardName;
                CheckBox chkStdHeaderCheckBox = e.Item.FindControl("chkAllStandards") as CheckBox;
                chkStdHeaderCheckBox.Attributes.Add("onclick", "CheckStdHeaderCheckbox(this," + e.Item.DisplayIndex + ")");
                List<int> lstStdDivision = new List<int>();
                lstStdDivision = moSchoolWorkinDetailsBL.SchoolWorkinDivisionDetails.Select(div => div.DivisionId).ToList();

                foreach (var lstDiv in moSchoolWorkinDetailsBL.SchoolWorkinDivisionDetails)
                {                    
                   CheckBox chkDiv = e.Item.FindControl("chk_" + lstDiv.DivisionId) as CheckBox;
                   if (chkDiv != null)
                   {
                       int iChkId = Convert.ToInt32(chkDiv.ID.Substring(4));
                       if (moSchoolWorkinDetailsBL.SchoolWorkingStdDivDetails.Any(div => div.DivisionID == iChkId && div.StandardId == iStandardId))
                           chkDiv.Visible = true;
                       else
                           chkDiv.Visible = false;
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
    /// This event is used to save the configured Standard & divisions.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="sender"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            moSchoolWorkinDetailsBL = new SchoolWorkinDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            string sStdDivIds = Populate();
            if (hidHalfDayDate.Value == string.Empty)
                hidHalfDayDate.Value = Convert.ToString(S_DATE_FORMATE.ToDateTime());
            moSchoolWorkinDetailsBL.Save(sStdDivIds, txtHalfDayDate.Text.ToDateTime(), hidHalfDayDate.Value.ToDateTime());
            FillDatewiseHalfDayDetails();
            FillStandardDetails(0);
            if (btnSave.Text == S_UPDATE_TEXT)
                base.DisplayMessage(S_UPDATE_MESSAGE, false, tdMessage);
            else
                base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
            ClearFields();
        }
        catch (SqlException ex)
        {
            base.DisplayMessage(ex.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to cleare all configured Standard & divisions.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="sender"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bound the data to listview.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="sender"></param>
    protected void lstViewHalfDayStandardDivDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                SchoolWorkingDetails oSchoolWorkingDetails = e.Item.DataItem as SchoolWorkingDetails;
                ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
                btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
                Label lblDate = e.Item.FindControl("lblDate") as Label;
                if (oSchoolWorkingDetails.HalfDayDate.ToString(Constants.S_DATE_FORMAT) != Constants.S_DEFAULT_DATE_6)
                    lblDate.Text = oSchoolWorkingDetails.HalfDayDate.ToString(Constants.S_DATE_FORMAT);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the commands used in listview
    /// </summary>
    /// <param name="e"></param>
    /// <param name="sender"></param>
    protected void lstViewHalfDayStandardDivDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                moSchoolWorkinDetailsBL = new SchoolWorkinDetailsBL(miSchoolId, miAcademicYearId, miUserId);
                Label lblDate = e.Item.FindControl("lblDate") as Label;
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnSave.Text = S_UPDATE_TEXT;
                    moSchoolWorkinDetailsBL.GetAll(lblDate.Text.ToDateTime());
                    SetCheckBoxCheck();
                    hidHalfDayDate.Value = lblDate.Text;
                    txtHalfDayDate.Text = lblDate.Text;
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moSchoolWorkinDetailsBL.Delete(lblDate.Text.ToDateTime());
                    FillDatewiseHalfDayDetails();
                    ClearFields();
                    base.DisplayMessage(S_DELETE_MESSAGE, false, tdMessage);
                }
            }
            else if (e.Item.ItemType == ListViewItemType.EmptyItem && e.CommandSource is LinkButton && e.CommandName == S_SORT_ROW)
            {
                base.RevertSortOrder(hidSortDirection);
                hidSortExpression.Value = e.CommandArgument.ToString();
                FillDatewiseHalfDayDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to deleting the data in listview.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="sender"></param>
    protected void lstViewHalfDayStandardDivDetails_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        try { }
        catch (Exception ex) { ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod()); }
    }

    /// <summary>
    /// This event is used to updating data in listview.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="sender"></param>
    protected void lstViewHalfDayStandardDivDetails_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        try { }
        catch (Exception ex) { ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod()); }
    }

    /// <summary>
    /// This event is used to display listview record according to value in page combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstViewHalfDayStandardDivDetails);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to bound the data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstViewHalfDayStandardDivDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstViewHalfDayStandardDivDetails.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooter(lstViewHalfDayStandardDivDetails, DtPgCount);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstViewHalfDayStandardDivDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            if(hidSortExpression.Value != e.SortExpression)
                hidSortDirection.Value = Constants.S_DESCENDING;
            base.RevertSortOrder(hidSortDirection);
            hidSortExpression.Value = e.SortExpression;
            FillDatewiseHalfDayDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method is used to fill the standard division details,
    /// </summary>
    /// <param name="dtHaldDayDate"></param>
    /// <param name="iVal"></param>
    private void FillStandardDetails(int aiVal)
    {
        List<SchoolWorkingStandardDetails> lstStandardDetails = new List<SchoolWorkingStandardDetails>();
        lstStandardDetails = moSchoolWorkinDetailsBL.GetAll(S_DATE_FORMATE.ToDateTime());
        StringBuilder oStringBuilder = new StringBuilder();
        List<SchoolWorkinDivisionDetails> lstSchoolDivisionDetails = moSchoolWorkinDetailsBL.SchoolWorkinDivisionDetails;
        BindListViewTemplate(lstSchoolDivisionDetails);
        if (aiVal == Constants.I_ONE)
        {
            foreach (var oDivisionItem in moSchoolWorkinDetailsBL.SchoolWorkinDivisionDetails)
            {
                oStringBuilder.Append(",").Append(oDivisionItem.DivisionId);                
            }            
            hidDivisionIds.Value = oStringBuilder.ToString().Substring(1);
        }
        lstViewStdDivDetails.DataSource = lstStandardDetails;
        lstViewStdDivDetails.DataBind();
    }

    /// <summary>
    /// This method is used to bind listview template.
    /// </summary>
    /// <param name="mlstSchoolDivisionDetails"></param>
    private void BindListViewTemplate(List<SchoolWorkinDivisionDetails> mlstSchoolDivisionDetails)
    {
        int iCount = mlstSchoolDivisionDetails.Count;

        if (iCount > Constants.I_ZERO)
        {
            lstViewStdDivDetails.LayoutTemplate = new ListViewStandardDivisionDetails(ListViewItemType.EmptyItem, mlstSchoolDivisionDetails, false);
            lstViewStdDivDetails.ItemTemplate = new ListViewStandardDivisionDetails(ListViewItemType.DataItem, mlstSchoolDivisionDetails, false);
            lstViewStdDivDetails.AlternatingItemTemplate = new ListViewStandardDivisionDetails(ListViewItemType.DataItem, mlstSchoolDivisionDetails, true);
        }
    }

    /// <summary>
    /// This method is used to populate All the details to save.
    /// </summary>
    private string Populate()
    {
        moSchoolWorkinDetailsBL = new SchoolWorkinDetailsBL(miSchoolId, miAcademicYearId, miUserId);
        moSchoolWorkinDetailsBL.GetAll(DateTime.Now.Date);
        StringBuilder oStringBuilder = new StringBuilder();
        string sStdDivIds = string.Empty;

        for (int iRowNo = 0; iRowNo < lstViewStdDivDetails.Items.Count; iRowNo++)
        {
            int iStandardId = Convert.ToInt32(lstViewStdDivDetails.DataKeys[iRowNo]["StandardId"]);

            foreach (var lstDiv in moSchoolWorkinDetailsBL.SchoolWorkinDivisionDetails)
            {
                CheckBox chkDivision = lstViewStdDivDetails.Items[iRowNo].FindControl("chk_" + lstDiv.DivisionId) as CheckBox;
                if (chkDivision != null)
                {
                    int iDivId = Convert.ToInt32(chkDivision.ID.Substring(4).ToString());
                    if (chkDivision.Checked)
                    {
                        int iStdDivId = moSchoolWorkinDetailsBL.SchoolWorkingStdDivDetails.Where(stdDivId => stdDivId.DivisionID == iDivId && stdDivId.StandardId == iStandardId).Select(stdDivId => stdDivId.StandardDivisionId).FirstOrDefault();
                        oStringBuilder.Append(",").Append(iStdDivId);
                    }
                }
            }
        }
        return oStringBuilder.ToString().Substring(1);
    }

    /// <summary>
    /// This method is used to set the Check box check on click of edit button in second listview.
    /// </summary>
    private void SetCheckBoxCheck()
    {
        for (int iRowNo = 0; iRowNo < lstViewStdDivDetails.Items.Count; iRowNo++)
        {
            foreach (var lstDiv in moSchoolWorkinDetailsBL.SchoolWorkinDivisionDetails)
            {
                CheckBox chkDivision = lstViewStdDivDetails.Items[iRowNo].FindControl("chk_" + lstDiv.DivisionId) as CheckBox;
                int iDivId = Convert.ToInt32(chkDivision.ID.Substring(4).ToString());
                int iStandardId = Convert.ToInt32(lstViewStdDivDetails.DataKeys[iRowNo]["StandardId"]);

                if (moSchoolWorkinDetailsBL.SchoolWorkingDetails.Any(ys => ys.DivisionID == iDivId && ys.StandardId == iStandardId))
                {
                    chkDivision.Checked = true;
                }
            }
        }
    }

    /// <summary>
    /// This method is used to set the java script attributes to controls.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        btnSave.Attributes.Add("onclick", "if(!CheckSaveCheckbox()){return false}");
        hidFirstFxFollowingErrors.Value = Resources.LocalizedResources.PleaseFixFollowingError;
        base.ApplyMouseHoverEffect(new List<Button> { btnBack, btnSave, btnCancel });
        if (txtHalfDayDate.Text == string.Empty)
            txtHalfDayDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
    }

    /// <summary>
    /// This method is used to fill date wise half day details.
    /// </summary>
    private void FillDatewiseHalfDayDetails()
    {
        lstViewHalfDayStandardDivDetails.DataSourceID = lstvwDSobj.ID;
    }

    /// <summary>
    /// This method is used to cleare All the fields.
    /// </summary>
    private void ClearFields()
    {
        btnSave.Text = S_SAVE_TEXT;
        moSchoolWorkinDetailsBL.GetAll(S_DATE_FORMATE.ToDateTime());
        txtHalfDayDate.Text = DateTime.Now.Date.ToString(Constants.S_DATE_FORMAT);
        if (chkAll.Checked)
            chkAll.Checked = false;
        for (int iRowNo = 0; iRowNo < lstViewStdDivDetails.Items.Count; iRowNo++)
        {
            foreach (var lstDiv in moSchoolWorkinDetailsBL.SchoolWorkinDivisionDetails)
            {
                CheckBox chkDivision = lstViewStdDivDetails.Items[iRowNo].FindControl("chk_" + lstDiv.DivisionId) as CheckBox;

                chkDivision.Checked = false;
            }
        }
        foreach (var lstDiv in moSchoolWorkinDetailsBL.SchoolWorkinDivisionDetails)
        {
            CheckBox chkAllDivision = lstViewStdDivDetails.FindControl("chkAllDivisions_" + lstDiv.DivisionId) as CheckBox;

            if (chkAllDivision != null)
                chkAllDivision.Checked = false;
        }
    }

    /// <summary>
    /// This class is used to fill list view template.
    /// </summary>
    public class ListViewStandardDivisionDetails : ITemplate
    {
        private ListViewItemType lstvwItemType;
        private List<SchoolWorkinDivisionDetails> lstWorkingDivisionDetails;
        private bool isAlterNateRow = false;

        public ListViewStandardDivisionDetails(ListViewItemType alstItemType, List<SchoolWorkinDivisionDetails> alstSchoolWorkingDivisions, bool isAlternate)
        {
            lstvwItemType = alstItemType;
            lstWorkingDivisionDetails = alstSchoolWorkingDivisions;
            isAlterNateRow = isAlternate;
        }

        public void InstantiateIn(Control aoContainer)
        {
            if (lstvwItemType == ListViewItemType.DataItem)
            {
                Literal ltrlDataItemTr = new Literal();
                Literal ltrlDataItemTd = new Literal();
                Label lblStandard = new Label();
                Label lblDivision = new Label();
                CheckBox chkAllStandards = new CheckBox();
                Literal ltrlDataItemName = new Literal();
                Literal ltrlDataItemTdClose = new Literal();
                Literal ltrlDataItemTrClose = new Literal();

                ltrlDataItemTr.Text = isAlterNateRow == false ? "<tr class='ClsGridRow'>" : "<tr class='ClsGridAltRow'>";
                ltrlDataItemTr.ID = "trHeaderCheck";
                aoContainer.Controls.Add(ltrlDataItemTr);

                ltrlDataItemTrClose.Text = "</tr>";

                ltrlDataItemTd.Text = "<td align ='left' width='40px'>";
                lblStandard.ID = "lblStandard";
                ltrlDataItemTrClose.Text = "</td>";

                chkAllStandards.ID = "ChkAllStandards";

                aoContainer.Controls.Add(ltrlDataItemTd);
                aoContainer.Controls.Add(chkAllStandards);
                aoContainer.Controls.Add(lblStandard);
                aoContainer.Controls.Add(ltrlDataItemTrClose);

                for (int iNo = 0; iNo < lstWorkingDivisionDetails.Count; iNo++)
                {
                    Literal ltrltd = new Literal();
                    Literal ltrtdClose = new Literal();
                    ltrltd.Text = "<td align = 'center' width='100px'>";
                    ltrtdClose.Text = "</td>";

                    CheckBox ChkDiv = new CheckBox();
                    ChkDiv.ID = "Chk_" + lstWorkingDivisionDetails[iNo].DivisionId;
                    ChkDiv.Width = Unit.Pixel(50);

                    aoContainer.Controls.Add(ltrltd);
                    aoContainer.Controls.Add(ChkDiv);
                    aoContainer.Controls.Add(ltrtdClose);
                }
                aoContainer.Controls.Add(ltrlDataItemTrClose);
            }
            else
            {
                Literal ltrlHeadertbl = new Literal();
                Literal ltrlDataItemTr = new Literal();
                Literal ltrlDataItemTd = new Literal();
                Label lblStandard = new Label();
                Label lblDivision = new Label();
                Literal ltrlDataItemName = new Literal();
                Literal ltrlDataItemTdClose = new Literal();
                Literal ltrlDataItemTrClose = new Literal();

                ltrlHeadertbl.Text = "<table cellpadding='0' cellspacing='1' style='color: #333333' class='GridBorder' align='center'>";
                ltrlHeadertbl.Text += "<tr class='ClsGridHeader'>";

                aoContainer.Controls.Add(ltrlHeadertbl);

                ltrlDataItemTd.Text = "<td align ='left' width='40px' style='padding-left: 10px;'>";
                lblStandard.Text = "Standards";
                lblStandard.Width = Unit.Pixel(100);
                ltrlDataItemTrClose.Text = "</td>";

                aoContainer.Controls.Add(ltrlDataItemTd);
                aoContainer.Controls.Add(lblStandard);
                aoContainer.Controls.Add(ltrlDataItemTrClose);

                for (int iNo = 0; iNo < lstWorkingDivisionDetails.Count; iNo++)
                {
                    Literal ltrthOpen = new Literal();
                    ltrthOpen.Text = "<th align='center'>";

                    Literal ltrthDivision = new Literal();
                    ltrthDivision.Text = lstWorkingDivisionDetails[iNo].DivisionName;

                    CheckBox chkAllDivision = new CheckBox();
                    chkAllDivision.ID = "chkAllDivisions_" + lstWorkingDivisionDetails[iNo].DivisionId;
                    chkAllDivision.Attributes.Add("onclick", "CheckHeaderCheckbox(this," + lstWorkingDivisionDetails[iNo].DivisionId + ")");

                    Literal ltrthClose = new Literal();
                    ltrthClose.Text = "</th>";

                    aoContainer.Controls.Add(ltrthOpen);
                    aoContainer.Controls.Add(chkAllDivision);
                    aoContainer.Controls.Add(ltrthDivision);
                    aoContainer.Controls.Add(ltrthClose);
                }

                Literal ltrlHeadertrClose = new Literal();
                ltrlHeadertrClose.Text = "</tr>";

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