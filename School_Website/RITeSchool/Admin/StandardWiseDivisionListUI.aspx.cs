// File Name     : StandardWiseDivisionListUI.aspx.cs
// Modified By   : Amit 
// Modified Date : 19/09/2009
// Description   : This class is used to configure divisions for each standard.

using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;

public partial class StandardWiseDivisionListUI : SchoolBase
{
    #region Constants

    const string S_SELECT_AT_LEAST_ONE_STD_FOR_DIV = "Atleast one standard should be assigned for each division.";
    const string S_SELECT_AT_LEAST_ONE_GROUP = "Atleast one division should be selected for each standard.";

    #region Standards
    const Int32 I_ORIGINAL_STANDARD_ID_COLUMN_NUMBER = 2;
    const Int32 I_STANDARD_ID_COLUMN_NUMBER = 1;
    const Int32 I_STANDARD_NAME_COLUMN_NUMBER = 3;
    const Int32 I_START_COUNT = 4;
    #endregion

    #endregion

    #region event handlers

    /// <summary>
    /// This event is used to fill standardwise division grid, 
    /// And to set javascript propeties to controls.
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
                SetClientSideAttributes();
                if (CheckPreCondition())
                    FillStandardGrid();
                RefreshValue();
            }
            else
            {
                FillStandardGrid();
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
            lblError.Text = CommonUtility.ModifyExceptionMessage(ex.Message, "Standard-Division", Resources.LocalizedResources.StandardDivision, "can not be removed since associated with", Resources.LocalizedResources.valRemoveText);
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set CSS style and names to grid checkboxes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStandandard_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].CssClass = "locked";
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].CssClass = "Llocked";
                e.Row.Cells[0].Style.Add("left", grdStandards.Style["scrollLeft"]);
            }
            if (e.Row.RowIndex >= 0)
            {
                // Set the standard name in the textbox.
                string sName = Convert.ToString(e.Row.Cells[I_STANDARD_NAME_COLUMN_NUMBER].Text);
                CheckBox chkCheckAll = ((CheckBox)e.Row.Cells[1].FindControl("CheckAllForRow"));
                chkCheckAll.Text = sName;
                int iRowNo = e.Row.RowIndex;
                chkCheckAll.Attributes.Add("onclick", "CheckUncheckAllInRow(this," + iRowNo + ",'" + grdStandards.AllowPaging + "')");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save standardwise division configuration for school.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = string.Empty;
            lblErr.Text = string.Empty;
            DataSet oDsSrc = (DataSet)grdStandards.DataSource;            
            int iStandardId;
            int iStdCount = grdStandards.Rows.Count;
            int iCountCols = Convert.ToInt32(hidRowCount.Value);
            CheckBox oChkDivision;
            TextBox otxtDivision=null;
            HiddenField ohidDisplayName=null;
            DataTable oDtClass = oDsSrc.Tables[2];
            SetPrimaryKey(oDtClass);
            Collection<StandardMasterBL> oStandardCollection = new Collection<StandardMasterBL>();
            object[] objPrimaryKey = new object[2];
            Collection<StandardDivisionMasterBL> oStdDivCollection = new Collection<StandardDivisionMasterBL>();

            for (int i = 0; i < iStdCount; i++)
            {
                StandardMasterBL oStandardMasterBL = new StandardMasterBL();
                Collection<DivisionMasterBL> oDivisionCollection = new Collection<DivisionMasterBL>();
                iStandardId = Convert.ToInt32(grdStandards.Rows[i].Cells[I_STANDARD_ID_COLUMN_NUMBER].Text);

                for (int iStandardIndex = I_START_COUNT; iStandardIndex <= iCountCols; iStandardIndex += 2)
                {

                    oChkDivision = (CheckBox)(grdStandards.Rows[i].Cells[iStandardIndex].Controls[0]);

                    if (grdStandards.Rows[i].Cells[iStandardIndex + 1].Controls.Count > 1)
                    {
                        ohidDisplayName = (HiddenField)(grdStandards.Rows[i].Cells[iStandardIndex + 1].Controls[0]);
                        otxtDivision = (TextBox)(grdStandards.Rows[i].Cells[iStandardIndex + 1].Controls[1]);
                    }
                    else
                    {
                        otxtDivision = (TextBox)(grdStandards.Rows[i].Cells[iStandardIndex + 1].Controls[0]);
                    }

                    int iDivisionId = Convert.ToInt32(grdStandards.Rows[i].Cells[iStandardIndex].Text);                    
                    string sDivisionName = grdStandards.HeaderRow.Cells[iStandardIndex].Text;
                    objPrimaryKey[0] = iStandardId;
                    objPrimaryKey[1] = iDivisionId;
                    DataRow oDrCurrentClass = oDtClass.Rows.Find(objPrimaryKey);
                    //insert
                    if (oChkDivision.Checked == true && (oDrCurrentClass == null))
                    {
                        DivisionMasterBL oDivisionMasterBL = SetDivisionMasterBL(iStandardId, iDivisionId, sDivisionName, otxtDivision.Text);
                        oDivisionMasterBL.ConfigurationAction = Constants.Action.Insert;
                        oDivisionCollection.Add(oDivisionMasterBL);
                    }
                    //delete
                    else if (oChkDivision.Checked == false && ((oDrCurrentClass != null)))
                    {
                        DivisionMasterBL oDivisionMasterBL = SetDivisionMasterBL(iStandardId, iDivisionId, sDivisionName, otxtDivision.Text);
                        oDivisionMasterBL.ConfigurationAction = Constants.Action.Delete;
                        oDivisionCollection.Add(oDivisionMasterBL);
                        StandardDivisionMasterBL oStdDivBL = SetStdDivObject(oDrCurrentClass["StdDivName"].ToString(), Convert.ToInt32(oDrCurrentClass["SchoolWise_Standard_Division_Id"]));
                        oStdDivCollection.Add(oStdDivBL);
                    }
                    //update
                    else if (oChkDivision.Checked == true &&  ((!string.IsNullOrEmpty(otxtDivision.Text) && ohidDisplayName == null) || 
                             (ohidDisplayName != null && !ohidDisplayName.Value.Equals(otxtDivision.Text))))
                    {
                        DivisionMasterBL oDivisionMasterBL = SetDivisionMasterBL(iStandardId, iDivisionId, sDivisionName, otxtDivision.Text);
                        oDivisionMasterBL.ConfigurationAction = Constants.Action.Update;
                        oDivisionCollection.Add(oDivisionMasterBL);
                    }
                }
                if (oDivisionCollection.Count > 0)
                {
                    oStandardMasterBL.StandardName = grdStandards.Rows[i].Cells[I_STANDARD_NAME_COLUMN_NUMBER].Text;
                    oStandardMasterBL.DivisionCollection = oDivisionCollection;
                    oStandardCollection.Add(oStandardMasterBL);
                }
            }

            // If there are Divisions to be deleted then give warning message to user about the same.
            // Update database with the configured Divisions.
            if (oStandardCollection.Count > 0)
            {
                CheckDependencies(oStdDivCollection, miAcademicYearId);
                StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
                oStandardCollectionBL.UpdateStandardDivisions(oStandardCollection);
            }
            string sIsConfig = ReadQuerystring();
            if (sIsConfig != "Y")
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.StandardwiseDivision));

            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration)));
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblErr.Text = CommonUtility.ModifyExceptionMessage(ex.Message, "Standard-Division", Resources.LocalizedResources.StandardDivision, "can not be removed since associated with", Resources.LocalizedResources.valRemoveText);
            FillStandardGrid();
        }
        catch (Exception ex)
        {
            lblError.Text = CommonUtility.ModifyExceptionMessage(ex.Message, "Standard-Division", Resources.LocalizedResources.StandardDivision, "can not be removed since associated with", Resources.LocalizedResources.valRemoveText);
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region private methods

    /// <summary>
    /// This method is used to check dependancie of standard-divisions with other school configurations. 
    /// </summary>
    /// <param name="oStdDivCollection"></param>
    /// <param name="aiAcademicYearId"></param>
    private void CheckDependencies(Collection<StandardDivisionMasterBL> oStdDivCollection, int aiAcademicYearId)
    {
        if (oStdDivCollection.Count > 0)
        {
            GenericReferenceList<StandardDivisionMasterBL> objStdDivsRefereces = new GenericReferenceList<StandardDivisionMasterBL>(oStdDivCollection, aiAcademicYearId);
            objStdDivsRefereces.CheckDependenciesAndThrowException("StandardDIvisionId", "StandardDivisionName", "ConfigurationAction", Constants.ReferenceId.StandardwiseDivision, false);
        }
    }

    /// <summary>
    /// This method is used to set primary key for data table.
    /// </summary>
    /// <param name="aoDtClass"></param>
    private void SetPrimaryKey(DataTable aoDtClass)
    {
        DataColumn[] oDtCols = new DataColumn[2];
        oDtCols[0] = aoDtClass.Columns["standard_Id"];
        oDtCols[1] = aoDtClass.Columns["division_Id"];
        aoDtClass.PrimaryKey = oDtCols;
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
    /// This method is used to add client side java scripts to controls.
    /// </summary>
    private void SetClientSideAttributes()
    {
        btnSave.Attributes.Add("onclick", "if(!saveChk('" + Resources.LocalizedResources.sSelectAtLeastOneGroup + "' , '"
                                    + Resources.LocalizedResources.sSelectAtLeastOneStdForDiv + "',this)){return false}");
        btnCancel.Attributes["onclick"] = "javascript:DisableButtons(this)";        
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave });
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration));
        grdStandards.Columns[0].HeaderText = "";
    }

    /// <summary>
    /// This method is used to generate standard-division grid columns.
    /// </summary>
    private void GenerateColumns()
    {
        const int I_DIV_TABLE_INDEX = 1;
        const int I_STDDIV_TABLE_INDEX = 2;
        DataSet oDs = (DataSet)grdStandards.DataSource;
        DataTable oDtDiv = oDs.Tables[I_DIV_TABLE_INDEX];
        DataTable oDtStdDiv = oDs.Tables[I_STDDIV_TABLE_INDEX];

        int iDivisionCount = oDtDiv.Rows.Count;
        int iStandardCount = grdStandards.Rows.Count;
        int k = 0;
        int headerCellNo = 0;

        //Add Division  checkbox to other rows
        for (int iStandardIndex = 0; iStandardIndex < iStandardCount; iStandardIndex++)
        {
            int iStandardId = Convert.ToInt32(grdStandards.Rows[iStandardIndex].Cells[I_STANDARD_ID_COLUMN_NUMBER].Text);
            for (int iDivisionIndex = 0; iDivisionIndex < iDivisionCount; iDivisionIndex++)
            {
                if (iStandardIndex == 0)
                {
                    DataControlFieldHeaderCell oTHeader = new DataControlFieldHeaderCell(null);
                    oTHeader.CssClass = "locked";
                    oTHeader.HorizontalAlign = HorizontalAlign.Center;
                    oTHeader.Wrap = false;
                    oTHeader.Style.Add(HtmlTextWriterStyle.PaddingLeft, "3");
                    oTHeader.Style.Add(HtmlTextWriterStyle.PaddingRight, "3");
                    oTHeader.Width = System.Web.UI.WebControls.Unit.Point(900);
                    oTHeader.Text = oDtDiv.Rows[iDivisionIndex][Constants.S_DIVISION_NAME_FIELD].ToString();
                    k = grdStandards.HeaderRow.Cells.Add(oTHeader);
                    if (k > 0)
                    {
                        CheckBox oChkHeader = new CheckBox();
                        oChkHeader.Text = oDtDiv.Rows[iDivisionIndex][Constants.S_DIVISION_NAME_FIELD].ToString();
                        grdStandards.HeaderRow.Cells[k].Controls.Add(oChkHeader);                       
                        oChkHeader.Attributes.Add("onclick", "CheckAll(this, " + headerCellNo + ", '" + grdStandards.AllowPaging + "')");
                        headerCellNo = headerCellNo + 3;
                    }

                    oTHeader = new DataControlFieldHeaderCell(null);
                    oTHeader.CssClass = "locked";
                    oTHeader.HorizontalAlign = HorizontalAlign.Center;
                    oTHeader.Wrap = false;
                    oTHeader.Style.Add(HtmlTextWriterStyle.PaddingLeft, "3");
                    oTHeader.Style.Add(HtmlTextWriterStyle.PaddingRight, "3");
                    oTHeader.Width = System.Web.UI.WebControls.Unit.Point(900);
                    oTHeader.Text = Resources.LocalizedResources.DisplayName;
                    k = grdStandards.HeaderRow.Cells.Add(oTHeader);
                }
                TableCell oT = new TableCell();
                CheckBox oChk = new CheckBox();
                oT.Width = 100;
                oT.HorizontalAlign = HorizontalAlign.Center;
                oT.Width = System.Web.UI.WebControls.Unit.Point(900);
                oT.Attributes.Add("title", "Std. " + oDs.Tables[0].Rows[iStandardIndex]["Standard_name"].ToString() + " [" + oDs.Tables[1].Rows[iDivisionIndex]["division_name"].ToString() + "]");
                oT.Text = oDtDiv.Rows[iDivisionIndex][Constants.S_DIVISION_ID_FIELD].ToString();
                k = grdStandards.Rows[iStandardIndex].Cells.Add(oT);
                int iDiv = Convert.ToInt32(grdStandards.Rows[iStandardIndex].Cells[k].Text);
                DataRow[] oDr = oDtStdDiv.Select("division_id = " + iDiv.ToString() + " AND Standard_Id = " + iStandardId.ToString());
                if (oDr.Length > 0)
                    oChk.Checked = true;
                grdStandards.Rows[iStandardIndex].Cells[k].Controls.Add(oChk);

                oT = new TableCell();
                TextBox oTextBox = new TextBox();
                oTextBox.Attributes.Add("onblur", "formatName(this)");
                HiddenField hidDisplayName = new HiddenField();
                oTextBox.MaxLength = 15;
                oT.Width = 120;
                oT.HorizontalAlign = HorizontalAlign.Center;
                oT.Width = System.Web.UI.WebControls.Unit.Point(900);
                k = grdStandards.Rows[iStandardIndex].Cells.Add(oT);
                if (oDr.Length > 0 && oDr[0]["DisplayNameForDivision"] != DBNull.Value)
                {
                    oTextBox.Text = oDr[0]["DisplayNameForDivision"].ToString();                   
                    hidDisplayName.Value = oTextBox.Text;
                    
                }
                grdStandards.Rows[iStandardIndex].Cells[k].Controls.Add(hidDisplayName);
                grdStandards.Rows[iStandardIndex].Cells[k].Controls.Add(oTextBox);

            }
        }
        hidRowCount.Value = k.ToString();
    }

    /// <summary>
    /// This method is used to check pre-condition to configure divisions for standard. 
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.StandardwiseDivision);

        if (!sLinks.Equals(""))
        {
            divErr.InnerHtml = sLinks;
            VisibleOrHideControls();
        }
        else
        {
            divErr.Visible = false;
            bReturn = true;
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to visible or hide controls on page load 
    /// as per configuration is done or not.
    /// </summary>
    private void VisibleOrHideControls()
    {
        chkAll.Visible = false;
        btnSave.Visible = false;
        grdStandards.Visible = false;
        btnCancel.Text = "Back";
        tdGrid.Visible = false;
    }

    /// <summary>
    /// This method is used to fill standard-division grid. 
    /// </summary>
    private void FillStandardGrid()
    {
        grdColsVisible(true);        
        StandardDivisionCollectionBL obj = new StandardDivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataSet oDS = obj.GetStdDivAssociation();
        grdStandards.DataSource = oDS;
        grdStandards.DataBind();
        grdColsVisible(false);
        GenerateColumns();
    }

    /// <summary>
    /// This method is used to show/hide columns in grid.
    /// </summary>
    /// <param name="abAction"></param>
    private void grdColsVisible(bool abAction)
    {
        // This method hides the Groupid column from Gridview grdStandards.
        grdStandards.Columns[I_STANDARD_ID_COLUMN_NUMBER].Visible = abAction;
        grdStandards.Columns[I_ORIGINAL_STANDARD_ID_COLUMN_NUMBER].Visible = abAction;
        grdStandards.Columns[I_STANDARD_NAME_COLUMN_NUMBER].Visible = abAction;
    }


    /// <summary>
    /// This method is used to pupulate StandardDivisionMasterBL object.
    /// </summary>
    /// <param name="asStdDivNameName"></param>
    /// <param name="aiStdDivId"></param>
    /// <returns></returns>
    private StandardDivisionMasterBL SetStdDivObject(string asStdDivNameName, int aiStdDivId)
    {
        StandardDivisionMasterBL oStdDivBL = new StandardDivisionMasterBL();
        oStdDivBL.StandardDIvisionId = aiStdDivId;
        oStdDivBL.StandardDivisionName = asStdDivNameName;
        oStdDivBL.ConfigurationAction = Constants.Action.Delete;
        return oStdDivBL;
    }

    /// <summary>
    /// This method is used to populate DivisionMasterBL object.
    /// </summary>
    /// <param name="aiStandardId"></param>
    /// <param name="aiDivisionId"></param>
    /// <param name="asDivisionName"></param>
    /// <returns></returns>
    private DivisionMasterBL SetDivisionMasterBL(int aiStandardId, int aiDivisionId, string asDivisionName,string asDisplayName)
    {
        // This method creates the default object for the configuration and returns the same.
        DivisionMasterBL oDivisionMasterBL = new DivisionMasterBL();
        oDivisionMasterBL.StandardId = aiStandardId;
        oDivisionMasterBL.DisplayName = asDisplayName;
        oDivisionMasterBL.DivisionId = aiDivisionId;
        oDivisionMasterBL.DivisionName = asDivisionName;
        oDivisionMasterBL.SchoolId = miSchoolId;
        oDivisionMasterBL.AcademicYearId = miAcademicYearId;
        oDivisionMasterBL.UpdatedById = miUserId;
        return oDivisionMasterBL;
    }
    /// <summary>
    /// This method used to value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        hidFirstFxFollowingErrors.Value = Resources.LocalizedResources.PleaseFixFollowingError;
        btnSave.Attributes.Add("onclick", "if(!saveChk('" + Resources.LocalizedResources.sSelectAtLeastOneGroup + "' , '"
                                  + Resources.LocalizedResources.sSelectAtLeastOneStdForDiv + "',this)){return false}");
    }
    #endregion
}
