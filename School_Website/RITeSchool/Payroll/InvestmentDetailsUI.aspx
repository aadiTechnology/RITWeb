<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="InvestmentDetailsUI.aspx.cs" Inherits="InvestmentDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <style type="text/css">
        .clsLabelHeader
        {
            font-weight: bold;
        }
        
        .clsSectionName
        {
            color: Black;
            font-weight: bold;
            background-color: #AAC6FF;
            font: Arial,sans-serif;
            font-size: 13px;
        }
        
        .clsMethodName
        {
            background-color: #D5E2FF;
            font: 0.75em Arial,sans-serif;
            font-size: 9pt;
        }
        
        .clsTotal
        {
            font-weight: bold;
            background-color: #84ACFF;
            font: Arial,sans-serif;
            font-size: 13px;
            color: Black;
        }
    </style>
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="80%">
            <tr>
                <td align="center">
                    <table width="80%">
                        <tr>
                            <td colspan="4">
                                <asp:ValidationSummary ID="ValSum" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" id="tdMessage" runat="server" align="center">
                                <asp:Label ID="lblMessage" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                    Text="" EnableViewState="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4" class="SocietyName">
                                <asp:Label ID="lblSchoolName" runat="server" Text="" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4" class="ActualSchoolName">
                                <asp:Label ID="lblSchoolAddress" runat="server" Text="" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 2px;">
                            <td>
                            </td>
                        </tr>
                        <tr style="background-color: #AAC6FF">
                            <td align="center" colspan="4">
                                <asp:Label ID="lblFormLabel" runat="server" Text="" Font-Bold="true" Font-Size="Large"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 5px;">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <span class="ClsLabel clsLabelHeader">Name :</span>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblName" runat="server" Text="" CssClass="ClsHilightTextB"></asp:Label>
                            </td>
                            <td class="ClsBorderlight">
                                <span class="ClsLabel clsLabelHeader">Designation :</span>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblDesignation" runat="server" Text="" CssClass="ClsHilightTextB"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <span class="ClsLabel clsLabelHeader">Employee No :</span>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblEmployeeNo" runat="server" Text="" CssClass="ClsHilightTextB"></asp:Label>
                            </td>
                            <td class="ClsBorderlight">
                                <span class="ClsLabel clsLabelHeader">Pan No :</span>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblPanNo" runat="server" Text="" CssClass="ClsHilightTextB"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <span class="ClsLabel clsLabelHeader">Address :</span>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblAddress" runat="server" Text="" CssClass="ClsHilightTextB"></asp:Label>
                            </td>
                            <td class="ClsBorderlight">
                                <span class="ClsLabel clsLabelHeader">Gender :</span>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblGender" runat="server" Text="" CssClass="ClsHilightTextB"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <span class="ClsLabel clsLabelHeader">Regime :</span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlRegime" runat="server" AutoPostBack="false" CssClass="MidCombo"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="ReqRegime" runat="server" ControlToValidate="ddlRegime" InitialValue="0" Display="None" ViewStateMode="Enabled" ErrorMessage="Regime category should be selected.">
                                </asp:RequiredFieldValidator>                               
                                <span id="starspan" class="ClsMdtStar" runat="server">*</span>
                            </td>
                        </tr>
                        <tr style="height: 5px;">
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table id="tblInvestmentDeclarations" runat="server" width="80%">
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="80%">
                        <tr class="Height10">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="justify">
                                <span style="line-height: 18px; float: inherit; font-family: Arial,sans-serif; font-size: 9pt;">
                                    I further undertake to provide all documentary proofs of payment made by me before
                                    25<span style="vertical-align:super">th</span> January, <span id="spnFinYear" runat="server"></span> and if I fail to do
                                    so, the school can make full deduction of income tax dues from February / March <span id="spnFinYear2"
                                        runat="server"></span> salary.<br />
                                    <br />
                                    I here by declare that Information as stated above is true and correct. I also authorize
                                    the School to recover tax (TDS) from my salary based on the declaration/documents
                                    submitted by me. I am personally liable to Income Tax proceedings for any misstatements
                                    in the declaration or proofs submitted herewith if they are inconsistent with the
                                    requirement of Income misstatements in the declaration or proofs submitted herewith
                                    if they are inconsistent with the requirement of Income Tax Act, 1961.<br />
                                    <br />
                                    I <span id="spnName" runat="server" style="font-weight: bold;"></span> solemnly declare
                                    that to the best of my knowledge and belief the information given above is correct
                                    and complete. </span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click"
                        UseSubmitBehavior="False" />
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn" UseSubmitBehavior="False"
                        OnClick="btnSubmit_Click" />
                </td>
            </tr>
            <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
        </table>
    </div>
    <script type="text/javascript" language="javascript">

        function OpenDocumentPopup(userid, investmentid, DocumentTypeId, AcademicYearId, IsSubmited, idnt) {            
            $.ajax({
                type: "POST",
                data: '{"aiUserId":"' + userid + '","aiDocumentId":"' + investmentid + '", "DocumentTypeId":"' + DocumentTypeId + '", "aiAcademicYearId":"' + AcademicYearId + '", "IsSubmited":"' + IsSubmited + '","asIdnt":"' + idnt + '"}',
                url: "InvestmentDetailsUI.aspx/GetQueryString",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    window.open('../Payroll/InvestmentDocumentPopup.aspx?' + msg.d, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1050,height=650').focus();
                    return false;
                },
                error: function (xhr, errorType, exception) {
                    var errorMessage = exception || xhr.statusText;
                    alert(errorMessage)
                }
            });
        }

        function UpdateFileUploadCount(count) {        
            if (count != null && count != "") {
                var cnt = count.split('$')
                if (cnt.length > 0) {                    
                    $('[id*='+cnt[1]+']').text(cnt[0])
                }
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
