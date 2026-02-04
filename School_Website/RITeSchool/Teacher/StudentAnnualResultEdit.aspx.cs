/*
* This Class is used to show student progress report 
 * rendered HTMLTable to show this progress report including subject group and test types.
 * Author: Shankar Gurav.
 * Date of creation: 28 Jan 2008
 * Date of modification: 2 Feb 2008
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class StudentAnnualResultEdit : SchoolBase
{
    #region Class Members

    Int32 miStudentId = 0;
    Int32 miClassTacherID = 0;    
    #endregion Class Members

    #region Events

    /// <summary>
    /// Overidded method for page initialization.
    /// </summary>
    /// <param name="e"></param>
    override protected void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);            
            GetQueryString();
            
            GenerateGraceResult(miStudentId);
            if (!IsPostBack)
                GenerateResult(miStudentId);
            else
                ResultContainer.Visible = false;

        }
        catch (BusinessLogic.Exceptions.MarksNotAvailableForResult Ex)
        {
            pnlErrorMsg.Visible = true;
            lblErrorMsg.Text = Ex.Message;
            btnResult.Visible = false;
        }
        catch (BusinessLogic.Exceptions.NoResultFound)
        {
            ResultContainer.Visible = false;
            hidResultGenrted.Value = Constants.I_ZERO.ToString();
        }
        catch (Exception ex)
        {
            ResultContainer.Visible = false;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This method is used to intialize the page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (moUserRole != Constants.UserRoles.Student)
                {
                    btnBack.Attributes.Add("onclick", "window.open('" + "../Teacher/StudentResultList.aspx?" + HidBackUrl.Value + "' , '_self').focus(); return false;");
                    btnResult.Attributes["onclick"] = "javascript:DisableButtons()";
                }
            }

            ApplyMouseHoverEffect(new List<Button> { btnResult, btnBack });
            if (moUserRole == Constants.UserRoles.Student)
                btnBack.Visible = false;
        }
        catch (BusinessLogic.Exceptions.ResultNotPublished ex)
        {
            pnlErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to generate result and show into 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnResult_Click(object sender, EventArgs e)
    {
        try
        {
            ResultContainer.Visible = true;
            if (hidEdited.Value == Constants.I_ONE.ToString() ||
                    hidResultGenrted.Value == Constants.I_ZERO.ToString())
            {
                StudentResultGrace oStudentResult = new StudentResultGrace(GridViewScrollContainer);
                oStudentResult.UpdateStudentMarks(miStudentId);
                GenerateGraceResult(miStudentId);
            }
            hidEdited.Value = Constants.I_ZERO.ToString();            
            GenerateResult(miStudentId);
        }
        catch (BusinessLogic.Exceptions.NoResultFound)
        {
            ResultContainer.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to navigate to back page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master; oMasterPage.RedirectToNextPage("../Teacher/StudentResultList.aspx?" + HidBackUrl.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion Events

    #region Private Methods

    /// <summary>
    /// This method is used to create an result of a student
    /// </summary>
    /// <param name="iStudentId"></param>
    private void GenerateResult(int iStudentId)
    {
        StudentResult oStudentResult = new StudentResult(ResultContainer,true);        
        oStudentResult.FillProgressReport(iStudentId);
    }

    /// <summary>
    /// This function sets the form fields according to the query string values.
    /// </summary>
    private void GetQueryString()
    {
        if (QueryString.Count > 0)
        {
            if (QueryString["StdDivId"] != null)
            {
                int miStdDivId = QueryString["StdDivId"].ToInt();
                SchoolWiseAnnualResultPublishBL oSWStdDivResultPublishBL = new SchoolWiseAnnualResultPublishBL(miSchoolId, miAcademicYearId, miStdDivId);
                if (oSWStdDivResultPublishBL.AnnualResult_publish_Id != 0)
                    btnResult.Enabled = false;
            }
         
            if (QueryString["TeacherId"] != null)
                miClassTacherID = QueryString["TeacherId"].ToInt();
            if (QueryString["StudentId"] != null)
                miStudentId = QueryString["StudentId"].ToInt();
        }

        if (moUserRole == Constants.UserRoles.Student)
        {
            miStudentId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ID]);
            CheckIsResultPublished();
        }
    }

    /// <summary>
    /// This method is used to show grace add edit mode annual result.
    /// </summary>
    /// <param name="miStudentId"></param>
    private void GenerateGraceResult(int miStudentId)
    {
        StudentResultGrace oStudentResult = new StudentResultGrace(GridViewScrollContainer);
        oStudentResult.FillProgressReport(miStudentId);
        hidSubjectLists.Value = oStudentResult.SubjectList;
    }

    /// <summary>
    /// This method is used to check that is Result is published or not
    /// </summary>
    private void CheckIsResultPublished()
    {
        int iStandardDivisionId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID]);
        SchoolWiseAnnualResultPublishBL oSchoolWisdeAnnualResultPublishBL = new SchoolWiseAnnualResultPublishBL(miSchoolId, miAcademicYearId, iStandardDivisionId);
        if (oSchoolWisdeAnnualResultPublishBL.AnnualResult_publish_Id == 0)
            throw new BusinessLogic.Exceptions.ResultNotPublished("Result not published for this class.");
    }

    #endregion Private Methods

}
