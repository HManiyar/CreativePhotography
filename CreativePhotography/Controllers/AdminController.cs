using PixelCreator.Content;
using PixelCreator.IService;
using PixelCreator.Utility.CustomAttributes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PixelCreator.Controllers
{
    public class AdminController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public AdminController(IFileService fileService, IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _fileService = fileService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var userCredentials = _configuration.GetSection("Credentials");
            // Check if username and password are correct
            if (username == userCredentials["UserName"] && password == userCredentials["Password"])
            {
                var claims = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, "custom");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(principal);
                // If correct, redirect to the UploadImage action
                return RedirectToAction("UploadImage");
            }
            else
            {
                ViewBag.ErrorMessage = ErrorMessages.invalidCredentials;
                return View();
            }
        }
        [Authorize]
        public IActionResult UploadImage()
        {
            ViewBag.PageTitle = PageHeaders.UploadImages;
            return View();
        }
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [MultipartFormData]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> HandleUpload(string categoryName, string subCategoryName)
        {
            if (!HttpContext.Request.HasFormContentType)
            {
                return BadRequest("Request does not contain multipart/form-data.");
            }

            try
            {
                // Get the file stream and content type from the request
                var fileUploadSummary = await _fileService.UploadFileAsync(HttpContext.Request.Body, Request.ContentType!, categoryName, subCategoryName);
                // Redirect back to the upload view with some indication of success
                return RedirectToAction("UploadImage", new { successMessage = "Files uploaded successfully!" });

            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _fileService.GetCategories());
        }
        [Authorize]
        public async Task<IActionResult> GetSubCategories(int categoryId)
        {
            return Ok(await _fileService.GetSubCategories(categoryId));
        }
        [Authorize]
        public IActionResult RemoveImage()
        {
            ViewBag.PageTitle = PageHeaders.RemoveImages;
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult GetRequestedImages(string categoryName, string subCategoryName)
        {

            var (originalImagePaths, compressedImagePaths) = _fileService.GetRequestedImages(categoryName, subCategoryName);
            var originalImageUrls = originalImagePaths.Select(path => Url.Content(path)).ToList();
            var compressedImageUrls = compressedImagePaths.Select(path => Url.Content(path)).ToList();
            var imageUrls = originalImageUrls.Zip(compressedImageUrls, (original, compressed) => new { Original = original, Compressed = compressed });

            if (imageUrls.Any())
            {
                return Ok(imageUrls);
            }
            else
            {
                return Ok(new List<string>());
            }
        }
        [Authorize]
        [HttpPost]
        public IActionResult DeleteImage(string imagePath)
        {
            string message = _fileService.DeleteImage(imagePath, _hostingEnvironment);
            if (message.Equals(ImageOperations.successDeleteImage))
                return Ok(ImageOperations.successDeleteImage);
            else
                return StatusCode(500, ImageOperations.failedDeleteImage);
        }
    }
}
