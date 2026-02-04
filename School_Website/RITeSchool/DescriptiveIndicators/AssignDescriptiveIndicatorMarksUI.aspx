<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="AssignDescriptiveIndicatorMarksUI.aspx.cs" Inherits="AssignDescriptiveIndicatorMarksUI" ViewStateMode="Disabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <style type="text/css">
        .ProgressReportHeader
        {
            font-weight: 700;
            font-size: 12pt;
            color: White;
            text-decoration: none;
            height: 20px;
            background-color: #8080C0;
            border-style:solid;
            border-width:1px;
            border-color:Navy;
        }
        
        .ProgressReportRow
        {
            font-weight: 700;
            font-size: 11pt;
            color: #333;
            text-decoration: none;
            height: 20px;
            background-color: skyblue;
        }
        
        .ProgressReportParameter
        {   
            font-size: 11pt;
            color: #333;
            text-decoration: none;
            height: 20px;
            background-color: #c8dffe;
        }
    
        .StudentDetailsHeader
        {
            font-weight: 700;
            font-size: 12pt;
            color: White;
            text-decoration: none;
            height: 20px;
            padding-left: 5px;
            background-color: #3D7C7C;
            border-style:solid;
            border-width:1px;
            border-color:Navy;
        }
    </style>
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="98%">
            <tr class="Height10">
                <td>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:ValidationSummary ID="valsum" runat="server" ViewStateMode="Enabled" />
                    <asp:CustomValidator ID="custValMarks" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidateMarksRange" ViewStateMode="Enabled"></asp:CustomValidator>
                    <asp:CustomValidator ID="custValGrade" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidateGrades" ViewStateMode="Enabled"></asp:CustomValidator>                    
                    <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidateObservation" ViewStateMode="Enabled"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td class="clsBorderLight" width="100px">
                                <span class="clsLabel">Term : </span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbTerm" runat="server" CssClass="MidCombo" AutoPostBack="True" ViewStateMode="Enabled"
                                    OnSelectedIndexChanged="cmbTerm_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 10px">
                            </td>
                            <td class="clsBorderLight" width="100px">
                                <span class="clsLabel">Section : </span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbSection" runat="server" CssClass="ExLrgCombo" Width="350px" ViewStateMode="Enabled"
                                    AutoPostBack="True" OnSelectedIndexChanged="cmbSection_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:HiddenField ID="hidStandardId" runat="server" Value="0" ViewStateMode="Enabled" />
                                <asp:HiddenField ID="hidYearwiseStudentId" runat="server" Value="0" ViewStateMode="Enabled" />
                                <asp:HiddenField ID="hidStdDivId" runat="server" Value="0" ViewStateMode="Enabled" />
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
                <td align="center">
                    <table style="width: 75%">
                        <tr>
                            <td align="center" colspan="6">
                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Font-Bold="true" EnableViewState="false"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbTerm" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbSection" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" align="center" class="StudentDetailsHeader">
                                <span>Descriptive Indicators</span>
                            </td>
                        </tr>
                        <tr class="height20">
                            <td colspan="6">
                            </td>
                        </tr>
                        <tr>
                            <td class="clsBorderLight" width="100px">
                                <span class="clsLabel">Roll No. :</span>
                            </td>
                            <td class="ClsBGWhite ClsBorderlight ClsHilightTextB" width="100px">
                                <asp:Label ID="lblRollNo" runat="server" CssClass="clsLabel" ViewStateMode="Enabled"></asp:Label>
                            </td>
                            <td class="clsBorderLight" width="100px">
                                <span class="clsLabel">Name :</span>
                            </td>
                            <td class="ClsBGWhite ClsBorderlight ClsHilightTextB">
                                <asp:Label ID="lblName" runat="server" CssClass="clsLabel" ViewStateMode="Enabled"></asp:Label>
                            </td>
                            <td class="clsBorderLight" width="100px">
                                <span class="clsLabel">Class :</span>
                            </td>
                            <td class="ClsBGWhite ClsBorderlight ClsHilightTextB" width="200px">
                                <asp:Label ID="lblClass" runat="server" CssClass="clsLabel" ViewStateMode="Enabled"></asp:Label>
                            </td>
                        </tr>
                        <tr class="height20">
                            <td colspan="6">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="tblMain" runat="server" style="width: 75%">
                            </table>
                            <asp:HiddenField ID="hidSkillIds" runat="server" Value="" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbTerm" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbSection" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr class="Height10">
                <td>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" ViewStateMode="Enabled"
                        CausesValidation="false" onclick="btnBack_Click" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" UseSubmitBehavior="False" ViewStateMode="Enabled"
                        OnClick="btnSave_Click" />
                    <asp:HiddenField ID="hidMaxValue" runat="server" Value="0" />
                </td>
            </tr>
        </table>
        <script type="text/javascript" language="javascript">
            _clienthidMaxValue = "<%=this.hidMaxValue.ClientID %>"



            function ValidateMarksRange(oSrc, args) {
                var foundMax = false
                var foundBlank = false
                var maxCount = $('#' + _clienthidMaxValue).val()
                var marks = document.getElementsByTagName("input");
                
                for (var k = 0; k < marks.length; k++) {
                    var mark = marks[k]
                    if (mark.value.trim() == "" && mark.type == "text") {
                        mark.style.backgroundColor = "lightyellow"
                        foundBlank = true;
                    }
                    else if (mark.value.trim() != "" && mark.type == "text" && parseFloat(mark.value) > parseInt(maxCount)) {
                        mark.style.backgroundColor = "lightgreen"
                        foundMax = true
                    }
                    else {
                        if (mark.style.backgroundColor == "lightgreen" || mark.style.backgroundColor == "lightyellow")
                            mark.style.backgroundColor = "white"
                    }
                }

                if (foundMax || foundBlank) {
                    if (foundMax && foundBlank)
                        oSrc.errormessage = "Marks should not be blank for yellow colored indicator(s) and should be less than or equal to " + maxCount + " for the green colored indicator(s)."
                    else if (foundMax)
                        oSrc.errormessage = "Mark should be less than or equal to " + maxCount + " for the green colored indicator(s)."
                    else
                        oSrc.errormessage = "Mark should not be blank for the yellow colored indicator(s)."

                    args.IsValid = false;
                    return true;
                }

                args.IsValid = true;
                return false;
            }


            //ddlMarks_

            function ValidateGrades(oSrc, args) {
                var found = false
                var finalFound = false

                //                $('[id*=ddlMarks_]').each(function () {
                //                    if ($(this).val() == '0') {
                //                        $(this).css('background-color', 'lightyellow')
                //                        found = true;
                //                    }
                //                    else
                //                        $(this).css('background-color', 'white')
                //                })

                var skillds = $("[ID$=hidSkillIds]").val().split(',')
                for (var k = 0; k < skillds.length; k++) {
                    if (skillds[k].trim() != '') {
                        var id = 'ddlMarks_' + skillds[k]
                        $('[id*=' + id + ']').each(function () {
                            if ($(this).val() != '0') {
                                found = true;
                            }
                        })

                        if (!found) {
                            $('[id*=' + id + ']').css('background-color', 'lightyellow')
                            finalFound = true;
                        }
                        else
                            $('[id*=' + id + ']').css('background-color', 'white')
                    }

                    found = false;
                }

                if (finalFound) {
                    oSrc.errormessage = "Grades should be selected for at least one Descriptors of each skill section."
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }

            function ValidateObservation(oSrc, args) {
                var foundBlank = false
                var foundLength = false
               
                var marks = document.getElementsByTagName("text");
                for (var k = 0; k < marks.length; k++) {
                    var mark = marks[k]
                    if (mark.value.trim() == "") {
                        mark.style.backgroundColor = "lightyellow"
                        foundBlank = true
                    }
                    else if (mark.value.trim().length > 200) {
                        mark.style.backgroundColor = "lightgreen"
                        foundLength = true
                    }
                    else {
                        if (mark.style.backgroundColor == "lightgreen" || mark.style.backgroundColor == "lightyellow")
                            mark.style.backgroundColor = "white"
                    }
                }

                if (foundBlank || foundLength) {
                    if (foundBlank && foundLength)
                        oSrc.errormessage = "Observation should not be blank for yellow colored indicator(s) and length should not be greater than 200 for green colored indicator(s)."
                    else if (foundBlank)
                        oSrc.errormessage = "Observation should not be blank for yellow colored indicator(s)."
                    else
                        oSrc.errormessage = "Observation length should not be greater than 200 for green colored indicator(s)."
                    args.IsValid = false;
                    return true;
                }

                args.IsValid = true;
                return false;
            }

        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
