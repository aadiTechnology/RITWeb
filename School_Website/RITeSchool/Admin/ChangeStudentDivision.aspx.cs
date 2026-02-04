/* File Name :- ChangeStudentDivision.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 18-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to transfer student from one division/standard to another.
*/

using System;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using SchoolAutoSearchService.Service;
using System.Data.SqlClient;

public partial class ChangeStudentDivision : SchoolBase
{
    #region Constants

    const string S_SCREENS_URL = "ScreensUI.aspx";
    const string S_BLANK_GRID_MESSAGE = "No student available.";

    #endregion

    #region Data Members

    static string msURL = String.Empty;

    #endregion

    #region Events

    /// <summary>
    /// this event is used to select masterpage according to source url.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);
			
			string sUrl = GetSourceUrl();
            if (IsPostBack)
                sUrl = msURL;
			if(sUrl.Contains(S_SCREENS_URL))
                this.Page.MasterPageFile = "~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master";
            else
                this.Page.MasterPageFile = "../MasterPages/MasterPage.master";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for following purposes :-  
    /// 1)To hide according to masterpage
    /// 2)Initialize default values to controls
    /// 3)Fill standard comboboxes.
    /// 4)Set postback url to cancel button and set javascript attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {               
            if (CheckPreCondition() && !IsPostBack)
            {
                HideControls();
                InitializeFields();
                FillStandardCombobox();
                ddlCurrentStandard.Focus();
                SetPostbackUrl();
                SetDefaultSortArrowOfGrid();
                SetJavascriptAttributes();
                ddlCurrentStandard.Focus();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

	/// <summary>
	/// This event is used to add the sort image for the FileList table.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_PreRender(object sender, EventArgs e)
	{
		try
		{
			GridViewRow headerRow = grdStudents.HeaderRow;
			int sortColumnIndex = CommonUtility.GetSortColumnIndex(grdStudents, hidSortExpression.Value);
			if(headerRow != null && sortColumnIndex != -1)
			{
				// Call the AddSortImage helper method to add
				// a sort direction image to the appropriate
				// column header. 
				CommonUtility.AddSortImage(sortColumnIndex, headerRow, hidSortDirection.Value);
			}
		}
		catch(Exception ex)
		{
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
		}
	}

    /// <summary>
    /// This event is used to fill source and target combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCurrentStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            
            tblTransferMessage.Visible = false;
            FillDivisions(ddlCurrentDiv, Convert.ToInt32(ddlCurrentStandard.SelectedValue));
            HideControlsOnStdDivChange();
            ShowContainer();
            //Fill target combobox.
            if (ddlTargetStandard.Visible == false)
                FillDivisions(ddlTargetDiv, Convert.ToInt32(ddlCurrentStandard.SelectedValue));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill target combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlTargetStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillDivisions(ddlTargetDiv, Convert.ToInt32(ddlTargetStandard.SelectedValue));
            if(ddlCurrentStandard.SelectedValue != ddlTargetStandard.SelectedValue )
                hidIsAcrossStandard.Value = Constants.S_ONE;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            tblTransferMessage.Visible = false;
            FillStudentGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to change division/standard of a particular student.
    /// </summary>
    /// <param name="e"></param>
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                StudentBL oStudentBL = new StudentBL();
                //This string contains XML for Student Ids.
                string sStudentIdsXML = GetXMLForStudentIds();
                string sInternalFeeMessage = string.Empty;
                if (ddlTargetStandard.Visible == true)
                    sInternalFeeMessage = oStudentBL.UpdateStudentDivision(sStudentIdsXML, Convert.ToInt32(ddlCurrentStandard.SelectedValue), Convert.ToInt32(ddlCurrentDiv.SelectedValue), Convert.ToInt32(ddlTargetStandard.SelectedValue), Convert.ToInt32(ddlTargetDiv.SelectedValue), miSchoolId, miAcademicYearId, miFinancialYearId);
                else
                    sInternalFeeMessage = oStudentBL.UpdateStudentDivision(sStudentIdsXML, Convert.ToInt32(ddlCurrentStandard.SelectedValue), Convert.ToInt32(ddlCurrentDiv.SelectedValue), Convert.ToInt32(ddlCurrentStandard.SelectedValue), Convert.ToInt32(ddlTargetDiv.SelectedValue), miSchoolId, miAcademicYearId, miFinancialYearId);

                RefreshStudentCache(sStudentIdsXML);

                tblTransferMessage.Visible = true;
                lblTransferMessage.Text = "Students transferred successfully!!";
                if (sInternalFeeMessage.Trim() != string.Empty)
                    lblMessage.Text = sInternalFeeMessage.Substring(0, sInternalFeeMessage.IndexOf('$'));
                FillStudentGrid();
            }
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
    /// This method is used to to fill division combo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCurrentDiv_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ShowContainer();
            if (ddlCurrentDiv.SelectedIndex != 0)
            {
                tblSearch.Visible = true;
                grdStudents.Visible = true;
                txtSearch.Text = String.Empty;
                SetDefaultSortArrowOfGrid();
                FillStudentGrid();
                if (grdStudents.Rows.Count > 0)
                {
                    btnTransfer.Visible = true;
                    ddlTargetDiv.Enabled = true;
                    grdStudents.PageIndex = 0;
                }
            }
            else
            {
                ddlTargetDiv.SelectedIndex = 0;
                HideControlsOnStdDivChange();
                trTotalRec.Visible = false;
            }
            tblTransferMessage.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #region Grid Events

