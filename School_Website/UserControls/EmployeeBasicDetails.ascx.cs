using System;
using System.IO;
using System.Reflection;
using SchoolEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using System.Resources;
using Utility;
using System.Globalization;
using System.Collections.Generic;
using System.Xml;
using System.Data;

public partial class EmployeeBasicDetailsUC : System.Web.UI.UserControl
{
    private int iStaffUserId;
    private int mischoolid;
    private int miacademicyearid;
    //private UsersStaffGroupsAssociationBL moUsersStaffGroupsAssociationBL;
    private EmployeeDetailsBL moEmployeeDetailsBL;
    
    public int StaffUserId
    {
        get { return iStaffUserId; }
        set { iStaffUserId = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillBankCombo();

            int iSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
            if (iSchoolId >= 150 && iSchoolId <= 161)
                colpnlAdditionalInfo.Collapsed = false;
            else
                colpnlAdditionalInfo.Collapsed = true;

            cstValEmail.Enabled = false;
            cstCmpnyEmail.Enabled = false;
        }
    }

    public void PopulateEmployeeBasicDetails()
    {
        moEmployeeDetailsBL = new EmployeeDetailsBL();
       // int miUserId = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
        int iAcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
        int iSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
        int UserId = iStaffUserId;
        	string sXMLOtherDetails = GetXMLOfOtherDetails( );
            string sXMLDetailsOfFamilyMember = GetXMLOfDetailsOfFamilyMember();
           // string sXMLPreviousEmployment = GetXMLOfPreviousEmployment();
            string sXMLStatutoryDetails = GetXMLOfStatutoryDetails();
           
           
            string sPrimaryEmail = txtPrimaryEmail.Text;
            moEmployeeDetailsBL.save(sXMLOtherDetails, sXMLDetailsOfFamilyMember,  sXMLStatutoryDetails, sPrimaryEmail, iAcademicYearId, iSchoolId, UserId);
    }

