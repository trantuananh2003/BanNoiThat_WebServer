using BanNoiThat.API.Model;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.ProductDtos;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.OutService;
using BanNoiThat.Application.Service.Products.Commands.CreateProduct;
using BanNoiThat.Application.Service.Products.Commands.UpdatePatchProduct;
using BanNoiThat.Application.Service.Products.Commands.UpdateProductItems;
using BanNoiThat.Application.Service.Products.Queries.FindProduct;
using BanNoiThat.Application.Service.Products.Queries.GetProductsPaging;
using BanNoiThat.Application.Service.Products.Queries.GetProductsRecommend;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUnitOfWork _uow;
        private IBlobService _blob;
        private IServiceProduct _serviceProduct;

        public ProductsController(IMediator mediator, IUnitOfWork uow, IBlobService blob
            , IServiceProduct serviceProduct)
        {
            _mediator = mediator;
            _uow = uow;
            _blob = blob;
            _serviceProduct = serviceProduct;
            _apiResponse = new ApiResponse();
        }

        //Get product by "slug" or "id"
        [HttpGet("{value}")]
        public async Task<ActionResult<ApiResponse>> GetProductByIdentityAsync([FromRoute] string value)
        {
            var createModel = new FindProductQuery() { IdentityValue = value };
            var modelResponse = await _mediator.Send(createModel);

            _apiResponse.Result = modelResponse;
            _apiResponse.StatusCode = HttpStatusCode.OK;

            return Ok(_apiResponse);
        }

        //Get product item by "slug" or "id"
        [HttpGet("product-items/{productItemId}")]
        public async Task<ActionResult<ApiResponse>> GetProductItemsAsync([FromRoute] string productItemId)
        {
            var model = await _uow.ProductRepository.GetProductItemByIdAsync(productItemId);

            _apiResponse.Result = model;
            _apiResponse.StatusCode = HttpStatusCode.OK;

            return Ok(_apiResponse);
        }

        //Get product for paged list
        [HttpGet]
        [AllowAnonymous]
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
        public async Task<ActionResult<ApiResponse>> UpdateProductItems([FromRoute]string productId, [FromForm] List<ProductItemRequest> items)
        {
            if(!items.Any() || string.IsNullOrEmpty(productId))
            {
                return BadRequest();
            }

            var command = new UpsertProductItemsCommand() {
                ProductId = productId,
                ListProductItems = items
            };

            await _mediator.Send(command);
                
            return Ok();
        }

        //Create product
        [HttpPost]
        public async Task<ActionResult> CreateProductAsync([FromForm] CreateProductsCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }

        //Get product for paged list
        [HttpPost("recommend")]
        public async Task<ActionResult<ApiResponse>> GetPagedListProductRecommendAsync(int pageCurrent, int pageSize, string? stringSearch,
            [FromForm] RecommendRequest? model)
        {
            
            var productIds = new List<string>();

            if(model.IsSpecial)
            {
                productIds.Add(model.InteractedProductId);
            }
            else
            {
                string? userInteractionJson = Request.Cookies["userInteraction"];
                productIds = JsonSerializer.Deserialize<List<string>>(userInteractionJson) ?? new List<string>();
            }

            GetPagedProductsRecommendQuery queryPagedProduct = new GetPagedProductsRecommendQuery
            {
                PageCurrent = pageCurrent,
                PageSize = pageSize,
                StringSearch = stringSearch,
                InteractedProductIds = productIds,
            };

            var pagedProductModel = await _serviceProduct.HandleRecommend(queryPagedProduct);

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

        [HttpDelete("{productId}")]
        public async Task<ActionResult<ApiResponse>> DeleteSoftProduct(string productId, [FromQuery] Boolean isDeleted = true)
        {
            await _uow.ProductRepository.DeleteSoft(productId, isDeleted);
            await _uow.SaveChangeAsync();
            return Ok();
        }

        #region model 3d
        [HttpPut("/api/product-items/{productItemId}/model")]
        public async Task<ActionResult<ApiResponse>> UpdateFilesModel3D([FromRoute] string productItemId, [FromForm] ProductModelRequest model)
        {
            var entityProductItem = await _uow.ProductRepository.GetProductItemByIdAsync(productItemId);

            if (model.model3DFile != null && model.model3DFile.Length > 0)
            {
                if (entityProductItem != null && !string.IsNullOrEmpty(entityProductItem.ModelUrl))
                    await _blob.DeleteBlob(entityProductItem.ModelUrl, StaticDefine.SD_Storage_Containter);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.model3DFile.FileName)}";
                entityProductItem.ModelUrl = await _blob.UploadBlob(model.model3DFile.FileName, "bannoithat-3dmodel", model.model3DFile);
            }

            await _uow.SaveChangeAsync();

            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;

            return Ok(_apiResponse);
        }
        #endregion 3D
        
        [HttpGet("/productsAI")]
        public async Task<ActionResult<ApiResponse>> GetProductForAI()
        {
            var listProduct = await _uow.ProductRepository
                .GetAllAsync(x => x.ProductItems.Any(x => x.Quantity > 0), includeProperties: "ProductItems,Category,Brand");

            // Ánh xạ danh sách sản phẩm, loại bỏ Description
            var productList = listProduct.Select(product => new
            {
                Id = product.Id,
                Category = new
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name
                },
                Brand = new
                {
                    Id = product.Brand.Id,
                    Name = product.Brand.Name
                },
                Name = product.Name,
                ThumbnailUrl = product.ThumbnailUrl,
                Slug = product.Slug,
                CreateAt = product.CreateAt,
                IsDeleted = product.IsDeleted,
                Rate = product.Rate,
                ProductItems = product.ProductItems.Select(item => new
                {
                    Id = item.Id,
                    Quantity = item.Quantity
                }).ToList()
            }).ToList();

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.Result = productList;

            return Ok(_apiResponse);
        }

    }
}