
/* This class is used for following purpose :-
 * 1)To display list of students.
 * 2)To allows user to edit students roll number."
 * Author: Shankar Gurav.
 * Date of creation: 08 July 2009
 * Date of modification: -
 */
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Utility;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using System.IO;
using System.Text;
using System.Threading;
using System.Web.UI.HtmlControls;
using System.Xml;

public partial class StudentRollNosGeneration : SchoolBase
{
    #region constants

    const Int32 I_COL_INDEX_ROLL_NO = 1;
    const string S_BLANK_GRID_MESSAGE = "No student available.";

    #endregion
    
    #region constants

    const string S_ROWCMD_DELETE_STUDENT = "DELETE_STUDENT";

    #endregion constants

    #region event handlers

    /// <summary>
    /// This event is used to perform operations at page load.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                InitializeFields();
                CheckRoleAndAssignDisplayView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set default values fill division combobox on change of selected index.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStd_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidDivisionId.Value = "0";
            hidStandardId.Value = cmbStandard.SelectedValue;
            if (Convert.ToInt32(cmbStandard.SelectedValue) == Constants.I_ZERO)
            {
                cmbDivision.Items.Clear();
                cmbDivision.Items.Add(new ListItem(Constants.S_SELECT, "0"));
            }
            else
                FillDivisionCombobox();

            lstvwStudentList.Visible = false;
            btnSave.Visible = false;
            btnSaveUp.Visible = false;
            btnBackUp.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to perform operations after changing the selected index.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidDivisionId.Value = cmbDivision.SelectedValue;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to go back on all student list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master; oMasterPage.RedirectToNextPage("~/Admin/AllStudentsUI.aspx");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search student information according to reg. no or name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillStudentGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to generate xml for students roll numbers and save bulk data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        // Save the changes to database.
        try
        {
            string sXmlStudentsRollNos = GenerateStudentsRollNosXML();
            StudentBL oStudentBL = new StudentBL();            
            int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
            int iDivisionId = Convert.ToInt32(cmbDivision.SelectedValue);
            oStudentBL.UpdateStudentsRollNos(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, sXmlStudentsRollNos);
            lblMessage.Text = "Roll numbers updated successfully !!!";
            tblMassage.Visible = true;
            FillStudentGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
       
    #region grid events

    protected void lstvwStudentList_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        ListViewDataItem oListViewDataItem = (ListViewDataItem)e.Item;
        TextBox oTextBox = (TextBox)e.Item.FindControl("txtNewRoll_No");
        oTextBox.Attributes.Add("onblur", "extractNumber(this,0,false)");
        oTextBox.Attributes.Add("onkeyup", "extractNumber(this,0,false)");
        oTextBox.Attributes.Add("onkeypress", "return blockNonNumbers (this, event, false, false);");
        oTextBox.Attributes.Add("onpaste", "event.returnValue=false");
        oTextBox.Attributes.Add("ondrop", "event.returnValue=false");
        if (lstvwStudentList.DataKeys[oListViewDataItem.DisplayIndex][1] != DBNull.Value)
        {
            HtmlTableRow oHtmlTableRow = (HtmlTableRow)e.Item.FindControl("trStudentRow");
            oHtmlTableRow.Style.Add(HtmlTextWriterStyle.Color, "red");
            oTextBox.Visible = false;
            Label oLabel = (Label)e.Item.FindControl("lblNewRoll_No");
            oLabel.Visible = true;
        }
    }

    #endregion

    #endregion

    #region helper methods
    /// <summary>
    /// This method is used to check if the login user is of superviser role and 
    /// check the access he have
    /// </summary>
    private void CheckRoleAndAssignDisplayView()
    {
        if (moUserRole== Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
            hidCanEdit.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.Student).ToString();
    }

