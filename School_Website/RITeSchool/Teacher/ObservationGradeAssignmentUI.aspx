<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ObservationGradeAssignmentUI.aspx.cs" Inherits="ObservationGradeAssignmentUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
    <style>
        .container {
            width: 700px;
            border: 2px solid #1f8f6d;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
            height:auto;
            top:100px;
            left:100px;
            position:fixed;
            background-color:White;
        }

        .title-bar {
            background-color: #1f8f6d;
            color: white;           
            font-size: 18px;
            font-weight: bold;
            text-align: center;
            padding:5px;
        }

        .content {            
            background-color: white;
            text-align:left;            
        }
    </style>
        <table id="tblNote" runat="server" style="margin:auto; width:50%;margin-top:25px;" class="LblNoRecord" visible="false">
            <tr>
                <td align="center">
                    <span>Observation parameters have not yet been submitted.</span>
                </td>
            </tr>
        </table>
        <table id="tblData" runat="server" align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <asp:ValidationSummary ID="valsum" runat="server" CssClass="ClsMdtStar" />
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidateGrades"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidateRemark"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td class="ClsBorderlight" width="100px">
                                <span class="ClsLabel">Exam : </span>
                            </td>
                            <td class="ClsHilightBGB" width="150px">
                                <asp:Label ID="lblExam" runat="server" Text="" CssClass="ClsLabel"></asp:Label>
                            </td>
                            <td class="ClsBorderlight" width="100px">
                                <span class="ClsLabel">Class : </span>
                            </td>
                            <td class="ClsHilightBGB" width="150px">
                                <asp:Label ID="lblClass" runat="server" Text="" CssClass="ClsLabel"></asp:Label>
                            </td>
                            <td class="ClsBorderlight" width="100px">
                                <span class="ClsLabel">Subject : </span>
                            </td>
                            <td class="ClsHilightBGB" width="150px">
                                <asp:Label ID="lblSubject" runat="server" Text="" CssClass="ClsLabel"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
             <tr class="Height10">
                <td>
                </td>
            </tr>
            <tr class="Height10">
                <td id="tdMessage" runat="server" align="center">
                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="ClsLabel" style="float:inherit"></asp:Label>
                </td>
            </tr>
             <tr class="Height10">
                <td>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table id="tblParameters" runat="server">
                    </table>
                </td>
            </tr>
             <tr class="Height10">
                <td>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" UseSubmitBehavior="false"
                        onclick="btnSave_Click" />
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn"
                        UseSubmitBehavior="false" Enabled="false" onclick="btnSubmit_Click" />
                         <asp:Button ID="btnUnSubmit" runat="server" Text="UnSubmit" CssClass="ClsBtn" Visible = "false"
                        UseSubmitBehavior="false" Enabled="false" onclick="btnUnSubmit_Click" />
                    <asp:HiddenField ID="hidTestId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidStdDivId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidSubjectId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidTeacherId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidIsClassTeacher" runat="server" Value="N" />
                    <asp:HiddenField ID="hidFilterStdDivId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidRemarks" runat="server" Value="" />                    
                </td>
            </tr>               
         </table>      

       <div id="divRemarkContainer" style="display:none;" class="container">
            <div class="title-bar">Remark Templates</div>
            <div id="divRemark" class="content">            
            </div>
       </div>
    </div>
    <script language="javascript" type="text/javascript">

        function SelectAll(obj) {
            var grades = document.getElementsByTagName("select");
            var parameterId = obj.id.split('_')[3]
            for (var k = 0; k < grades.length; k++) {
                var grade = grades[k]
                var arr = grade.id.split('_')
                if (arr.length > 3 && arr[4] == parameterId) {
                    grade.value = obj.value;
                    SetColor(grade)
                }
            }
        }

        HighlightGrade();
        function HighlightGrade() {
            var grades = document.getElementsByTagName("select");
            for (var k = 0; k < grades.length; k++) {
                var grade = grades[k]
                var arr = grade.id.split('_')
                if (arr.length > 4 && grade.value == "0") {
                    grade.style.color = "Red";
                }
            }
        }

        function ValidateGrades(oSrc, args) {
            var isFound = false;
            var grades = document.getElementsByTagName("select");
            for (var k = 0; k < grades.length; k++) {
                var grade = grades[k]
                var arr = grade.id.split('_')
                if (arr.length > 4 && grade.value != "0") {                    
                    isFound = true;
                }
            }
            if (!isFound) {
                oSrc.errormessage = "Grade should be selected for at least one parameter.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateRemark(src, args) {
            var found = false;
            $('[id*=txtRemark]').each(function () {
                var id = this.id.replace('_txtRemark_', '_cmb_')
                var gradeId = $('#' + id).val()

                if ($(this).val() != '' && gradeId == 0) {
                    $('#' + id).css('background-color', 'lightyellow');
                    found = true
                }
                else
                    $('#' + id).css('background-color', 'white');
            })

            if (found) {
                src.errormessage = 'Grade should be selected for Yellow coloured fields if need to save remark.'
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function SetColor(obj) {
            if(obj.value == "0")
                obj.style.color = "Red";
            else
                obj.style.color = "black";
        }

        function ChangeAllRemark(obj, prmId) {
            $('[id$=_' + prmId + '][id*=txtRemark]').val($(obj).val())
        }

        function FillRemarks(obj,skillId, rmkId) {
            $('#divRemarkContainer').fadeIn(500)
            $('#divRemarkContainer').css({"left":((window.screen.width/2)-350)+'px'})            
            $('[id*=txtRemark]').css('background-color','white');
            $('[id$='+rmkId+']').css('background-color','lightyellow');

            var remarks = $('[id$=hidRemarks]').val()
            var remarkData = JSON.parse(remarks)
            var filteredData = remarkData.filter(rmk => rmk.Id == skillId);
            
            var sContent = ''
            for(var k=0; k< filteredData.length; k++)
            {
                sContent += '<li><a href="#" onclick="SetRemark(\''+filteredData[k].Remarks+'\',\''+rmkId+'\');return false;">'+filteredData[k].Remarks+'</a></li>'
            }

            $('#divRemark').html('<ol>'+sContent+'</ol><a style="float:right;padding-right:10px;" href="#" onclick="CloseDiv(\''+rmkId+'\');return false;">Close</a>')
        }

        function CloseDiv(rmkId)
        {
            $('#divRemarkContainer').hide()
            $('[id$='+rmkId+']').css('background-color','white');
        }

        function SetRemark(rmk, rmkId)
        {
            $('[id$='+rmkId+']').val(rmk)
            $('[id$='+rmkId+']').css('background-color','white');
            $('#divRemarkContainer').hide()
            $('[id$='+rmkId+']').focus();
        }

    </script>

    <script>
        $(document).click(function (event) {            
            if (event.target.id.match('btnPlus') == null && !$(event.target).closest("#divRemarkContainer").length) {
                $("#divRemarkContainer").fadeOut(500);
                $('[id*=txtRemark]').css('background-color', 'white');
            }
        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
