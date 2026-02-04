<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="SalaryPaymentPopup.aspx.cs" Inherits="SalaryPaymentPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="left">
                    <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="height: 20px" class="ClsGrayMainTitle" valign="middle">
                                <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                    <tr>
                                        <td align="center" class="MainTitleHead" style="height: 20px">
                                            <span style="font-weight: bold">Salary Payment Details</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 10px;">
                <td>
                </td>
            </tr>
            <tr>
                <td style="height: 10px;" align="right">
                    <span class="ClsMdtStar">* Mandatory Fields</span>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:ValidationSummary ID="valSum" runat="server" CssClass="LblErrorMsg" />
                    <asp:CustomValidator ID="cstValNumber" runat="server" Display="None" ClientValidationFunction="ValidateNumber"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td class="ClsBorderlight" align="left" width="200px">
                                <span class="ClsLabel">Year : </span>
                            </td>
                            <td align="left" class="ClsHilightBGB">
                                <label id="lblYear" class="ClsLabel">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight" align="left">
                                <span class="ClsLabel">Month : </span>
                            </td>
                            <td align="left" class="ClsHilightBGB">
                                <label id="lblMonth" class="ClsLabel">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight" align="left">
                                <span class="ClsLabel">Payment Type : </span>
                            </td>
                            <td align="left">
                                <asp:RadioButton ID="optCheque" runat="server" Text="Cheque" CssClass="ClsLabel"
                                    GroupName="PaymentType" />
                                <asp:RadioButton ID="optOnline" runat="server" Text="Online" CssClass="ClsLabel"
                                    GroupName="PaymentType" />
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight" align="left">
                                <span class="ClsLabel">Cheque / Transaction Number : </span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtNumber" runat="server" CssClass="LrgTxtBox" MaxLength="25" Style="text-align: right;
                                    padding-right: 2px;" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                    ondrop="event.returnValue=false"></asp:TextBox>
                                <span class="ClsMdtStar">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="SaveDetails(); return false;"
                                    CssClass="ClsBtn" />
                                <asp:HiddenField ID="hidMonthId" runat="server" Value="0" />
                                <asp:HiddenField ID="hidYear" runat="server" Value="0" />
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td colspan="2">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div id="divPaymentDetails">
                    </div>
                </td>
            </tr>
            <tr style="height: 10px;">
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="ClosePopup(); return false;"
                        CssClass="ClsBtn" />
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            var _schoolId = "<%=miSchoolId %>";
            var _clientoptOnline = "<%=this.optOnline.ClientID %>"
            var _clientoptCheque = "<%=this.optCheque.ClientID %>"
            var _clienttxtNumber = "<%=this.txtNumber.ClientID %>"
            var _clienthidMonthId = "<%=this.hidMonthId.ClientID %>"
            var _clienthidYear = "<%=this.hidYear.ClientID %>"

            $(function () {
                FillPaymentDetails();
            });

            function FillPaymentDetails() {
                var questionGrid = $("#divPaymentDetails").kendoGrid({
                    columns: [
                        { field: "Year", title: "Year", width: "15%", filterable: false },
                        { field: "Month", title: "Month", width: "20%",

                            sortable: {
                                compare: function (a, b) {
                                    return a.MonthId === b.MonthId ? 0 : (a.MonthId > b.MonthId) ? 1 : -1;
                                }
                            },
                            filterable: false
                        },
                        { field: "IsOnlineTransactionText", title: "Is OnlineTransaction?", width: "30%", filterable: false },
                        { field: "TransactionNumber", title: "Cheque / Transaction Number", width: "35%", sortable: false }
                        ],
                    pageable: {
                        refresh: true,
                        pageSizes: true,
                        buttonCount: 5
                    },
                    filterable: true,
                    sortable: { mode: "multiple" },
                    editable: false,
                    selectable: "single row",
                    dataBound: SetButtonState,
                    dataSource: {
                        pageSize: 10,
                        schema: {
                            data: "d.Data",
                            total: "d.Total",
                            model: {
                            }
                        },
                        batch: true,
                        transport: {
                            read: {
                                url: "SalaryPaymentPopup.aspx/GetAllPaymentDetails",
                                contentType: "application/json; charset=utf-8",
                                type: "POST"
                            },
                            parameterMap: function (data, operation) {
                                if (data.models) {
                                    return JSON.stringify({ products: data.models });
                                } else if (operation == "read") {
                                    data = $.extend({ sort: null, filter: null }, data);
                                    data = $.extend({ aiSchoolId: _schoolId }, data);
                                    return JSON.stringify(data);
                                }
                            }
                        }
                    }
                });
            }

            function SetButtonState(e) {
                var grid = this;
                grid.tbody.find('>tr').each(function () {
                    var dataItem = grid.dataItem(this);
                    var editButton = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-Edit");

                    if (dataItem.IsLastRecord == false) {
                        editButton.hide();
                    }
                    else {
                        $("#lblYear").text(dataItem.Year);
                        $("#lblMonth").text(dataItem.Month);
                        if (dataItem.IsOnlineTransaction)
                            document.getElementById(_clientoptOnline).checked = true;
                        else
                            document.getElementById(_clientoptCheque).checked = true;

                        document.getElementById(_clienttxtNumber).value = dataItem.TransactionNumber;
                        document.getElementById(_clienthidMonthId).value = dataItem.MonthId;
                        document.getElementById(_clienthidYear).value = dataItem.Year;
                    }
                })
            }

            function SaveDetails() {
                var validationResult = true;
                if (typeof (Page_ClientValidate) == 'function')
                    validationResult = Page_ClientValidate("");

                if (validationResult) {
                    var isOnlineTransaction = document.getElementById(_clientoptOnline).checked
                    var transactionNumber = document.getElementById(_clienttxtNumber).value
                    var monthId = document.getElementById(_clienthidMonthId).value
                    var year = document.getElementById(_clienthidYear).value
                    var isOnlineTransaction = (isOnlineTransaction ? 1 : 0)

                    $.ajax({
                        type: "POST",
                        data: '{"abIsOnlineTransaction":"' + isOnlineTransaction + '","asTransactionNumber":"' + transactionNumber + '","aiSchoolId":"' + _schoolId + '","aiMonthId":"' + monthId + '","aiYear":"' + year + '"}',
                        url: "SalaryPaymentPopup.aspx/SavePaymentDetails",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            $("#divPaymentDetails").data("kendoGrid").dataSource.read();
                            alert("Salary payment details saved successfully !!!");
                        },
                        error: function (msg) {
                            alert("Failed to save salary payment details.");
                        }
                    });
                }
            }

            function ClosePopup() {
                window.close();
            }

            function ValidateNumber(oSrc, args) {
                var number = document.getElementById(_clienttxtNumber).value;
                number = number.trim()
                if (number == "" || number == 0) {
                    oSrc.errormessage = "Cheque / Transaction Number should not be blank or zero.";
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }

        </script>
    </div>
</asp:Content>
