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
        height: auto;
        top: 100px;
        left: 100px;
        position: fixed;
        background-color: White;
        z-index: 1000;
    }

    .title-bar {
        background-color: #1f8f6d;
        color: white;
        font-size: 18px;
        font-weight: bold;
        text-align: center;
        padding: 5px;
    }

    .content {
        background-color: white;
        text-align: left;
    }

    /* Freeze header rows + Roll No. / Student Name columns */
    /* max-width:0 on cell forces scroll inside div (stops page from expanding with wide table) */
    .obs-scroll-cell {
        width: 100%;
        max-width: 0;
    }

    #divParametersScroll {
        width: 100%;
        max-width: 100%;
        height: calc(100vh - 280px);
        min-height: 520px;
        overflow-x: auto;
        overflow-y: auto;
        border: 1px solid #ccc;
        text-align: left;
        box-sizing: border-box;
        -webkit-overflow-scrolling: touch;
    }

    /*
        Table should use available screen width when possible.
        If parameter columns require more space, table expands
        and the parent div automatically shows horizontal scrollbar.
    */
    #divParametersScroll table {
        border-collapse: separate;
        border-spacing: 0;
        width: max-content;
        min-width: 100%;
    }

    /*
        Compact parameter cells.
        Width is controlled from C# based on parameter count.
    */
    #divParametersScroll td.obs-parameter-cell {
        box-sizing: border-box;
        padding-left: 5px;
        padding-right: 5px;
        vertical-align: middle;
    }

    /*
        Parameter names should wrap instead of increasing column width.
        Maximum header height is approximately 2 lines.
    */
    #divParametersScroll td.obs-parameter-header {
        white-space: normal !important;
        word-break: break-word;
        overflow-wrap: anywhere;
        line-height: 18px;
        height: 36px;
        max-height: 36px;
        vertical-align: middle;
    }

    /*
        Keep dropdowns, textboxes and input controls
        inside their parameter column.
    */
    #divParametersScroll td.obs-parameter-cell select,
    #divParametersScroll td.obs-parameter-cell textarea,
    #divParametersScroll td.obs-parameter-cell input {
        max-width: 100%;
        box-sizing: border-box;
    }

    /*
        Override shared Styles2.css height:20px
        so sticky header rows keep their actual height.
    */
    #divParametersScroll td.obs-freeze-top {
        position: -webkit-sticky !important;
        position: sticky !important;
        top: 0;
        z-index: 3;
        height: auto !important;
        background-color: #BFE0F2 !important;
        background-clip: padding-box;
    }

    #divParametersScroll thead td.obs-freeze-top {
        position: -webkit-sticky !important;
        position: sticky !important;
    }

    /*
        Freeze Roll No. column while horizontal scrolling.
    */
    .obs-freeze-left-roll {
        position: -webkit-sticky !important;
        position: sticky !important;
        left: 0;
        z-index: 2;
        background-color: #e6effc !important;
        min-width: 100px;
        width: 100px;
        box-sizing: border-box;
        background-clip: padding-box;
    }

    /*
        Freeze Student Name column while horizontal scrolling.
    */
    .obs-freeze-left-name {
    position: -webkit-sticky !important;
    position: sticky !important;
    left: 100px;
    z-index: 2;
    background-color: #e6effc !important;

    width: 200px;
    min-width: 200px;
    max-width: 200px;

    box-sizing: border-box;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    background-clip: padding-box;
}

    /*
        Header background for frozen columns.
    */
    .ClsProgressGridTestHeader.obs-freeze-left-roll,
    .ClsProgressGridTestHeader.obs-freeze-left-name,
    .obs-freeze-corner {
        background-color: #BFE0F2 !important;
    }

    /*
        Top-left corner cells should stay above
        both vertical and horizontal scrolling.
    */
    .obs-freeze-corner {
        z-index: 6 !important;
    }

    /*
        Header cells that are also left-frozen
        stay above scrolling body cells.
    */
    #divParametersScroll thead td.obs-freeze-left-roll,
    #divParametersScroll thead td.obs-freeze-left-name {
        z-index: 6 !important;
    }
