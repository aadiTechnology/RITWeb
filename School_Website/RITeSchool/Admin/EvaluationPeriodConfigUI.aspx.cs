using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Globalization;
using System.Data.SqlClient;
using System.Text;

public partial class EvaluationPeriodConfigUI : SchoolBase
{
    #region "Constants"

    private const string S_SAVE_MESSAGE = "Exam period saved successfully !!!";
    private const string S_COPY_MESSAGE = "Exam period copied Sucessfully!!!";
    private const string S_DEFAULT_DATETIME = "1/1/1900 12:00:00 AM";

    #endregion "Constants"

    #region "Members"

    private SchoolwiseTermConfigurationMasterBL moSchoolwiseTermConfigurationMasterBL;
   
    #endregion "Members"

    #region "Events"
   /// <summary>
    /// This event is used to set javascript attributes for buttons, set default values to controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
     try
        {
            moSchoolwiseTermConfigurationMasterBL = new SchoolwiseTermConfigurationMasterBL(miSchoolId, miAcademicYearId);
            if (!IsPostBack)
            {             
                FillTestCombobox();
                FillTestConfigurationListView();
                SetJavaScriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            save();
            base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    ///// <summary>
    ///// This event is used to copy test configuration details.
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    protected void btnCopy_Click(object sender, EventArgs e)
    {
       try
        {
            CopyExamListView();
            base.DisplayMessage(S_COPY_MESSAGE, false, tdMessage);
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to select test combo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
         {
            FillTestConfigurationListView();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    ///  This event is used to set values to listview columns.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTestConfiguration_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                EvaluationPeriodDetails oEvaluationPeriodDetails = e.Item.DataItem as EvaluationPeriodDetails;
                TextBox txtStartDate = oCurrentItem.FindControl("txtStartDate") as TextBox;
                if (oEvaluationPeriodDetails.TestStartDate != S_DEFAULT_DATETIME.ToDateTime())
                    txtStartDate.Text = oEvaluationPeriodDetails.TestStartDate.ToString(Constants.S_DATE_FORMAT);
                else
                    txtStartDate.Text = "";

                TextBox txtEndDate = oCurrentItem.FindControl("txtEndDate") as TextBox;
                if (oEvaluationPeriodDetails.TestEndDate != S_DEFAULT_DATETIME.ToDateTime())
                    txtEndDate.Text = oEvaluationPeriodDetails.TestEndDate.ToString(Constants.S_DATE_FORMAT);
                else
                    txtEndDate.Text = "";
                   }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }
    #endregion "Events"

    #region "private Methods"

    /// <summary>
    /// This is used to save the Details.
    /// </summary>
    private void save()
    {   
        moSchoolwiseTermConfigurationMasterBL.InsertEvatualtionPeriodDetails(base.GenerateXml(Populate()), miUserId, cmbTests.SelectedValue.ToInt());
        FillTestConfigurationListView();
    }

    /// <summary>
    /// This method is used to fill list of schoolwise Evaluation details
    /// </summary>
    /// <returns></returns>
    private List<EvaluationPeriodDetails> Populate()
    {
        EvaluationPeriodDetails oEvaluationPeriodDetails = null;
        List<EvaluationPeriodDetails> lstvwEvaluation = new List<EvaluationPeriodDetails>();
        for (int iRowNo = 0; iRowNo < lstvwTestConfiguration.Items.Count; iRowNo++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwTestConfiguration.Items[iRowNo];
            oEvaluationPeriodDetails = new EvaluationPeriodDetails();
            TextBox txtStartDate = oCurrentItem.FindControl("txtStartDate") as TextBox;
            TextBox txtEndDate = oCurrentItem.FindControl("txtEndDate") as TextBox;
            int iStandardId = Convert.ToInt32(lstvwTestConfiguration.DataKeys[iRowNo]["StandardId"]);
            oEvaluationPeriodDetails.StandardId = iStandardId;
            oEvaluationPeriodDetails.TestStartDate = !string.IsNullOrEmpty(txtStartDate.Text) ? Convert.ToDateTime(txtStartDate.Text) : oEvaluationPeriodDetails.TestStartDate = Convert.ToDateTime(S_DEFAULT_DATETIME);
            oEvaluationPeriodDetails.TestEndDate = !string.IsNullOrEmpty(txtEndDate.Text) ? Convert.ToDateTime(txtEndDate.Text) : oEvaluationPeriodDetails.TestEndDate = Convert.ToDateTime(S_DEFAULT_DATETIME);
            lstvwEvaluation.Add(oEvaluationPeriodDetails);     
        }
        return lstvwEvaluation;     
    }

    /// <summary>
    /// This method is used to fill SchoolwiseTermConfiguration listview.
    /// </summary>
    private void FillTestConfigurationListView()
    {
        List<EvaluationPeriodDetails> lstEvaluationPeriodDetails = moSchoolwiseTermConfigurationMasterBL.GetAllEvaluationPeriods(cmbTests.SelectedValue.ToInt());
        lstvwTestConfiguration.DataSource = lstEvaluationPeriodDetails;
        lstvwTestConfiguration.DataBind();
        DataTable oDsAllTests = GetData();
        FillTestList(oDsAllTests);
    }

    /// <summary>
    /// This method is used to Get All Tests data.
    /// </summary>
    /// <returns></returns>
    private DataTable GetData()
    {
        TestCollectionBL oTestCollectionBL = new TestCollectionBL(miSchoolId, miAcademicYearId);
        return oTestCollectionBL.GetAllTestsForSchool();
    }

   /// <summary>
    /// This method fills the combobox for the tests.
    /// </summary>
    private void FillTestCombobox()
    {
        DataTable oDsAllTests = GetData();
        ControlUtility.FillDropDownList(
            oDsAllTests,
            ref cmbTests,
            Constants.S_TEST_ID_FIELD,
            Constants.S_TEST_NAME_FIELD,
            string.Empty);
        FillTestList(oDsAllTests);
    }

    /// <summary>
    /// This method fills the Checkboxlist for the tests.
    /// </summary>
    /// <param name="oDsAllTests"></param>
    private void FillTestList(DataTable oDsAllTests)
    {
        DataTable dv = oDsAllTests.Select("schoolwise_test_id<>" + cmbTests.SelectedValue).CopyToDataTable();
        ListSource.FillCheckBoxList(
            dv,
             ChkExamList,
             Constants.S_TEST_NAME_FIELD,
             Constants.S_TEST_ID_FIELD
             );
    }

    /// <summary>
    /// This method is used to copy Exam List View details.
    /// </summary>
    private void CopyExamListView()
    {                
        StringBuilder asTargetTestIds = new StringBuilder();
        for (int iCount = 0; iCount < ChkExamList.Items.Count; iCount++)
        {
            if (ChkExamList.Items[iCount].Selected)
                asTargetTestIds.Append(ChkExamList.Items[iCount].Value + ",");
        }
        string sTestIds = "";
        if (asTargetTestIds.ToString().EndsWith(","))
            sTestIds = asTargetTestIds.ToString().Substring(0, asTargetTestIds.Length - 1);
        moSchoolwiseTermConfigurationMasterBL.CopyEvaluationPeriods(miSchoolId, miAcademicYearId, cmbTests.SelectedValue.ToInt(), sTestIds, miUserId);
        for (int iCount = 0; iCount < ChkExamList.Items.Count; iCount++)
        ChkExamList.Items[iCount].Selected = false;
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnCopy, btnSave });
        btnSave.Attributes.Add("onclick", "if(!CheckValidations()){return false;}");
        btnCopy.Attributes.Add("onclick", "if(!ConfirmCopy()){return false;}");
    }

    /// <summary>
    /// This method is used to Clear Fields
    /// </summary>
    private void ClearFields()
    {
        hidTestId.Value = Constants.S_ZERO;
        cmbTests.ClearSelection();
        lblMessage.Text = string.Empty;
    }
   
    #endregion "Private Method"
}


