/*File Name - StudentAssessmentDetailsUI.aspx.cs
 * Created Date - 18 oct 2024
 * Created By - Vishakha
 * Description - This class is used to save student assessment details.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class StudentAssessmentDetailsUI : SchoolBase
{
    #region Data Member(s)

    private StudentAssessmentBL moStudentAssessmentBL;
    private const string S_GRADES = "GradeData";
    private const string S_FAVLIST = "FavList";
    
    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill grade, test related dropdowns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStudentAssessmentBL = new StudentAssessmentBL(miSchoolId, 0, miUserId);
            if (!IsPostBack)
            {
                FillAcademicYear();
                FillGradeList();
                FillTests();
                SetDefaultValues();
                ReadQueryString();
                HideFields();
                ddlStudentName.Items.Add(new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO });
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill Category and student drodpwn.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlTest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlCategory.SelectedValue = Constants.S_ONE;
            ddlCategory_SelectedIndexChanged(ddlCategory,null);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill student dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            HideFields();

            lstvwStudentFavDetails.DataSource = null;
            lstvwStudentFavDetails.DataBind();
            
            lstvwStudentAssessmentDetails.DataSource = null;
            lstvwStudentAssessmentDetails.DataBind();

            lstvwCategorywiseParameters.DataSource = null;
            lstvwCategorywiseParameters.DataBind();

            trAspectsHeader.Visible = false;
            DisplayLegends(false);
            FillStudents();    
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStudentName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCategory.SelectedValue == "1")
                FillStudentFavDetailsListview();
            else
            {
                lstvwStudentFavDetails.DataSource = null;
                lstvwStudentFavDetails.DataBind();
            }

            FillStudentAssessmentDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save student assessment details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            lblMessage.Text = "Student assessment details saved successfully !!!";
            FillStudentFavDetailsListview();
            FillStudentAssessmentDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            //ClearFields();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit assessment details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
           bool bSubmitStudentAssessmentDetails = true;
           moStudentAssessmentBL.SubmitStudentAssessmentDetails(ddlAcademicYear.SelectedValue.ToInt(), ddlCategory.SelectedValue.ToInt(), ddlTest.SelectedValue.ToInt(),bSubmitStudentAssessmentDetails, ddlStudentName.SelectedValue.ToInt());
           lblMessage.Text = "Student assessment details submited successfully !!!";
           FillStudentFavDetailsListview();
           FillStudentAssessmentDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill listview dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentAssessmentDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                StudentAssessmentDetails oStudentAssessmentDetails = e.Item.DataItem as StudentAssessmentDetails;
                if (ViewState[S_GRADES] != null)
                {
                    DataTable dtGrade = (DataTable)ViewState[S_GRADES];
                    DropDownList ddlGrade = e.Item.FindControl("ddlGrade") as DropDownList;
                    ListSource.FillDropDownList(dtGrade, ddlGrade, "Name", "Id", Constants.S_SELECT);

                    ddlGrade.SelectedValue = oStudentAssessmentDetails.GradeId.ToString();
                }

                Label lblSerialNo = e.Item.FindControl("lblSerialNo") as Label;
                lblSerialNo.Text = (e.Item.DisplayIndex + 1).ToString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwStudentFavDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                TextBox txtComments = e.Item.FindControl("txtComments") as TextBox;
               
                Label lblSerialNo = e.Item.FindControl("lblSerialNoFav") as Label;
                lblSerialNo.Text = (e.Item.DisplayIndex + 1).ToString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwCategorywiseParameters_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Label lblSrNo = e.Item.FindControl("lblSerialNoForCategorywiseComment") as Label;
                lblSrNo.Text = (e.Item.DisplayIndex + 1).ToString();
            } 
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region methods

    private void ReadQueryString()
    {
        if (moUserRole == Constants.UserRoles.Student)
        {
            //hidStudentId.Value = Session[Constants.S_SESSION_STUDENT_ID].ToString();
            btnBack.Visible = false;
        }
        else
        {
            hidStudentId.Value = Convert.ToString(QueryString["StudentId"]);
            btnBack.Visible = true;
            btnBack.PostBackUrl = "StudentListForAssessmentDetailsUI.aspx?" + CommonUtility.EncryptQuerystring("StandardId=" + QueryString["StandardId"].ToString() + "&TestId=" + QueryString["TestId"].ToString() + "&DivisionId=" + QueryString["DivisionId"].ToString());
        }

        if (moUserRole == Constants.UserRoles.Student)
        {
            //hidStdId.Value = Session[Constants.S_SESSION_STUDENT_STANDERED_ID].ToString();
        }
        else
            hidStdId.Value = Convert.ToString(QueryString["StandardId"]);

        if (QueryString["TestId"] != null && QueryString["TestId"].ToString() != string.Empty)
        {
            ddlTest.SelectedValue = QueryString["TestId"].ToString();
            ddlTest_SelectedIndexChanged(ddlTest, null);
        }
    }
    
    /// <summary>
    /// This method is used to fill test dropdown.
    /// </summary>
    private void FillTests()
    {
        DataTable oDT = moStudentAssessmentBL.GetTestNames(ddlAcademicYear.SelectedValue.ToInt());
        ControlUtility.FillDropDownList(oDT, ref ddlTest, "SchoolWise_Test_Id", "SchoolWise_Test_Name", Constants.S_SELECT);
    }

    private void FillAcademicYear()
    {
        int iStudentId = 0;

        if (moUserRole == Constants.UserRoles.Student)
            iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
        else
            iStudentId = Convert.ToInt32(QueryString["StudentId"]);

        DataTable odt = moStudentAssessmentBL.GetAcademicYear(iStudentId);
        ControlUtility.FillDropDownList(odt, ref ddlAcademicYear, "AcademicYearId", "AcademicYear", string.Empty);
        hidStudentId.Value = odt.Rows[0]["OldStudentId"].ToString();
        hidStdId.Value = odt.Rows[0]["OldStandardId"].ToString();
        ddlAcademicYear.Enabled = false;
     }

    /// <summary>
    /// This method is used to fill student dropdown.
    /// </summary>
    private void FillStudents()
    {
        DataTable oDT = moStudentAssessmentBL.GetStudents(Convert.ToInt32(hidStudentId.Value), ddlCategory.SelectedValue.ToInt(), ddlAcademicYear.SelectedValue.ToInt());
        ControlUtility.FillDropDownList(oDT, ref ddlStudentName, "YearWise_Student_Id", "StudentName", Constants.S_SELECT);

        if (oDT.Rows.Count == 1)
        {
            ddlStudentName.SelectedIndex = 1;
            ddlStudentName_SelectedIndexChanged(ddlStudentName, null);
        }
    }

    /// <summary>
    /// This method is used to fill Comment related listview.
    /// </summary>
    private void FillStudentFavDetailsListview()
    {
        List<StudentFavouriteListDetails> lstStudentFavouriteListDetails = moStudentAssessmentBL.GetListOfStudentFavDetails(ddlAcademicYear.SelectedValue.ToInt(), Convert.ToInt32(hidStdId.Value), ddlStudentName.SelectedValue.ToInt(), ddlTest.SelectedValue.ToInt());
        lstvwStudentFavDetails.DataSource = lstStudentFavouriteListDetails;
        lstvwStudentFavDetails.DataBind();
    }

    /// <summary>
    /// This method is used to fill listview.
    /// </summary>
    private void FillStudentAssessmentDetails()
    {
        List<StudentAssessmentDetails> lstStudentAssessmentDetails = moStudentAssessmentBL.GetStudentAssessmentDetails(ddlAcademicYear.SelectedValue.ToInt(), Convert.ToInt32(hidStdId.Value), ddlCategory.SelectedValue.ToInt(), ddlStudentName.SelectedValue.ToInt(), ddlTest.SelectedValue.ToInt());
        lstvwStudentAssessmentDetails.DataSource = lstStudentAssessmentDetails;
        lstvwStudentAssessmentDetails.DataBind();

        List<CategorywiseComment> lstCategorywiseComment = moStudentAssessmentBL.CategorywiseComments;
        lstvwCategorywiseParameters.DataSource = lstCategorywiseComment;
        lstvwCategorywiseParameters.DataBind();

        trAspectsHeader.Visible = lstStudentAssessmentDetails.Count > 0;
        
        DisplayLegends(lstStudentAssessmentDetails.Count > 0);

        trCategorywiseComment.Visible = lstCategorywiseComment.Count > 0;

        if (moStudentAssessmentBL.ButtonStates.IsSaved)
        {
            if (moStudentAssessmentBL.ButtonStates.IsSubmitted)
            {
                btnSave.Enabled = false;
                btnSubmit.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
                btnSubmit.Enabled = true;
            }
        }
        else
        {
            btnSave.Enabled = true;
            btnSubmit.Enabled = false;
        }

        if (lstStudentAssessmentDetails.Count > 0)
        {
            trAspectsHeader.Visible = true;
            DisplayLegends(true);
        }
        else
        {
            trAspectsHeader.Visible = false;
            DisplayLegends(false);
            btnSave.Enabled = false;
            btnSubmit.Enabled = false;
        }
     }

    
    /// <summary>
    /// This method is used to return favpurite related details from database.
    /// </summary>
    private void GetDetails()
    {
        StudentFavouriteDetails oStudentFavouriteDetails = moStudentAssessmentBL.GetAll(Convert.ToInt32(hidStudentId.Value), ddlTest.SelectedValue.ToInt(), ddlAcademicYear.SelectedValue.ToInt());
        if (oStudentFavouriteDetails.FavouriteFood != null)
        {
            txtFavColor.Text = oStudentFavouriteDetails.FavouriteColour;
            txtFavFood.Text = oStudentFavouriteDetails.FavouriteFood;
            txtFavSport.Text = oStudentFavouriteDetails.FavouriteSport;
            txtFavSubject.Text = oStudentFavouriteDetails.FavouriteSubject;
        }
        else
        {
            txtFavColor.Text = string.Empty;
            txtFavFood.Text = string.Empty;
            txtFavSport.Text = string.Empty;
            txtFavSubject.Text = string.Empty;
        }
    }
    
    /// <summary>
    /// This method is used to fill grade dropdown.
    /// </summary>
    private void FillGradeList()
    {
        DataTable odtGrade = moStudentAssessmentBL.GetGrades(ddlAcademicYear.SelectedValue.ToInt());
        ViewState[S_GRADES] = odtGrade;
    }

    /// <summary>
    /// This method is used to save student assessment details.
    /// </summary>
    private void Save()
    {
        List<StudentAssessmentDetails> oStudentAssessmentDetails = PopulateStudentAssessmentDetails();
        List<StudentFavouriteListDetails> oStudentFavouriteListDetails = PopulateStudentFavList();
        StudentFavouriteDetails oStudentFavouriteDetails = PopulateStudentFavDetails();
        List<CategorywiseComment> oCategorywiseComment = PopulateCategorywisecomment();
        string sXml = base.GenerateXml(oStudentAssessmentDetails);
        string sFavListXml = base.GenerateXml(oStudentFavouriteListDetails);
        string sCategorywiseCommentXml = base.GenerateXml(oCategorywiseComment);
        moStudentAssessmentBL.Save(sXml, sFavListXml, sCategorywiseCommentXml, ddlAcademicYear.SelectedValue.ToInt(), ddlTest.SelectedValue.ToInt(), ddlStudentName.SelectedValue.ToInt(), oStudentFavouriteDetails);
    }

    /// <summary>
    /// This methos is used to populate student assessment details.
    /// </summary>
    /// <returns></returns>
    private List<StudentAssessmentDetails> PopulateStudentAssessmentDetails()
    {
        List<StudentAssessmentDetails> lstStudentAssessmentDetails = new List<StudentAssessmentDetails>();
        {
            foreach (ListViewDataItem item in lstvwStudentAssessmentDetails.Items)
            {
                DropDownList ddlGrade = item.FindControl("ddlGrade") as DropDownList;
                int iParameterId = lstvwStudentAssessmentDetails.DataKeys[item.DisplayIndex]["ParameterId"].ToInt();
                
                StudentAssessmentDetails oStudentAssessmentDetails = new StudentAssessmentDetails
                {
                    ParameterId = iParameterId,
                    GradeId = ddlGrade.SelectedValue.ToInt(),
                };
                lstStudentAssessmentDetails.Add(oStudentAssessmentDetails);
            }
            return lstStudentAssessmentDetails;
        }
    }

    private List<StudentFavouriteListDetails> PopulateStudentFavList()
    {
        List<StudentFavouriteListDetails> lstStudentFavouriteListDetails = new List<StudentFavouriteListDetails>();
        {
            foreach (ListViewDataItem item in lstvwStudentFavDetails.Items)
            {
                TextBox txtComment = item.FindControl("txtComments") as TextBox;
                int iParameterId = lstvwStudentFavDetails.DataKeys[item.DisplayIndex]["ParameterId"].ToInt();

                StudentFavouriteListDetails oStudentFavouriteListDetails = new StudentFavouriteListDetails
                {
                    ParameterId = iParameterId,
                    Comment = txtComment.Text,
                };
                lstStudentFavouriteListDetails.Add(oStudentFavouriteListDetails);
            }
            return lstStudentFavouriteListDetails;
        }
    }

    private List<CategorywiseComment> PopulateCategorywisecomment()
    {
        List<CategorywiseComment> lstCategorywiseComment = new List<CategorywiseComment>();
        {
            foreach (ListViewDataItem item in lstvwCategorywiseParameters.Items)
            {
                TextBox txtCategorywiseComment = item.FindControl("txtCategorywiseComments") as TextBox;
                int iParameterId = lstvwCategorywiseParameters.DataKeys[item.DisplayIndex]["ParameterId"].ToInt();

                CategorywiseComment oCategorywiseComment = new CategorywiseComment
                {
                    ParameterId = iParameterId,
                    CommentForCategory = txtCategorywiseComment.Text,
                };
                lstCategorywiseComment.Add(oCategorywiseComment);
            }
            return lstCategorywiseComment;
        }
    }

    /// <summary>
    /// This method is used to populate student favourite details.
    /// </summary>
    /// <returns></returns>
    private StudentFavouriteDetails PopulateStudentFavDetails()
    {
        StudentFavouriteDetails oStudentFavouriteDetails = new StudentFavouriteDetails();

        oStudentFavouriteDetails.FavouriteColour = txtFavColor.Text.Trim();
        oStudentFavouriteDetails.FavouriteFood = txtFavFood.Text.Trim();
        oStudentFavouriteDetails.FavouriteSport = txtFavSport.Text.Trim();
        oStudentFavouriteDetails.FavouriteSubject = txtFavSubject.Text.Trim();

        return oStudentFavouriteDetails;
    }

    /// <summary>
    /// This method is used to hide TRs.
    /// </summary>
    private void HideFields()
    {
        if (ddlCategory.SelectedItem.Text == "Self Assessment")
        {            
            DisableControls(true);
            GetDetails();
        }
        else
        {         
            DisableControls(false);
        }

        if (ddlCategory.SelectedItem.Text == "Self Assessment" && moSchool == Constants.SchoolId.PPSN)
        {
            trFavColor.Visible = false;
            trFavFood.Visible = false;
            trFavSport.Visible = false;
            trFavSubject.Visible = false;

            ReqFavColour.Enabled = false;
            ReqFavFood.Enabled = false;
            ReqFavSport.Enabled = false;
            ReqFavSub.Enabled = false;
            cstComment.Enabled = true;
        }
    }

    private void SetDefaultValues()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    private void DisableControls(bool abAction)
    {
        trFavColor.Visible = abAction;
        trFavFood.Visible = abAction;
        trFavSport.Visible = abAction;
        trFavSubject.Visible = abAction;
        TrStudentFavList.Visible = abAction;

        ReqFavColour.Enabled = abAction;
        ReqFavFood.Enabled = abAction;
        ReqFavSport.Enabled = abAction;
        ReqFavSub.Enabled = abAction;
    }

    /// <summary>
    /// This method is used to display legends.
    /// </summary>
    /// <param name="abStatus"></param>
    private void DisplayLegends(bool abStatus)
    {
        if (moSchool == Constants.SchoolId.PPSN)
            trLegendforPPSN.Visible = abStatus;
        else
            trLegend.Visible = abStatus;
    }

    #endregion
}