using DocumentIntelligence.Modules.Receipt.Application.Interfaces;
using DocumentIntelligence.Modules.Receipt.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace DocumentIntelligence.Modules.Receipt.Controllers
{
    /// <summary>
    /// Controlador para operaciones de recibos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ReceiptController : ControllerBase
    {
        private readonly IAnalyzeReceiptUseCase _analyzeReceiptUseCase;
        private readonly ILogger<ReceiptController> _logger;
        const long MAX_FILE_SIZE = 4L * 1024L * 1024L; // 4 Megas tarifa Free

        /// <summary>
        /// Constructor de la clase ReceiptController.
        /// </summary>
        public ReceiptController(ILogger<ReceiptController> logger, IAnalyzeReceiptUseCase analyzeReceiptUseCase)
        {
            _logger = logger;
            _analyzeReceiptUseCase = analyzeReceiptUseCase;
        }

        /// <summary>
        /// Analiza un recibo y devuelve información estructurada.
        /// </summary>
        /// <remarks>
        /// Endpoint que recibe un fichero (imagen o PDF) en multipart/form-data y devuelve
        /// una lista con los campos extraídos del recibo. El tamaño máximo permitido es 4 MB.
        /// </remarks>

        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeReceipt(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No se ha proporcionado ningún fichero a analizar.");

            if (file.Length > MAX_FILE_SIZE)
                return BadRequest("El archivo pesa mas de 4 MB.");

            try
            {
                using var stream = file.OpenReadStream();
                var result = await _analyzeReceiptUseCase.AnalyzeReceiptAsync(stream, HttpContext.RequestAborted);

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
                _logger.LogError(ex, "Error al analizar el recibo.");
                return StatusCode(500, "Ocurrió un error al procesar el recibo.");
            }
        }
    }
}
