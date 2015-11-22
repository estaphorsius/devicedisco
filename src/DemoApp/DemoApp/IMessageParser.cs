namespace DemoApp
{
    public interface IMessageParser
    {
        Message Parse(string rawMessage);
    }
}