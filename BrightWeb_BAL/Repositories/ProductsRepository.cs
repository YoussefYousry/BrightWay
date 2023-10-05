using AutoMapper;
using BrightWeb_BAL.Contracts;
using BrightWeb_BAL.DTO;
using BrightWeb_BAL.Extentions;
using BrightWeb_BAL.RequestFeature;
using BrightWeb_BAL.ViewModels;
using BrightWeb_DAL.Data;
using BrightWeb_DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Repositories
{
    public class ProductsRepository : RepositoryBase<Product>, IProductsRepository
    {
        private  AppDbContext _appDbContext { get; set; }
        private IFilesManager _filesManager { get; set; }
        private IMapper _mapper {  get; set; }
        public ProductsRepository(AppDbContext context, IMapper mapper, IFilesManager filesManager) : base(context)
        {
            _appDbContext = context;
            _mapper = mapper;
            _filesManager = filesManager;
        }
        public async Task<List<DocumentViewModel>> GetAllProducts(DocumentsParamters paramters)
        => await FindAll(false)
        .SearchProducts(paramters.SearchTerm!)
        .Skip((paramters.PageNumber - 1) * paramters.PageSize)
        .Take(paramters.PageSize)
        .Select(p=>new DocumentViewModel
        {
            Description = p.Description,
            Title = p.Title,
            Id = p.Id,
            Price = p.Price,
           // FileBytes = _filesManager.GetFileBytes(p.FileUrl),
            ImageBytes = _filesManager.GetFileBytes(p.ImageUrl)
        })
       .ToListAsync();
        public async Task<DocumentViewModel?> GetProductWithoutFile(int productId)
            => await FindByCondition(p => p.Id == productId, false)
            .Select(p => new DocumentViewModel
            {
                Description = p.Description,
                Title = p.Title,
                Id = p.Id,
                Price = p.Price,
               // FileBytes = _filesManager.GetFileBytes(p.FileUrl),
                ImageBytes = _filesManager.GetFileBytes(p.ImageUrl)
            }).FirstOrDefaultAsync();
        public async Task<DocumentViewModel?> GetProductWithFile(int productId)
            => await FindByCondition(p => p.Id == productId, false)
            .Select(p => new DocumentViewModel
            {
                Description = p.Description,
                Title = p.Title,
                Id = p.Id,
                Price = p.Price,
                FileBytes = _filesManager.GetFileBytes(p.FileUrl),
                ImageBytes = _filesManager.GetFileBytes(p.ImageUrl)
            }).FirstOrDefaultAsync();
        public async Task<bool> ValidateStudentPay(string studentId , int productId)
        {
            ICollection<Product>? products = new List<Product>(); 
           products = await _appDbContext.Students.Where(s=>s.Id==studentId).Include(s=>s.Products).Select(s=>s.Products).FirstOrDefaultAsync();
           var check = products != null && products.Any(c=>c.Id==productId);
            return check;
        }
        public async Task AddProductToStudent(int productId,string studentId)
        {
           var product = await _appDbContext.Products.Where(p=>p.Id==productId).FirstOrDefaultAsync();
            var student = await _appDbContext.Students.Where(s => s.Id == studentId).Include(p=>p.Products).FirstOrDefaultAsync();
             student!.Products.Add(product!);

        }
        public async Task<Product> CreateProduct(ProductForCreateDto productDto)
        {
            Product Product = _mapper.Map<Product>(productDto);
           var result= await _appDbContext.Products.AddAsync(Product);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<Product?> GetProduct(int id)
          => await FindByCondition(p=>p.Id == id,trackChanges:true).FirstOrDefaultAsync();
        public void DeleteProduct(Product product) => Delete(product);

        public async Task UploadFile(int productId,IFormFile file)
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync(p=>p.Id == productId);
           string url =  _filesManager.UploadFiles(file);
            product!.FileUrl = url;
        }
        public async Task UploadImage(int productId,IFormFile file)
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync(p=>p.Id == productId);
           string url =  _filesManager.UploadFiles(file);
            product!.ImageUrl = url;
        }

    }
}
