using NubeeAPI.DTOs.Habilitacion;
using NubeeAPI.Models;

namespace NubeeAPI.Services.Habilitacion
{
    /// <summary>
    /// Contrato del servicio de habilitación electrónica.
    /// Toda la lógica de negocio de los pasos 1-6 pasa por aquí.
    /// El controller solo orquesta: valida JWT, llama al servicio, devuelve HTTP.
    /// </summary>
    public interface IHabilitacionService
    {
        // ── Paso 1: Perfil empresa ────────────────────────────────────
        Task<PerfilEmpresaDto?> ObtenerPerfilEmpresaAsync(int usuarioId);
        Task ActualizarPerfilEmpresaAsync(int usuarioId, PerfilEmpresaDto dto);

        // ── Paso 1b: Software DIAN ────────────────────────────────────
        Task GuardarSoftwareAsync(int usuarioId, ConfiguracionSoftwareDto dto);

        // ── Paso 2: Certificado digital ───────────────────────────────
        Task<CertificadoResponseDto> GuardarCertificadoAsync(int usuarioId, CertificadoDto dto);
        Task<CertificadoResponseDto?> ObtenerCertificadoAsync(int usuarioId);

        // ── Paso 3: Test Set DIAN ─────────────────────────────────────
        Task GuardarTestSetAsync(int usuarioId, TestSetDto dto);

        // ── Paso 4: Resolución DIAN ───────────────────────────────────
        Task<ResolucionDIAN> GuardarResolucionAsync(int usuarioId, ResolucionDianDto dto);

        // ── Paso 4b: Registrar rango en Factus ───────────────────────
        Task<object> RegistrarRangoFactusAsync(int usuarioId, RegistrarRangoDto dto);

        // ── Paso 5: Consultar rango activo ────────────────────────────
        Task<RangoActivoResponseDto?> ObtenerRangoActivoAsync(int usuarioId);

        // ── Paso 6: Estado de habilitación ────────────────────────────
        Task<EstadoHabilitacionDto> ObtenerEstadoAsync(int usuarioId);
        Task FinalizarHabilitacionAsync(int usuarioId);
    }
}
