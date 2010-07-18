<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Backend: Add a new resource
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="box left box-width-100"> 
	<h3 class="box-name"><span><span>Pøidat aplikaèní zdroj</span></span></h3> 
	<div class="box-content"> 
    <%= ViewData["form"] %>
		
		<span class="left-corner"></span> 
		<span class="right-corner"></span> 
	</div> 
</div> 
</asp:Content>
