// File Name  : PrePrimaryProgressReportSubSubjectsConfigList.aspx.cs
// Created By : Milind
// Description :This class provided preprimary progress report configuration.

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using ProgressReportEntities;
using Utility;


public partial class PrePrimaryProgressReportSubSubjectsConfigList : SchoolBase
{
    #region Constants

    const string S_DEFUALT_SORT_EXPR = "ModuleName";

    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                cmbStandard.Focus();
                SetDefaultProperties();
                FillComboxes();
                HideVisibleControls(false);
            }
            SetJavaScriptAttributes();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
   
    protected void cmbModuleName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillSubjectCombobox();
            lblSuccess.Visible = false;
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbStandard.SelectedValue != "0")
            {
                DtPgCount.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, false);
                HideVisibleControls(true);
                FillSubSubjects();
                cmbSubjectName.Enabled = false;
            }
            else
            {
                HideVisibleControls(false);
                divSortOrder.Visible = false;
            }
                btnSave.Text = "Save";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void FillSubSubjects()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId,miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetConfiguredPrePrimaryStandards();
        DataRow[] oDataRow = oDtStandardCollection.Select(Constants.S_STANDARD_ID_FIELD + "<>" + cmbStandard.SelectedValue, " original_standard_id ");
        ControlUtility.FillDropDownList(oDataRow, ref cmbCopyStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT);
        lstvwSubject.DataSourceID = lstDSobj.ID;
    }

    protected void lstvwSubject_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwSubject.Items.Count > 0)
            {
                ControlUtility.FillListViewPagerFooter(lstvwSubject, DtPgCount);
                AddSortImage();
                divSortOrder.Visible = true;
                tbllstSub.Visible = true;
                if (DtPgCount.TotalRowCount > DtPgCount.PageSize)
                {
                    trDtPgr.Visible = true;
                    DtPgCount.Visible = true;
                }
                else
                {
                    trDtPgr.Visible = false;
                    DtPgCount.Visible = false;
                }
                string sQueryString = "StandardId=" + cmbStandard.SelectedValue;
                string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
                hlnkSortOrder.Attributes.Add("onclick", "window.open('" + hlnkSortOrder.NavigateUrl + "?" + sEncrypt
                                                     + "' , '_blank','scrollbars=yes,resizable=yes,top=0,left=0,width=950,height=600'); return false;");
            }
            else
            {
                DtPgCount.Visible = false;
                divSortOrder.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwSubject_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton imgbtnDelete = e.Item.FindControl("imgbtnDeleteReq") as ImageButton;
                imgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwSubject_Sorting(object sender, ListViewSortEventArgs e)
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

    protected void lstvwSubject_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Remove")
            {
                int iSubSubjectId = Convert.ToInt32(((ImageButton)(e.CommandSource)).CommandArgument);
                PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
                if (!oPrePrimaryProgressSheetConfigBL.Dependent(iSubSubjectId, miAcademicYearId,miSchoolId))
                {
                    PrePrimaryProgressSheetConfigBL.DeleteSubSubject(iSubSubjectId);

                    lstvwSubject.DataSourceID = lstDSobj.ID;
                    lstvwSubject.DataBind();
                    HideVisibleControls(true);
                    btnSave.Text = "Save";
                    if (PrePrimaryProgressSheetConfigBL.CountAllConfiguredPrePrimarySubSubjects(miSchoolId, miAcademicYearId, Convert.ToInt32(cmbStandard.SelectedValue)) <= 0)
                        DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.PrePrimarySubSubjectsConfiguration));
                }
                else
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "Skills / Behaviour name can not be deleted since grades are already assigned to pre-primary students.";
                }
                

            }
            else if (e.CommandName == "Modify")
            {
                int iSubSubjectId = Convert.ToInt32(((ImageButton)(e.CommandSource)).CommandArgument);
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

                int iSubjectID = Convert.ToInt32(lstvwSubject.DataKeys[iRowId]["SubjectID"]);
                int iModuleID = Convert.ToInt32(lstvwSubject.DataKeys[iRowId]["ModuleID"]);
                cmbModuleName.SelectedValue = iModuleID.ToString();
                FillSubjectCombobox();
                cmbSubjectName.SelectedValue = iSubjectID.ToString();
                txtSubjectName.Text = (lstvwSubject.DataKeys[iRowId]["SubSubjectName"]).ToString();
                hidSubSubjectId.Value = iSubSubjectId.ToString();
                AddSortImage();
                btnSave.Text = "Update";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill the list view according to the selected pageindex in the combo box. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwSubject);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        try
        {
            int iSourceStdId = Convert.ToInt32(cmbStandard.SelectedValue);
            int iTargetStdId = Convert.ToInt32(cmbCopyStandard.SelectedValue);
            
            PrePrimaryProgressSheetConfigBL.CopyPrePrimaryConfiguration(miSchoolId, miAcademicYearId, iSourceStdId, iTargetStdId);
            FillSubSubjects();
            lblSuccess.Visible = true;
            lblSuccess.Text = "Preprimary Skills and Behaviour has been copied successfully !!!";
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            lblErrorMsg.Text = ex.Message;
            lblErrorMsg.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
                PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
                oPrePrimaryProgressSheetConfigBL.PrePrimaryProgressReportSubSubjectsEntity = PopulatePrePrimaryProgressSheetConfigBL();
                bool isDuplicate = false;

                if (hidSubSubjectId.Value != string.Empty)
                {
                    if (!oPrePrimaryProgressSheetConfigBL.IsDuplicateSubSubjectName(Convert.ToInt32(hidSubSubjectId.Value)))
                        oPrePrimaryProgressSheetConfigBL.UpdateSubSubject(Convert.ToInt32(hidSubSubjectId.Value));
                    else
                        isDuplicate = true;
                }
                else
                {
                    if (!oPrePrimaryProgressSheetConfigBL.GetCinfigStudCntForStd())
                    {
                        if (!oPrePrimaryProgressSheetConfigBL.IsDuplicateSubSubjectName())
                            oPrePrimaryProgressSheetConfigBL.SaveSubSubject();
                        else
                            isDuplicate = true;
                    }
                    else
                    {
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.Text = "Skills / Behaviour name can not be added since grades are already assigned to pre-primary students.";
                    }
                }
                if (lblErrorMsg.Text == string.Empty)
                {
                    if (!isDuplicate)
                    {
                        HideVisibleControls(true);
                        btnSave.Text = "Save";
                        lstvwSubject.DataSourceID = lstDSobj.ID;
                        lstvwSubject.DataBind();
                        hidSubSubjectId.Value = string.Empty;
                    }
                    else
                        lblErrorMsg.Text = "Skills / Behaviour name should not be duplicated.";
                }
            string sIsConfig = ReadQuerystring();
            if (sIsConfig != "Y")
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.PrePrimarySubSubjectsConfiguration));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void SetJavaScriptAttributes()
    {
        btnCopy.Attributes.Add("Onclick", "if(!(ConfirmCopyAction())){return false;}");        
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBack, btnCopy });
    }

    private string ReadQuerystring()
    {
        return QueryString["Is_Configured"];
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            HideVisibleControls(true);
            btnSave.Text = "Save";
            hidSubSubjectId.Value = string.Empty;
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

    ///<Summary>
    ///This method is used to set default properties to controls.
    ///</Summary>
    private void SetDefaultProperties()
    {
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related));
        ValSummaryErrMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ValSummaryCopy.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidSortExpression.Value = lstvwSubject.SortExpression.ToString();
        hidSortDirection.Value = Constants.S_ASCENDING;
        hidSubSubjectId.Value = string.Empty;
        divSortOrder.Visible = false;
    }

    /// <summary>
    /// This function fills combobox with standards
    /// </summary>
    private void FillComboxes()
    {
        PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
        oPrePrimaryProgressSheetConfigBL.GetPrePrimaryStandardsAndModuleName(miSchoolId,miAcademicYearId);

        cmbStandard.Items.Add(new ListItem(Constants.S_SELECT, "0"));
        oPrePrimaryProgressSheetConfigBL.LstPrePrimaryStandards.ForEach(standard => cmbStandard.Items.Add(new ListItem(standard.StandardName, standard.StandardID.ToString())));

        cmbModuleName.Items.Add(new ListItem(Constants.S_SELECT, "0"));
        oPrePrimaryProgressSheetConfigBL.LstPrePrimaryModule.ForEach(module => cmbModuleName.Items.Add(new ListItem(module.ModuleName, module.ModuleID.ToString())));
        cmbSubjectName.Enabled = false;
    }

    private void FillSubjectCombobox()
    {
        int iModuleId = Convert.ToInt32(Convert.ToInt32(cmbModuleName.SelectedValue));
        cmbSubjectName.Items.Clear();
        PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
        int IsSubjectApplicable = oPrePrimaryProgressSheetConfigBL.IsSubjectApplicable(iModuleId);
        hidIsSubjectApplicable.Value = IsSubjectApplicable.ToString();
        if (IsSubjectApplicable == 0)
        {
            lblSubjectMandMark.Visible = false;
            cmbSubjectName.Enabled = false;
        }
        else
        {
            lblSubjectMandMark.Visible = true;
            cmbSubjectName.Enabled = true;
        }
        oPrePrimaryProgressSheetConfigBL.GetPrePrimaryProgressReportSubjects(miSchoolId, miAcademicYearId, iModuleId);
        cmbSubjectName.Items.Add(new ListItem(Constants.S_SELECT, "0"));
        oPrePrimaryProgressSheetConfigBL.LstPrePrimarySubjects.ForEach(subject => cmbSubjectName.Items.Add(new ListItem(subject.PrePrimaryProgressReportSubjectName, subject.PrePrimaryProgressReportSubjectID.ToString())));
    }

    /// <summary>
    /// This method is used to set sorting image in list view column header.
    /// </summary>
    private void AddSortImage()
    {
        if (lstvwSubject.SortDirection.ToString() == "Ascending" || lstvwSubject.SortDirection.ToString() == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
        if (lstvwSubject.SortExpression != string.Empty)
            hidSortExpression.Value = lstvwSubject.SortExpression.ToString();
        else
            hidSortExpression.Value = S_DEFUALT_SORT_EXPR;
        HtmlTableRow oHtmlTableHeaderRow = lstvwSubject.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    private PrePrimaryProgressReportSubSubjects PopulatePrePrimaryProgressSheetConfigBL()
    {
        PrePrimaryProgressReportSubSubjects oPrePrimaryProgressReportSubSubjects = new PrePrimaryProgressReportSubSubjects
        {
            SubSubjectName = txtSubjectName.Text.Trim(),
            SubjectID = Convert.ToInt32(cmbSubjectName.SelectedValue),
            ModuleID = Convert.ToInt32(cmbModuleName.SelectedValue),
            StandardID = Convert.ToInt32(cmbStandard.SelectedValue),
            AcademicYearId = miAcademicYearId,
            SchoolId = miSchoolId,
            InsertedById = miUserId,
            UpdatedById = miUserId
        };
        return oPrePrimaryProgressReportSubSubjects;
    }

    private void HideVisibleControls(bool abFlag)
    {
        tblCopy.Visible = abFlag;
        tbllstSub.Visible = abFlag;
        tblSave.Visible = abFlag;
        txtSubjectName.Text = string.Empty;
        cmbSubjectName.Items.Clear();
        cmbSubjectName.Items.Add(new ListItem(Constants.S_SELECT, "0"));
        cmbSubjectName.SelectedValue = "0";
        cmbModuleName.SelectedValue = "0";
        lblErrorMsg.Text = string.Empty;
    }

    #endregion
}
