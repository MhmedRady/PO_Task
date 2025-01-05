using Aswaq.Api.Controllers;
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

        [HttpPost]
        public async Task<Guid> Add(AddPurchaseOrderRequest request)
        {
            AddPurchaseOrderCommand command = request;
            Guid poId = await _sender.Send(command);
            return poId;
        }

        [HttpPost("create-multiple")]
        public async Task<IActionResult> CreateMultiple([FromBody] BulkPurchaseOrderCreateRequest request)
        {
            var listPOIDs = await _sender.Send(request);
            return Ok(listPOIDs);
        }

        [HttpPut("AddOrderItem/{OrderId:guid}")]
        public async Task<bool> AddPurchaseOrderItem(Guid OrderId, AddPurchaseOrderItemRequest request)
        {
            
            AddPurchaseOrderItemCommand command = request;
            return await _sender.Send(command);
        }

        /*[HttpDelete("DeleteOrderItem/{id:guid}")]
        public async Task<Guid> DeletePurchaseOrderItem(PurchaseOrderItemRequest request)
        {
            AddPurchaseOrderCommand command = request;

            Guid poId = await _sender.Send(command);

            return poId;
        }*/

    }
}
