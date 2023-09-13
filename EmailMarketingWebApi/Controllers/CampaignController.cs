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
        // Add route for creating a new campaign
        [HttpPost(Name = "CreateCampaign")]
        public IActionResult CreateCampaign([FromBody] Campaign campaign)
        {
            // Create a new campaign and save it to the database
            // Return a 201 Created response
            return new CreatedResult("GetCampaign", campaign);












        }



    }
}
