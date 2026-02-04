using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;

public partial class OnlineEnquiryFormUI : SchoolBase
{
    #region Property(s)

    private int SchoolId
    {
        get
        {
            return ConfigurationManager.AppSettings["SchoolId"].ToInt();
        }
    }

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is sued to fill year, standards etc.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetJavascriptAttributes();
            SetNextEnquiryNo();
            FillAcademicYear();
            FillArea();
            FillReferences();
        }
    }

    /// <summary>
    /// This event is used to get respected standard as per selected acadamic year.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            StandardCollectionBL oStdCollection = new StandardCollectionBL(SchoolId, Convert.ToInt32(cmbYear.SelectedValue));
            DataTable oDT = oStdCollection.GetAssociatedStandardsForEnquiry(Constants.I_ZERO);
            ControlUtility.FillDropDownList(oDT, ref cmbStd, "standard_id", "standard_name", Constants.S_SELECT);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit enquiry details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //bool bIsHuman = base.ValidateCaptcha();
            //Captcha1.ValidateCaptcha(txtCaptcha.Text);            
            if (Page.IsValid)
            {
                SaveStudentsDetails();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "showModal();", true);                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is sued to validate DOB.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void DOB_ServerValidate(object source, ServerValidateEventArgs args)
    {
        DateTime dtInputDate;
        if (DateTime.TryParse(args.Value, out dtInputDate))
        {
            if (dtInputDate < DateTime.Today)
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }
        else
        {
            args.IsValid = false;
        }
    }

    #endregion

    #region Method(s)
    
    /// <summary>
    /// This method isused to fill area.
    /// </summary>
    private void FillArea()
    {
        DataTable oDataTableArea = SchoolEnquiryBL.GetAllAreas(SchoolId);
        ControlUtility.FillDropDownList(oDataTableArea, ref cmbArea, "FeeAreaNameId", "Fee_AreaName", Constants.S_SELECT);
    }

    /// <summary>
    /// This method isused to academic year.
    /// </summary>
    private void FillAcademicYear()
    {
        DataSet oDataSet = SchoolEnquiryBL.GetAllMasterDataForStudentEnquiry(SchoolId, "Y");
        ControlUtility.FillDropDownList(oDataSet.Tables[0], ref cmbYear, "Academic_Year_ID", "AcademicYear", Constants.S_SELECT);

        if (cmbYear.Items.Count == 2)
        {
            cmbYear.SelectedIndex = 1;
            cmbYear_SelectedIndexChanged(cmbYear, null);
            cmbYear.Enabled = false;
        }
    }

    /// <summary>
    /// This method isused to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        //hidCaptData.Value = base.GetCaptcheHeaderData(phScripts);
        lblSchoolName.InnerText = ConfigurationManager.AppSettings["SchoolName"].ToString();
        txtCalDobPopup.Attributes["type"] = "date";
    }

    /// <summary>
    /// This method isused to set next enquiry number.
    /// </summary>
    private void SetNextEnquiryNo()
    {
        SchoolEnquiryBL oSchoolEnquiryBL = new SchoolEnquiryBL();
        txtEnqNo.Text = oSchoolEnquiryBL.GetNextEnquiryNo(SchoolId, 11);
    }

    /// <summary>
    /// This method is used to fill school enquiry references.
    /// </summary>
    private void FillReferences()
    {
        SchoolEnquiryBL oSchoolEnquiryBL = new SchoolEnquiryBL();
        List<EnquiryReference> lstEnquiryReference = oSchoolEnquiryBL.GetAllSchoolReference();
        ListSource.FillCheckBoxList(lstEnquiryReference, chklstReferences, "Name", "Id");
    }

    /// <summary>
    /// This method is used to save student details.
    /// </summary>
    private void SaveStudentsDetails()
    {
        int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
        string sEnquiryXML = GetEnquiryXML();
        SchoolEnquiryBL oSchoolEnquiryBL = new SchoolEnquiryBL();
        string enquiryNo = txtEnqNo.Text.Substring(0, 7);

        oSchoolEnquiryBL.EnquiryDetails = sEnquiryXML;
        string sSchoolReferences = GetSchoolReferences();
        int iDBEnquiryId;
        oSchoolEnquiryBL.InsertSchoolEnquiryDetails(iSchoolId, 0, sSchoolReferences, out iDBEnquiryId);
    }

    /// <summary>
    /// This method is used to return selected school references.
    /// </summary>
    /// <returns></returns>
    private string GetSchoolReferences()
    {
        StringBuilder obj = new StringBuilder();
        for (int iListIndex = 0; iListIndex < chklstReferences.Items.Count; iListIndex++)
        {
            if (chklstReferences.Items[iListIndex].Selected == true)
                obj.Append("," + chklstReferences.Items[iListIndex].Value);
        }

        if (obj.Length > 0)
            return obj.ToString().Substring(1);
        else
            return string.Empty;
    }

    /// <summary>
    /// This method is used to generate XML format for student enquiry details.
    /// </summary>
    /// <returns></returns>
    private string GetEnquiryXML()
    {

        const char C_FEMALE = 'F';
        const char C_MALE = 'M';
        const string S_ELEMENT = "element";
        const int I_MASTER = 5;
        const int I_MISS = 6;

        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("SchoolEnquiry");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "SchoolEnquiry", "");
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "SchoolEnquiryDetails", "");

        // Student Details
        string sAtrrName = "School_Id";
        XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = ConfigurationManager.AppSettings["SchoolID"];
        oXmlNode.Attributes.Append(attr);

        int iAdmissionFor = Constants.I_ZERO;
        sAtrrName = "Admission_For";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = iAdmissionFor.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Enquiry_No";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtEnqNo.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Student_First_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtStudFirstName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Student_Last_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtStudLastName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Student_Middle_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtStudMiddleName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Gender";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbGender.SelectedValue == "1" ? C_MALE.ToString() : C_FEMALE.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "DOB";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtCalDobPopup.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Current_school_name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSchoolName.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Acedemic_Year_Id";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbYear.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "For_Standard";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbStd.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_First_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMFirstName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_Last_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMLastName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_Middle_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMMiddleName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Father_First_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFFirstName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Father_Last_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFLastName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Father_Middle_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFMiddleName.Text.ToTitleCase();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Address";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtAddress.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_Mobile_Number1";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMotherMob1.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_Mobile_Number2";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMotherMob2.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Father_Mobile_Number1";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFatherMob1.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Father_Mobile_Number2";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFatherMob2.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Sibling_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = TxtSibling.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Friend_Colleague_Name";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFrnd.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Area";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbArea.SelectedIndex.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Email_Address";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtEmail.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_Email_Address";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = string.Empty;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Salutation_Id";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = cmbGender.SelectedValue == "1" ? I_MASTER.ToString() : I_MISS.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "FOccupationId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = string.Empty;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "MOccupationId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = string.Empty;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "MOccupationId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = string.Empty;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Nationality";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = string.Empty;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "PassportNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = string.Empty;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "PermanentAddress";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = string.Empty;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "ResidencePhoneNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = string.Empty;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "OfficePhoneNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = string.Empty;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "LastSchoolAddress";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = string.Empty;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "MoQualification";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = string.Empty;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "FoQualification";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = string.Empty;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Father_WhatsUp_Number";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = string.Empty;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Mother_WhatsUp_Number";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = string.Empty;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Pre_Standard";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "0";
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Landmark";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = string.Empty;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "AadharCardNumber";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtAadharCardNumber.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "LocationId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = "0";
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "CategoryId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = Constants.S_ZERO;
        oXmlNode.Attributes.Append(attr);

        // Add the node to root node.
        oXmlRootNode.AppendChild(oXmlNode);

        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    #endregion
}