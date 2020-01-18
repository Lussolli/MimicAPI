using Microsoft.AspNetCore.Mvc;

namespace MimicAPI.V2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    // [Route("api/[controller]")]
    [ApiVersion("2.0")]
    public class PalavrasController : ControllerBase
    {
        [HttpGet("", Name = "ObterTodas")]
        public string ObterTodas()
        {
            return "Versão 2.0";
        }
    }
}
