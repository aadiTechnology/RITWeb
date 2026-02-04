<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="AssemblyDetailsUI.aspx.cs" Inherits="AssemblyDetailsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <style type="text/css">
        .ClsAssemblyTopHeader
        {
            font-weight: 700;
            font-size: 9pt;
            color: White;
            text-decoration: none;
            padding-right: 5px;
            height: 20px;
            background-color: #000071;
            border-bottom-style: solid;
            border-bottom-width: 1px;
        }
        
        .ClsAssemblyHeader
        {
            font-weight: 700;
            font-size: 9pt;
            color: White;
            text-decoration: none;
            padding-right: 5px;
            height: 20px;
            background-color: #006697;
            border-bottom-style: solid;
            border-bottom-width: 1px;
        }
        
        .ClsAssemblyCell
        {
            background-color: #E1EAFF;
            font-family: Arial;
            font-size: 9pt;
            padding-right: 5px;
            border-color: White;
        }
    </style>
    <div class="MainBodyDiv">
        <table width="98%" align="center">
            <tr>
                <td align="left">
                    <asp:ValidationSummary ID="valSum" runat="server" CssClass="ErrMsg" />
                    <asp:CustomValidator ID="cstValAnswerLength" runat="server" ClientValidationFunction="ValidateAssembly"
                        SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                    <asp:CustomValidator ID="cstValAssemblyAnswerLength" runat="server" ClientValidationFunction="ValidateAssemblylength"
                        SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>                    
                </td>
            </tr>
            <tr>
                <td id="tdMessage" runat="server">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
            </tr>            
            <tr align="center">
                <td>
                    <table width="80%">
                        <tr align="center">
                            <td class="ClsBorderlight">
                                <asp:Label ID="lblDate" runat="server" CssClass="clsLabel" Text="Date"></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="right" style="text-align: left;">
                                <asp:TextBox ID="txtDate" CssClass="SmlTxtBox" runat="server" ReadOnly="True"></asp:TextBox>
                                <rjs:PopCalendar ID="cal_AssemblyDate" runat="server" Control="txtDate" Format="dd MMM yyyy"
                                    Culture="en" ShowWeekend="True" AutoPostBack="False" To-Today="true" />
                            </td>
                            <td style="width: 69%;">
                            </td>
                            <td class="ClsBorderlight">
                                <asp:Label ID="lblDays" runat="server" CssClass="ClsLabel" Text="Day"></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td>
                                <asp:Label ID="lblDay" CssClass="clsLabel" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table id="tblQuestions" runat="server" cellspacing="1" width="80%">                      
                    </table>
                </td>
            </tr>            
            <tr>
                <td align="center">
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false"
                        PostBackUrl="~/RITeSchool/Admin/AssemblyListUI.aspx" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click"
                        UseSubmitBehavior="false" />
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn" UseSubmitBehavior="false"
                        Enabled="false" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnPublish" runat="server" Text="Publish" CssClass="ClsBtnMid" UseSubmitBehavior="false"
                        OnClick="btnPublish_Click" />
                </td>
            </tr>
            <asp:HiddenField ID="hidAssemblyId" runat="server" Value="0" />
            <asp:HiddenField ID="hidFileUpload" runat="server" Value="null" />
            <asp:HiddenField ID="hidPhotoFilePath" runat="server" Value="" />
        </table>
        <script type="text/javascript" language="javascript">

            _clienttxtDate = "<%=this.txtDate.ClientID %>"

            function ChangeDate() {
                var date = convertdate(document.getElementById(_clienttxtDate).value)
                var weekday = new Array(7);
                weekday[0] = "Sunday";
                weekday[1] = "Monday";
                weekday[2] = "Tuesday";
                weekday[3] = "Wednesday";
                weekday[4] = "Thursday";
                weekday[5] = "Friday";
                weekday[6] = "Saturday";
                var da = new Date(date);
                var dat = da.getDay();
                var day = weekday[dat];
                $('#<%=lblDay.ClientID%>').html(day);
            }

            function ConfirmSubmit() {
                return confirm('This action will submit only saved details. Do you want to continue?');
            }

            function ValidateAssembly(oSrc, args) {
                var sRows = ""
                var Assembly = []
                Assembly = document.getElementsByTagName("input");
                for (var k = 0; k < Assembly.length; k++) {
                    var Answers = Assembly[k]
                    if (Answers.id.match("txt_") != null) {
                        if (Answers.value.trim() == "") {
                            if (sRows.match((k + 1)) == null)
                                sRows = sRows + ", " + (k + 1)
                        }
                    }
                }
                if (sRows != "") {
                    sRows = sRows.substring(1)
                    oSrc.errormessage = "Assembly status should not be blank";
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }

            function ValidateAssemblylength(oSrc, args) {
                var sRows = ""
                var Assembly = []
                Assembly = document.getElementsByTagName("input");
                for (var k = 0; k < Assembly.length; k++) {
                    var Answers = Assembly[k]
                    if (Answers.id.match("txt_") != null) {
                        if (Answers.value.trim().length > 500) {
                            if (sRows.match((k + 1)) == null)
                                sRows = sRows + ", " + (k + 1)
                        }
                    }
                }
                if (sRows.length != "") {
                    sRows = sRows.substring(1)
                    oSrc.errormessage = "Assembly status length should not be greater than 500 characters.";
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }

            function OpenFile(fileName) {
                window.open('../DOWNLOADS/Assembly/' + fileName);
            }          
        </script>
    </div>
</asp:Content>
