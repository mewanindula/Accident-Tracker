<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="WebApplication1.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="Button1" runat="server" Text="History" OnClick="Button1_Click" />

        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Location" />

        <asp:Table ID="Table1" runat="server">

            <asp:TableRow>
                <asp:TableHeaderCell>Telephone</asp:TableHeaderCell>
                <asp:TableHeaderCell>Status</asp:TableHeaderCell>
                <asp:TableHeaderCell>Latitude</asp:TableHeaderCell>
                <asp:TableHeaderCell>Longitude</asp:TableHeaderCell>
                <asp:TableHeaderCell>Force</asp:TableHeaderCell>
                <asp:TableHeaderCell>Flame</asp:TableHeaderCell>
            </asp:TableRow>

        </asp:Table>
    
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    
    </div>
    </form>
</body>
</html>