    public void InitializeFields()
    {
        int iSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
        moEmployeeDetailsBL = new EmployeeDetailsBL();
        EmployeeDetails oEmployeeDetails = moEmployeeDetailsBL.GetEmployeeBasicDetails( iStaffUserId ,iSchoolId);
        if (oEmployeeDetails.PermanentContactNo != null)
        {
            // txtGender.Text = Convert.ToBoolean(oEmployeeDetails.Gender).ToString();
            txtReference.Text = oEmployeeDetails.Reference;
            // txtMaritalStatus.Text = Convert.ToBoolean(oEmployeeDetails.Maritalstatus).ToString();
            txtSalaryScale.Text = Convert.ToDecimal(oEmployeeDetails.SalaryScale).ToString();
            //txtWhatsappNo.Text = Convert.ToInt32(oEmployeeDetails.WhatsAppNo).ToString();  //
            txtWhatsappNo.Text = oEmployeeDetails.WhatsAppNo.ToString();     //
            txtGPFACNo.Text = oEmployeeDetails.GPFAcNumber;
            txtPrimaryEmail.Text = oEmployeeDetails.PrimaryEmailId;
            txtCompanyEmail.Text = oEmployeeDetails.CompanyEmail;
            txtPermanentContNo.Text = oEmployeeDetails.PermanentContactNo;
            txtExtensionNo.Text = oEmployeeDetails.Extensionno;
            txtCompanyContNo.Text = oEmployeeDetails.CompanyContactNo;   //
            txtName.Text = oEmployeeDetails.FamilyMemberName;
            txtAge.Text = Convert.ToInt32(oEmployeeDetails.Age).ToString();
            txtRelation.Text = oEmployeeDetails.Relation;
            txtOccupation.Text = oEmployeeDetails.Occupaton;
            //txtDesignation.Text = oEmployeeDetails.DesignationName;
            //txtLastSalary.Text = Convert.ToInt32(oEmployeeDetails.LastSalary).ToString();
            //txtDuration.Text = oEmployeeDetails.Duration;
            //txtJobDescription.Text = oEmployeeDetails.JobDescription;
            //txtReasonForLeaving.Text = oEmployeeDetails.ReasonforLeaving;
            txtEPFNumber.Text = oEmployeeDetails.EPFNumber;
            // txtIsVPFDeduction.Text = Convert.ToBoolean(oEmployeeDetails.IsVPSDeduction).ToString();
            txtVPFContributionID.Text = Convert.ToInt32(oEmployeeDetails.VPSContributionId).ToString();
            txtVPFPercentage.Text = Convert.ToDecimal(oEmployeeDetails.VPFPercentage).ToString();
            txtVPFContrEffectiveForm.Text = Convert.ToDateTime(oEmployeeDetails.VPSContributionEffectiveForm).ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));
            txtVPFAmount.Text = Convert.ToDecimal(oEmployeeDetails.UPFAmount).ToString();
            // txtBankName.Text = oEmployeeDetails.BankName;
            cmbBank.SelectedValue = oEmployeeDetails.BankName;
            txtBranch.Text = oEmployeeDetails.Branch;
            txtAccNumber.Text = oEmployeeDetails.AccountNo;
            txtIncrementDate.Text = Convert.ToDateTime(oEmployeeDetails.IncrementDate).ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));

            txtUAN.Text = oEmployeeDetails.UAN;
            txtIncomeTaxStatusID.Text = Convert.ToInt32(oEmployeeDetails.IncomeTaxStatusId).ToString();
            txtPAyrollId.Text = Convert.ToInt32(oEmployeeDetails.PayrollId).ToString();
            txtBasicPay.Text = Convert.ToDecimal(oEmployeeDetails.BasicPay).ToString();
            txtPayrollGroupId.Text = Convert.ToInt32(oEmployeeDetails.PayrollGroupId).ToString();
            txtPayScale.Text = Convert.ToDecimal(oEmployeeDetails.PayScale).ToString();
            txtEPFJoinDate.Text = Convert.ToDateTime(oEmployeeDetails.EPFJoinDate).ToString(Constants.S_DATE_FORMAT, new CultureInfo("en"));


            if (oEmployeeDetails.Gender == true)
                rdoMale.Checked = true;
            else
                rdoFemale.Checked = true;

            if (oEmployeeDetails.Maritalstatus == true)
                rdomarried.Checked = true;
            else
                rdounmarried.Checked = true;

            if (oEmployeeDetails.IsVPSDeduction == true)
                rdoVPFDeduction1.Checked = true;
            else
                rdoVPFDeduction2.Checked = true;
        }
    }

    public void ClearFields()
    {
       // txtGender.Text = string.Empty;
        txtReference.Text = string.Empty;
      //  txtMaritalStatus.Text = string.Empty;
        txtSalaryScale.Text = string.Empty;
        txtWhatsappNo.Text = string.Empty;
        txtGPFACNo.Text = string.Empty;
        txtPrimaryEmail.Text = string.Empty;
       
        txtCompanyEmail.Text = string.Empty;
        txtPermanentContNo.Text = string.Empty;
        txtExtensionNo.Text = string.Empty;
        txtCompanyContNo.Text = string.Empty;
        txtName.Text = string.Empty;
        txtAge.Text = string.Empty;
        txtRelation.Text = string.Empty;
        txtOccupation.Text = string.Empty;
        //txtDesignation.Text = string.Empty;
        //txtLastSalary.Text = string.Empty;
        //txtDuration.Text = string.Empty;
        //txtJobDescription.Text = string.Empty;
        //txtReasonForLeaving.Text = string.Empty;
        txtEPFNumber.Text = string.Empty;
        //txtIsVPFDeduction.Text = string.Empty; //
        txtVPFContributionID.Text = string.Empty;
        txtVPFPercentage.Text = string.Empty;
        txtVPFContrEffectiveForm.Text = string.Empty;
        txtVPFAmount.Text = string.Empty;
    //    txtBankName.Text = string.Empty;
        cmbBank.ClearSelection();
        txtBranch.Text = string.Empty;
        txtAccNumber.Text = string.Empty;
        txtIncrementDate.Text = string.Empty;
       
        txtUAN.Text = string.Empty;
        txtIncomeTaxStatusID.Text = string.Empty;
        txtPAyrollId.Text = string.Empty;
        txtBasicPay.Text = string.Empty;
        txtPayrollGroupId.Text = string.Empty;
        txtPayScale.Text = string.Empty;
        txtEPFJoinDate.Text = string.Empty;
    }

    private string GetXMLOfOtherDetails()
    {

        //const int C_FEMALE = 0;
        //const int C_MALE = 1;
        //const int C_Married =  1;
        //const int C_Unmarried = 0;
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement root = oDoc.CreateElement("EmployeeBasicInformations");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "EmployeeBasicInformation", "");
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "EmployeeBasicInformation", "");

        
       // string sAtrrName = "Gender";
       // XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
       //// attr.Value = txtGender.Text.ToTitleCase();
       // attr.Value = rdoMale.Checked ? C_MALE.ToString() : C_FEMALE.ToString();
       //// oXmlNode.Attributes.Append(attr);

        string sAtrrName = "Gender";
        XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
        var svalue = 0;
        if (rdoMale.Checked)
            svalue = 1;
        if (rdoFemale.Checked)
            svalue = 0;
        attr.Value = svalue.ToString();
        oXmlNode.Attributes.Append(attr);


        sAtrrName = "Reference";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtReference.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Maritalstatus";
        attr = oDoc.CreateAttribute(sAtrrName);
        var sval = 0;
        if (rdomarried.Checked)
            sval = 1;
        if (rdounmarried.Checked)
            sval = 0;
        attr.Value = sval.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "SalaryScale";
        var salaryscale = 0.00;
        attr = oDoc.CreateAttribute(sAtrrName);
        if (txtSalaryScale.Text == "")
            attr.Value = salaryscale.ToString().Trim();
        else
            attr.Value = txtSalaryScale.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "WhatsAppNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtWhatsappNo.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "GPFAcNumber";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtGPFACNo.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);


        sAtrrName = "UAN";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtUAN.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "BankAcNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtAccNumber.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);
       // oXmlNode.Attributes.Append(attr);

        // Add the node to root node.
        oXmlRootNode.AppendChild(oXmlNode);

        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    
    private string GetXMLOfDetailsOfFamilyMember()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement root = oDoc.CreateElement("EmployeeFamilyDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "EmployeeFamilyDetail", "");
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "EmployeeFamilyDetail", "");

        string sAtrrName = "FamilyMemberName";
        XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtName.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Age";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtAge.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Relation";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtRelation.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Occupation";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtOccupation.Text;
        oXmlNode.Attributes.Append(attr);

        oXmlRootNode.AppendChild(oXmlNode);
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);
        // return the string generated.
        return root.InnerXml;
    }
    //private string GetXMLOfPreviousEmployment()
    //{
    //    const string S_ELEMENT = "element";
    //    XmlDocument oDoc = new XmlDocument();
    //    // Create a root level element.
    //    XmlElement root = oDoc.CreateElement("TeacherExperienceDetails");
    //    XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "TeacherExperienceDetail", "");
    //    XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "TeacherExperienceDetail", "");

    //    string sAtrrName = "PreviousDesignation";
    //    XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
    //    attr.Value = txtDesignation.Text;
    //    oXmlNode.Attributes.Append(attr);




    //    sAtrrName = "Last_Salary";
    //    var slastsalary = 0.00;
    //    attr = oDoc.CreateAttribute(sAtrrName);
    //    if (txtLastSalary.Text == "")
    //        attr.Value = slastsalary.ToString().Trim();
    //    else
    //    attr.Value = txtLastSalary.Text;
    //    oXmlNode.Attributes.Append(attr);

    //    sAtrrName = "DurationDays";
    //    attr = oDoc.CreateAttribute(sAtrrName);
    //    attr.Value = txtDuration.Text;
    //    oXmlNode.Attributes.Append(attr);

    //    sAtrrName = "Job_Description";
    //    attr = oDoc.CreateAttribute(sAtrrName);
    //    attr.Value = txtJobDescription.Text;
    //    oXmlNode.Attributes.Append(attr);

    //    sAtrrName = "Reason_For_Leaving";
    //    attr = oDoc.CreateAttribute(sAtrrName);
    //    attr.Value = txtReasonForLeaving.Text;
    //    oXmlNode.Attributes.Append(attr);

    //    oXmlRootNode.AppendChild(oXmlNode);
    //    // Add the root node to document element. 
    //    root.AppendChild(oXmlRootNode);
    //    // return the string generated.
    //    return root.InnerXml;
    //}
    private string GetXMLOfStatutoryDetails()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement root = oDoc.CreateElement("EmployeeJobDetailsInformations");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "EmployeeJobDetailsInformation", "");
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "EmployeeJobDetailsInformation", "");

        string sAtrrName = "EPFNumber";
        XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtEPFNumber.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "IsVPSDeduction";
        attr = oDoc.CreateAttribute(sAtrrName);
        var svalDeduction = 0;
        if (rdoVPFDeduction1.Checked)
            svalDeduction = 1;
        if (rdoVPFDeduction2.Checked)
            svalDeduction = 0;
        attr.Value = svalDeduction.ToString();
        oXmlNode.Attributes.Append(attr);


        sAtrrName = "VPSContributionId";   //
        var vpsContrId = 0;
        attr = oDoc.CreateAttribute(sAtrrName);
        if (txtVPFContributionID.Text == "")
            attr.Value = vpsContrId.ToString();
        else
        attr.Value = txtVPFContributionID.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "VPFPercentage";
        var vpfpercentage = 0;
        attr = oDoc.CreateAttribute(sAtrrName);
        if (txtVPFPercentage.Text == "")
            attr.Value = vpfpercentage.ToString();
        else
        attr.Value = txtVPFPercentage.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "VPSContributionEffectiveForm";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtVPFContrEffectiveForm.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "UPFAmount";
        var upfamt = 0;
        attr = oDoc.CreateAttribute(sAtrrName);
        if (txtVPFAmount.Text == "")
            attr.Value = upfamt.ToString();
        else 
        attr.Value = txtVPFAmount.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "BankName";
        attr = oDoc.CreateAttribute(sAtrrName);
      //  attr.Value = txtBankName.Text;
        attr.Value = cmbBank.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Branch";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtBranch.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "IncrementDate";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtIncrementDate.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "IncomeTaxStatusId";
        var incometaxstatusid = 0;
        attr = oDoc.CreateAttribute(sAtrrName);
        if (txtIncomeTaxStatusID.Text == "")
            attr.Value = incometaxstatusid.ToString();
        else
        attr.Value = txtIncomeTaxStatusID.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "PayrollId";
        var payrollid = 0;
        attr = oDoc.CreateAttribute(sAtrrName);
        if (txtPAyrollId.Text == "")
            attr.Value = payrollid.ToString();
        else
        attr.Value = txtPAyrollId.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "BasicPay";
        var basicpay = 0.00;
        attr = oDoc.CreateAttribute(sAtrrName);
        if (txtBasicPay.Text == "")
            attr.Value = basicpay.ToString().Trim();
        else
        attr.Value = txtBasicPay.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "PayrollGroupId";
        var payrollgrpid = 0;
        attr = oDoc.CreateAttribute(sAtrrName);
        if(txtPayrollGroupId.Text=="")
            attr.Value= payrollgrpid.ToString();
        else
        attr.Value = txtPayrollGroupId.Text;
        oXmlNode.Attributes.Append(attr);



        sAtrrName = "PayScale";
        var spayscale = 0.00;
        attr = oDoc.CreateAttribute(sAtrrName);
        if (txtPayScale.Text == "")
            attr.Value = spayscale.ToString();
        else
            attr.Value = txtPayScale.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "EPFJoinDate";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtEPFJoinDate.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "CompanyEmail";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtCompanyEmail.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "PermanatContactNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtPermanentContNo.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "ExtensionNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtExtensionNo.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "CompanyContactNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtCompanyContNo.Text;
        oXmlNode.Attributes.Append(attr);

        oXmlRootNode.AppendChild(oXmlNode);
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);
        // return the string generated.
        return root.InnerXml;
    }


    /// <summary>
    /// This method is used to fill bank combobox.
    /// </summary>
    private void FillBankCombo()
    {
        int miSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
        EmployeeDetailsBL oSchoolwiseBankMasterBL = new EmployeeDetailsBL();
        DataTable dtBankList = oSchoolwiseBankMasterBL.getAllBank(miSchoolId);
        ControlUtility.FillDropDownList(dtBankList, ref cmbBank, "Schoolwise_Bank_Id", "Bank_Name", Constants.S_SELECT);
    }

}