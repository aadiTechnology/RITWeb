/* File Name - StudentSearchUI.aspx.cs
 * Created By - Sachin
 * Created Date - 9 Jun 2015
 * Description - This class is used to search student.
 */
using System;
using System.Reflection;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.UI.WebControls;
using FeeEntities;
using System.Xml;
using System.Web;
using System.IO;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Web.UI;

using FeeReportNS = SchoolEntities.StudentFee.FeeReport;

public partial class StudentSearchUI : ExportToExcel
{
    #region Constant(s)

    private const string S_ELEMENT = "element";
    private const char S_HIGHER_SECONDARY = 'H';
    const string DASH = "-";

    private int miStudentPaidFeeStartupRow = 1;
    private List<Student> mlstStudents;
    private StudentFeeDetailsBL moStudentFeeDetailsBL;
    
    private FeeReportNS.FeeReport moFeeReport;

    #endregion

    #region Event(s)

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetJavascriptAttributes();

            if (!Settings.IsAaryanSchool)
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
                txtStartDate.Text = startDate.ToString("dd-MMM-yyyy");
                txtEndDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                trDatefilter1.Visible = false;
                trDatefilter2.Visible = false;

                optExport.Checked = true;
                optExport_CheckedChanged(optExport, null);

                optExport.Visible = false;
                optSearch.Visible = false;
            }
        }
    }

    /// <summary>
    /// This event is used to search student details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearchStudent_Click(object sender, EventArgs e)
    {
        try
        {
            StudentBL oStudentBL = new StudentBL();
            Student oStudent = oStudentBL.GetStudentDetails(miSchoolId, miAcademicYearId, txtName.Text.Trim());

            if (oStudent.StudentId != 0)
            {

                string sQueryString = "StudentId=" + oStudent.StudentId.ToString()
                                + "&StudentName=" + oStudent.Name
                                + "&ClassName=" + oStudent.ClassName
                                + "&RegNo=" + oStudent.RegistraionNo
                                + "&StandardId=" + oStudent.StandardId.ToString()
                                + "&DivisionId=" + oStudent.DivisionId.ToString()
                                + "&standardName=" + string.Empty
                                + "&DivisionName=" + string.Empty
                                + "&NewMode=" + "N"
                                + "&pIndex=1"
                                + "&pSortExp=" + string.Empty
                                + "&pSortDirc=" + string.Empty
                                + "&Is_Configured=" + "Y"
                                + "&DivSelectedValue=" + oStudent.DivisionId.ToString()
                                + "&StdSelectedValue=" + oStudent.StandardId.ToString()
                                + "&NameOrRegNo=" + oStudent.RegistraionNo
                                + "&abIsExactMatch=" + true
                                + "&IsSchoolLeft=" + string.Empty
                                + "&ClassId=" + oStudent.StdDivId.ToString()
                                + "&asOperator=" + "1"
                                + "&asPrefix=" + string.Empty
                                + "&asPostfix=" + string.Empty
                                + "&SearchedNumber=" + oStudent.RegistraionNo
                                + "&Is_SuperAdmin=" + "N"
                                + "&IsDirectSearch=" + "Y";
                hidQueryString.Value = CommonUtility.EncryptQuerystring(sQueryString);
                hidSearchMode.Value = "StudentSearch";
            }
            else
            {
                hidQueryString.Value = string.Empty;
                hidSearchMode.Value = string.Empty;
                lblMessage.Text = "Student not found.";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This even is used to print receipt.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
            int iYearwiseStudentId = oStudentFeeDetailsBL.GetStudentDetails(miSchoolId, miAcademicYearId, txtReceiptNumber.Text.Trim());
            if (iYearwiseStudentId != 0)
            {
                hidQueryString.Value = CommonUtility.EncryptQuerystring(string.Format("&PostBackUrl=~/PayFeePopUp.aspx&StudentId={0}&ReceiptNo={1}",
                                        iYearwiseStudentId,
                                        txtReceiptNumber.Text));
                hidSearchMode.Value = "ReceiptSearch";
            }
            else
            {
                hidQueryString.Value = string.Empty;
                hidSearchMode.Value = string.Empty;
                lblReceiptMessage.Text = "Student not found.";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display search fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optSearch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            SetSearchVisibility(true);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display export fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void optExport_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            SetSearchVisibility(false);
            StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
            DataTable dt = oStandardCollectionBL.GetAllStandards();
            
            chkStandards.DataSource = dt;
            chkStandards.DataTextField = "Standard_Name";
            chkStandards.DataValueField = "Standard_Id";
            chkStandards.DataBind();
            
            DataTable dtFeeType = oStandardCollectionBL.GetAllFeeTypes();
            ListSource.FillDropDownList(dtFeeType, cmbFeeTypes, "Display_Member", "Value_Member", Constants.S_ALL);
            
            ListSource.FillDropDownList(dt, cmbStandard, "Standard_Name", "Standard_Id", Constants.S_ALL);
            ListSource.FillDropDownList(dt, cmbExStandard, "Standard_Name", "Standard_Id", Constants.S_ALL);
            FillDivisions(cmbStandard.SelectedValue.ToInt(), cmbDivision);
            FillDivisions(cmbExStandard.SelectedValue.ToInt(), cmbExDivision);
            cmbexStudent.Items.Add(new ListItem { Value = Constants.S_ZERO, Text = Constants.S_ALL });
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill division combo box.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillDivisions(cmbStandard.SelectedValue.ToInt(), cmbDivision);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to export fee details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (miSchoolId == Constants.SchoolId.PPS.ToInt())
            {
                moStudentFeeDetailsBL = new StudentFeeDetailsBL(miSchoolId, miAcademicYearId, 0, miUserId);
                mlstStudents = moStudentFeeDetailsBL.GetStudentAllFeesDetails(cmbStandard.SelectedValue.ToInt(), cmbDivision.SelectedValue.ToInt(), txtStartDate.Text.Trim(), txtEndDate.Text.Trim());  //date filter

                string sFileName = "StudentPaidFeeDetails_" + Guid.NewGuid() + ".xlsx";
                string filePath = base.BasePath + @"\RITeSchool\UPLOADS\ResultSheet\" + sFileName;

                using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    CreateWorkBookPartForStudentPaidFeeReport(workbookPart);
                }

                Response.Write(string.Format("<Script language='Javascript'>window.open('../UPLOADS/ResultSheet/" + sFileName + "')</Script>"));
            }
            else if (Settings.IsAaryanSchool)
            {
                FeeReportBL moFeeReportBL = new FeeReportBL(miSchoolId, miAcademicYearId);
                moFeeReport = moFeeReportBL.GetFeeDetailsForReport(cmbStandard.SelectedValue.ToInt(), cmbDivision.SelectedValue.ToInt());

                string sFileName = "StudentFeeDetails_" + Guid.NewGuid() + ".xlsx";
                string filePath = base.BasePath + @"\RITeSchool\UPLOADS\ResultSheet\" + sFileName;

                using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    CreateWorkBookForAaryan(workbookPart);
                }

                Response.Write(string.Format("<Script language='Javascript'>window.open('../UPLOADS/ResultSheet/" + sFileName + "')</Script>"));
            }
            else
            {
                moStudentFeeDetailsBL = new StudentFeeDetailsBL(miSchoolId, miAcademicYearId, 0, miUserId);
                mlstStudents = moStudentFeeDetailsBL.GetStudentAllFeesDetails(cmbStandard.SelectedValue.ToInt(), cmbDivision.SelectedValue.ToInt(), txtStartDate.Text.Trim(), txtEndDate.Text.Trim());  //date filter

                DataTable dt = new DataTable();
                List<string> lstColumns = AddColumns(dt, moStudentFeeDetailsBL);

                List<int> lstOriginalStandardIds = mlstStudents.Select(st => st.OriginalStandardId).Distinct().ToList();

                lstOriginalStandardIds.OrderBy(id => id).ToList().ForEach
                    (
                        stdId =>
                        {
                            List<int> lstOriginalDivisionIds = mlstStudents.Where(std => std.OriginalStandardId == stdId).Select(st => st.OriginalDivisionId).Distinct().ToList();
                            lstOriginalDivisionIds.OrderBy(id => id).ToList().ForEach(
                                divId =>
                                {
                                    mlstStudents.Where(std => std.OriginalStandardId == stdId && std.OriginalDivisionId == divId).OrderBy(std => std.RollNo).ToList().ForEach
                                    (
                                        stud =>
                                        {
                                            DataRow dr = dt.NewRow();
                                            dr["Class Name"] = stud.ClassName;
                                            dr["Roll No."] = stud.RollNo;
                                            dr["Student Name"] = stud.Name;

                                            moStudentFeeDetailsBL.FeeDetails.Where(std => std.StudentId == stud.YearWiseStudentId).ToList().ForEach(
                                            fee =>
                                            {
                                                dr[fee.PayableFor] = moStudentFeeDetailsBL.FeeDetails.Where(std => std.StudentId == stud.YearWiseStudentId && std.PayableFor == fee.PayableFor).Sum(std => std.Amount);
                                            }
                                            );

                                            dr["Total"] = "<B>" + moStudentFeeDetailsBL.FeeDetails.Where(std => std.StudentId == stud.YearWiseStudentId).Sum(std => std.Amount) + "</B>";

                                            dt.Rows.Add(dr);
                                        }
                                    );

                                    DataRow drBlankRow = dt.NewRow();
                                    dt.Rows.Add(drBlankRow);
                                }
                                );
                        }
                    );

                AddSummaryRow(dt, moStudentFeeDetailsBL, lstColumns);

                ExportToExcel("FeeDetails.xls", dt);
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
        
    /// <summary>
    /// This event is used to fill up division list for fee XML export.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbExStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillDivisions(cmbExStandard.SelectedValue.ToInt(), cmbExDivision);
            cmbexStudent.Items.Clear();
            cmbexStudent.Items.Add(new ListItem { Value = Constants.S_ZERO, Text = Constants.S_ALL });
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill up student list for fee XML export.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbExDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = StudentBL.GetAllStudents(miSchoolId, cmbExStandard.SelectedValue.ToInt(), cmbExDivision.SelectedValue.ToInt(), miAcademicYearId);
            ControlUtility.FillDropDownList(dt, ref cmbexStudent, "Yearwise_Student_Id", "Name", Constants.S_ALL);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to  export fee details in XML format.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExVouchers_Click(object sender, EventArgs e)
    {
        try
        {
            StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL(miSchoolId, miAcademicYearId, 0, miUserId);
            List<FeeDetailsToExport> lstFeeDetails = oStudentFeeDetailsBL.GetFeeDetailsToExport(cmbExStandard.SelectedValue.ToInt(), cmbExDivision.SelectedValue.ToInt(), cmbexStudent.SelectedValue.ToInt());

            string S_ELEMENT = "element";
            XmlDocument oDoc = new XmlDocument();
            XmlElement root = oDoc.CreateElement("ENVELOPE");

            XmlNode oXmlFee = null;

            var studentIds = lstFeeDetails.Select(std => new { std.StudentId, std.SerialNo, std.RowNo }).Distinct().OrderBy(std => std.RowNo).ToList();

            XmlNode oHeader = oDoc.CreateNode(S_ELEMENT, "HEADER", string.Empty);

            XmlNode oTallyRequest = oDoc.CreateNode(S_ELEMENT, "TALLYREQUEST", string.Empty);
            oTallyRequest.InnerText = "Import Data";
            oHeader.AppendChild(oTallyRequest);

            root.AppendChild(oHeader);

            XmlNode oBody = oDoc.CreateNode(S_ELEMENT, "BODY", string.Empty);

            XmlNode oImportData = oDoc.CreateNode(S_ELEMENT, "IMPORTDATA", string.Empty);

            XmlNode oReqDesc = oDoc.CreateNode(S_ELEMENT, "REQUESTDESC", string.Empty);

            XmlNode oReportName = oDoc.CreateNode(S_ELEMENT, "REPORTNAME", string.Empty);
            oReportName.InnerText = "Vouchers";
            oReqDesc.AppendChild(oReportName);

            var oStaticVar = oDoc.CreateNode(S_ELEMENT, "STATICVARIABLES", string.Empty);
            oReqDesc.AppendChild(oStaticVar);

            var oCurrentCompany = oDoc.CreateNode(S_ELEMENT, "SVCURRENTCOMPANY", string.Empty);
            oCurrentCompany.InnerText = Settings.SchoolNameForFeeXML;
            oStaticVar.AppendChild(oCurrentCompany);

            oImportData.AppendChild(oReqDesc);

            var oReqData = oDoc.CreateNode(S_ELEMENT, "REQUESTDATA", string.Empty);

            foreach (var stud in studentIds)
            {
                var oTallyMessage = oDoc.CreateNode(S_ELEMENT, "TALLYMESSAGE", string.Empty);

                AddAttribute("xmlns:UDF", "TallyUDF", oTallyMessage, oDoc);

                var oVoucher = oDoc.CreateNode(S_ELEMENT, "VOUCHER", string.Empty);

                AddAttribute("REMOTEID", "7995bf93-5052-4587-9558-abd51caff250-" + stud.SerialNo, oVoucher, oDoc);
                AddAttribute("VCHKEY", "7995bf93-5052-4587-9558-abd51caff250-0000a301:00000be8", oVoucher, oDoc);
                AddAttribute("VCHTYPE", "Journal", oVoucher, oDoc);
                AddAttribute("ACTION", "Create", oVoucher, oDoc);
                AddAttribute("OBJVIEW", "Accounting Voucher View", oVoucher, oDoc);

                lstFeeDetails.Where(std => std.StudentId == stud.StudentId && std.ParentId == 0).ToList().ForEach(
                    fee =>
                    {
                        oXmlFee = oDoc.CreateNode(S_ELEMENT, fee.Field, string.Empty);
                        oXmlFee.InnerText = fee.Value;

                        if (fee.Field == "OLDAUDITENTRYIDS.LIST")
                            AddAttribute("TYPE", "Number", oXmlFee, oDoc);

                        if (lstFeeDetails.Any(fd => fd.ParentId == fee.Id))
                            AddNodes(oXmlFee, fee.Id, lstFeeDetails, oDoc, stud.StudentId);

                        oVoucher.AppendChild(oXmlFee);
                    }
                );

                oTallyMessage.AppendChild(oVoucher);

                oReqData.AppendChild(oTallyMessage);
            }

            oImportData.AppendChild(oReqData);

            oBody.AppendChild(oImportData);

            root.AppendChild(oBody);

            oDoc.AppendChild(root);

            string sVoucherXMLFilePath = HttpContext.Current.Server.MapPath("..") + "\\DOWNLOADS\\FeeVouchers.xml";

            if (File.Exists(sVoucherXMLFilePath))
                File.Delete(sVoucherXMLFilePath);
            oDoc.Save(sVoucherXMLFilePath);
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=FeeVoucher.xml");
            HttpContext.Current.Response.TransmitFile(sVoucherXMLFilePath);
            HttpContext.Current.Response.End();
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnExportLedgers_Click(object sender, EventArgs e)
    {
        try
        {
            StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL(miSchoolId, miAcademicYearId, 0, miUserId);
            List<FeeLedger> lstLedgers = oStudentFeeDetailsBL.GetAllFeeLedgers(cmbexStudent.SelectedValue.ToInt(), cmbExStandard.SelectedValue.ToInt(), cmbExDivision.SelectedValue.ToInt());

            XmlDocument oDoc = new XmlDocument();
            XmlElement root = oDoc.CreateElement("ENVELOPE");

            XmlNode oXmlFee = null;

            XmlNode oHeader = oDoc.CreateNode(S_ELEMENT, "HEADER", string.Empty);

            XmlNode oTallyRequest = oDoc.CreateNode(S_ELEMENT, "TALLYREQUEST", string.Empty);
            oTallyRequest.InnerText = "Import Data";
            oHeader.AppendChild(oTallyRequest);

            root.AppendChild(oHeader);

            XmlNode oBody = oDoc.CreateNode(S_ELEMENT, "BODY", string.Empty);

            XmlNode oImportData = oDoc.CreateNode(S_ELEMENT, "IMPORTDATA", string.Empty);

            XmlNode oReqDesc = oDoc.CreateNode(S_ELEMENT, "REQUESTDESC", string.Empty);

            XmlNode oReportName = oDoc.CreateNode(S_ELEMENT, "REPORTNAME", string.Empty);
            oReportName.InnerText = "All Masters";
            oReqDesc.AppendChild(oReportName);

            var oStaticVar = oDoc.CreateNode(S_ELEMENT, "STATICVARIABLES", string.Empty);
            oReqDesc.AppendChild(oStaticVar);

            var oCurrentCompany = oDoc.CreateNode(S_ELEMENT, "SVCURRENTCOMPANY", string.Empty);
            oCurrentCompany.InnerText = Settings.SchoolNameForFeeXML;
            oStaticVar.AppendChild(oCurrentCompany);

            oImportData.AppendChild(oReqDesc);

            var oReqData = oDoc.CreateNode(S_ELEMENT, "REQUESTDATA", string.Empty);

            if (lstLedgers.Count > 0)
            {
                lstLedgers.ForEach(
                    ledger =>
                    {
                        var oTallyMessage = oDoc.CreateNode(S_ELEMENT, "TALLYMESSAGE", string.Empty);

                        AddAttribute("xmlns:UDF", "TallyUDF", oTallyMessage, oDoc);

                        XmlNode oLedger = oDoc.CreateNode(S_ELEMENT, "LEDGER", string.Empty);
                        AddAttribute("NAME", ledger.Name, oLedger, oDoc);
                        AddAttribute("RESERVEDNAME", string.Empty, oLedger, oDoc);

                        AddNode("CURRENCYNAME", "Rs.", oXmlFee, oLedger, oDoc);
                        AddNode("PARENT", "Direct Incomes", oXmlFee, oLedger, oDoc);
                        AddNode("TAXCLASSIFICATIONNAME", string.Empty, oXmlFee, oLedger, oDoc);
                        AddNode("TAXTYPE", "Others", oXmlFee, oLedger, oDoc);
                        AddNode("GSTTYPE", string.Empty, oXmlFee, oLedger, oDoc);
                        AddNode("SERVICECATEGORY", string.Empty, oXmlFee, oLedger, oDoc);
                        AddNode("EXCISEDUTYTYPE", string.Empty, oXmlFee, oLedger, oDoc);
                        AddNode("TRADERLEDNATUREOFPURCHASE", string.Empty, oXmlFee, oLedger, oDoc);
                        AddNode("TDSDEDUCTEETYPE", string.Empty, oXmlFee, oLedger, oDoc);
                        AddNode("TDSRATENAME", string.Empty, oXmlFee, oLedger, oDoc);
                        AddNode("LEDGERFBTCATEGORY", string.Empty, oXmlFee, oLedger, oDoc);

                        AddNode("ISBILLWISEON", "No", oXmlFee, oLedger, oDoc);
                        AddNode("ISCOSTCENTRESON", "Yes", oXmlFee, oLedger, oDoc);
                        AddNode("ISINTERESTON", "No", oXmlFee, oLedger, oDoc);
                        AddNode("ALLOWINMOBILE", "No", oXmlFee, oLedger, oDoc);

                        AddNode("ISCONDENSED", "No", oXmlFee, oLedger, oDoc);
                        AddNode("AFFECTSSTOCK", "No", oXmlFee, oLedger, oDoc);

                        AddNode("FORPAYROLL", "No", oXmlFee, oLedger, oDoc);
                        AddNode("INTERESTONBILLWISE", "No", oXmlFee, oLedger, oDoc);
                        AddNode("OVERRIDEINTEREST", "No", oXmlFee, oLedger, oDoc);

                        AddNode("OVERRIDEADVINTEREST", "No", oXmlFee, oLedger, oDoc);
                        AddNode("USEFORVAT", "No", oXmlFee, oLedger, oDoc);
                        AddNode("IGNORETDSEXEMPT", "No", oXmlFee, oLedger, oDoc);
                        AddNode("ISTCSAPPLICABLE", "No", oXmlFee, oLedger, oDoc);
                        AddNode("ISTDSAPPLICABLE", "No", oXmlFee, oLedger, oDoc);
                        AddNode("ISFBTAPPLICABLE", "No", oXmlFee, oLedger, oDoc);
                        AddNode("ISGSTAPPLICABLE", "No", oXmlFee, oLedger, oDoc);
                        AddNode("SHOWINPAYSLIP", "No", oXmlFee, oLedger, oDoc);
                        AddNode("USEFORGRATUITY", "No", oXmlFee, oLedger, oDoc);
                        AddNode("FORSERVICETAX", "No", oXmlFee, oLedger, oDoc);

                        AddNode("ISINPUTCREDIT", "No", oXmlFee, oLedger, oDoc);
                        AddNode("ISEXEMPTED", "No", oXmlFee, oLedger, oDoc);
                        AddNode("ISABATEMENTAPPLICABLE", "No", oXmlFee, oLedger, oDoc);
                        AddNode("TDSDEDUCTEEISSPECIALRATE", "No", oXmlFee, oLedger, oDoc);
                        AddNode("AUDITED", "No", oXmlFee, oLedger, oDoc);

                        AddNode("SORTPOSITION", ledger.SortOrder.ToString(), oXmlFee, oLedger, oDoc);

                        XmlNode oLanguageList = oDoc.CreateNode(S_ELEMENT, "LANGUAGENAME.LIST", string.Empty);
                        XmlNode oNameList = oDoc.CreateNode(S_ELEMENT, "NAME.LIST", string.Empty);
                        AddNode("NAME", ledger.Name, oXmlFee, oNameList, oDoc);
                        oLanguageList.AppendChild(oNameList);

                        AddNode("LANGUAGEID", 1033.ToString(), oXmlFee, oLanguageList, oDoc);
                        oLedger.AppendChild(oLanguageList);

                        oTallyMessage.AppendChild(oLedger);

                        oReqData.AppendChild(oTallyMessage);
                    }
                    );
            }

            oImportData.AppendChild(oReqData);

            oBody.AppendChild(oImportData);

            root.AppendChild(oBody);

            oDoc.AppendChild(root);

            string sVoucherXMLFilePath = HttpContext.Current.Server.MapPath("..") + "\\DOWNLOADS\\Ledgers.xml";

            if (File.Exists(sVoucherXMLFilePath))
                File.Delete(sVoucherXMLFilePath);
            oDoc.Save(sVoucherXMLFilePath);
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=Ledgers.xml");
            HttpContext.Current.Response.TransmitFile(sVoucherXMLFilePath);
            HttpContext.Current.Response.End();
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to  export fee details in excess sheet format.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnFeeExport_Click(object sender, EventArgs e)
    {
        try
        {
            StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL(miSchoolId, miAcademicYearId, 0, miUserId);

            StringBuilder standards = new StringBuilder(string.Empty);
            foreach (ListItem list in chkStandards.Items)
            {
                if (list.Selected)
                {
                    standards.Append(list.Value).Append(", ");
                }
            }
            string sStandardIds = standards.ToString().TrimEnd(' ').TrimEnd(',');
            List<FeeStandards> lstFeeStandards = oStudentFeeDetailsBL.GetAllFeeDetailsForExport(sStandardIds, cmbFeeTypes.SelectedValue.ToInt());
            ExportFee(lstFeeStandards);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    protected void ExportToExcel(string strFileName, DataTable oDatatable)
    {
        DataGrid dg = new DataGrid();
        dg.DataSource = oDatatable;
        dg.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=" + strFileName);
        Response.ContentType = "application/excel";
        System.IO.StringWriter sw = new System.IO.StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        dg.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
        dg = null;
        dg.Dispose();
    }

    /// <summary>
    /// This method is used to Export StandardwiseFee.
    /// </summary>
    /// <param name="alstFeeStandards"></param>
    private void ExportFee(List<FeeStandards> alstFeeStandards)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Report-" + miAcademicYearId + ".xls");
        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:15px; font-family:Calibri; background:white;'>");
        string sHeader = string.Empty;
        AddBlankLIne();

        if (alstFeeStandards.Any(st => st.IsPrePrimary == Constants.C_YES))
        {
            SetStandardSection(alstFeeStandards, Constants.C_YES);
            AddBlankLIne();
        }

        if (alstFeeStandards.Any(st => st.IsPrePrimary == Constants.C_NO))
        {
            SetStandardSection(alstFeeStandards, Constants.C_NO);
            AddBlankLIne();
        }
        if (alstFeeStandards.Any(st => st.IsPrePrimary == S_HIGHER_SECONDARY))
        {
            SetStandardSection(alstFeeStandards, S_HIGHER_SECONDARY);
        }


        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    /// <summary>
    /// This method is used to Add blank line in Fee Export.
    /// </summary>
    private static void AddBlankLIne()
    {
        HttpContext.Current.Response.Write("<TR>");
        HttpContext.Current.Response.Write("</TR>");
    }

    /// <summary>
    /// This method is used to Add Headers in Fee Export.
    /// </summary>
    private static void AddHeaders(List<FeeStandards> alstFeeStandards, char acIsPerPrimary)
    {
        HttpContext.Current.Response.Write("<TR>");
        AddCell(string.Empty, 1);
        AddCell(string.Empty, 1);
        alstFeeStandards.Where(st => st.IsPrePrimary == acIsPerPrimary).Select(ft => new { ft.OriginalFeeTypeId, ft.FeeType, ft.HeaderName }).Distinct().OrderBy(ft => ft.OriginalFeeTypeId).ToList().ForEach
                                        (
                                            feeType =>
                                            {
                                                int iPayableCount = alstFeeStandards.Where(pf => pf.OriginalFeeTypeId == feeType.OriginalFeeTypeId && pf.IsPrePrimary == acIsPerPrimary).Select(pf => pf.PayableFor).Distinct().Count();

                                                AddHeaderCell(feeType.HeaderName, iPayableCount * 6, "text-align:center");
                                            }
     );
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR>");
        AddCell(string.Empty, 1);
        AddCell(string.Empty, 1);
        alstFeeStandards.Where(st => st.IsPrePrimary == acIsPerPrimary).Select(ft => new { ft.OriginalFeeTypeId, ft.FeeType }).Distinct().OrderBy(ft => ft.OriginalFeeTypeId).ToList().ForEach
                                        (
                                            feeType =>
                                            {
                                                int iPayableCount = alstFeeStandards.Where(pf => pf.OriginalFeeTypeId == feeType.OriginalFeeTypeId && pf.IsPrePrimary == acIsPerPrimary).Select(pf => pf.PayableFor).Distinct().Count();

                                                AddHeaderCell(feeType.FeeType, iPayableCount * 6, "text-align:center");
                                            }
     );
        HttpContext.Current.Response.Write("</TR>");
        HttpContext.Current.Response.Write("<TR>");
        AddHeaderCell("Sr.No.", 1);
        AddHeaderCell("Class Name", 1);
        alstFeeStandards.Where(st => st.IsPrePrimary == acIsPerPrimary).Select(ft => new { ft.OriginalFeeTypeId, ft.FeeType }).Distinct().OrderBy(ft => ft.OriginalFeeTypeId).ToList().ForEach
                                        (
                                            feeType =>
                                            {

                                                alstFeeStandards.Where(pf => pf.OriginalFeeTypeId == feeType.OriginalFeeTypeId && pf.IsPrePrimary == acIsPerPrimary).Select(pf => pf.PayableFor).Distinct().OrderBy(pf => pf).ToList().ForEach
                                                    (
                                                        payableFor =>
                                                        {
                                                            AddHeaderCell(payableFor, 6, "text-align:center");
                                                        }
                                                );
                                            }
     );
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR>");
        AddCell(string.Empty, 1);
        AddCell(string.Empty, 1);
        alstFeeStandards.Where(st => st.IsPrePrimary == acIsPerPrimary).Select(ft => new { ft.OriginalFeeTypeId, ft.FeeType }).Distinct().OrderBy(ft => ft.OriginalFeeTypeId).ToList().ForEach
                                        (
                                            feeType =>
                                            {

                                                alstFeeStandards.Where(pf => pf.OriginalFeeTypeId == feeType.OriginalFeeTypeId && pf.IsPrePrimary == acIsPerPrimary).Select(pf => pf.PayableFor).Distinct().OrderBy(pf => pf).ToList().ForEach
                                                    (
                                                        payableFor =>
                                                        {
                                                            AddHeaderCell("To Be Collected", 2, "text-align:center");
                                                            AddHeaderCell("Collected", 2, "text-align:center");
                                                            AddHeaderCell("Pending", 2, "text-align:center");
                                                        }
                                                );
                                            }
     );
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR>");
        AddCell(string.Empty, 1);
        AddCell(string.Empty, 1);
        alstFeeStandards.Where(st => st.IsPrePrimary == acIsPerPrimary).Select(ft => new { ft.OriginalFeeTypeId, ft.FeeType }).Distinct().OrderBy(ft => ft.OriginalFeeTypeId).ToList().ForEach
                                        (
                                            feeType =>
                                            {

                                                alstFeeStandards.Where(pf => pf.OriginalFeeTypeId == feeType.OriginalFeeTypeId && pf.IsPrePrimary == acIsPerPrimary).Select(pf => pf.PayableFor).Distinct().OrderBy(pf => pf).ToList().ForEach
                                                    (
                                                        payableFor =>
                                                        {
                                                            AddHeaderCell("Str", 1, "text-align:center");
                                                            AddHeaderCell("Amount", 1, "text-align:center");
                                                            AddHeaderCell("Str", 1, "text-align:center");
                                                            AddHeaderCell("Amount", 1, "text-align:center");
                                                            AddHeaderCell("Str", 1, "text-align:center");
                                                            AddHeaderCell("Amount", 1, "text-align:center");
                                                        }
                                                );
                                            }
     );
        HttpContext.Current.Response.Write("</TR>");
    }

    /// <summary>
    /// This method is used to Add footer in Fee Export.
    /// </summary>
    /// <param name="alstFeeStandards"></param>
    /// <param name="acIsPerPrimary"></param>
    private static void AddFooter(List<FeeStandards> alstFeeStandards, char acIsPerPrimary)
    {
        HttpContext.Current.Response.Write("<TR>");
        AddHeaderCell("Total", 2, "text-align:center; font-size:16px; font-family:Calibri;");
        alstFeeStandards.Where(ft => ft.IsPrePrimary == acIsPerPrimary).Select(ft => new { ft.OriginalFeeTypeId, ft.FeeType }).Distinct().OrderBy(ft => ft.OriginalFeeTypeId).ToList().ForEach
                                        (
                                            feeType =>
                                            {

                                                alstFeeStandards.Where(pf => pf.OriginalFeeTypeId == feeType.OriginalFeeTypeId && pf.IsPrePrimary == acIsPerPrimary).Select(pf => pf.PayableFor).Distinct().OrderBy(pf => pf).ToList().ForEach
                                                    (
                                                        payableFor =>
                                                        {
                                                            var sVAl = alstFeeStandards.Where(pf => pf.OriginalFeeTypeId == feeType.OriginalFeeTypeId && pf.PayableFor == payableFor && pf.IsPrePrimary == acIsPerPrimary);
                                                            if (sVAl.Count() > Constants.I_ZERO)
                                                            {
                                                                var oAmountDetails = alstFeeStandards.Where(pf => pf.OriginalFeeTypeId == feeType.OriginalFeeTypeId && pf.PayableFor == payableFor && pf.IsPrePrimary == acIsPerPrimary);

                                                                int iToBeCollCount = 0;
                                                                int iToBeCollAmt = 0;
                                                                int iCollCount = 0;
                                                                int iCollAmt = 0;
                                                                int iPendingCount = 0;
                                                                int iPendingAmount = 0;

                                                                if (oAmountDetails.Any(ad => ad.Type == 1))
                                                                {
                                                                    iToBeCollCount = oAmountDetails.Where(ad => ad.Type == 1).Sum(st => st.Count.ToInt());
                                                                    iToBeCollAmt = oAmountDetails.Where(ad => ad.Type == 1).Sum(st => st.PayableAmount).ToInt();                                                                    
                                                                }
                                                                AddHeaderCell(iToBeCollCount.ToString(), 0, "text-align:center; font-size:16px; font-family:Calibri;");
                                                                AddHeaderCell(iToBeCollAmt.ToString(), 0, "text-align:center; font-size:16px; font-family:Calibri;");

                                                                if (oAmountDetails.Any(ds => ds.Type == 2))
                                                                {
                                                                    iCollCount = oAmountDetails.Where(ad => ad.Type == 2).Sum(st => st.Count.ToInt());
                                                                    iCollAmt = oAmountDetails.Where(ad => ad.Type == 2).Sum(st => st.PayableAmount).ToInt();                                                                    
                                                                }
                                                                AddHeaderCell(iCollCount.ToString(), 0, "text-align:center; font-size:16px; font-family:Calibri;");
                                                                AddHeaderCell(iCollAmt.ToString(), 0, "text-align:center; font-size:16px; font-family:Calibri;");

                                                                if (oAmountDetails.Any(ds => ds.Type == 3))
                                                                {
                                                                    iPendingCount = oAmountDetails.Where(ad => ad.Type == 3).Sum(st => st.Count.ToInt());
                                                                    iPendingAmount = oAmountDetails.Where(ad => ad.Type == 3).Sum(st => st.PayableAmount.ToInt());
                                                                }
                                                                
                                                                AddHeaderCell(Convert.ToString(iPendingCount), 0, "text-align:center; font-size:14px; font-family:Calibri;");
                                                                AddHeaderCell(Convert.ToString(iPendingAmount), 0, "text-align:center; font-size:16px; font-family:Calibri;");
                                                            }
                                                        }
                                                );
                                            }
     );
        HttpContext.Current.Response.Write("</TR>");
    }

    /// <summary>
    /// This method is used to set stamdard Details for Fee Export.
    /// </summary>
    /// <param name="alstFeeStandards"></param>
    /// <param name="acIsPerPrimary"></param>
    private static void SetStandardSection(List<FeeStandards> alstFeeStandards, char acIsPerPrimary)
    {
        AddHeaders(alstFeeStandards, acIsPerPrimary);
        alstFeeStandards.Where(pf => pf.IsPrePrimary == acIsPerPrimary).Select(pf => new { pf.StandardId, pf.OriginalStandardId, pf.StandardName }).OrderBy(pf => pf.OriginalStandardId).Distinct().ToList().ForEach
                               (
                                   std =>
                                   {
                                       HttpContext.Current.Response.Write("<TR>");

                                       AddHeaderCell(std.StandardName);
                                       AddHeaderCell("Total");
                                       

                                       var oFeeCount = alstFeeStandards.Where(ft => ft.StandardId == std.StandardId).Select(ft => new { ft.OriginalFeeTypeId, ft.FeeType }).Distinct();

                                       alstFeeStandards.Where(ft => ft.StandardId == std.StandardId).Select(ft => new { ft.OriginalFeeTypeId, ft.FeeType, ft.IsPrePrimary }).Distinct().OrderBy(ft => ft.OriginalFeeTypeId).ToList().ForEach
                                           (
                                               feeType =>
                                               {
                                                   var payCount = alstFeeStandards.Where(pf => pf.OriginalFeeTypeId == feeType.OriginalFeeTypeId && pf.StandardId == std.StandardId).Select(pf => pf.PayableFor).Distinct();

                                                   alstFeeStandards.Where(pf => pf.OriginalFeeTypeId == feeType.OriginalFeeTypeId && pf.IsPrePrimary == feeType.IsPrePrimary).Select(pf => pf.PayableFor).Distinct().OrderBy(pf => pf).ToList().ForEach
                                                       (
                                                           payableFor =>
                                                           {
                                                               var oAmountDetails = alstFeeStandards.Where(pf => pf.OriginalFeeTypeId == feeType.OriginalFeeTypeId && pf.PayableFor == payableFor && pf.StandardId == std.StandardId);
                                                               if (oAmountDetails.Count() > 0)
                                                               {
                                                                   int iToBeCollCount = 0;
                                                                   int iToBeCollAmt = 0;
                                                                   int iCollCount = 0;
                                                                   int iCollAmt = 0;
                                                                   int iPendingCount = 0;
                                                                   int iPendingAmount = 0;

                                                                   if (oAmountDetails.Any(ad => ad.Type == 1))
                                                                   {
                                                                       iToBeCollCount = oAmountDetails.Where(ad => ad.Type == 1).FirstOrDefault().Count.ToInt();
                                                                       iToBeCollAmt = oAmountDetails.Where(ad => ad.Type == 1).FirstOrDefault().PayableAmount.ToInt();
                                                                   }
                                                                   AddCell(iToBeCollCount.ToString());
                                                                   AddCell(iToBeCollAmt.ToString());

                                                                   if (oAmountDetails.Any(ds => ds.Type == 2))
                                                                   {
                                                                       iCollCount = oAmountDetails.Where(ad => ad.Type == 2).FirstOrDefault().Count.ToInt();
                                                                       iCollAmt = oAmountDetails.Where(ad => ad.Type == 2).FirstOrDefault().PayableAmount.ToInt();
                                                                   }
                                                                   AddCell(iCollCount.ToString());
                                                                   AddCell(iCollAmt.ToString());

                                                                   if (oAmountDetails.Any(ds => ds.Type == 3))
                                                                   {
                                                                       iPendingCount = oAmountDetails.Where(ad => ad.Type == 3).FirstOrDefault().Count.ToInt();
                                                                       iPendingAmount = oAmountDetails.Where(ad => ad.Type == 3).FirstOrDefault().PayableAmount.ToInt();
                                                                   }
                                                                   AddCell(Convert.ToString(iPendingCount));
                                                                   AddCell(Convert.ToString(iPendingAmount));
                                                               }
                                                               else
                                                               {
                                                                   AddCell(Constants.S_ZERO);
                                                                   AddCell(Constants.S_ZERO);
                                                                   AddCell(Constants.S_ZERO);
                                                                   AddCell(Constants.S_ZERO);
                                                                   AddCell(Constants.S_ZERO);
                                                                   AddCell(Constants.S_ZERO);
                                                               }
                                                           }
                                                       );
                                               });

                                       HttpContext.Current.Response.Write("</TR>");
                                   }
           );
        AddFooter(alstFeeStandards, acIsPerPrimary);
    }

    /// <summary>
    /// This method is used to add row.
    /// </summary>
    /// <param name="asValue"></param>
    /// <param name="aiColSpan"></param>
    /// <param name="asStyle"></param>
    private static void AddCell(string asValue, int aiColSpan = 1, string asStyle="")
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle = "style='" + asStyle + "'";

        HttpContext.Current.Response.Write("<TD colspan='" + aiColSpan + "' " + sStyle + ">");
        HttpContext.Current.Response.Write(asValue);
        HttpContext.Current.Response.Write("</TD>");
    }

    /// <summary>
    /// This method is used to add header.
    /// </summary>
    /// <param name="asValue"></param>
    /// <param name="aiColSpan"></param>
    /// <param name="asStyle"></param>
    private static void AddHeaderCell(string asValue, int aiColSpan = 1, string asStyle = "")
    {
        string sStyle = string.Empty;
        if (asStyle != string.Empty)
            sStyle = "style='" + asStyle + "'";

        HttpContext.Current.Response.Write("<TD colspan='" + aiColSpan + "' " + sStyle + ">");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(asValue);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</TD>");
    }
    
    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        valSumStudent.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        valSumReceipt.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnSearchStudent.Attributes.Add("onclick", "ClearMessages();");
        btnPrint.Attributes.Add("onclick", "ClearMessages();");
        optSearch.Checked = true;
        SetSearchVisibility(true);

        if (miSchoolId == Constants.SchoolId.DSK.ToInt())
        {
            trVoucher.Visible = true;
            trBreak.Visible = true;
        }
        else
        {
            trVoucher.Visible = false;
            trBreak.Visible = false;
        }
        if (miSchoolId == Constants.SchoolId.SNS.ToInt() && optExport.Checked == true)
        {
            trFeeSummary.Visible = true;
        }
        else
        {
            trFeeSummary.Visible = false;
        }

    }

    /// <summary>
    /// This method is used to set field visibility.
    /// </summary>
    /// <param name="abVal"></param>
    private void SetSearchVisibility(bool abVal)
    {
        tdSearch.Visible = abVal;
        tdExport.Visible = !abVal;

        if (miSchoolId == Constants.SchoolId.SNS.ToInt() && optExport.Checked == true)
            trFeeSummary.Visible = true;
        else
            trFeeSummary.Visible = false;
    }

    /// <summary>
    /// This method is used to fill divisions.
    /// </summary>
    private void FillDivisions(int aiStandardId, DropDownList oDropDownList)
    {
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable dt = oDivisionCollectionBL.GetAllDivisionsForStandard(aiStandardId);
        ListSource.FillDropDownList(dt, oDropDownList, "Division_Name", "Division_Id", Constants.S_ALL);
    }

    /// <summary>
    /// This method is used to add summary row.
    /// </summary>
    /// <param name="adtFeeDetails"></param>
    /// <param name="aoStudentFeeDetailsBL"></param>
    private void AddSummaryRow(DataTable adtFeeDetails, StudentFeeDetailsBL aoStudentFeeDetailsBL, List<string> alstColumns)
    {
        DataRow drTotal = adtFeeDetails.NewRow();
        drTotal["Class Name"] = string.Empty;
        drTotal["Roll No."] = string.Empty;
        drTotal["Student Name"] = "Total";
        alstColumns.ForEach(
               interval =>
               {
                   drTotal[interval] = "<B>" + aoStudentFeeDetailsBL.FeeDetails.Where(stud => stud.PayableFor == interval).Sum(stud => stud.Amount) + "</B>";
               }
           );

        drTotal["Total"] = "<B>" + aoStudentFeeDetailsBL.FeeDetails.Sum(stud => stud.Amount) + "</B>";
        adtFeeDetails.Rows.Add(drTotal);
    }

    /// <summary>
    /// This method is used to add columns.
    /// </summary>
    /// <param name="adtFeeDetails"></param>
    /// <param name="aoStudentFeeDetailsBL"></param>
    private List<string> AddColumns(DataTable adtFeeDetails, StudentFeeDetailsBL aoStudentFeeDetailsBL)
    {
        List<string> lstColumns = new List<string>();
        adtFeeDetails.AddColumns(new string[] { "Class Name", "Roll No.", "Student Name" });
        aoStudentFeeDetailsBL.Intervals.Select(INT => INT.PayableFor).Distinct().ToList().ForEach(
                interval =>
                {
                    adtFeeDetails.Columns.Add(interval);
                    lstColumns.Add(interval);
                }
            );

        aoStudentFeeDetailsBL.FeeDetails.Select(fd => fd.PayableFor).Except(aoStudentFeeDetailsBL.Intervals.Select(intv => intv.PayableFor)).Distinct().ToList().
            ForEach(
                interval =>
                {
                    adtFeeDetails.Columns.Add(interval);
                    lstColumns.Add(interval);
                }
            );

        adtFeeDetails.Columns.Add("Total");
        return lstColumns;
    }

    /// <summary>
    /// This method is used to add attribute.
    /// </summary>
    /// <param name="asName"></param>
    /// <param name="asValue"></param>
    /// <param name="aoXmlNode"></param>
    /// <param name="aoDoc"></param>
    private void AddAttribute(string asName, string asValue, XmlNode aoXmlNode, XmlDocument aoDoc)
    {
        XmlAttribute oXmlAttribute = aoDoc.CreateAttribute(asName);
        oXmlAttribute.Value = asValue;
        aoXmlNode.Attributes.Append(oXmlAttribute);
    }

    /// <summary>
    /// This method is used to add node.
    /// </summary>
    /// <param name="aoXmlFee"></param>
    /// <param name="aiParentId"></param>
    /// <param name="alstFeeDetails"></param>
    /// <param name="aoDoc"></param>
    /// <param name="aiStudentId"></param>
    private void AddNodes(XmlNode aoXmlFee, int aiParentId, List<FeeDetailsToExport> alstFeeDetails, XmlDocument aoDoc, int aiStudentId)
    {
        alstFeeDetails.Where(fee => fee.ParentId == aiParentId && fee.StudentId == aiStudentId).ToList()
            .ForEach(
            fee =>
            {
                var oXmlFee = aoDoc.CreateNode(S_ELEMENT, fee.Field, string.Empty);
                oXmlFee.InnerText = fee.Value;

                if (fee.Field == "OLDAUDITENTRYIDS.LIST")
                    AddAttribute("TYPE", "Number", oXmlFee, aoDoc);

                if (alstFeeDetails.Any(fd => fd.ParentId == fee.Id))
                    AddNodes(oXmlFee, fee.Id, alstFeeDetails, aoDoc, fee.StudentId);

                aoXmlFee.AppendChild(oXmlFee);
            }
            );
    }

    /// <summary>
    /// This method is used to add nodes.
    /// </summary>
    /// <param name="asName"></param>
    /// <param name="asInnerText"></param>
    /// <param name="aoXmlFee"></param>
    /// <param name="aoLedger"></param>
    /// <param name="aoDoc"></param>
    private void AddNode(string asName, string asInnerText, XmlNode aoXmlFee, XmlNode aoLedger, XmlDocument aoDoc)
    {
        aoXmlFee = aoDoc.CreateNode(S_ELEMENT, asName, string.Empty);
        aoXmlFee.InnerText = asInnerText;
        aoLedger.AppendChild(aoXmlFee);
    }

    #endregion

    #region Export to Excel

    #region Aaryan Fee report

    /// <summary>
    /// This method is used to create work book part for student paid fee details report.
    /// </summary>
    /// <param name="aoPart"></param>
    private void CreateWorkBookForAaryan(WorkbookPart aoPart)
    {
        WorkbookStylesPart workbookStylesPart1 = aoPart.AddNewPart<WorkbookStylesPart>("rId3");
        base.GenerateReportStyles(workbookStylesPart1);

        WorksheetPart worksheetPart1 = aoPart.AddNewPart<WorksheetPart>("rId1");
        GenerateStudentFeeDetailsForAaryan(worksheetPart1);

        GeneratePartContent(aoPart, "Fee Details");
    }
    
    /// <summary>
    /// This method is used to geenerate fee details.
    /// </summary>
    /// <param name="aoWorksheetPart1"></param>
    private void GenerateStudentFeeDetailsForAaryan(WorksheetPart aoWorksheetPart1)
    {
        int iColCount = moFeeReport.FeeTypes.Count;

        Worksheet worksheet1 = new Worksheet();
        worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        base.AddSheetDetails(worksheet1);
        SheetData sheetData1 = new SheetData();

        SetStudentFeeDetailsColumnWidthForAaryan(worksheet1, iColCount);
        AddPaidFeeHeaderForAaryan(sheetData1, iColCount);

        AddStudentPaidFeeDataRowsForAaryan(sheetData1, iColCount);

        worksheet1.Append(sheetData1);

        base.AddPrintOptions(worksheet1);
        base.SetPageMargin(worksheet1, 0.2);
        base.SetPageSetup(worksheet1, OrientationValues.Landscape);
        aoWorksheetPart1.Worksheet = worksheet1;
    }

    /// <summary>
    /// This method is used to fill fee details.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="iColCount"></param>
    private void AddStudentPaidFeeDataRowsForAaryan(SheetData aoSheetData1, int iColCount)
    {
        miStudentPaidFeeStartupRow++;

        SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
        DataTable oDT = oSchoolWiseAcademicYearMasterBL.GetAllAcademicYearsForSchool(miSchoolId);

        string sAcademicYear = string.Empty;
        DataRow[] drArr = oDT.Select("Academic_Year_Id=" + miAcademicYearId);
        if (drArr.Length > 0)
            sAcademicYear = drArr[0]["YearValue"].ToString();

        moFeeReport.StudentInfo.OrderBy(stud => stud.OrgStdId).ThenBy(stud => stud.OrdDivId).ThenBy(stud => stud.RollNo).ToList().ForEach
            (
                stud =>
                {
                    var lstReceiptNos = moFeeReport.PaidFeeDetails.Where(fd => fd.YearwiseStudentId == stud.YearwiseStudentId).Select(fd => fd.ReceiptNumber.ToInt()).Distinct().OrderBy(fd => fd).ToList();

                    lstReceiptNos.ForEach(rcpt =>
                    {
                        var lstStudPaidFeeList = moFeeReport.PaidFeeDetails.Where(fd => fd.YearwiseStudentId == stud.YearwiseStudentId && fd.ReceiptNumber == rcpt.ToString()).Select(fd => fd.PayableFor).Distinct().ToList();
                        foreach (var payable in lstStudPaidFeeList)
                        {
                            AddStudentRow(aoSheetData1, stud, payable, false, rcpt.ToString(), sAcademicYear);
                        }
                    });

                    List<string> lstStudFeeList = moFeeReport.SchooolwiseStudentFeeDetailss.Where(fd => fd.YearwiseStudentId == stud.YearwiseStudentId).Select(fd => fd.PayableFor).Distinct().ToList();
                    foreach (var payable in lstStudFeeList)
                    {
                        AddStudentRow(aoSheetData1, stud, payable, true, "", sAcademicYear);
                    }
                }
        );

    }

    /// <summary>
    /// This method is used to add student record.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="stud"></param>
    /// <param name="payable"></param>
    /// <param name="abIsDebitEntry"></param>
    /// <param name="receiptNo"></param>
    private void AddStudentRow(SheetData aoSheetData1, FeeReportNS.StudentInfo stud, string payable, bool abIsDebitEntry, string receiptNo, string asAcademicYear)
    {
        if (abIsDebitEntry)
        {
            Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 14 };

            AddBasicData(stud, payable, row, asAcademicYear, abIsDebitEntry, receiptNo);
            AddFeeTypes(stud, payable, abIsDebitEntry, row, string.Empty);

            row.Append(AddCell("0", CellValues.String, StudentPaidFeeEnum.CenterData));
            row.Append(AddCell("0", CellValues.String, StudentPaidFeeEnum.CenterData));

            int iPayableAmout = moFeeReport.SchooolwiseStudentFeeDetailss.Where(ssfd => ssfd.YearwiseStudentId == stud.YearwiseStudentId && ssfd.PayableFor == payable).Sum(ssfd => ssfd.Amount);
            row.Append(AddCell(iPayableAmout.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));
            row.Append(AddCell(DASH, CellValues.String, StudentPaidFeeEnum.CenterData));
            row.Append(AddCell(DASH, CellValues.String, StudentPaidFeeEnum.CenterData));
            row.Append(AddCell(DASH, CellValues.String, StudentPaidFeeEnum.CenterData));
            row.Append(AddCell(DASH, CellValues.String, StudentPaidFeeEnum.CenterData));
            row.Append(AddCell(DASH, CellValues.String, StudentPaidFeeEnum.CenterData));
            row.Append(AddCell(DASH, CellValues.String, StudentPaidFeeEnum.CenterData));
            row.Append(AddCell(DASH, CellValues.String, StudentPaidFeeEnum.CenterData));
            row.Append(AddCell(DASH, CellValues.String, StudentPaidFeeEnum.CenterData));

            AddStatusAndFeeCategory(stud, row);

            AddTransportSection(stud, row);

            aoSheetData1.Append(row);
            miStudentPaidFeeStartupRow++;
        }
        else
        {
            List<FeeReportNS.PaidFeeDetails> lstPaidFeeDetails = moFeeReport.PaidFeeDetails.Where(ssfd => ssfd.YearwiseStudentId == stud.YearwiseStudentId && ssfd.PayableFor == payable).ToList();

            List<int> oPaidDetails = lstPaidFeeDetails.Select(ssfd => ssfd.ReceiptNumber.ToInt()).Distinct().OrderBy(ssfd => ssfd).ToList();

            List<FeeReportNS.PaidFeeDetails> lstPaid = lstPaidFeeDetails.Where(ssfd => ssfd.ReceiptNumber == receiptNo).ToList();

            Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 14 };

            AddBasicData(stud, payable, row, asAcademicYear, abIsDebitEntry, receiptNo);
            AddFeeTypes(stud, payable, abIsDebitEntry, row, receiptNo);

            int iConcessionAmount = lstPaid.Where(ssfd => ssfd.ConcessionAmount != 0).Select(ssfd => ssfd.ConcessionAmount).FirstOrDefault();

            row.Append(AddCell(iConcessionAmount.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));

            int iPaidAmount = lstPaid.Sum(ssfd => ssfd.Amount);
            iPaidAmount = iPaidAmount - iConcessionAmount;

            row.Append(AddCell(iPaidAmount.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));

            int iPayableAmout = moFeeReport.SchooolwiseStudentFeeDetailss.Where(ssfd => ssfd.YearwiseStudentId == stud.YearwiseStudentId && ssfd.PayableFor == payable).Sum(ssfd => ssfd.Amount);
            int iPaidAmout = moFeeReport.PaidFeeDetails.Where(ssfd => ssfd.YearwiseStudentId == stud.YearwiseStudentId && ssfd.PayableFor == payable && ssfd.ReceiptNumber.ToInt() > receiptNo.ToInt()).Sum(ssfd => ssfd.Amount);
            row.Append(AddCell((iPayableAmout + iPaidAmout).ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));

            if (lstPaid.Count > 0)
            {
                FeeReportNS.PaidFeeDetails obj = lstPaid[0];
                row.Append(AddCell(obj.PaidDate.ToString(Constants.S_DATE_FORMAT), CellValues.String, StudentPaidFeeEnum.CenterData));
                row.Append(AddCell(obj.PaymentMode, CellValues.String, StudentPaidFeeEnum.CenterData));
                row.Append(AddCell((String.IsNullOrEmpty(obj.TransactionId) ? "-" : obj.TransactionId), CellValues.String, StudentPaidFeeEnum.LeftData));
                row.Append(AddCell((obj.ChequeDate.ToString(Constants.S_DATE_FORMAT) == "01-Jan-1900" ? "-" : obj.ChequeDate.ToString(Constants.S_DATE_FORMAT)), CellValues.String, StudentPaidFeeEnum.CenterData));
                row.Append(AddCell((String.IsNullOrEmpty(obj.BankName) ? "-" : obj.BankName), CellValues.String, StudentPaidFeeEnum.LeftData));
                row.Append(AddCell(obj.ReceiptNumber, CellValues.String, StudentPaidFeeEnum.CenterData));
                row.Append(AddCell((String.IsNullOrEmpty(obj.AdditionalRemark) ? "-" : obj.AdditionalRemark), CellValues.String, StudentPaidFeeEnum.LeftData));
                row.Append(AddCell(obj.CreatedBy, CellValues.String, StudentPaidFeeEnum.LeftData));
            }

            AddStatusAndFeeCategory(stud, row);

            AddTransportSection(stud, row);

            aoSheetData1.Append(row);
            miStudentPaidFeeStartupRow++;
        }
    }

    /// <summary>
    /// This method is used to add basix record.
    /// </summary>
    /// <param name="stud"></param>
    /// <param name="payable"></param>
    /// <param name="row"></param>
    private void AddBasicData(FeeReportNS.StudentInfo stud, string payable, Row row, string asAcademicYear, bool abIsDebitEntry, string asReceiptNo)
    {
        StudentPaidFeeEnum oStudentPaidFeeEnum;
        if (abIsDebitEntry)
            oStudentPaidFeeEnum = StudentPaidFeeEnum.LeftDataWithGreenColor;
        else
        {
            int iTotalPaid = moFeeReport.PaidFeeDetails.Where(ssfd => ssfd.YearwiseStudentId == stud.YearwiseStudentId && ssfd.PayableFor == payable && ssfd.ReceiptNumber == asReceiptNo).Sum(ssfd => ssfd.Amount);
            int iPayableAmount = moFeeReport.PayableSummaryDetails.Where(ssfd => ssfd.YearwiseStudentId == stud.YearwiseStudentId && ssfd.PayableFor == payable).FirstOrDefault().TotalAmount;

            if (iPayableAmount == iTotalPaid)
                oStudentPaidFeeEnum = StudentPaidFeeEnum.LeftDataWithLightRedColor;
            else if (iPayableAmount != iTotalPaid && iTotalPaid != 0)
                oStudentPaidFeeEnum = StudentPaidFeeEnum.LeftDataWithLightBlueColor;
            else
                oStudentPaidFeeEnum = StudentPaidFeeEnum.LeftDataWithGreenColor;
        }

        row.Append(AddCell(Settings.Location, CellValues.String, (abIsDebitEntry ? StudentPaidFeeEnum.LeftDataWithGreenColor : oStudentPaidFeeEnum)));

        row.Append(AddCell(asAcademicYear, CellValues.String, StudentPaidFeeEnum.LeftData));
        row.Append(AddCell(stud.Class, CellValues.String, StudentPaidFeeEnum.LeftData));
        row.Append(AddCell(stud.EnrolmentNo, CellValues.String, StudentPaidFeeEnum.LeftData));
        row.Append(AddCell(stud.StudentName, CellValues.String, StudentPaidFeeEnum.LeftData));

        row.Append(AddCell(payable, CellValues.String, StudentPaidFeeEnum.LeftData));
    }

    /// <summary>
    /// This method is used to add fee types.
    /// </summary>
    /// <param name="stud"></param>
    /// <param name="payable"></param>
    /// <param name="abIsDebitEntry"></param>
    /// <param name="row"></param>
    /// <param name="asReceiptNo"></param>
    private void AddFeeTypes(FeeReportNS.StudentInfo stud, string payable, bool abIsDebitEntry, Row row, string asReceiptNo)
    {
        moFeeReport.FeeTypes.OrderBy(fd => fd.OrgFeeTypeId).ToList().ForEach(fd =>
        {
            int iAmount = 0;

            if (abIsDebitEntry)
                iAmount = moFeeReport.SchooolwiseStudentFeeDetailss.Where(ssfd => ssfd.YearwiseStudentId == stud.YearwiseStudentId && ssfd.PayableFor == payable && ssfd.FeeType == fd.Name).Select(ssfd => ssfd.Amount).FirstOrDefault();
            else
                iAmount = moFeeReport.PaidFeeDetails.Where(ssfd => ssfd.YearwiseStudentId == stud.YearwiseStudentId && ssfd.PayableFor == payable && ssfd.FeeType == fd.Name && ssfd.ReceiptNumber == asReceiptNo).Select(ssfd => ssfd.Amount).FirstOrDefault();

            row.Append(AddCell(iAmount.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));

        });
    }


    /// <summary>
    /// This method is used to add fee category.
    /// </summary>
    /// <param name="stud"></param>
    /// <param name="row"></param>
    private void AddStatusAndFeeCategory(FeeReportNS.StudentInfo stud, Row row)
    {
        row.Append(AddCell(stud.Status, CellValues.String, StudentPaidFeeEnum.LeftData));
        row.Append(AddCell(stud.FeeCategory, CellValues.String, StudentPaidFeeEnum.LeftData));
    }

    /// <summary>
    /// This method is used to add add section.
    /// </summary>
    /// <param name="stud"></param>
    /// <param name="row"></param>
    private void AddTransportSection(FeeReportNS.StudentInfo stud, Row row)
    {
        var otransportDetails = moFeeReport.TransportDetails.Where(td => td.UserId == stud.UserId).FirstOrDefault();
        if (otransportDetails != null)
        {
            row.Append(AddCell(otransportDetails.PickupRoute, CellValues.String, StudentPaidFeeEnum.LeftData));
            row.Append(AddCell(otransportDetails.PickupStop, CellValues.String, StudentPaidFeeEnum.LeftData));
            row.Append(AddCell(otransportDetails.DropRoute, CellValues.String, StudentPaidFeeEnum.LeftData));
            row.Append(AddCell(otransportDetails.DropStop, CellValues.String, StudentPaidFeeEnum.LeftData));
        }
        else
        {
            row.Append(AddCell(DASH, CellValues.String, StudentPaidFeeEnum.LeftData));
            row.Append(AddCell(DASH, CellValues.String, StudentPaidFeeEnum.LeftData));
            row.Append(AddCell(DASH, CellValues.String, StudentPaidFeeEnum.LeftData));
            row.Append(AddCell(DASH, CellValues.String, StudentPaidFeeEnum.LeftData));
        }
    }

    /// <summary>
    /// This method is used to set column width.
    /// </summary>
    /// <param name="aoWorksheet1"></param>
    /// <param name="aiNoOfDays"></param>
    private void SetStudentFeeDetailsColumnWidthForAaryan(Worksheet aoWorksheet1, int aiNoOfDays)
    {
        Columns columns1 = new Columns();
        columns1.Append(new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 20D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 16D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)3U, Max = (UInt32Value)3U, Width = 20D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)4U, Max = (UInt32Value)4U, Width = 15D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)5U, Max = (UInt32Value)5U, Width = 30D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)6U, Max = (UInt32Value)6U, Width = 20D, CustomWidth = true });

        columns1.Append(new Column() { Min = (UInt32Value)7U, Max = Convert.ToUInt32(7 + aiNoOfDays), Width = 20D, CustomWidth = true });

        columns1.Append(new Column() { Min = Convert.ToUInt32(7 + aiNoOfDays) + 1, Max = Convert.ToUInt32(7 + aiNoOfDays) + 17, Width = 20D, CustomWidth = true });

        aoWorksheet1.Append(columns1);
    }

    /// <summary>
    /// This method is used to add columns.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="iColCount"></param>
    private void AddPaidFeeHeaderForAaryan(SheetData aoSheetData1, int iColCount)
    {
        Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

        row.Append(AddCell("Branch", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Academic Year", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        
        row.Append(AddCell("Class", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Reg. No.", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Student Name", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Payable For", CellValues.String, StudentPaidFeeEnum.LeftHeader));

        for (int iIndex = 0; iIndex < iColCount; iIndex++)
        {
            row.Append(AddCell(moFeeReport.FeeTypes[iIndex].Name, CellValues.String, StudentPaidFeeEnum.LeftHeader));
            row.Height = 39D;
        }

        row.Append(AddCell("Concession Amount", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Paid Amount", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Unpaid Amount", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Payment Date", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Payment Type", CellValues.String, StudentPaidFeeEnum.CenterHeader));

        row.Append(AddCell("Cheque/Transaction No.", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Cheque Date", CellValues.String, StudentPaidFeeEnum.CenterHeader));

        row.Append(AddCell("Bank Name", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Receipt No.", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Additional Remark", CellValues.String, StudentPaidFeeEnum.LeftHeader));

        row.Append(AddCell("Creator", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Status", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Fee Category", CellValues.String, StudentPaidFeeEnum.LeftHeader));

        row.Append(AddCell("Pickup Route", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Pickup Stop", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Drop Route", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Drop Stop", CellValues.String, StudentPaidFeeEnum.LeftHeader));

        aoSheetData1.Append(row);
    } 

    #endregion

    /// <summary>
    /// This method is used to create work book part for student paid fee details report.
    /// </summary>
    /// <param name="aoPart"></param>
    private void CreateWorkBookPartForStudentPaidFeeReport(WorkbookPart aoPart)
    {
        WorkbookStylesPart workbookStylesPart1 = aoPart.AddNewPart<WorkbookStylesPart>("rId3");
        GenerateReportStyles(workbookStylesPart1);

        WorksheetPart worksheetPart1 = aoPart.AddNewPart<WorksheetPart>("rId1");
        GenerateStudentPaidFeeDetailsReportContent(worksheetPart1);

        GeneratePartContent(aoPart, "Paid Fee");
    }

    /// <summary>
    /// This method is used to generate worksheet part 1 content.
    /// </summary>
    /// <param name="aoWorksheetPart1"></param>
    private void GenerateStudentPaidFeeDetailsReportContent(WorksheetPart aoWorksheetPart1)
    {
        int iColCount = moStudentFeeDetailsBL.Intervals.Count;

        Worksheet worksheet1 = new Worksheet();
        worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        AddSheetDetails(worksheet1);
        SheetData sheetData1 = new SheetData();

        SetStudentPaidFeeColumnWidth(worksheet1, iColCount);
        AddPaidFeeHeader(sheetData1, iColCount);
        AddStudentPaidFeeDataRows(sheetData1, iColCount);

        worksheet1.Append(sheetData1);

        AddPrintOptions(worksheet1);
        SetPageMargin(worksheet1, 0.2);
        SetPageSetup(worksheet1, OrientationValues.Landscape);
        aoWorksheetPart1.Worksheet = worksheet1;
    }

    /// <summary>
    /// This method is used add column header to excel file.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="iColCount"></param>
    private void AddPaidFeeHeader(SheetData aoSheetData1, int iColCount)
    {
        Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

        row.Append(AddCell("Reg. No.", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Class", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Roll No.", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Student Name", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Mobile No.", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Pending Amt.", CellValues.String, StudentPaidFeeEnum.CenterHeader));

        for (int iIndex = 0; iIndex < iColCount; iIndex++)
        {
            row.Append(AddCell(moStudentFeeDetailsBL.Intervals[iIndex].FeeType + "\n" + moStudentFeeDetailsBL.Intervals[iIndex].PayableFor, CellValues.String, StudentPaidFeeEnum.LeftHeader));
            row.Height = 39D;
        }

        row.Append(AddCell("Total Paid", CellValues.String, StudentPaidFeeEnum.CenterHeader));

        aoSheetData1.Append(row);
    }

    /// <summary>
    /// This method is used to add all students paid fee details data in excel file.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="iColCount"></param>
    private void AddStudentPaidFeeDataRows(SheetData aoSheetData1, int iColCount)
    {
        miStudentPaidFeeStartupRow++;

        mlstStudents.OrderBy(stud => stud.OriginalStandardId).ThenBy(stud => stud.OriginalDivisionId).ThenBy(stud => stud.RollNo).ToList().ForEach
            (
                stud =>
                {
                    Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 14 };
                    row.Append(AddCell(stud.RegistraionNo, CellValues.String, StudentPaidFeeEnum.LeftData));
                    row.Append(AddCell(stud.ClassName, CellValues.String, StudentPaidFeeEnum.LeftData));
                    row.Append(AddCell(stud.RollNo.ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
                    row.Append(AddCell(stud.Name, CellValues.String, StudentPaidFeeEnum.LeftData));
                    row.Append(AddCell(stud.MobileNumber, CellValues.String, StudentPaidFeeEnum.LeftData));

                    var iPaidAmount = moStudentFeeDetailsBL.FeeDetails.Where(std => std.StudentId == stud.YearWiseStudentId).Sum(std => std.Amount);
                    row.Append(AddCell((stud.TotalPayable - iPaidAmount).ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));

                    foreach (var payablefor in moStudentFeeDetailsBL.Intervals)
                    {
                        var PayableFor = moStudentFeeDetailsBL.FeeDetails.Where(ss => ss.StudentId == stud.YearWiseStudentId && ss.PayableFor == payablefor.PayableFor && ss.FeeType == payablefor.FeeType).FirstOrDefault();

                        if (PayableFor != null)
                        {
                            row.Append(AddCell((PayableFor.Amount == 0 ? string.Empty : PayableFor.Amount.ToString()), CellValues.String, StudentPaidFeeEnum.CenterData));
                            //row.Height = 39D;
                        }
                        else
                            row.Append(AddCell("-", CellValues.String, StudentPaidFeeEnum.CenterData));
                    }

                    row.Append(AddCell(iPaidAmount.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));

                    aoSheetData1.Append(row);
                    miStudentPaidFeeStartupRow++;
                }
        );

    }

    ///// <summary>
    ///// This method is used to set column width.
    ///// </summary>
    ///// <param name="aoWorksheet1"></param>
    ///// <param name="aiNoOfDays"></param>
    private void SetStudentPaidFeeColumnWidth(Worksheet aoWorksheet1, int aiNoOfDays)
    {
        Columns columns1 = new Columns();
        columns1.Append(new Column() { Min = (UInt32Value)3U, Max = (UInt32Value)3U, Width = 9D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)4U, Max = (UInt32Value)4U, Width = 35.57D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 15D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 15D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)5U, Max = (UInt32Value)5U, Width = 15D, CustomWidth = true });

        columns1.Append(new Column() { Min = (UInt32Value)6U, Max = Convert.ToUInt32(aiNoOfDays + 4), Width = 18D, CustomWidth = true });

        columns1.Append(new Column() { Min = Convert.ToUInt32(aiNoOfDays + 5), Max = Convert.ToUInt32(aiNoOfDays + 7), Width = 15D, CustomWidth = true });
        columns1.Append(new Column() { Min = Convert.ToUInt32(aiNoOfDays + 8), Max = Convert.ToUInt32(aiNoOfDays + 8), Width = 15D, CustomWidth = true });

        aoWorksheet1.Append(columns1);
    } 

    #endregion
}