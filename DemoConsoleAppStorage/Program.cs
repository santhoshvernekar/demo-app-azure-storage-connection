using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;

namespace DemoConsoleAppStorage
{
    class Program
    {
        static CloudStorageAccount storageAccount;
        static void Main(string[] args)
        {
            Console.WriteLine("Main method calling");
            // Please Copy Connection String from Azure Storage account
            string connectionString = ""; // "DefaultEndpointsProtocol=https;AccountName=demostorageazure001;AccountKey=xyz";



            if (
                CloudStorageAccount.TryParse(connectionString, out storageAccount))
            {
                Console.WriteLine("Connection is establishing");
            }
            else
            {
                Console.WriteLine("Connection  not established");
                Console.ReadLine();
            }
            Console.WriteLine("Demo of Blob Storage");

            // ProcessAsyncBlobStorage().GetAwaiter().GetResult();

            Console.WriteLine("Demo of Table Storage");
            ProcessTableStorage().GetAwaiter().GetResult();
            Console.ReadLine();
        }

        private static async Task ProcessAsyncBlobStorage()
        {
            // Create CloudBlobClient
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            // Create Container
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("quickstartblobs");
            await cloudBlobContainer.CreateIfNotExistsAsync();

            // Set the Blob Storgae permissions
            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };

            await cloudBlobContainer.SetPermissionsAsync(permissions);

            //List the blobs in container

            Console.WriteLine("Listing the blobs in container");

            BlobContinuationToken blobContinuationToken = null;

            do
            {
                var results = await cloudBlobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
                blobContinuationToken = results.ContinuationToken;

                foreach (IListBlobItem item in results.Results)
                {
                    Console.WriteLine("Listing the Uri");
                    Console.WriteLine(item.Uri);
                } 
            } while (blobContinuationToken != null);  //Loop while blobContinuationToken is not null

            Console.WriteLine("End of the items");
            Console.WriteLine();
            Console.ReadLine();

        }

        private static async Task ProcessTableStorage()
        {
            // Create new client
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table if doesnt exist then it creates the table
            CloudTable table = tableClient.GetTableReference("people");

            // Create the table if it doesnt exist
           await table.CreateIfNotExistsAsync();
        }
    }
}
