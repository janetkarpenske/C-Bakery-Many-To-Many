using Microsoft.AspNetCore.Mvc;
using Bakery.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Authorization; //allows us to authorize users
using Microsoft.AspNetCore.Identity; //allows interactions with users from database
using System.Threading.Tasks; //needed for asyncs tasks
using System.Security.Claims; //need for claim-based authentication. Claim is who user is, not what they can do.

namespace Bakery.Controllers
{
  public class TreatsController : Controller
  {
    private readonly BakeryContext _db;
    private readonly UserManager<ApplicationUser> _userManager; //new line

    //updated constructor
    public TreatsController(UserManager<ApplicationUser> userManager, BakeryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

// public async Task<ActionResult> Index() //updated for authentication
// {
//     var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //"this" refers to item controller itself. "?" is an existential operator- only does code to right if code to left does not return null.
//     var currentUser = await _userManager.FindByIdAsync(userId);
//     var userTreats = _db.Treats.Where(entry => entry.User.Id == currentUser.Id).ToList();
//     return View(userTreats);
// }
  public ActionResult Index()
  {
    List<Treat> model = _db.Treats.ToList();
      return View(model);
  }
public ActionResult Create()
{
    //ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
    return View();
}

[HttpPost] //create POST method is updated for authentication
public async Task<ActionResult> Create(Treat treat, int FlavorId)
{
    var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var currentUser = await _userManager.FindByIdAsync(userId);
    treat.User = currentUser;
    _db.Treats.Add(treat);
    if (FlavorId != 0)
    {
        _db.FlavorTreat.Add(new FlavorTreat() { FlavorId = FlavorId, TreatId = treat.TreatId });
    }
    _db.SaveChanges();
    return RedirectToAction("Index");
}

    public ActionResult Details(int id)
    {
        var thisTreat = _db.Treats
        .Include(treat => treat.Flavors)
        .ThenInclude(join => join.Flavor)
        .FirstOrDefault(treat => treat.TreatId == id);
    return View(thisTreat);
    }

public ActionResult Edit(int id)
{
    var thisTreat = _db.Treats.FirstOrDefault(treats => treats.TreatId == id);
    //ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
    return View(thisTreat);
}

    [HttpPost]
    public ActionResult Edit(Treat treat, int FlavorId)
    {
      if (FlavorId != 0)
      {
        _db.FlavorTreat.Add(new FlavorTreat() { FlavorId = FlavorId, TreatId = treat.TreatId });
      }
      _db.Entry(treat).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddFlavor(int id)
{
    var thisTreat = _db.Treats.FirstOrDefault(treats => treats.TreatId == id);
    ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
    return View(thisTreat);
}

[HttpPost]
public ActionResult AddFlavor(Treat treat, int FlavorId)
{
    if (FlavorId != 0)
    {
    _db.FlavorTreat.Add(new FlavorTreat() { FlavorId = FlavorId, TreatId = treat.TreatId });
    }
    _db.SaveChanges();
    return RedirectToAction("Index");
}

public ActionResult Delete(int id)
{
    var thisTreat = _db.Treats.FirstOrDefault(treats => treats.TreatId == id);
    return View(thisTreat);
}

[HttpPost, ActionName("Delete")]
public ActionResult DeleteConfirmed(int id)
{
    var thisTreat = _db.Treats.FirstOrDefault(treats => treats.TreatId == id);
    _db.Treats.Remove(thisTreat);
    _db.SaveChanges();
    return RedirectToAction("Index");
}

[HttpPost]
public ActionResult DeleteFlavor(int joinId)
{
    var joinEntry = _db.FlavorTreat.FirstOrDefault(entry => entry.FlavorTreatId == joinId);
    _db.FlavorTreat.Remove(joinEntry);
    _db.SaveChanges();
    return RedirectToAction("Index");
}

  }
}