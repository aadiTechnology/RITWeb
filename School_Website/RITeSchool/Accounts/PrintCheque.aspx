<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintCheque.aspx.cs" Inherits="PrintCheque" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Print Cheque</title>
    <link href="../Styles/Styles.css" type="text/css" rel="stylesheet" />
    <link href="../Styles/Styles1.css" type="text/css" rel="stylesheet" />
    <link href="../Styles/Styles2.css" type="text/css" rel="stylesheet" />
    <link href="../Styles/Styles3.css" type="text/css" rel="stylesheet" />
	<style type="text/css">
		body, form, div, span { margin: 0px; padding: 0px; }
		#crosschq { top: 0; left: 0; }
		#canvas { position: relative; font-size: 10pt; }
		#canvas span, #canvas img { position: absolute; text-align: justify; overflow: hidden; }
		#amount { font-weight: bold; }
		#company, #signatory1, #signatory2 { text-align: center !important; }
	</style>
</head>
<body onload="window.print();">
    <form id="form1" runat="server">
		<div id="canvas">
			<img id="crosschq" runat="server" clientidmode="Static" src="../images/crosschq.png"></img>
			<span id="date" runat="server" clientidmode="Static"></span>
			<span id="payee" runat="server" clientidmode="Static"></span>
			<span id="amount" runat="server" clientidmode="Static"></span>
			<span id="amountinwords" runat="server" clientidmode="Static"></span>
			<span id="company" runat="server" clientidmode="Static"></span>
			<span id="signatory1" runat="server" clientidmode="Static"></span>
			<span id="signatory2" runat="server" clientidmode="Static"></span>
		</div>
    </form>
</body>
</html>
