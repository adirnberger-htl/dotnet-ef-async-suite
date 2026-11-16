class Response 
{
    public Response (string name, Languge lang, Formality form, Pronoun pronoun)
    {
        Response.Name = name;
        Response.Lang = lang;
        Response.Form = form;
        Response.Pronoun = pronoun;
    }

    public string Name;
    public Pronoun Pronoun;
    public Language Lang;
    public Farmality Form;

    public string Greeting;
    public string Quote;

    public string getResponse()
    {
        
    }
}