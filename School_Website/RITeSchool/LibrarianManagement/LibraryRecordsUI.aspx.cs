using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using Utility;
using SchoolEntities;
using System.Reflection;
using BookEntities;
using System.Data.SqlClient;

public partial class LibraryRecordsUI : SchoolBase
{
    #region  " Constants "

    private const string S_ISSUE_MESSAGE = "Books issued successfully !!!";
    private const string S_RETURN_MESSAGE = "Books retured successfully !!!";
    private const string S_TIME_FORMAT = "hh:mm tt";

    #endregion

    #region "DataMember's"

    private LibraryRecordsBL moLibraryRecordsBL;
        
    #endregion

    #region "Event's"

    /// <summary>
    /// This event is used to load default controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moLibraryRecordsBL = new LibraryRecordsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                FillStandards();
                FillDivisions();
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used Item Data bound.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudents_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;

                CheckBox chkSelect = e.Item.FindControl("chkSelect") as CheckBox;
                chkSelect.Attributes.Add("onclick", "EnableDisableRow(" + e.Item.DisplayIndex + ")");
                CheckBox chkIsAbsent = e.Item.FindControl("chkIsAbsent") as CheckBox;
                chkIsAbsent.Attributes.Add("onclick", "SetAccessionNoState(" + e.Item.DisplayIndex + ")");

                HiddenField hidIsAbsent = e.Item.FindControl("hidIsAbsent") as HiddenField;
                
                bool bIsChecked = Convert.ToBoolean(lstvwStudents.DataKeys[oCurrentItem.DisplayIndex]["IsAbsent"]);


