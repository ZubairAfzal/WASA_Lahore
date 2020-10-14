<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Template.aspx.cs" Inherits="WASA_EMS.Template" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title></title>
</head>
<script src="Scripts/jquery-1.7.2.min.js"></script>
<body>
    <form id="form2" runat="server" style="flex-direction: column">
        <div class="main">
            <div class="container">
                <div class="span12">
                    <div class="widget stacked">
                        <div class="widget-content">
                            <div class="container" dir="auto" style="margin-left: 30px; margin-top: 50px; text-align: left; align-content: center">

                                <div>
                                    <asp:HyperLink ID="hyperlink1"
                                        NavigateUrl="~/Home/Index"
                                        Text="Go to Home"
                                        runat="server" />
                                </div>

                                <div>
                                    <h1>Template</h1>
                                </div>
                                <p>&nbsp;</p>
                                <div id="divTemp" runat="server">
                                    <div>
                                        <asp:Label ID="lbl1" runat="server" Text="Template Name"></asp:Label>
                                        &nbsp;&nbsp;&nbsp; &nbsp;
            <asp:TextBox ID="txtTempName" runat="server" Width="160px"></asp:TextBox>
                                        <br />
                                        <br />
                                        <asp:Label ID="lbl2" runat="server" Text="Total Parameters"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtParameterCount" runat="server" Width="160px"></asp:TextBox>
                                        <br />
                                        <br />
                                        <asp:Label ID="lbl3" runat="server" Text="Separated By"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtSeparator" runat="server" Width="160px" MaxLength="1" CausesValidation="true"></asp:TextBox>
                                    </div>
                                    <p>&nbsp;</p>
                                    <div>
                                        <asp:Button ID="btnTempSelect" runat="server" Text="Next" OnClick="btnTempSelect_Click" />
                                    </div>
                                </div>

                                <div id="divlist" runat="server" style="text-align: left; margin-top: 20px; width: 1000px;" dir="auto" visible="False">
                                    <div style="width: 200px; height: 300px; float: left">
                                        <asp:ListBox runat="server" ID="listbox1" Height="200px" Width="150px" Style="margin-top: 20px; margin-left: 40px" SelectionMode="Multiple" DataSourceID="dsParameters" DataTextField="ParameterName" DataValueField="ParameterOrder"></asp:ListBox>
                                        <asp:SqlDataSource ID="dsParameters" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT [ParameterName], [ParameterOrder] FROM [tblParameter] WHERE ([CompanyID] = @CompanyID)">
                                            <SelectParameters>
                                                <asp:SessionParameter DefaultValue="0" Name="CompanyID" SessionField="CompanyID" Type="Int32" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>


                                    </div>

                                    <div style="width: 60px; height: 300px; float: left">
                                        <div style="text-align: center; vertical-align: central; margin-top: 90px">
                                            <asp:ImageButton ID="selectOne" runat="server" ImageUrl="~/Images/rightArrow.png" Width="20px" OnClick="selectOne_Click" />
                                            <br />
                                            <asp:ImageButton ID="deselectOne" runat="server" ImageUrl="~/IMages/leftArrow.png" Width="20px" Style="margin-top: 5px" OnClick="deselectOne_Click" />
                                        </div>
                                    </div>

                                    <div style="width: 180px; height: 300px; float: left">
                                        <asp:ListBox runat="server" ID="listbox2" Height="200px" Width="150px" Style="margin-top: 20px; margin-left: 15px" OnSelectedIndexChanged="listbox2_SelectedIndexChanged" SelectionMode="Multiple"></asp:ListBox>
                                    </div>

                                    <div style="width: 60px; height: 300px; float: left">
                                        <div style="text-align: center; vertical-align: central; margin-top: 90px">
                                            <asp:ImageButton ID="btnmoveUp" runat="server" ImageUrl="~/Images/upArrow.png" Width="20px" OnClick="ImageButton1_Click" />
                                            <br />
                                            <asp:ImageButton ID="btnmoveDown" runat="server" ImageUrl="~/IMages/downArrow.png" Width="20px" Style="margin-top: 5px" OnClick="btnmoveDown_Click" />
                                        </div>
                                    </div>

                                    <div style="width: 60px; height: 300px; float: left">
                                        <div style="text-align: center; vertical-align: central; margin-top: 90px">
                                            <asp:Button ID="btnOk" runat="server" Text="Proceed" OnClick="btnOk_Click" Width="90px" />
                                        </div>

                                        <div style="text-align: center; vertical-align: central; margin-top: 20px">
                                            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" Width="90px" />
                                        </div>

                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </form>
</body>
</html>
