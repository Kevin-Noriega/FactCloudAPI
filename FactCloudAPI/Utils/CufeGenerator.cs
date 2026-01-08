using System.Security.Cryptography;
using System.Text;

namespace FactCloudAPI.Utils
{
    public static class CufeGenerator
    {
        public static string GenerarCUFE(
            string numeroFactura,
            string fechaFactura,
            string horaFactura,
            string valorTotal,
            string valorIVA,
            string nitEmisor,
            string nitAdquiriente,
            string claveTecnica)
        {
            // Cadena según DIAN UBL 2.1
            string cadena =
                $"NumFac={numeroFactura}" +
                $"&FecFac={fechaFactura}" +
                $"&HorFac={horaFactura}" +
                $"&ValFac={valorTotal}" +
                $"&ValIVA={valorIVA}" +
                $"&NITEmi={nitEmisor}" +
                $"&NITAdq={nitAdquiriente}" +
                $"&ClavTec={claveTecnica}";

            using (SHA384 sha384 = SHA384.Create())
            {
                byte[] hashBytes = sha384.ComputeHash(Encoding.UTF8.GetBytes(cadena));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
