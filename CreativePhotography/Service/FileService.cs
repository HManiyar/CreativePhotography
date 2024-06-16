using CreativePhotography.Content;
using CreativePhotography.IRepository;
using CreativePhotography.IService;
using CreativePhotography.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace CreativePhotography.Service
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
        public async Task<FileUploadSummary> UploadFileAsync(Stream fileStream, string contentType, string categoryName, string subCategoryName)
        {
            var fileCount = 0;
            long totalSizeInBytes = 0;
            var boundary = GetBoundary(MediaTypeHeaderValue.Parse(contentType));
            var multipartReader = new MultipartReader(boundary, fileStream);
            var section = await multipartReader.ReadNextSectionAsync();
            var filePaths = new List<string>();
            var notUploadedFiles = new List<string>();

            // Determine the main directory based on category and subcategory names
            string mainDirectory;
            if (!categoryName.Equals("None"))
            {
                mainDirectory = categoryName;
                if (!subCategoryName.Equals("None"))
                {
                    mainDirectory = Path.Combine(mainDirectory, subCategoryName);
                }
            }
            else
            {
                mainDirectory = "Home";
            }

            while (section != null)
            {
                var fileSection = section.AsFileSection();
                if (fileSection != null)
                {
                    totalSizeInBytes += await SaveFileAsync(fileSection, filePaths, notUploadedFiles, mainDirectory);
                    fileCount++;
                }
                section = await multipartReader.ReadNextSectionAsync();
            }

            return new FileUploadSummary
            {
                TotalFilesUploaded = fileCount,
                TotalSizeUploaded = ConvertSizeToString(totalSizeInBytes),
                FilePaths = filePaths,
                NotUploadedFiles = notUploadedFiles
            };
        }

        private async Task<long> SaveFileAsync(FileMultipartSection fileSection, List<string> filePaths, List<string> notUploadedFiles, string mainDirectory)
        {
            try
            {
                // Construct the full directory path
                var directoryPath = Path.Combine("wwwroot", "UploadedFiles", mainDirectory);

                // Create directory if it doesn't exist
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filePath = Path.Combine(directoryPath, fileSection.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await fileSection.FileStream!.CopyToAsync(fileStream);
                }

                // Compress and save the image to a "Compressed" folder
                await CompressAndSaveImageAsync(filePath, directoryPath);

                filePaths.Add(filePath);
                return fileSection.FileStream.Length;
            }
            catch (Exception ex)
            {
                notUploadedFiles.Add(ex.Message);
                return 0;
            }
        }
        private async Task CompressAndSaveImageAsync(string filePath, string directoryPath)
        {
            // Construct the compressed directory path
            var compressedDirectoryPath = Path.Combine(directoryPath, "Compressed");

            // Create compressed directory if it doesn't exist
            if (!Directory.Exists(compressedDirectoryPath))
            {
                Directory.CreateDirectory(compressedDirectoryPath);
            }

            var compressedFilePath = Path.Combine(compressedDirectoryPath, Path.GetFileName(filePath));

            await Task.Run(() =>
            {
                using (Image image = Image.Load(filePath))
                {
                    // Adjust quality and size as needed for your use case
                    var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder
                    {
                        Quality = 75 // Adjust the quality value as needed
                    };

                    // Resize the image
                    image.Mutate(x => x.Resize(width: image.Width/4,height: image.Height/4));

                    // Save the compressed image with JPEG encoder
                    image.Save(compressedFilePath, encoder);
                }
            });
        }

        private string GetBoundary(MediaTypeHeaderValue contentType)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary);
            return boundary.Value!;
        }
        private string ConvertSizeToString(long sizeInBytes)
        {
            const int byteConversion = 1024;
            double bytes = Convert.ToDouble(sizeInBytes);

            if (bytes >= Math.Pow(byteConversion, 3))
            {
                return $"{(bytes / Math.Pow(byteConversion, 3)):##.##} GB";
            }
            if (bytes >= Math.Pow(byteConversion, 2))
            {
                return $"{(bytes / Math.Pow(byteConversion, 2)):##.##} MB";
            }
            if (bytes >= byteConversion)
            {
                return $"{(bytes / byteConversion):##.##} KB";
            }
            return $"{bytes} Bytes";
        }

        public async Task<IEnumerable<CategoryModel>> GetCategories()
        {
            return await _fileRepository.GetCategories();
        }

        public async Task<IEnumerable<SubcategoryModel>> GetSubCategories(int categoryId)
        {
            return await _fileRepository.GetSubCategories(categoryId);
        }

        public (List<string> originalImagePaths, List<string> compressedImagePaths) GetRequestedImages(string categoryName, string subCategoryName)
        {
            try
            {
                var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles");
                var originalImagePaths = new List<string>();
                var compressedImagePaths = new List<string>();

                if (!categoryName.Equals("None"))
                {
                    var categoryPath = Path.Combine(rootPath, categoryName);

                    if (!subCategoryName.Equals("None"))
                    {
                        var subCategoryPath = Path.Combine(categoryPath, subCategoryName);

                        if (Directory.Exists(subCategoryPath))
                        {
                            // Get all original images from the subcategory directory
                            var originalFiles = Directory.GetFiles(subCategoryPath);
                            originalImagePaths.AddRange(originalFiles);

                            // Get all compressed images from the subcategory directory
                            var compressedSubCategoryPath = Path.Combine(subCategoryPath, "Compressed");
                            if (Directory.Exists(compressedSubCategoryPath))
                            {
                                var compressedFiles = Directory.GetFiles(compressedSubCategoryPath);
                                compressedImagePaths.AddRange(compressedFiles);
                            }
                        }
                    }
                    else
                    {
                        if (Directory.Exists(categoryPath))
                        {
                            // Get all original images from the category directory
                            var originalFiles = Directory.GetFiles(categoryPath);
                            originalImagePaths.AddRange(originalFiles);

                            // Get all compressed images from the category directory
                            var compressedCategoryPath = Path.Combine(categoryPath, "Compressed");
                            if (Directory.Exists(compressedCategoryPath))
                            {
                                var compressedFiles = Directory.GetFiles(compressedCategoryPath);
                                compressedImagePaths.AddRange(compressedFiles);
                            }
                        }
                    }
                }
                else
                {
                    var homePath = Path.Combine(rootPath, "Home");

                    if (Directory.Exists(homePath))
                    {
                        // Get all original images from the Home directory
                        var originalFiles = Directory.GetFiles(homePath);
                        originalImagePaths.AddRange(originalFiles);

                        // Get all compressed images from the Home directory
                        var compressedHomePath = Path.Combine(homePath, "Compressed");
                        if (Directory.Exists(compressedHomePath))
                        {
                            var compressedFiles = Directory.GetFiles(compressedHomePath);
                            compressedImagePaths.AddRange(compressedFiles);
                        }
                    }
                }

                return (originalImagePaths, compressedImagePaths);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return (new List<string>(), new List<string>());
            }
        }
        public string DeleteImage(string imagePath, IWebHostEnvironment _hostingEnvironment) { 

            var relativePath = imagePath.Replace(_hostingEnvironment.WebRootPath, string.Empty)
                                            .Replace(Path.DirectorySeparatorChar, '/');

            // Delete the image file from the Compressed directory
            var compressedFilePath = _hostingEnvironment.WebRootPath + relativePath;
            if (System.IO.File.Exists(compressedFilePath))
            {
                System.IO.File.Delete(compressedFilePath);

                // Get the parent directory of the Compressed directory
                var parentDirectory = Directory.GetParent(Path.GetDirectoryName(compressedFilePath)!);
                if (parentDirectory != null)
                {
                    // Construct the path to the image file in the parent directory
                    var parentFilePath = Path.Combine(parentDirectory.FullName, Path.GetFileName(imagePath));

                    // Delete the image file from the parent directory if it exists
                    if (System.IO.File.Exists(parentFilePath))
                    {
                        System.IO.File.Delete(parentFilePath);
                    }
                }

                return ImageOperations.successDeleteImage;
            }
            else
            {
                return ImageOperations.failedDeleteImage;
            }
        }
    }
}