    ///// <summary>
    ///// This function is used to bind grid according to the search criteria
    ///// </summary>
    ///// <param name="aiSchoolId"></param>
    private void FillStudentGrid()
    {
        lstvwStudentList.Visible = true;        
        int iStandardId = Convert.ToInt32(cmbStandard.SelectedValue);
        int iDivisionId = Convert.ToInt32(cmbDivision.SelectedValue);

        StudentBL oStudentBL = new StudentBL();
        DataTable oDataTable = oStudentBL.GetAllStudents(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, string.Empty, string.Empty, 1000, 0);
        lstvwStudentList.DataSource = oDataTable;
        lstvwStudentList.DataBind();
        if (lstvwStudentList.Items.Count > 0)
        {
            btnSave.Visible = true;
            btnSaveUp.Visible = true;
            btnBackUp.Visible = true;
        }
        btnSave.Attributes.Add("onClick", "if(!ValidatePage('txtNewRoll_No','lblNewRoll_No','lblEnrollNo','" + lstvwStudentList.ClientID + "','" + lstvwStudentList.Items.Count + "')) return false;");
        btnSaveUp.Attributes.Add("onClick", "if(!ValidatePage('txtNewRoll_No','lblNewRoll_No','lblEnrollNo','" + lstvwStudentList.ClientID + "','" + lstvwStudentList.Items.Count + "')) return false;");
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
    /// This function is used to initialise field values
    /// </summary>
    private void InitializeFields()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSearch, btnSave, btnSaveUp, btnBack, btnBackUp });
        hidSortDirection.Value = Constants.S_ASCENDING;
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        
        if (moUserRole != Constants.UserRoles.Admin)
            hidUserHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.Student).ToString();
        
        ReadQuerystring();
        
        if (moUserRole == Constants.UserRoles.Admin || Boolean.Parse(hidUserHasFullAccess.Value))
        {
            FillStandardCombobox();
            if (cmbStandard.SelectedIndex > 0)
                FillDivisionCombobox();
        }
        
        DisplayFormFieldsAccordingToUser();
    }

    /// <summary>
    /// This method is used to display form fields according to user.
    /// </summary>
    private void DisplayFormFieldsAccordingToUser()
    {
        tdStandardDivisionLabel.Visible = false;
        tdStandardDivisionValue.Visible = false;

        if (moUserRole != Constants.UserRoles.Admin && moUserRole!= Constants.UserRoles.Supervisor && !Boolean.Parse(hidUserHasFullAccess.Value))
        {
            if (Session[Constants.S_SESSION_IS_CLASS_TEACHER] != null && Session[Constants.S_SESSION_IS_CLASS_TEACHER].ToString() == Constants.C_YES.ToString())
            {
                tblSearch.Visible = false;
                cmbDivision.Visible = false;
                cmbStandard.Visible = false;
                btnBack.Text = "Back";
                btnBackUp.Text = "Back";
                tdBack.Visible = true;
                SetStandardDivisionOfTeacher();
            }
            else
            {
                lblErrorMsg.Text = "Access denied! Only admin or class-teacher can access this page. ";
            }
        }
    }

    /// <summary>
    /// This method is used to set standard division of a teacher.
    /// </summary>
    private void SetStandardDivisionOfTeacher()
    {
        DataTable oDT = SchoolWiseStandardDivisionTeacherAssignmentMasterBL.GetStandardDivisionOfTeacher
                      (Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID].ToString()), miAcademicYearId);
        if (oDT.Rows.Count > 0)
        {
            string sStandardId = oDT.Rows[0]["standard_Id"].ToString();
            string sDivisionId = oDT.Rows[0]["division_Id"].ToString();
            string sStandardName = oDT.Rows[0]["standard_Name"].ToString();
            string sDivisionName = oDT.Rows[0]["division_Name"].ToString();
            hidStandardId.Value = oDT.Rows[0]["standard_Id"].ToString();
            hidDivisionId.Value = oDT.Rows[0]["division_Id"].ToString();
            tdStandardDivisionLabel.Visible = true;
            tdStandardDivisionValue.Visible = true;
            lblStandardDivisionValue.Text = sStandardName + "-" + sDivisionName;
        }
    }

    /// <summary>
    /// This function is used to fill combobox with all standards available in current school.
    /// </summary>
    private void FillStandardCombobox()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDtStandardCollection, ref cmbStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT);
        cmbStandard.SelectedValue = hidStandardId.Value;
    }

    /// <summary>
    /// This function is used to fill Division combobox.
    /// </summary>
    /// <param name="aiStandardId"></param>    
    private void FillDivisionCombobox()
    {
        int aiStandardId = Convert.ToInt32(hidStandardId.Value);        
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(aiStandardId);

        ControlUtility.FillDropDownList(oDSStandardCollection, ref cmbDivision,
                                       Constants.S_DIVISION_ID_FIELD,
                                       Constants.S_DIVISION_NAME_FIELD,
                                       string.Empty);
        if (Convert.ToInt32(hidDivisionId.Value) != 0)
            cmbDivision.SelectedValue = hidDivisionId.Value;
    }

    /// <summary>
    /// This method is used to decrypt the encrypted querystring.
    /// </summary>       
    private void ReadQuerystring()
    {
        try
        {
	        hidStandardId.Value = QueryString["StandardId"] ?? Constants.S_ZERO;
	        hidDivisionId.Value = QueryString["DivisionId"] ?? Constants.S_ZERO;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Generate XML for the RollNos order.
    /// </summary>
    /// <returns></returns>
    private string GenerateStudentsRollNosXML()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StudentsRollNosCollection");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentsRollNosCollection", "");

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount < lstvwStudentList.Items.Count; iRowCount++)
        {
            TextBox oTextBox = (TextBox)lstvwStudentList.Items[iRowCount].FindControl("txtNewRoll_No");
            // Create root xml element.
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentsRollNos", "");

            string sAtrrName = "YearWise_Student_Id";
            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = lstvwStudentList.DataKeys[iRowCount][0].ToString();
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

    #endregion
}