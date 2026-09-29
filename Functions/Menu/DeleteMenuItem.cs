using Azure.Data.Tables;
using POE_CoffeeNChill.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging; 

namespace POE_CoffeeNChill.Functions.Menu;

public class DeleteMenuItem
{
    private readonly ILogger<DeleteMenuItem> _logger;
    private readonly TableClient _tableClient;

    public DeleteMenuItem(ILogger<DeleteMenuItem> logger, TableClient tableClient)
    {
        _logger = logger;
        _tableClient = tableClient;
    }

    [Function("DeleteMenuItem")]
    public async Task<IActionResult> DeleteItem(
        [HttpTrigger( AuthorizationLevel.Anonymous, "delete", Route = "menu/{category}/{Id}")]
        HttpRequest request,string category, string Id)
    {
        try
        {
            var normalizedCategory = MenuCategoryHelper.Normalize(category);

            if (!MenuCategoryHelper.IsValid(normalizedCategory))
            {
                return new BadRequestObjectResult("A valid category is required.");
            }

            if (string.IsNullOrWhiteSpace(Id))
            {
                return new BadRequestObjectResult("A menu item ID is required.");
            }

            var response = await _tableClient.GetEntityIfExistsAsync<MenuItems>(normalizedCategory, Id);

            if (!response.HasValue)
            {
                return new NotFoundObjectResult($"Menu item '{Id}' was not found.");
            }

            var menuItem = response.Value;

            await _tableClient.DeleteEntityAsync(menuItem.PartitionKey, menuItem.RowKey, menuItem.ETag);

            _logger.LogInformation("Deleted menu item {MenuId} from {Category}.", Id, normalizedCategory);

            return new NoContentResult();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting menu item {MenuId}.", Id);

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);  // Posts the error message
        }
    }
}