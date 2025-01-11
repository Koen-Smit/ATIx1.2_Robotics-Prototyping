public interface IBattery
{
    public List<Battery> batteries { get; set; }
    public void GetBatteryFromMqtt(string message);
    public void InsertBattery(Battery battery);
    public List<Battery> GetBattery();
    public void DeleteBattery(int id);
    public Task LoadBatteries();
    public string GetBatteryColor(int percentage);
}

 