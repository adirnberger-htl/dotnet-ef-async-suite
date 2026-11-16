static class CommandParser 
{
    public string verifyCommand()
    {
        string pattern = @"(?<flag>-[nifl])(?:\s+(?:""(?<val>[^""]+)""|(?<val>[^\s-]+)))?";
    }

    public Response createResp()
    {

    }
}