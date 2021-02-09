<%@ Page   Language="C#" AutoEventWireup="true" CodeBehind="get_pubdata.aspx.cs" Inherits="WebApplication1.get_pubdata" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>get_pubdata</title>
</head>
<body>
    <script type="text/javascript" src="/Scripts/jquery1.12.4.js"></script>
    <script type="text/javascript" src="/Scripts/json2.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.xdomainrequest.min.js"></script>
    <script type="text/javascript" src="/Scripts/sense-web-server.js"></script>
    <form id="form1" runat="server">
        <div>
        <h2>
        获取公开区数据
        </h2>
        </div>
        <asp:Label ID="Label1" runat="server" Text="账号"></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <p>
            <asp:Label ID="Label2" runat="server" Text="密码"></asp:Label>
            <asp:TextBox ID="TextBox2" runat="server" EnableViewState="False"></asp:TextBox>
        </p>
        <div style="display:none;" id="pub_data_required" guid = "<%=guid %>""schema="<%=request_schema %>" checkpath="<%=checkpath %>"></div>
         <script type="text/javascript" src="/Scripts/get_pubdata.js"></script>
        <button type="button" onclick="getlock_pubdata()">获取公开区数据</button>
    </form>
</body>
</html>