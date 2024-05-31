namespace Porfolio.Code.Entidades;

public class OAuth2
{
    public string grant_type { get; set; }
    public string code { get; set; }
    public string client_id { get; set; }
    public string client_secret { get; set; }
    public string redirect_uri { get; set; }
    public string scope { get; set; }
}

public class OAuth2Services
{
    public OAuth2 OAuth2 { get; set; }
}
