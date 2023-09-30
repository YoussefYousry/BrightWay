using AutoMapper;
using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.DTO;
using BrightWeb_BAL.RequestFeature;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrightWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        public DocumentsController(IRepositoryManager repositoryManager,IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }
        [HttpGet("Products")]
        public async Task<IActionResult> GetAllProducts([FromQuery]DocumentsParamters paramters)
        {
            var products = await _repositoryManager.Products.GetAllProducts(paramters);
            return Ok(products);
        }
        [HttpGet("Products/{productId}")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var product = await _repositoryManager.Products.GetProductWithoutFile(productId);
            if(product is null)
            {
                return NotFound($"Product with id {productId} Not Found");
            }
            return Ok(product);
        }
        [HttpPost("Product")]
        public async Task<IActionResult> CreateProduct(ProductForCreateDto product)
        {
            if(!ModelState.IsValid)
            {
                 return BadRequest($"Something Wrong in Model{ModelState}"); ;
            }
           var productEntity= await _repositoryManager.Products.CreateProduct(product);
            return StatusCode(201,new {
                ProductId = productEntity.Id,
            });
        }
        [HttpPost("Product/UploadFile/{productId}")]
        public async Task<IActionResult> UploadFile(int productId, [FromForm] IFormFile file)
        {
            try
            {
                await _repositoryManager.Products.UploadFile(productId, file);
                return StatusCode(201);
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
            
        }
        [HttpPost("Product/UploadImage/{productId}")]
        public async Task<IActionResult> UploadImage(int productId, [FromForm] IFormFile file)
        {
            try
            {
                await _repositoryManager.Products.UploadImage(productId, file);
                return StatusCode(201);
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
            
        }
        [HttpPut("AddProductToStudent")]
        public async Task<IActionResult> AddProductToStudent(int productId,string studentId)
        {
            try
            {
                await _repositoryManager.Products.AddProductToStudent(productId, studentId);
                return StatusCode(201);
            }
            catch(Exception ex)
            {
                return StatusCode(201,ex.Message);
            }
            
        }
        [HttpGet("Publications")]
        public async Task<IActionResult> GetAllPublications([FromQuery] DocumentsParamters paramters)
        {
            var publications = await _repositoryManager.Publications.GetAllPublication(paramters);
            return Ok(publications);
        }
        [HttpGet("Publications/{publicationId}")]
        public async Task<IActionResult> GetPublication(int publicationId)
        {
            var publication = await _repositoryManager.Publications.GetPublication(publicationId);
            if (publication is null)
            {
                return NotFound($"Publication with id {publicationId} Not Found");
            }
            return Ok(publication);
        }
        [HttpPost("Publications")]
        public async Task<IActionResult> CreatePublication(PublicationForCreateDto publication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Something Wrong in Model {ModelState}");
            }
            var publicationEntity = await _repositoryManager.Publications.CreatePublication(publication);
            return StatusCode(201, new
            {
                PublicationId = publicationEntity.Id,
            });
        }
        [HttpPost("Publications/UploadFile/{productId}")]
        public async Task<IActionResult> UploadPublicationFile(int publicationid, [FromForm] IFormFile file)
        {
            try
            {
                await _repositoryManager.Publications.UploadFile(publicationid, file);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost("Publications/UploadImage/{productId}")]
        public async Task<IActionResult> UploadPublicationImage(int publicationid, [FromForm] IFormFile file)
        {
            try
            {
                await _repositoryManager.Publications.UploadImage(publicationid, file);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

    }
}
