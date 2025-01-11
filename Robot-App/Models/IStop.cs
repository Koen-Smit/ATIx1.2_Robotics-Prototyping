public interface IStop
{
    public string stopMessage { get; set; }
    public string robotStatus { get; set; }
    public Task StartRobot();
    public Task StopRobot();
    public Task HandleStatus(string message);

}