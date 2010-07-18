<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Backend map</h2>
    <ul>
        <li><a href="/backend/">Map</a></li>
        <li><a href="/backend/listArticles">Articles List</a></li>
        <li><a href="/backend/listCategories">Categories List</a></li>
        <li><a href="/backend/listResources">Resources List</a></li>
        <li><a href="/backend/listRoles">Roles List</a></li>
        <li><a href="/backend/listUsers">Users List</a></li>
        <li><a href="/backend/settings">Settings</a></li>
    </ul>

</asp:Content>
