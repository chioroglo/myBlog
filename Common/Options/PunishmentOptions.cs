namespace Common.Options;

public class PunishmentOptions : BaseApplicationOptions
{
    public new static string Config => "PunishmentOptions";
    public int MaxAllowedWarns { get; set; }
}