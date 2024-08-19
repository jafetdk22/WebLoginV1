using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Configuration;
using OtraPrueba.Models.Request.Login;


namespace OtraPrueba.Controllers.Login
{
    public class LoginController : Controller
    {
        private readonly ApiLoginClient _apiLogin;
        private readonly IConfiguration _configuration;

        public LoginController(ApiLoginClient ApiLogin, IConfiguration configuration)
        {
            _configuration = configuration;
            _apiLogin = new ApiLoginClient(_configuration["ApiLoginUrl"], new HttpClient());
        }


        public IActionResult Index()
        {
            return View("~/Views/Login/Index.cshtml");
        }
        public IActionResult Error()
        {
            return View("~/Views/Login/Error.cshtml");
        }
        public async Task<IActionResult> PostLogin(LoginPostRequest request)
        {
            var UserLogin = new LoginRequest();
            UserLogin.UsernameOrEmail= request.UsernameOrEmail;
            UserLogin.Password = request.Password;
            try
            {
                var apiUser = await _apiLogin.LoginAsync(UserLogin);
                if (!apiUser.IsSuccess)
                {
                    TempData["Error"] = apiUser.ErrorMessage;
                    return RedirectToAction("Index", "Login");
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex) {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Error", "Login");
            }
        }
    }
}
