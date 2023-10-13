﻿namespace BancaApi.Utility
{
    public class JwtOptions
    {
        public string Issuer { get; init; } = string.Empty;

        public string Audience { get; init; } = string.Empty;

        public string Key { get; set; } = string.Empty;
    }
}
