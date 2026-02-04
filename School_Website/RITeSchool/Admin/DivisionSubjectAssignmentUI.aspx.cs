// File Name     : DivisionSubjectAssignmentUI.aspx.cs
// Modified By   :  
// Modified Date : 11/09/2009
// Description   : This class is used to save classwise subject configuration.

using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;

public partial class DivisionSubjectAssignmentUI : SchoolBase
{
    #region Constants

    const string S_SELECT_AT_LEAST_ONE_GROUP1 = "Atleast one subject should be assigned for each division.";
    const string S_SELECT_AT_LEAST_ONE_GROUP = "Atleast one division should be selected for each subject.";
    const Int32 I_STANDARD_ID_COLUMN_NUMBER = 1;
    const Int32 I_STANDARD_DIVISION_ID_COLUMN_NUMBER = 2;
    const Int32 I_STANDARD_NAME_COLUMN_NUMBER = 3;
    const Int32 I_START_COUNT = 4;

    private const string S_COLUMN_SUBJECT_ID = "Subject_Id";
    private const string S_COLUMN_SUBJECT_NAME = "Subject_Name";

    #endregion

    #region Data Members

    private DataSet moDSAllStdandardDivisions;
    private string IsConfig;

    #endregion

    #region event handlers

    /// <summary>
    /// This event is used to initialise form control and to fill classwise subject assignment. 
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
                InitializeForm();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }
            FillStandardwiseDivisionsGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to move on previous page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration)));            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used to save classwise subject assignment.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int iStandardDivisionId;
            CheckBox oChkHeader = new CheckBox();
            int iClassCount = grdDivisions.Rows.Count;
            int iCountCols = grdDivisions.Rows[0].Cells.Count;

            CheckBox oChkSubject;
            Collection<StandardDivisionMasterBL> oStandardDivisionCollection = new Collection<StandardDivisionMasterBL>();
            Collection<SchoolwiseDivisionSubjectMasterBL> oDivSubjectCollection = new Collection<SchoolwiseDivisionSubjectMasterBL>();
            for (int iRow = 0; iRow < iClassCount; iRow++)
            {
                StandardDivisionMasterBL oStandardDivisionMasterBL = new StandardDivisionMasterBL();
                Collection<SubjectMasterBL> oSubjectCollection = new Collection<SubjectMasterBL>();
                StandardWiseDivisionListBL moStandardDivisionBL = new StandardWiseDivisionListBL();

                iStandardDivisionId = Convert.ToInt32(grdDivisions.Rows[iRow].Cells[I_STANDARD_DIVISION_ID_COLUMN_NUMBER].Text);
                SubjectCollectionBL oSubjectCollectionBL = new SubjectCollectionBL(miSchoolId, miAcademicYearId);

                for (int iSubjectIndex = I_START_COUNT; iSubjectIndex < iCountCols; iSubjectIndex++)
                {
                    oChkSubject = (CheckBox)(grdDivisions.Rows[iRow].Cells[iSubjectIndex].Controls[0]);
                    int iSubjectId = Convert.ToInt32(grdDivisions.Rows[iRow].Cells[iSubjectIndex].Text);
                    oChkHeader = (CheckBox)(grdDivisions.HeaderRow.Cells[iSubjectIndex].Controls[0]);
                    string sSubjectName = oChkHeader.Text;
                    DataRow[] oDrClassSubjects = moDSAllStdandardDivisions.Tables[3].Select("Standard_Division_Id=" + iStandardDivisionId.ToString() + " AND subject_id=" + iSubjectId.ToString());
                    //insert
                    if ((oChkSubject.Visible) && oChkSubject.Checked == true && (oDrClassSubjects.Length == 0))
                    {
                        SubjectMasterBL oSubjectMasterBL = SetSubjectMasterBL(iStandardDivisionId, iSubjectId, sSubjectName);
                        oSubjectMasterBL.ConfigurationAction = Constants.Action.Insert;
                        oSubjectCollection.Add(oSubjectMasterBL);
                    }
                    //delete
                    else if ((oChkSubject.Visible) && oChkSubject.Checked == false && (oDrClassSubjects.Length > 0))
                    {
                        SubjectMasterBL oSubjectMasterBL = SetSubjectMasterBL(iStandardDivisionId, iSubjectId, sSubjectName);
                        oSubjectMasterBL.ConfigurationAction = Constants.Action.Delete;
                        oSubjectCollection.Add(oSubjectMasterBL);
                        SchoolwiseDivisionSubjectMasterBL oDivSubject = GetDivSubject(Convert.ToInt32(oDrClassSubjects[0]["Schoolwise_Division_Subject_Id"]), oDrClassSubjects[0]["DivSubjectName"].ToString());
                        oDivSubjectCollection.Add(oDivSubject);
                    }
                }
                if (oSubjectCollection.Count > 0)
                {
                    oStandardDivisionMasterBL.SubjectCollection = oSubjectCollection;
                    oStandardDivisionMasterBL.StandardDivisionName = grdDivisions.Rows[iRow].Cells[I_STANDARD_NAME_COLUMN_NUMBER].Text;
                    oStandardDivisionCollection.Add(oStandardDivisionMasterBL);
                }
            }
            // If there are Divisions to be deleted then give warning message to user about the same.
            // Update database with the configured Divisions.
            if (oStandardDivisionCollection.Count > 0)
            {
                CheckDependencies(oDivSubjectCollection);
                StandardDivisionCollectionBL oStandardDivisionCollectionBL = new StandardDivisionCollectionBL(miSchoolId,miAcademicYearId);
                oStandardDivisionCollectionBL.UpdateDivisionsSubjects(oStandardDivisionCollection);
                ReadQuerystring();
                if (IsConfig != "Y")
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.DivisionwiseSubjects));
            }
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration)));
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblError.Text = CommonUtility.ModifyExceptionMessage(ex.Message, string.Empty, string.Empty, "can not be removed since associated with", Resources.LocalizedResources.valRemoveText);
            FillStandardwiseDivisionsGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to check checkbox for subjects associated to class. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdDivisions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].CssClass = "locked";
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].CssClass = "Llocked";
                e.Row.Cells[0].Style.Add("left", grdDivisions.Style["scrollLeft"]);
            }
            if (e.Row.RowIndex >= 0)
            {
                // Set the standard name in the textbox.
                string sName = Convert.ToString(e.Row.Cells[I_STANDARD_NAME_COLUMN_NUMBER].Text);
                CheckBox chkCheckAll = ((CheckBox)e.Row.Cells[1].FindControl("CheckAllForRow"));
                chkCheckAll.Text = sName;
                int iRowNo = e.Row.RowIndex;
                chkCheckAll.Attributes.Add("onclick", "CheckUncheckAllInRow(this," + iRowNo + ")");
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
    /// This method is used to initialise form controls.
    /// </summary>
    private void InitializeForm()
    {
        grdDivisions.Columns[0].HeaderText = "";
        btnSave.Attributes.Add("onclick", "if(!saveChk('" + Resources.LocalizedResources.AtleastIOneDivisionShouldBeSelectedForEachSubject + "' , '" + Resources.LocalizedResources.sSelectAtLeastOneGroup1 + "' , this)){return false}");
        btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";        
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave });
        hidFirstFxFollowingErrors.Value = Resources.LocalizedResources.PleaseFixFollowingError;
    }

    /// <summary>
    /// This method is used to check dependancy between classe and subject.
    /// </summary>
    /// <param name="aoDivSubjectCollection"></param>
    private void CheckDependencies(Collection<SchoolwiseDivisionSubjectMasterBL> aoDivSubjectCollection)
    {
        if (aoDivSubjectCollection.Count > 0)
        {
            GenericReferenceList<SchoolwiseDivisionSubjectMasterBL> objStdDivsRefereces = new GenericReferenceList<SchoolwiseDivisionSubjectMasterBL>(aoDivSubjectCollection, miAcademicYearId);
            objStdDivsRefereces.CheckDependenciesAndThrowException("SchoolwiseDivisionSubjectId", "DivisionSubjectName", Constants.ReferenceId.DivisionwiseSubjects);
        }
    }

    /// <summary>
    /// This method is used to populate SchoolwiseDivisionSubjectMasterBL object and returns the same.
    /// </summary>
    /// <param name="aiDivSubjectId"></param>
    /// <param name="asDivSubjectName"></param>
    /// <returns></returns>
    private SchoolwiseDivisionSubjectMasterBL GetDivSubject(int aiDivSubjectId, string asDivSubjectName)
    {
        SchoolwiseDivisionSubjectMasterBL oDivsSubject = new SchoolwiseDivisionSubjectMasterBL();
        oDivsSubject.SchoolwiseDivisionSubjectId = aiDivSubjectId;
        oDivsSubject.DivisionSubjectName = asDivSubjectName;
        return oDivsSubject;
    }

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        try
        {
            if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
            {
                IsConfig = QueryString["Is_Configured"];
            }
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master; 
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    /// <summary>
    /// This method checks the preconditons of, 
    /// Configured Std-Divisions and Subjects for Division wise Subjects criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.DivisionwiseSubjects);
        if (sLinks.Equals(""))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            VisibleOrHideControls();
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to populate SubjectMasterBL object and returns the same.
    /// </summary>
    /// <param name="aiStandardDivisionId"></param>
    /// <param name="aiSubjectId"></param>
    /// <param name="asSubjectName"></param>
    /// <returns></returns>
    private SubjectMasterBL SetSubjectMasterBL(int aiStandardDivisionId, int aiSubjectId, string asSubjectName)
    {
        // This method creates the default object for the configuration and returns the same.
        SubjectMasterBL oSubjectMasterBL = new SubjectMasterBL();        
        oSubjectMasterBL.StandardDivisionId = aiStandardDivisionId;
        oSubjectMasterBL.SubjectId = aiSubjectId;
        oSubjectMasterBL.SubjectName = asSubjectName;
        oSubjectMasterBL.AcademicYearId = miAcademicYearId;
        oSubjectMasterBL.SchoolId = miSchoolId;
        oSubjectMasterBL.UpdatedById = miUserId;

        return oSubjectMasterBL;
    }

    /// <summary>
    ///  This method gets all subjects for school and puts them in newly generated columns.
    /// </summary>
    private void GenerateSubjectColumnsOfGrid()
    {
        int iSubjectCount = moDSAllStdandardDivisions.Tables[1].Rows.Count;//no of columns to be generated
        int iStandardDivisionCount = grdDivisions.Rows.Count;//no of rows 
        int k = 0;
        int iHeaderCellNo = 0;
        int iSubjectIndex;

        //generate other columns
        for (int iRowIndex = 0; iRowIndex < iStandardDivisionCount; iRowIndex++)
        {
            int iStandardId = Convert.ToInt32(grdDivisions.Rows[iRowIndex].Cells[I_STANDARD_ID_COLUMN_NUMBER].Text);
            DataRow[] oArrStdSubjects;
            TableCell oT = null;
            for (iSubjectIndex = 0; iSubjectIndex < iSubjectCount; iSubjectIndex++)
            {
                // header row
                if (iRowIndex == 0)
                {
                    DataControlFieldHeaderCell oTHeader = new DataControlFieldHeaderCell(null);
                    oTHeader.CssClass = "locked";
                    oTHeader.Wrap = false;
                    oTHeader.Text = moDSAllStdandardDivisions.Tables[1].Rows[iSubjectIndex][S_COLUMN_SUBJECT_ID].ToString();
                    iHeaderCellNo = grdDivisions.HeaderRow.Cells.Add(oTHeader);
                    if (iHeaderCellNo > 0)
                    {
                        CheckBox oChkHeader = new CheckBox();
                        oChkHeader.Text = moDSAllStdandardDivisions.Tables[1].Rows[iSubjectIndex][S_COLUMN_SUBJECT_NAME].ToString();
                        grdDivisions.HeaderRow.Cells[iHeaderCellNo].Controls.Add(oChkHeader);
                        iHeaderCellNo = iHeaderCellNo - 4;
                        oChkHeader.Attributes.Add("onclick", "CheckAll(this, " + iHeaderCellNo + ")");
                    }
                }

                if (iSubjectIndex == 0)
                    grdDivisions.Rows[iRowIndex].Cells[iSubjectIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "#aae2cd");

                oT = new TableCell();
                CheckBox oChk = new CheckBox();
                oT.Text = moDSAllStdandardDivisions.Tables[1].Rows[iSubjectIndex][S_COLUMN_SUBJECT_ID].ToString();
                oT.Attributes.Add("title", moDSAllStdandardDivisions.Tables[0].Rows[iRowIndex]["StandardDivision"].ToString() + " [" + moDSAllStdandardDivisions.Tables[1].Rows[iSubjectIndex]["Subject_Name"].ToString() + "]");
                oT.HorizontalAlign = HorizontalAlign.Center;
                oT.Wrap = false;
                oT.Style.Add(HtmlTextWriterStyle.PaddingLeft, "5");
                oT.Style.Add(HtmlTextWriterStyle.PaddingRight, "5");
                oChk.Style.Add("white-space", "nowrap");
                k = grdDivisions.Rows[iRowIndex].Cells.Add(oT);
                int iSubject = Convert.ToInt32(grdDivisions.Rows[iRowIndex].Cells[k].Text);
                oArrStdSubjects = moDSAllStdandardDivisions.Tables[2].Select("Standard_Id = " + iStandardId + "AND subject_id =" + iSubject);
                if (oArrStdSubjects.Length > 0)
                {
                    oChk.Visible = true;
                    int iClassId = Convert.ToInt32(grdDivisions.Rows[iRowIndex].Cells[I_STANDARD_DIVISION_ID_COLUMN_NUMBER].Text);
                    DataRow[] oArrStdDivSubjects = moDSAllStdandardDivisions.Tables[3].Select("Standard_Division_Id =" + iClassId + "AND subject_id =" + iSubject);
                    if (oArrStdDivSubjects.Length > 0)
                        oChk.Checked = true;
                    else
                        oChk.Checked = false;
                }
                else
                    oChk.Visible = false;
                grdDivisions.Rows[iRowIndex].Cells[k].Controls.Add(oChk);
            }
        }
    }

    /// <summary>
    /// This method is used to fill grid that displays classwise subject configuraton.
    /// </summary>
    private void FillStandardwiseDivisionsGrid()
    {
        if (CheckPreCondition())
        {
            VisibleOrHideColumnsofDivisionsGrid(true);
            moDSAllStdandardDivisions = SchoolwiseDivisionSubjectMasterBL.GetStandardDivisionSubjectsAssociation(miSchoolId,miAcademicYearId);
            grdDivisions.DataSource = moDSAllStdandardDivisions.Tables[0].DefaultView;
            grdDivisions.DataBind();
            VisibleOrHideColumnsofDivisionsGrid(false);
            GenerateSubjectColumnsOfGrid();
        }
    }

    /// <summary>
    /// This method is used to show/hide grid columns. 
    /// </summary>
    /// <param name="abAction"></param>
    private void VisibleOrHideColumnsofDivisionsGrid(bool abAction)
    {
        // This method hides the Groupid column from Gridview grdDivisions.
        grdDivisions.Columns[I_STANDARD_DIVISION_ID_COLUMN_NUMBER].Visible = abAction;
        grdDivisions.Columns[I_STANDARD_ID_COLUMN_NUMBER].Visible = abAction;
        grdDivisions.Columns[I_STANDARD_NAME_COLUMN_NUMBER].Visible = abAction;
    }

    /// <summary>
    /// This method is used to visible/hide page controls.
    /// </summary>
    private void VisibleOrHideControls()
    {
        chkAll.Visible = false;
        btnSave.Visible = false;
        grdDivisions.Visible = false;
        btnCancel.Text = Resources.LocalizedResources.Back;
        tdGrid.Visible = false;
    }
    /// <summary>
    /// This method used to value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        btnSave.Attributes.Add("onclick", "if(!saveChk('" + Resources.LocalizedResources.AtleastIOneDivisionShouldBeSelectedForEachSubject + "' , '" + Resources.LocalizedResources.sSelectAtLeastOneGroup1 + "' , this)){return false}");
        hidFirstFxFollowingErrors.Value = Resources.LocalizedResources.PleaseFixFollowingError;
    }

    #endregion
}

