/*File Name - StudentAssessmentDetailsUI.aspx.cs
 * Created Date - 26 oct 2024
 * Created By - Vishakha
 * Description - This class is used to get student list for assessment details.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class StudentListForAssessmentDetailsUI : SchoolBase
{
    #region Data Member(s)
    
    private StudentListForAssessmentBL moStudentListForAssessmentBL; 

    #endregion

    #region Constant(s)
    
    const string S_TEACHER_DATA = "TEACHER_DATA"; 

    #endregion

    #region Event(s)
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStudentListForAssessmentBL = new StudentListForAssessmentBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                CheckUserAccess();
                SetClassTeacherDetails();
                FillTests();
                FillStandardDropdown();
                ReadQueryString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void ddlTest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillStudentList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlStandard.SelectedValue != Constants.S_ZERO)
                FillDivisions();
            else
            {
                ddlDivision.Items.Clear();
                ddlDivision.Items.Add(new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO });
                ResetListview();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlDivision.SelectedValue != Constants.S_ZERO)
                FillStudentList();
            else
                ResetListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwStudentListForAssessment_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            int aiStudentId = Convert.ToInt32(lstvwStudentListForAssessment.DataKeys[e.Item.DisplayIndex]["StudentId"]);
            int aiStandardId = Convert.ToInt32(lstvwStudentListForAssessment.DataKeys[e.Item.DisplayIndex]["StandardId"]);
            string sQueryString = "StudentId=" + aiStudentId + "&StandardId=" + aiStandardId + "&TestId=" + ddlTest.SelectedValue + "&DivisionId=" + ddlDivision.SelectedValue;
            sQueryString = CommonUtility.EncryptQuerystring(sQueryString);

            ImageButton imgBtnSelect = e.Item.FindControl("imgBtnSelect") as ImageButton;
            imgBtnSelect.Attributes.Add("onclick", "OpenWindow('" + sQueryString + "'); return false;");

            Image ImgSelfSubmit = e.Item.FindControl("ImgSelfSubmit") as Image;
            Image ImgPeerSubmit = e.Item.FindControl("ImgPeerSubmit") as Image;
            Image ImgParentSubmit = e.Item.FindControl("ImgParentSubmit") as Image;

            StudentListForAssessment oStudentListForAssessment = e.Item.DataItem as StudentListForAssessment;

            if (oStudentListForAssessment.IsSelfSubmitted)
                ImgSelfSubmit.ImageUrl = "../images/IconGrid_AssignTrue.gif";
            else
                ImgSelfSubmit.ImageUrl = "~/RITeSchool/images/IconGrid_Delete.gif";

            if (oStudentListForAssessment.IsPeerSubmitted)
                ImgPeerSubmit.ImageUrl = "../images/IconGrid_AssignTrue.gif";
            else
                ImgPeerSubmit.ImageUrl = "~/RITeSchool/images/IconGrid_Delete.gif";

            if (oStudentListForAssessment.IsParentSubmitted)
                ImgParentSubmit.ImageUrl = "../images/IconGrid_AssignTrue.gif";
            else
                ImgParentSubmit.ImageUrl = "~/RITeSchool/images/IconGrid_Delete.gif";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)
    
    private void FillTests()
    {
        DataTable oDT = moStudentListForAssessmentBL.GetTestNames();
        ControlUtility.FillDropDownList(oDT, ref ddlTest, "SchoolWise_Test_Id", "SchoolWise_Test_Name", Constants.S_SELECT);
    }

    private void FillStudentList()
    {
        if (ddlTest.SelectedValue != Constants.S_ZERO && ddlStandard.SelectedValue != Constants.S_ZERO && ddlDivision.SelectedValue != Constants.S_ZERO)
        {
            List<StudentListForAssessment> lstStudentListForAssessment = moStudentListForAssessmentBL.GetStudentList(ddlStandard.SelectedValue.ToInt(), ddlDivision.SelectedValue.ToInt(), ddlTest.SelectedValue.ToInt());
            lstvwStudentListForAssessment.DataSource = lstStudentListForAssessment;
            lstvwStudentListForAssessment.DataBind();
        }
        else        
            ResetListview();        
    }

    private void FillStandardDropdown()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();

        DataTable odtStandard = oDtStandardCollection.Clone();
        if (moUserRole == Constants.UserRoles.Admin || hidHasEditAccess.Value == Constants.S_YES)
        {
            odtStandard = oDtStandardCollection;
        }
        else if (moUserRole == Constants.UserRoles.Teacher)
        {
            DataTable oDT = ViewState[S_TEACHER_DATA] as DataTable;
            List<int> lstStdIds = oDT.AsEnumerable().Select(std => std.Field<int>("Standard_Id")).ToList().Distinct().ToList();
            var oData = (from std in oDtStandardCollection.AsEnumerable()
                         join sid in lstStdIds
                         on std.Field<int>("Standard_Id") equals sid
                         select std);

            if (oData != null && oData.Count() > 0)
                odtStandard = oData.CopyToDataTable();
        }

        ControlUtility.FillDropDownList(odtStandard, ref ddlStandard, Constants.S_STANDARD_ID_FIELD, Constants.S_STANDARD_NAME_FIELD, Constants.S_SELECT);

        if (odtStandard.Rows.Count == 1)
        {
            ddlStandard.SelectedIndex = 1;
            ddlStandard_SelectedIndexChanged(ddlStandard, new EventArgs());
        }
        else
        {
            ddlDivision.Items.Add(new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO });
        }
    }

    private void FillDivisions()
    {
        int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(iStandardId);

        DataTable oDTDivisions = oDSStandardCollection.Clone();
        if (moUserRole == Constants.UserRoles.Admin || hidHasEditAccess.Value == Constants.S_YES)
        {
            oDTDivisions = oDSStandardCollection;
        }
        else if (moUserRole == Constants.UserRoles.Teacher)
        {
            DataTable oDT = ViewState[S_TEACHER_DATA] as DataTable;
            List<int> lstDivIds = oDT.AsEnumerable().Where(std => std.Field<int>("Standard_Id") == iStandardId).Select(std => std.Field<int>("Division_Id")).ToList().Distinct().ToList();
            var oData = (from div in oDSStandardCollection.AsEnumerable()
                         join did in lstDivIds
                        on div.Field<int>("Division_Id") equals did
                         select div);

            if (oData != null && oData.Count() > 0)
                oDTDivisions = oData.CopyToDataTable();
        }
        
        ControlUtility.FillDropDownList(oDTDivisions, ref ddlDivision, Constants.S_DIVISION_ID_FIELD, Constants.S_DIVISION_NAME_FIELD, Constants.S_SELECT);

        if (oDTDivisions.Rows.Count == 1)
        {
            ddlDivision.SelectedIndex = 1;
            ddlDivision_SelectedIndexChanged(ddlDivision, null);
        }
        else
            ResetListview();
    }

    private void ReadQueryString()
    {
        if (QueryString["TestId"] != null)
            ddlTest.SelectedValue = QueryString["TestId"].ToString();

        if (QueryString["StandardId"] != null)
        {
            ddlStandard.SelectedValue = QueryString["StandardId"].ToString();
            ddlStandard_SelectedIndexChanged(ddlStandard, null);
        }

        if (QueryString["DivisionId"] != null)
        {
            ddlDivision.SelectedValue = QueryString["DivisionId"].ToString();
            ddlDivision_SelectedIndexChanged(ddlDivision, null);
        }
    }

    private void CheckUserAccess()
    {
        hidHasEditAccess.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.StudentListForSelfAssessment).ToString();
    }

    private void SetClassTeacherDetails()
    {
        Char cCanEdit = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.StudentListForSelfAssessment);

        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable oDataTable = oMasterDataCollectionBL.GetClassTeachers(miSchoolId, miAcademicYearId);

        if (moUserRole == Constants.UserRoles.Admin || cCanEdit == Constants.C_YES)
        {
            ViewState[S_TEACHER_DATA] = oDataTable;
        }        
        else if (moUserRole == Constants.UserRoles.Teacher)
        {
            if (Session[Constants.S_SESSION_IS_CLASS_TEACHER] != null && Convert.ToString(Session[Constants.S_SESSION_IS_CLASS_TEACHER]) == Constants.S_YES)
            {
                DataRow[] oDataRow = oDataTable.Select("Teacher_Id=" + Convert.ToString(Session[Constants.S_SESSION_TEACHER_ID]));
                if (oDataRow.Length > 0)
                    ViewState[S_TEACHER_DATA] = oDataRow.CopyToDataTable();
                else
                    ViewState[S_TEACHER_DATA] = oDataTable.Clone();
            }
            else  
                ViewState[S_TEACHER_DATA] = oDataTable.Clone();
          }
    }

    private void ResetListview()
    {
        lstvwStudentListForAssessment.DataSource = null;
        lstvwStudentListForAssessment.DataBind();
    }

    #endregion
}