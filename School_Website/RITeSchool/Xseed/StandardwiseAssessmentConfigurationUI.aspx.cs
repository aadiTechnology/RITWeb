// File Name    : StandardwiseAssessmentConfigurationUI.aspx   
// Created By   : Vinod     
// Created Date : 25 May 2011    
// Description  : This class is used to configure standardwise assessment details.

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using Utility;
using XseedReportEntities;

public partial class StandardwiseAssessmentConfigurationUI : SchoolBase
{
    #region "Members"

   private int miCount = 0;

    #endregion "Members"

    #region "Events"

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
                if (CheckPreCondition())
                {
                    FillStandardCombobox();
                    valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                    cmbStandard.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save standardwise assessment details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveStandardwiseAssessmentDetails();
            bool bIsConfigured = QueryString[Constants.S_IS_CONFIGURED] != Constants.S_YES;
			if (bIsConfigured)
				SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.StandardwiseAssessmentConfiguration));
            lblUpdateSuccess.Visible = true;
            lblUpdateSuccess.Text = "Standardwise Assessment configuration saved successfully !!!";
            FillStandardwiseAssessmentListView();
        }
        catch (SqlException sqlEx)
        {
            FillStandardwiseAssessmentListView();
            lblErrorMsg.Text = sqlEx.Message;
        }
        catch (ReferenceExceptions ex)
        {
            FillStandardwiseAssessmentListView();
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill assessment details in listview with for selected standard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbStandard.SelectedIndex != 0)
            {
                FillStandardwiseAssessmentListView();
                HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwStandardwiseAssessmentConfig.FindControl("trHeader");
                CheckBox chkSelectAll = oHtmlTableRow.FindControl("ChkSelectAll") as CheckBox;
                chkSelectAll.Checked = false;
                SetVisibility(true);
            }
            else
            {
                lstvwStandardwiseAssessmentConfig.DataSource = null;
                lstvwStandardwiseAssessmentConfig.DataBind();
                SetVisibility(false);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set values to listview columns.
    /// </summary> 
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStandardwiseAssessmentConfig_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = oCurrentItem.DisplayIndex;
                Label lblRowNo = oCurrentItem.FindControl("lblRowNo") as Label;
                CheckBox chkSelect = oCurrentItem.FindControl("ChkSelect") as CheckBox;
                RadioButton optIsFinal = oCurrentItem.FindControl("optIsFinal") as RadioButton;
                DropDownList cmbSortOrder = oCurrentItem.FindControl("cmbSortOrder") as DropDownList;
                TextBox txtcalstartDate = oCurrentItem.FindControl("txtcalstartDate") as TextBox;
                if (txtcalstartDate.Text == "01-Jan-1900")
                    txtcalstartDate.Text = string.Empty;                
                TextBox txtcalEndDate = oCurrentItem.FindControl("txtcalEndDate") as TextBox;
                if (txtcalEndDate.Text == "01-Jan-1900")
                    txtcalEndDate.Text = string.Empty;
                optIsFinal.Attributes.Add("onclick", "CheckUncheckRadioBtn(this, " + iRowId + ");");
                chkSelect.Attributes.Add("onclick", "javascript:SetControlEnability(this, " + iRowId + " );");
                lblRowNo.Text = (iRowId + 1).ToString();
                cmbSortOrder.Items.Add(new ListItem(Constants.S_SELECT, Constants.I_ZERO.ToString()));
                StandardwiseAssessmentMaster oStandardwiseAssessmentMaster = oCurrentItem.DataItem as StandardwiseAssessmentMaster;
                for (int iRowCount = 1; iRowCount <= miCount; iRowCount++)
                    cmbSortOrder.Items.Add(new ListItem(iRowCount.ToString(), iRowCount.ToString()));
                cmbSortOrder.SelectedValue = lstvwStandardwiseAssessmentConfig.DataKeys[iRowId]["SortOrder"].ToString(); 
				if (oStandardwiseAssessmentMaster.IsDeleted == "N")
					chkSelect.Checked = true;
				else
				{
					chkSelect.Checked = false;
					cmbSortOrder.Enabled = false;
                    txtcalstartDate.Text = string.Empty;
                    txtcalEndDate.Text = string.Empty;
					optIsFinal.InputAttributes.Add("disabled", "disabled");                     
				}

                if (oStandardwiseAssessmentMaster.IsFinalAssessment == Constants.I_ONE)
                    optIsFinal.Checked = true;
                else
                    optIsFinal.Checked = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set visibility of control after fill liste view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStandardwiseAssessmentConfig_DataBound(object sender, EventArgs e)
    {
        try
        {
            hidRowCount.Value = lstvwStandardwiseAssessmentConfig.Items.Count.ToString();
            if (lstvwStandardwiseAssessmentConfig.Items.Count > 0)
                SetVisibility(true);
            else
                SetVisibility(false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Events"

    #region "Private Methods"

    /// <summary>
    /// This method is used to fill Standardwise Assessment details in list view.
    /// </summary>
    private void FillStandardwiseAssessmentListView()
    {
        StandardwiseAssessmentMasterBL oStandardwiseAssessmentMasterBL = new StandardwiseAssessmentMasterBL(miSchoolId, miAcademicYearId);
        SchoolwiseAcademicYrDates oSchoolwiseAcademicYrDates = new SchoolwiseAcademicYrDates();

        oStandardwiseAssessmentMasterBL.GetStandardwiseAssessmentDetails(Convert.ToInt32(cmbStandard.SelectedValue));

        hidStdStartDate.Value = oSchoolwiseAcademicYrDates.AcademicYearStartDate.ToString("dd-MMM-yyyy");
        hidStdEndDate.Value = oSchoolwiseAcademicYrDates.AcademicYearEndDate.ToString("dd-MMM-yyyy");

        miCount = oStandardwiseAssessmentMasterBL.lstStandardwiseAssessmentDetails.Count;

        lstvwStandardwiseAssessmentConfig.DataSource = oStandardwiseAssessmentMasterBL.lstStandardwiseAssessmentDetails;
        lstvwStandardwiseAssessmentConfig.DataBind();
    }

    /// <summary>
    /// This method is used to fill standard combobox.
    /// </summary>
    private void FillStandardCombobox()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId,miAcademicYearId);
        List<StandardMaster> lstStandards = oStandardCollectionBL.GetStandardsForStandardwiseAssessment();
	    ListSource.FillDropDownList(lstStandards, cmbStandard, "StandardName", "StandardId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to save standardwise assessment details.
    /// </summary>
    private void SaveStandardwiseAssessmentDetails()
    {
        StandardwiseAssessmentMasterBL oStandardwiseAssessmentMasterBL = new StandardwiseAssessmentMasterBL(miSchoolId, miAcademicYearId);
        List<StandardwiseAssessmentMaster> lstNotConfigStandardwiseAssessmentMaster = PopulateNotConfigureStdAssessmentDetails();
        List<StandardwiseAssessmentMaster> lstStandardwiseAssessmentMaster = PopulateStdAssessmentDetails();
        string sMessage = CheckDependencies(lstNotConfigStandardwiseAssessmentMaster, miAcademicYearId);

        if (string.IsNullOrEmpty(sMessage))
            oStandardwiseAssessmentMasterBL.SaveStandardwiseAssessmentDetails(GetStandarwiseAssessmentXML(lstStandardwiseAssessmentMaster), miUserId, Convert.ToInt32(cmbStandard.SelectedValue));
        else
            throw new ReferenceExceptions(sMessage);
    }

    /// <summary>
    /// This method is used to check dependencies.
    /// </summary>
    /// <param name="lstSubjectSectionConfigurationMaster"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <returns></returns>
    private string CheckDependencies(List<StandardwiseAssessmentMaster> lstStandardwiseAssessmentMaster, int aiAcademicYearId)
    {
        GenericReferenceList<StandardwiseAssessmentMaster> objStdRefereces = new GenericReferenceList<StandardwiseAssessmentMaster>(lstStandardwiseAssessmentMaster, aiAcademicYearId);
        return objStdRefereces.CheckDependenciesForList("StandardwiseAssessmentId", "AssessmentName", "Action", Constants.ReferenceId.StandardwiseAssessmentConfig, false);
    }

    /// <summary>
    /// This method is used to populate standardwise assessment details.
    /// </summary>
    /// <returns></returns>
    private List<StandardwiseAssessmentMaster> PopulateNotConfigureStdAssessmentDetails()
    {
        List<StandardwiseAssessmentMaster> lstStandardwiseAssessmentDetails = new List<StandardwiseAssessmentMaster>();
        StandardwiseAssessmentMaster oStandardwiseAssessmentDetails = null;
        for (int iRowCount = 0; iRowCount < lstvwStandardwiseAssessmentConfig.Items.Count; iRowCount++)
        {
            oStandardwiseAssessmentDetails = new StandardwiseAssessmentMaster();
            ListViewDataItem oCurrentItem = lstvwStandardwiseAssessmentConfig.Items[iRowCount] as ListViewDataItem;
            CheckBox chkSelect = lstvwStandardwiseAssessmentConfig.Items[iRowCount].FindControl("ChkSelect") as CheckBox;
	        Label lblAssessmentName = oCurrentItem.FindControl("lblAssessmentName") as Label;
			if (!chkSelect.Checked && lstvwStandardwiseAssessmentConfig.DataKeys[iRowCount]["IsDeleted"].ToString() == "N")
			{
				oStandardwiseAssessmentDetails.StandardwiseAssessmentId = Convert.ToInt32(lstvwStandardwiseAssessmentConfig.DataKeys[iRowCount]["StandardwiseAssessmentId"].ToString());
				oStandardwiseAssessmentDetails.AssessmentName = lblAssessmentName.Text;
				oStandardwiseAssessmentDetails.Action = Constants.Action.Delete;
				lstStandardwiseAssessmentDetails.Add(oStandardwiseAssessmentDetails);
			}
        }

        return lstStandardwiseAssessmentDetails;
    }

    /// <summary>
    /// This method is used to populate standardwise assessment details.
    /// </summary>
    /// <returns></returns>
    private List<StandardwiseAssessmentMaster> PopulateStdAssessmentDetails()
    {
        List<StandardwiseAssessmentMaster> lstStandardwiseAssessmentDetails = new List<StandardwiseAssessmentMaster>();
        StandardwiseAssessmentMaster oStandardwiseAssessmentDetails = null;
        for (int iRowCount = 0; iRowCount < lstvwStandardwiseAssessmentConfig.Items.Count; iRowCount++)
        {
            oStandardwiseAssessmentDetails = new StandardwiseAssessmentMaster();
            ListViewDataItem oCurrentItem = lstvwStandardwiseAssessmentConfig.Items[iRowCount] as ListViewDataItem;
            CheckBox chkSelect = lstvwStandardwiseAssessmentConfig.Items[iRowCount].FindControl("ChkSelect") as CheckBox;
            RadioButton optIsFinalAssessment = oCurrentItem.FindControl("optIsFinal") as RadioButton;
            DropDownList cmbSortOrder = oCurrentItem.FindControl("cmbSortOrder") as DropDownList;
            TextBox txtStartDate = oCurrentItem.FindControl("txtcalstartDate") as TextBox;
            TextBox txtEndDate = oCurrentItem.FindControl("txtcalEndDate") as TextBox;
            if (chkSelect.Checked)
            {
                oStandardwiseAssessmentDetails.StandardwiseAssessmentId = Convert.ToInt32(lstvwStandardwiseAssessmentConfig.DataKeys[iRowCount]["StandardwiseAssessmentId"].ToString());
                oStandardwiseAssessmentDetails.AssessmentId = Convert.ToInt32(lstvwStandardwiseAssessmentConfig.DataKeys[iRowCount]["AssessmentId"].ToString());
                oStandardwiseAssessmentDetails.StandardId = Convert.ToInt32(cmbStandard.SelectedValue);
                oStandardwiseAssessmentDetails.IsFinalAssessment = Convert.ToInt32(optIsFinalAssessment.Checked);
                oStandardwiseAssessmentDetails.SortOrder = Convert.ToInt32(cmbSortOrder.SelectedValue);
                oStandardwiseAssessmentDetails.Action = chkSelect.Checked ? Constants.Action.Delete : Constants.Action.Insert;
                oStandardwiseAssessmentDetails.StartDate = txtStartDate.Text.ToDateTime();
                oStandardwiseAssessmentDetails.EndDate = txtEndDate.Text.ToDateTime();
                lstStandardwiseAssessmentDetails.Add(oStandardwiseAssessmentDetails);
            }
        }

        return lstStandardwiseAssessmentDetails;
    }

    /// <summary>
    /// This method is used to generate standardwise assessment XML.
    /// </summary>
    /// <param name="lstStandardwiseAssessmentDetails"></param>
    /// <returns></returns>
    private string GetStandarwiseAssessmentXML(List<StandardwiseAssessmentMaster> alstStandardwiseAssessmentDetails)
    {
        StringWriter sw = new StringWriter();
        new XmlSerializer(alstStandardwiseAssessmentDetails.GetType()).Serialize(sw, alstStandardwiseAssessmentDetails);
        string sXML = sw.ToString();
        sXML = sXML.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
        return sXML;
    }

    /// <summary>
    /// This method is used to set javascript attributes to controls.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> {btnCancel, btnSave});
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Xseed_Report_Related));
    }

    /// <summary>
    /// This method checks the preconditons of Configured Assessment for standard.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.StandardwiseAssessmentConfiguration);

        if (sLinks.Equals(""))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            btnSave.Visible = false;
            tblMainAssessmentTable.Visible = false;
        }

        return bReturn;
    }

    /// <summary>
    /// This method is used to set default visibility of contols.
    /// </summary>
    /// <param name="abFlab"></param>
    private void SetVisibility(bool abFlab)
    {
        btnSave.Visible = abFlab;
        divContainer.Visible = abFlab;
        trNoRecordMsg.Visible = !abFlab;
    }

    #endregion "Private Method"
}
