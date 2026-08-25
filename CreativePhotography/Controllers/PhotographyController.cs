using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace PixelCreator.Controllers
{
    public class PhotographyController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public PhotographyController(IWebHostEnvironment hostingEnviornment)
        {
            _hostingEnvironment = hostingEnviornment;
        }
        public IActionResult PreWedding(int pageNumber = 1, int pageSize = 9)
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Pre-Wedding");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);

            // Get all original image file paths in the directory
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Pre-Wedding", Path.GetFileName(path)))
                                                   .OrderBy(path => GetImageHeight(path))
                                                   .ToArray();
            int totalImages = originalImagePaths.Length;
            int totalPages = (int)Math.Ceiling((double)totalImages / pageSize);
            string[] allImagePaths = originalImagePaths;
            int pageStartIndex = (pageNumber - 1) * pageSize;
            originalImagePaths = originalImagePaths.Skip(pageStartIndex)
                                                   .Take(pageSize)
                                                   .ToArray();
            string[] compressedImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Pre-Wedding/Compressed", Path.GetFileName(path)))
                                                   .ToArray();
            int totalCompressedImages = compressedImagePaths.Length;
            int totalCompressedPages = (int)Math.Ceiling((double)totalCompressedImages / pageSize);
            compressedImagePaths = compressedImagePaths.Skip((pageNumber - 1) * pageSize)
                                                   .Take(pageSize)
                                                   .ToArray();
            ViewBag.OriginalImagePaths = originalImagePaths;
            ViewBag.CompressedImagePaths = compressedImagePaths;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.AllImagePaths = allImagePaths;
            ViewBag.PageStartIndex = pageStartIndex;

            return View();
        }
      
        public IActionResult Engagement(int pageNumber = 1, int pageSize = 9)
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Engagement");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);

            // Get all original image file paths in the directory
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Engagement", Path.GetFileName(path)))
                                                   .OrderBy(path => GetImageHeight(path))
                                                   .ToArray();
            int totalImages = originalImagePaths.Length;
            int totalPages = (int)Math.Ceiling((double)totalImages / pageSize);
            string[] allImagePaths = originalImagePaths;
            int pageStartIndex = (pageNumber - 1) * pageSize;
            originalImagePaths = originalImagePaths.Skip(pageStartIndex)
                                                   .Take(pageSize)
                                                   .ToArray();
            string[] compressedImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Engagement/Compressed", Path.GetFileName(path)))
                                                   .ToArray();
            int totalCompressedImages = compressedImagePaths.Length;
            int totalCompressedPages = (int)Math.Ceiling((double)totalCompressedImages / pageSize);
            compressedImagePaths = compressedImagePaths.Skip((pageNumber - 1) * pageSize)
                                                   .Take(pageSize)
                                                   .ToArray();
            ViewBag.OriginalImagePaths = originalImagePaths;
            ViewBag.CompressedImagePaths = compressedImagePaths;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.AllImagePaths = allImagePaths;
            ViewBag.PageStartIndex = pageStartIndex;
            return View();
        }
        public IActionResult Wedding(int pageNumber = 1, int pageSize = 9)
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Wedding");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);

            // Get all original image file paths in the directory
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Wedding", Path.GetFileName(path)))
                                                    .OrderBy(path => GetImageHeight(path))
                                                   .ToArray();
            int totalImages = originalImagePaths.Length;
            int totalPages = (int)Math.Ceiling((double)totalImages / pageSize);
            string[] allImagePaths = originalImagePaths;
            int pageStartIndex = (pageNumber - 1) * pageSize;
            originalImagePaths = originalImagePaths.Skip(pageStartIndex)
                                                   .Take(pageSize)
                                                   .ToArray();
            string[] compressedImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Wedding/Compressed", Path.GetFileName(path)))
                                                   .ToArray();
            int totalCompressedImages = compressedImagePaths.Length;
            int totalCompressedPages = (int)Math.Ceiling((double)totalCompressedImages / pageSize);
            compressedImagePaths = compressedImagePaths.Skip((pageNumber - 1) * pageSize)
                                                   .Take(pageSize)
                                                   .ToArray();
            ViewBag.OriginalImagePaths = originalImagePaths;
            ViewBag.CompressedImagePaths = compressedImagePaths;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.AllImagePaths = allImagePaths;
            ViewBag.PageStartIndex = pageStartIndex;
            return View();
        }
        public IActionResult BabyShower(int pageNumber = 1, int pageSize = 9)
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Babyshower");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);
            // Get all original image file paths in the directory
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Babyshower", Path.GetFileName(path)))
                                                   .OrderBy(path => GetImageHeight(path))
                                                   .ToArray();
            int totalImages = originalImagePaths.Length;
            int totalPages = (int)Math.Ceiling((double)totalImages / pageSize);
            string[] allImagePaths = originalImagePaths;
            int pageStartIndex = (pageNumber - 1) * pageSize;
            originalImagePaths = originalImagePaths.Skip(pageStartIndex)
                                                   .Take(pageSize)
                                                   .ToArray();
            string[] compressedImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Babyshower/Compressed", Path.GetFileName(path)))
                                                   .ToArray();
            int totalCompressedImages = compressedImagePaths.Length;
            int totalCompressedPages = (int)Math.Ceiling((double)totalCompressedImages / pageSize);
            compressedImagePaths = compressedImagePaths.Skip((pageNumber - 1) * pageSize)
                                                   .Take(pageSize)
                                                   .ToArray();
            ViewBag.OriginalImagePaths = originalImagePaths;
            ViewBag.CompressedImagePaths = compressedImagePaths;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.AllImagePaths = allImagePaths;
            ViewBag.PageStartIndex = pageStartIndex;
            return View();
        }

        public IActionResult Birthday(int pageNumber = 1, int pageSize = 9)
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Birthday");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);
            // Get all original image file paths in the directory
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Birthday", Path.GetFileName(path)))
                                                    .OrderBy(path => GetImageHeight(path))
                                                   .ToArray();
            int totalImages = originalImagePaths.Length;
            int totalPages = (int)Math.Ceiling((double)totalImages / pageSize);
            string[] allImagePaths = originalImagePaths;
            int pageStartIndex = (pageNumber - 1) * pageSize;
            originalImagePaths = originalImagePaths.Skip(pageStartIndex)
                                                   .Take(pageSize)
                                                   .ToArray();
            string[] compressedImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Birthday/Compressed", Path.GetFileName(path)))
                                                   .ToArray();
            int totalCompressedImages = compressedImagePaths.Length;
            int totalCompressedPages = (int)Math.Ceiling((double)totalCompressedImages / pageSize);
            compressedImagePaths = compressedImagePaths.Skip((pageNumber - 1) * pageSize)
                                                   .Take(pageSize)
                                                   .ToArray();
            ViewBag.OriginalImagePaths = originalImagePaths;
            ViewBag.CompressedImagePaths = compressedImagePaths;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.AllImagePaths = allImagePaths;
            ViewBag.PageStartIndex = pageStartIndex;
            return View();
        }
        public IActionResult Decoration(int pageNumber = 1, int pageSize = 9)
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Decoration");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);

            // Get all original image file paths in the directory
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Decoration", Path.GetFileName(path)))
                                                    .OrderBy(path => GetImageHeight(path))
                                                   .ToArray();
            int totalImages = originalImagePaths.Length;
            int totalPages = (int)Math.Ceiling((double)totalImages / pageSize);
            string[] allImagePaths = originalImagePaths;
            int pageStartIndex = (pageNumber - 1) * pageSize;
            originalImagePaths = originalImagePaths.Skip(pageStartIndex)
                                                   .Take(pageSize)
                                                   .ToArray();
            string[] compressedImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Decoration/Compressed", Path.GetFileName(path)))
                                                   .ToArray();
            int totalCompressedImages = compressedImagePaths.Length;
            int totalCompressedPages = (int)Math.Ceiling((double)totalCompressedImages / pageSize);
            compressedImagePaths = compressedImagePaths.Skip((pageNumber - 1) * pageSize)
                                                   .Take(pageSize)
                                                   .ToArray();
            ViewBag.OriginalImagePaths = originalImagePaths;
            ViewBag.CompressedImagePaths = compressedImagePaths;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.AllImagePaths = allImagePaths;
            ViewBag.PageStartIndex = pageStartIndex;
            return View();
        }
        public IActionResult IndustrialPhotoshoot()
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Industrial-Photoshoot");
            EnsureUploadDirectoriesExist(uploadPath);
            string[] imagePaths = Directory.GetFiles(uploadPath)
                .Select(f => $"/UploadedFiles/Industrial-Photoshoot/{Path.GetFileName(f)}")
                .OrderBy(f => f)
                .ToArray();
            ViewBag.OriginalImagePaths = imagePaths;
            ViewBag.AllImagePaths = imagePaths;
            ViewBag.PageNumber = 1;
            ViewBag.TotalPages = 1;
            ViewBag.PageStartIndex = 0;
            return View();
        }

        public IActionResult Photoshoot()
        {
            return View();
        }

        public IActionResult PhotoshootECommerce()
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Photoshoot", "E-Commerce");
            EnsureUploadDirectoriesExist(uploadPath);
            string[] imagePaths = Directory.GetFiles(uploadPath)
                .Select(f => $"/UploadedFiles/Photoshoot/E-Commerce/{Path.GetFileName(f)}")
                .OrderBy(f => f)
                .ToArray();
            ViewBag.OriginalImagePaths = imagePaths;
            ViewBag.AllImagePaths = imagePaths;
            ViewBag.PageNumber = 1;
            ViewBag.TotalPages = 1;
            ViewBag.PageStartIndex = 0;
            return View();
        }

        public IActionResult PhotoshootGlassProduct()
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Photoshoot", "Glass-Product");
            EnsureUploadDirectoriesExist(uploadPath);
            string[] imagePaths = Directory.GetFiles(uploadPath)
                .Select(f => $"/UploadedFiles/Photoshoot/Glass-Product/{Path.GetFileName(f)}")
                .OrderBy(f => f)
                .ToArray();
            ViewBag.OriginalImagePaths = imagePaths;
            ViewBag.AllImagePaths = imagePaths;
            ViewBag.PageNumber = 1;
            ViewBag.TotalPages = 1;
            ViewBag.PageStartIndex = 0;
            return View();
        }

        public IActionResult PhotoshootIceCream()
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Photoshoot", "Ice-Cream");
            EnsureUploadDirectoriesExist(uploadPath);
            string[] imagePaths = Directory.GetFiles(uploadPath)
                .Select(f => $"/UploadedFiles/Photoshoot/Ice-Cream/{Path.GetFileName(f)}")
                .OrderBy(f => f)
                .ToArray();
            ViewBag.OriginalImagePaths = imagePaths;
            ViewBag.AllImagePaths = imagePaths;
            ViewBag.PageNumber = 1;
            ViewBag.TotalPages = 1;
            ViewBag.PageStartIndex = 0;
            return View();
        }

        public IActionResult PhotoshootPoches()
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Photoshoot", "Poches");
            EnsureUploadDirectoriesExist(uploadPath);
            string[] imagePaths = Directory.GetFiles(uploadPath)
                .Select(f => $"/UploadedFiles/Photoshoot/Poches/{Path.GetFileName(f)}")
                .OrderBy(f => f)
                .ToArray();
            ViewBag.OriginalImagePaths = imagePaths;
            ViewBag.AllImagePaths = imagePaths;
            ViewBag.PageNumber = 1;
            ViewBag.TotalPages = 1;
            ViewBag.PageStartIndex = 0;
            return View();
        }

        public IActionResult PhotoshootStoneItems()
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Photoshoot", "Stone-Items");
            EnsureUploadDirectoriesExist(uploadPath);
            string[] imagePaths = Directory.GetFiles(uploadPath)
                .Select(f => $"/UploadedFiles/Photoshoot/Stone-Items/{Path.GetFileName(f)}")
                .OrderBy(f => f)
                .ToArray();
            ViewBag.OriginalImagePaths = imagePaths;
            ViewBag.AllImagePaths = imagePaths;
            ViewBag.PageNumber = 1;
            ViewBag.TotalPages = 1;
            ViewBag.PageStartIndex = 0;
            return View();
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
