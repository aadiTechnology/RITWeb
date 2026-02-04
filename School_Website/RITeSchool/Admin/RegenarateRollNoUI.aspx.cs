// File Name    : RegenarateRollNoUI.aspx.cs
// Created By   : Milind
// Start Date   : 11 May 2009    
// Description  : This class is used to regenerate students roll numbers according to selected criteria.

using System;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using System.Resources;
using Utility;

public partial class RegenarateRollNoUI : SchoolBase
{
    #region -- MEMBER(s) --

    private const string S_SHOW = "Show";
    private const string S_REASSIGN_UPDATE_MSG = "Roll numbers updated successfully !!!";
    private const string S_REGENERATE_UPDATE_MSG = "Roll numbers regenerated successfully !!!";
    ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
    #endregion -- MEMBER(s) --

    #region -- EVENT HANDLER(s) --

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

                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                hidbtnShow.Value = "Show";
                FillStandardCombobox();
                ShowHideControls(false);
                SetDefaultProperties();
                SetClientScriptAttributes();
                RefreshValue();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();                           
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
            hidStandardId.Value = cmbStandard.SelectedValue;
            FillDivisionCombobox();

            stdMdtStar.Style["visibility"] = rbtnReassign.Checked ? "visible" : "hidden";
            divMdtStar.Style["visibility"] = rbtnReassign.Checked ? "visible" : "hidden";
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
            if (btnShow.Text == Resources.LocalizedResources.Show )
            {
                grdStudents.PageIndex = 0;
                hidbtnShow.Value = "Change Input";
                btnShow.Text = Resources.LocalizedResources.ChangeInput;
                SetGridViewDateColumnProperties();
                grdStudents.PageSize = rbtnReassign.Checked ? 1000 : 20;
                grdStudents.DataSourceID = GrdDSobj.ID;
                ShowHideControls(true);
                FillSortingControls();
                hidStandardName.Value = cmbStandard.SelectedItem.Text;
                hidDivisionName.Value = cmbDivision.SelectedItem.Text;

                stdMdtStar.Style["visibility"] = rbtnReassign.Checked ? "visible" : "hidden";
                divMdtStar.Style["visibility"] = rbtnReassign.Checked ? "visible" : "hidden";
            }
            else
            {
                hidSortExpression.Value = String.Empty;
                hidSortDirection.Value = Constants.S_ASCENDING;
                hidbtnShow.Value = "Show";
                btnShow.Text = Resources.LocalizedResources.Show;
                grdStudents.Sort(String.Empty, SortDirection.Ascending);
                grdStudents.DataSourceID = null;
                ShowHideControls(false);
                btnSave.Visible = false;
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
            var oStudentBL = new StudentBL();

            string sFilter = CreateSortingFilter();
            sFilter = sFilter.Substring(0, sFilter.LastIndexOf(','));
            int iStdId = cmbStandard.SelectedValue.ToInt();
            int iDivId = cmbDivision.SelectedValue.ToInt();

            oStudentBL.RegenerateStudentRollNo(miSchoolId, miAcademicYearId, iStdId, iDivId, sFilter);

            lblMessage.Text = Resources.LocalizedResources.RollNumbersReGeneratedSuccessfully;
            tblMassage.Visible = true;

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
            var pageList = pagerRow.Cells[0].FindControl("PageDropDownList") as DropDownList;

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
            GridViewRow currentRow = e.Row;
            switch (currentRow.RowType)
            {
                case DataControlRowType.Pager:
                    {
                        // Retrieve the DropDownList and Label controls from the row.
                        var pageList = currentRow.Cells[0].FindControl("PageDropDownList") as DropDownList;
                        var pageLabel = currentRow.Cells[0].FindControl("CurrentPageLabel") as Label;

                        if (pageList != null)
                        {
                            // Create the values for the DropDownList control based on
                            // the  total number of pages required to display the data
                            // source.
                            for (int i = 0; i < grdStudents.PageCount; i++)
                            {
                                // Create a ListItem object to represent a page.
                                int pageNumber = i + 1;
                                var item = new ListItem(pageNumber.ToString());

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
                            hidPageNo.Value = Resources.LocalizedResources.PageNo;
                            hidOf.Value = Resources.LocalizedResources.Of;
                            hidOutOflst.Value = Resources.LocalizedResources.OutOflst;
                            pageLabel.Text = Resources.LocalizedResources.PageNo + " " + currentPage.ToString() + " " + Resources.LocalizedResources.Of + " " + grdStudents.PageCount.ToString()+" "+Resources.LocalizedResources.OutOflst;
                        }
                    }
                    break;
                case DataControlRowType.DataRow:
                    {
                        var oTextBox = currentRow.FindControl("txtNewRollNo") as TextBox;
                        oTextBox.Attributes.Add("onblur", "extractNumber(this,0,false)");
                        oTextBox.Attributes.Add("onkeyup", "extractNumber(this,0,false)");
                        oTextBox.Attributes.Add("onkeypress", "return blockNonNumbers (this, event, false, false);");
                        oTextBox.Attributes.Add("onpaste", "event.returnValue=false");
                        oTextBox.Attributes.Add("ondrop", "event.returnValue=false");

                        if (grdStudents.DataKeys[currentRow.RowIndex]["SchoolLeft_Date"] != DBNull.Value)
                        {
                            var oLabel = currentRow.FindControl("lblNewRoll_No") as Label;
                            if (rbtnReassign.Checked)
                            {
                                oLabel.Visible = true;
                                oLabel.Text = oTextBox.Text;
                            }
                            currentRow.Style.Add(HtmlTextWriterStyle.Color, "red !important;");
                            oTextBox.Visible = false;

                            if (grdStudents.DataKeys[currentRow.RowIndex]["Roll_No"].ToString() == "0")
                            {
                                currentRow.Cells[2].Text = string.Empty;
                                oLabel.Visible = false;
                            }
                        }
                    }
                    break;
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
            GridViewRow currentRow = e.Row;

            if (currentRow.RowType == DataControlRowType.Header)
            {
                var sGridviewName = sender as GridView;

                // Call the GetSortColumnIndex helper method to determine
                // the index of the column being sorted.
                int iSortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, sGridviewName.SortExpression);

                // Call the AddSortImage helper method to add a sort direction image to the appropriate column header.
                CommonUtility.AddSortImage(iSortColumnIndex != -1 ? iSortColumnIndex : 2, currentRow, sGridviewName.SortDirection);
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
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
            {
                int iTotal;
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
                lblEndIndex.Text = Convert.ToString((lblStartIndex.Text.ToInt() + grdStudents.PageSize) - 1);
                if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
                {
                    lblTotal.Text = e.ReturnValue.ToString();

                    if (!(e.ReturnValue is DataTable))
                    {
                        if (lblEndIndex.Text.ToInt() > lblTotal.Text.ToInt())
                            lblEndIndex.Text = e.ReturnValue.ToString();
                        tdTotalRec.Visible = e.ReturnValue.ToString() != "0";
                    }
                    if (lblTotal.Text != "")
                    {
                        tdTotalRec.Visible = lblTotal.Text.ToInt() > Constants.I_GRID_PAGE_COUNT;
                    }
                }
            }
            btnSave.Attributes.Add("OnClick", "if(!ValidatePage('txtNewRollNo','lblNewRoll_No','lblRegNo1','" + grdStudents.ClientID + "','" + e.ReturnValue.ToString() + "')) return false;");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        // Save the changes to database.
        try
        {
            string sXmlStudentsRollNos = GenerateStudentsRollNosXML();
            var oStudentBL = new StudentBL();
            int iStandardId = cmbStandard.SelectedValue.ToInt();
            int iDivisionId = cmbDivision.SelectedValue.ToInt();
            oStudentBL.UpdateStudentsRollNos(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, sXmlStudentsRollNos);
            lblMessage.Text = Resources.LocalizedResources.RollNumbersUpdatedSuccessfully;
            tblMassage.Visible = true;
            grdStudents.DataSourceID = GrdDSobj.ID;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion -- EVENT HANDLER(s) --

    #region -- PRIVATE METHOD(s) --

    /// <summary>
    /// This method fills combobox with all standards available in current school.
    /// </summary>
    private void FillStandardCombobox()
    {
        var oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        cmbStandard.Bind(oDtStandardCollection, Constants.S_STANDARD_ID_FIELD, Constants.S_STANDARD_NAME_FIELD, Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method fills combobox with Divisions
    /// </summary>
    private void FillDivisionCombobox()
    {
        const string S_STDDIV_ID_FLD = "division_Id";
        var oDiv = new DivisionCollectionBL(miSchoolId, miAcademicYearId);

        DataTable dtDivision = hidStandardId.Value.IsNullOrEmpty() ? oDiv.GetAllSchoolDivisions() : oDiv.GetAllDivisionsForStandard(hidStandardId.Value.ToInt());

        //This method is used to fill current division's combo.
        if (rbtnReassign.Checked)
        {
            if (cmbStandard.SelectedIndex == 0)
                cmbDivision.Bind(dtDivision, S_STDDIV_ID_FLD, Constants.S_DIVISION_NAME_FIELD, Constants.S_SELECT);
            else
                cmbDivision.Bind(dtDivision, S_STDDIV_ID_FLD, Constants.S_DIVISION_NAME_FIELD);
        }
        else
            cmbDivision.Bind(dtDivision, S_STDDIV_ID_FLD, Constants.S_DIVISION_NAME_FIELD, Constants.S_SELECT_ALL);

        cmbStandard.Items[0].Text = rbtnReassign.Checked ? Constants.S_SELECT : Constants.S_SELECT_ALL;
    }

    ///<Summary>
    ///This method is used to set default properties to controls.
    ///</Summary>
    private void SetDefaultProperties()
    {
        SetDefaultButton(btnShow);
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
        rbtnReassign.Enabled = !bFlag;
        rbtnRegenerate.Enabled = !bFlag;
        grdStudents.Columns[3].Visible = rbtnReassign.Checked;
        UpdatePanel3.Visible = rbtnRegenerate.Checked;
        btnSave.Visible = rbtnReassign.Checked;
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
        hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to fill all comboboxes related to sorting fields .
    /// </summary>
    private void FillAllComboBoxes()
    {
        const string S_SORT_FIELD_ID = "Sorting_Field_Id";
        const string S_SORT_FIELD_NAME = "Field_Name";

        var oStudentBL = new StudentBL();
        DataTable oDSSortField = oStudentBL.RetriveSortingField();

        ddlFieldFirst.Bind(oDSSortField, S_SORT_FIELD_ID, S_SORT_FIELD_NAME, Constants.S_SELECT);
        ddlFieldSecond.Bind(oDSSortField, S_SORT_FIELD_ID, S_SORT_FIELD_NAME, Constants.S_SELECT);
        ddlFieldThird.Bind(oDSSortField, S_SORT_FIELD_ID, S_SORT_FIELD_NAME, Constants.S_SELECT);
        ddlFieldFourth.Bind(oDSSortField, S_SORT_FIELD_ID, S_SORT_FIELD_NAME, Constants.S_SELECT);
        ddlFieldFifth.Bind(oDSSortField, S_SORT_FIELD_ID, S_SORT_FIELD_NAME, Constants.S_SELECT);
        ddlFieldSix.Bind(oDSSortField, S_SORT_FIELD_ID, S_SORT_FIELD_NAME, Constants.S_SELECT);
        
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
        optAscOrderSix.Checked = true;

        optDescOrderFirst.Checked = false;
        optDescOrderSecond.Checked = false;
        optDescOrderThird.Checked = false;
        optDescOrderFourth.Checked = false;
        optDescOrderFifth.Checked = false;
        optDescOrderSix.Checked = false;


        ddlFieldFirst.SelectedIndex = 0;
        ddlFieldSecond.SelectedIndex = 0;
        ddlFieldThird.SelectedIndex = 0;
        ddlFieldFourth.SelectedIndex = 0;
        ddlFieldFifth.SelectedIndex = 0;
        ddlFieldSix.SelectedIndex = 0;
    }

    /// <summary>
    /// This function is used to set the date format for date column property
    /// </summary>
    private void SetGridViewDateColumnProperties()
    {
        int iDateColumnIndex = 5;
        var oReceivedDate = (BoundField)grdStudents.Columns[iDateColumnIndex];
        oReceivedDate.HtmlEncode = false;
        oReceivedDate.DataFormatString = Constants.S_STANDARD_GRID_DATE_FORMAT;
    }

    /// <summary>
    /// This method is used to creating filter for updating roll number of students.
    /// </summary>
    private string CreateSortingFilter()
    {
        var oStringBuilder = new StringBuilder("ORDER BY ");

        if (ddlFieldFirst.SelectedIndex != Constants.I_ZERO)
            oStringBuilder.Append(GetSortString(ddlFieldFirst.SelectedValue.ToInt(),
                                                optAscOrderFirst.Checked ? Constants.S_ASCENDING : Constants.S_DESCENDING));

        if (ddlFieldSecond.SelectedIndex != Constants.I_ZERO)
            oStringBuilder.Append(GetSortString(ddlFieldSecond.SelectedValue.ToInt(),
                                                optAscOrderSecond.Checked ? Constants.S_ASCENDING : Constants.S_DESCENDING));

        if (ddlFieldThird.SelectedIndex != Constants.I_ZERO)
            oStringBuilder.Append(GetSortString(ddlFieldThird.SelectedValue.ToInt(),
                                                optAscOrderThird.Checked ? Constants.S_ASCENDING : Constants.S_DESCENDING));

        if (ddlFieldFourth.SelectedIndex != Constants.I_ZERO)
            oStringBuilder.Append(GetSortString(ddlFieldFourth.SelectedValue.ToInt(),
                                                optAscOrderFourth.Checked ? Constants.S_ASCENDING : Constants.S_DESCENDING));

        if (ddlFieldFifth.SelectedIndex != Constants.I_ZERO)
            oStringBuilder.Append(GetSortString(ddlFieldFifth.SelectedValue.ToInt(),
                                                optAscOrderFifth.Checked ? Constants.S_ASCENDING : Constants.S_DESCENDING));

        if (ddlFieldSix.SelectedIndex != Constants.I_ZERO)
            oStringBuilder.Append(GetSortString(ddlFieldSix.SelectedValue.ToInt(),
                                                optAscOrderSix.Checked ? Constants.S_ASCENDING : Constants.S_DESCENDING));

        return oStringBuilder.ToString();
    }

    /// <summary>
    /// This method is used to return sorted field according to selection .
    /// </summary>
    private string GetSortString(int iIndex, string sDirection)
    {
        string sFilter = String.Empty;
        switch (iIndex)
        {
            case 1:
                sFilter = "vw_BaseStudentDetails.First_Name " + sDirection + ",";
                break;
            case 2:
                sFilter = "vw_BaseStudentDetails.Middle_Name " + sDirection + ",";
                break;
            case 3:
                sFilter = "vw_BaseStudentDetails.Last_Name " + sDirection + ",";
                break;
             case 4:
                sFilter = "vw_BaseStudentDetails.Sex " + sDirection + ",";
                break;
            case 5:
                sFilter = "Category_Master.Category_Name " + sDirection + ",";
                break;
            case 6:
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
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnShow, btnGenerate });
        btnGenerate.Attributes.Add("Onclick", "if(!(DuplicateField())){return false;}");
    }

    /// <summary>
    /// Generate XML for the RollNos order.
    /// </summary>
    /// <returns></returns>
    private string GenerateStudentsRollNosXML()
    {
        const string S_ELEMENT = "element";
        var oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StudentsRollNosCollection");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentsRollNosCollection", "");

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount < grdStudents.Rows.Count; iRowCount++)
        {
            var oTextBox = grdStudents.Rows[iRowCount].Cells[3].FindControl("txtNewRollNo") as TextBox;
            // Create root xml element.
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentsRollNos", "");

            string sAtrrName = "YearWise_Student_Id";
            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = grdStudents.DataKeys[iRowCount]["YearWise_Student_Id"].ToString();
            oXmlNode.Attributes.Append(attr);

            sAtrrName = "RollNo";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = oTextBox.Text;
            oXmlNode.Attributes.Append(attr);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);

        }
        // Add the root node to document element.
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }
    /// <summary>
    /// This method used to refresh value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        hidPleaseSelectDifferentFieldsForGeneratingRollNumber.Value = Resources.LocalizedResources.PleaseSelectDifferentFieldsForGeneratingRollNumber;
        hidPleaseSelectAtleastOneFieldForGeneratingRollNumber.Value = Resources.LocalizedResources.PleaseSelectAtleastOneFieldForGeneratingRollNumber;
        hidStandardAndDivisionShouldBeSelected.Value = Resources.LocalizedResources.StandardAndDivisionShouldBeSelected;
        hidAll.Value = Resources.LocalizedResources.All;
        hidItsAll.Value = Resources.LocalizedResources.ItsAll;
        hidStandardsAnd.Value = Resources.LocalizedResources.StandardsAnd;
        hidDivisionsAreYouSureYouWantToContinue.Value = Resources.LocalizedResources.DivisionsAreYouSureYouWantToContinue; 
        hidAndDivision.Value = Resources.LocalizedResources.AndDivision;
        hidand.Value = Resources.LocalizedResources.And;
        hidYouAreUpdatingRollNumbersOfStandard.Value = Resources.LocalizedResources.YouAreUpdatingRollNumbersOfStandard;
        hidYouAreUpdatingRollNumbersOf.Value = Resources.LocalizedResources.YouAreUpdatingRollNumbersOf;
        hidAreYouSureYouWantToContinue.Value = Resources.LocalizedResources.ValRegenerateMsg;
        HidDotInMarathi.Value = Resources.LocalizedResources.HidDotInMarathi;
        hidValMsg.Value = Resources.LocalizedResources.Rollnumbers;
        hidValJsForRollNo.Value = Resources.LocalizedResources.ValJsForRollNo;
        hidValjsDuplicate.Value = Resources.LocalizedResources.ValjsDuplicate;
        hidMsgStandard.Value = Resources.LocalizedResources.MsgStandard;
        btnShow.Text = oResourceManager.GetString(hidbtnShow.Value.Replace(" ", string.Empty));
    }
    #endregion -- PRIVATE METHOD(s) --
    
}