<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="AssemblyListUI.aspx.cs" Inherits="AssemblyListUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="center">
                <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"
                    ForeColor="Blue" Style="font-weight: bold"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <div id="divAssembly" data-role="grid" style="width: 40%; float: inherit; text-align: center;
                    margin-top: 10px;">
                </div>
                <asp:HiddenField ID="hidSchoolId" runat="server" />
                <asp:HiddenField ID="hidAcademicYearId" runat="server" />
                <asp:HiddenField ID="hidAssemblyId" runat="server" Value="0" />
                <asp:HiddenField ID="hidUserId" runat="server" />
                <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="ClsBtn" />
            </td>
        </tr>
    </table>
    <style type="text/css">  
     .k-header .k-link{
     text-align  : center;
    }                
    </style>
    <script type="text/javascript">


        var _schoolId = $("#" + "<%=hidSchoolId.ClientID %>").val();
        var _academicYearId = $("#" + "<%=hidAcademicYearId.ClientID %>").val();
        var _AssemblyId = "<%=this.hidAssemblyId.ClientID %>"
        var _loginUserId = "<%=this.hidUserId.ClientID %>";
        var _lblMessage = "<%=this.lblMessage.ClientID %>";

        $(function () {
            $("#divAssembly").kendoGrid({
                columns: [
                        { field: "Date", title: "Assembly Date", attributes: { style: "text-align:center;" }, format: "{0:dd-MMM-yyyy}" },
                         {
                             command: [                            
                            { text: "Edit", name: "Edit", click: GetAssembly },
                            { text: "Delete", name: "Delete", click: DeleteAssembly },
                            ], title: "Action", width: "175px"
                         }
                        ],
                pageable: true,
                filterable: true,
                sortable: true,
                editable: false,
                selectable: "single row",
                dataBound: OnDataBound,
                dataSource: {
                    pageSize: 20,
                    schema: {
                        data: "d.Data",
                        total: "d.Total",
                        model: {
                            fields: {
                                "Id": { editable: false, type: "number" },
                                "Date": { editable: false, type: "date" }
                            }
                        }
                    },
                    batch: true,
                    transport: {
                        read: {
                            url: "AssemblyListUI.aspx/GetAllAssemblyList",
                            contentType: "application/json; charset=utf-8",
                            type: "POST",
                            complete: function () {
                            }
                        },

                        parameterMap: function (data, operation) {
                            if (data.models) {
                                return JSON.stringify({ products: data.models });
                            } else if (operation == "read") {
                                data = $.extend({ sort: null, filter: null }, data);
                                data = $.extend({ aiSchoolId: _schoolId, aiAcademicYearId: _academicYearId }, data);
                                return JSON.stringify(data);
                            }
                        }
                    }
                }
            });
        });

        function OnDataBound(e) {
            var grid = this;
            grid.tbody.find('>tr').each(function () {
                var dataItem = grid.dataItem(this);
                var EditButton = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-Edit");
                var DeleteButton = grid.tbody.find("tr[data-uid='" + dataItem.uid + "']").find(".k-grid-Delete");

                if (dataItem.IsSubmit && dataItem.IsPublish) {
                    DeleteButton.prop("disabled", true).addClass("k-state-disabled");
                    EditButton.text("View")
                }
                else {
                    if (dataItem.IsSubmit) {
                        EditButton.text("View")
                        DeleteButton.prop("disabled", true).addClass("k-state-disabled");
                    }
                    else {
                        EditButton.text("Edit")
                        EditButton.prop("enable", true).removeClass("k-state-disabled");
                        DeleteButton.prop("enable", true).removeClass("k-state-disabled");
                    }
                }
            })
        }

        function DeleteAssembly(e) {
            e.preventDefault();
            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
            if (window.confirm('Are you sure you want to delete this record?')) {
                $.ajax({
                    type: "POST",
                    data: '{"aiAssemblyId":"' + dataItem.Id + '","aiSchoolId":"' + _schoolId + '","aiAcademicYearId":"' + _academicYearId + '","aiUserId":"' + $("#" + _loginUserId).val() + '" }',
                    url: "AssemblyListUI.aspx/DeleteAssembly",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        $("#" + _lblMessage).text("Assembly details deleted successfully!!!");
                        ReadGrid();
                    },
                    error: function () {
                    }
                });
            }
        }

        function GetAssembly(e) {
            e.preventDefault();
            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
            var sDate = dataItem.Date.format("dd-MMM-yyyy");
            $.ajax({
                type: "POST",
                data: '{"aiAssemblyId":"' + dataItem.Id + '","adtDate":"' + sDate + '" }',
                url: "AssemblyListUI.aspx/GetQuerystring",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    window.open(msg.d, "_self");
                },
                error: function (msg) {
                }
            });
        }


        function ReadGrid() {
            $("#divAssembly").data("kendoGrid").dataSource.read();
        }

        function OpenAssembly() {
            _clienthidQueryString = "<%=this.hidQueryString.ClientID %>";
            var sEncryptedString = document.getElementById(_clienthidQueryString).value;
            window.open('AssemblyDetailsUI.aspx?' + sEncryptedString, '_self', 'scrollbars=yes,resizable=no,top=0,left=0')
            return false;
        }
    </script>
</asp:Content>
