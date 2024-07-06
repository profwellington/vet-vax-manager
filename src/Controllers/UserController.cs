using Microsoft.AspNetCore.Mvc;
using VetVaxManager.Models;
using VetVaxManager.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.ObjectModel;

namespace VetVaxManager.Controllers;

public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        if (!ModelState.IsValid)
            return View(user);

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        _ = _userRepository.NewUser(user);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        User user = _userRepository.GetByUsername(username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            //TODO(Error): não está apresentando mensagem de erro
            ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos");
            return View();
        }

        Collection<Claim> claims = new()
        {
            new(ClaimTypes.Name, user.Owner.Name),
            new("Username", user.Username),
            new("OwnerId", user.Owner.OwnerId.ToString())
        };

        ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        AuthenticationProperties authProperties = new() { IsPersistent = true };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        return RedirectToAction("MyAnimals", "Animal");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
