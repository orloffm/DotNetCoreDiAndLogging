using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApplication.Controllers
{
[Route("api/[controller]")]
public class AnimalController : Controller
{
    [HttpGet]
    public string Get()
    {
        return "temp";
    }
}
}
