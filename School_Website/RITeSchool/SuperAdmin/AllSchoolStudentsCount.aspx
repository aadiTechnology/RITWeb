<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="AllSchoolStudentsCount.aspx.cs" Inherits="AllSchoolStudentsCount" ViewStateMode="Disabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <style type="text/css">
        .width-40
        {
            width: 40%;
        }
         .width-80
        {
            width: 80%;
        }
        .custom-width
        {
            width: 225px;
        }
        .padding-top-30
        {
            padding-top: 30px;
        }
        .float-initial
        {
            float: inherit;
        }
        .height-16
        {
            height:16px;
        }
    </style>
    <table align="center" class="width-40">
        <tr align="center">
            <td class="ClsBorderlight custom-width">
                <asp:Label ID="lblAcademicYear" runat="server" CssClass="ClsLabel height-16" Text="Academic Year"></asp:Label>
                <span class="ClsLabel colonPadding">:</span>
            </td>
            <td align="left">
                <asp:DropDownList ID="cmbAcademicYear" runat="server" CssClass="MidCombo" AutoPostBack="true"
                    CausesValidation="true" ViewStateMode="Enabled">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table align="center" class="width-80 padding-top-30">
        <tr>
            <td align="center" colspan="2">
                <asp:UpdatePanel ID="upnlStudentCount" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="divStudentsCount" data-role="grid" class="float-initial">
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        var _academicYear = $("#<%=this.cmbAcademicYear.ClientID %> option:selected").text();

        $(function () {
            $("#divStudentsCount").kendoGrid({
                columns: [
                        { field: "SchoolName", title: "School Name", width: "40%"},
                        { field: "Total", title: "Total Students", width: "20%", attributes: { style: "text-align:center;" }, headerAttributes: { style: "text-align:center" }, filterable: false },
                        { field: "Girls", title: "Girls", width: "20%", attributes: { style: "text-align:center;" }, headerAttributes: { style: "text-align:center" }, filterable: false },
                        { field: "Boys", title: "Boys", width: "20%", attributes: { style: "text-align:center;" }, headerAttributes: { style: "text-align:center" }, filterable: false },
                        ],
                pageable: true,
                filterable: true,
                sortable: true,
                dataBound: function (e) {
                },
                dataSource: {
                    pageSize: 20,
                    schema: {
                        data: "d.Data",
                        total: "d.Total"
                    },
                    batch: true,
                    transport: {
                        read: {
                            url: "AllSchoolStudentsCount.aspx/GetStudentsCount",
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
                                data = $.extend({ asAcademicYear: _academicYear }, data);
                                return JSON.stringify(data);
                            }
                        }
                    }
                }
            });
        });
    </script>
</asp:Content>