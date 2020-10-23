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
   [Authorize]
  public class FlavorsController : Controller
  {
  private readonly BakeryContext _db;

    public FlavorsController(BakeryContext db)
    {
      _db = db;
    }
[AllowAnonymous]
  public ActionResult Index()
  {
    List<Flavor> model = _db.Flavors.ToList();
      return View(model);
  }
  [Authorize(Roles = "Administrator")]
    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Flavor flavor)
    {
      _db.Flavors.Add(flavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    [AllowAnonymous]
public ActionResult Details(int id)
{
    var thisFlavor = _db.Flavors
        .Include(flavor => flavor.Treats)
        .ThenInclude(join => join.Treat)
        .FirstOrDefault(flavor => flavor.FlavorId == id);
    return View(thisFlavor);
}
[Authorize(Roles = "Administrator")]
    public ActionResult Edit(int id)
    {
      var thisFlavor = _db.Flavors.FirstOrDefault(flavors => flavors.FlavorId == id);
      return View(thisFlavor);
    }

    [HttpPost]
    public ActionResult Edit(Flavor flavor)
    {
      _db.Entry(flavor).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }




    public ActionResult AddTreat(int id)
{
    var thisFlavor = _db.Flavors.FirstOrDefault(flavors => flavors.FlavorId == id);
    ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "TreatName");
    return View(thisFlavor);
}

[HttpPost]
public ActionResult AddTreat(Flavor flavor, int TreatId)
{
    if (TreatId != 0)
    {
    _db.FlavorTreat.Add(new FlavorTreat() { TreatId = TreatId, FlavorId = flavor.FlavorId });
    }
    _db.SaveChanges();
    return RedirectToAction("Index");
}





[Authorize(Roles = "Administrator")]
  public ActionResult Delete(int id)
{
    var thisFlavor = _db.Flavors.FirstOrDefault(flavors => flavors.FlavorId == id);
    return View(thisFlavor);
}


}