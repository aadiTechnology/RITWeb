<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentRecordUI.aspx.cs" Inherits="StudentRecordUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="98%">
            <tr>
                <td style="height: 10px;">
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 30px" valign="bottom">
                  <asp:Label ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False"
                        Text="All fields are mandatory."></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ValidationSummary ID="ValSum" runat="server" />
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Display="None"
                        ClientValidationFunction="ValidateFields"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="70%" style="font-family: Arial">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Blue"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="100%">
                                    <tr>
                                        <td align="right">
                                            <span class="clsLabel" style="float: inherit">Date : </span>
                                        </td>
                                        <td align="left" style="width: 150px;">
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="SmlTxtBox" MaxLength="11" ReadOnly="true"></asp:TextBox>
                                            <rjs:PopCalendar ID="cDate" runat="server" Control="txtDate" Culture="en" Format="dd MMM yyyy"
                                                ShowErrorMessage="false" To-Today="true" ShowWeekend="True" InvalidDateMessage="Invalid date format." />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <table style="width:99%">
                                    <tr>
                                        <td align="center">
                                            <span style="font-weight: bold; font-size: 25px;"><u>Student's Record Form</u></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="text-align: justify; font-weight: bold; color: Red;" class="ClsLabel">
                                            <span>The following information is for professional use and will be handled confidentially.
                                                This information will assist the counsellor for the child's evaluation.</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="text-align: justify; font-weight: bold; color: Red;" class="ClsLabel">
                                            <span>Please complete the following questions as fully and accurately as possible. If
                                                you are unable to complete a question you may consult other subject teachers for
                                                the better understanding of the child.</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        
                        <tr>
                            <td align="center">
                      <table style="width: 99%">
                                    <tr style="background-color: #006179; color: White; border-style: solid; border-color: #006179;
                                        border-width: thin;">
                                        <td align="center">
                                            <span style="font-weight: bold; line-height: 1.5px;">General Information</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <table width="100%" style="border-style:solid;border-color:#006179;border-width:2px;" >
                                                <tr class="Height10">
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 200px">
                                                        <span class="ClsLabel">Name of the student : </span>
                                                    </td>
                                                    <td>
                                                        <span id="spnStudentName" runat="server" class="ClsLabel"></span>
                                                    </td>
                                                    <td align="left" style="width: 200px">
                                                        <span class="ClsLabel">Date of Birth : </span>
                                                    </td>
                                                    <td>
                                                        <span id="spnDOB" runat="server" class="ClsLabel"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" class="height20">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <span style="font-weight: bold;" class="clsLabel">Family Details</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 200px">
                                                        <span class="ClsLabel">Mother Name : </span>
                                                    </td>
                                                    <td>
                                                        <span id="spnMotherName" runat="server" class="ClsLabel"></span>
                                                    </td>
                                                    <td align="left" style="width: 200px">
                                                        <span class="ClsLabel">Mother Occupation : </span>
                                                    </td>
                                                    <td>
                                                        <span id="spnMotherOccuption" runat="server" class="ClsLabel"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 200px">
                                                        <span class="ClsLabel">Father Name : </span>
                                                    </td>
                                                    <td>
                                                        <span id="spnFatherName" runat="server" class="ClsLabel"></span>
                                                    </td>
                                                    <td align="left" style="width: 200px">
                                                        <span class="ClsLabel">Father Occupation : </span>
                                                    </td>
                                                    <td>
                                                        <span id="spnFatherOccuption" runat="server" class="ClsLabel"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" class="height20">
                                                    </td>
                                                </tr>
                                                <tr id="trSiblingHeader" runat="server">
                                                    <td colspan="4">
                                                        <span style="font-weight: bold;" class="clsLabel">Please list all siblings</span>
                                                    </td>
                                                </tr>
                                                <tr id="trSibling" runat="server">
                                                    <td align="left" colspan="4">
                                                        <table id="tblSibling" runat="server" style="border-style: ridge; border-width: thin;
                                                            border-color: #006179; margin-left: 5px;">
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr class="Height10">
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>     
                            </td>
                        </tr>
                        <tr class="height20">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="1">
                                <div id="div1" runat="server">
                                </div>
                                <table id="tblSections" runat="server" style="width: 100%;">
                                </table>
                            </td>
                        </tr>
                        <tr class="height20">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="padding-left: 5px;">
                                <span style="font-weight: bold;" class="clsLabel"><u>Comment(s)</u></span>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="1">
                                <table id="tblComments" runat="server" style="width: 99%;">
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1" align="center">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click"
                                    disable-page="true" UseSubmitBehavior="false" />
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn" CausesValidation="false"
                                    Enabled="false" OnClick="btnSubmit_Click" />
                                <asp:Button ID="btnAddComment" runat="server" Text="Add Comment" CssClass="ClsBtn"
                                    Enabled="false" CausesValidation="false" />
                                <asp:Button ID="btnSubmitComment" runat="server" Text="Submit Comment" CssClass="ClsBtn"
                                    Enabled="false" CausesValidation="false" OnClick="btnSubmitComment_Click" />
                                <asp:Button ID="btnRead" runat="server" Text="Mark as Read" CssClass="ClsBtn" Visible="false"
                                    CausesValidation="false" OnClick="btnRead_Click" />
                                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false" />
                                <asp:HiddenField ID="hidStudentId" runat="server" Value="0" />
                                <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
                                <asp:HiddenField ID="hidIsReadMode" runat="server" Value="1" />
                                <asp:HiddenField ID="hidIsPrincipal" runat="server" Value="0" />
                                <asp:HiddenField ID="hidIsCounsellor" runat="server" Value="0" />
                                <asp:HiddenField ID="hidFilter" runat="server" Value="" />
                                <asp:HiddenField ID="hidFilterIsRiseAndShinde" runat="server" Value="" />
                                <asp:HiddenField ID="hidIsSubjectTeacher" runat="server" Value="0" />
                                <asp:HiddenField ID="hidIsClassTeacher" runat="server" Value="0" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script>
        function ValidateFields(oSrc, args) {
            var cnt = false
            $('.LrgTxtBox').each(function () {
                if ($(this).val().trim() != "") {
                    cnt = true;
                }
            });

            if (cnt == false) {
                oSrc.errormessage = "Please enter details for at least one field.";
                args.IsValid = false
                return true;
            }

            args.IsValid = true
            return false
        }

        function ShowPopup(studid, commentid, isReadMode, isPrincipal, isCounsellor, isClassTeacher, stdDivId, showOnlySavedRecord) {
            recordFilter = $('#' + '<%=this.hidFilter.ClientID %>').val()

            var dt = '{"asStudId":"' + studid + '","asCommentId":"' + commentid + '","asIsReadMode":"' + isReadMode + '","asIsPrincipal":"' + isPrincipal + '","asIsCounsellor":"' + isCounsellor + '", "asIsClassTeacher":"' + isClassTeacher +
                     '","asStdDivId":"' + stdDivId + '","asFilter":"' + recordFilter + '","asShowOnlySavedRecord":"' + showOnlySavedRecord + '"}'

            $.ajax({ type: "POST", data: dt, url: "StudentRecordUI.aspx/GetQueryString", contentType: "application/json; charset=utf-8", dataType: "json", success: function (msg) {
                var data = msg.d
                window.open('StudentRecordCommentPopup.aspx?' + data, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=500')
            }, error: function (msg) { }
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
