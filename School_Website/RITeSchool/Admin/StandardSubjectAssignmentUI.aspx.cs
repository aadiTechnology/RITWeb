/* File Name    :   StandardSubjectAssignmentUI.aspx.cs
* Purpose       :   This class is used to display all standards and subjects of the respective
*                  school to assign subjects to the school.
* Created By   :  Madhura Bendale.    Created Date :  20-Oct-2007
*/
/* Page History 
 * File modified by : Madhura Bendale.
 * Purpose          : Adding hyperlink of "Assign subjects to divisions" to this page.
 * Date of Modification : 29-nov-2007
 */
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using System.Collections.ObjectModel;
using BusinessLogic;
using Utility;

public partial class StandardSubjectAssignmentUI : SchoolBase
{
    #region Constants

    const string S_SELECT_AT_LEAST_ONE_SUBJECT_FOR_STANDARD = "Atleast one subject should be assigned for each standard.";
    const string S_SELECT_AT_LEAST_ONE_STANDARD_FOR_SUBJECTS = "Atleast one standard should be selected for each subject.";

    const Int32 I_STANDARD_ID_DATAKEY_NAME = 0;
    const Int32 I_ORIGINAL_STANDARD_ID_DATAKEY_NAME = 1;
    const Int32 I_STANDARD_NAME_DATAKEY_NAME = 2;
    const Int32 I_START_COUNT = 1;

    const int I_STDSUBJECT_TABLE_INDEX = 2;

