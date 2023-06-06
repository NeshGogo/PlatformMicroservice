using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/c/[Controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
  public PlatformsController()
  {

  }


  [HttpPost]
  public ActionResult TestConnection(){
    Console.WriteLine("--> Inbound POST # Command Service");
    return Ok();
  }
}