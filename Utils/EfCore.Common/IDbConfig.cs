namespace EfCore.Common
{
    public interface IDbConfig
    {
        string ConnectionString { get; set; }
        
        DbType? DbType { get; set; }
    }
}