    private const string S_COLUMN_SUBJECT_ID = "Subject_Id";
    private const string S_COLUMN_SUBJECT_NAME = "Subject_Name";

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill the grid which shows subject associated to standard.
    /// It also sets client side validation and properties.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                if (CheckPreCondition())
                {
                    hlnkSortOrder.Attributes.Add("onclick", "window.open('SubjectsSortOrderPopUp.Aspx?" + Server.UrlDecode(Request.QueryString.ToString())
                                                          + "', '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=650,height=530');return false;");
                    btnSave.Attributes.Add("onclick", "if(!saveChk('" + Resources.LocalizedResources.sSelectAtLeastOneSubjectForStandard + "' , '"
                                           + Resources.LocalizedResources.sSelectAtLeastOneStandardForSubject + "',this)){return false}");
                    btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";                    
                }
            }
            bool bIsUseSubmitBehavior = CommonUtility.CheckCancelOrBackClickEvent(this.Page);
            if (bIsUseSubmitBehavior == true)
            {
                FillGridWithStandardsAndsubjects();
                grdStandards.UseAccessibleHeader = true;
            }
            InitializeForm();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save subject configuration as per standard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int iStandardId;
            CheckBox ochkSubjects;
            int iColumnIndex = Convert.ToInt32(hidColumnCount.Value);
            Collection<StandardMasterBL> oStandardCollection = new Collection<StandardMasterBL>();
            SubjectCollectionBL oSubjectCollectionBL = new SubjectCollectionBL(miSchoolId, miAcademicYearId);
            
            Collection<SchoolWiseStandardSubjectMasterBL> oStdSubjectCollection = new Collection<SchoolWiseStandardSubjectMasterBL>();
            
            DataTable oDtSubjectIds = ((DataSet)grdStandards.DataSource).Tables[I_STDSUBJECT_TABLE_INDEX];
            SetPrimaryKey(oDtSubjectIds);
            object[] objStdSubject = new object[2];
            DataRow oDrStdSubject;
            for (int iRowCount = 0; iRowCount < grdStandards.Rows.Count; iRowCount++)
            {
                StandardMasterBL oStandardMasterBL = new StandardMasterBL();
                Collection<SubjectMasterBL> oSubjectCollection = new Collection<SubjectMasterBL>();

                iStandardId = Convert.ToInt32(grdStandards.DataKeys[iRowCount][I_STANDARD_ID_DATAKEY_NAME].ToString());

                for (int iColumnCount = I_START_COUNT; iColumnCount <= iColumnIndex; iColumnCount++)
                {
                    ochkSubjects = (CheckBox)(grdStandards.Rows[iRowCount].Cells[iColumnCount].Controls[0]);
                    int iSubjectId = Convert.ToInt32(grdStandards.Rows[iRowCount].Cells[iColumnCount].Text);
                    string sSubjectName = grdStandards.HeaderRow.Cells[iColumnCount].Text;
                    objStdSubject[0] = iStandardId;
                    objStdSubject[1] = iSubjectId;
                    oDrStdSubject = oDtSubjectIds.Rows.Find(objStdSubject);
                    if (ochkSubjects.Checked == true && (oDrStdSubject == null))
                    {
                        SubjectMasterBL oSubjectMasterBL = SetSubjectMasterBL(iStandardId, iSubjectId, sSubjectName);
                        oSubjectMasterBL.ConfigurationAction = Constants.Action.Insert;
                        oSubjectCollection.Add(oSubjectMasterBL);
                    }
                    else if (ochkSubjects.Checked == false && (oDrStdSubject != null))
                    {
                        SubjectMasterBL oSubjectMasterBL = SetSubjectMasterBL(iStandardId, iSubjectId, sSubjectName);
                        oSubjectMasterBL.ConfigurationAction = Constants.Action.Delete;
                        oSubjectCollection.Add(oSubjectMasterBL);
                        int iStdSubjectId = Convert.ToInt32(oDrStdSubject["SchoolWise_Standard_Subject_Id"]);
                        string sStdSubjectName = Convert.ToString(oDrStdSubject["StdSubject"]);
                        
                        SchoolWiseStandardSubjectMasterBL objStdSubjectBL;
                        objStdSubjectBL = SetStdSubjectObject(iStdSubjectId, sStdSubjectName);
                        oStdSubjectCollection.Add(objStdSubjectBL);
                    }
                }
                if (oSubjectCollection.Count > 0)
                {
                    oStandardMasterBL.StandardName = grdStandards.DataKeys[iRowCount][I_STANDARD_NAME_DATAKEY_NAME].ToString();
                    oStandardMasterBL.SubjectCollection = oSubjectCollection;
                    oStandardCollection.Add(oStandardMasterBL);
                }
            }
            if (oStandardCollection.Count > 0)
            {
                CheckDependencies(oStdSubjectCollection);
                StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId);
                oStandardCollectionBL.UpdateStandardSubjects(oStandardCollection);
            }

            string sIsConfig = ReadQuerystring();
            if (sIsConfig != "Y")
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.StandardwiseSubjects));

            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration)));
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
			lblErr.Text = CommonUtility.ModifyExceptionMessage(ex.Message, string.Empty, string.Empty,  "can not be removed since associated with", Resources.LocalizedResources.valRemoveText);
            FillGridWithStandardsAndsubjects();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion Events

    #region GridView Event

    /// <summary>
    /// This event is used to set the standard name in the template field and also
    /// set attributes property to each row of the grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStandards_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].CssClass = "locked";
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].CssClass = "Llocked";

            if (e.Row.RowIndex >= 0)
            {
                // Set the standard name in the first column.
                string sName = grdStandards.DataKeys[e.Row.RowIndex][I_STANDARD_NAME_DATAKEY_NAME].ToString();
                CheckBox chkCheckAll = ((CheckBox)e.Row.Cells[0].FindControl("CheckAllForRow"));
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

    /// <summary>
    /// This event is used change pagging of grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStandards_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdStandards.PageIndex = e.NewPageIndex;
            FillGridWithStandardsAndsubjects();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to apply style for paging.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStandards_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Table PagerTable = (Table)e.Row.Cells[0].Controls[0];
                PagerTable.CssClass = "ClsNwGridPaging";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region "Private Methods"

    /// <summary>
    /// This method is used to initialise page controls.
    /// </summary>
    private void InitializeForm()
    {
        grdStandards.Columns[0].HeaderText = "";
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration));        
        ApplyMouseHoverEffect(new List<Button> { btnCancel,btnSave});
        hidFirstFxFollowingErrors.Value = Resources.LocalizedResources.PleaseFixFollowingError;
        btnSave.Attributes.Add("onclick", "if(!saveChk('" + Resources.LocalizedResources.sSelectAtLeastOneSubjectForStandard + "' , '"
                                          + Resources.LocalizedResources.sSelectAtLeastOneStandardForSubject + "',this)){return false}");
    }

    /// <summary>
    /// This method is used to check dependancies of standard or subject to other school configuration.
    /// </summary>
    /// <param name="aoStdSubjectCollection"></param>
    private void CheckDependencies(Collection<SchoolWiseStandardSubjectMasterBL> aoStdSubjectCollection)
    {
        if (aoStdSubjectCollection.Count > 0)
        {
            GenericReferenceList<SchoolWiseStandardSubjectMasterBL> objStdDivsRefereces = new GenericReferenceList<SchoolWiseStandardSubjectMasterBL>(aoStdSubjectCollection, miAcademicYearId);
            objStdDivsRefereces.CheckDependenciesAndThrowException("SchoolWiseStandardSubjectId", "StdSubjectName", Constants.ReferenceId.StandardwiseSubjects);
        }
    }

    /// <summary>
    /// This method is used to populate SchoolWiseStandardSubjectMasterBL,
    /// which is used to save stadardwise subject configuration.
    /// </summary>
    /// <param name="iStdSubjectId"></param>
    /// <param name="sStdSubjectName"></param>
    /// <returns></returns>
    private SchoolWiseStandardSubjectMasterBL SetStdSubjectObject(int iStdSubjectId, string sStdSubjectName)
    {
        SchoolWiseStandardSubjectMasterBL objStdSubjectBL = new SchoolWiseStandardSubjectMasterBL();
        objStdSubjectBL.SchoolWiseStandardSubjectId = iStdSubjectId;
        objStdSubjectBL.StdSubjectName = sStdSubjectName;
        return objStdSubjectBL;
    }

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private string ReadQuerystring()
    {
        try
        {
			if (QueryString["Is_Configured"] != null)
				return QueryString["Is_Configured"];
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master; 
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }

        return String.Empty;
    }

    /// <summary>
    /// This method is used set primary key to datatable.
    /// </summary>
    /// <param name="aoDtClass"></param>
    private void SetPrimaryKey(DataTable aoDtClass)
    {
        DataColumn[] oDtCols = new DataColumn[2];
        oDtCols[0] = aoDtClass.Columns["standard_Id"];
        oDtCols[1] = aoDtClass.Columns["Subject_Id"];
        aoDtClass.PrimaryKey = oDtCols;
    }

    /// <summary>
    /// This method is used to visible or hide controls on page load as per configuration is 
    /// done or not.
    /// </summary>
    private void VisibleOrHideControls()
    {
        chkAll.Visible = false;
        btnSave.Visible = false;
        btnCancel.Text = "Back";
        tdGrid.Visible = false;
        hlnkSortOrder.Visible = false;
        divToprLinkHlilight.Visible = false;
    }

    /// <summary>
    /// This method is used to generate columns of subjects of grid dynamically
    /// which is attached to grid one by one and show checkbox is checked true when the subject
    /// is already assgned to that standard.
    /// </summary>
    private void GenerateSubjectColumnsOfGrid()
    {
        int iCellIndex = 0;

        const int I_SUBJECT_TABLE_INDEX = 1;

        DataSet oDs = (DataSet)grdStandards.DataSource;
        DataTable oDtSubjects = oDs.Tables[I_SUBJECT_TABLE_INDEX];
        DataTable oDtStdSubjects = oDs.Tables[I_STDSUBJECT_TABLE_INDEX];

        for (int iCount = 0; iCount < oDtSubjects.Rows.Count; iCount++)
        {
            DataControlFieldHeaderCell oTableCell = new DataControlFieldHeaderCell(null);
            oTableCell.Text = oDtSubjects.Rows[iCount][S_COLUMN_SUBJECT_NAME].ToString();
            oTableCell.HorizontalAlign = HorizontalAlign.Center;
            oTableCell.CssClass = "locked";
            oTableCell.Wrap = false;
            oTableCell.Width = System.Web.UI.WebControls.Unit.Point(900);
            grdStandards.HeaderRow.Cells.Add(oTableCell);
            grdStandards.HeaderRow.HorizontalAlign = HorizontalAlign.Center;

            CheckBox ocheckb = new CheckBox();
            ocheckb.Text = oDtSubjects.Rows[iCount][S_COLUMN_SUBJECT_NAME].ToString();
            int headerCellNo = iCount;
            ocheckb.Attributes.Add("onclick", "CheckAll(this, " + headerCellNo + ")");
            oTableCell.Wrap = false;
            ocheckb.Style.Add("white-space", "nowrap");
            oTableCell.Style.Add(HtmlTextWriterStyle.PaddingLeft, "5");
            oTableCell.Style.Add(HtmlTextWriterStyle.PaddingRight, "5");
            oTableCell.Controls.Add(ocheckb);
            oTableCell.HorizontalAlign = HorizontalAlign.Center;


            for (int iRowIndex = 0; iRowIndex < grdStandards.Rows.Count; iRowIndex++)
            {
                int iStandardId = Convert.ToInt32(grdStandards.DataKeys[iRowIndex][I_STANDARD_ID_DATAKEY_NAME].ToString());

                TableCell osTableCell = new TableCell();
                CheckBox oChk = new CheckBox();
                osTableCell.Text = oDtSubjects.Rows[iCount][S_COLUMN_SUBJECT_ID].ToString();
                osTableCell.HorizontalAlign = HorizontalAlign.Center;
                osTableCell.Attributes.Add("title", Resources.LocalizedResources.Std + " " + oDs.Tables[0].Rows[iRowIndex]["Standard_Name"].ToString() + " [" + oDs.Tables[1].Rows[iCount]["Subject_Name"].ToString() + "]");
                iCellIndex = grdStandards.Rows[iRowIndex].Cells.Add(osTableCell);
                int iSubjectId = Convert.ToInt32(grdStandards.Rows[iRowIndex].Cells[iCellIndex].Text);
                DataRow[] oDr = oDtStdSubjects.Select("subject_id =" + iSubjectId.ToString() + " AND standard_Id = " + iStandardId.ToString());
                if (oDr.Length > 0)
                {
                    oChk.Checked = true;
                }
                grdStandards.Rows[iRowIndex].Cells[iCellIndex].Controls.Add(oChk);
            }
        }
        hidColumnCount.Value = iCellIndex.ToString();
    }

    /// <summary>
    /// This method populate the object for the SubjectMaster and returns the same.
    /// </summary>
    /// <param name="aiStandardId"></param>
    /// <param name="aiSubjectId"></param>
    /// <returns></returns>
    private SubjectMasterBL SetSubjectMasterBL(int aiStandardId, int aiSubjectId, string asSubjectName)
    {
        SubjectMasterBL oSubjectMasterBL = new SubjectMasterBL();
        oSubjectMasterBL.StandardId = aiStandardId;
        oSubjectMasterBL.SubjectId = aiSubjectId;
        oSubjectMasterBL.SubjectName = asSubjectName;
        oSubjectMasterBL.SchoolId = miSchoolId;
        oSubjectMasterBL.AcademicYearId = miAcademicYearId;
        oSubjectMasterBL.UpdatedById =miUserId;
        return oSubjectMasterBL;
    }

    /// <summary>
    /// This method is used to fill grid with standard names.
    /// </summary>
    private void FillStandardsGrid()
    {
        SchoolWiseStandardSubjectMasterBL obj = new SchoolWiseStandardSubjectMasterBL();
        DataSet oDs = obj.GetStdSubjectAssociation(miSchoolId, miAcademicYearId);
        grdStandards.DataSource = oDs;
        grdStandards.DataBind();
    }

    /// <summary>
    /// This method is used to fill grid with standards and the generate columns of subjects
    /// dynamically to the grid after checking all required configurations are done or not.
    /// </summary>
    private void FillGridWithStandardsAndsubjects()
    {
        FillStandardsGrid();
        GenerateSubjectColumnsOfGrid();
    }

    /// <summary>
    /// This method is used to check preconditions.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;

        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.StandardwiseSubjects);

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

  #endregion
}

