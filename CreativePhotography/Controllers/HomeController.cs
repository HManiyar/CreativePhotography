using CreativePhotography.Content;
using CreativePhotography.IService;
using CreativePhotography.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;
using System.Net.Mail;

namespace CreativePhotography.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public HomeController(ILogger<HomeController> logger,IMailService mailService,IWebHostEnvironment hostingEnviornment)
        {
            _logger = logger;
            _mailService = mailService;
            _hostingEnvironment = hostingEnviornment;
        }
        public IActionResult Index(int pageNumber = 1, int pageSize = 9)
        {
            ViewBag.PageTitle = PageHeaders.Home;
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Home");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);
            // Get all original image file paths in the directory
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Home", Path.GetFileName(path)))
                                                   .OrderBy(path => GetImageHeight(path))
                                                   .ToArray();
            string[] compressedImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Home/Compressed", Path.GetFileName(path)))
                                                   .ToArray();
            // Pass both original and compressed image paths to the view
            ViewBag.OriginalImagePaths = originalImagePaths;
            ViewBag.CompressedImagePaths = compressedImagePaths;
            
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            ViewBag.PageTitle = PageHeaders.About;
            return View();
        }
        public IActionResult AlbumDesign()
        {
            ViewBag.PageTitle = PageHeaders.AlbumDesign;
            return View();
        }
        public IActionResult Photography()
        {
            ViewBag.PageTitle = PageHeaders.PhotoGraphy;
            return View();
        }
        public IActionResult PhotoEditing(int pageNumber = 1, int pageSize = 9)
        {
            ViewBag.PageTitle = PageHeaders.PhotoEditing;
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Photo Editing");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);

            // Get all original image file paths in the directory
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Photo Editing", Path.GetFileName(path)))
                                                   .OrderBy(path => GetImageHeight(path))
                                                   .ToArray();

            // Apply pagination to original image paths
            int totalImages = originalImagePaths.Length;
            int totalPages = (int)Math.Ceiling((double)totalImages / pageSize);
            string[] allPhotoEditingPaths = originalImagePaths;
            int photoEditingPageStart = (pageNumber - 1) * pageSize;
            originalImagePaths = originalImagePaths.Skip(photoEditingPageStart)
                                                   .Take(pageSize)
                                                   .ToArray();


            string[] compressedImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Photo Editing/Compressed", Path.GetFileName(path)))
                                                   .ToArray();
            // Convert original image paths to corresponding compressed image paths
            int totalCompressedImages = compressedImagePaths.Length;
            int totalCompressedPages = (int)Math.Ceiling((double)totalCompressedImages / pageSize);
            compressedImagePaths = compressedImagePaths.Skip(photoEditingPageStart)
                                                   .Take(pageSize)
                                                   .ToArray();

            // Pass both original and compressed image paths to the view
            ViewBag.OriginalImagePaths = originalImagePaths;
            ViewBag.CompressedImagePaths = compressedImagePaths;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.AllImagePaths = allPhotoEditingPaths;
            ViewBag.PageStartIndex = photoEditingPageStart;
            return View();
        }
        public IActionResult CollageDesign(int pageNumber = 1, int pageSize = 9)
        {
            ViewBag.PageTitle = PageHeaders.CollageDesign;
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Collage Design");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);

            // Get all original image file paths in the directory
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Collage Design", Path.GetFileName(path)))
                                                   .OrderBy(path => GetImageHeight(path))
                                                   .ToArray();

            // Apply pagination to original image paths
            int totalImages = originalImagePaths.Length;
            int totalPages = (int)Math.Ceiling((double)totalImages / pageSize);
            string[] allCollageDesignPaths = originalImagePaths;
            int collageDesignPageStart = (pageNumber - 1) * pageSize;
            originalImagePaths = originalImagePaths.Skip(collageDesignPageStart)
                                                   .Take(pageSize)
                                                   .ToArray();


            string[] compressedImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Collage Design/Compressed", Path.GetFileName(path)))
                                                   .ToArray();
            // Convert original image paths to corresponding compressed image paths
            int totalCompressedImages = compressedImagePaths.Length;
            int totalCompressedPages = (int)Math.Ceiling((double)totalCompressedImages / pageSize);
            compressedImagePaths = compressedImagePaths.Skip(collageDesignPageStart)
                                                   .Take(pageSize)
                                                   .ToArray();

            // Pass both original and compressed image paths to the view
            ViewBag.OriginalImagePaths = originalImagePaths;
            ViewBag.CompressedImagePaths = compressedImagePaths;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.AllImagePaths = allCollageDesignPaths;
            ViewBag.PageStartIndex = collageDesignPageStart;
            return View();
        }
        public IActionResult Videos()
        {
            ViewBag.PageTitle = PageHeaders.Videos;
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public async Task<ActionResult> SendMail(ContactUsModel form)
        {
            string message = await _mailService.SendMailToAdmin(form);
            if (message.Equals(EmailOperations.successSendEmail))
                return Json(message);
            else
                return Json(message);
        }
        private int GetImageHeight(string imagePath)
        {
            // Get the web root path
            var webRootPath = _hostingEnvironment.WebRootPath;

            // Combine the web root path with the virtual path to get the physical path
            var physicalPath = Path.Combine(webRootPath, imagePath.TrimStart('/').Replace('/', '\\'));

            // Load the image using the physical path
            using (var image = Image.FromFile(physicalPath))
            {
                return image.Height;
            }
        }
        private string EnsureUploadDirectoriesExist(string uploadPath)
        {
            string uploadCompressedPath = Path.Combine(uploadPath, "Compressed");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            if (!Directory.Exists(uploadCompressedPath))
            {
                Directory.CreateDirectory(uploadCompressedPath);
            }
            return uploadCompressedPath;
        }
    }
}