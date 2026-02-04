// File Name    : StandardwiseDocumentUI.aspx   
// Created By   : Vinod     
// Created Date : 17 March 2011    
// Description  : This class is used to configure standardwise documents, 
//                which are required at the time of admission of student.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using System.Xml;
using System.Data;
using Utility;
using DocumentEntity;
using System.Xml.Serialization;

public partial class StandardwiseDocumentUI : SchoolBase
{
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
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                DesignSettingAccordingLanguage();
                if (CheckPreCondition())
                {
                    FillStandardCombobox();
                    FillDocumentListView();
                    SetJavaScriptAttributes();
                }
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                DesignSettingAccordingLanguage();
            }
            cmbStandards.Focus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save standardwise document details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            string sIsConfigured = ReadQuerystring();
            if (sIsConfigured != "Y")
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.StandardwiseDocument));
            lblUpdateSucess.Visible = true;
            lblUpdateSucess.Text = Resources.LocalizedResources.DocumentConfigurationSavedSuccessfully;
            FillDocumentListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to move on school configuration screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration)));
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
    protected void lstvwDocumentConfiguration_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("ChkSelect");
            CheckBox chkIsContinue = (CheckBox)e.Item.FindControl("ChkIsContinue");
            CheckBox chkIsAppForExisStud = (CheckBox)e.Item.FindControl("chkIsAppForExisStud");
            CheckBox chkIsSubmitted = (CheckBox)e.Item.FindControl("chkIsSubmitted");

            chkIsAppForExisStud.Attributes["onclick"] = "javascript:SetIsSubmitStatus(this, " + iRowId + " );";
            chkIsSubmitted.Attributes["onclick"] = "javascript:SetIsAppStatus(this, " + iRowId + " );";
            if (lstvwDocumentConfiguration.DataKeys[iRowId]["SchoolId"].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
            {
                chkIsSelected.Checked = true;
                chkIsSelected.Attributes["onclick"] = "javascript:SetIscontinuedSatus(this, " + iRowId + ",false );";
            }
            else
            {
                chkIsSelected.Checked = false;
                chkIsAppForExisStud.Enabled = true;
                chkIsSubmitted.Enabled = true;
                chkIsSelected.Attributes["onclick"] = "javascript:SetIscontinuedSatus(this, " + iRowId + ", true  );";
            }
            chkIsContinue.Checked = Convert.ToBoolean(lstvwDocumentConfiguration.DataKeys[iRowId]["IsContinue"]);
        }
    }

    /// <summary>
    /// This method checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.StandardwiseDocument);

        if (sLinks.Equals(""))
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
        tblTermList.Visible = false;
        trStandard.Visible = false;
        trlblErr.Visible = false;
        trLables.Visible = false;
        btnCancel.Visible = true;
        btnCancel.Text = Resources.LocalizedResources.Back;
        btnSave.Visible = false;
    }

    /// <summary>
    /// This event is used to fill document details in listview with for selected standard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandards_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillDocumentListView();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Events"

    #region "Private Methods"

    /// <summary>
    /// This method is used to fill standard combobox.
    /// </summary>

    private void FillStandardCombobox()
    {
        StandardwiseDocumentMasterBL oStandardwiseDocumentMasterBL = new StandardwiseDocumentMasterBL(miSchoolId, miAcademicYearId);
        DataTable oDTStandards = oStandardwiseDocumentMasterBL.GetAllStandardDetails();

        cmbStandards.Items.Clear();
        cmbStandards.DataSource = oDTStandards;
        cmbStandards.DataTextField = "Standard_Name";
        cmbStandards.DataValueField = "Original_Standard_Id";
        cmbStandards.DataBind();
    }

    /// <summary>
    /// This method is used to fill listview with document details.
    /// </summary>
    private void FillDocumentListView()
    {
        StandardwiseDocumentMasterBL oStandardwiseDocumentMasterBL = new StandardwiseDocumentMasterBL(miSchoolId, miAcademicYearId);

        lstvwDocumentConfiguration.DataSource = oStandardwiseDocumentMasterBL.GetAllDocumentsByStandard(Convert.ToInt32(cmbStandards.SelectedValue));
        lstvwDocumentConfiguration.DataBind();
        hidRowCnt.Value = Convert.ToString(lstvwDocumentConfiguration.Items.Count);
    }

    /// <summary>
    /// This method is used to save all standardwise documents details.
    /// </summary>
    private void Save()
    {
        StandardwiseDocumentMasterBL oStandardwiseDocumentMasterBL = new StandardwiseDocumentMasterBL(miSchoolId, miAcademicYearId);
        oStandardwiseDocumentMasterBL.SaveStandardwiseDocumentDetails(GenerateXml(PopulateDocumentDetails()), miUserId);
    }

    /// <summary>
    /// This method is used to populate document details.
    /// </summary>
    /// <returns></returns>
    private List<StandardwiseDocument> PopulateDocumentDetails()
    {
        List<StandardwiseDocument> lstStandardwiseDocumentInfo = new List<StandardwiseDocument>();
        StandardwiseDocument oStandardwiseDocument = null;

        for (int iRowCount = 0; iRowCount < lstvwDocumentConfiguration.Items.Count; iRowCount++)
        {
            oStandardwiseDocument = new StandardwiseDocument();
            ListViewDataItem oCurrentItem = lstvwDocumentConfiguration.Items[iRowCount] as ListViewDataItem;
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

            CheckBox oChkSelect = lstvwDocumentConfiguration.Items[iRowCount].FindControl("ChkSelect") as CheckBox;
            TextBox otxtDocName = lstvwDocumentConfiguration.Items[iRowCount].FindControl("txtDocumentName") as TextBox;
           TextBox otxtsortorder = lstvwDocumentConfiguration.Items[iRowCount].FindControl("txtsortorder") as TextBox;
            CheckBox oChkIsContinue = lstvwDocumentConfiguration.Items[iRowCount].FindControl("ChkIsContinue") as CheckBox;
            CheckBox ochkIsAppForExisStud = lstvwDocumentConfiguration.Items[iRowCount].FindControl("chkIsAppForExisStud") as CheckBox;
            CheckBox ochkIsSubmitted = lstvwDocumentConfiguration.Items[iRowCount].FindControl("chkIsSubmitted") as CheckBox;

            int iStandardwiseDocumentId = Convert.ToInt32(lstvwDocumentConfiguration.DataKeys[iRowCount]["StandardwiseDocumentId"]);
            int iSchoolId = Convert.ToInt32(lstvwDocumentConfiguration.DataKeys[iRowCount]["SchoolId"]);
            int iStdId = Convert.ToInt32(cmbStandards.SelectedValue);
            int iDocId = Convert.ToInt32(lstvwDocumentConfiguration.DataKeys[iRowCount]["OriginalDocumentId"]);


            if (oChkSelect.Checked || iSchoolId != -9999)
            {
                oStandardwiseDocument.StandardwiseDocumentId = iStandardwiseDocumentId;
                oStandardwiseDocument.DocumentName = otxtDocName.Text;
                oStandardwiseDocument.SortOrder = otxtsortorder.Text.ToInt();
                oStandardwiseDocument.OriginalStandardId = iStdId;
                oStandardwiseDocument.OriginalDocumentId = iDocId;
                oStandardwiseDocument.IsContinue = oChkIsContinue.Checked;
                oStandardwiseDocument.Is_Deleted = Convert.ToInt32(!oChkSelect.Checked);
                oStandardwiseDocument.IsAppForExisStud = ochkIsAppForExisStud.Checked;
                oStandardwiseDocument.IsSubmit = ochkIsSubmitted.Checked;
                lstStandardwiseDocumentInfo.Add(oStandardwiseDocument);
            }
        }
        return lstStandardwiseDocumentInfo;
    }

    /// <summary>
    /// This method is used to decrypt query string.
    /// </summary>
    /// <returns></returns>
    private string ReadQuerystring()
    {
        return QueryString["Is_Configured"];
    }

    /// <summary>
    /// This method is used to set javascript attributes to controls.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave });
        btnSave.Attributes["onclick"] = "javascript:ResetUpdateLbl()";
        btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";
        cmbStandards.Attributes["onchange"] = "javascript:CheckUncheckSelectAllCheckBox();";
    }

    /// <summary>
    /// This method is used to set design according to selected language.
    /// </summary>
    private void DesignSettingAccordingLanguage()
    {
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidDocumentNameShouldNotBeDuplicated.Value = Resources.LocalizedResources.DocumentNameShouldNotBeDuplicated;
        hidAtLeastOneDocumentShouldBeSelected.Value = Resources.LocalizedResources.AtLeastOneDocumentShouldBeSelected;
        hidDocumentNameShouldNotBeBlank.Value = Resources.LocalizedResources.DocumentNameShouldNotBeBlank;
    }

    #endregion "Private Method"
}
