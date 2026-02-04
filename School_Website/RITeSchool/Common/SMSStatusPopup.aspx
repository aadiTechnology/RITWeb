<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="SMSStatusPopup.aspx.cs" Inherits="SMSStatusPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr align="center" id="trProgressImage" style="display: none;">
                <td align="center">
                    <table>
                        <tr>
                            <td>
                                <asp:Image ImageUrl="~/RITeSchool/images/Loading9.gif" runat="server" ID="imgLoading9" />
                            </td>
                            <td valign="middle" class="ClsLoadingBG">
                                <span class="LoadingTxt">Please wait...</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" class="ClsGrayMainTitle">
                    <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                        <tr>
                            <td align="left" class="MainTitleHead" style="height: 20px">
                                <span style="font-weight: bold">SMS Delivery Status</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="Height10">
                <td>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td align="left" class="ClsBorderlight" width="250px">
                                <span class="ClsLabel">SMS Sent Date :</span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtSMSDate" CssClass="smlTxtBox" runat="server" ReadOnly="true" />
                                <rjs:PopCalendar ID="cal_PaymentDate" runat="server" Control="txtSMSDate" Format="dd MMM yyyy"
                                    Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="SMS sent date should not be blank."
                                    AutoPostBack="False" To-Today="true" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="ClsBorderlight">
                                <span class="ClsLabel">Mobile No. / SMS Text / Delivery Status :</span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtFilter" CssClass="ExLrgTxtBox" runat="server" MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" OnClientClick="LoadData(); return false;" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="Height10">
                <td>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <table>
                        <tr>
                            <td align="left" colspan="2">
                                <span class="ClsLblLgnd">Delivery Status Details : </span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span class="ClsLabel"><b>EXPIRED206</b></span>
                            </td>
                            <td align="left">
                                <span class="ClsLabel">- Unidentified Subscriber.</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span class="ClsLabel"><b>EXPIRED20d</b></span>
                            </td>
                            <td align="left">
                                <span class="ClsLabel">- Call Barred.</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span class="ClsLabel"><b>EXPIRED21b, UNDELIV21b</b></span>
                            </td>
                            <td align="left">
                                <span class="ClsLabel">- Mobile switched off.</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span class="ClsLabel"><b>EXPIRED220, UNDELIV220</b></span>
                            </td>
                            <td align="left">
                                <span class="ClsLabel">- Inbox Full.</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span class="ClsLabel"><b>EXPIRED222</b></span>
                            </td>
                            <td align="left">
                                <span class="ClsLabel">- Network System Failure.</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span class="ClsLabel"><b>SUBMIT</b></span>
                            </td>
                            <td align="left">
                                <span class="ClsLabel">- SMS submitted from our server.</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span class="ClsLabel"><b>UNDELIV201</b></span>
                            </td>
                            <td align="left">
                                <span class="ClsLabel">- Unknown Subscriber.</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span class="ClsLabel"><b>UNDELIV20b</b></span>
                            </td>
                            <td align="left">
                                <span class="ClsLabel">- Tele Service Not Provisioned.</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span class="ClsLabel"><b>UNDELIV400, EXPIRED401</b></span>
                            </td>
                            <td align="left">
                                <span class="ClsLabel">- Network issue.</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="Height10">
                <td>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div id="divSMSStatus">
                    </div>
                </td>
            </tr>
            <tr class="Height10">
                <td>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" OnClientClick="window.close();" />
                </td>
            </tr>
        </table>
        <script type="text/javascript">

            _clienttxtSMSDate = "<%=this.txtSMSDate.ClientID %>"
            _clienttxtFilter = "<%=this.txtFilter.ClientID %>"
            _clientbtnSearch = "<%=this.btnSearch.ClientID %>"
            _clientbtnClose = "<%=this.btnClose.ClientID %>"

            $(function () {
                DisableButtons(true)
                FillSMSStatus();
            });

            function FillSMSStatus() {
                var questionGrid = $("#divSMSStatus").kendoGrid({
                    columns: [
                        { field: "SMSTime", title: "SMS Time", width: "100px" },
                        { field: "MobileNos", title: "Mobile No(s)", width: "200px" },
                        { field: "SMSText", title: "SMS Text" },
                        { field: "TotalSMS", title: "Total SMS", width: "100px" },
                        { field: "DeliveryStatus", title: "Delivery Status", width: "150px" }
                        ],
                    pageable: {
                        refresh: true,
                        pageSizes: true,
                        buttonCount: 5
                    },
                    filterable: false,
                    sortable: false,
                    editable: false,
                    selectable: "single row",
                    dataBound: onDataBound,
                    dataSource: {
                        serverPaging: true,
                        serverSorting: false,
                        serverFiltering: false,
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
                                url: "SMSStatusPopup.aspx/GetSMSStatusDetails",
                                contentType: "application/json; charset=utf-8",
                                type: "POST"
                            },
                            parameterMap: function (data, operation) {
                                if (data.models) {
                                    return JSON.stringify({ products: data.models });
                                } else if (operation == "read") {
                                    data = $.extend({ sort: null, filter: null }, data);
                                    data = $.extend({ asSMSSentDate: $('#' + _clienttxtSMSDate).val(), asFilter: $('#' + _clienttxtFilter).val() }, data);
                                    return JSON.stringify(data);
                                }
                            }
                        }
                    }
                });
            }

            function DisableButtons(status) {
                if (status == true) {
                    $("#trProgressImage").show();
                    $("#" + _clientbtnSearch).attr("disabled", "disabled");
                    $("#" + _clientbtnClose).attr("disabled", "disabled");
                }
                else {
                    $("#" + _clientbtnSearch).removeAttr("disabled");
                    $("#" + _clientbtnClose).removeAttr("disabled");
                    $("#trProgressImage").hide();
                }
            }

            function onDataBound(arg) {
                DisableButtons(false)
            }

            function LoadData() {
                $("#divSMSStatus").data("kendoGrid").dataSource.read();
                $("#divSMSStatus").data("kendoGrid").dataSource.page(1);
            }

        </script>
    </div>
</asp:Content>
