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
    public interface IPublicationRepository
    {
        Task<List<DocumentViewModel>> GetAllPublication(DocumentsParamters paramters);
        Task<DocumentViewModel?> GetPublication(int id);
        Task<Publication> CreatePublication(PublicationForCreateDto publicationDto);
        Task<Publication?> GetPublicationEntity(int id);
        void DeletePublication(Publication publication);
        Task UploadFile(int productId, IFormFile file);
        Task UploadImage(int productId, IFormFile file);
        Task<FileStream> GetPublicationFile(int pubId);
    }
}
