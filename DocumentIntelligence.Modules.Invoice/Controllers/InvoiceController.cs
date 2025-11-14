using DocumentIntelligence.Modules.Invoice.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace DocumentIntelligence.Modules.Invoice.Controllers
{
    /// <summary>
    /// Controlador para operaciones de facturas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class InvoiceController : ControllerBase
    {
        private readonly IAnalyzeInvoiceUseCase _analyzeInvoiceUseCase;
        private readonly ILogger<InvoiceController> _logger;
        const long MAX_FILE_SIZE = 4L * 1024L * 1024L; // 4 Megas tarifa Free

        /// <summary>
        /// Constructor de la clase InvoiceController.
        /// </summary>
        public InvoiceController(ILogger<InvoiceController> logger, IAnalyzeInvoiceUseCase analyzeInvoiceUseCase)
        {
            _logger = logger;
            _analyzeInvoiceUseCase = analyzeInvoiceUseCase;
        }

        /// <summary>
        /// Analiza una factura y devuelve información estructurada.
        /// </summary>
        /// <remarks>
        /// Endpoint que recibe un fichero (imagen o PDF) en multipart/form-data y devuelve
        /// una lista con los campos extraídos de la factura. El tamaño máximo permitido es 4 MB.
        /// </remarks>

        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeInvoice(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No se ha proporcionado ningún fichero a analizar.");

            if (file.Length > MAX_FILE_SIZE)
                return BadRequest("El archivo pesa mas de 4 MB.");

            try
            {
                using var stream = file.OpenReadStream();
                var result = await _analyzeInvoiceUseCase.AnalyzeInvoiceAsync(stream, HttpContext.RequestAborted);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.ErrorMessage);
                }

                return Ok(result.Data);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Request cancelled");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al analizar la factura.");
                return StatusCode(500, "Ocurrió un error al procesar la factura.");
            }
        }
    }
}
