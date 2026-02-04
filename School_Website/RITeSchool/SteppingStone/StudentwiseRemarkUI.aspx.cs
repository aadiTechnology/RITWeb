
// File Name  : StudentwiseRemarkUI.aspx.cs
// Created By : Vinod
// Date       : 12 Dec 11
// Description: This class is used save student remark details.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using System.IO;
using System.Data;
using System.Xml.Serialization;
using ProgressReportEntities;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class StudentwiseRemarkUI : SchoolBase
{
    #region Members

    public static List<StudentwiseRemarkConfigDetails> lstStudentwiseRemarkConfigDetails;
    public static List<RemarkMaster> lstRemarkMaster;


    //const int I_STUDENT_PROGRESS_REPORT = 82;
    #endregion

    #region Events

    /// <summary>
    /// This event is used to set session variable values.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {            
            base.OnInit(e);
            if (cmbTeachers.ClientID != null)
                BindListViewTemplate(Convert.ToInt32(Request.Params[cmbTeachers.ClientID.Replace("_", "$")]), Convert.ToInt32(Request.Params[cmbStudents.ClientID.Replace("_", "$")]), Convert.ToInt32(Request.Params[cmbTermName.ClientID.Replace("_", "$")]));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill all combobox and set javascripts attributes to controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                cmbTeachers.Focus();
                valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                FillTermComboBox();
                FillTeachersComboBox();
            }
            SetDefaultValues();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill student remark listbox of peraticular class teacher.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTeachers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DtPgCount.SetPageProperties(0, Constants.I_GRID_PAGE_COUNT, false);
            btnSave.Enabled = Convert.ToInt32(cmbTeachers.SelectedValue) == Constants.I_ZERO ? false : true;
            DataTable oDTStudents = GetStudentDataTable(Convert.ToInt32(cmbTeachers.SelectedValue));
            FillStudentsComboBox(oDTStudents);
            BindListViewTemplate();
            BindListViewData();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill student remark listbox of peraticular term.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTermName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DtPgCount.SetPageProperties(0, Constants.I_GRID_PAGE_COUNT, false);
            DisplayStudentList(Convert.ToInt32(cmbTeachers.SelectedValue));
            BindListViewTemplate();
            BindListViewData();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill student remark listbox of peraticular student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStudents_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DtPgCount.SetPageProperties(0, Constants.I_GRID_PAGE_COUNT, false);
            BindListViewTemplate();
            BindListViewData();
            lblUpdateSucess.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill pager dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetDataPagerAccordingToPageNo();
            DropDownList ddlCnt = (DtPgDropDown.Controls[0].FindControl("ddlCnt")) as DropDownList;
            hidPageNo.Value = (ddlCnt.SelectedIndex + 1).ToString();
            BindListViewTemplate();
            BindListViewData();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This event is used to bind data row wise.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentRemarkDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Label lblRollNo = e.Item.FindControl("lblRollNo") as Label;
                Label lblName = e.Item.FindControl("lblName") as Label;

                ListViewDataItem lstvwDataItem = e.Item as ListViewDataItem;
                if (hidRollNo.Value != ((StudentwiseRemarkConfigDetails)lstvwDataItem.DataItem).RollNo.ToString())
                {
                    if (lblRollNo != null)
                        lblRollNo.Text = ((StudentwiseRemarkConfigDetails)lstvwDataItem.DataItem).RollNo.ToString();
                    if (lblName != null)
                        lblName.Text = ((StudentwiseRemarkConfigDetails)lstvwDataItem.DataItem).StudentName;

                    for (int inum = 0; inum < lstRemarkMaster.Count; inum++)
                    {
                        TextBox oTextBox = e.Item.FindControl("txt" + lstRemarkMaster[inum].RemarkName) as TextBox;
                        if (oTextBox != null)
                            oTextBox.Text = ("txt" + ((StudentwiseRemarkConfigDetails)lstvwDataItem.DataItem).RemarkMaster.RemarkName) == oTextBox.ID ? ((StudentwiseRemarkConfigDetails)lstvwDataItem.DataItem).Remark : string.Empty;
                    }
                    var lstrem = lstStudentwiseRemarkConfigDetails.Where(s => s.RollNo == ((StudentwiseRemarkConfigDetails)lstvwDataItem.DataItem).RollNo).ToList();
                    int iCnt = 0;
                    if (lstrem.Count > 1)
                    {
                        for (int inum = 0; inum < lstrem.Count; inum++)
                        {
                            while (iCnt < lstRemarkMaster.Count)
                            {
                                TextBox txtRemarkName = e.Item.FindControl("txt" + lstRemarkMaster[iCnt].RemarkName) as TextBox;
                                if (txtRemarkName != null)
                                    if ("txt" + lstrem[inum].RemarkMaster.RemarkName == txtRemarkName.ID)
                                    {
                                        txtRemarkName.Text = lstrem[inum].Remark;
                                        break;
                                    }
                                iCnt++;
                            }
                        }
                    }
                }
                hidRollNo.Value = ((StudentwiseRemarkConfigDetails)lstvwDataItem.DataItem).RollNo.ToString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save the remark of student in database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveRemarkDetails();
            lblUpdateSucess.Visible = true;
            lblUpdateSucess.Text = "Progress remarks saved successfully !!!";
            BindListViewTemplate();
            BindListViewData();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    ///  This class is used to create list view template dynamically.
    /// </summary>
    /// 
    public class CustomeTemplate : ITemplate
    {
        ListViewItemType _lstvwItemType;
        List<StudentwiseRemarkConfigDetails> _lstRemarkDetails;
        bool _isAlterNate = false;

        public CustomeTemplate(ListViewItemType lstItemType, List<StudentwiseRemarkConfigDetails> lstRemarkDetails, bool isAlterNate)
        {
            _lstvwItemType = lstItemType;
            _lstRemarkDetails = lstRemarkDetails;
            _isAlterNate = isAlterNate;
        }

        public void InstantiateIn(Control container)
        {
            if (_lstvwItemType == ListViewItemType.DataItem)
            {
                Literal ltrlDataItemTr = new Literal();
                Literal ltrlDataItemTd = new Literal();
                Label lblRollNo = new Label();
                Literal ltrlDataItemName = new Literal();
                Literal ltrlDataItemTdClose = new Literal();
                Literal ltrlDataItemTrClose = new Literal();

                ltrlDataItemTr.Text = _isAlterNate == false ? "<tr class='ClsGridRow'>" : "<tr class='ClsGridAltRow'>";
                ltrlDataItemTd.Text = "<td align ='center' width='60px'>";
                lblRollNo.ID = "lblRollNo";
                ltrlDataItemTrClose.Text = "</td>";

                container.Controls.Add(ltrlDataItemTr);
                container.Controls.Add(ltrlDataItemTd);
                container.Controls.Add(lblRollNo);
                container.Controls.Add(ltrlDataItemTrClose);

                Literal ltrlDataItemTdName = new Literal();
                Label lblName = new Label();
                Literal ltrlDataItemTdNameClose = new Literal();

                ltrlDataItemTdName.Text = "<td style='padding-left:8px' width='300px'>";
                lblName.ID = "lblName";
                ltrlDataItemTdNameClose.Text = "</td>";
                ltrlDataItemTrClose.Text = "</tr>";

                container.Controls.Add(ltrlDataItemTdName);
                container.Controls.Add(lblName);
                container.Controls.Add(ltrlDataItemTdNameClose);
                container.Controls.Add(ltrlDataItemTrClose);

                for (int iNo = 0; iNo < lstRemarkMaster.Count; iNo++)
                {
                    Literal ltrltd = new Literal();
                    Literal ltrtdClose = new Literal();
                    ltrltd.Text = "<td align = 'center'>";
                    ltrtdClose.Text = "</td>";

                    TextBox txtId = new TextBox();
                    txtId.ID = "txt" + lstRemarkMaster[iNo].RemarkName;
                    txtId.Width = Unit.Pixel(200);
                    txtId.TextMode = TextBoxMode.MultiLine;

                    container.Controls.Add(ltrltd);
                    container.Controls.Add(txtId);
                    container.Controls.Add(ltrtdClose);
                }
                container.Controls.Add(ltrlDataItemTrClose);
            }
            else
            {
                Literal ltrlHeadertbl = new Literal();
                ltrlHeadertbl.Text = "<table cellpadding='0' cellspacing='1' style='color: #333333' class='GridBorder' align='center'>";
                ltrlHeadertbl.Text += "<tr class='ClsGridHeader'><th align='center'>Roll No.</th><th align='left' style='padding-left:8px'>Name</th>";

                Literal ltrthClose = new Literal();
                ltrthClose.Text = "</th>";

                Literal ltrlHeadertrClose = new Literal();
                ltrlHeadertrClose.Text = "</tr>";

                container.Controls.Add(ltrlHeadertbl);

                for (int iNo = 0; iNo < lstRemarkMaster.Count; iNo++)
                {
                    Literal ltrlthHeader = new Literal();
                    ltrlthHeader.Text = "<th align='center'>" + lstRemarkMaster[iNo].RemarkName.ToString() + "</th>";
                    container.Controls.Add(ltrlthHeader);
                }
                container.Controls.Add(ltrlHeadertrClose);

                Literal ltrlItemPlaceHolder = new Literal();
                ltrlItemPlaceHolder.ID = "itemPlaceholder";
                Literal ltrlHeadertblClose = new Literal();
                ltrlHeadertblClose.Text = "</table>";

                container.Controls.Add(ltrlItemPlaceHolder);
                container.Controls.Add(ltrlHeadertblClose);
            }
        }
    }

    /// <summary>
    /// This method is used to save remark details.
    /// </summary>
    private void SaveRemarkDetails()
    {
        StudentwiseRemarkMasterBL oStudentwiseRemarkMasterBL = new StudentwiseRemarkMasterBL();
        int iStandardDivId = Convert.ToInt32(lstvwStudentRemarkDetails.DataKeys[0]["StandardDivisionId"]);
        oStudentwiseRemarkMasterBL.UpdateStudentwiseRemarkDetails(GenerateXml(PopulateStudentwiseRemarkObject()), miSchoolId, miAcademicYearId,miUserId, iStandardDivId, Convert.ToInt32(cmbTermName.SelectedValue));
    }

    /// <summary>
    /// This method is used to set default values. 
    /// </summary>
    private void SetDefaultValues()
    {
        hidRollNo.Value = string.Empty;
        cmbTeachers.Focus();
        btnSave.Enabled = Convert.ToInt32(cmbTeachers.SelectedValue) == Constants.I_ZERO ? false : true;
        lblNorecord.Visible = false;
    }

    /// <summary>
    /// This event is used to fill teacher combo box.
    /// </summary>
    private void FillTeachersComboBox()
    {
        //get all class teachers
        DataTable oDt = SchoolWiseStandardDivisionTeacherAssignmentMasterBL.GetAllClassTeachers(miSchoolId,miAcademicYearId);
        ControlUtility.FillDropDownList(oDt, ref cmbTeachers,
                                            Constants.S_TEACHER_ID_FIELD,
                                             Constants.S_TEACHER_NAME_FIELD,
                                             Constants.S_SELECT);
        if (moUserRole == Constants.UserRoles.Teacher)
        {
            cmbTeachers.SelectedValue = Session[Constants.S_SESSION_TEACHER_ID].ToString();
            cmbTeachers.Enabled = false;
            DataTable oDTStudents = GetStudentDataTable(Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]));
            FillStudentsComboBox(oDTStudents);
        }
    }

    /// <summary>
    ///  This method isused to get all student list of selected teacher.
    /// </summary>
    /// <param name="iTeacherId"></param>
    /// <returns></returns>
    private DataTable GetStudentDataTable(int iTeacherId)
    {
        StudentwiseRemarkMasterBL oStudentwiseRemarkMasterBL = new StudentwiseRemarkMasterBL();
        DataTable oDSStudentsList = oStudentwiseRemarkMasterBL.GetStudentListOfGivenClassTeacher(iTeacherId,miAcademicYearId,miSchoolId, Convert.ToInt32(cmbTermName.SelectedValue));
        return oDSStudentsList;
    }

    /// <summary>
    /// This methd is used to fill term combobox.
    /// </summary>
    private void FillTermComboBox()
    {
        DataTable oDataTable = StudentwiseRemarkMasterBL.GetTestwiseTerm(miSchoolId);
        ControlUtility.FillDropDownList(oDataTable, ref cmbTermName,
                                       "Value_Member",
                                      "Display_Member",
                                      string.Empty);
    }

    /// <summary>
    /// This event is used to fill student combo box.
    /// </summary>
    /// <param name="aoDtStudent"></param>
    private void FillStudentsComboBox(DataTable aoDtStudent)
    {
        ControlUtility.FillDropDownList(aoDtStudent, ref cmbStudents,
                                                "Student_Id",
                                                "Student_Name",
                                                Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to create list view template dynamically.
    /// </summary>
    /// <param name="aTeacherId"></param>
    /// <param name="aiStudentId"></param>
    /// <param name="aiTermId"></param>
    private void BindListViewTemplate(int aTeacherId, int aiStudentId, int aiTermId)
    {
        StudentwiseRemarkMasterBL oStudentwiseRemarkMasterBL = new StudentwiseRemarkMasterBL();
        oStudentwiseRemarkMasterBL.GetStudentwiseRemarkConfigDetails(miSchoolId, miAcademicYearId, aTeacherId, aiStudentId, aiTermId);
        lstStudentwiseRemarkConfigDetails = oStudentwiseRemarkMasterBL.olstStudentwiseRemarkConfigDetails;
        lstRemarkMaster = oStudentwiseRemarkMasterBL.olstRemarkMaster;
        int iListCount = lstStudentwiseRemarkConfigDetails.Count;
        if (lstRemarkMaster.Count > 0)
        {
            trNorecordFound.Visible = false;
            trListView.Visible = btnSave.Enabled = true;
            lstvwStudentRemarkDetails.LayoutTemplate = new CustomeTemplate(ListViewItemType.EmptyItem, lstStudentwiseRemarkConfigDetails, false);
            lstvwStudentRemarkDetails.ItemTemplate = new CustomeTemplate(ListViewItemType.DataItem, lstStudentwiseRemarkConfigDetails, false);
            lstvwStudentRemarkDetails.AlternatingItemTemplate = new CustomeTemplate(ListViewItemType.DataItem, lstStudentwiseRemarkConfigDetails, true);
        }
        else
        {
            lstvwStudentRemarkDetails.LayoutTemplate = new CustomeTemplate(ListViewItemType.EmptyItem, lstStudentwiseRemarkConfigDetails, false);
            btnSave.Enabled = trListView.Visible = trPagerUser.Visible = false;
            trNorecordFound.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to create list view template dynamically.
    /// </summary>
    /// <param name="aTeacherId"></param>
    /// <param name="aiStudentId"></param>
    /// <param name="aiTermId"></param>
    private void BindListViewTemplate()
    {
        StudentwiseRemarkMasterBL oStudentwiseRemarkMasterBL = new StudentwiseRemarkMasterBL();
        oStudentwiseRemarkMasterBL.GetStudentwiseRemarkConfigDetails(miSchoolId, miAcademicYearId, Convert.ToInt32(cmbTeachers.SelectedValue), Convert.ToInt32(cmbStudents.SelectedValue), Convert.ToInt32(cmbTermName.SelectedValue));
        lstStudentwiseRemarkConfigDetails = oStudentwiseRemarkMasterBL.olstStudentwiseRemarkConfigDetails;
        lstRemarkMaster = oStudentwiseRemarkMasterBL.olstRemarkMaster;
        hidRemarkListCount.Value = lstRemarkMaster.Count.ToString();

        if (lstRemarkMaster.Count == 0)
            btnSave.Enabled = trListView.Visible = trPagerUser.Visible = false;

        lstvwStudentRemarkDetails.LayoutTemplate = new CustomeTemplate(ListViewItemType.EmptyItem, lstStudentwiseRemarkConfigDetails, false);
        lstvwStudentRemarkDetails.ItemTemplate = new CustomeTemplate(ListViewItemType.DataItem, lstStudentwiseRemarkConfigDetails, false);
        lstvwStudentRemarkDetails.AlternatingItemTemplate = new CustomeTemplate(ListViewItemType.DataItem, lstStudentwiseRemarkConfigDetails, true);
        for (int iNo = 0; iNo < lstRemarkMaster.Count; iNo++)
            hidRemarkNameList.Value = hidRemarkNameList.Value == string.Empty ? lstRemarkMaster[iNo].RemarkName : hidRemarkNameList.Value + "," + lstRemarkMaster[iNo].RemarkName;
    }

    /// <summary>
    /// This method is used to display Student combo box vales on page load.
    /// </summary>
    /// <param name="miSchoolId"></param>
    private void DisplayStudentList(int iTeacherId)
    {
        DataTable oDTStudents = GetStudentDataTable(iTeacherId);
        FillStudentsComboBox(oDTStudents);
    }

    /// <summary>
    /// This method is used to bind data to list view.
    /// </summary>
    private void BindListViewData()
    {
        lstvwStudentRemarkDetails.DataSource = GenaratelstDistinctStudentwiseRemarkConfigList();
        lstvwStudentRemarkDetails.DataBind();
        FillListViewPagerFooter();
        DropDownList ddlCnt = (DtPgDropDown.Controls[0].FindControl("ddlCnt")) as DropDownList;
        ddlCnt.Attributes.Add("onchange", "if(!MessageAlert('" + ddlCnt.ClientID + "')){return false;}");
        hidListviewPageRowCnt.Value = lstvwStudentRemarkDetails.Items.Count.ToString();
        tdPgr.Width = (375 + (200 * (lstRemarkMaster.Count))).ToString() + "px";
    }

    /// <summary>
    /// This method is used to fill data pager.
    /// </summary>
    private void FillListViewPagerFooter()
    {
        tblDataPager.Visible = trPagerUser.Visible = false;
        int iCurrPage = (DtPgDropDown.StartRowIndex / DtPgDropDown.PageSize) + 1;
        int iTotalPage = DtPgDropDown.TotalRowCount / DtPgDropDown.PageSize;
        if (iTotalPage * DtPgDropDown.PageSize < DtPgDropDown.TotalRowCount)
            iTotalPage += 1;

        if (iTotalPage > 1)
        {
            tblDataPager.Visible = trPagerUser.Visible = true;
            DropDownList ddlCnt = DtPgDropDown.Controls[0].FindControl("ddlCnt") as DropDownList;
            if (ddlCnt.Items.Count == Constants.I_ZERO)
            {
                for (int iNo = 1; iNo <= iTotalPage; iNo++)
                    ddlCnt.Items.Add(iNo.ToString());
                //Set the DDL to the appropriate page value
                ddlCnt.Items.FindByValue(iCurrPage.ToString()).Selected = true;
                Label lblCurrentPageLabel = (DtPgDropDown.Controls[0].FindControl("CurrentPageLabel")) as Label;
                lblCurrentPageLabel.Font.Bold = true;
                lblCurrentPageLabel.Text = "Page " + iCurrPage + " of " + iTotalPage;
            }
        }
    }

    /// <summary>
    /// This method is used to fill dictinct student list view.
    /// </summary>
    /// <returns></returns>
    public List<StudentwiseRemarkConfigDetails> GenaratelstDistinctStudentwiseRemarkConfigList()
    {
        List<StudentwiseRemarkConfigDetails> lstDistinctStudentwiseRemarkConfigDetails = new List<StudentwiseRemarkConfigDetails>();
        foreach (StudentwiseRemarkConfigDetails student in lstStudentwiseRemarkConfigDetails)
        {
            if (lstDistinctStudentwiseRemarkConfigDetails.Where(sRollNo => sRollNo.RollNo == student.RollNo).ToList().Count == Constants.I_ZERO)
                lstDistinctStudentwiseRemarkConfigDetails.Add(student);
        }
        hidStudentwiaseRemarkListCount.Value = lstDistinctStudentwiseRemarkConfigDetails.Count.ToString();
        return lstDistinctStudentwiseRemarkConfigDetails;

    }
    
    /// <summary>
    /// This method is used to Populate Studentwise Remark Object.
    /// </summary>
    /// <returns></returns>
    public List<StudentwiseRemarkConfigDetails> PopulateStudentwiseRemarkObject()
    {
        List<StudentwiseRemarkConfigDetails> lstStudentwiseRemarkConfigDetail = new List<StudentwiseRemarkConfigDetails>();
        StudentwiseRemarkConfigDetails oStudentwiseRemarkConfigDetails;
        RemarkMaster oRemarkMaster;

        foreach (ListViewDataItem oCurrentItem in lstvwStudentRemarkDetails.Items)
        {
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            int iCnt = 0;
            while (iCnt < lstRemarkMaster.Count)
            {
                TextBox oTxtRemark = (TextBox)oCurrentItem.FindControl("txt" + lstRemarkMaster[iCnt].RemarkName);
                if (oTxtRemark != null)
                {
                    if (oTxtRemark.Text.Trim() != string.Empty)
                    {
                        oStudentwiseRemarkConfigDetails = new StudentwiseRemarkConfigDetails();

                        oStudentwiseRemarkConfigDetails.YearwiseStudentId = Convert.ToInt32(lstvwStudentRemarkDetails.DataKeys[iRowId]["YearwiseStudentId"]);
                        oStudentwiseRemarkConfigDetails.StudentwiseRemarkId = Convert.ToInt32(lstvwStudentRemarkDetails.DataKeys[iRowId]["StudentwiseRemarkId"]);
                        oStudentwiseRemarkConfigDetails.Remark = oTxtRemark.Text.Trim();
                        oRemarkMaster = new RemarkMaster()
                        {
                            RemarkConfigId = lstRemarkMaster[iCnt].RemarkConfigId
                        };
                        oStudentwiseRemarkConfigDetails.RemarkMaster = oRemarkMaster;

                        lstStudentwiseRemarkConfigDetail.Add(oStudentwiseRemarkConfigDetails);
                    }
                }
                iCnt++;
            }
        }
        return lstStudentwiseRemarkConfigDetail;
    }

    /// <summary>
    /// This method is used to set JavaScript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ApplyMouseHoverEffect(new List<Button>{btnBack,btnSave});
    }

    /// <summary>
    /// This method is used to set list view according selected page from the pager dropdownlist.
    /// Pager control name should be same as defined here.
    /// e.g. DtPgDropDown is the datapager name which contains the drop down list.
    /// Same for drop down list in the pager control as well as label
    /// </summary>
    public void SetDataPagerAccordingToPageNo()
    {
        DropDownList oddlCnt = (DtPgDropDown.Controls[0].FindControl("ddlCnt")) as DropDownList;
        int iRowIndex = (Convert.ToInt32(oddlCnt.SelectedValue) - 1) * DtPgDropDown.PageSize;

        DtPgDropDown.SetPageProperties(iRowIndex, DtPgDropDown.PageSize, true);

        int icurrentPage = (DtPgDropDown.StartRowIndex / DtPgDropDown.PageSize) + 1;
        int itotalPages = DtPgDropDown.TotalRowCount / DtPgDropDown.PageSize;

        Label lblCurrentPageLabel = (DtPgDropDown.Controls[0].FindControl("CurrentPageLabel")) as Label;
        lblCurrentPageLabel.Text = "Page " + icurrentPage + " of " + itotalPages;
    }

    #endregion
}