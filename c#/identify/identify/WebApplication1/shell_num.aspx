<%@ Page Title="request_shell_num" Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="shell_num.aspx.cs" Inherits="WebApplication1.shell_num" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <meta http-equiv="X-UA-Compatible" content="IE=10" />
    <script type="text/javascript" src="/Scripts/jquery1.12.4.js"></script>
    <script type="text/javascript" src="/Scripts/json2.js"></script>
    <script type="text/javascript" src="/Scripts/jquery.xdomainrequest.min.js"></script>
    <script type="text/javascript" src="/Scripts/sense-web-server.js"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        获取锁号
    </h2>
    <div style="display:none;" id="shellnum_required" guid = "<%=guid %>""schema="<%=request_schema %>" checkpath="<%=checkpath %>"></div>
    <script type="text/javascript" src="/Scripts/get_shellnum.js"></script>
    <button type="button" onclick="get_shellnum()">获取锁号</button>
</asp:Content>