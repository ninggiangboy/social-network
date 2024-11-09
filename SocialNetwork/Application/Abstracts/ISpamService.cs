namespace Application.Abstracts;

public interface ISpamService
{
    Task<bool> IsSpamAsync(string content);
}