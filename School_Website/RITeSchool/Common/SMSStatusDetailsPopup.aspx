<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="SMSStatusDetailsPopup.aspx.cs" Inherits="SMSStatusDetailsPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
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
                            <td align="left" class="ClsBorderlight">
                                <span class="ClsLabel">Mobile No. / Delivery Status :</span>
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
                <td align="center">
                    <div id="divSMSStatus">
                    </div>
                    <asp:HiddenField ID="hidSMSShootId" runat="server" Value="" />
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

            _clienttxtFilter = "<%=this.txtFilter.ClientID %>"
            _clientbtnSearch = "<%=this.btnSearch.ClientID %>"
            _clientbtnClose = "<%=this.btnClose.ClientID %>"
            _clienthidSMSShootId = "<%=this.hidSMSShootId.ClientID %>"

            $(function () {
                DisableButtons(true)
                FillSMSStatus();
            });

            function FillSMSStatus() {
                var questionGrid = $("#divSMSStatus").kendoGrid({
                    columns: [
                        { field: "MobileNos", title: "Mobile No.", width: "200px" },
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
                                url: "SMSStatusDetailsPopup.aspx/GetSMSStatusDetails",
                                contentType: "application/json; charset=utf-8",
                                type: "POST"
                            },
                            parameterMap: function (data, operation) {
                                if (data.models) {
                                    return JSON.stringify({ products: data.models });
                                } else if (operation == "read") {
                                    data = $.extend({ sort: null, filter: null }, data);
                                    data = $.extend({ asFilter: $('#' + _clienttxtFilter).val(), asSMSShootId: $('#' + _clienthidSMSShootId).val() }, data);
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
            }

        </script>
    </div>
</asp:Content>
