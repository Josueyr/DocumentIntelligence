namespace DocumentIntelligence.Api.Auth.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string subject);
    }
}
