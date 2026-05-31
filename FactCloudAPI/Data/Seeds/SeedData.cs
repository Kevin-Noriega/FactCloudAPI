// NubeeAPI/Data/Seeds/SeedData.cs
using Microsoft.EntityFrameworkCore;

namespace NubeeAPI.Data.Seeds
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            SeedCuentasPUC.Seed(modelBuilder);
            SeedImpuestos.Seed(modelBuilder);
        }
    }
}