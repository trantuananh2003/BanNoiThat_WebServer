using BanNoiThat.API.Model;
using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.DTOs.Product;
using BanNoiThat.Application.Service.Products.Commands.CreateProduct;
using BanNoiThat.Application.Service.Products.Commands.UpdatePatchProduct;
using BanNoiThat.Application.Service.Products.Commands.UpdateProductItems;
using BanNoiThat.Application.Service.Products.Queries.FindProduct;
using BanNoiThat.Application.Service.Products.Queries.GetProductsPaging;
using BanNoiThat.Application.Service.Products.Queries.GetProductsRecommend;
using BanNoiThat.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace BanNoiThat.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;
        private ApiResponse _apiResponse;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
            _apiResponse = new ApiResponse();
        }

        #region Both Admin and Client
        //Get product by "slug" or "id"
        [HttpGet("{value}")]
        public async Task<ActionResult<ApiResponse>> GetProductByIdentityAsync([FromRoute] string value)
        {
            var createModel = new FindProductQuery() { IdentityValue = value };
            var modelResponse = await _mediator.Send(createModel);

            _apiResponse.Result = modelResponse;
            _apiResponse.StatusCode = HttpStatusCode.OK;

            return _apiResponse;
        }

        //Get product for paged list
        [HttpPost("recommend")]
        public async Task<ActionResult<ApiResponse>> GetPagedListProductRecommendAsync(int pageCurrent, int pageSize, string? stringSearch, [FromForm] RecommendRequest model)
        {
            GetPagedProductsRecommendQuery queryPagedProduct = new GetPagedProductsRecommendQuery
            {
                PageCurrent = pageCurrent,
                PageSize = pageSize,
                StringSearch = stringSearch,
                InteractedProductIds = model.InteractedProductIds.ToArray(),
            };

            var pagedProductModel = await _mediator.Send(queryPagedProduct);

            PaginationDto pagination = new PaginationDto()
            {
                CurrentPage = pagedProductModel.PageCurrent,
                PageSize = pagedProductModel.PageSize,
                TotalRecords = pagedProductModel.TotalCount,
            };

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));
            _apiResponse.Result = pagedProductModel.Items;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
        #endregion

        #region Admin
        //Create product
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateProductAsync([FromForm] CreateProductsCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }

        //Get product for paged list
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetPagedListProductAsync([FromQuery] GetPagedProductsQuery queryPagedProduct)
        {
            var pagedProductModel = await _mediator.Send(queryPagedProduct);

            PaginationDto pagination = new PaginationDto()
            {
                CurrentPage = pagedProductModel.PageCurrent,
                PageSize = pagedProductModel.PageSize,
                TotalRecords = pagedProductModel.TotalCount,
            };

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));
            _apiResponse.Result = pagedProductModel.Items;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        //Update product
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> UpdateProductByIdAsync([FromRoute]string id, [FromForm] UpdateProductRequest modelUpdateRequest)
        {
            await _mediator.Send(new UpdateProductCommand() { Id = id, updateProductRequest = modelUpdateRequest });
            return Ok();
        }

        //Cập nhập product item trong product id
        [HttpPut("{productId}/product-items")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> UpdateProductItems([FromRoute]string productId, [FromBody] List<ProductItemRequest> items)
        {
            var command = new UpsertProductItemsCommand() {
                ProductId = productId,
                ListProductItems = items
            };

            await _mediator.Send(command);
            return NoContent();
        }
        #endregion

        #region Client 

        #endregion
    }
}