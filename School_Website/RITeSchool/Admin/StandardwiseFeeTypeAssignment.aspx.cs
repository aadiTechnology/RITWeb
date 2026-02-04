// File Name  : StandardwiseTestAssignment.aspx.cs
// Created By : Anugandha
// Date       : 06/02/2008
//Description :This Form is used to assign different fee types
//             to particular standard of a particular school.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class Standardwise_Fee_Type_Assignment : SchoolBase
{
    #region Constants

    private const int I_STANDARD_ID_DATAKEY_NAME = 0;
    private const int I_STANDARD_NAME_DATAKEY_NAME = 2;
    private const int I_START_COUNT = 1;
    private const int I_STDFEETYPE_TABLE_INDEX = 2;
    private const int I_INTERVAL = 0;
    private const int I_FEETYPE = 0;
    private const int I_FEETYPE_ID = 1;
    private const int I_STANDARD_NAME = 1;
    private const int I_INTERVAL_HEADER = 1;

    private const string S_COLUMN_FEE_TYPE_ID = "Fee_Type_Id";
    private const string S_COLUMN_FEE_TYPE_NAME = "Fee_Type";
    private const string S_COLUMN_INTERVAL = "Interval";

    #endregion

    #region Datamembers

    private string mbIsConfig;

    #endregion

    #region Events

    /// <summary>
    /// Overide method for page initialization.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            InitializeMemberVariables();
            if (CheckPreCondition())
            {
                GenerateFeeTypeColumnsOfGrid();
                FillStandardsGrid();
            }
            else
            {
                lblNote.Visible = false;
                Label.Visible = false;
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///This method is used to fill the grid of standards,generating columns of fee types
    /// as per grid and set validations on "save" click.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";
                HidRefURl.Value = getRedirectURL();                
                ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave });
                grdStandards.Columns[0].HeaderText = string.Empty;
                btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Fee_Related));
            }            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save all checked values of standards and tests to the database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int iStandardId;            
            string sInterval = Constants.S_ZERO;
            int iColumnIndex = Convert.ToInt32(hidColumnCount.Value);
            Collection<StandardMasterBL> oStandardCollection = new Collection<StandardMasterBL>();
            DataSet oDSFeeTypes = (DataSet)grdStandards.DataSource;            
            DataTable oDSFeeTypeId = oDSFeeTypes.Tables[I_STDFEETYPE_TABLE_INDEX];
            Collection<SchoolwiseStandardFeeTypeMasterBL> oAllFeeTypeCollection = new Collection<SchoolwiseStandardFeeTypeMasterBL>();

            Collection<SchoolwiseStandardFeeTypeMasterBL> oObjFeeTypeCollection = new Collection<SchoolwiseStandardFeeTypeMasterBL>();
            for (int iRowCount = 0; iRowCount < grdStandards.Rows.Count; iRowCount++)
            {
                StandardMasterBL oStandardMasterBL = new StandardMasterBL();
                Collection<SchoolwiseStandardFeeTypeMasterBL> oFeeTypeCollection = new Collection<SchoolwiseStandardFeeTypeMasterBL>();
                SchoolwiseStandardFeeTypeMasterCollectionBL oSchoolwiseStandardFeeTypeMasterCollectionBL = new SchoolwiseStandardFeeTypeMasterCollectionBL(miSchoolId, miAcademicYearId);
                iStandardId = Convert.ToInt32(grdStandards.DataKeys[iRowCount][I_STANDARD_ID_DATAKEY_NAME]);
                string sStandardName = grdStandards.DataKeys[iRowCount][I_STANDARD_NAME_DATAKEY_NAME].ToString();
                for (int iColumnCount = I_START_COUNT; iColumnCount <= iColumnIndex; iColumnCount++)
                {
                    if (!(grdStandards.Rows[iRowCount].Cells[iColumnCount].Controls[I_INTERVAL] as TextBox).Text.IsNullOrEmpty())
                        sInterval = (grdStandards.Rows[iRowCount].Cells[iColumnCount].Controls[I_INTERVAL] as TextBox).Text;                    
                    
                    string sFeesTypeName = "Standard " + sStandardName + "-" + (grdStandards.HeaderRow.Cells[iColumnCount].Controls[I_FEETYPE] as Label).Text;
                    int iFeeTypeId = Convert.ToInt32((grdStandards.Rows[iRowCount].Cells[iColumnCount].Controls[I_FEETYPE_ID] as HiddenField).Value);
                    DataRow[] oDrFeeTypes = oDSFeeTypeId.Select("Standard_Id = " + iStandardId + " AND Fee_Type_Id= " + iFeeTypeId);
                    
                    if (!sInterval.IsNullOrEmpty() && sInterval != Constants.S_ZERO && (oDrFeeTypes.Length == 0 || oDrFeeTypes[0]["SchoolWise_Standard_FeeType_Id"].ToString()==Constants.S_ZERO))
                    {
                        SchoolwiseStandardFeeTypeMasterBL oSchoolwiseStandardFeeTypeMasterBL = SetSchoolwiseStandardFeeTypeMasterBL(iStandardId, iFeeTypeId, sFeesTypeName, 0, sInterval.ToInt());
                        oSchoolwiseStandardFeeTypeMasterBL.ConfigurationAction = Constants.Action.Insert;
                        oFeeTypeCollection.Add(oSchoolwiseStandardFeeTypeMasterBL);                        
                    }
                    else if (!sInterval.IsNullOrEmpty() && sInterval != Constants.S_ZERO && (oDrFeeTypes.Length != 0) && oDrFeeTypes[0]["SchoolWise_Standard_FeeType_Id"].ToString() != Constants.S_ZERO)
                    {
                        int iInterval=0;
                        if (!sInterval.IsNullOrEmpty())                            
                            iInterval = sInterval.ToInt();

                        SchoolwiseStandardFeeTypeMasterBL oSchoolwiseStandardFeeTypeMasterBL = SetSchoolwiseStandardFeeTypeMasterBL(iStandardId, iFeeTypeId, sFeesTypeName, Convert.ToInt32(oDrFeeTypes[0]["SchoolWise_Standard_FeeType_Id"]), iInterval);
                        oSchoolwiseStandardFeeTypeMasterBL.ConfigurationAction = Constants.Action.Update;
                        oFeeTypeCollection.Add(oSchoolwiseStandardFeeTypeMasterBL);

                        if (oSchoolwiseStandardFeeTypeMasterBL.Interval != oDrFeeTypes[0][S_COLUMN_INTERVAL].ToInt())
                           oObjFeeTypeCollection.Add(oSchoolwiseStandardFeeTypeMasterBL);
                    }
                    else if ((sInterval.IsNullOrEmpty() || sInterval == Constants.S_ZERO) && (oDrFeeTypes.Length != 0) && oDrFeeTypes[0]["SchoolWise_Standard_FeeType_Id"].ToString() != Constants.S_ZERO)                    
                    {
                        SchoolwiseStandardFeeTypeMasterBL oSchoolwiseStandardFeeTypeMasterBL = SetSchoolwiseStandardFeeTypeMasterBL(iStandardId, iFeeTypeId, sFeesTypeName, Convert.ToInt32(oDrFeeTypes[0]["SchoolWise_Standard_FeeType_Id"]), sInterval.ToInt());
                        oSchoolwiseStandardFeeTypeMasterBL.ConfigurationAction = Constants.Action.Delete;
                        oFeeTypeCollection.Add(oSchoolwiseStandardFeeTypeMasterBL);
                        oAllFeeTypeCollection.Add(oSchoolwiseStandardFeeTypeMasterBL);
                    }
                }

                if (oFeeTypeCollection.Count > 0)
                {
                    oStandardMasterBL.FeeTypeCollection = oFeeTypeCollection;
                    oStandardCollection.Add(oStandardMasterBL);
                }
            }

            if (oStandardCollection.Count > 0)
            {
                CheckDependencies(oAllFeeTypeCollection);
                CheckDependencies(oObjFeeTypeCollection);
                StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId);
                oStandardCollectionBL.UpdateStandardFeeTypes(oStandardCollection);
            }

            ReadQuerystring();
            if (mbIsConfig != "Y")            
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.StandardwiseFeeTypes));
            
            MasterPage oMasterPage = (MasterPage)this.Master; oMasterPage.RedirectToNextPage(HidRefURl.Value);
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblErr.Text = CommonUtility.ModifyExceptionMessage(ex.Message, "Standard", Resources.LocalizedResources.Standard, "fees can not be modified since associated with", Resources.LocalizedResources.valFeeRemoveText);
            FillGridWithStandardsAndFeeTypes();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Grid Events

    /// <summary>
    /// This method is used to set the standard name in the first column and also
    /// set attributes property to each row of the grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStandards_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                string sName = grdStandards.DataKeys[e.Row.RowIndex][I_STANDARD_NAME_DATAKEY_NAME].ToString();
                Label lblStandard = e.Row.Cells[0].Controls[I_STANDARD_NAME] as Label;
                lblStandard.Text = sName;               

                if (e.Row.RowIndex == 0)
                {
                    TextBox oTextBox = grdStandards.HeaderRow.Cells[1].Controls[I_INTERVAL_HEADER] as TextBox;                    
                    if (oTextBox != null)
                        oTextBox.Focus();
                }
            }
            
            
        
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is for the AllowPaging propetry of the grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStandards_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdStandards.PageIndex = e.NewPageIndex;
            FillGridWithStandardsAndFeeTypes();
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

    #region Private Methods

    /// <summary>
    /// This function is used to check the dependencies over latefee settings.
    /// </summary>
    /// <param name="aoStdFeeTypeCollection"></param>
    private void CheckDependencies(Collection<SchoolwiseStandardFeeTypeMasterBL> aoStdFeeTypeCollection)
    {
        if (aoStdFeeTypeCollection.Count > 0)
        {
            GenericReferenceList<SchoolwiseStandardFeeTypeMasterBL> objStdFeesRefereces = new GenericReferenceList<SchoolwiseStandardFeeTypeMasterBL>(aoStdFeeTypeCollection, miAcademicYearId);
            objStdFeesRefereces.CheckDependenciesAndThrowException("SchoolWise_Standard_FeeType_Id", "StandardFeeTypeName", Constants.ReferenceId.StandardFees);
        }
    }

    /// <summary>
    /// This function return the URL for back button.
    /// </summary>
    /// <returns></returns>
    private string getRedirectURL()
    {
        string sUrl;
        int iSegCnt = HttpContext.Current.Request.UrlReferrer.Segments.Length;
        if (HttpContext.Current.Request.UrlReferrer.Segments[iSegCnt - 1].Equals("schoolconfigurationcontrolpanel.aspx"))
            sUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Fee_Related)); 
        else
            sUrl = HttpContext.Current.Request.UrlReferrer.ToString();

        return sUrl;
    }

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        try
        {
            mbIsConfig = QueryString["Is_Configured"];
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
			oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    /// <summary>
    /// This method is used to fill grid with standards and to generate columns of fee types
    /// dynamically to the grid after checking all required configurations.
    /// </summary>
    /// 
    private void FillGridWithStandardsAndFeeTypes()
    {
        if (CheckPreCondition())
            FillStandardsGrid();        
    }

    /// <summary>
    /// This function checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.StandardwiseFeeTypes);
        if (sLinks.IsNullOrEmpty())
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
    /// This method is used to visible or hide controls depends 
    /// on configuration is done or not.
    /// </summary>
    private void VisibleOrHideControls()
    {
        btnSave.Visible = false;
        divGridView.Visible = false;
        btnCancel.Visible = true;
        btnCancel.Text = "Back";
    }

    /// <summary>
    /// This method is used to fill grid with standard names.
    /// </summary>
    private void FillStandardsGrid()
    {
        SchoolwiseStandardFeeTypeMasterCollectionBL obj = new SchoolwiseStandardFeeTypeMasterCollectionBL(miSchoolId, miAcademicYearId);
        DataSet oDSStandardwiseFeeDetails = ViewState["StandardwiseFeeDetails"] as DataSet;
        grdStandards.DataSource = oDSStandardwiseFeeDetails;
        grdStandards.DataBind();
        SetValuesForGrid(oDSStandardwiseFeeDetails);
    }

    /// <summary>
    /// This method is used to generate columns of fee types of grid dynamically
    /// which is attached to grid one by one and show checkbox is checked true when the test
    /// is already assigned to that standard.
    /// </summary>
    private void GenerateFeeTypeColumnsOfGrid()
    {
        SchoolwiseStandardFeeTypeMasterCollectionBL oSchoolwiseStandardFeeTypeMasterCollectionBL = new SchoolwiseStandardFeeTypeMasterCollectionBL(miSchoolId, miAcademicYearId);
        DataSet oDSDetails = oSchoolwiseStandardFeeTypeMasterCollectionBL.GetStdExamAssociation();
        ViewState["StandardwiseFeeDetails"] = oDSDetails;  
        const int I_FEETYPE_TABLE_INDEX = 1;
        DataTable oDtFeeTypes = oDSDetails.Tables[I_FEETYPE_TABLE_INDEX];
        DataTable oDtStdFeeTypes = oDSDetails.Tables[I_STDFEETYPE_TABLE_INDEX];

        for (int iRowIndex = 0; iRowIndex < oDtFeeTypes.Rows.Count; iRowIndex++)
        {
            TemplateField oTemplateField;

            //Here we initilizes templates for the gridview.
            oTemplateField = new TemplateField
            {
                HeaderTemplate = new GridviewTextBoxItemTemplate(DataControlRowType.Header, oDtFeeTypes.Rows[iRowIndex][S_COLUMN_FEE_TYPE_NAME].ToString(), iRowIndex, oDtFeeTypes.Rows[iRowIndex][S_COLUMN_FEE_TYPE_ID].ToInt())                
            };

            foreach (DataRow oDataRow in oDtStdFeeTypes.Rows)
            {
                oTemplateField = new TemplateField
                {
                    ItemTemplate = new GridviewTextBoxItemTemplate(DataControlRowType.DataRow, oDataRow[S_COLUMN_INTERVAL].ToString(), iRowIndex, oDtFeeTypes.Rows[iRowIndex][S_COLUMN_FEE_TYPE_ID].ToInt()),
                    HeaderTemplate = new GridviewTextBoxItemTemplate(DataControlRowType.Header, oDtFeeTypes.Rows[iRowIndex][S_COLUMN_FEE_TYPE_NAME].ToString(), iRowIndex, oDtFeeTypes.Rows[iRowIndex][S_COLUMN_FEE_TYPE_ID].ToInt())                                       
                };
                oTemplateField.ItemStyle.HorizontalAlign = HorizontalAlign.Center;                
            }

            grdStandards.Columns.Add(oTemplateField);
            hidColumnCount.Value = oDtFeeTypes.Rows.Count.ToString();
        }
    }

    /// <summary>
    /// This is used to set the values for the interval in the gridview.
    /// </summary>
    /// <param name="aoStandardwiseFeeDetails"></param>
    private void SetValuesForGrid(DataSet aoStandardwiseFeeDetails)
    {
        const int I_FEETYPE_TABLE_INDEX = 1;
        DataTable oDtFeeTypes = aoStandardwiseFeeDetails.Tables[I_FEETYPE_TABLE_INDEX];
        DataTable oDtStdFeeTypes = aoStandardwiseFeeDetails.Tables[I_STDFEETYPE_TABLE_INDEX];

        for (int iRowIndex = 0; iRowIndex < grdStandards.Rows.Count; iRowIndex++)
        {
            int iStandardId = Convert.ToInt32(grdStandards.DataKeys[iRowIndex]["standard_id"]);
            for (int iCellCount = 1; iCellCount <= oDtFeeTypes.Rows.Count; iCellCount++)
            {
                int iFeeTypeId = (grdStandards.Rows[iRowIndex].Cells[iCellCount].Controls[I_FEETYPE_ID] as HiddenField).Value.ToInt();
                TextBox oTextBox = grdStandards.Rows[iRowIndex].Cells[iCellCount].Controls[I_INTERVAL] as TextBox;
                DataRow[] oDRDetails = oDtStdFeeTypes.Select("Standard_Id=" + iStandardId + " AND Fee_Type_Id=" + iFeeTypeId);
                if (oDRDetails.Length > 0)
                    oTextBox.Text = oDRDetails[0]["Interval"].ToString();
                else
                    oTextBox.Text = Constants.S_ZERO;
            }            
        }
    }
    
    /// <summary>
    /// This method is used to set the properties of SchoolwiseStandardFeeTypeMasterBL class.
    /// is already assigned to that standard.
    /// </summary>
    private SchoolwiseStandardFeeTypeMasterBL SetSchoolwiseStandardFeeTypeMasterBL(int aiStandardId, int aiFeeTypeId, string asFeeTypeName, int aiStdFeeTypeId,int aiInterval)
    {
        SchoolwiseStandardFeeTypeMasterBL oSchoolwiseStandardFeeTypeMasterBL;        
        return oSchoolwiseStandardFeeTypeMasterBL = new SchoolwiseStandardFeeTypeMasterBL
        {
            Standard_Id = aiStandardId,
            Fee_Type_Id = aiFeeTypeId,
            School_Id = miSchoolId,
            academic_Year_Id = miAcademicYearId,
            Inserted_By_id = Convert.ToString(miUserId),
            Updated_By_Id = Convert.ToString(miUserId),
            StandardFeeTypeName = asFeeTypeName,
            SchoolWise_Standard_FeeType_Id = aiStdFeeTypeId,
            Interval = aiInterval
        };
    }

    #endregion
}

