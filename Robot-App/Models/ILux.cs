public interface ILux
{
    List<Lux> luxes { get; set; }
    void GetluxFromMqtt(string message);
    Task LoadLuxes();
    List<Lux> GetLuxes();
    Task InsertLux(Lux lux);
}