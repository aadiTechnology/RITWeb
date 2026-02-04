<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="RITeSchoolUsageUI.aspx.cs" Inherits="RITeSchoolUsageUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <style type="text/css">
        .ClsHeadings
        {
            font-size: 12pt;
            font-family: Arial;
            font-weight: bold;
        }
        
        .ClsTitle
        {
            font-size: 14pt;
            font-family: Arial;
            font-weight: bold;
        }
    </style>
    <div class="panel-body col-sm-10">
        <div class="col-sm-1">
        </div>
        <div class="col-sm-10">
            <div class="col-sm-12">
                <span class="ClsTitle">RITeSchool Usage Details</span>
            </div>
            <div class="row">
                <div class="col-sm-12 col-lg-12 text-left">
                    <div class="col-sm-6 col-lg-6 text-left">
                        <span class="ClsHeadings">Verification Dates</span>
                    </div>
                    <div class="col-sm-2 col-lg-2 col-md-2" style="float: right; padding-bottom:10px;">
                        <input type="button" value="Generate Report" class="ClsBtn" style="width: 150px" onclick="GenerateReport(); return false;" />
                    </div>
                </div>
              <%--  <div class="col-sm-12" style="height:10px;">
                </div>--%>
                <div class="widget-body col-sm-12 form-group">
                    <div id="divDates" data-role="grid">
                    </div>
                </div>
            </div>
            <div id="divTotalDetails" style="display: none;" class="row">
                <div class="col-sm-12 text-left">
                    <span id="detailMessage" class="ClsHeadings">RiteSchool Usage Details</span>
                </div>
                <div class="widget-body col-sm-12 form-group">
                    <div id="divDetails" data-role="grid">
                    </div>
                </div>
            </div>
            <br />
            <div class="col-sm-12">
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClientClick="NavigatetoControlPanel(); return false;"
                    CssClass="ClsBtn" />
                <asp:Button ID="btnSendMail" runat="server" Text="Send Mail" OnClientClick="SendMail(); return false;"
                    CssClass="ClsBtn" />
                <input type="hidden" id="hidDate" value="" />
            </div>
            <script>
                $(function () {
                    $("#divDates").kendoGrid({
                        columns: [
                      { field: "Date", title: "Date", format: "{0:dd-MMM-yyyy}" },
                      {
                          command: [
                            {
                                text: "Details",
                                name: "Details",
                                click: showDetails
                            },
                            ], title: "Action",
                          width: "30%"
                      }
                     ],
                        pageable: {
                            info: true,
                            buttonCount: 5
                        },
                        filterable: false,
                        sortable: {
                            mode: "single",
                            allowUnsort: false
                        },
                        editable: false,
                        selectable: "single row",
                        dataBound: function (e) {
                            setToolTip();
                        },
                        dataSource: {
                            serverPaging: true,
                            serverSorting: true,
                            serverFiltering: true,
                            pageSize: 5,
                            schema: {
                                data: "d.Data",
                                total: "d.Total",
                                model: {
                                    fields: {
                                        Date: { type: "date" }
                                    }
                                }
                            },
                            batch: true,
                            transport: {
                                read: {
                                    url: "RITeSchoolUsageUI.aspx/GetAllExecutionDates",
                                    contentType: "application/json; charset=utf-8",
                                    type: "POST",
                                    complete: function () {
                                        showHidePager();
                                    }
                                },
                                parameterMap: function (data, operation) {
                                    if (data.models) {
                                        return JSON.stringify({ products: data.models });
                                    } else if (operation == "read") {
                                        data = $.extend({ sort: null, filter: null }, data);
                                        return JSON.stringify(data);
                                    }
                                }
                            }
                        }
                    });
                });

                // This method is used to read date and pass it to function to fill up usage details.
                function showDetails(e) {
                    e.preventDefault();
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    FillDetails(dataItem.Date.format("dd-MMM-yyyy"));
                }

                // This method is used to fill usage details according to selected date.
                function FillDetails(executionDate) {
                    SetFieldState(executionDate);
                    $("#divDetails").show()
                    $("#divDetails").kendoGrid({
                        columns: [
                          {
                              field: "QueryName",
                              title: "Feature"
                          },
                          {
                              field: "Legend",
                              title: "Status",
                              width: "30%",
                              sortable: false
                          }
                     ],
                        pageable: {
                            info: true,
                            buttonCount: 5
                        },
                        filterable: false,
                        sortable: {
                            mode: "single",
                            allowUnsort: false
                        },
                        editable: false,
                        selectable: "single row",
                        dataSource: {
                            serverPaging: true,
                            serverSorting: true,
                            serverFiltering: true,
                            pageSize: 10,
                            schema: {
                                data: "d.Data",
                                total: "d.Total",
                                model: {
                                    fields: {
                                        Date: { type: "date" }
                                    }
                                }
                            },
                            batch: true,
                            transport: {
                                read: {
                                    url: "RITeSchoolUsageUI.aspx/GetAllUsageDetails",
                                    contentType: "application/json; charset=utf-8",
                                    type: "POST",
                                    complete: function () {
                                        showHidePager();
                                    }
                                },
                                parameterMap: function (data, operation) {
                                    if (data.models) {
                                        return JSON.stringify({ products: data.models });
                                    } else if (operation == "read") {
                                        data = $.extend({ sort: null, filter: null }, data);
                                        data = $.extend({ asDate: executionDate }, data);
                                        return JSON.stringify(data);
                                    }
                                }
                            }
                        }
                    });
                }

                function SetFieldState(executionDate) {
                    $("#detailMessage").text("RITeSchool Usage Details (" + executionDate + ")")
                    $("#btnSendMail").show();
                    $("#hidDate").val(executionDate);
                    $("#divTotalDetails").show();

                    //                    if (!($('#btnSendMail').data('kendoTooltip')))
                    //                        $('#btnSendMail').kendoTooltip({ content: "Send Usage Mail" });
                }

                // This method is used to send mail.
                function SendMail() {
                    var dt = $("#hidDate").val();
                    DisableButtons(true);
                    $.ajax({
                        type: "POST",
                        data: '{"asDate": "' + dt + '"}',
                        url: "RITeSchoolUsageUI.aspx/SendRITUsageMail",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            alert("RIT Usage mail sent successfully !!!");
                            DisableButtons(false);
                        },
                        error: function (msg) {
                            alert("Failed to send mail.");
                            DisableButtons(false);
                        }
                    });
                }


                function GenerateReport() {
                    DisableButtons(true);
                    $.ajax({
                        type: "POST",
                       // data: '{"asDate": "' + dt + '"}',
                        url: "RITeSchoolUsageUI.aspx/GenerateReport",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            alert("RIT Usage report is generated successfully !!!");
                            DisableButtons(false);

                            var grid = $("#divDates").data("kendoGrid");
                            grid.dataSource.read()

                            $("#divDetails").hide()

                            $("#detailMessage").text("")
                            $("#hidDate").val("");
                            $("#divTotalDetails").hide()

                        },
                        error: function (msg) {
                            alert("Failed to generate report.");
                            DisableButtons(false);
                        }
                    });
                }


                $(document).ready(function () {
                    $("#btnSendMail").hide();
                    fnover("btnBack", this);
                    //                    if (!($('#btnBack').data('kendoTooltip')))
                    //                        $('#btnBack').kendoTooltip({ content: "Back" });                  
                });

                // this emthod is used to disable buttons.
                function DisableButtons(val) {
                    if (val == true) {
                        $("#btnBack").attr("disabled", "disabled");
                        $("#btnSendMail").attr("disabled", "disabled");
                        $(".k-grid-Details").attr("disabled", "disabled");
                    }
                    else {
                        $("#btnBack").removeAttr("disabled");
                        $("#btnSendMail").removeAttr("disabled");
                        $(".k-grid-Details").removeAttr("disabled", "disabled");
                    }
                }

                // This method is used to navigate to dashboard.
                function NavigatetoControlPanel() {
                    window.open("ScreensUI.aspx", "_self");
                }

                // This function is used to set tooltip to kendoButtons.
                function setToolTip() {
                    if (!($('.k-grid-Details').data('kendoTooltip'))) {
                        //$('.k-grid-Details').kendoTooltip({ content: "Details" });

                        //$('.k-grid-Details').addClass("ClsBtn");
                        //$('.k-grid-Details').removeClass("k-button");
                    }
                }

                function showHidePager() {
                    $(".k-grid-pager").parent().each(function (e) {
                        var grid = $(this).data().kendoGrid;
                        if (grid.pager.dataSource._total / grid.pager.dataSource._pageSize <= 1)
                            $(this).children(".k-grid-pager").hide();
                        else
                            $(this).children(".k-grid-pager").show();
                    });
                }

            </script>
        </div>
        <div class="col-sm-1">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