    /// <summary>
    /// This method is used for paging.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdStudents.PageIndex = e.NewPageIndex;
            FillStudentGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used for sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
            FillStudentGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void grdStudents_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow pagerRow = e.Row;

				// Retrieve the DropDownList and Label controls from the row.
                DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
                Label pageLabel = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel");

                if (pageList != null)
                {

                    // Create the values for the DropDownList control based on 
                    // the  total number of pages required to display the data
                    // source.
                    for (int iPageCount = 0; iPageCount < grdStudents.PageCount; iPageCount++)
                    {
                        // Create a ListItem object to represent a page.
                        int pageNumber = iPageCount + 1;
                        ListItem item = new ListItem(pageNumber.ToString());

                        if (iPageCount == grdStudents.PageIndex)
                            item.Selected = true;

                        // Add the ListItem object to the Items collection of the DropDownList.                        
                        pageList.Items.Add(item);
                    }
                }

                if (pageLabel != null)
                {
                    // Calculate the current page number.
                    int currentPage = grdStudents.PageIndex + 1;

                    // Update the Label control with the current page information.
                    pageLabel.Text = "Page " + currentPage.ToString() + " of " + grdStudents.PageCount.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            GridViewRow pagerRow = grdStudents.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");

            // Set the PageIndex property to display that page selected by the user.
            grdStudents.PageIndex = pageList.SelectedIndex;
            tblTransferMessage.Visible = false;
            FillStudentGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != String.Empty)
            {
                lblStartIndex.Text = Convert.ToString((grdStudents.PageSize * grdStudents.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdStudents.PageSize) - 1);
                if (e.ReturnValue != null && e.ReturnValue.ToString() != String.Empty)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                        if (Convert.ToInt32(e.ReturnValue) < 20)
                            trTotalRec.Visible = false;
                        else
                            trTotalRec.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

	protected void Validate_Student(object obj, ServerValidateEventArgs e)
    {
        try
        {
            StudentBL oStudentBL = new StudentBL();
            string sStudentIdsXML = GetXMLForStudentIds();
            string sStudentNames = oStudentBL.ValidateTransferStudent(miSchoolId, miAcademicYearId, sStudentIdsXML, ddlTargetDiv.SelectedValue.ToInt(), ddlTargetStandard.SelectedValue.ToInt());

            if (sStudentNames == "")
                e.IsValid = true;
            else
            {
                e.IsValid = false;

                CustomValidator cv = obj as CustomValidator;
                cv.ErrorMessage = "There exist student(s) with same first and last name in target class. Student Name(s) : "+sStudentNames;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #endregion

    #region Methods

    // Currently this is unusable code this may require in future.
    /// <summary>
    /// Disables those students in the Grid, who have paid some fees atleast.
    /// </summary>
    private void DisableFeesPaidStudents()
    {
		if(ddlTargetStandard.Visible && grdStudents.Rows.Count > 0)
		{
			string[] sarrPaidFeeStudentIds = hidPaidFeesStudentIds.Value.Split(new char[]{','});
			bool bSameStdSelected = ddlCurrentStandard.SelectedIndex != 0 && ddlTargetStandard.SelectedIndex != 0 && ddlCurrentStandard.SelectedValue != ddlTargetStandard.SelectedValue;
			foreach(GridViewRow row in grdStudents.Rows)
			{
				if(row.RowType == DataControlRowType.DataRow)
				{
					bool bHasPaidFees = false;
					foreach(string id in sarrPaidFeeStudentIds)
					{
						if(id == grdStudents.DataKeys[row.DataItemIndex]["YearWise_Student_Id"].ToString())
							bHasPaidFees = true;
					}
					
					if(bHasPaidFees)
					{
						if(bSameStdSelected)
							row.Style.Add("background-color", "GainsBoro");
						else
							row.Style.Remove("background-color");

						CheckBox oCheckBox = row.FindControl("ChkBoxDelete") as CheckBox;
						if(oCheckBox != null)
						{
							oCheckBox.Enabled = !bSameStdSelected;
							oCheckBox.Checked = false;
						}
					}
				}
			}
		}
    }
    
    /// <summary>
    /// This function checks the preconditons of Configured Std-Divisions and Subjects for Division wise Subjects criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.ChangeStudentDivision);
        if (sLinks.Equals(String.Empty))
        {
            trPrecondition.Visible = false;
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            trPrecondition.Visible = true;
            divErr.Visible = true;
            divErr.InnerHtml = sLinks;
            HideAllControls();
        }
        return bReturn;

    }

    /// <summary>
    /// This method is used to set default properties to grid and to fill standard combo.
    /// </summary>
    private void InitializeFields()
    {
        ValSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        grdStudents.PageSize = Constants.I_GRID_PAGE_COUNT;
        grdStudents.EmptyDataText = S_BLANK_GRID_MESSAGE;
        lblTo.Text = Constants.S_TO;
        lblOutOf.Text = Constants.S_OUT_OF;
        lblRecords.Text = Constants.S_RECORDS;

        tblTransferMessage.Visible = false;
        tblSearch.Visible = false;
        trTotalRec.Visible = false;
        SetDefaultButton(btnSearch);

        if (moSchool == Constants.SchoolId.PPSN)
            CustValidator.Enabled = true;
    }

    /// <summary>
    /// This method is used to set postback url of cancel button.
    /// </summary>
    private void SetPostbackUrl()
    {
        string sUrl = GetSourceUrl();
        if (sUrl.Contains(S_SCREENS_URL))
            btnCancel.PostBackUrl = "../SuperAdmin/ScreensUI.aspx";
        else
            btnCancel.Visible = false;
    }

    /// <summary>
    /// This method fills current and target combobox with all standards available in current school.
    /// </summary>
    private void FillStandardCombobox()
    {
        YearWIseStudentsBL oYearWiseSTudentInfoBL = new YearWIseStudentsBL();        
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId,miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        //Fill current standards into source and target combobox.
        ControlUtility.FillDropDownList(oDtStandardCollection, ref ddlCurrentStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT);

        ControlUtility.FillDropDownList(oDtStandardCollection, ref ddlTargetStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT);
        FillDefaultDivisionValue();
    }

    /// <summary>
    /// This method ios used to insert default calues into division comboboxes.
    /// </summary>
    private void FillDefaultDivisionValue()
    {
        ListItem olstDivision = new ListItem();
        olstDivision.Text = "-- Select --";
        olstDivision.Value = "0";
        ddlCurrentDiv.Items.Add(olstDivision);
        ddlTargetDiv.Items.Add(olstDivision);
    }

    /// <summary>
    /// This method is used to hide controls according to masterpage.
    /// </summary>
    private void HideControls()
    {
        string sUrl = String.Empty;
        if (hidBackUrl.Value != null && hidBackUrl.Value != String.Empty)
            sUrl = hidBackUrl.Value;
        else
            sUrl = GetSourceUrl();

        msURL = sUrl;

        tblTransferMessage.Visible = false;

        if (sUrl.Contains(S_SCREENS_URL))
        {
            hidBackUrl.Value = "../SuperAdmin/ScreensUI.aspx";
            tdArrow1.Visible = true;
            tdArrow2.Visible = true;
            tdTargerStdLabel.Visible = true;
            tdTargetStdCombo.Visible = true;
        }
        else
        {
            //tdArrow1.Visible = false;
            //tdArrow2.Visible = false;
            //tdTargerStdLabel.Visible = false;
            //tdTargetStdCombo.Visible = false;
            //lblCurrentStandard.Text = "Select Standard : ";
        }
    }

    /// <summary>
    /// This method is used to fill given combobox with divisions of selected standards. 
    /// </summary>
    /// <param name="ddlList"></param>
    /// <param name="aiStandardId"></param>
    private void FillDivisions(DropDownList ddlList, int aiStandardId)
    {
        DivisionCollectionBL oDiv = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtClass = oDiv.GetAllDivisionsForStandard(aiStandardId);
        //This method is used to fill current division's combo.
        ControlUtility.FillDropDownList(oDtClass, ref ddlList,
                                       "division_Id",
                                       Constants.S_DIVISION_NAME_FIELD,
                                       Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill student's grid of a particular standard and division.
    /// </summary>    
    private void FillStudentGrid()
    {
        int iCurrentStandardId = Convert.ToInt32(ddlCurrentStandard.SelectedValue);
        int iCurrentDivId = Convert.ToInt32(ddlCurrentDiv.SelectedValue);
        int iStartRowIndex = grdStudents.PageIndex - 1 * Constants.I_GRID_PAGE_COUNT;

        StudentBL oStudentBL = new StudentBL();
        DataTable oDTCurrentStudents = oStudentBL.GetAllCurrentStudents(miSchoolId, miAcademicYearId, iCurrentStandardId, iCurrentDivId, txtSearch.Text.Trim(), hidSortExpression.Value + ' ' + hidSortDirection.Value, 1000, 0, false);
        if (oDTCurrentStudents.Rows.Count > 0)
        {
            divContainer.Visible = true;
            btnTransfer.Enabled = true;
            grdStudents.DataSource = oDTCurrentStudents;
            grdStudents.DataBind();
            
            List<string> lstPaidFeesStudentIds = StudentBL.GetPaidFeesStudents(miSchoolId, miAcademicYearId, iCurrentStandardId, iCurrentDivId);
            hidPaidFeesStudentIds.Value = String.Join(",", lstPaidFeesStudentIds.ToArray());
        }
        else
        {
            divContainer.Visible = false;
            btnTransfer.Enabled = false;
        }

        SetTransferAttribute();
    }

    /// <summary>
    /// This method is used to set transfer butto attributes.
    /// </summary>
    private void SetTransferAttribute()
    {
        string sErrorMessageToSelectStudents;
        if (ddlTargetStandard.Visible == false)
            sErrorMessageToSelectStudents = "At least one student should be selected for changing division.";
        else
            sErrorMessageToSelectStudents = "At least one student should be selected for changing standard/division.";

        btnTransfer.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdStudents.AllowPaging + "','" + sErrorMessageToSelectStudents + "'))){return false;}");
    }

    /// <summary>
    /// This method is used to set sort variables
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ddlTargetStandard.Attributes["onchange"] = "javascript:ValidateStandardDivisions()";
        ddlCurrentStandard.Attributes["onchange"] = "javascript:ValidateStandardDivisions()";
        ddlCurrentDiv.Attributes["onchange"] = "javascript:ValidateStandardDivisions()";        
        ApplyMouseHoverEffect(new List<Button> { btnTransfer, btnCancel, btnSearch });

        trLegend.Visible = msURL.Contains(S_SCREENS_URL);
        trStdChkLst.Visible = msURL.Contains(S_SCREENS_URL);
    }

    /// <summary>
    /// This method is used to set initial values of sort variables
    /// </summary>
    private void SetDefaultSortArrowOfGrid()
    {
        hidSortExpression.Value = grdStudents.Columns[2].SortExpression;
        hidSortDirection.Value = Constants.S_ASCENDING;
    }
    /// <summary>
    /// This method is used to ger referrence page URL.
    /// </summary>
    /// <returns></returns>
    private string GetSourceUrl()
    {
        string sSourcePageUrl = string.Empty;
        if (Request.UrlReferrer != null)
        {
            sSourcePageUrl = Request.UrlReferrer.AbsolutePath;
            sSourcePageUrl = sSourcePageUrl.Substring(sSourcePageUrl.LastIndexOf("/") + 1);
        }
        return sSourcePageUrl;
    }

    /// <summary>
    /// This method creates an XML for student list.
    /// </summary>
    /// <returns></returns>
    private string GetXMLForStudentIds()
    {
        const string S_CHECK_BOX_DELETE = "ChkBoxDelete";
        const string S_ELEMENT = "element";
        const Int32 I_PK_USER_ID = 0;

        XmlDocument oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement("StudentList");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentList", string.Empty);
        int iRowCount = grdStudents.Rows.Count;
        for (int i = 0; i < iRowCount; i++)
        {
            CheckBox chkDeleteflag = (CheckBox)grdStudents.Rows[i].FindControl(S_CHECK_BOX_DELETE);

            if (chkDeleteflag.Checked == true)
            {
                int iStudentId = Convert.ToInt32(grdStudents.DataKeys[i][I_PK_USER_ID].ToString());
                string sAtrrName;
                XmlAttribute attr;

                XmlNode oXmlNode;
                oXmlNode = oDoc.CreateNode(S_ELEMENT, "Student", String.Empty);

                sAtrrName = "YrWIse_Student_Id";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = iStudentId.ToString();
                oXmlNode.Attributes.Append(attr);
                oXmlRootNode.AppendChild(oXmlNode);
            }

        }
        root.AppendChild(oXmlRootNode);
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to disable or hide controls.
    /// </summary>
    private void HideControlsOnStdDivChange()
    {
        btnTransfer.Visible = false;
        grdStudents.Visible = false;
        tblTransferMessage.Visible = false;
        tblSearch.Visible = false;
        trTotalRec.Visible = false;
    }

    /// <summary>
    /// This method is used to hide all the controls if precondition becomes false.
    /// </summary>
    private void HideAllControls()
    {
        tdValidationSummary.Visible = false;
        trMiddle.Visible = false;
        tdGrid.Visible = false;
        tblSearch.Visible = false;
        tdInfo.Visible = false;
        tdTransfer.Visible = false;
        btnCancel.Text = "Back";

    }

    private void ShowContainer()
    {
        if (ddlCurrentDiv.SelectedValue == "0" || ddlCurrentStandard.SelectedValue == "0")
            divContainer.Visible = false;
        else
            divContainer.Visible = true;

    }

    /// <summary>
    /// This method is used to refresh student cache.
    /// </summary>
    private void RefreshStudentCache(string asStudentIdsXML)
    {
        XDocument doc = XDocument.Parse(asStudentIdsXML);
        List<int> lstStudentIds = doc.Root.Elements().Attributes().Select(s => s.Value.ToInt()).ToList();

        AutoSearchService oAutoSearchService = new AutoSearchService();
        oAutoSearchService.RefreshStudentCache(miSchoolId, miAcademicYearId, lstStudentIds, Constants.Action.Insert);
    }

    #endregion
}
