<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="OnlineExamProgressReportUI.aspx.cs" Inherits="OnlineExamProgressReportUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <style type="text/css">
            .clsLabelHeader
            {
                font-weight: bold;
            }
            
            .clsSchoolName
            {
                color: Black;
                font-weight: bold;
                background-color: #AAC6FF;
                font: Arial;
                font-size: 13px;
            }
            
            .clsOrgName
            {
                border: 1px solid #AAC6FF;
                color: Black;
                font-weight: bold;
                background-color: #E4F1F1;
                font: Arial;
                font-size: 18px;
            }
            
            
            .clsHeader1
            {
                color: Black;
                font-weight: bold;
                background-color: #AAC6FF;
                font: Arial;
                font-size: 15px;
            }
            
            .clsStudentInfo
            {
                border: 1px solid #AAC6FF;
                font: Arial;
            }
            
            .clsStudentInfoData
            {
                border: 1px solid #AAC6FF;
                color: Maroon;
                font-weight: bold;
                font: Arial;
            }
            
            .clsStudentMarks
            {
                border: 1px solid #AAC6FF;
                color: Black;
                font-weight: bold;
                background-color: #AAC6FF;
                padding-left: 10px;
                padding-right: 10px;
                font: Arial;
            }
            
            
            .clsExamsAndSubjects
            {
                border: 1px solid #CCDDFF;
                color: Black;
                font-weight: bold;
                background-color: #CCDDFF;
                padding-left: 10px;
                padding-right: 10px;
                font: Arial;
            }
            
            .clsStudentActualMarks
            {
                border: 1px solid #AAC6FF;
                color: Black;
                font-weight: 500;
                background-color: #E4F1F1;
                padding-left: 10px;
                padding-right: 10px;
                font: Arial;
            }
            
            .clsMethodName
            {
                background-color: #D5E2FF;
                font: 0.75em Arial;
                font-size: 9pt;
            }
            
            .clsTotal
            {
                font-weight: bold;
                background-color: #84ACFF;
                font: Arial;
                font-size: 13px;
                color: Black;
            }
        </style>
        <table width="98%">
            <tr id="trFilter" runat="server">
                <td>
                    <table align="center">
                        <tr>
                            <td class="clsBorderLight" style="width: 100px">
                                <span class="clsLabel">Class Teacher : </span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbClass" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                    OnSelectedIndexChanged="cmbClass_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="clsBorderLight" style="width: 100px">
                                <span class="clsLabel">Student : </span>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbStudent" runat="server" CssClass="ExLrgCombo">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" CssClass="ClsBtn" />
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
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="50%" class="LblNoRecord" id="trMessage" runat="server" visible="false">
                                <tr>
                                    <td align="left">
                                        No any exam is published.
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="80%" id="tblMainTable" runat="server">
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hidUserHasFullAccess" runat="server" Value="N" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
