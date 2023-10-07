using AutoMapper;
using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.DTO;
using BrightWeb_BAL.RequestFeature;
using BrightWeb_BAL.ViewModels;
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
        [HttpDelete("Product/{productId}")]
        public async Task<IActionResult> DeleteCourse(int productId)
        {
            var product = await _repositoryManager.Products.GetProduct(productId);
            if (product is null)
            {
                return NotFound($"Product with ID: {productId} doesn't exist in the database ");
            }
            _repositoryManager.Products.DeleteProduct(product);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("Product/{productId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductForUpdateDto product
            , int productId)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var productEntity = await _repositoryManager.Products.GetProduct(productId);
            if (productEntity is null)
            {
                return NotFound($"Product with ID: {productId} doesn't exist in the database ");
            }
            _mapper.Map(product, productEntity);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();

        }
        [HttpPost("Product/UploadFile/{productId}")]
        public async Task<IActionResult> UploadFile(int productId, [FromForm] FileToUploadViewModel fileVM)
        {
            try
            {
                await _repositoryManager.Products.UploadFile(productId, fileVM.File);
                await _repositoryManager.SaveChangesAsync();
                return StatusCode(201);
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
            
        }
        [HttpPost("Product/UploadImage/{productId}")]
        public async Task<IActionResult> UploadImage(int productId, [FromForm] FileToUploadViewModel fileVM)
        {
            try
            {
                await _repositoryManager.Products.UploadImage(productId, fileVM.File);
                await _repositoryManager.SaveChangesAsync();
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
                await _repositoryManager.SaveChangesAsync();
                return StatusCode(201);
            }
            catch(Exception ex)
            {
                return StatusCode(201,ex.Message);
            }
            
        }
        [HttpGet("Product/GetFile/{productId}")]
        public async Task<IActionResult> GetProductFile(int productId)
        {
            var product = await _repositoryManager.Products.GetProduct(productId);
            if(product is null)
            {
                return NotFound();
            }
            FileStream file = await _repositoryManager.Products.GetProductFile(productId);
            return new FileStreamResult(file,"application/pdf");
        } 
        [HttpGet("Publication/GetFile/{publicationId}")]
        public async Task<IActionResult> GetPublicationFile(int publicationId)
        {
            var pub = await _repositoryManager.Publications.GetPublication(publicationId);
            if(pub is null)
            {
                return NotFound();
            }
            FileStream file = await _repositoryManager.Products.GetProductFile(publicationId);
            return new FileStreamResult(file,"application/pdf");
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
        [HttpPost("Publications/UploadFile/{publicationid}")]
        public async Task<IActionResult> UploadPublicationFile(int publicationid, [FromForm] FileToUploadViewModel fileVM)
        {
            try
            {
                await _repositoryManager.Publications.UploadFile(publicationid, fileVM.File);
                await _repositoryManager.SaveChangesAsync();
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost("Publications/UploadImage/{publicationid}")]
        public async Task<IActionResult> UploadPublicationImage(int publicationid, [FromForm] FileToUploadViewModel fileVM)
        {
            try
            {
                await _repositoryManager.Publications.UploadImage(publicationid, fileVM.File);
                await _repositoryManager.SaveChangesAsync();
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpDelete("Publication/{publicationId}")]
        public async Task<IActionResult> DeletePublication(int publicationId)
        {
            var publication = await _repositoryManager.Publications.GetPublicationEntity(publicationId);
            if (publication! is null)
            {
                return NotFound($"Publication with ID: {publicationId} doesn't exist in the database ");
            }
            _repositoryManager.Publications.DeletePublication(publication!);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("Publication/{publicationId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePublication([FromBody] PublicationForUpdateDto publication
            , int publicationId)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var publicationEntity = await _repositoryManager.Publications.GetPublicationEntity(publicationId);
            if (publicationEntity is null)
            {
                return NotFound($"publication with ID: {publicationId} doesn't exist in the database ");
            }
            _mapper.Map(publication!, publicationEntity!);
            await _repositoryManager.SaveChangesAsync();
            return NoContent();

        }

    }
}
