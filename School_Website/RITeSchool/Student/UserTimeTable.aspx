<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="UserTimeTable.aspx.cs" Inherits="UserTimeTable" ViewStateMode="Disabled" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="95%" align="center">
            <tr>
                <td align="Center" id="tdError" runat="server" visible="false" class="LblNoRecord">
                    <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="ClsConfigText" ForeColor = "Blue"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div id="divLink" runat="server" style="width: 120px; text-align: center" class="ClsHilightBGTT"
                        visible="false">
                        <asp:LinkButton runat="server" ViewStateMode="Enabled" ID="hlnkTTSchedule" Text="Lecture Timings" CssClass="ClsLogoutNew"></asp:LinkButton>
                    </div>
                </td>
            </tr>
            <tr>
				<td align="left" colspan="2" id="tdLegend" runat="server">
					<table>
						<tr>
							<td>
								<span class="ClsLblLgnd">Legend :</span>
							</td>
							<td style="padding: 0; border: 1px solid #000;">
								<span class="UsrTTNA" style="vertical-align: middle; padding: 3px; display: block;"><b><font color="black" face="Verdana" size="2">N/C</font></b></span>
							</td>
							<td>
								<span class="ClsLblLgnd" style="margin-left: 3px; color: black;">Not Configured</span>
							</td>
						</tr>
					</table>
				</td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <div id="divTT" style="margin-top: 10px;">
                        <asp:GridView ID="grdTT" runat="server" AutoGenerateColumns="false" PageSize="200"
                            CellPadding="0" CellSpacing="1" EnableViewState="false" BackColor="#6394d6" ForeColor="#333333"
                            GridLines="None" OnRowDataBound="grdTT_RowDataBound">
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:BoundField DataField="Lecture_Name" HeaderText="Weekdays>>" SortExpression="Lecture_No">
                                    <ItemStyle HorizontalAlign="Center" Wrap="False" VerticalAlign="Middle" CssClass="LblSmlV" />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="True" Width="110px" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="UsrGridHead" />
                            <AlternatingRowStyle CssClass="TTCells" />
                            <RowStyle CssClass="TTCells" />
                            <EmptyDataRowStyle CssClass="LblNoRecord" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center" style="width: 50%">
                    <div runat="server" id="divSubjectLect" class="GrdTotal" visible="false" style="width: 80%">
                        <span id="lblHead">Class-Subject Lecture Count</span></div>
                </td>
                <td align="center" class="ClspaddingSmallT">
                    <div runat="server" id="divAdditionalLect" class="GrdTotal " style="width: 80%">
                        <span id="Span1">Additional Lectures</span></div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView ID="grdSubjectLect" Width="80%" runat="server" ViewStateMode="Enabled" AutoGenerateColumns="false"
                        CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None" BackColor="#5C6F7B"
                        DataKeyNames="Teacher_Subject_Id,Subject_Id,Standard_Division_Id">
                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                        </PagerStyle>
                        <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                            FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                        <Columns>
                            <asp:BoundField DataField="Class_Subject" HeaderText="Class-Subjects">
                                <ItemStyle HorizontalAlign="Center" Wrap="False" VerticalAlign="Middle" Height="25px"
                                    CssClass="LblSmlV" />
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Count" HeaderText="Lecture Count">
                                <ItemStyle HorizontalAlign="Center" Wrap="False" VerticalAlign="Middle" Height="25px"
                                    CssClass="LblSmlV" />
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                            </asp:BoundField>
                        </Columns>
                        <RowStyle CssClass="TTCells" />
                        <HeaderStyle CssClass="UsrGridHead" />
                        <AlternatingRowStyle CssClass="TTCells" />
                        <EmptyDataRowStyle CssClass="LblNoRecord" />
                    </asp:GridView>
                </td>
                <td align="center" valign="top">
                    <asp:GridView ID="grdAdditionalClasses" Width="80%" runat="server" ViewStateMode="Enabled" AutoGenerateColumns="false"
                        CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None" BackColor="#5C6F7B"
                        Visible="true" EmptyDataText="No Additional Lectures Assigned">
                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                        </PagerStyle>
                        <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                            FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                        <Columns>
                            <asp:BoundField DataField="WeekDayName" HeaderText="WeekDay">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" VerticalAlign="Middle" CssClass="LblSmlVP" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" CssClass="LblSmlVP"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="LectureNumber" HeaderText="Lecture Number">
                                <ItemStyle HorizontalAlign="Center" Wrap="False" VerticalAlign="Middle" CssClass="LblSmlVP" />
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" CssClass="LblSmlVP"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="ClassName" HeaderText="Class">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" VerticalAlign="Middle" CssClass="LblSmlVP" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" CssClass="LblSmlVP"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="SubjectName" HeaderText="Subject">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" VerticalAlign="Middle" CssClass="LblSmlVP" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" CssClass="LblSmlVP"/>
                            </asp:BoundField>
                        </Columns>
                        <RowStyle CssClass="TTCells" />
                        <HeaderStyle CssClass="UsrGridHead" />
                        <AlternatingRowStyle CssClass="TTCells" />
                        <EmptyDataRowStyle CssClass="LblNoRecord" HorizontalAlign="Center" />
                    </asp:GridView>
                </td>
            </tr>
            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <tr>
                    <td colspan="2" rowspan="1">
                        <asp:Label ID="lblConfigError" CssClass="ClsConfigText" runat="server" ViewStateMode="Enabled" ForeColor="blue"
                            Width="100%" Visible="False"></asp:Label>
                        <br />
                        <asp:HyperLink ID="lnkLecturesPerDay" CssClass="ClsConfigLink" Text="Lectures Per day"
                            runat="server" ViewStateMode="Enabled" NavigateUrl="~/RITeSchool/Admin/LecturesPerStandardWeekday.aspx"
                            Visible="false"></asp:HyperLink>
                        <br />
                        <asp:HyperLink ID="lnkLectureInweek" CssClass="ClsConfigLink" Text="Standard subject Lectures Per week"
                            runat="server" ViewStateMode="Enabled" NavigateUrl="~/RITeSchool/Admin/StandardSubjectwiseLectures.aspx"
                            Visible="false"></asp:HyperLink>
                    </td>
                </tr>
            </asp:Panel>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <asp:HiddenField ID="hidSortExpression" runat="server" ViewStateMode="Enabled"/>
                    <asp:HiddenField ID="hidSortDirection" runat="server" ViewStateMode="Enabled"/>
                    <asp:HiddenField ID="hidSchoolId" runat="server" ViewStateMode="Enabled"/>
                    <asp:HiddenField ID="hidAcademicYearId" runat="server" ViewStateMode="Enabled"/>
                    <asp:HiddenField ID="hidStandardId" runat="server" ViewStateMode="Enabled"/>
                </td>
            </tr>
        </table>        
    </div>  
</asp:Content>
