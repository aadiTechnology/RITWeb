// File Name  : RemarkTemplateConfigUI.aspx.cs
// Created By : Pravin
// Date       : 30 Mar 12
// Description: This class gives the facility to add template for a perticular remark

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Resources;
using System.Data;
using System.Threading;
/// <summary>
/// This class gives the facility to add template for a perticular remark
/// </summary>
public partial class RemarkTemplateConfigUI :ExportDataTable
{
    private ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
    #region -- CONSTANT(s) --  
    const string S_DEFAULT_SORT_EXP = "TemplateId";    

    #endregion -- CONSTANT(s) --

    #region --Events--

    /// <summary>
    /// this method is called upon pageload
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {        
            if (!IsPostBack)
            {
                hidSave.Value = "Save";
                if(Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                DesignSettingAccordingLanguage();
                InitializeValues();
                FillRemarksCombo();
                FillGradsCombo();
                if (CheckPreCondition())
                    DisplayTemplateRemarks();
                SetJavaScriptAttributes();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                DesignSettingAccordingLanguage();
               	
            }
            FillTemplateKeywords();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save template details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            RemarkTemplateBL oTemplateConfigurationBL = new RemarkTemplateBL();
            RemarkTemplateConfig oRemarkTemplateConfig = new RemarkTemplateConfig
            {
                Template = txtRemarkTemplate.Text.Trim(),
                RemarkId = Convert.ToInt32(cmbCategory.SelectedValue),
                InsertedById = miUserId,                
                SchoolId = miSchoolId,
                TemplateId = Convert.ToInt32(hidRemarkTemplateId.Value),
                OriginalConfigId = Convert.ToInt32(cmbGrades.SelectedValue)
            };
            bool bDuplicate = oTemplateConfigurationBL.IsDuplicate(oRemarkTemplateConfig);
            if (!bDuplicate)
            {
                oTemplateConfigurationBL.Save(oRemarkTemplateConfig);
                if (Convert.ToInt32(hidRemarkTemplateId.Value) != 0)
                    SetMessage(Resources.LocalizedResources.RemarkTemplateUpdatedSuccessfully, false);
                else
                    SetMessage(Resources.LocalizedResources.RemarkTemplateSavedSuccessfully, false);
                ClearControls();
                hidRemarkTemplateId.Value = Constants.S_ZERO;
            }
            else
            {
                SetMessage(Resources.LocalizedResources.RemarkTemplateAlreadyExists, true);
            }            
            DisplayTemplateRemarks();
            if (ReadQueryString() != Constants.S_YES)
                SaveConfigDetails(Constants.SchoolConfigurations.RemarkTemplate.ToInt());
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to check if precondition of remarks is exixt or not
    /// </summary>
    /// <returns></returns>
    protected string ReadQueryString()
    {
        return QueryString["Is_Configured"];
    }

    /// <summary>
    /// This event is used to search templates on Remark and Remark
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            SearchRemarkTemplate();
            lstvwTemplates.Focus();
            if (lstvwTemplates.Items.Count == 0)
                trItemCount.Visible = false;
            else
                trItemCount.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle commands for listview buttons
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTemplates_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iTemplateId = Convert.ToInt32(lstvwTemplates.DataKeys[oCurrentItem.DisplayIndex]["TemplateId"]);                            
               
                switch (e.CommandName)
                {
                    case Constants.S_COMMAND_UPDATE:                        
                        hidRemarkTemplateId.Value = lstvwTemplates.DataKeys[oCurrentItem.DisplayIndex]["TemplateId"].ToString(); 
                        LoadTemplateDetails(iTemplateId);
                        DisplayTemplateRemarks();
                        break;
                    case Constants.S_COMMAND_REMOVE:
                        RemarkTemplateBL oTemplateConfigurationBL = new RemarkTemplateBL();
                        RemarkTemplateConfig oRemarkTemplateConfig= new RemarkTemplateConfig { TemplateId = iTemplateId, UpdatedById = miUserId };
                        oTemplateConfigurationBL.Delete(oRemarkTemplateConfig);                        
                        SetMessage(Resources.LocalizedResources.RemarkTemplateDeletedSuccessfully, false);
                        hidRemarkTemplateId.Value = Constants.S_ZERO;
                        ClearControls();
                        
                        int icurrentPage = (DtPgCount.StartRowIndex / DtPgCount.PageSize) + 1;
                        if (lstvwTemplates.Items.Count == 1 && icurrentPage > 1)
                        {
                            DataPager oDtPgDropDown = lstvwTemplates.FindControl("DtPgDropDown") as DataPager;
                            DropDownList ocmbPageCount = (oDtPgDropDown.Controls[0].FindControl("ddlCnt")) as DropDownList;
                            if (ocmbPageCount != null && ocmbPageCount.Items.Count > 0)
                            {
                                ocmbPageCount.SelectedValue = (Convert.ToInt32(ocmbPageCount.SelectedValue) - 1).ToString();
                                ddlCnt_SelectedIndexChanged(null, new EventArgs());
                            }                            
                        }
                        DisplayTemplateRemarks();
                        break;
                }      
                
            }
            lblErrorMsg.Text = string.Empty;
        }       
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill listview pager footer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTemplates_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwTemplates.Items.Count >0)
            {
                ControlUtility.FillListViewPagerFooterWithCulture(lstvwTemplates, DtPgCount, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst); ;
                DtPgCount.Visible = DtPgCount .TotalRowCount> Constants.I_GRID_PAGE_COUNT;               
            }
            else
                DtPgCount.Visible = false;
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to sort listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTemplates_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
			hidSortExpression.Value = e.SortExpression;
            SetSortDirection();            
            DataPager oDtPgDropDown = lstvwTemplates.FindControl("DtPgDropDown") as DataPager;
            DropDownList ocmbPageCount = (oDtPgDropDown.Controls[0].FindControl("ddlCnt")) as DropDownList;
            if (ocmbPageCount != null && ocmbPageCount.Items.Count > 0)
            {
                ocmbPageCount.SelectedIndex = 0;
                ddlCnt_SelectedIndexChanged(null, new EventArgs());
            }
            else
                DisplayTemplateRemarks();
            AddSortImage();
		}
		catch (Exception ex)
		{
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
    }

    /// <summary>
    /// this event is used to call bind data to image button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTemplates_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event clears controls on button cancel click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            hidRemarkTemplateId.Value = Constants.S_ZERO;
            ClearControls();
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for pagination
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNoAndCulture(lstvwTemplates, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);
            DisplayTemplateRemarks();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }



    /// <summary>
    /// This event is used to Export the data on Excel sheet.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            int iSrNo = 1;
            RemarkTemplateBL oTemplateConfigurationBL = new RemarkTemplateBL();
            DataTable oDataTable = new DataTable();
            oDataTable.Columns.Add("Sr No", typeof(int));
            oDataTable.Columns.Add("Remark Category", typeof(string));
            oDataTable.Columns.Add("Remark Template", typeof(string));
            List<RemarkTemplateConfig> lstRemarkTemplates = oTemplateConfigurationBL.GetAll(miSchoolId, 0, hidSortExpression.Value, hidSortDirection.Value, txtSearch.Text.Trim(), miAcademicYearId,0,0);
            foreach (RemarkTemplateConfig oRemarkTemplateConfig in lstRemarkTemplates)
            {
                oDataTable.Rows.Add(iSrNo, oRemarkTemplateConfig.Name, oRemarkTemplateConfig.Template);
                iSrNo++;
            }
            ExportToExcel("RemarkTemplateConfiguration.xls", oDataTable);
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion --Events--

    #region --Private Methos--

    /// <summary>
    ///This method is used to fill the notes with appropriate rows and columns.
    /// </summary>
    private void FillTemplateKeywords()
    {
        List<RemarkTemplateKeyword> olstRemarkTemplateKeywords = RemarksConfigurationBL.GetTemplateNotes();

        HtmlTableRow oHtmlTableRow=new HtmlTableRow();
        HtmlTableCell oHtmlTableCell;
        HyperLink hlnkKeyword;
        int iCount = 1;
        olstRemarkTemplateKeywords.ForEach(keyword =>
            {
                oHtmlTableCell = new HtmlTableCell { Align="left",Width="200px"};

                hlnkKeyword = new HyperLink { Text = keyword.Keyword, ToolTip = keyword.Description + ' ' + "<br>E.g. " + keyword.Example, EnableViewState = true, CssClass = "clsLabel class1", NavigateUrl = "#" };
                hlnkKeyword.Attributes.Add("onclick", "SetText('" + keyword.Keyword + "')");
                hlnkKeyword.Font.Underline = false;
                oHtmlTableCell.Controls.Add(hlnkKeyword);
                oHtmlTableCell.Attributes.Add("class","ClsBorderlight");

                oHtmlTableRow.Cells.Add(oHtmlTableCell);
                
                if (iCount != Constants.I_ZERO && iCount % Constants.I_TWO == Constants.I_ZERO)
                {
                    tblNotes.Rows.Add(oHtmlTableRow);
                    oHtmlTableRow = new HtmlTableRow();
                }
                iCount++;
            });   
    }

    /// <summary>
    /// This method is used to fill remarks combo
    /// </summary>
    private void FillRemarksCombo()
    {
        try
        {
            List<RemarksCategory> olstRemarkTemplateConfig = RemarksCategoryBL.GetConfig(miSchoolId, miAcademicYearId);
            ListSource.FillDropDownList(olstRemarkTemplateConfig, cmbCategory, "Name", "Id", Constants.S_SELECT);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void FillGradsCombo()
    {
        try
        {
            DataTable oDtGrades = RemarksCategoryBL.GetGrades(miSchoolId, miAcademicYearId);
            cmbGrades.Bind(oDtGrades, "Original_Config_Id", "Grade_Name", Constants.S_SELECT);
        }
        catch (Exception ex)
        {

            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to Set the sort direction of a listview.
    /// </summary>
    private void SetSortDirection()
    {
        if (string.IsNullOrEmpty(hidSortDirection.Value) || hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method sets the java script attributes
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnSearch, btnClose, btnExport });    
        btnClose.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related));
        HtmlForm oform = (HtmlForm)this.Master.FindControl("form1");
        oform.DefaultButton = btnSearch.UniqueID;
    }
    
    /// <summary>
    /// This method is used to initialize values used throught the project.
    /// </summary>
    private void InitializeValues()
    {
        hidTemplateLength.Value = Settings.RemarkLength.ToString();
        cmbCategory.Focus();
        hidSortDirection.Value = Constants.S_ASCENDING;
        hidSortExpression.Value = "Name";
        DtPgCount.PageSize = Constants.I_GRID_PAGE_COUNT;
    }
    /// <summary>
    /// This method is used to load template details in textboxes
    /// </summary>
    /// <param name="aiTemplateConfigurationId"></param>
    private void LoadTemplateDetails(int aiTemplateConfigurationId)
    {
        RemarkTemplateBL oTemplateConfigurationBL = new RemarkTemplateBL();
        RemarkTemplateConfig oRemarkTemplateConfig = oTemplateConfigurationBL.Get(miSchoolId, aiTemplateConfigurationId);
        cmbCategory.SelectedValue = oRemarkTemplateConfig.RemarkId.ToString();
        txtRemarkTemplate.Text = oRemarkTemplateConfig.Template;
        cmbGrades.SelectedValue = oRemarkTemplateConfig.OriginalConfigId.ToString();
        btnSave.Text = Resources.LocalizedResources.Update;
        hidSave.Value = "Update";
    }

    /// <summary>
    /// This method is used to display message after action
    /// </summary>
    /// <param name="asMessage"></param>
    /// <param name="IsError"></param>
    private void SetMessage(string asMessage, bool abIsErrorMessage)
    {
        lblUpdateMessage.Text = asMessage;
        lblUpdateMessage.Font.Bold = true;
        if (abIsErrorMessage)                
            lblUpdateMessage.ForeColor = Color.Red;       
        else
            lblUpdateMessage.ForeColor = Color.Blue;
    }

   /// <summary>
   /// This methhod is used to load tempalte detaails in listview
   /// </summary>
    private void DisplayTemplateRemarks()
    {
        RemarkTemplateBL oTemplateConfigurationBL = new RemarkTemplateBL();
        lstvwTemplates.DataSource = oTemplateConfigurationBL.GetAll(miSchoolId, 0, hidSortExpression.Value, hidSortDirection.Value, txtSearch.Text.Trim(),miAcademicYearId,0,0);
        lstvwTemplates.DataBind();
        if (lstvwTemplates.Items.Count == 0)
        {
            trItemCount.Visible = false;
            DeleteConfigDetails(Constants.SchoolConfigurations.RemarkTemplate.ToInt());
        }
        else
            trItemCount.Visible = true;
    }

    /// <summary>
    /// This function checks the preconditons of RemarkTemplates.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.RemarkTemplate);

        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
            upnlListView.Visible = true;
        }
        else
        {
            trPrecondition.Visible = true;
            divErr.InnerHtml = sLinks;
            upnlListView.Visible = false;
        }

        return bReturn;
    }
    /// <summary>
    /// This method is used to clear controls
    /// </summary>
    private void ClearControls()
    {
        btnSave.Text = Resources.LocalizedResources.Save;
        hidSave.Value = "Save";
        cmbCategory.ClearSelection();
        txtRemarkTemplate.Text = string.Empty;
        cmbGrades.ClearSelection();
    }

    /// <summary>
    /// This method is used to search the remark templates and remarks
    /// </summary>
    private void SearchRemarkTemplate()
    {
        RemarkTemplateBL oTemplateConfigurationBL = new RemarkTemplateBL();
        lstvwTemplates.DataSource = oTemplateConfigurationBL.GetAll(miSchoolId, Constants.I_ZERO, String.Empty, String.Empty, txtSearch.Text.Trim(),miAcademicYearId,0,0);
        lstvwTemplates.DataBind();
    }

    /// <summary>
    /// This method is used to add image for sorted column.
    /// </summary>
    private void AddSortImage()
    {
        HtmlTableRow oHtmlTableHeaderRow = lstvwTemplates.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            AddImageToHeader(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// this method is used to add image indicating a sort direction on header
    /// </summary>    
    private void AddImageToHeader(HtmlTableRow aoHtmlTableRow, string asSortExpression, string asSortDirection)
    {
        if (asSortExpression.Trim().Equals(""))
            return;

        // Create the sorting image based on the sort direction.
        System.Web.UI.WebControls.Image sortImage = new System.Web.UI.WebControls.Image();
        sortImage.ID = "sortImage";
        if (asSortDirection == "asc")
        {
            sortImage.ImageUrl = "~/RITeSchool/images/up.gif";
            sortImage.AlternateText = "Ascending Order";
        }
        else if (asSortDirection == "desc")
        {
            sortImage.ImageUrl = "~/RITeSchool/images/down.gif";
            sortImage.AlternateText = "Descending Order";
        }
        // Iterate through the Columns collection to determine the index
        // of the column being sorted.
        foreach (HtmlTableCell oHtmlTableCell in aoHtmlTableRow.Cells)
        {
            asSortExpression = asSortExpression.Replace(" ", "").Replace("asc", "").Replace("desc", "");

            // Iterate through the cells collection to determine the index
            // of the cell being sorted.
            foreach (Control oControl in oHtmlTableCell.Controls)
            {
                LinkButton oLinkButton = oControl as LinkButton;
                if (oLinkButton != null && oLinkButton.CommandArgument == asSortExpression)
                {
                    System.Web.UI.WebControls.Image oImage = (System.Web.UI.WebControls.Image)oHtmlTableCell.FindControl("sortImage");
                    if (oImage == null)
                    {
                        // Add the image to the appropriate header cell.
                        if (sortImage.ImageUrl != "")
                        {
                            oHtmlTableCell.Controls.Add(sortImage);
                            break;
                        }
                    }

                }
            }
        }
    }

    /// <summary>
    /// This method is used to set design according to the selected language.
    /// </summary>
    private void DesignSettingAccordingLanguage()
    {
        btnSave.Text = oResourceManager.GetString(hidSave.Value.Replace(" ", string.Empty));
        valsumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidRemarkTemplateShouldNotBeBlank.Value = Resources.LocalizedResources.RemarkTemplateShouldNotBeBlank;
        hidRemarkTemplateShouldNotExceed.Value = Resources.LocalizedResources.RemarkTemplateShouldNotExceed;
        hidAreYouSureYouWantToDeleteRemarkTemplate.Value = Resources.LocalizedResources.AreYouSureYouWantToDeleteRemarkTemplate;
        hidKeywordLimitNote.Value = Resources.LocalizedResources.KeywordLimitNote;
        hidCharacters.Value = Resources.LocalizedResources.Characters;
        if (lstvwTemplates.Items.Count > 0)
            ControlUtility.FillListViewPagerFooterWithCulture(lstvwTemplates, DtPgCount, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);
    }
    #endregion --Private Methods--   
  
}