using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public ProfileController(UserManager<IdentityUser> userManager, ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
    {
        _userManager = userManager;
        _context = context;
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.Id == user.Id);

        if (profile == null)
        {
            profile = new Profile { Id = user.Id };
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        return View(profile);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Profile model, IFormFile profilePicture)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.Id == user.Id);

        if (profile == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(model.FullName))
        {
            profile.FullName = model.FullName;
        }

        if (profilePicture != null && profilePicture.Length > 0)
        {
            var fileName = Path.GetFileName(profilePicture.FileName);
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images");

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            profile.ProfilePictureUrl = "/images/" + fileName;
        }

        _context.Update(profile);
        await _context.SaveChangesAsync();

        ViewBag.SuccessMessage = "Profil başarıyla güncellendi!";
        return View(profile);
    }
}
