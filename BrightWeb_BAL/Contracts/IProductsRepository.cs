using BrightWeb_BAL.DTO;
using BrightWeb_BAL.RequestFeature;
using BrightWeb_BAL.ViewModels;
using BrightWeb_DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Contracts
{
    public interface IProductsRepository
    {
        Task<List<DocumentViewModel>> GetAllProducts(DocumentsParamters paramters);
        Task<DocumentViewModel?> GetProductWithoutFile(int productId);
        Task<DocumentViewModel?> GetProductWithFile(int productId);
        Task<bool> ValidateStudentPay(string studentId, int productId);
        Task<Product> CreateProduct(ProductForCreateDto productDto);
        Task<Product?> GetProduct(int id);
       void DeleteProduct(Product product);
        Task UploadFile(int productId, IFormFile file);
        Task UploadImage(int productId, IFormFile file);
        Task AddProductToStudent(int productId, string studentId);

    }
}
