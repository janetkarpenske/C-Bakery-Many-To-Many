
using Microsoft.AspNetCore.Mvc;
using Bakery.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Bakery.Controllers
{
  //[Authorize]
  public class TreatsController : Controller
  {
    private readonly BakeryContext _db;
    //private readonly UserManager<ApplicationUser> _userManager; 

    public TreatsController(BakeryContext db)
    {
      //_userManager = userManager;
      _db = db;
    }

    //updated Index method
  public ActionResult Index()
  {
    List<Treat> model = _db.Treats.ToList();
      return View(model);
  }
public ActionResult Create()
{
    return View();
}


    [HttpPost]
    public ActionResult Create(Treat treat)
    {
      _db.Treats.Add(treat);
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