/// <summary>
/// This class is used to define gridview ItemTemplate.
/// </summary>
public class GridviewTextBoxItemTemplate : ITemplate
{
    #region Members

    private readonly DataControlRowType moTemplateType;
    private string msFeeType;    
    private int miRowId;
    private int miFeeTypeId;

    #endregion

    /// <summary>
    /// This is constructor called for Initialization of values.
    /// </summary>
    /// <param name="aoFeeType"></param>
    /// <param name="asFeeType"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiFeeTypeId"></param>
    public GridviewTextBoxItemTemplate(DataControlRowType aoRowType, string asHeaderText, int aiRowIndex, int aiRowID)
    {
        moTemplateType = aoRowType;
        msFeeType = asHeaderText;        
        miRowId = aiRowIndex;
        miFeeTypeId = aiRowID;
    }    

    public void InstantiateIn(Control aoContainer)
    {
        // Create the content for the different row types.
        switch (moTemplateType)
        {
            case DataControlRowType.Header:
                // Create the controls to put in the header                
                Label lblFeeType = new Label();
                lblFeeType.ID = miFeeTypeId.ToString();                
                lblFeeType.Text = msFeeType;
                lblFeeType.Style.Add("padding-bottom", "5px");
                lblFeeType.Style.Add("padding-right", "7px");
                lblFeeType.Style.Add("white-space", "nowrap");
                TextBox txtFeeType = new TextBox();                
                txtFeeType.ID = "ctl" + miRowId;
                SetTextBoxProperties(txtFeeType);                
                HiddenField hidFeeTypeId = new HiddenField();
                hidFeeTypeId.ID = "hid_" + miFeeTypeId + "_" + miRowId;
                hidFeeTypeId.Value = hidFeeTypeId + "" + miFeeTypeId.ToString();    
                txtFeeType.Attributes.Add("onchange", "CheckAll(this, " + miRowId + ")");                
                aoContainer.Controls.Add(lblFeeType);
                aoContainer.Controls.Add(txtFeeType);
                aoContainer.Controls.Add(hidFeeTypeId);              
                
                break;

            case DataControlRowType.DataRow:
                // Create the controls to put in a data row section and set their properties.                    
                TextBox oTextBox = new TextBox();                                
                oTextBox.ID = "ctl" + (miRowId);                
                SetTextBoxProperties(oTextBox);
                HiddenField hidFeeTypesId = new HiddenField();
                hidFeeTypesId.ID = "hid_" + miFeeTypeId + "_" + miRowId;
                hidFeeTypesId.Value = miFeeTypeId.ToString();
                aoContainer.Controls.Add(oTextBox);
                aoContainer.Controls.Add(hidFeeTypesId);                
                break;            

            default:                
                break;
        }
    }

    #region Private Methods
    /// <summary>
    /// This is a common function used to set properties for all textboxes in grid.
    /// </summary>
    /// <param name="txtFeeType"></param>
    private void SetTextBoxProperties(TextBox txtFeeType)
    {
        txtFeeType.MaxLength = 2;
        txtFeeType.TextMode = TextBoxMode.SingleLine;
        txtFeeType.CssClass = "SmlTxtBox";
        txtFeeType.Width = Unit.Pixel(50);
        txtFeeType.Height = Unit.Pixel(20);
        txtFeeType.Attributes.Add("onkeyup", "extractNumber(this, 1 ,false);");
        txtFeeType.Attributes.Add("onkeypress", "return blockNonNumbers(this, event, false, false);");
        txtFeeType.Attributes.Add("onpaste", "event.returnValue=false;");
        txtFeeType.Attributes.Add("ondrop", "event.returnValue=false;");
        txtFeeType.Attributes.Add("onblur", "extractNumber(this,2,false);");
    }

    #endregion
}
