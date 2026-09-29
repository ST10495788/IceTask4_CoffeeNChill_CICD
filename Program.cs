using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);
builder.ConfigureFunctionsWebApplication();

// Configure the MenuItems table used to store menu data in Azure Table Storage.

builder.Services.AddSingleton<TableClient>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();

    // Retrieve the configured storage connection string for the Table Storage service.

    var connectionString = configuration["AzureTableStorage"] ?? configuration["AzureWebJobsStorage"];

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("Azure Storage connection string is missing.");
    }

    var tableServiceClient = new TableServiceClient(connectionString);

    var tableClient = tableServiceClient.GetTableClient("MenuItems");

    // Create the MenuItems table automatically if it does not already exist.
    tableClient.CreateIfNotExists();

    return tableClient;
});

// Configure Blob Storage for storing and retrieving staff documents.

builder.Services.AddSingleton<BlobServiceClient>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();

    var connectionString = configuration["AzureWebJobsStorage"];

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("AzureWebJobsStorage connection string is missing.");
    }

    return new BlobServiceClient(connectionString);
});

builder.Build().Run();

// Reference List
//Docker, 2026. Build and share a containerized application. [Online] Available at: https://docs.docker.com/get-started/tutorials/run-an-app/ [Accessed 13 September 2026].
//Docker, 2026. Docker run. [Online] Available at: https://docs.docker.com/reference/cli/docker/container/run [Accessed 10 September 2026].
//Hostinger, 2026. Docker cheat sheet: Most important commands + free PDF. [Online] Available at: https://www.hostinger.com/tutorials/docker-cheat-sheet/ [Accessed 10 September 2026].
//Microsoft, 2026a. Azure Blob Storage documentation. [Online] Available at: https://learn.microsoft.com/en-us/azure/storage/blobs/ [Accessed 9 September 2026].
//Microsoft, 2026b. Azure Functions documentation. [Online] Available at: https://learn.microsoft.com/en-us/azure/azure-functions/ [Accessed 9 September 2026].
//Microsoft, 2026c. Azure Table Storage documentation. [Online] Available at: https://learn.microsoft.com/en-us/azure/storage/tables/table-storage-overview [Accessed 9 September 2026].
//Microsoft, 2026d. Use the Azurite emulator for local Azure Storage development. [Online] Available at: https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite [Accessed 11 September 2026].
//Postman, 2026a. Define variables in Postman. [Online] Available at: https://learning.postman.com/latest-v-12/docs/use/send-requests/variables/define-variables [Accessed 12 September 2026].
//Postman, 2026b. Export data from Postman. [Online] Available at: https://learning.postman.com/docs/getting-started/importing-and-exporting/exporting-data [Accessed 12 September 2026].
//Postman, 2026. Write scripts to test API response data in Postman. [Online] Available at: https://learning.postman.com/docs/tests-and-scripts/write-scripts/test-scripts [Accessed 10 September 2026].
//Postman, 2026c. Test your API using the Collection Runner. [Online] Available at: https://learning.postman.com/docs/tests-and-scripts/running-collections/intro-to-collection-runs [Accessed 10 September 2026].