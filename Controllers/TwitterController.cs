
using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using Microsoft.Extensions.Configuration;


namespace FactFluxV3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwitterController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        public TwitterController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public string GetTwitterInfo()
        {
            Auth.SetUserCredentials(Configuration["IntegrationSettings:Twitter:ConsumerKey"],
            Configuration["IntegrationSettings:Twitter:ConsumerSecret"],
            Configuration["IntegrationSettings:Twitter:AccessToken"],
            Configuration["IntegrationSettings:Twitter:AccessTokenSecret"]);

            var authenticatedUser = Tweetinvi.User.GetAuthenticatedUser();

            var user = Tweetinvi.User.GetUserFromId(970207298);

            var use3r = Tweetinvi.User.GetUserFromScreenName("SenWarren");

            var test = Timeline.GetUserTimeline(970207298, 10);

            return "Hello";
        }
    }
}