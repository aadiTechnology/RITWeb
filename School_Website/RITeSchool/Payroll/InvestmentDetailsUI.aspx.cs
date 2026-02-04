using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;
using System.Web.Services;

public partial class InvestmentDetailsUI : SchoolBase
{
    #region Data Member(s)

    private InvestmentDeclarationBL moInvestmentDeclarationBL;
    private List<InvestmentMethod> mlstMethods;
    
    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill investment details.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            moInvestmentDeclarationBL = new InvestmentDeclarationBL(miSchoolId, miFinancialYearId, miUserId);

            if (Page.Request.Params.Get("__EVENTTARGET") != null)
            {
                if (btnSave.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")) ||
                     btnSubmit.ClientID.Replace('_', '$').Equals(Page.Request.Params.Get("__EVENTTARGET")))
                    FillInvestmentDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display investment details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                DisableValidations();
                FillRegimeDropdown();
                FillInvestmentDetails();
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save investment details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            List<InvestmentDeclaration> lstDeclarations = new List<InvestmentDeclaration>();
            foreach (HtmlTableRow tr in tblInvestmentDeclarations.Rows)
            {
                if (tr.ID != null && tr.ID.Contains("_"))
                {
                    string sId = tr.ID.Substring(3);
                    TextBox txtAmount = tr.FindControl("txt_" + sId) as TextBox;
                    if (txtAmount != null)
                    {
                        if (txtAmount.Enabled && txtAmount.Text != string.Empty && Convert.ToDecimal(txtAmount.Text) != (decimal)0)
                        {
                            InvestmentDeclaration oInvestmentDeclaration = new InvestmentDeclaration
                            {
                                InvestmentMethodId = sId.ToInt(),
                                Amount = Convert.ToDecimal(txtAmount.Text)
                            };
                            lstDeclarations.Add(oInvestmentDeclaration);
                        }
                    }
                }
            }

            moInvestmentDeclarationBL.SaveInvestmentDeclaration(miUserId, base.GenerateXml(lstDeclarations), ddlRegime.SelectedValue.ToInt());

            base.DisplayMessage("Investment details saved successfully !!!", false, tdMessage);
            FillInvestmentDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit investment details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            moInvestmentDeclarationBL.SubmitInvestmentDetails(miUserId);
            base.DisplayMessage("Investment details submitted successfully !!!", false, tdMessage);
            FillInvestmentDetails();
        }
        catch (SqlException ex)
        {
            base.DisplayMessage(ex.Message, true, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

   #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnSubmit });
        btnSubmit.Attributes.Add("onclick", "if(!window.confirm('After submission you will not be able to update any details. Do you want to continue?')) return false;");
        ValSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    /// <summary>
    /// This method is used to fill up investment details.
    /// </summary>
    private void FillInvestmentDetails()
    {
        tblInvestmentDeclarations.Rows.Clear();
        mlstMethods = moInvestmentDeclarationBL.GetInvestmentDetails(miUserId);
        SetSchoolDetails();
        FillSections();
        FillFooterDetails();
        SetButtonState();
    }

    /// <summary>
    /// This method is used to set footer details.
    /// </summary>
    private void FillFooterDetails()
    {
        spnFinYear.InnerText = moInvestmentDeclarationBL.UserDetails.FinancialYearEnd;
        spnFinYear2.InnerText = moInvestmentDeclarationBL.UserDetails.FinancialYearEnd;
        spnName.InnerText = moInvestmentDeclarationBL.UserDetails.UserName;
    }

    /// <summary>
    /// This method is used to manage button state.
    /// </summary>
    private void SetButtonState()
    {
        btnSave.Enabled = true;
        btnSubmit.Enabled = true;
        if (moInvestmentDeclarationBL.UserDetails.IsSubmitted)
        {
            btnSave.Enabled = false;
            btnSubmit.Enabled = false;
        }
        else
        {
            if (!moInvestmentDeclarationBL.UserDetails.IsSaved)
                btnSubmit.Enabled = false;
        }
    }

    /// <summary>
    /// This method is used to fill section details.
    /// </summary>
    private void FillSections()
    {
        moInvestmentDeclarationBL.SectionDetails.OrderBy(st => st.SortOrder).ToList().ForEach
            (
                section =>
                {
                    if (mlstMethods.Any(im => im.SectionId == section.Id))
                    {
                        DisplaySectionDetails(section);

                        int iSrNo = 1;
                        int iRowSpan = 1;
                        HtmlTableCell oBaseCell = null;
                        mlstMethods.Where(im => im.SectionId == section.Id).OrderBy(im => im.Name).ToList().ForEach
                            (
                                im =>
                                {
                                    HtmlTableRow trMethods = new HtmlTableRow();
                                    trMethods.ID = "tr_" + im.Id;
                                    base.AddCell(trMethods, iSrNo.ToString(), "clsMethodName", "left", 1);
                                    base.AddCell(trMethods, im.Name, "clsMethodName", "left", 1);
                                    LinkButton lnkBtn = new LinkButton();
                                    string sId = "lnkCount_" + im.Id;
                                    lnkBtn.ID = sId;
                                    lnkBtn.Text = im.DocumentCount.ToString();
                                    lnkBtn.Attributes.Add("onclick", "if(!OpenDocumentPopup(" + miUserId + "," + im.Id + "," + Constants.DocumentTypes.InvestmentDocuments.ToInt() + ", " + miAcademicYearId + ", " + moInvestmentDeclarationBL.UserDetails.IsSubmitted.ToInt() + ",'" + sId + "')) return false;");
                                    
                                    if (im.AssociatedEarnDeductId == 0)
                                        base.AddCell(trMethods, string.Empty, "clsMethodName", "Center", 1, "width:150px;padding-right:10px", lnkBtn);
                                    else
                                        base.AddCell(trMethods, string.Empty, "clsMethodName", "Center", 1, "width:150px;padding-right:10px");

                                    if (section.GroupMaxAmount != 0)
                                    {
                                        if (iSrNo == 1)
                                        {
                                            HtmlTableCell tdMethod = new HtmlTableCell();
                                            tdMethod.InnerHtml = (section.GroupMaxAmount != 0 ? section.GroupMaxAmount : im.MaxAmount).ToString();
                                            tdMethod.Attributes.Add("class", "clsMethodName");
                                            tdMethod.Align = "right";
                                            tdMethod.Style.Add("padding-right", "5px");
                                            trMethods.Cells.Add(tdMethod);
                                            oBaseCell = tdMethod;
                                        }
                                    }
                                    else
                                    {
                                        HtmlTableCell tdMethod = new HtmlTableCell();
                                        tdMethod.InnerHtml = (section.GroupMaxAmount != 0 ? section.GroupMaxAmount : im.MaxAmount).ToString();
                                        tdMethod.Attributes.Add("class", "clsMethodName");
                                        tdMethod.Align = "right";
                                        tdMethod.Style.Add("padding-right", "5px");
                                        trMethods.Cells.Add(tdMethod);
                                    }

                                    ManageAmountColumn(im, trMethods);

                                    tblInvestmentDeclarations.Rows.Add(trMethods);
                                    iSrNo++;
                                    iRowSpan++;
                                }

                            );

                        if (oBaseCell != null)
                            oBaseCell.RowSpan = iRowSpan - 1;

                        DisplaySectionTotal(section);
                        AddEmptyRow();
                    }
                }

            );

        DisplayGrandTotal();
    }

    /// <summary>
    /// This method is used to display grand total.
    /// </summary>
    private void DisplayGrandTotal()
    {
        HtmlTableRow trTotal = new HtmlTableRow();
        var dcTotal = moInvestmentDeclarationBL.InvestmentDeclarations.Sum(sd => sd.Amount);
        base.AddCell(trTotal, string.Empty, string.Empty, "right", 2);
        base.AddCell(trTotal, "Grand Total", "clsTotal", "left", 1, "font-weight:bold");
        base.AddCell(trTotal, dcTotal.ToString(), "clsTotal", "right", 1, "font-weight:bold;padding-right:12px;");
        tblInvestmentDeclarations.Rows.Add(trTotal);
    }

    /// <summary>
    /// This method is used to display section total.
    /// </summary>
    /// <param name="aoSectionDetails"></param>
    private void DisplaySectionTotal(SectionDetails aoSectionDetails)
    {
        HtmlTableRow trSubTotal = new HtmlTableRow();
        var dcSubTotal = moInvestmentDeclarationBL.InvestmentDeclarations.Where(sd => sd.SectionId == aoSectionDetails.Id).Sum(sd => sd.Amount);
        base.AddCell(trSubTotal, string.Empty, string.Empty, "right", 3);
        base.AddCell(trSubTotal, "Total Amount", "clsTotal", "right", 1, "font-weight:bold;padding-right:5px;");
        base.AddCell(trSubTotal, dcSubTotal.ToString(), "clsTotal", "right", 1, "font-weight:bold;padding-right:12px;");
        tblInvestmentDeclarations.Rows.Add(trSubTotal);
    }

    /// <summary>
    /// This method is used to add empty row.
    /// </summary>
    private void AddEmptyRow()
    {
        HtmlTableRow trEmptyRow = new HtmlTableRow();
        base.AddCell(trEmptyRow, string.Empty, "height20");
        tblInvestmentDeclarations.Rows.Add(trEmptyRow);
    }

    /// <summary>
    /// This method is used to manage amount column.
    /// </summary>
    /// <param name="aoInvestmentMethod"></param>
    /// <param name="atrMethods"></param>
    private void ManageAmountColumn(InvestmentMethod aoInvestmentMethod, HtmlTableRow atrMethods)
    {
        if (aoInvestmentMethod.AssociatedEarnDeductId == 0)
        {
            TextBox oTextBox = new TextBox();
            oTextBox.ID = "txt_" + aoInvestmentMethod.Id;
            oTextBox.CssClass = "midTextbox";
            oTextBox.MaxLength = 7;
            oTextBox.Attributes.Add("onkeypress", "return blockNonNumbers (this, event, true, false);");
            oTextBox.Attributes.Add("onblur", "extractNumber(this,2,false)");
            oTextBox.Attributes.Add("onkeyup", "extractNumber(this,2,false)");

            oTextBox.Style.Add("text-align", "right");
            oTextBox.Style.Add("padding-right", "2px");
            oTextBox.Attributes.Add("onpaste", "event.returnValue=false");
            oTextBox.Attributes.Add("ondrop", "event.returnValue=false");

            var oDeclaration = moInvestmentDeclarationBL.InvestmentDeclarations.Where(id => id.InvestmentMethodId == aoInvestmentMethod.Id).FirstOrDefault();
            if (oDeclaration != null)
                oTextBox.Text = oDeclaration.Amount.ToString();

            if (moInvestmentDeclarationBL.UserDetails.IsSubmitted)
                oTextBox.Enabled = false;

            base.AddCell(atrMethods, string.Empty, "clsMethodName", "right", 1, "width:150px;padding-right:10px", oTextBox);
        }
        else
        {
            Label oLabel = new Label();
            var oDeclaration = moInvestmentDeclarationBL.InvestmentDeclarations.Where(id => id.InvestmentMethodId == aoInvestmentMethod.Id).FirstOrDefault();
            if (oDeclaration != null)
                oLabel.Text = oDeclaration.Amount.ToString();
            else
                oLabel.Text = Constants.S_ZERO;

            base.AddCell(atrMethods, string.Empty, "clsMethodName", "right", 1, "width:150px;font-weight:Bold;padding-right:12px;", oLabel);
        }
    }

    /// <summary>
    /// This method is used to display section details.
    /// </summary>
    /// <param name="aoSectionDetails"></param>
    private void DisplaySectionDetails(SectionDetails aoSectionDetails)
    {
        HtmlTableRow trSection = new HtmlTableRow();
        base.AddCell(trSection, aoSectionDetails.Name, "clsSectionName", "left", 2);
        base.AddCell(trSection, "Attachment Count", "clsSectionName", "Center", 1, "width:100px");
        base.AddCell(trSection, "Maximum Limit Rs.", "clsSectionName", "right", 1, "width:150px;padding-right:5px");
        base.AddCell(trSection, "Amount", "clsSectionName", "Center", 1, "width:150px");
        tblInvestmentDeclarations.Rows.Add(trSection);
    }

    private void FillRegimeDropdown()
    {
        List<UserDetails> lstRegime = moInvestmentDeclarationBL.GetRegimeDetails();
        ListSource.FillDropDownList(lstRegime, ddlRegime, "Name", "Id", Constants.S_SELECT);
    }
    
    /// <summary>
    /// This method is used to set school and user details.
    /// </summary>
    private void SetSchoolDetails()
    {
        lblSchoolName.Text = moInvestmentDeclarationBL.UserDetails.SchoolName;
        lblSchoolAddress.Text = moInvestmentDeclarationBL.UserDetails.SchoolAddress;
        lblFormLabel.Text = "INVESTMENT DECLARATION FORM FOR FINANCIAL YEAR " + moInvestmentDeclarationBL.UserDetails.FinancialYear;

        lblName.Text = moInvestmentDeclarationBL.UserDetails.UserName;
        lblDesignation.Text = moInvestmentDeclarationBL.UserDetails.Designation;
        lblEmployeeNo.Text = moInvestmentDeclarationBL.UserDetails.EmployeeNo;
        lblPanNo.Text = moInvestmentDeclarationBL.UserDetails.PanNo;
        lblGender.Text = moInvestmentDeclarationBL.UserDetails.Gender;
        lblAddress.Text = moInvestmentDeclarationBL.UserDetails.Address;

        if(!IsPostBack)
            ddlRegime.SelectedValue = moInvestmentDeclarationBL.UserDetails.Id.ToString();
    }

    /// <summary>
    /// This method is used to return query string.
    /// </summary>
    /// <param name="aiUserId"></param>
    /// <param name="aiDocumentId"></param>    
    /// <returns></returns>
    [WebMethod]
    public static string GetQueryString(int aiUserId, int aiDocumentId, int DocumentTypeId, int aiAcademicYearId, int IsSubmited, string asIdnt)
    {
        return CommonUtility.EncryptQuerystring("UserId=" + aiUserId + "&DocumentId=" + aiDocumentId + "&DocumentTypeId=" + DocumentTypeId + "&AcademicYear=" + aiAcademicYearId + "&IsSubmited=" + IsSubmited + "&ClientId=" + asIdnt);
    }

    private void DisableValidations()
    {
        if (miSchoolId != Constants.SchoolId.PPSN.ToInt())
        {
            ReqRegime.Enabled = false;
            starspan.Visible = false;
        }
        else
        {
            ReqRegime.Enabled = true;
            starspan.Visible = true;
        }
    }
         
    
   

    #endregion
}