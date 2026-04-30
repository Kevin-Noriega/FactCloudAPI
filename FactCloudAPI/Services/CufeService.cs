using NubeeAPI.Models;
using System.Security.Cryptography;
using System.Text;
namespace NubeeAPI.Services
{
    public class CufeService
    {
        public static string GenerarCUFE(Factura factura)
        {
            string data = $"{factura.Id}-{factura.FechaEmision:yyyy-MM-ddTHH:mm:ss}-{factura.TotalFactura}";
            using (SHA384 sha = SHA384.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
