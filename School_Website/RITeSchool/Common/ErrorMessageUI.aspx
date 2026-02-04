<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="ErrorMessageUI.aspx.cs" Inherits="RITeSchool_Common_ErrorMessageUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" Runat="Server">
 <style>
        .alert-box {
            max-width: 80%;
            margin: 50px auto;
            padding: 20px;
            background-color: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
            border-radius: 8px;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }
        .alert-box h3 {
            margin-top: 0;
            font-size: 20px;
        }
        .alert-box p {
            margin: 10px 0;
        }
        .alert-box .btn {
            display: inline-block;
            padding: 10px 15px;
            background-color: #dc3545;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            margin-top: 10px;
        }
        .alert-box .btn:hover {
            background-color: #c82333;
        }
    </style>
     <div class="alert-box">
            <h3>⚠️ Something went wrong</h3>            
            <p>Kindly send the issue details to the Software Coordinator using the Message Center facility.</p>           
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" Runat="Server">
</asp:Content>

