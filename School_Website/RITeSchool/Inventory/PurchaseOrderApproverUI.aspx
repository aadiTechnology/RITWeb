<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PurchaseOrderApproverUI.aspx.cs" Inherits="PurchaseOrderApproverUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table style="width: 100%; text-align: center;" align="center">
        <tr>
            <td align="right" style="padding-right: 30px" valign="bottom">
                <span class="ClsMdtStar">*</span>
                <asp:Label ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False"
                    Text="Mandatory Fields"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table width="75%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"
                                ForeColor="Blue" Style="font-weight: bold"></asp:Label>
                            <asp:Label ID="lblErrorMsg" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"
                                ForeColor="Red" Style="float: left;"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td style="width: 155px;" class="ClsBorderlight">
                            <asp:Label ID="lblLocation" runat="server" CssClass="ClsLabel" Text="Status" Height="16px"></asp:Label>
                            <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" style="padding-left: 5px; width: 300px; height: 32px;">
                            <asp:DropDownList ID="cmbStatus" runat="server" Width="180px" 
                                AutoPostBack="false">
                                <asp:ListItem Value="0" Text="Waiting for My Approval."></asp:ListItem>
                                <asp:ListItem Value="1" Text="Approved."></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <div id="divPODetails" data-role="grid" style="width: 60%; float: inherit; text-align: center;
                    margin-top: 10px;">
                </div>
                <asp:HiddenField ID="hidSchoolId" runat="server" />
                <asp:HiddenField ID="hidAcademicYearId" runat="server" />
                <asp:HiddenField ID="hidPOId" runat="server" Value="0" />
                <asp:HiddenField ID="hidUserId" runat="server" />
            </td>
        </tr>       
    </table>
    <script type="text/javascript">

        var _schoolId = $("#" + "<%=hidSchoolId.ClientID %>").val();
        var _academicYearId = $("#" + "<%=hidAcademicYearId.ClientID %>").val();
        var _POId = "<%=this.hidPOId.ClientID %>"
        var _loginUserId = "<%=this.hidUserId.ClientID %>";
        _clientcmbStatus = "<%=this.cmbStatus.ClientID %>";
        var _lblMessage = "<%=this.lblMessage.ClientID %>";
        _clientlblMessage = "<%=this.lblMessage.ClientID %>";

        $(function () {
            $("#divPODetails").kendoGrid({
                columns: [
                        { field: "RequesterName", title: "Name", attributes: { style: "text-align:left;"} },
                        { field: "PODate", title: "PO Date", attributes: { style: "text-align:left;" }, format: "{0:dd-MMM-yyyy}" },
                        { field: "POCode", title: "PO Code", attributes: { style: "text-align:left;"} },
                         {
                             command: [
                            { text: "View", name: "Edit", click: GetPurchaseOrder },
                            { text: "Approve", name: "Delete", click: ApprovePurchaseOrder },
                            ], title: "Action", width: "175px"
                         }
                        ],
                pageable: true,
                filterable: true,
                sortable: true,
                editable: "popup",
                selectable: "single row",
                dataBound: OnDataBound,
                dataSource: {
                    pageSize: 20,
                    schema: {
                        data: "d.Data",
                        total: "d.Total",
                        model: {
                            fields: {
                                "POId": { editable: false, type: "number" },
                                "RequesterName": { editable: false, type: "string" },
                                "PODate": { editable: false, type: "date" },
                                "POCode": { editable: false, type: "string" },
                                "UserId": { editable: false, type: "number" },
                                "RequesterId": { editable: false, type: "number" }
                            }
                        }
                    },
                    batch: true,
                    transport: {
                        read: {
                            url: "PurchaseOrderApproverUI.aspx/GetAllPODetails",
                            contentType: "application/json; charset=utf-8",
                            type: "POST",
                            complete: function () {
                            }
                        },

                        parameterMap: function (data, operation) {
                            if (data.models) {
                                return JSON.stringify({ products: data.models });
                            } else if (operation == "read") {
                                var asStatusId = document.getElementById(_clientcmbStatus).value;
                                data = $.extend({ sort: null, filter: null }, data);
                                data = $.extend({ asSchoolId: _schoolId, asUserId: $("#" + _loginUserId).val(), asStatusId: asStatusId }, data);
                                return JSON.stringify(data);
                            }
                        }
                    }
                }
            });
        });

        function GetPurchaseOrder(e) {
            e.preventDefault();
            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));            
            var sDate = dataItem.PODate.format("dd-MMM-yyyy");            
            $.ajax({
                type: "POST",
                data: '{"asPoId":"' + dataItem.POId + '","adtDate":"' + sDate + '","asRequesterId":"' + dataItem.RequesterId + '","asStatusId":"'+ document.getElementById(_clientcmbStatus).value + '" }',
                url: "PurchaseOrderApproverUI.aspx/GetQuerystring",
                contentType: "application/json; charset=utf-8",
                dataType: "json",                
                success: function (msg) {
                    window.open(msg.d, "_self");
                },
                error: function (msg) {
                }
            });
        }

        function ApprovePurchaseOrder(e) {
            e.preventDefault();
            
            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
            var asStatusId = document.getElementById(_clientcmbStatus).value;            
            if (asStatusId == "1")
                return false;      

            if (window.confirm('Are you sure you want to approve this PO?')) {
                $.ajax({
                    type: "POST",
                    data: '{"asPOId":"' + dataItem.POId + '","asSchoolId":"' + _schoolId + '","asUserId":"' + $("#" + _loginUserId).val() + '" }',
                    url: "PurchaseOrderApproverUI.aspx/ApprovePurchaseOrder",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        $("#" + _lblMessage).text("PO details approved successfully!!!");
                        ReadGrid();
                    },
                    error: function () {
                    }
                });
            }
        }

        function OnDataBound(e) {            
            var grid = this;
            grid.tbody.find('>tr').each(function () {
                var dataItem = grid.dataItem(this);

                var currenRow = grid.table.find("tr[data-uid='" + dataItem.uid + "']");
                var ApproveButton = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-Delete");
                var asStatusId = document.getElementById(_clientcmbStatus).value;

                if (asStatusId == "1") {                  
                    ApproveButton.html("Approved");
                }
                else {                   
                    ApproveButton.html("Approve");
                }
                
            })
        }

        function ReadGrid() {            
            $("#divPODetails").data("kendoGrid").dataSource.read();            
        }

        function SetAttributes() {
            $("#" + _lblMessage).text("");
            ReadGrid();            
            var grid = $("#divPODetails").data("kendoGrid");
            grid.dataSource.page(1);
        }

    </script>
</asp:Content>