<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	N�st�nka
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 
<div class="box left box-width-100"> 
	<h3 class="box-name"><span><span>N�st�nka</span></span></h3> 
	<div class="box-content"> 

   <!-- <ul >
        <li><a href="http://localhost:17815/backend/">Kategorie</a></li>
        <li><a href="http://localhost:17815/backend/listArticles">U�ivatel�</a></li>
        <li><a href="http://localhost:17815/backend/listCategories">Opr�vn�n�</a></li>
        <li id="static"><a href="http://localhost:17815/backend/listResources">Statick� str�nky</a></li>
        <li id="products"><a href="http://localhost:17815/backend/listRoles">Produkty</a></li>
        <li><a href="http://localhost:17815/backend/listUsers">Nastaven�</a></li>
        <li id="files"><a href="http://localhost:17815/backend/settings">Soubory</a></li>
    </ul>-->
     
    <ul id="dashboard">
        <!--<li><a href="/backend/">Map</a></li>-->
        <!--<li><a href="/backend/listArticles">Articles List</a></li>-->
        <li id="static"><a href="<%= Url.Action("listPages","Backend") %>">Statick� str�nky</a></li>
        <li id="categories"><a href="<%= Url.Action("listCategories","Backend") %>">Kategorie</a></li>
        <li id="access"><a href="<%= Url.Action("listPages","Backend") %>">Opr�vn�n�</a></li>
        <!--<li><a href="/backend/listRoles">Roles List</a></li>-->
        <li id="users"><a href="<%= Url.Action("listUsers","Backend") %>">U�ivatel�</a></li>
        <li id="settings"><a href="<%= Url.Action("listSettings","Backend") %>">Nastaven�</a></li>
    </ul>
		
		<span class="left-corner"></span> 
		<span class="right-corner"></span> 
	</div> 
</div> 

</asp:Content>
