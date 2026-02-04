<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ComboRpt.ascx.cs" Inherits="ASP.ComboRpt" %>
<asp:DropDownList runat="server" ID="cmbUC" Width="47%"></asp:DropDownList>
<asp:RequiredFieldValidator ID="RFVDDLParamReport" runat="server" ControlToValidate ="cmbUC"
                                            Display="None" InitialValue="0" Visible="True"></asp:RequiredFieldValidator>
<asp:Label ID="lblDDLMandatory" runat="server" ForeColor="red" Text="*" 
                                                                    Visible="True"></asp:Label>