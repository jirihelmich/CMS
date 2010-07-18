<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Backend: Add a new role
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Backend: Add a new role</h2>
    <p>You are about creating a new role inherited from the role <%= ViewData["name"] %>:</p>
    <%= ViewData["form"] %>

</asp:Content>
