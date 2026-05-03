using CreativePhotography.Content;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace CreativePhotography.Controllers
{
    public class AlbumDesignController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public AlbumDesignController(IWebHostEnvironment hostingEnviornment)
        {
            _hostingEnvironment = hostingEnviornment;
        }
        public IActionResult ClassicClean()
        {
            return View();
        } public IActionResult Ambaince()
        {
            return View();
        } public IActionResult Fusion()
        {
            return View();
        } public IActionResult Traditional()
        {
            return View();
        }
        public IActionResult ClassicCleanLayout1(int pageNumber = 1, int pageSize = 9)
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Classic Clean", "Classic_clean_layout1");
            string uploadCompressedPath=EnsureUploadDirectoriesExist(uploadPath);
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Classic Clean/Classic_clean_layout1", Path.GetFileName(path)))
                                                   .OrderBy(path => GetImageHeight(path))
                                                   .ToArray();
            int totalImages = originalImagePaths.Length;
            int totalPages = (int)Math.Ceiling((double)totalImages / pageSize);
            string[] allImagePaths = originalImagePaths;
            int pageStartIndex = (pageNumber - 1) * pageSize;
            originalImagePaths = originalImagePaths.Skip(pageStartIndex)
                                                   .Take(pageSize)
                                                   .ToArray();

            string[] compressedImagePaths = Directory.GetFiles(uploadCompressedPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Classic Clean/Classic_clean_layout1/Compressed", Path.GetFileName(path)))
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

       

        public IActionResult ClassicCleanLayout2(int pageNumber = 1, int pageSize = 9)
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Classic Clean", "Classic_clean_layout2");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Classic Clean/Classic_clean_layout2", Path.GetFileName(path)))
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
                                                   .Select(path => Path.Combine("/UploadedFiles/Classic Clean/Classic_clean_layout2/Compressed", Path.GetFileName(path)))
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

        public IActionResult ClassicCleanLayout3(int pageNumber = 1, int pageSize = 9)
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Classic Clean", "Classic_clean_layout3");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Classic Clean/Classic_clean_layout3", Path.GetFileName(path)))
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
                                                   .Select(path => Path.Combine("/UploadedFiles/Classic Clean/Classic_clean_layout3/Compressed", Path.GetFileName(path)))
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
        public IActionResult AmbianceLayout1(int pageNumber = 1, int pageSize = 9)
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Ambiance", "Ambiance_layout1");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Ambiance/Ambiance_layout1", Path.GetFileName(path)))
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
                                                   .Select(path => Path.Combine("/UploadedFiles/Ambiance/Ambiance_layout1/Compressed", Path.GetFileName(path)))
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

        public IActionResult AmbianceLayout2(int pageNumber = 1, int pageSize = 9)
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Ambiance", "Ambiance_layout2");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Ambiance/Ambiance_layout2", Path.GetFileName(path)))
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
                                                   .Select(path => Path.Combine("/UploadedFiles/Ambiance/Ambiance_layout2/Compressed", Path.GetFileName(path)))
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

        public IActionResult AmbianceLayout3(int pageNumber = 1, int pageSize = 9)
        {
            // Your action method logic here
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Ambiance", "Ambiance_layout3");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Ambiance/Ambiance_layout3", Path.GetFileName(path)))
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
                                                   .Select(path => Path.Combine("/UploadedFiles/Ambiance/Ambiance_layout3/Compressed", Path.GetFileName(path)))
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
        public IActionResult FusionLayout1(int pageNumber = 1, int pageSize = 9)
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Fusion", "Fusion_layout1");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);
            // Get all original image file paths in the directory
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Fusion/Fusion_layout1", Path.GetFileName(path)))
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
                                                   .Select(path => Path.Combine("/UploadedFiles/Fusion/Fusion_layout1/Compressed", Path.GetFileName(path)))
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
        public IActionResult FusionLayout2(int pageNumber = 1, int pageSize = 9)
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Fusion", "Fusion_layout2");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Fusion/Fusion_layout2", Path.GetFileName(path)))
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
                                                   .Select(path => Path.Combine("/UploadedFiles/Fusion/Fusion_layout2/Compressed", Path.GetFileName(path)))
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
        public IActionResult FusionLayout3(int pageNumber = 1, int pageSize = 9)
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Fusion", "Fusion_layout3");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Fusion/Fusion_layout3", Path.GetFileName(path)))
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
                                                   .Select(path => Path.Combine("/UploadedFiles/Fusion/Fusion_layout3/Compressed", Path.GetFileName(path)))
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
        public IActionResult TraditionalLayout1(int pageNumber = 1, int pageSize = 9)
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Traditional", "Traditional_layout1");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Traditional/Traditional_layout1", Path.GetFileName(path)))
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
                                                   .Select(path => Path.Combine("/UploadedFiles/Traditional/Traditional_layout1/Compressed", Path.GetFileName(path)))
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
        public IActionResult TraditionalLayout2(int pageNumber = 1, int pageSize = 9)
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Traditional", "Traditional_layout2");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Traditional/Traditional_layout2", Path.GetFileName(path)))
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
                                                   .Select(path => Path.Combine("/UploadedFiles/Traditional/Traditional_layout2/Compressed", Path.GetFileName(path)))
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
        public IActionResult TraditionalLayout3(int pageNumber = 1, int pageSize = 9)
        {
            string uploadPath = Path.Combine("wwwroot", "UploadedFiles", "Traditional", "Traditional_layout3");
            string uploadCompressedPath = EnsureUploadDirectoriesExist(uploadPath);
            string[] originalImagePaths = Directory.GetFiles(uploadPath)
                                                   .Select(path => Path.Combine("/UploadedFiles/Traditional/Traditional_layout3", Path.GetFileName(path)))
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
                                                   .Select(path => Path.Combine("/UploadedFiles/Traditional/Traditional_layout3/Compressed", Path.GetFileName(path)))
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
