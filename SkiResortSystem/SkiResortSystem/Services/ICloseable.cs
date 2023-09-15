namespace SkiResortSystem.Services
{
    public interface ICloseable
    {
        void Close();
        bool? DialogResult { get; set; }
    }
}
