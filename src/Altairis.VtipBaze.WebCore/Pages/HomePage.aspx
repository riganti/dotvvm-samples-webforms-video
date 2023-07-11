<%@ Page Title="VtipBáze" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="Altairis.VtipBaze.WebCore.Pages.HomePage" ViewStateMode="Disabled" %>
<asp:Content runat="server" ContentPlaceHolderID="MainCPH">
    <asp:PlaceHolder ID="WelcomePlaceHolder" runat="server" Visible="false">
        <article>
            <header>
                <asp:Localize runat="server" Text="Vítejte na VtipBázi" />
            </header>
            <p>VtipBáze je - jak již bystřejším z vás došlo - databáze vtipů. Původně vzniklá z mé soukromé sbírky, kterou jsem se léta dokopával zveřejnit. Nakonec mě k tomu přiměl státní smutek, který mi přijde pitomý sám o sobě a <a href="http://www.weblog.rider.cz/articles/265-stat-nejsem-ja">v případě Havlova úmrtí teprve</a>. Jak lépe bojovat proti hysterickému patosu, než vtipem?</p>
            <p>Pokud zde svůj oblíbený vtip nenajdete, <a href="/new">přidejte ho</a>.
                <!--Pokud chcete, můžete si též nechat <a href="/mailing">nové vtipy posílat e-mailem</a>.-->
            </p>
        </article>
    </asp:PlaceHolder>
    <asp:ListView ID="JokesList" runat="server" ItemType="Altairis.VtipBaze.Data.Joke" SelectMethod="SelectJokes" OnItemCommand="JokesList_ItemCommand">
        <EmptyDataTemplate>
            <article>
                <header>
                    <asp:Localize runat="server" Text="Chyba" />
                </header>
                <p>
                    <asp:Localize runat="server" Text="Nebyly nalezeny žádné vtipy odpovídající zadaným parametrům." />
                </p>
            </article>
        </EmptyDataTemplate>
        <ItemTemplate>
            <article id="<%# "j" + Item.JokeId %>">
                <header>
                    <asp:HyperLink runat="server" CssClass="id" Text="<%# Item.JokeId %>" NavigateUrl='<%# this.GetRouteUrl("SingleJoke", new { JokeId = Item.JokeId })%>' />
                    <asp:Literal Text="<%# Item.DateCreated %>" runat="server" />
                </header>
                <asp:Panel runat="server" CssClass="ui-widget-content ui-state-highlight ui-corner-all" Style="padding: .5ex 1ex; margin-bottom: 1ex;" Visible="<%# !Item.Approved %>">
                    <asp:LinkButton Text="[schválit]" ToolTip="Schválit" runat="server" CssClass="ui-icon ui-icon-check" CommandName="Approve" CommandArgument="<%# Item.JokeId %>" />
                    <asp:LinkButton Text="[zamítnout]" ToolTip="Zamítnout" runat="server" CssClass="ui-icon ui-icon-closethick" CommandName="Reject" CommandArgument="<%# Item.JokeId %>" data-confirmprompt="Opravdu chcete tento vtip smazat?" />
                    <asp:Localize runat="server" Text="Tento vtip dosud nebyl schválen ke zveřejnění." />
                </asp:Panel>
                <my:WikiPlexLiteral Text="<%# Item.Text %>" runat="server" />
                <aside>
                    <asp:Panel runat="server" DefaultButton="AddTagButton" Style="float: right" Visible="<%# this.Request.IsAuthenticated %>">
                        <asp:TextBox ID="TextBoxNewTag" runat="server" Width="120px" CssClass="textbox ac-tag" />
                        <asp:LinkButton ID="AddTagButton" runat="server" Text="[přidat]" ToolTip="Přidat tag" CommandName="AddTag" CommandArgument="<%# Item.JokeId %>" CssClass="ui-icon ui-icon-circle-plus" />
                        <asp:LinkButton runat="server" Text="[smazat vše]" ToolTip="Smazat všechny tagy" CommandName="ClearTags" CommandArgument="<%# Item.JokeId %>" CssClass="ui-icon ui-icon-circle-close" data-confirmprompt="Opravdu chcete smazat všechny tagy u tohoto vtipu?" />
                    </asp:Panel>
                    <asp:ListView runat="server" DataSource="<%# Item.Tags %>" ItemType="Altairis.VtipBaze.Data.Tag">
                        <LayoutTemplate>
                            <ul class="taglist">
                                <asp:PlaceHolder runat="server" ID="ItemPlaceHolder" />
                            </ul>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li><span class="ui-icon ui-icon-tag"></span>
                                <asp:HyperLink runat="server" Text="<%# Item.TagName %>" NavigateUrl='<%# this.GetRouteUrl("TagSearch", new { TagName = Item.TagName, PageIndex = "1" }) %>' />
                            </li>
                        </ItemTemplate>
                    </asp:ListView>
                </aside>
            </article>
        </ItemTemplate>
    </asp:ListView>
    <asp:Panel ID="PagingPanel" runat="server" CssClass="paging">
        <div style="float: left">
            <asp:HyperLink ID="PageFirst" runat="server" Text="&lt;&lt; první" />
            <asp:HyperLink ID="PagePrev" runat="server" Text="&lt; předchozí" />
        </div>
        <asp:HyperLink ID="PageNext" runat="server" Text="další &gt;" />
        <asp:HyperLink ID="PageLast" runat="server" Text="poslední &gt;&gt;" />
    </asp:Panel>
    <altairis:DataPagerExtender runat="server" TargetControlID="JokesList" PageSize="5" FirstLinkID="PageFirst" LastLinkID="PageLast" NextLinkID="PageNext" PreviousLinkID="PagePrev" InactiveLinkMode="Disable" />
</asp:Content>
