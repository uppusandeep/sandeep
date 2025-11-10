using Microsoft.AspNetCore.Mvc;

namespace KentuckyUnemployment.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReferenceDataController : ControllerBase
{
    private static readonly string[] KentuckyCounties =
    [
        "Adair", "Allen", "Anderson", "Ballard", "Barren", "Bath", "Bell", "Boone", "Bourbon", "Boyd",
        "Boyle", "Bracken", "Breathitt", "Breckinridge", "Bullitt", "Butler", "Caldwell", "Calloway",
        "Campbell", "Carlisle", "Carroll", "Carter", "Casey", "Christian", "Clark", "Clay", "Clinton",
        "Crittenden", "Cumberland", "Daviess", "Edmonson", "Elliott", "Estill", "Fayette", "Fleming",
        "Floyd", "Franklin", "Fulton", "Gallatin", "Garrard", "Grant", "Graves", "Grayson", "Green",
        "Greenup", "Hancock", "Hardin", "Harlan", "Harrison", "Hart", "Henderson", "Henry", "Hickman",
        "Hopkins", "Jackson", "Jefferson", "Jessamine", "Johnson", "Kenton", "Knott", "Knox", "Larue",
        "Laurel", "Lawrence", "Lee", "Leslie", "Letcher", "Lewis", "Lincoln", "Livingston", "Logan",
        "Lyon", "Madison", "Magoffin", "Marion", "Marshall", "Martin", "Mason", "McCracken", "McCreary",
        "McLean", "Meade", "Menifee", "Mercer", "Metcalfe", "Monroe", "Montgomery", "Morgan", "Muhlenberg",
        "Nelson", "Nicholas", "Ohio", "Oldham", "Owen", "Owsley", "Pendleton", "Perry", "Pike", "Powell",
        "Pulaski", "Robertson", "Rockcastle", "Rowan", "Russell", "Scott", "Shelby", "Simpson", "Spencer",
        "Taylor", "Todd", "Trigg", "Trimble", "Union", "Warren", "Washington", "Wayne", "Webster",
        "Whitley", "Wolfe", "Woodford"
    ];

    [HttpGet("counties")]
    public ActionResult<IEnumerable<string>> GetCounties()
    {
        return Ok(KentuckyCounties);
    }

    [HttpGet("workshops")]
    public ActionResult<IEnumerable<object>> GetCareerWorkshops()
    {
        var workshops = new[]
        {
            new
            {
                Id = Guid.NewGuid(),
                Title = "KCC Louisville: Resume Essentials",
                Location = "Louisville Career Center",
                County = "Jefferson",
                Start = new DateTime(2025, 11, 18, 10, 0, 0, DateTimeKind.Utc),
                End = new DateTime(2025, 11, 18, 11, 30, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = Guid.NewGuid(),
                Title = "UI Eligibility 101",
                Location = "Virtual Webinar",
                County = "Statewide",
                Start = new DateTime(2025, 11, 20, 15, 0, 0, DateTimeKind.Utc),
                End = new DateTime(2025, 11, 20, 16, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = Guid.NewGuid(),
                Title = "Lexington Job Club",
                Location = "Lexington Career Center",
                County = "Fayette",
                Start = new DateTime(2025, 11, 22, 13, 0, 0, DateTimeKind.Utc),
                End = new DateTime(2025, 11, 22, 14, 30, 0, DateTimeKind.Utc)
            }
        };

        return Ok(workshops);
    }
}
