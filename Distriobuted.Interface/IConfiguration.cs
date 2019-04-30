namespace Distriobuted.Interface
{
    public interface IConfiguration<TConfiguration>
        where TConfiguration:class,new()
    {
        void SetConfiguration(TConfiguration configuration);
        TConfiguration Value { get; }
    }
}
