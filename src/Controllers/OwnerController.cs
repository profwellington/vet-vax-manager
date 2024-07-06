using Microsoft.AspNetCore.Mvc;
using VetVaxManager.Models;
using VetVaxManager.Repository;

namespace VetVaxManager.Controllers;

public class OwnerController : Controller
{
    private readonly IOwnerRepository _ownerRepository;
    public OwnerController(IOwnerRepository ownerRepository)
    {
        _ownerRepository = ownerRepository;
    }

    [HttpGet]
    public IActionResult MyData()
    {
        var ownerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "OwnerId");
        int ownerId = int.Parse(ownerIdClaim.Value);
        var owner = _ownerRepository.GetOwnerById(ownerId);
        return View(owner);
    }

    [HttpPost]
    public ActionResult MyData(Owner owner)
    {
        var errors = ModelState.Values.SelectMany(v => v.Errors);
        if (ModelState.IsValid)
        {
            var updatedOwner = _ownerRepository.UpdateOwner(owner);
            return RedirectToAction("MyAnimals", "Animal");
        }
        owner = _ownerRepository.GetOwnerById(owner.OwnerId);
        return View(owner);
    }
}
