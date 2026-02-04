using FactCloudAPI.Data;
using FactCloudAPI.DTOs.DocumentoSoporte;
using FactCloudAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FactCloudAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentosSoporteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IDocumentoSoporteService _documentoService;
        private readonly IConfiguration _configuration;

        public DocumentosSoporteController(
            ApplicationDbContext context,
            IDocumentoSoporteService documentoService,
            IConfiguration configuration)
        {
            _context = context;
            _documentoService = documentoService;
            _configuration = configuration;
        }

        // GET: api/DocumentosSoporte
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentoSoporteResponseDto>>> GetDocumentosSoporte()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var documentos = await _context.DocumentosSoporte
                .Where(d => d.UsuarioId == usuarioId && d.Estado)
                .OrderByDescending(d => d.FechaGeneracion)
                .Select(d => new DocumentoSoporteResponseDto
                {
                    Id = d.Id,
                    NumeroDocumento = d.NumeroDocumento,
                    CUDS = d.CUDS,
                    FechaGeneracion = d.FechaGeneracion,
                    ProveedorNombre = d.ProveedorNombre,
                    ProveedorNit = d.ProveedorNit,
                    ProveedorTipoIdentificacion = d.ProveedorTipoIdentificacion,
                    Descripcion = d.Descripcion,
                    Subtotal = d.Subtotal,
                    IVA = d.IVA,
                    Descuento = d.Descuento,
                    ValorTotal = d.ValorTotal,
                    Observaciones = d.Observaciones,
                    EstadoDian = d.EstadoDian,
                    MensajeDian = d.MensajeDian,
                    FechaRespuestaDian = d.FechaRespuestaDian
                })
                .ToListAsync();

            return Ok(documentos);
        }

        // GET: api/DocumentosSoporte/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentoSoporteResponseDto>> GetDocumentoSoporte(int id)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var documento = await _context.DocumentosSoporte
                .Where(d => d.Id == id && d.UsuarioId == usuarioId && d.Estado)
                .Select(d => new DocumentoSoporteResponseDto
                {
                    Id = d.Id,
                    NumeroDocumento = d.NumeroDocumento,
                    CUDS = d.CUDS,
                    FechaGeneracion = d.FechaGeneracion,
                    ProveedorNombre = d.ProveedorNombre,
                    ProveedorNit = d.ProveedorNit,
                    ProveedorTipoIdentificacion = d.ProveedorTipoIdentificacion,
                    Descripcion = d.Descripcion,
                    Subtotal = d.Subtotal,
                    IVA = d.IVA,
                    Descuento = d.Descuento,
                    ValorTotal = d.ValorTotal,
                    Observaciones = d.Observaciones,
                    EstadoDian = d.EstadoDian,
                    MensajeDian = d.MensajeDian,
                    FechaRespuestaDian = d.FechaRespuestaDian
                })
                .FirstOrDefaultAsync();

            if (documento == null)
                return NotFound("Documento soporte no encontrado");

            return Ok(documento);
        }

        // POST: api/DocumentosSoporte
        [HttpPost]
        public async Task<ActionResult<DocumentoSoporteResponseDto>> CreateDocumentoSoporte(
            [FromBody] DocumentoSoporteCreateDto documentoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var usuario = await _context.Usuarios.FindAsync(usuarioId);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            // Obtener el último consecutivo
            var ultimoConsecutivo = await _context.DocumentosSoporte
                .Where(d => d.UsuarioId == usuarioId)
                .MaxAsync(d => (int?)d.Consecutivo) ?? 0;

            var nuevoConsecutivo = ultimoConsecutivo + 1;
            var prefijo = "DS";

            // Calcular subtotal
            var subtotal = documentoDto.ValorTotal - documentoDto.IVA + documentoDto.Descuento;

            // Generar CUDS
            var nitSoftware = _configuration["DIAN:NitSoftwareProvider"] ?? "900123456";
            var pinSoftware = _configuration["DIAN:PinSoftware"] ?? "12345";

            var cuds = _documentoService.GenerarCUDS(
                prefijo,
                nuevoConsecutivo,
                DateTime.Now,
                usuario.NitNegocio ?? usuario.NumeroIdentificacion ?? "",
                documentoDto.ProveedorNit,
                documentoDto.ValorTotal,
                nitSoftware,
                pinSoftware
            );

            // Crear documento
            var documento = new Models.DocumentoSoporte
            {
                Prefijo = prefijo,
                Consecutivo = nuevoConsecutivo,
                NumeroDocumento = $"{prefijo}{nuevoConsecutivo:D10}",
                CUDS = cuds,
                FechaGeneracion = DateTime.Now,
                ProveedorNombre = documentoDto.ProveedorNombre,
                ProveedorNit = documentoDto.ProveedorNit,
                ProveedorTipoIdentificacion = documentoDto.ProveedorTipoIdentificacion,
                ProveedorDireccion = documentoDto.ProveedorDireccion,
                ProveedorCiudad = documentoDto.ProveedorCiudad,
                ProveedorDepartamento = documentoDto.ProveedorDepartamento,
                ProveedorEmail = documentoDto.ProveedorEmail,
                ProveedorTelefono = documentoDto.ProveedorTelefono,
                Descripcion = documentoDto.Descripcion,
                Subtotal = subtotal,
                IVA = documentoDto.IVA,
                Descuento = documentoDto.Descuento,
                ValorTotal = documentoDto.ValorTotal,
                Observaciones = documentoDto.Observaciones,
                UsuarioId = usuarioId,
                EstadoDian = "Pendiente",
                Estado = true,
                FechaRegistro = DateTime.Now
            };

            _context.DocumentosSoporte.Add(documento);
            await _context.SaveChangesAsync();

            // Generar XML
            try
            {
                var xmlContent = _documentoService.GenerarXML(documento, usuario);
                var carpetaXML = Path.Combine(Directory.GetCurrentDirectory(), "documentos", "xml");
                Directory.CreateDirectory(carpetaXML);

                var rutaXML = Path.Combine(carpetaXML, $"{documento.CUDS}.xml");
                await System.IO.File.WriteAllTextAsync(rutaXML, xmlContent);
                documento.RutaXML = rutaXML;

                // Enviar a DIAN (simulado por ahora)
                var enviado = await _documentoService.EnviarADIAN(documento, xmlContent);
                if (enviado)
                {
                    documento.EstadoDian = "Aceptado";
                    documento.FechaRespuestaDian = DateTime.Now;
                    documento.MensajeDian = "Documento aceptado por DIAN";
                }
            }
            catch (Exception ex)
            {
                documento.EstadoDian = "Error";
                documento.MensajeDian = $"Error al procesar: {ex.Message}";
            }

            await _context.SaveChangesAsync();

            var response = new DocumentoSoporteResponseDto
            {
                Id = documento.Id,
                NumeroDocumento = documento.NumeroDocumento,
                CUDS = documento.CUDS,
                FechaGeneracion = documento.FechaGeneracion,
                ProveedorNombre = documento.ProveedorNombre,
                ProveedorNit = documento.ProveedorNit,
                ProveedorTipoIdentificacion = documento.ProveedorTipoIdentificacion,
                Descripcion = documento.Descripcion,
                Subtotal = documento.Subtotal,
                IVA = documento.IVA,
                Descuento = documento.Descuento,
                ValorTotal = documento.ValorTotal,
                EstadoDian = documento.EstadoDian,
                MensajeDian = documento.MensajeDian
            };

            return CreatedAtAction(nameof(GetDocumentoSoporte), new { id = documento.Id }, response);
        }

        // PUT: api/DocumentosSoporte/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocumentoSoporte(int id,
            [FromBody] DocumentoSoporteCreateDto documentoDto)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var documento = await _context.DocumentosSoporte
                .FirstOrDefaultAsync(d => d.Id == id && d.UsuarioId == usuarioId);

            if (documento == null)
                return NotFound("Documento soporte no encontrado");

            if (documento.EstadoDian == "Aceptado")
                return BadRequest("No se puede editar un documento aceptado por DIAN");

            // Actualizar campos
            documento.ProveedorNombre = documentoDto.ProveedorNombre;
            documento.ProveedorNit = documentoDto.ProveedorNit;
            documento.ProveedorTipoIdentificacion = documentoDto.ProveedorTipoIdentificacion;
            documento.ProveedorDireccion = documentoDto.ProveedorDireccion;
            documento.ProveedorCiudad = documentoDto.ProveedorCiudad;
            documento.ProveedorDepartamento = documentoDto.ProveedorDepartamento;
            documento.ProveedorEmail = documentoDto.ProveedorEmail;
            documento.ProveedorTelefono = documentoDto.ProveedorTelefono;
            documento.Descripcion = documentoDto.Descripcion;
            documento.Subtotal = documentoDto.ValorTotal - documentoDto.IVA + documentoDto.Descuento;
            documento.IVA = documentoDto.IVA;
            documento.Descuento = documentoDto.Descuento;
            documento.ValorTotal = documentoDto.ValorTotal;
            documento.Observaciones = documentoDto.Observaciones;
            documento.FechaActualizacion = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/DocumentosSoporte/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumentoSoporte(int id)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var documento = await _context.DocumentosSoporte
                .FirstOrDefaultAsync(d => d.Id == id && d.UsuarioId == usuarioId);

            if (documento == null)
                return NotFound("Documento soporte no encontrado");

            if (documento.EstadoDian == "Aceptado")
                return BadRequest("No se puede eliminar un documento aceptado por DIAN");

            documento.Estado = false;
            documento.FechaActualizacion = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/DocumentosSoporte/5/pdf
        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> DescargarPDF(int id)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var documento = await _context.DocumentosSoporte
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(d => d.Id == id && d.UsuarioId == usuarioId);

            if (documento == null)
                return NotFound("Documento soporte no encontrado");

            var pdfBytes = _documentoService.GenerarPDF(documento, documento.Usuario);

            return File(pdfBytes, "application/pdf", $"DS-{documento.NumeroDocumento}.pdf");
        }

        // GET: api/DocumentosSoporte/5/xml
        [HttpGet("{id}/xml")]
        public async Task<IActionResult> DescargarXML(int id)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var documento = await _context.DocumentosSoporte
                .FirstOrDefaultAsync(d => d.Id == id && d.UsuarioId == usuarioId);

            if (documento == null)
                return NotFound("Documento soporte no encontrado");

            if (string.IsNullOrEmpty(documento.RutaXML) || !System.IO.File.Exists(documento.RutaXML))
                return NotFound("Archivo XML no encontrado");

            var xmlContent = await System.IO.File.ReadAllBytesAsync(documento.RutaXML);

            return File(xmlContent, "application/xml", $"DS-{documento.NumeroDocumento}.xml");
        }

        // POST: api/DocumentosSoporte/5/enviar-email
        [HttpPost("{id}/enviar-email")]
        public async Task<IActionResult> EnviarEmail(int id)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var documento = await _context.DocumentosSoporte
                .FirstOrDefaultAsync(d => d.Id == id && d.UsuarioId == usuarioId);

            if (documento == null)
                return NotFound("Documento soporte no encontrado");

            if (string.IsNullOrEmpty(documento.ProveedorEmail))
                return BadRequest("El proveedor no tiene email registrado");

            // TODO: Implementar envío de email real
            // Por ahora retorna éxito

            return Ok(new { mensaje = "Email enviado exitosamente" });
        }
    }
}
