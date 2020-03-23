using Microsoft.EntityFrameworkCore;

namespace EasyRates.Model.Ef.Pg
{
    public static class ModelBuilderExtensions 
    {
        public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.DisplayName());
            }
        }
    }
}