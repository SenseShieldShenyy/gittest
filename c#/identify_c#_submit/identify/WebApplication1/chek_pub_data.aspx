<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chek_pub_data.aspx.cs" Inherits="WebApplication1.chek_pub_data" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:Label ID="Label1" runat="server" Text="账号"></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <p>
            <asp:Label ID="Label2" runat="server" Text="密码"></asp:Label>
            <asp:TextBox ID="TextBox2" runat="server" EnableViewState="False"></asp:TextBox>
        </p>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="确定" />
    </form>
</body>
</html>
