namespace Porfolio.Code.Entidades;


public class Locale
{
    public string Country { get; set; }
    public string Language { get; set; }
}

public class User
{
    public string Sub { get; set; }
    public bool EmailVerified { get; set; }
    public string Name { get; set; }
    public Locale Locale { get; set; }
    public string GivenName { get; set; }
    public string FamilyName { get; set; }
    public string Email { get; set; }
    public string Picture { get; set; }
}


