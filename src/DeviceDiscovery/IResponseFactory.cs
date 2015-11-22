namespace DeviceDiscovery
{
    public interface IResponseFactory
    {
        string CreateResponse(string requestString);
    }
}