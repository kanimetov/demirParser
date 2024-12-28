using DemirParser.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemirParser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParserController : ControllerBase
    {
        private readonly ILoggerService _loggerService;

        public ParserController(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        [HttpPost("postXml")]
        [Consumes("application/xml")]
        public async Task<IActionResult> PostXML([FromBody] Data data)
        {
            try
            {
                _loggerService.Log(data);
                return Ok(new { Message = "XML processed successfully", Data = data });
            }
            catch
            {
                return BadRequest("Invalid XML format");
            }
        }
    }
}