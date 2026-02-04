<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/OnlineAdmission.master"
    AutoEventWireup="true" CodeFile="PaymentStatusUI.Aspx.cs" EnableEventValidation="false"
    Inherits="PaymentStatusUI" Title= "Online Admission for 2013 - 14." %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
  <table id="divErrorMsg" align="center" runat="server" style="width: 97%; height: 100%;">
        <tr>
            <td align="center" valign="middle">               
                     <asp:Label  ID="lblErrorMsg" Font-Bold="true"
                    runat="server" ForeColor="Red" EnableViewState="False" CssClass="ClsMdtStar"
                    Text="Error occured while processing your transaction. Please try again."></asp:Label>
            </td>
        </tr>
    </table>
    <div style="width: 97%; height: 100%; vertical-align: middle; text-align: center;" visible="false"
        align="center">
        <div align="center" runat="server" id="divControls" visible="false">
            <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4">
            </b></b>
            <table>
                <tr>
                    <td>
                        Merchant Id
                    </td>
                    <td>
                        <asp:TextBox ID="SRCSITEID" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Merchant Tran Id
                    </td>
                    <td>
                        <asp:TextBox ID="PRN" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Item Code
                    </td>
                    <td>
                        <asp:TextBox ID="ITC" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Currency
                    </td>
                    <td>
                        <asp:TextBox ID="CRN" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Amount
                    </td>
                    <td>
                        <asp:TextBox ID="AMT" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Bank Id
                    </td>
                    <td>
                        <asp:TextBox ID="CSTBANKID" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Tran Reference No
                    </td>
                    <td>
                        <asp:TextBox ID="BID" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Tran Status
                    </td>
                    <td>
                        <asp:TextBox ID="PAID" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <b class="rbottom"><b class="r4"></b><b class="r3"></b><b class="r2"></b><b class="r1">
            </b></b>
        </div>
        
        <br />
    </div>  
      <script language="JavaScript" type="text/javascript">
        window.history.forward(1)
        history.go(1);
    </script>
</asp:Content>
