using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebsiteShopping.Models;

namespace WebsiteShopping.Controllers;

public class HomeController : Controller
{
    // No Views/Home folder exists: the home page is Views/Product/index.cshtml
    // (karl/index.html), served by ProductController.Index because the default
    // route is {controller=Product}/{action=Index}. So this controller only keeps
    // the actions that actually have a view.

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
