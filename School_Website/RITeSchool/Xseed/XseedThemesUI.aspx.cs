using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Linq;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using XseedReportEntities;
/// <summary>
/// This class is used to cover preprimary themes.
/// </summary>
public partial class XseedThemesUI : SchoolBase
{
    #region "Constants"

    const string S_DEFAULT_SORT_EXP = "Theme";
    const string S_SAVE_MESSGE = "Pre-Primary Progress Report theme details saved successfully !!!";
    const string S_UPDATE_MESSAGE = "Pre-Primary Progress Report theme details updated successfully !!!";
    const string S_SAVE = "Save";
    const string S_UPDATE = "Update";
    
    #endregion
    #region
    
    XseedThemesBL moXseedThemesBL = null;

    #endregion
    #region "Event"
    /// <summary>
    /// This event is used to set javascript attributes for buttons, set default values to controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                SetJavaScriptAttributes();
                FillStandardComboBox();
                FillAssessmentComboBox();
                FillXseedThemeListView();
                hidMode.Value = Constants.S_NEW_MODE;
                hidSortDirection.Value = Constants.S_ASCENDING;
                hidRowNo.Value = Constants.I_ZERO.ToString();
                AddSortImage();
                cmbStandard.Focus();
                ClearFields();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to save Xseed Theme details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            hidRowNo.Value = Constants.I_ZERO.ToString();
            bool bIsConfigured = QueryString[Constants.S_IS_CONFIGURED] == Constants.S_YES;
            if (bIsConfigured)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.Theme));
            FillXseedThemeListView();
          
            if (btnSave.Text == S_SAVE)
                lblUpdateSucess.Text = S_SAVE_MESSGE;
            else
                lblUpdateSucess.Text = S_UPDATE_MESSAGE;
            ClearFields();
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to fill Assessment combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillAssessmentComboBox();
            AddSortImage();
            FillXseedThemeListView();
            ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to edit or delete records.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwThemeDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                hidRowNo.Value = ((Label)oCurrentItem.FindControl("lblSrNo")).Text;
                int iRowIndex = oCurrentItem.DisplayIndex;
                if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnSave.Text = S_UPDATE;
                    lblUpdateSucess.Text = string.Empty;
                    Label lblTheme = e.Item.FindControl("lblTheme") as Label;
                    Label lblSortOrder = e.Item.FindControl("lblSortOrder") as Label;
                    txtSortOrder.Text = lblSortOrder.Text;
                    txtTheme.Text = lblTheme.Text;
                    hidThemeId.Value = lstvwThemeDetails.DataKeys[iRowIndex]["ThemeId"].ToString();
                    hidMode.Value = Constants.S_EDIT_MODE;
                }
                else if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    int iThemeId = Convert.ToInt32(lstvwThemeDetails.DataKeys[iRowIndex]["ThemeId"].ToString());
                    Delete(iThemeId);
                    FillXseedThemeListView();
                    moXseedThemesBL = new XseedThemesBL(miSchoolId, miAcademicYearId);
                    if (moXseedThemesBL.GetCount() == 0)
                        DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.Theme));
                }
            }
            else if (e.CommandName == Constants.S_COMMAND_SORT)
                hidSortExpression.Value = e.CommandArgument.ToString();
            
        }
        catch (Exception ex)
        {

            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This event is used to set default controls and add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
            AddSortImage();
            hidMode.Value = Constants.S_NEW_MODE;
        }
        catch (Exception ex)
        {

            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to set values to listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwThemeDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            Label lblSrNo = oCurrentItem.FindControl("lblSrNo") as Label;
            int iRowId = oCurrentItem.DisplayIndex;
            lblSrNo.Text = (iRowId + Constants.I_ONE).ToString();
            ImageButton imgBtnDelete = oCurrentItem.FindControl("imgBtnDelete") as ImageButton;
            imgBtnDelete.Attributes.Add("onclick", "if(!ConfirmRemove()) {return false;}");
        }
    }
    /// <summary>
    /// This event is used for sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwThemeDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = (e.SortExpression != string.Empty) ? e.SortExpression
                                                : S_DEFAULT_SORT_EXP;
            SetSortVariables();
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbAssessment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillXseedThemeListView();
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion
    #region "Private Methods"
    /// <summary>
    /// This method is used to save Xseed theme details.
    /// </summary>
    private void Save()
    {
        int ThemeId = 0;
        moXseedThemesBL = new XseedThemesBL(miSchoolId, miAcademicYearId);
        if (hidMode.Value == Constants.S_NEW_MODE)
            ThemeId = Constants.I_ZERO;
        else
            ThemeId = Convert.ToInt32(hidThemeId.Value);
        int iStandardwiseAssessmentId = Convert.ToInt32(cmbAssessment.SelectedValue);
        string sTheme = txtTheme.Text.Trim();
        int iSortOrder = Convert.ToInt32(txtSortOrder.Text);
        moXseedThemesBL.Save(iStandardwiseAssessmentId, sTheme, iSortOrder, ThemeId, miUserId);
      
    }
    /// <summary>
    /// This method is used to fill listview.
    /// </summary>
    private void FillXseedThemeListView()
    {
        int iStandardwiseAssessmentId = Convert.ToInt32(cmbAssessment.SelectedValue);
        moXseedThemesBL = new XseedThemesBL(miSchoolId, miAcademicYearId);
        List<XseedTheme> lstXseedTheme = XseedThemesBL.GetAll(hidSortExpression.Value, iStandardwiseAssessmentId, hidSortDirection.Value, miSchoolId);
        lstvwThemeDetails.DataSource = lstXseedTheme;
        lstvwThemeDetails.DataBind();
        hidMode.Value = Constants.S_NEW_MODE;
        trNoRecordFound.Visible = !(lstvwThemeDetails.Items.Count > 0 || cmbAssessment.SelectedIndex == 0);
    }
    /// <summary>
    /// This method is used to fill standard dropdownlist.
    /// </summary>
    private void FillStandardComboBox()
    {
        moXseedThemesBL = new XseedThemesBL(miSchoolId, miAcademicYearId);
        StandardwiseAssessmentMaster oStandardwiseAssessmentMaster = new StandardwiseAssessmentMaster();
        List<StandardwiseAssessmentMaster> lstStandardwiseAssessmentMaster = StandardwiseAssessmentMasterBL.GetStandardAndAssessment(miSchoolId, miAcademicYearId);
        var lstStandardwiseAssessment = lstStandardwiseAssessmentMaster.Select(assessment => new { StandardId = assessment.StandardId, StandardName = assessment.StandardName }).ToList();
        ListSource.FillDropDownList(lstStandardwiseAssessment.Distinct(), cmbStandard, "StandardName", "StandardId", Constants.S_SELECT);
    }
    /// <summary>
    /// This method is used to fill Assessment dropdownlist.
    /// </summary>
    private void FillAssessmentComboBox()
    {
        int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
        StandardwiseAssessmentMasterBL oStandardwiseAssessmentMasterBL = new StandardwiseAssessmentMasterBL(miSchoolId, miAcademicYearId);
        List<StandardwiseAssessmentMaster> lstAssement = oStandardwiseAssessmentMasterBL.GetStandardwiseAssementDetailsList(iStandardId);
        ListSource.FillDropDownList(lstAssement, cmbAssessment, "AssessmentName", "StandardwiseAssessmentId", Constants.S_SELECT);
    }
    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Xseed_Report_Related));
        ApplyMouseHoverEffect(new List<Button> {btnCancel, btnSave,btnBack});
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// This method is used to delete Xseed theme details.
    /// </summary>
    /// <param name="aiThemeId"></param>
    private void Delete(int aiThemeId)
    {
        moXseedThemesBL = new XseedThemesBL(miSchoolId);
        moXseedThemesBL.Delete(aiThemeId);
        ClearFields();
    }
    /// <summary>
    /// This method is used to set default control.
    /// </summary>
    private void ClearFields()
    {
        if (lstvwThemeDetails.Items.Count == 0 && cmbAssessment.SelectedValue != "0")
        {
            cmbStandard.SelectedValue = Constants.I_ZERO.ToString();
            cmbAssessment.SelectedValue = Constants.I_ZERO.ToString();
            trNoRecordFound.Visible = false;
        }
        txtSortOrder.Text = string.Empty;
        txtTheme.Text = string.Empty;
        if (btnSave.Text == S_UPDATE)
            btnSave.Text = S_SAVE;

    }
    #endregion
    /// <summary>
    /// This method is used to set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        FillXseedThemeListView();

    }
    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    private void AddSortImage()
    {
        if (string.IsNullOrEmpty(hidSortExpression.Value))
            hidSortExpression.Value = S_DEFAULT_SORT_EXP;
        HtmlTableRow oHtmlTableHeaderRow = lstvwThemeDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }
    
}
