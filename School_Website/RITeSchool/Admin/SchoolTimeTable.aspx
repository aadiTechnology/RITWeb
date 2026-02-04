<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="SchoolTimeTable.aspx.cs" Inherits="SchoolTimeTable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel4">
        <ContentTemplate>
            <table width="90%" align="center">
                <tr>
                    <td align="center" colspan="2">
                        <div id="divLink" runat="server" style="width: 120px; text-align: center" class="ClsHilightBGTT"
                            visible="false">
                            <asp:LinkButton runat="server" ID="hlnkTTSchedule" Text="Lecture Timings" CssClass="ClsLogoutNew"
                                Visible="false"></asp:LinkButton>
                        </div>
                    </td>
                </tr>
                <tr id="trClassTeacher" runat="server">
                    <td align="left" width="50%" class="ClsBorderlight">
                        <asp:RadioButton ID="optTeacher" runat="server" GroupName="TimeTable" OnCheckedChanged="optTeacher_CheckedChanged"
                            AutoPostBack="true" />
                            <span class="ClsLabelNrml">
                            <asp:Label ID="lblteachersTimeTableText" runat="server" Text="<%$ Resources:LocalizedResources, TeachersTimetable %>"></asp:Label>
                            </span>
                    </td>                    
                    <td align="left" width="50%" class="ClsBorderlight">
                        <asp:RadioButton ID="optClass" runat="server" GroupName="TimeTable" AutoPostBack="true"
                            OnCheckedChanged="optClass_CheckedChanged" />
                            <span class="ClsLabelNrml">
                            <asp:Label ID="lblClassesTimetableText" runat="server" Text="<%$ Resources:LocalizedResources, ClassesTimetable %>"></asp:Label>
                            </span>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <div id="divSTT" runat="server">
                            <asp:Panel ID="pnlContainer" runat="server" Visible="true" Style="width: 850px;">
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <div style="width: 100%" id="divErr" runat="server">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:HiddenField ID="hidIs_TeachersTT" runat="server" Value="Y" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="optTeacher" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="optClass" EventName="CheckedChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
