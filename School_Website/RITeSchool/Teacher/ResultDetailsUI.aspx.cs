/*File Name - UpdatePaymentDetailsUI.aspx.cs
 * Created Date - 18 sept 2024
 * Created By - Vishakha
 * Description - This class is used to save result details.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Linq;
using System.Web.UI.HtmlControls;

public partial class ResultDetailsUI : SchoolBase
{
    #region Data Member(s)

    private ResultDetailsBL moResultDetailsBL;

    private const string S_VW_CONDUCT = "Conductdata";
    private const string S_VW_PUNCTUATION = "PunctuationData";
    private const string S_VW_RESULT = "ResultData";
    private const string S_TEACHER_DATA = "TEACHER_DATA";

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill standard, division, term dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moResultDetailsBL = new ResultDetailsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                CheckUserAccess();
                SetDefaultValues();
                SetClassTeacherDetails();
                FillStandards();                
                FillTerms();                
                FillConductList();
                FillPunctuationList();
                FillResultList();
                FillResultDetails();
                HideConductColumnForVpSchool();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill division dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillDivisions();            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill result detail listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillResultDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill result detail listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlTerm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillResultDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save result details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            lblMessage.Text = "Result details saved successfully !!!";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set value for listview dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwResultDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ResultDetails oResultDetails = e.Item.DataItem as ResultDetails;
                if (ViewState[S_VW_CONDUCT] != null)
                {
                    DataTable dtConduct = (DataTable)ViewState[S_VW_CONDUCT];
                    ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                    DropDownList ddlConduct = e.Item.FindControl("ddlConduct") as DropDownList;
                    ListSource.FillDropDownList(dtConduct, ddlConduct, "Name", "Id", Constants.S_SELECT);

                    ddlConduct.SelectedValue = oResultDetails.ConductId.ToString();
                }

                if (ViewState[S_VW_PUNCTUATION] != null)
                {
                    DataTable dtPunctuation = (DataTable)ViewState[S_VW_PUNCTUATION];
                    ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                    DropDownList ddlPunctuality = e.Item.FindControl("ddlPunctuality") as DropDownList;
                    ListSource.FillDropDownList(dtPunctuation, ddlPunctuality, "Name", "Id", Constants.S_SELECT);

                    ddlPunctuality.SelectedValue = oResultDetails.PunctualityId.ToString();
                }

                if (ViewState[S_VW_RESULT] != null)
                {
                    DataTable dtResult = (DataTable)ViewState[S_VW_RESULT];
                    ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                    DropDownList ddlResult = e.Item.FindControl("ddlResult") as DropDownList;
                    ListSource.FillDropDownList(dtResult, ddlResult, "Name", "Id", Constants.S_SELECT);

                    ddlResult.SelectedValue = oResultDetails.ResultId.ToString();
                }

                HtmlTableCell tdConduct = e.Item.FindControl("tdConduct") as HtmlTableCell;
                if (tdConduct != null)
                {
                    if (miSchoolId == Constants.SchoolId.VPMCPS.ToInt())
                        tdConduct.Visible = false;
                    else
                        tdConduct.Visible = true;
                }
               
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// these event is used to hide column
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwResultDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            HideConductColumnForVpSchool();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region methods
    /// <summary>
    /// This method is used to fill result details listview.
    /// </summary>
    private void FillResultDetails()
    {
        if (ddlDivision.SelectedValue != Constants.S_ZERO && ddlTerm.SelectedValue != Constants.S_ZERO)
        {
            List<ResultDetails> lstResultDetails = moResultDetailsBL.GetResultDetails(ddlStandard.SelectedValue.ToInt(), ddlDivision.SelectedValue.ToInt(), ddlTerm.SelectedValue.ToInt());
            lstvwResultDetails.DataSource = lstResultDetails.OrderBy(rd => rd.RollNo).ToList();
            lstvwResultDetails.DataBind();
        }
        else
        {
            lstvwResultDetails.DataSource = null;
            lstvwResultDetails.DataBind();
        }
    }

    /// <summary>
    /// This method is used to fill standard dropdown.
    /// </summary>
    private void FillStandards()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        
        DataTable oDTStandard = oDtStandardCollection.Clone();
        if (moUserRole == Constants.UserRoles.Admin || hidHasEditAccess.Value == Constants.S_YES)
        {
            oDTStandard = oDtStandardCollection;
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
                oDTStandard = oData.CopyToDataTable();
        }

        ControlUtility.FillDropDownList(oDTStandard, ref ddlStandard, Constants.S_STANDARD_ID_FIELD, Constants.S_STANDARD_NAME_FIELD, Constants.S_SELECT);

        if (oDTStandard.Rows.Count == 1)
        {
            ddlStandard.SelectedIndex = 1;
            ddlStandard_SelectedIndexChanged(ddlStandard, new EventArgs());
        }
        else
        {
            ClearDivisions();
        }
    }

    private void ClearDivisions()
    {
        ddlDivision.Items.Clear();
        ListItem olstDivision = new ListItem();
        olstDivision.Value = Constants.S_ZERO;
        olstDivision.Text = Constants.S_SELECT;
        ddlDivision.Items.Add(olstDivision);
    }

    /// <summary>
    /// This method is used to fill division dropdown.
    /// </summary>
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

        ControlUtility.FillDropDownList(oDSStandardCollection, ref ddlDivision, Constants.S_DIVISION_ID_FIELD, Constants.S_DIVISION_NAME_FIELD, Constants.S_SELECT);

        if (oDTDivisions.Rows.Count == 1)
        {
            ddlDivision.SelectedIndex = 1;
            ddlDivision_SelectedIndexChanged(ddlDivision, null);
        }
        else
        {
            lstvwResultDetails.DataSource = null;
            lstvwResultDetails.DataBind();            
        }
    }

    /// <summary>
    /// This method is used to fill term dropdown.
    /// </summary>
    private void FillTerms()
    {
        DataTable oDataTable = StudentwiseRemarkMasterBL.GetTestwiseTerm(miSchoolId);
        ControlUtility.FillDropDownList(oDataTable, ref ddlTerm, "Value_Member", "Display_Member", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill conduct dropdown in listview.
    /// </summary>
    private void FillConductList()
    {        
        ViewState[S_VW_CONDUCT] = moResultDetailsBL.GetConductList();
    }

    /// <summary>
    /// This method is used to fill Punctuality dropdown in listview.
    /// </summary>
    private void FillPunctuationList()
    {
        ViewState[S_VW_PUNCTUATION] = moResultDetailsBL.GetPunctuationList();
    }

    /// <summary>
    /// This method is used to fill Result dropdown in listview.
    /// </summary>
    private void FillResultList()
    {
        ViewState[S_VW_RESULT] = moResultDetailsBL.GetResultList();
    }

    /// <summary>
    /// This method is used to save details.
    /// </summary>
    private void Save()
    {
        List<ResultDetails> oResultDetails = PopulateResultDetails();
        string sXml = base.GenerateXml(oResultDetails);
        moResultDetailsBL.Save(sXml, ddlTerm.SelectedValue.ToInt());
    }

    /// <summary>
    /// This method is used to populate result details.
    /// </summary>
    /// <returns></returns>
    private List<ResultDetails> PopulateResultDetails()
    {
        List<ResultDetails> lstResultDetails = new List<ResultDetails>();
        {
            foreach (ListViewDataItem item in lstvwResultDetails.Items)
            {
                DropDownList ddlConduct = item.FindControl("ddlConduct") as DropDownList;
                DropDownList ddlPunctuality = item.FindControl("ddlPunctuality") as DropDownList;
                DropDownList ddlResult = item.FindControl("ddlResult") as DropDownList;
                int iStudentId = lstvwResultDetails.DataKeys[item.DisplayIndex]["StudentId"].ToInt();

                ResultDetails oResultDetails = new ResultDetails
                {
                    StudentId = iStudentId,
                    ConductId = ddlConduct.SelectedValue.ToInt(),
                    PunctualityId = ddlPunctuality.SelectedValue.ToInt(),
                    ResultId = ddlResult.SelectedValue.ToInt(),
                };
                lstResultDetails.Add(oResultDetails);
            }
            return lstResultDetails;
        }
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    private void CheckUserAccess()
    {
        hidHasEditAccess.Value = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.ResultDetails).ToString();
    }

    private void SetClassTeacherDetails()
    {
        if (moUserRole == Constants.UserRoles.Teacher)
        {
            MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
            DataTable oDataTable = oMasterDataCollectionBL.GetClassTeachers(miSchoolId, miAcademicYearId);
            DataRow[] oDataRow = oDataTable.Select("Teacher_Id=" + Convert.ToString(Session[Constants.S_SESSION_TEACHER_ID]));
            if (oDataRow != null && oDataRow.Length > 0)
            ViewState[S_TEACHER_DATA] = oDataRow.CopyToDataTable();
        }
    }

    /// <summary>
    /// this method is used to hide conduct column for vp school
    /// </summary>
    private void HideConductColumnForVpSchool()
    {
        HtmlTableRow trHeader = lstvwResultDetails.FindControl("trHeader") as HtmlTableRow;
        if (trHeader != null)
        {
            HtmlTableCell thConduct = trHeader.FindControl("thConduct") as HtmlTableCell;
            if (thConduct != null)
            {
                thConduct.Visible = (miSchoolId != Constants.SchoolId.VPMCPS.ToInt());
            }
        }
    }

    #endregion
}