                hidIsAbsent.Value = Constants.S_ZERO;
                if(bIsChecked == true)
                {
                    chkIsAbsent.Checked = bIsChecked;
                    hidIsAbsent.Value = Constants.S_ONE;
                }
            }
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to issue books.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnIssue_Click(object sender, EventArgs e)
    {
        try
        {
            LibraryRecordsBL oLibraryRecordsBL = new LibraryRecordsBL(miSchoolId, miAcademicYearId, miUserId);
            string sXmlBookIssueDetails = PopulateBookDetails();
            if (sXmlBookIssueDetails != string.Empty)
            {
                DateTime dtIssueReturnDateTime = Convert.ToDateTime(txtIssueReturnDate.Text + " " + txtIssueReturnTime.Text.ToString());
                moLibraryRecordsBL.SaveBookDetails(sXmlBookIssueDetails, dtIssueReturnDateTime, Constants.I_ONE);
                FillStudents();
                base.DisplayMessage(S_ISSUE_MESSAGE, false, tdMessage);
            }
        }
        catch (ApplicationException ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (SqlException ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event is used for return books.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReturn_Click(object sender, EventArgs e)
    {
        try
        {
            LibraryRecordsBL oLibraryRecordsBL = new LibraryRecordsBL(miSchoolId, miAcademicYearId, miUserId);
            string sXmlBookIssueDetails = PopulateBookDetails();
            if (sXmlBookIssueDetails != string.Empty)
            {
                DateTime dtIssueReturnDateTime = Convert.ToDateTime(txtIssueReturnDate.Text + " " + txtIssueReturnTime.Text);
                moLibraryRecordsBL.SaveBookDetails(sXmlBookIssueDetails, dtIssueReturnDateTime, Constants.I_TWO);
                FillStudents();
                base.DisplayMessage(S_RETURN_MESSAGE, false, tdMessage);
            }
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used to Fill Division combobox as per selected standard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandards_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillDivisions();
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is Used to Display Student Details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            FillStudents();
        }
        catch (Exception ex)
        {
            BusinessLogic.Exceptions.ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region"Private Method's"

    /// <summary>
    /// This Method is used to Populate Book details For Save.
    /// </summary>
    /// <returns></returns>
    private string PopulateBookDetails()
    {
        List<LibraryRecord> lstLibraryRecord = new List<LibraryRecord>();
        int i = Constants.I_ZERO;
        int iCount = lstvwStudents.Items.Count;
        string sRowNo = string.Empty;
        string sIssueTimeError = string.Empty;
        while (i < lstvwStudents.Items.Count)
        {
            int iId = lstvwStudents.DataKeys[i]["Id"].ToInt();
            TextBox txtBookAssessionNo = (TextBox)lstvwStudents.Items[i].FindControl("txtAccessionNo");
            TextBox txtComment = (TextBox)lstvwStudents.Items[i].FindControl("txtRemark");
            CheckBox chkSelect = (CheckBox)lstvwStudents.Items[i].FindControl("chkSelect");
            CheckBox chkAbsent = (CheckBox)lstvwStudents.Items[i].FindControl("chkIsAbsent");
            if (chkSelect.Checked)
            {
                if (txtBookAssessionNo.Text != string.Empty || chkAbsent.Checked)
                {
                    LibraryRecord oLibraryRecord = new LibraryRecord();
                    oLibraryRecord.Id = iId;
                    oLibraryRecord.UserId = Convert.ToInt32(lstvwStudents.DataKeys[i]["UserId"]);
                    oLibraryRecord.BookNo = txtBookAssessionNo.Text;
                    oLibraryRecord.Comment = txtComment.Text;
                    oLibraryRecord.IsAbsent = chkAbsent.Checked;

                    lstLibraryRecord.Add(oLibraryRecord);
                }
                else
                {
                    int iValue = i;
                    iValue = iValue + 1;
                    sRowNo = sRowNo + "," + iValue;
                }
            }
            i++;
        }
       if (sRowNo != string.Empty)
       {
           sRowNo = sRowNo.Substring(Constants.I_ONE);
           throw new ApplicationException("Please enter Accession No. for row no. - " + sRowNo);
       }

        if (lstLibraryRecord.Count > Constants.I_ZERO)
            return base.GenerateXml(lstLibraryRecord);
        else
            return string.Empty;
    }

    /// <summary>
    /// This USP is used to set Javascript attributes to controls.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnIssue, btnReturn});
        ValSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ValSumIssue.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ValSumReturn.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        txtShowDate.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);

        btnIssue.Attributes.Add("onclick","ClearMessage()");
        btnReturn.Attributes.Add("onclick", "ClearMessage()");
        btnShow.Attributes.Add("onclick", "ClearMessage()");
    }

    /// <summary>
    /// This Method is used to fill Division ComboBox.
    /// </summary>
    private void FillDivisions()
    {
       DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId,miAcademicYearId);
       DataTable odtDivisions = oDivisionCollectionBL.GetAllDivisionsForStandard(cmbStandards.SelectedValue.ToInt());
        ControlUtility.FillDropDownList(odtDivisions, ref cmbDivisions,
                                       Constants.S_DIVISION_ID_FIELD,
                                       Constants.S_DIVISION_NAME_FIELD,
                                       Constants.S_SELECT);
    }

    /// <summary>
    /// This Method is used to fill Standard combobox.
    /// </summary>
    private void FillStandards()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable odtStadnards = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(odtStadnards, ref cmbStandards,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       Constants.S_SELECT);
    }   

    /// <summary>
    /// This Method is used to FillStudents ListView.
    /// </summary>
    private void FillStudents()
    {
        List<LibraryRecord> lstLibraryRecords = moLibraryRecordsBL.GetAll(cmbStandards.SelectedValue.ToInt(), cmbDivisions.SelectedValue.ToInt(), txtShowDate.Text.ToDateTime());

        if (lstLibraryRecords.Count > Constants.I_ZERO)
        {
            trBookIssueReturnDate.Visible = true;
            txtIssueReturnDate.Text = DateTime.Now.ToString(Constants.S_DATE_FORMAT);
            txtIssueReturnTime.Text = DateTime.Now.ToString(S_TIME_FORMAT);
        }
        else
            trBookIssueReturnDate.Visible = false;

        lstvwStudents.DataSource = lstLibraryRecords;
        lstvwStudents.DataBind();

        btnIssue.Visible = lstLibraryRecords.Any(lr => lr.BookNo == string.Empty);
        btnReturn.Visible = lstLibraryRecords.Any(lr => lr.BookNo != string.Empty);
    }

    #endregion
}