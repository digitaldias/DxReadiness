<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PicturificWebSite._Default" %>



<script runat="server">
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
            try
            {
                
                FileUpload1.SaveAs("C:\\Uploads\\" + 
                     FileUpload1.FileName);
                Label1.Text = "File name: " +
                     FileUpload1.PostedFile.FileName + "<br>" +
                     FileUpload1.PostedFile.ContentLength + " kb<br>" +
                     "Content type: " +
                     FileUpload1.PostedFile.ContentType;
            }
            catch (Exception ex)
            {
                Label1.Text = "ERROR: " + ex.Message.ToString();
            }
        else
        {
            Label1.Text = "You have not specified a file.";
        }
    }
</script>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="background-image:url(pics/background-woman.jpg)">
        <h1 style="color:yellow">Upload your picture to picturific</h1>
        <p class="lead">Top Block</p>
        <p>
        <asp:FileUpload ID="FileUpload1" runat="server" />
        </p>
        <hr />
        <p>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Upload picture" />
           </p>
         <p>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </p>
           
    </div>

</asp:Content>
