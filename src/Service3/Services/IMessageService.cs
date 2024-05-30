namespace Service3.Services;

public interface IMessageService
{
    Task Send(string message);
}
