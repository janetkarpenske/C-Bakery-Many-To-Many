using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Bakery.Models;
using System.Threading.Tasks; //allows async
using Bakery.ViewModels;

namespace Bakery.Controllers
{
    public class AccountController : Controller
    {
        private readonly BakeryContext _db;
        private readonly UserManager<ApplicationUser> _userManager; //userManager handles saving and updating user info
        private readonly SignInManager<ApplicationUser> _signInManager; //signInManager handles signing in

        public AccountController (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, BakeryContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost] //post method for registering an account is async because it needs the user to add info before it can do anything (obviously)
        public async Task<ActionResult> Register (RegisterViewModel model)
        {
            var user = new ApplicationUser { UserName = model.Email };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password); //note: Identity comes with default requirements for passwords yay
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

    public ActionResult Login() //next two allow user to login
{
    return View();
}

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return RedirectToAction("Index");
        }
        else
        {
            return View();
        }
    }

    [HttpPost] //allows user to log off
public async Task<ActionResult> LogOff()
{
    await _signInManager.SignOutAsync();
    return RedirectToAction("Index");
}
    }
}