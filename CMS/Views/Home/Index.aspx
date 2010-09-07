<%@ Import Namespace="System.IO" %>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CMS.CMS.OutputModels.NewsListOutputModel>>" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Úvodní strana
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    
		<div id='carousel'>
			<div class='clearfix'>
				<div class='carousel-item number1 selected'>
					<span>&nbsp;</span>
					<div class='carousel-content'>
						<div class='left'>
							<h3>Měřící technika</h3>
							<ul>
								<li><a href='#'>Měřiče tepla a tlaku</a></li>
								<li><a href='#'>Měřiče tepla a tlaku</a></li>
							</ul>
							<p>Lorem ipsum dolor sit amet, etur adipiscing elit. Donec varius ius ultrices. Sed tellus quam, venenatis ut molestie </p>
						</div>
						<div class='right'>
							<img src='images/carousel-photo.jpg' alt='Photo' />
							<ul class='thumbnails'>
								<li><a href='#' class='selected'><img src='images/carousel-thumbnail.jpg' alt='Photo' /></a></li>
								<li><a href='#'><img src='images/carousel-thumbnail.jpg' alt='Photo' /></a></li>
								<li><a href='#'><img src='images/carousel-thumbnail.jpg' alt='Photo' /></a></li>
							</ul>
						</div>
					</div>
				</div>
				<div class='carousel-item number2'>
					<span>&nbsp;</span>
					<div class='carousel-content'>
						bb
					</div>
				</div>
				<div class='carousel-item number3'>
					<span>&nbsp;</span>
					<div class='carousel-content'>
						cc
					</div>
				</div>
				<div class='carousel-item number4'>
					<span>&nbsp;</span>
					<div class='carousel-content'>
						dd
					</div>
				</div>
				<div class='carousel-item number5'>
					<span>&nbsp;</span>
					<div class='carousel-content'>
						ee
					</div>
				</div>
				<div class='carousel-item number6'>
					<span>&nbsp;</span>
					<div class='carousel-content'>
						ff
					</div>
				</div>
				<div class='carousel-item number7'>
					<span>&nbsp;</span>
					<div class='carousel-content'>
						gg
					</div>
				</div>
			</div>
		</div> <!-- /#carousel-->
		<div id='news-container' class='clearfix'>
        <%
            int i = 0;
            
            foreach (CMS.CMS.OutputModels.NewsListOutputModel news in Model)
            {
        %>
			<div class='news <% if(i==0){ %>left <% }else if(i==3){ %>dark<% } %>'>
				<div class='news-inner'>
					<div class='news-image'>
						<img src='' alt='aktualita' />
					</div>
					<div class='news-text'>
						<h3><%= news.Title.getByCulture("cz") %></h3>
						<%= news.Shortdesc.getByCulture("cz") %><a href='#'>...</a>
					</div>
				</div>
			</div> <!-- /#news-->
        <%
            }
        %>
		</div> <!-- /#news-container-->
</asp:Content>
