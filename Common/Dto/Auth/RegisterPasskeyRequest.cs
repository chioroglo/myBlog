using System.ComponentModel.DataAnnotations;

namespace Common.Dto.Auth;

public record RegisterPasskeyRequest
{
    [Required]
    public string Id { get; init; }
    [Required]
    public string RawId { get; init; }
    [Required]
    public string AttestationObject { get; init; }
    [Required]
    public string ClientDataJson { get; set; }
    [Required]
    public string Type { get; init; }
}