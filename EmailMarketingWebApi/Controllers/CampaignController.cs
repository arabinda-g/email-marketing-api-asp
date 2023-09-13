using EmailMarketingWebApi.Data;
using EmailMarketingWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmailMarketingWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CampaignController
    {
        private readonly ApplicationDbContext _context;

        public CampaignController(ApplicationDbContext context)
        {
            _context = context;
        }


        // Add route for creating a new campaign
        [HttpPost(Name = "CreateCampaign")]
        public IActionResult CreateCampaign([FromBody] Campaign campaign)
        {
            _context.Campaigns.Add(campaign);
            _context.SaveChanges();
            return new ObjectResult(campaign);

        }



    }
}
