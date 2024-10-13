namespace Service.Abstract;

public interface IUserPunishmentsService
{
    Task Ban(int userId, string reason, CancellationToken ct = default);
    Task Unban(int userId, CancellationToken ct = default);
    Task Warn(int userId, string reason, CancellationToken ct = default);
    Task Unwarn(int warnId, CancellationToken ct = default);
}