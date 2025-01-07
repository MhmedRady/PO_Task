using Aswaq.Api.Controllers;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PO_Task.Application.PurchaseOrders;

namespace PO_Task.Api.Controllers
{
    [ApiController]
    [Route($"api/v{ApiVersions.V1}/PurchaseOrders")]

    public class PurchaseOrderController : ControllerBase
    {
        private readonly ISender _sender;

        public PurchaseOrderController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Add([FromBody] AddPurchaseOrderRequest request)
        {
            AddPurchaseOrderCommand command = request;
            var result = await _sender.Send(command);
            return Ok(result);
        }

        [HttpPost("create-multiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] BulkPurchaseOrderCreateRequest requests)
        {
            BulkPurchaseOrderCreateCommand bulkPurchaseOrderCommand = requests;
            var listPOIDs = await _sender.Send(bulkPurchaseOrderCommand);
            return !listPOIDs.Any()? BadRequest(): Ok(listPOIDs);
        }
    }
}
