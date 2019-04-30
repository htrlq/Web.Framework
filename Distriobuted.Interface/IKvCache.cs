namespace Distriobuted.Interface
{
    public interface IKvCache
    {
        TEntity Get<TEntity>(string key)
            where TEntity : class, new();

        void StartCache();
    }

}
