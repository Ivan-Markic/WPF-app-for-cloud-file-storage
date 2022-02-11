using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudFileStorage.Dal
{
    static class Repository
    {
        private const string ContainerName = "blobcontainer";
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;

        private static readonly Lazy<BlobContainerClient> containter = new Lazy<BlobContainerClient>(() => new BlobServiceClient(connectionString).GetBlobContainerClient(ContainerName));
        public static BlobContainerClient Container => containter.Value;
    }
}
