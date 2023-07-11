<%@ Page Title="Přihlášení" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Altairis.VtipBaze.WebCore.Pages.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainCPH" runat="server">
    <article>
        <header>
            <asp:Localize runat="server" Text="Přihlášení" />
        </header>
        <asp:Login runat="server" RenderOuterTable="false" FailureText="<p><b>Chyba:</b> Přihlášení se nezdařilo.</p>">
            <LayoutTemplate>
                <asp:Literal ID="FailureText" runat="server" />
                <div class="form">
                    <p>
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Text="Uživatelské jméno:" />
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="Není zadáno uživatelské jméno" ValidationGroup="login" Display="None" />
                        <br />
                        <asp:TextBox ID="UserName" runat="server" TextMode="SingleLine" Width="100%" />
                    </p>
                    <p>
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Text="Heslo" />
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Není zadáno heslo" ValidationGroup="login" Display="None" />
                        <br />
                        <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="100%" />
                    </p>
                    <p>
                        <asp:CheckBox ID="RememberMe" runat="server" Text="pamatuj si mne na tomto počítači" Checked="true" />
                    </p>
                    <asp:ValidationSummary runat="server" ValidationGroup="login" />
                    <p class="buttons">
                        <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Odeslat" ValidationGroup="login" />
                    </p>
                </div>
            </LayoutTemplate>
        </asp:Login>
    </article>
</asp:Content>
