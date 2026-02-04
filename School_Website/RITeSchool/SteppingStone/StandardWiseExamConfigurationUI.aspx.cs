using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using BusinessLogic;
using BusinessLogic.Exceptions;
using StandardWiseExamConfigurationEntities;
using Utility;

public partial class StandardWiseExamConfigurationUI : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
			if (!IsPostBack)
            {
                FillGridWithStandardsAndtests();
                lstvwTests.Visible = false;
                btnSave.Visible = false;
                SetJavaScriptAttributes();
            }
        }
        catch (Exception ex)
        {
	        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void SetJavaScriptAttributes()
    {
        hlnkSortOrder.Attributes.Add("onclick", "window.open('../Admin/TestsSortOrderPopUp.Aspx?" + Server.UrlDecode(Request.QueryString.ToString())
                                                  + "', '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=650,height=530');return false;");
        new Button[] { btnCancel, btnSave }.ApplyEffect();
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Exam_Related));
    }

    protected void lstvwTests_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            CheckBox ochkSelect = ((CheckBox)oCurrentItem.FindControl("ChkSelect"));
            ochkSelect.Attributes.Add("onclick", "EnableDisableControlsOfRow(this,'" + oCurrentItem.DisplayIndex + "','" + Convert.ToString(lstvwTests.DataKeys[oCurrentItem.DisplayIndex]["IsPublished"]) + "')");
            TextBox otxtConsiderMarksOutOf = ((TextBox)oCurrentItem.FindControl("txtConsiderMarksOutOf"));
            if (Convert.ToInt32(lstvwTests.DataKeys[oCurrentItem.DisplayIndex]["SchoolwiseStandardTestId"]) != 0)
            {
                if (Convert.ToString(lstvwTests.DataKeys[oCurrentItem.DisplayIndex]["OutOfMarks"]) != Constants.I_ZERO.ToString())
                    otxtConsiderMarksOutOf.Text = Convert.ToString(lstvwTests.DataKeys[oCurrentItem.DisplayIndex]["OutOfMarks"]);
                ochkSelect.Checked = true;
                otxtConsiderMarksOutOf.Enabled = true;
                if (Convert.ToString(lstvwTests.DataKeys[oCurrentItem.DisplayIndex]["IsPublished"]) == Constants.S_YES)
                    otxtConsiderMarksOutOf.Enabled = false;
            }
        }
        catch (Exception ex)
        {
	        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill grid with standards and the generate columns of tests
    /// dynamically to the grid after checking all required configurations.
    /// </summary>
    private void FillGridWithStandardsAndtests()
    {
        if (CheckPreCondition())
        {
            FillStandardsGrid();
        }
    }

    /// <summary>
    /// This method is used to fill grid with standard names.
    /// </summary>
    private void FillStandardsGrid()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDTStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDTStandardCollection, ref cmbStandard, Constants.S_STANDARD_ID_FIELD, Constants.S_STANDARD_NAME_FIELD, Constants.S_SELECT);
        FillTests();
    }

    private void FillTests()
    {
        StandardWiseExamConfigurationBL oStandardWiseExamConfigurationBL = new StandardWiseExamConfigurationBL(miSchoolId, miAcademicYearId);
        lstvwTests.DataSource = oStandardWiseExamConfigurationBL.GetExamsForStandard(Convert.ToInt32(cmbStandard.SelectedValue));
        lstvwTests.DataBind();
    }


    /// <summary>
    /// This function checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {

        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.StandardWiseExamConfiguration);

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
    /// This method is used to visible or hide controls depends 
    /// on configuration is done or not.
    /// </summary>
    private void VisibleOrHideControls()
    {
        btnSave.Visible = false;
        //divGridView.Visible = false;
        btnCancel.Text = "Back";
        //ListViewScrollContainer.Visible = false;
    }
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbStandard.SelectedValue != Constants.I_ZERO.ToString())
            {
                lstvwTests.Visible = true;
                btnSave.Visible = true;
                //trNoRecordFound.Visible = false;
                FillTests();
                HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwTests.FindControl("trHeader");
                CheckBox chkSelectAll = oHtmlTableRow.FindControl("ChkSelectAll") as CheckBox;
                chkSelectAll.Checked = false;
            }
            else
            {
                btnSave.Visible = false;
                //trNoRecordFound.Visible = true;
                lstvwTests.Visible = false;
            }
        }
        catch (Exception ex)
        {
	        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            StandardWiseExamConfigurationBL oStandardWiseExamConfigurationBL = new StandardWiseExamConfigurationBL(miSchoolId, miAcademicYearId);
            List<StandardWiseExamConfiguration> lstStandardWiseExamConfiguration = GenerateStandardWiseExamDetailsList();
            string sMessage = CheckDependencies(lstStandardWiseExamConfiguration.Where(Test => Test.Action == Constants.Action.Delete).ToList());
            if (sMessage == string.Empty)
            {
                oStandardWiseExamConfigurationBL.Save(GenerateStandardWiseExamXML(lstStandardWiseExamConfiguration), Convert.ToInt32(cmbStandard.SelectedValue), miUserId);
                lblSuccessfullMsg.Text = "Standard wise exam details saved successfully!!!";
                if (!IsConfigured())
                    SaveConfigDetails();
            }
            else
            {
                lblError.Text = sMessage;
            }
        }
        catch (Exception ex)
        {
	        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private List<StandardWiseExamConfiguration> GenerateStandardWiseExamDetailsList()
    {
        List<StandardWiseExamConfiguration> lstStandardWiseExamConfiguration = new List<StandardWiseExamConfiguration>();
        StandardWiseExamConfiguration oStandardWiseExamConfiguration;
        foreach (ListViewDataItem oCurrentItem in lstvwTests.Items)
        {
            if (((CheckBox)oCurrentItem.FindControl("chkSelect")).Checked)
            {
                oStandardWiseExamConfiguration = new StandardWiseExamConfiguration()
                {
                    SchoolwiseStandardTestId = Convert.ToInt32(lstvwTests.DataKeys[oCurrentItem.DisplayIndex]["SchoolwiseStandardTestId"]),
                    SchoolwiseTestId = Convert.ToInt32(lstvwTests.DataKeys[oCurrentItem.DisplayIndex]["SchoolwiseTestId"]),
                    Action = (Convert.ToInt32(lstvwTests.DataKeys[oCurrentItem.DisplayIndex]["SchoolwiseStandardTestId"]) != Constants.I_ZERO) ? Constants.Action.Update : Constants.Action.Insert,
                };
                TextBox otxtConsiderMarksOutOf = ((TextBox)oCurrentItem.FindControl("txtConsiderMarksOutOf"));
                if (otxtConsiderMarksOutOf.Text != string.Empty)
                    oStandardWiseExamConfiguration.OutOfMarks = Convert.ToInt32(otxtConsiderMarksOutOf.Text);
                lstStandardWiseExamConfiguration.Add(oStandardWiseExamConfiguration);
            }
            else if (Convert.ToInt32(lstvwTests.DataKeys[oCurrentItem.DisplayIndex]["SchoolwiseStandardTestId"]) != 0)
            {
                oStandardWiseExamConfiguration = new StandardWiseExamConfiguration()
                {
                    SchoolwiseStandardTestId = Convert.ToInt32(lstvwTests.DataKeys[oCurrentItem.DisplayIndex]["SchoolwiseStandardTestId"]),
                    SchoolwiseTestName = ((Label)oCurrentItem.FindControl("lblTestName")).Text,
                    Action = Constants.Action.Delete
                };
                lstStandardWiseExamConfiguration.Add(oStandardWiseExamConfiguration);
            }
        }
        return lstStandardWiseExamConfiguration;
    }

    private string GenerateStandardWiseExamXML(List<StandardWiseExamConfiguration> alstStandardWiseExamConfiguration)
    {
        StringWriter sw = new StringWriter();
        new XmlSerializer(alstStandardWiseExamConfiguration.GetType()).Serialize(sw, alstStandardWiseExamConfiguration);
        string sXML = sw.ToString();
        sXML = sXML.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
        return sXML;
    }

    private string CheckDependencies(List<StandardWiseExamConfiguration> alstStandardWiseExamConfiguration)
    {
        GenericReferenceList<StandardWiseExamConfiguration> objStdTestRefereces = new GenericReferenceList<StandardWiseExamConfiguration>(alstStandardWiseExamConfiguration, miAcademicYearId);
        return objStdTestRefereces.CheckDependenciesForList("SchoolwiseStandardTestId", "SchoolwiseTestName", "Action", Constants.ReferenceId.StandardExams, false);
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    /// <returns></returns>
    private bool IsConfigured()
    {
        return QueryString[Constants.S_IS_CONFIGURED] == Constants.S_YES;
    }

    /// <summary>
    /// This method is used to save config details.
    /// </summary>
    private void SaveConfigDetails()
    {
        ConfigurationSchoolMasterBL oConfiguration = new ConfigurationSchoolMasterBL()
        {
            OriginalConfigId = Convert.ToInt32(Constants.SchoolConfigurations.StandardWiseExamConfiguration),
            SchoolId = miSchoolId,
            AcademicYearId = miAcademicYearId,
            IsConfigure = Constants.C_YES,
            InsertedById = miUserId,
            UpdateById = miUserId,
        };
        oConfiguration.InsertConfigurationSchoolMaster();
    }
}