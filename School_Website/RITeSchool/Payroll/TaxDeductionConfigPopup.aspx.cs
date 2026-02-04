// Class Name       :- ITCommissionerInfoPopup
// Purpose          :- This class is used to save Income tax commissioner details, quarter details and income tax deductor details.
// Date Of creation :- 18 Feb 2013
// Author Name      :- Pravin Shinde

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class TaxDeductionConfigPopup : SchoolBase
{
    #region Constants

    private const string S_SAVE_ITDETAILS = "CIT Configuration saved successfully !!!";
    private const string S_SAVE_QUARTERS = "Quarter Details saved successfully !!!";
    private const string S_SAVE_TAXDEDUCTOR = "Deductor Person Details saved successfully !!!";
    private const string S_SINGLE_SPACE = " "; 

    #endregion

    #region Data Member(s)

    private TaxDeductionBL moTaxDeductionBL;

    #endregion

    #region "Events"

    /// <summary>
    /// This event used to intialize controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {               
   			moTaxDeductionBL = new TaxDeductionBL(miSchoolId, miFinancialYearId, miUserId, miAcademicYearId); 
            if (!IsPostBack)
            {
                CheckIsPublished();      
                SetDefaultValues();
                LoadControlsData();                
            }            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
 
    /// <summary>
    /// This event is used lock or save CIT details for School.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {   
            ITCommissionerDetails oITCommissionerDetails = new ITCommissionerDetails
            {
                Address = txtAddress.Text.Trim().Replace(System.Environment.NewLine, S_SINGLE_SPACE),
                City = txtCity.Text.Trim(),
                Pincode = txtPincode.Text
            };
            moTaxDeductionBL.SaveCITDetails(oITCommissionerDetails);
            lblMessage.Text = S_SAVE_ITDETAILS;            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex,System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save the Quarter details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveQurters_Click(object sender, EventArgs e)
    {
        try
        {   
            List<Quarter> lstQuarters = new List<Quarter>();
            for (int iRowCount = 0; iRowCount < lstvwQuarters.Items.Count; iRowCount++)
            {
                int iId = lstvwQuarters.DataKeys[iRowCount]["Id"].ToInt();
                TextBox txtReceiptNumber = lstvwQuarters.Items[iRowCount].FindControl("txtReceiptNumber") as TextBox;

                Quarter oQuarter = new Quarter
                {
                    Id = iId,
                    ReceiptNumber = txtReceiptNumber.Text.Trim()                    
                };
                lstQuarters.Add(oQuarter);
            }

            string sQuarterXML = GenerateXml(lstQuarters);
            moTaxDeductionBL.SaveQuarters(sQuarterXML);
            lblMessage.Text = S_SAVE_QUARTERS;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save the Deductor details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveDeductor_Click(object sender, EventArgs e)
    {
        try
        {   
            TaxDeductorDetails oTaxDeductorDetails = new TaxDeductorDetails
            {
                Id = hidDeductorId.Value.ToInt(),
                SalutationId = cmbSalutation.SelectedValue.ToInt(),
                DesignationId = cmbDesignations.SelectedValue.ToInt(),
                Name = txtFirstName.Text.Trim(),
                FatherName = txtFatherName.Text.Trim()
            };

            moTaxDeductionBL.SaveTaxDeductorDetails(oTaxDeductorDetails);
            lblMessage.Text = S_SAVE_TAXDEDUCTOR;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region "Methods"

    /// <summary>
    /// This function is used to check whether IT details are published or not.
    /// </summary>
    private void CheckIsPublished()
    {
        IncomeTaxDetailsBL oIncomeTaxDetailsBL = new IncomeTaxDetailsBL(miSchoolId, miFinancialYearId, miUserId, miAcademicYearId);
        if (oIncomeTaxDetailsBL.CheckIsPublished())
            ShowHideControls(true);        
        else
            ShowHideControls(false);            
    }

    /// <summary>
    /// This is a common method to show and hide controls depends on publish status.
    /// </summary>
    /// <param name="abState"></param>
    private void ShowHideControls(bool abState)
    {
        hidIsPublished.Value = abState ? Constants.S_YES : Constants.S_NO;
        btnSaveQurters.Enabled = !abState;
        btnSaveDeductor.Enabled = !abState;
        btnSave.Enabled = !abState;
        btnClear.Enabled = !abState;
        btnClearDeductor.Enabled = !abState;
        trPublishMessage.Visible = abState;
    }

    /// <summary>
    /// This method is used to fill all the CIT details for school on pageload.
    /// </summary>
    private void LoadControlsData()
    {
        FillITCommissionerDetails();
        FillQuarterDetails();
        FilllIncomeTaxDeductorDetails();

        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillSalutationComboBox(ref cmbSalutation);
        oMasterDataCollectionBL.FillDesignationCombobox(ref cmbDesignations);
    }

    /// <summary>
    /// This method is used to fill up income tax deductor details.
    /// </summary>
    private void FilllIncomeTaxDeductorDetails()
    {
        TaxDeductorDetails oTaxDeductorDetails = moTaxDeductionBL.GetTaxDeductorDetails();
        if (!oTaxDeductorDetails.IsNull())
        {
            cmbSalutation.SelectedValue = oTaxDeductorDetails.SalutationId.ToString();
            cmbDesignations.SelectedValue = oTaxDeductorDetails.DesignationId.ToString();
            txtFirstName.Text = oTaxDeductorDetails.Name;
            txtFatherName.Text = oTaxDeductorDetails.FatherName;
        }
    }

    /// <summary>
    /// This method is used to fillup quarter details.
    /// </summary>
    private void FillQuarterDetails()
    {
        List<Quarter> lstQuarter = moTaxDeductionBL.GetAllQuarters();
        lstvwQuarters.DataSource = lstQuarter;
        lstvwQuarters.DataBind();
        if (lstQuarter.Count == 0)
            btnSaveQurters.Enabled = false;
    }

    /// <summary>
    /// THis method is sued to fill IT commissioner details.
    /// </summary>
    private void FillITCommissionerDetails()
    {
        ITCommissionerDetails oITCommissionerDetails = moTaxDeductionBL.GetCITDetails();
        if (!oITCommissionerDetails.IsNull())
        {
            txtAddress.Text = oITCommissionerDetails.Address;
            txtCity.Text = oITCommissionerDetails.City;
            txtPincode.Text = oITCommissionerDetails.Pincode;
        }
    }
    
    /// <summary>
    /// This method is used to set the default values.
    /// </summary>
    private void SetDefaultValues()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnSaveDeductor.Attributes.Add("onclick", "ClearLabel()");
        btnSaveQurters.Attributes.Add("onclick", "ClearLabel()");
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel,btnClear,btnClearDeductor,btnSaveDeductor,btnSaveQurters });
        txtAddress.Focus();
    }

    #endregion
}
