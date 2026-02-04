// File Name    : RegenarateRollNoUI.aspx.cs
// Created By   : Milind
// Start Date   : 11 May 2009
// Description  : This class is used to regenerate students roll numbers according to selected criteria.


using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BusinessLogic;
using Utility;
using System.Text;
using System.Collections.Generic;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class RegenarateRollNoUI :SchoolBase
{
  
    # region Events

    /// <summary>
    /// This event is used to fill all controls data like standards and divisions.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            { 
                FillStandardCombobox();
                ShowHideControls(false);
                SetDefaultProperties();
                SetClientScriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill division combox according to selected standard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStd_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidStandardId.Value = cmbStandard.SelectedValue.ToString();
            FillDivisionCombobox();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used show the grid according to the search criteria.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnShow.Text == "Show")
            {
                grdStudents.PageIndex = 0;
                btnShow.Text = "Change Input";
                SetGridViewDateColumnProperties();
                grdStudents.DataSourceID = GrdDSobj.ID;
                ShowHideControls(true);
                FillSortingControls();
                hidStandardName.Value = cmbStandard.SelectedItem.Text.ToString();
                hidDivisionName.Value = cmbDivision.SelectedItem.Text.ToString();
            }
            else
            {
                btnShow.Text = "Show";
                grdStudents.DataSourceID = null;
                ShowHideControls(false);

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is updating roll no of students according to sorting criteria.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            StudentBL oStudentBL = new StudentBL();

            string sFilter = string.Empty;
            sFilter = CreateSortingFilter();
            sFilter = sFilter.Substring(0, sFilter.LastIndexOf(','));
            int iStdId = Convert.ToInt32(cmbStandard.SelectedValue);
            int iDivId = Convert.ToInt32(cmbDivision.SelectedValue);
            int iSchoolId =miSchoolId;
            int iAcadmicYearId = miAcademicYearId;

            oStudentBL.RegenerateStudentRollNo(iSchoolId, iAcadmicYearId, iStdId, iDivId, sFilter);
            grdStudents.DataSourceID = GrdDSobj.ID;
            SetDefaultControlProperties();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for sorting.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
            SetGridViewDateColumnProperties();
            grdStudents.DataSourceID = GrdDSobj.ID;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to map gridview pageindex with combobox index.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            GridViewRow pagerRow = grdStudents.BottomPagerRow;
            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
            // Set the PageIndex property to display that page selected by the user.
            grdStudents.PageIndex = pageList.SelectedIndex;
            SetGridViewDateColumnProperties();
            grdStudents.DataSourceID = GrdDSobj.ID;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill Dropdownlist.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
                    for (int i = 0; i < grdStudents.PageCount; i++)
                    {

                        // Create a ListItem object to represent a page.
                        int pageNumber = i + 1;
                        ListItem item = new ListItem(pageNumber.ToString());

                        // If the ListItem object matches the currently selected
                        // page, flag the ListItem object as being selected. Because
                        // the DropDownList control is recreated each time the pager
                        // row gets created, this will persist the selected item in
                        // the DropDownList control.   
                        if (i == grdStudents.PageIndex)
                            item.Selected = true;

                        // Add the ListItem object to the Items collection of the 
                        // DropDownList.
                        pageList.Items.Add(item);
                    }
                }
                if (pageLabel != null)
                {
                    // Calculate the current page number.
                    int currentPage = grdStudents.PageIndex + 1;

                    // Update the Label control with the current page information.
                    pageLabel.Text = "Page " + currentPage.ToString() +
                      " of " + grdStudents.PageCount.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set sort direction image to the appropriate column header.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = ((System.Web.UI.WebControls.GridView)(sender));

            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Call the GetSortColumnIndex helper method to determine
                // the index of the column being sorted.
                int iSortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, sGridviewName.SortExpression);
                if (iSortColumnIndex != -1)
                {
                    // Call the AddSortImage helper method to add
                    // a sort direction image to the appropriate
                    // column header. 
                    CommonUtility.AddSortImage(iSortColumnIndex, e.Row, sGridviewName.SortDirection);
                }
                else
                {
                    CommonUtility.AddSortImage(2, e.Row, sGridviewName.SortDirection);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set values to record range labels.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdDSobj_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        int iTotal;
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
            {
                if (int.TryParse(e.ReturnValue.ToString(), out iTotal) && iTotal == 0)
                {
                    tbGrid.Visible = false;
                    tblHeader.Visible = false;
                    tblStudent.Visible = true;
                }
                else
                {
                    tbGrid.Visible = true;
                    tblHeader.Visible = true;
                }
                lblStartIndex.Text = Convert.ToString((grdStudents.PageSize * grdStudents.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdStudents.PageSize) - 1);
                if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                        if (e.ReturnValue.ToString() == "0")
                            tdTotalRec.Visible = false;
                        else
                            tdTotalRec.Visible = true;
                    }
                    if (lblTotal.Text != "")
                    {
                        if (Convert.ToInt32(lblTotal.Text) <= Constants.I_GRID_PAGE_COUNT)
                            tdTotalRec.Visible = false;
                        else
                            tdTotalRec.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method fills combobox with all standards available in current school.
    /// </summary>
    private void FillStandardCombobox()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDtStandardCollection, ref cmbStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method fills combobox with Divisions
    /// </summary>
    private void FillDivisionCombobox()
    {
        const string S_STDDIV_ID_FLD = "division_Id";
        DivisionCollectionBL oDiv = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable dtDivision;

        if (hidStandardId.Value != "")
            dtDivision = oDiv.GetAllDivisionsForStandard(Convert.ToInt32(hidStandardId.Value));
        else
            dtDivision = oDiv.GetAllSchoolDivisions();
        //This method is used to fill current division's combo.
        ControlUtility.FillDropDownList(dtDivision, ref cmbDivision,
                                       S_STDDIV_ID_FLD,
                                       Constants.S_DIVISION_NAME_FIELD,
                                       Constants.S_SELECT_ALL);
    }

    ///<Summary>
    ///This method is used to set default properties to controls.
    ///</Summary>   
    private void SetDefaultProperties()
    {
        System.Web.UI.HtmlControls.HtmlForm oform = (HtmlForm)this.Master.FindControl("form1");
        oform.DefaultButton = btnShow.UniqueID;

        cmbDivision.Items.Add(new ListItem(Constants.S_SELECT_ALL, "0"));
        hidSortDirection.Value = Constants.S_ASCENDING;
        grdStudents.PageSize = Constants.I_GRID_PAGE_COUNT;
    }

    /// <summary>
    /// This method is used to hide and show the control.
    /// </summary>
    private void ShowHideControls(bool bFlag)
    {
        cmbStandard.Enabled = !bFlag;
        cmbDivision.Enabled = !bFlag;
        tbGrid.Visible = bFlag;
        tblHeader.Visible = bFlag;
        tblStudent.Visible = bFlag;
        tblError.Visible = bFlag;
    }

    /// <summary>
    /// This function is used to change sort order.
    /// </summary>
    private void SetSortVariables()
    {
        if (hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to fill all comboboxes related to sorting fields .
    /// </summary>
    private void FillAllComboBoxes()
    {
        const string S_SORT_FIELD_ID = "Sorting_Field_Id";
        const string S_SORT_FIELD_NAME = "Field_Name";

        StudentBL oStudentBL = new StudentBL();
        DataTable oDSSortField = oStudentBL.RetriveSortingField();

        ControlUtility.FillDropDownList(oDSSortField, ref ddlFieldFirst,
                                        S_SORT_FIELD_ID,
                                        S_SORT_FIELD_NAME,
                                        Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDSSortField, ref ddlFieldSecond,
                                        S_SORT_FIELD_ID,
                                        S_SORT_FIELD_NAME,
                                        Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDSSortField, ref ddlFieldThird,
                                       S_SORT_FIELD_ID,
                                       S_SORT_FIELD_NAME,
                                       Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDSSortField, ref ddlFieldFourth,
                                        S_SORT_FIELD_ID,
                                        S_SORT_FIELD_NAME,
                                        Constants.S_SELECT);
        ControlUtility.FillDropDownList(oDSSortField, ref ddlFieldFifth,
                                        S_SORT_FIELD_ID,
                                        S_SORT_FIELD_NAME,
                                        Constants.S_SELECT);

    }

    /// <summary>
    /// This method is used to fill all sorting related controls .
    /// </summary>
    private void FillSortingControls()
    {
        FillAllComboBoxes();
        SetDefaultControlProperties();
    }

    /// <summary>
    /// This method is used to set default value to the sorting controls .
    /// </summary>
    private void SetDefaultControlProperties()
    {
        optAscOrderFirst.Checked = true;
        optAscOrderSecond.Checked = true;
        optAscOrderThird.Checked = true;
        optAscOrderFourth.Checked = true;
        optAscOrderFifth.Checked = true;

        optDescOrderFirst.Checked = false;
        optDescOrderSecond.Checked = false;
        optDescOrderThird.Checked = false;
        optDescOrderFourth.Checked = false;
        optDescOrderFifth.Checked = false;


        ddlFieldFirst.SelectedIndex = 0;
        ddlFieldSecond.SelectedIndex = 0;
        ddlFieldThird.SelectedIndex = 0;
        ddlFieldFourth.SelectedIndex = 0;
        ddlFieldFifth.SelectedIndex = 0;
    }

    /// <summary>
    /// This function is used to set the date format for date column property 
    /// </summary>    
    private void SetGridViewDateColumnProperties()
    {
        const int I_DATE_COLUMN = 4;

        BoundField oReceivedDate = (BoundField)grdStudents.Columns[I_DATE_COLUMN];
        oReceivedDate.HtmlEncode = false;
        oReceivedDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
    }

    /// <summary>
    /// This method is used to creating filter for updating roll number of students.
    /// </summary>
    private string CreateSortingFilter()
    {
        StringBuilder oStringBuilder = new StringBuilder("ORDER BY ");
        int iIndex;

        if (ddlFieldFirst.SelectedIndex != Constants.I_ZERO)
        {
            iIndex = Convert.ToInt32(ddlFieldFirst.SelectedValue);
            if (optAscOrderFirst.Checked)
                oStringBuilder.Append(sSortingField(iIndex, Constants.S_ASCENDING));
            else
                oStringBuilder.Append(sSortingField(iIndex, Constants.S_DESCENDING));
        }
        if (ddlFieldSecond.SelectedIndex != Constants.I_ZERO)
        {
            iIndex = Convert.ToInt32(ddlFieldSecond.SelectedValue);
            if (optAscOrderSecond.Checked)
                oStringBuilder.Append(sSortingField(iIndex, Constants.S_ASCENDING));
            else
                oStringBuilder.Append(sSortingField(iIndex, Constants.S_DESCENDING));
        }
        if (ddlFieldThird.SelectedIndex != Constants.I_ZERO)
        {
            iIndex = Convert.ToInt32(ddlFieldThird.SelectedValue);
            if (optAscOrderThird.Checked)
                oStringBuilder.Append(sSortingField(iIndex, Constants.S_ASCENDING));
            else
                oStringBuilder.Append(sSortingField(iIndex, Constants.S_DESCENDING));
        }
        if (ddlFieldFourth.SelectedIndex != Constants.I_ZERO)
        {
            iIndex = Convert.ToInt32(ddlFieldFourth.SelectedValue);
            if (optAscOrderFourth.Checked)
               oStringBuilder.Append(sSortingField(iIndex, Constants.S_ASCENDING));
            else
                oStringBuilder.Append(sSortingField(iIndex, Constants.S_DESCENDING));
        }
        if (ddlFieldFifth.SelectedIndex != Constants.I_ZERO)
        {
            iIndex = Convert.ToInt32(ddlFieldFifth.SelectedValue);
            if (optAscOrderFifth.Checked)
                oStringBuilder.Append(sSortingField(iIndex, Constants.S_ASCENDING));
            else
                oStringBuilder.Append(sSortingField(iIndex, Constants.S_DESCENDING));
        } 

        return oStringBuilder.ToString();
    }

    /// <summary>
    /// This method is used to return sorted field according to selection .
    /// </summary>
    private string sSortingField(int iIndex, string sDirection)
    {
        string sFilter = "";
        switch (iIndex)
        {
            case 1:
                sFilter = "vw_BaseStudentDetails.First_Name " + sDirection + ",";
                break;
            case 2:
                sFilter = "vw_BaseStudentDetails.Last_Name " + sDirection + ",";
                break;
            case 3:
                sFilter = "vw_BaseStudentDetails.Sex " + sDirection + ",";
                break;
            case 4:
                sFilter = "Category_Master.Category_Name " + sDirection + ",";
                break;
            case 5:
                sFilter = "vw_BaseStudentDetails.Enrolment_Number " + sDirection + ",";
                break;
        }
        return sFilter;
    }

    /// <summary>
    /// This method is used to set java script properties to page controls.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnShow, btnGenerate, btnBack });
        btnGenerate.Attributes.Add("Onclick", "if(!(DuplicateField())){return false;}");           
    }

    #endregion

}
