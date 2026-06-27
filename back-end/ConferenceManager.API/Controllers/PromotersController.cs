namespace ConferenceManager.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using ConferenceManager.API.DTOs.Promoters;
    using ConferenceManager.API.Services.Promoters;


    [ApiController]
    [Route("api/[controller]")]
    public class PromotersController : ControllerBase
    {
        private readonly IPromoterService _promoterService;

        public PromotersController(IPromoterService promoterService)
        {
            _promoterService = promoterService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AssignPromoter(AssignPromoterDto dto)
        {
            var result = _promoterService.AssignPromoter(dto.UserId, dto.ReferralCode);

            if(!result)
            {
                return BadRequest("Unable to assign promoter");
            }

            return Ok("Promoter assigned successfully");
        }
       
    }
}