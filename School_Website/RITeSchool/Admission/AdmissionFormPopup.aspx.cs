/*
 * File Name - AdmissionFormPopup.aspx.cs
 * Created By -Sachin
 * Created Date - 20 Mar 2015
 * Description - This class is used to display admission form link and receipt link. If form is not submitted it will redirect for admissionform page.
 */
using System;
using System.Data;
using System.Reflection;
using System.Threading;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class AdmissionFormPopup : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            int iNextStandardId = QueryString["StandardId"].ToInt();
            int iAdmissionId = StudentAdmissionsBL.IsAdmissionDone(miSchoolId, iNextStandardId, miUserId);

            if (iAdmissionId != 0)
            {
                var oStudentAdmissionsBL = new StudentAdmissionsBL();
                DataTable oDataTable = oStudentAdmissionsBL.GetStudentAdmissionDetails(iAdmissionId, miSchoolId);
                if (oDataTable.Rows.Count > 0)
                {
                    string sMobileNumber = Convert.ToString(oDataTable.Rows[0]["MobileNumber"]);
                    string sFormNumber = Convert.ToString(oDataTable.Rows[0]["Form_Number"]);
                    string sQueryString = CommonUtility.EncryptQuerystring(String.Format("iAdmissionId={0}&Form_Number={1}&Mobile_Number={2}&EnableAdmissionFormFee=true", iAdmissionId, sFormNumber, sMobileNumber));

                    tblLinks.Visible = true;
                    hlnkReceipt.NavigateUrl = hlnkReceipt.NavigateUrl + "?" + sQueryString;
                    hlnkReceipt.Attributes.Add("onclick", "window.open('" + hlnkReceipt.NavigateUrl
                                                           + "' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600'); return false;");
                    hlnkAdmissionForm.NavigateUrl = hlnkAdmissionForm.NavigateUrl + "?" + sQueryString;
                    hlnkAdmissionForm.Attributes.Add("onclick", "window.open('" + hlnkAdmissionForm.NavigateUrl
                                                            + "' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600');return false;");
                }
            }
            else
            {
                Session["IsInternalAdmission"] = Constants.S_YES;
                Response.Redirect("AdmissionFormDocuments.aspx?" + Request.QueryString);
            }
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
}