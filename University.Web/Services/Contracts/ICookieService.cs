namespace University.Web.Services.Contracts
{
    public interface ICookieService
    {
        string GetBearerToken();
        void RemoveBearerToken();
        void SetBearerToken(string token);
    }
}