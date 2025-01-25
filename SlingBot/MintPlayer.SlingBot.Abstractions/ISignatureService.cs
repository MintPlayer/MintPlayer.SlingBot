namespace MintPlayer.SlingBot.Abstractions;

public interface ISignatureService
{
    bool VerifySignature(string signature, string? secret, string requestBody);
}
