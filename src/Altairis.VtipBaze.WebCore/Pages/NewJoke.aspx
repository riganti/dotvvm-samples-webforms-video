<%@ Page Title="Nový vtip" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="NewJoke.aspx.cs" Inherits="Altairis.VtipBaze.WebCore.Pages.NewJoke" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainCPH" runat="server">
    <article>
        <header>
            <asp:Localize runat="server" Text="Přidat nový vtip" />
        </header>
        <asp:MultiView ID="MultiViewPage" ActiveViewIndex="0" runat="server">
            <asp:View ID="ViewForm" runat="server">
                <p>
                    <asp:Localize runat="server" Text="Zde můžete zadat text nového vtipu, který chcete ve VtipBázi publikovat. Vtip bude publikován až po schválení editorem." />
                </p>
                <p>
                    <asp:TextBox ID="TextTextBox" runat="server" TextMode="MultiLine" Width="100%" Height="200px" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TextTextBox" Display="None" ErrorMessage="Není zadán text vtipu" />
                </p>
                <asp:LoginView runat="server">
                    <AnonymousTemplate>
                        <p>
                            <altairis:ReCaptchaImage runat="server" ID="ReCaptchaImage" PrivateKey="<%$ AppSettings:ReCaptchaPrivateKey %>" PublicKey="<%$ AppSettings:ReCaptchaPublicKey %>" Theme="clean" />
                            <altairis:ReCaptchaValidator runat="server" ControlToValidate="ReCaptchaImage" Display="None" ErrorMessage="Není správně opsán text z obrázku" />
                        </p>
                    </AnonymousTemplate>
                </asp:LoginView>
                <asp:ValidationSummary runat="server" />
                <p>
                    <asp:Button ID="ButtonSubmit" runat="server" Text="Odeslat" OnClick="ButtonSubmit_Click" />
                </p>
            </asp:View>
            <asp:View ID="ViewResult" runat="server">
                <p>
                    <asp:Localize runat="server" Text="Váš vtip byl přidán do databáze a bude publikován po schválení editorem. Děkujeme." />
                </p>
            </asp:View>
        </asp:MultiView>
    </article>
</asp:Content>
