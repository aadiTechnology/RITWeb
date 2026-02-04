<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="UploadPhotoViewUI.aspx.cs" Inherits="PhotoGalleryUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <asp:UpdatePanel ID="UPnlPhotoGallery" runat="server">
        <ContentTemplate>
            <table align="center" border="0" cellpadding="0"  cellspacing="1" style="width: 97%;">
                <tr align="center">
					<td align="center">
						<table width="100%" align="center">
							<tr>
								<td class="ClsGrayMainTitle" style="height: 20px;" align="left">
									<asp:Label ID="lblAddAcademicYear" runat="server" CssClass="MainTitleHead" Font-Bold="True"
											   Text="View Images" EnableViewState="false"></asp:Label>
								</td>
							</tr>
						</table>
					</td>
				</tr>
                <tr>
                    <td align="center" >
                        <table width="80%" cellpadding="1" cellspacing="1" >
                            <tr>
                                <td align="center">
                                	<asp:Label ID="lblErrorMessage"
											   runat="server"
											   EnableViewState="false"
											   CssClass="ClsLabel"
											   style="width: 100%; margin: 8px 0;"
											   ForeColor="Red"
											   Visible="false" />
									<asp:Label ID="lblUpateMessage"
											   runat="server"
											   EnableViewState="false"
											   CssClass="ClsLabel"
											   style="width: 100%; margin: 8px 0;"
											   ForeColor="Blue"
											   Font-Bold="true"
											   Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="height:20px">
                                    <asp:Label ID="lblGalleryNameHeader" runat="server" BorderWidth="0px" 
                                        CssClass="ClsLblLgnd" EnableViewState="False" Font-Bold="True" 
                                        Text="Gallery Name : " Width="90px"></asp:Label>
                                    <asp:Label ID="lblGalleryName" runat="server" CssClass="ClsHilightPhotoBGB" 
                                        EnableViewState="true" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblExistingImages" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                        EnableViewState="False" Font-Bold="True" Text="Existing Uploaded Photos :"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table width="100%" cellspacing="1" cellpadding="1">
                                        <tr>
                                            <td width="70px" class="ClsBorderlight" align="left">
                                                <%--<asp:Label ID="lblComment" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                    Text="Comment :"></asp:Label>--%><span class="ClsLabel">Comment :</span>
                                            </td>
                                            <td  align="center">
                                                <asp:TextBox ID="txtComment" runat="server" MaxLength="200" CssClass="LrgTxtBox"
                                                    Width="99%"></asp:TextBox>
                                            </td>
                                            <td width="100px" align="right">
                                                <asp:Button ID="btnPhotoUpdate" runat="server" BorderStyle="Solid" CssClass="ClsBtnMid"
                                                    Text="Update" UseSubmitBehavior="False" ValidationGroup="valGrpDetailsUpdate"
                                                    OnClick="btnPhotoUpdate_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr >
                    <td style="background-color: white;" align="center" valign="top">
                        <table width="80%" cellpadding="1" cellspacing="1"> 
                            <tr>
                                <td id="s">
                                    <asp:GridView ID="grdPhotos" runat="server" AutoGenerateColumns="False" CellPadding="0"
                                        CellSpacing="1" CssClass="GridBorder" DataKeyNames="Gallery_Id" ForeColor="#333333"
                                        GridLines="None" OnRowCommand="grdPhotos_RowCommand" OnRowDataBound="grdPhotos_RowDataBound"
                                        PageSize="1000" Width="100%" EmptyDataText="No photo available.">
                                        <PagerStyle Font-Bold="True" Font-Underline="False" ForeColor="Black" HorizontalAlign="Right" />
                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                            NextPageText="Next" Position="TopAndBottom" PreviousPageText="Previous" />
                                        <Columns>
                                            <asp:BoundField DataField="Image_Path" HeaderText="Image Path">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>
                                            <asp:ImageField AlternateText="Image Path" DataImageUrlField="Image_Path" HeaderText="Images">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                                            </asp:ImageField>
                                            <asp:BoundField DataField="Comment" HeaderText="Comments">
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Wrap="true" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Wrap="true" />
                                            </asp:BoundField>
                                            <asp:ButtonField ButtonType="Image" CommandName="EDIT_ROW" HeaderText="Edit" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                Text="Edit">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                            </asp:ButtonField>
                                            <asp:ButtonField ButtonType="Image" CommandName="DELETE_ROW" HeaderText="Delete" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                Text="Delete">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                            </asp:ButtonField>
                                        </Columns>
                                        <RowStyle CssClass="ClsGridRow" />
                                        <HeaderStyle CssClass="ClsGridHeader" />
                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnClose" Text="Close" CssClass="ClsBtnSml" BorderStyle="Solid" runat="server"
                            BorderWidth="1px" CausesValidation="false" UseSubmitBehavior="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hidGalleryId" runat="server" Value="0" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript" language="javascript">
        function ConfirmPhotoDelete() {
            var bResult = true;
            if (!window.confirm("Are you sure you want to delete this photo?"))
                bResult = false;
            return bResult;
        }
        function refreshParent() {
            window.opener.location = window.opener.location;
            window.close();
            window.opener.focus();
        }
    </script>
</asp:Content>
