
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AzureBlobExplorer.Controllers
{
    public class AzureBlobExplorerController : Controller
    {
        private static string _connectionString;

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Connect(string connectionString)
        {
            _connectionString = connectionString;
            return RedirectToAction("ListContainers");
        }



        public async Task<IActionResult> ListContainers()
        {
            var client = new BlobServiceClient(_connectionString);
            var containers = new List<BlobContainerItem>();

            await foreach (var container in client.GetBlobContainersAsync())
            {
                containers.Add(container);
            }
            containers = containers.OrderByDescending(b => b.Properties.LastModified)
                .ToList();
            return View(containers);
        }

        public async Task<IActionResult> BrowseContainer(string containerName, string? prefix)
        {
            var containerClient = new BlobContainerClient(_connectionString, containerName);
            var result = new List<(string Name, bool IsDirectory, long Size, DateTimeOffset? CreatedOn, DateTimeOffset? LastModified)>(); // Simplified type

            await foreach (var item in containerClient.GetBlobsByHierarchyAsync(prefix: prefix, delimiter: "/"))
            {
                if (item.IsPrefix)
                {
                    result.Add((item.Prefix, true, 0, null, null));
                }
                else
                {
                    var name = item.Blob.Name;
                    var size = item.Blob.Properties.ContentLength ?? 0;
                    var lastModified = item.Blob.Properties.LastModified;

                    // To get CreatedOn, you need an additional call:
                    var blobClient = containerClient.GetBlobClient(name);
                    // var props = await blobClient.GetPropertiesAsync();
                    // var createdOn = props.Value.CreatedOn;

                    result.Add((name, false, size, null, lastModified));
                }
            }
            result = result
                // .OrderByDescending(b => b.CreatedOn ?? b.LastModified)
                .OrderByDescending(b => b.LastModified)
                .ToList();
            ViewBag.ContainerName = containerName;
            ViewBag.Prefix = prefix ?? "";
            return View(result);
        }



        public async Task<IActionResult> ListBlobs()
        {
            var serviceClient = new BlobServiceClient(_connectionString);
            var containers = serviceClient.GetBlobContainers();
            var blobs = new List<(string ContainerName, string BlobName)>();

            foreach (var container in containers)
            {
                var containerClient = serviceClient.GetBlobContainerClient(container.Name);
                await foreach (var blob in containerClient.GetBlobsAsync())
                {
                    blobs.Add((container.Name, blob.Name));
                }
            }

            return View(blobs);
        }

        public async Task<IActionResult> DownloadBlob(string container, string blob)
        {
            var client = new BlobContainerClient(_connectionString, container);
            var blobClient = client.GetBlobClient(blob);
            var stream = new MemoryStream();
            await blobClient.DownloadToAsync(stream);
            stream.Position = 0;
            return File(stream, "application/octet-stream", blob);
        }

        public async Task<IActionResult> DeleteBlob(string container, string blob)
        {
            var client = new BlobContainerClient(_connectionString, container);
            var blobClient = client.GetBlobClient(blob);
            await blobClient.DeleteIfExistsAsync();
            return RedirectToAction("ListBlobs");
        }

        [HttpPost]
        public async Task<IActionResult> UploadBlob(IFormFile file, string container)
        {
            if (file != null && file.Length > 0)
            {
                var client = new BlobContainerClient(_connectionString, container);
                var blobClient = client.GetBlobClient(file.FileName);
                using var stream = file.OpenReadStream();
                await blobClient.UploadAsync(stream, overwrite: true);
            }
            return RedirectToAction("ListContainers");
        }
    }
}
