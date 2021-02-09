<%@ Page Title="登录" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication1.Account.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <meta http-equiv="X-UA-Compatible" content="IE=10" />
    <script type="text/javascript" src="/Scripts/jquery1.12.4.js"></script>
    <script type="text/javascript" src="/Scripts/json2.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.xdomainrequest.min.js"></script>
    <script type="text/javascript" src="/Scripts/sense-web-server.js"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        登录
    </h2>
    <div style="display:none;" id="lockinfo_required", random_value="<%=hash %>" guid="<% = guid %>" schema="<%=request_schema %>" need_pub="<%=need_pub %>" licid="<%=licid %>" checkpath="<%=checkpath %>"></div>
    <script type="text/javascript" src="/Scripts/getlockinfo-demo.js"></script>
    <button type="button" onclick="getlockinfo()">验证</button>
</asp:Content>