<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="DBUsers.Users" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style>
        .Button
        {
            font:12px Times;

        }

    </style>

</head>
<body>

    <form id="form1" runat="server">
        
        
        




        <asp:GridView ID="GridView1" runat="server" CustomSortField="Id" CustomSortDirection="ASC" AllowPaging="True" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None" PageSize="3" AllowSorting="True" OnSorting="GridView1_Sorting">
            <AlternatingRowStyle BackColor="PaleGoldenrod" />
            <FooterStyle BackColor="Tan" />
            <HeaderStyle BackColor="Tan" Font-Bold="True" />
            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
            <SortedAscendingCellStyle BackColor="#FAFAE7" />
            <SortedAscendingHeaderStyle BackColor="#DAC09E" />
            <SortedDescendingCellStyle BackColor="#E1DB9C" />
            <SortedDescendingHeaderStyle BackColor="#C2A47B" />
        </asp:GridView>
        
        <asp:Repeater runat="server" ID="repeaterpaging">
            <ItemTemplate>
                <asp:LinkButton runat="server" id="linkButton" Text='<%#Eval("Text") %>' 
                    CommandArgument='<%#Eval("Value") %>'
                    Enabled='<%#Eval("Enabled") %>'
                    onclick="linkButton_Click"
                    causesvalidation="false"/>
            </ItemTemplate>
        </asp:Repeater>



        <br />

        <div runat="server" style="font: 12px Times">
            Imię:<br />
            <asp:TextBox runat="server" ID="imie" type="text"  Font-Size="Small" Font-Names="Times"/>
            <asp:RequiredFieldValidator runat="server" ID="rfvimie" ControlToValidate="imie"
                ErrorMessage="Pole 'Imię' jest wymagane." />
            <br />
            Nazwisko:<br />
            <asp:TextBox runat="server" ID="nazwisko" type="text" Font-Size="Small" Font-Names="Times"/>
            <asp:RequiredFieldValidator runat="server" ID="rfvnazwisko" ControlToValidate="nazwisko"
                ErrorMessage="Pole 'Nazwisko' jest wymagane." />
            <br />
            Data urodzenia dd.mm.rrrr lub dd/mm/rrrr:<br />
            <asp:TextBox runat="server" ID="ur1" />
            <asp:RegularExpressionValidator runat="server" ID="rfvur1"
                ControlToValidate="ur1"
                ErrorMessage="Data urodzenia powinna wyglądać następująco: dd.mm.rrrr"
                ValidationExpression="((0[1-9]|[12]\d|3[01])\.(0[1-9]|1[012])\.(19[0-9][0-9]|200[0-9]|201[0-8]))|((0[1-9]|[12]\d|3[01])\/(0[1-9]|1[012])\/(19[0-9][0-9]|200[0-9]|201[0-8]))" />
            <asp:RequiredFieldValidator runat="server" ID="rfvur2" ControlToValidate="ur1"
                ErrorMessage="Pole 'Data Urodzenia' jest wymagane." />
            <br />
            Administrator (0/1):<br />
            <asp:TextBox runat="server" ID="administrator"  Font-Size="Small" Font-Names="Times"/>
            <asp:RegularExpressionValidator runat="server" ID="rfvadm1"
                ControlToValidate="administrator"
                ErrorMessage="Administrator powinien wyglądać następująco: 0 lub 1"
                ValidationExpression="([01])" />
            <asp:RequiredFieldValidator runat="server" ID="rfvadm2" ControlToValidate="administrator"
                ErrorMessage="Pole 'Administrator' jest wymagane." />
            <br />


            <asp:Button runat="server" ID="wyslij" Text="Dodaj" OnClick="dodaj_user" CssClass="Button"/>


        </div>


    </form>
</body>
</html>
