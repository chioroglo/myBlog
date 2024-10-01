﻿using Common.Dto.Auth;
using Common.Models.Passkey;
using Domain;

namespace Service.Abstract.Auth.Passkeys;

public interface IPasskeyAuthService
{
    Task<PasskeyRegistrationOptionsModel> GetOrCreateRegistrationSession(int userId, CancellationToken ct);
    Task Register(RegisterPasskeyRequest request, int userId, CancellationToken ct);
    Task<PasskeyAuthenticationOptionsModel> StartAuthenticationSession(CancellationToken ct);
    Task<AuthenticateResponse> Authenticate(AuthenticatePasskeyRequest request, CancellationToken ct);
}