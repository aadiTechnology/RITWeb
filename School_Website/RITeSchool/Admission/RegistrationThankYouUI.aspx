<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistrationThankYouUI.aspx.cs" Inherits="RegistrationThankYouUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .alert-box {
            max-width: 80%;
            margin: 50px auto;
            padding-top: 20px;
            background-color: #d4edda; /* Light green background */
            color: #155724; /* Dark green text */
            border: 1px solid #c3e6cb; /* Softer green border */
            border-radius: 8px;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
            text-align: center;
        }

            .alert-box h3 {
                margin-top: 0;
                font-size: 22px;
                font-weight: 600;
            }

            .alert-box p {
                margin: 10px 0;
                font-size: 16px;
            }

            .alert-box .btn {
                display: inline-block;
                padding: 10px 18px;
                background-color: #28a745; /* Success green */
                color: white;
                text-decoration: none;
                border-radius: 5px;
                margin-top: 15px;
                transition: background-color 0.3s ease;
            }              
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="alert-box">            
           <span id="spnMessage" runat="server"><h3>Your enquiry form has been submitted successfully.</h3></span>
        </div>
    </form>
</body>
</html>
