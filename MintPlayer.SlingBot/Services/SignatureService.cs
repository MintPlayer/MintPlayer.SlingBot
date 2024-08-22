﻿using MintPlayer.SlingBot.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace MintPlayer.SlingBot.Services;

internal class SignatureService : ISignatureService
{
    public bool VerifySignature(string signature, string? secret, string requestBody)
    {
        if (string.IsNullOrEmpty(secret))
        {
            return true;
        }

        var keyBytes = Encoding.UTF8.GetBytes(secret);
        var bodyBytes = Encoding.UTF8.GetBytes(requestBody);

        var hash = HMACSHA256.HashData(keyBytes, bodyBytes);
        var hashHex = Convert.ToHexString(hash);
        var expectedHeader = $"sha256={hashHex.ToLower(System.Globalization.CultureInfo.InvariantCulture)}";
        return (signature == expectedHeader);
    }
}
