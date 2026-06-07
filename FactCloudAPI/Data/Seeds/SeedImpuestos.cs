// NubeeAPI/Data/Seeds/SeedImpuestos.cs
using NubeeAPI.Models;
using NubeeAPI.Models.Impuestos;
using Microsoft.EntityFrameworkCore;

namespace NubeeAPI.Data.Seeds
{
    public static class SeedImpuestos
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            SeedImpuestosData(modelBuilder);
            SeedAutorretenciones(modelBuilder);
        }

        private static void SeedImpuestosData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Impuesto>().HasData(

                // ── IVA ────────────────────────────────────────────────────────────────────
                // CuentaCreditoVentas  = IVA generado (pasivo 2408)
                // CuentaDebitoCompras  = IVA descontable compras (pasivo 2408 – naturaleza C, actúa como descontable)
                // CuentaDevolucionVentas = Descontable por devoluciones en ventas (2408)
                // CuentaDevolucionCompras = Devolución en compras (2408)

                new Impuesto
                {
                    Id = 1,
                    Codigo = 1,
                    Nombre = "IVA 19%",
                    TipoImpuesto = "IVA",
                    Tarifa = 19.00m,
                    CodigoTributoDIAN = "01",
                    CuentaCreditoVentasId = 10362,  // 24080601 – Iva generado
                    CuentaDebitoComprasId = 10366,  // 24081001 – Iva descontable por compras 19
                    CuentaDevolucionVentasId = 10375,  // 24082001 – Descontable por devoluciones 19
                    CuentaDevolucionComprasId = 10367, // 24081002 – Iva Devolución en compras 19
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 2,
                    Codigo = 2,
                    Nombre = "IVA 5%",
                    TipoImpuesto = "IVA",
                    Tarifa = 5.00m,
                    CodigoTributoDIAN = "01",
                    CuentaCreditoVentasId = 10363,  // 24080602 – Iva generado (5 %)
                    CuentaDebitoComprasId = 10368,  // 24081003 – Iva descontable por compras 5
                    CuentaDevolucionVentasId = 10376,  // 24082002 – Descontable por devoluciones 5
                    CuentaDevolucionComprasId = 10369, // 24081004 – Iva Devolución en compras 5
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 22,
                    Codigo = 22,
                    Nombre = "IVA 0%",
                    TipoImpuesto = "IVA",
                    Tarifa = 0.00m,
                    CodigoTributoDIAN = "01",
                    CuentaCreditoVentasId = 10362,  // mismas cuentas que IVA 19 %
                    CuentaDebitoComprasId = 10366,
                    CuentaDevolucionVentasId = 10375,
                    CuentaDevolucionComprasId = 10367,
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 23,
                    Codigo = 23,
                    Nombre = "IVA 16%",
                    TipoImpuesto = "IVA",
                    Tarifa = 16.00m,
                    CodigoTributoDIAN = "01",
                    CuentaCreditoVentasId = 10364,  // 24080603 – Iva generado 16
                    CuentaDebitoComprasId = 10370,  // 24081005 – Compras iva 16
                    CuentaDevolucionVentasId = 10377,  // 24082003 – Devolución en venta 16
                    CuentaDevolucionComprasId = 10371, // 24081006 – Devolución de compra 16
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },

                // ── Retefuente ─────────────────────────────────────────────────────────────
                // CuentaDebitoVentas  = Anticipo retención (activo 1355xx)
                // CuentaCreditoCompras = Retención por pagar (pasivo 2365xx)

                new Impuesto
                {
                    Id = 3,
                    Codigo = 3,
                    Nombre = "Retefuente 11%",
                    TipoImpuesto = "Retefuente",
                    Tarifa = 11.00m,
                    CodigoTributoDIAN = "05",
                    CuentaDebitoVentasId = 10077,   // 13551509 – Anticipo Ret 11
                    CuentaCreditoComprasId = 10249,  // 23651501 – Honorarios (pasivo)
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 4,
                    Codigo = 4,
                    Nombre = "Retefuente 10%",
                    TipoImpuesto = "Retefuente",
                    Tarifa = 10.00m,
                    CodigoTributoDIAN = "05",
                    CuentaDebitoVentasId = 10075,   // 13551507 – Anticipo Ret 10
                    CuentaCreditoComprasId = 10281,  // 23654001 – Ret por compras 2,5 (genérica)
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 5,
                    Codigo = 5,
                    Nombre = "Retefuente 6%",
                    TipoImpuesto = "Retefuente",
                    Tarifa = 6.00m,
                    CodigoTributoDIAN = "05",
                    CuentaDebitoVentasId = 10073,   // 13551505 – Anticipo Ret 6
                    CuentaCreditoComprasId = 10265,  // 23652501 – Servicios 6
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 6,
                    Codigo = 6,
                    Nombre = "Retefuente 4%",
                    TipoImpuesto = "Retefuente",
                    Tarifa = 4.00m,
                    CodigoTributoDIAN = "05",
                    CuentaDebitoVentasId = 10071,   // 13551503 – Anticipo Ret 4
                    CuentaCreditoComprasId = 10267,  // 23652503 – Servicios 4
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 7,
                    Codigo = 7,
                    Nombre = "Retefuente 2.5%",
                    TipoImpuesto = "Retefuente",
                    Tarifa = 2.50m,
                    CodigoTributoDIAN = "05",
                    CuentaDebitoVentasId = 10069,   // 13551501 – Anticipo Ret 2,5
                    CuentaCreditoComprasId = 10281,  // 23654001 – Ret por compras 2,5
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 18,
                    Codigo = 18,
                    Nombre = "Retefuente 3.5%",
                    TipoImpuesto = "Retefuente",
                    Tarifa = 3.50m,
                    CodigoTributoDIAN = "05",
                    CuentaDebitoVentasId = 10081,   // 13551513 – Anticipo Ret 3,5
                    CuentaCreditoComprasId = 10253,  // 23651505 – Retención 3,5
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 19,
                    Codigo = 19,
                    Nombre = "Retefuente 7%",
                    TipoImpuesto = "Retefuente",
                    Tarifa = 7.00m,
                    CodigoTributoDIAN = "05",
                    CuentaDebitoVentasId = 10079,   // 13551511 – Anticipo Ret 7
                    CuentaCreditoComprasId = 10251,  // 23651503 – Retención 7
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 20,
                    Codigo = 20,
                    Nombre = "Retefuente 2%",
                    TipoImpuesto = "Retefuente",
                    Tarifa = 2.00m,
                    CodigoTributoDIAN = "05",
                    CuentaDebitoVentasId = 10083,   // 13551515 – Anticipo Ret 2
                    CuentaCreditoComprasId = 10255,  // 23651507 – Retención 2
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 21,
                    Codigo = 21,
                    Nombre = "Retefuente 1%",
                    TipoImpuesto = "Retefuente",
                    Tarifa = 1.00m,
                    CodigoTributoDIAN = "05",
                    CuentaDebitoVentasId = 10085,   // 13551517 – Anticipo Ret 1
                    CuentaCreditoComprasId = 10257,  // 23651509 – Retención 1
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                // Tarifas especiales comparten las cuentas genéricas de 2,5 % / compras
                new Impuesto
                {
                    Id = 90,
                    Codigo = 90,
                    Nombre = "Retefuente 1.5%",
                    TipoImpuesto = "Retefuente",
                    Tarifa = 1.50m,
                    CodigoTributoDIAN = "05",
                    CuentaDebitoVentasId = 10069,   // 13551501
                    CuentaCreditoComprasId = 10281,  // 23654001
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 91,
                    Codigo = 91,
                    Nombre = "Retefuente 0.10%",
                    TipoImpuesto = "Retefuente",
                    Tarifa = 0.10m,
                    CodigoTributoDIAN = "05",
                    CuentaDebitoVentasId = 10069,
                    CuentaCreditoComprasId = 10281,
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 92,
                    Codigo = 92,
                    Nombre = "Retefuente 0.50%",
                    TipoImpuesto = "Retefuente",
                    Tarifa = 0.50m,
                    CodigoTributoDIAN = "05",
                    CuentaDebitoVentasId = 10069,
                    CuentaCreditoComprasId = 10281,
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 93,
                    Codigo = 93,
                    Nombre = "Retefuente 20%",
                    TipoImpuesto = "Retefuente",
                    Tarifa = 20.00m,
                    CodigoTributoDIAN = "05",
                    CuentaDebitoVentasId = 10069,
                    CuentaCreditoComprasId = 10281,
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },

                // ── ReteICA ────────────────────────────────────────────────────────────────
                // CuentaDebitoVentas   = Rete-ICA activo (1355xx)
                // CuentaCreditoCompras = Rete-ICA pasivo (23680xxx)

                new Impuesto
                {
                    Id = 8,
                    Codigo = 8,
                    Nombre = "ReteICA 11.04",
                    TipoImpuesto = "ReteICA",
                    Tarifa = 11.04m,
                    CodigoTributoDIAN = "06",
                    CuentaDebitoVentasId = 10095,   // 13551801
                    CuentaCreditoComprasId = 10305,  // 23680501
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 9,
                    Codigo = 9,
                    Nombre = "ReteICA 13.8",
                    TipoImpuesto = "ReteICA",
                    Tarifa = 13.80m,
                    CodigoTributoDIAN = "06",
                    CuentaDebitoVentasId = 10097,   // 13551803
                    CuentaCreditoComprasId = 10307,  // 23680503
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 10,
                    Codigo = 10,
                    Nombre = "ReteICA 9.66",
                    TipoImpuesto = "ReteICA",
                    Tarifa = 9.66m,
                    CodigoTributoDIAN = "06",
                    CuentaDebitoVentasId = 10099,   // 13551805
                    CuentaCreditoComprasId = 10309,  // 23680505
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 11,
                    Codigo = 11,
                    Nombre = "ReteICA 8",
                    TipoImpuesto = "ReteICA",
                    Tarifa = 8.00m,
                    CodigoTributoDIAN = "06",
                    CuentaDebitoVentasId = 10101,   // 13551807
                    CuentaCreditoComprasId = 10311,  // 23680507
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 12,
                    Codigo = 12,
                    Nombre = "ReteICA 7",
                    TipoImpuesto = "ReteICA",
                    Tarifa = 7.00m,
                    CodigoTributoDIAN = "06",
                    CuentaDebitoVentasId = 10103,   // 13551809
                    CuentaCreditoComprasId = 10313,  // 23680509
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 13,
                    Codigo = 13,
                    Nombre = "ReteICA 6.9",
                    TipoImpuesto = "ReteICA",
                    Tarifa = 6.90m,
                    CodigoTributoDIAN = "06",
                    CuentaDebitoVentasId = 10105,   // 13551811
                    CuentaCreditoComprasId = 10315,  // 23680511
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 14,
                    Codigo = 14,
                    Nombre = "ReteICA 4.14",
                    TipoImpuesto = "ReteICA",
                    Tarifa = 4.14m,
                    CodigoTributoDIAN = "06",
                    CuentaDebitoVentasId = 10107,   // 13551813
                    CuentaCreditoComprasId = 10317,  // 23680513
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },

                // ── ReteIVA ────────────────────────────────────────────────────────────────
                // CuentaDebitoVentas   = IVA retenido activo (135517xx)
                // CuentaCreditoCompras = IVA retenido pasivo (236701xx)

                new Impuesto
                {
                    Id = 15,
                    Codigo = 15,
                    Nombre = "ReteIVA 15%",
                    TipoImpuesto = "ReteIVA",
                    Tarifa = 15.00m,
                    CodigoTributoDIAN = "04",
                    CuentaDebitoVentasId = 10089,   // 13551701 – Impuesto a las ventas retenido 15
                    CuentaCreditoComprasId = 10294,  // 23670101 – Impuesto a las ventas retenido 15 (pasivo)
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 29,
                    Codigo = 29,
                    Nombre = "ReteIVA 100%",
                    TipoImpuesto = "ReteIVA",
                    Tarifa = 100.00m,
                    CodigoTributoDIAN = "04",
                    CuentaDebitoVentasId = 10091,   // 13551703 – Impuesto a las ventas retenido 100
                    CuentaCreditoComprasId = 10296,  // 23670103 – Impuesto a las ventas retenido 100 (pasivo)
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },

                // ── Impoconsumo ────────────────────────────────────────────────────────────
                // Cuentas 2495 – Impuesto al consumo nacional

                new Impuesto
                {
                    Id = 16,
                    Codigo = 16,
                    Nombre = "Impoconsumo 8%",
                    TipoImpuesto = "Impoconsumo",
                    Tarifa = 8.00m,
                    CodigoTributoDIAN = "04",  // INC = 04 (estándar DIAN/Factus/CUFE)
                    CuentaCreditoVentasId = 10401,  // 24950101 – Impuesto al consumo en ventas
                    CuentaDebitoComprasId = 10403,  // 24950103 – Impuesto al consumo en compras
                    CuentaDevolucionVentasId = 10402,  // 24950102 – Impuesto al consumo devol ventas
                    CuentaDevolucionComprasId = 10404, // 24950104 – Impuesto al consumo devol compras
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 17,
                    Codigo = 17,
                    Nombre = "Impoconsumo por valor",
                    TipoImpuesto = "Impoconsumo",
                    Tarifa = 0.00m,
                    CodigoTributoDIAN = "04",  // INC = 04 (estándar DIAN/Factus/CUFE)
                    CuentaCreditoVentasId = 10401,
                    CuentaDebitoComprasId = 10403,
                    CuentaDevolucionVentasId = 10402,
                    CuentaDevolucionComprasId = 10404,
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },

                // ── Ad-Valorem ─────────────────────────────────────────────────────────────
                // Ventas/devoluciones en ventas → 2464 (pasivo)
                // Compras/devoluciones en compras → 5115 (gasto)

                new Impuesto
                {
                    Id = 24,
                    Codigo = 24,
                    Nombre = "AdValorem 20%",
                    TipoImpuesto = "Ad-Valorem",
                    Tarifa = 20.00m,
                    CodigoTributoDIAN = "ZY",
                    CuentaCreditoVentasId = 10386,  // 24640501 – Impuesto Ad valorem en ventas 20
                    CuentaDebitoComprasId = 10632,  // 51159502 – Impuesto Ad valorem en compras 20
                    CuentaDevolucionVentasId = 10389,  // 24640601 – Impuesto Ad valorem devol ventas 20
                    CuentaDevolucionComprasId = 10634, // 51159504 – Impuesto Ad valorem devol compras 20
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 25,
                    Codigo = 25,
                    Nombre = "AdValorem 25%",
                    TipoImpuesto = "Ad-Valorem",
                    Tarifa = 25.00m,
                    CodigoTributoDIAN = "ZY",
                    CuentaCreditoVentasId = 10387,  // 24640502 – Impuesto Ad valorem en ventas 25
                    CuentaDebitoComprasId = 10633,  // 51159503 – Impuesto Ad valorem en compras 25
                    CuentaDevolucionVentasId = 10390,  // 24640602 – Impuesto Ad valorem devol ventas 25
                    CuentaDevolucionComprasId = 10635, // 51159505 – Impuesto Ad valorem devol compras 25
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },

                // ── Comestibles ultraprocesados ────────────────────────────────────────────
                // Cuentas 2465xxxx (auxiliares nivel 5, CodigoPadre = 24)

                new Impuesto
                {
                    Id = 94,
                    Codigo = 94,
                    Nombre = "Comestibles ultraprocesados 15%",
                    TipoImpuesto = "Comestibles ultraprocesados",
                    Tarifa = 15.00m,
                    CodigoTributoDIAN = "ZA",
                    CuentaCreditoVentasId = 10391,  // 24651005 – Ventas 15
                    CuentaDebitoComprasId = 10393,  // 24651007 – Compras 15
                    CuentaDevolucionVentasId = 10392,  // 24651006 – Devol ventas 15
                    CuentaDevolucionComprasId = 10394, // 24651008 – Devol compras 15
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Impuesto
                {
                    Id = 95,
                    Codigo = 95,
                    Nombre = "Comestibles ultraprocesados 20%",
                    TipoImpuesto = "Comestibles ultraprocesados",
                    Tarifa = 20.00m,
                    CodigoTributoDIAN = "ZA",
                    CuentaCreditoVentasId = 10395,  // 24651009 – Ventas 20
                    CuentaDebitoComprasId = 10397,  // 24651011 – Compras 20
                    CuentaDevolucionVentasId = 10396,  // 24651010 – Devol ventas 20
                    CuentaDevolucionComprasId = 10398, // 24651012 – Devol compras 20
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                }

            );   // ← cierra HasData
        }        // ← cierra método

        private static void SeedAutorretenciones(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Autoretencion>().HasData(

                new Autoretencion
                {
                    Id = 1,
                    Codigo = 26,
                    Nombre = "Autorretención 0.40%",
                    TipoAutoretencion = "Autoretención 2201",
                    Tarifa = 0.40m,
                    CuentaDebitoId = 10087,   // 13551519 – Autorretención activo
                    CuentaCreditoId = 10290,   // 23657501 – Autorretenciones por pagar
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Autoretencion
                {
                    Id = 2,
                    Codigo = 27,
                    Nombre = "Autorretención 0.80%",
                    TipoAutoretencion = "Autoretención 2201",
                    Tarifa = 0.80m,
                    CuentaDebitoId = 10087,
                    CuentaCreditoId = 10290,
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                },
                new Autoretencion
                {
                    Id = 3,
                    Codigo = 28,
                    Nombre = "Autorretención 1.60%",
                    TipoAutoretencion = "Autoretención 2201",
                    Tarifa = 1.60m,
                    CuentaDebitoId = 10087,
                    CuentaCreditoId = 10290,
                    EnUso = true,
                    FechaCreacion = new DateTime(2025, 1, 1),
                    UsuarioId = null
                }
            );
        }
    }
}
