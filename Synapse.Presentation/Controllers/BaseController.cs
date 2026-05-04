using Microsoft.AspNetCore.Mvc;

namespace Synapse.Presentation.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class BaseController : ControllerBase
{
}