</style>

        <table id="tblNote" runat="server" style="margin:auto; width:50%;margin-top:25px;" class="LblNoRecord" visible="false">
            <tr>
                <td align="center">
                    <span id="spnNote" runat="server">Observation parameters have not yet been submitted.</span>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnNoteBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false" />
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
                <td align="left" class="obs-scroll-cell">
                    <div id="divParametersScroll">
                        <table id="tblParameters" runat="server">
                        </table>
                    </div>
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
                    <asp:HiddenField ID="hidIsSummaryMode" runat="server" Value="N" />
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
                sContent += '<li><a href="#" onclick="SetRemark(\''+filteredData[k].Remarks.replace('\'','$')+'\',\''+rmkId+'\');return false;">'+filteredData[k].Remarks+'</a></li>'
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
            rmk = rmk.replace('$','\'')
            $('[id$='+rmkId+']').val(rmk)
            $('[id$='+rmkId+']').css('background-color','white');
            $('#divRemarkContainer').hide()
            $('[id$='+rmkId+']').focus();
        }

        function FillRemarksDynamic(btn, skillId, parameterId, txtClientId) {         
            var td = btn.closest('td');
            var ddl = td.querySelector('select');

            var gradeId = ddl ? ddl.value : 0;

            if (gradeId == 0 || gradeId === "0") {
                if (ddl) {
                    ddl.focus();
                    ddl.style.backgroundColor = 'lightyellow';
                }
                return false;
            }
            
           var rmkId = txtClientId

           gradeId = gradeId || $('#' + rmkId.replace('_txtRemark_', '_cmb_')).val();
            $('#divRemarkContainer').fadeIn(500)
            $('#divRemarkContainer').css({"left":((window.screen.width/2)-350)+'px'})            
            $('[id*=txtRemark]').css('background-color','white');
            $('[id$='+rmkId+']').css('background-color','lightyellow');

            var remarks = $('[id$=hidRemarks]').val()
            var remarkData = JSON.parse(remarks)

            var filteredData = remarkData.filter(rmk => rmk.Id == skillId && rmk.ParameterId == parameterId && rmk.GradeId == gradeId);

            var sContent = ''
         
            for(var k=0; k< filteredData.length; k++)
            {
                sContent += '<li><a href="#" onclick="SetRemark(\''+filteredData[k].Remarks+'\',\''+rmkId+'\');return false;">'+filteredData[k].Remarks+'</a></li>'
            }

            $('#divRemark').html('<ol>'+sContent+'</ol><a style="float:right;padding-right:10px;" href="#" onclick="CloseDiv(\''+rmkId+'\');return false;">Close</a>')

            return false;
        }

    </script>

    <script>
        $(document).click(function (event) {            
            if (event.target.id.match('btnPlus') == null && !$(event.target).closest("#divRemarkContainer").length) {
                $("#divRemarkContainer").fadeOut(500);
                $('[id*=txtRemark]').css('background-color', 'white');
            }
        });

        // Move Skills / Parameter / bulk-control rows into <thead> so sticky top works reliably
        function EnsureObsHeaderThead() {
            var $table = $('#divParametersScroll table').first();
            if ($table.length === 0 || $table.data('obsTheadDone') === true) {
                return;
            }

            var headerRows = $table.find('tr.obs-header-row').get();
            if (headerRows.length === 0) {
                return;
            }

            var thead = $table.children('thead')[0];
            if (!thead) {
                thead = document.createElement('thead');
                $table[0].insertBefore(thead, $table[0].firstChild);
            }

            for (var i = 0; i < headerRows.length; i++) {
                thead.appendChild(headerRows[i]);
            }

            $table.data('obsTheadDone', true);
        }

        function ApplyObsFreezeOffsets() {
            var $scroll = $('#divParametersScroll');
            if ($scroll.length === 0) {
                return;
            }

            EnsureObsHeaderThead();

            // Horizontal freeze: Student Name sticks after actual Roll No. width
            var rollWidth = $scroll.find('td.obs-freeze-left-roll').first().outerWidth();
            if (!rollWidth || rollWidth < 1) {
                rollWidth = 100;
            }
            $scroll.find('td.obs-freeze-left-name').css('left', rollWidth + 'px');

            // Vertical freeze: cumulative top for each header row (Skills, Parameters, bulk controls)
            var $rows = $scroll.find('thead tr.obs-header-row');
            if ($rows.length === 0) {
                $rows = $scroll.find('tr.obs-header-row');
            }

            var top = 0;
            $rows.each(function () {
                var row = this;
                var rowHeight = row.offsetHeight || $(row).outerHeight() || 0;
                var cells = row.cells || $(row).children('td').get();
                for (var c = 0; c < cells.length; c++) {
                    cells[c].style.position = 'sticky';
                    cells[c].style.top = top + 'px';
                }
                top = top + rowHeight;
            });
        }

        $(document).ready(function () {
            ApplyObsFreezeOffsets();
            setTimeout(ApplyObsFreezeOffsets, 50);
            setTimeout(ApplyObsFreezeOffsets, 250);
        });

        $(window).load(function () {
            ApplyObsFreezeOffsets();
        });

        $(window).resize(function () {
            ApplyObsFreezeOffsets();
        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
