
using Microsoft.AspNetCore.Mvc;
using Bakery.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Bakery.Controllers
{
  [Authorize]
  public class TreatsController : Controller
  {
    private readonly BakeryContext _db;

    public TreatsController(BakeryContext db)
    {
      _db = db;
    }
    [AllowAnonymous]
    public ActionResult Index()
    {
      List<Treat> model = _db.Treats.ToList();
        return View(model);
    }
    [Authorize(Roles = "Administrator")]
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
    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      var thisTreat = _db.Treats
      .Include(treat => treat.Flavors)
      .ThenInclude(join => join.Flavor)
      .FirstOrDefault(treat => treat.TreatId == id);
    return View(thisTreat);
    }
    [Authorize(Roles = "Administrator")]
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
    [Authorize(Roles = "Administrator")]
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