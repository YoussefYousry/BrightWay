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
    public class PublicationRepository : RepositoryBase<Publication>, IPublicationRepository
    {
        private AppDbContext _appDbContext { get; set; }
        private IFilesManager _filesManager { get; set; }
        private IMapper _mapper { get; set; }
        public PublicationRepository(AppDbContext context,IMapper mapper,IFilesManager filesManager) : base(context)
        {
            _appDbContext = context;
            _mapper = mapper;
            _filesManager = filesManager;
        }
        public async Task<List<DocumentViewModel>> GetAllPublication(DocumentsParamters paramters)
        => await FindAll(false)
        .SearchPublication(paramters.SearchTerm!)
        .Skip((paramters.PageNumber - 1) * paramters.PageSize)
        .Take(paramters.PageSize)
        .Select(p => new DocumentViewModel
        {
            Description = p.Description,
            Title = p.Title,
            Id = p.Id,
            Price = p.Price,
            // FileBytes = _filesManager.GetFileBytes(p.FileUrl),
            ImageBytes = _filesManager.GetFileBytes(p.ImageUrl)
        })
       .ToListAsync();
        public async Task<DocumentViewModel?> GetPublication(int id)
            => await FindByCondition(p => p.Id == id, false)
            .Select(p => new DocumentViewModel
            {
                Description = p.Description,
                Title = p.Title,
                Id = p.Id,
                Price = p.Price,
                FileBytes = _filesManager.GetFileBytes(p.FileUrl),
                ImageBytes = _filesManager.GetFileBytes(p.ImageUrl)
            }).FirstOrDefaultAsync();
        public async Task<Publication> CreatePublication(PublicationForCreateDto publicationDto)
        {
            Publication Publication = _mapper.Map<Publication>(publicationDto);
           var result = await _appDbContext.Publications.AddAsync(Publication);
           await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<Publication?> GetPublicationEntity(int id)
          => await FindByCondition(p => p.Id == id, trackChanges: true).FirstOrDefaultAsync();
        public void DeletePublication(Publication publication) => Delete(publication);

        public async Task UploadFile(int productId, IFormFile file)
        {
            var product = await _appDbContext.Publications.FirstOrDefaultAsync(p => p.Id == productId);
            string url = _filesManager.UploadFiles(file);
            product!.FileUrl = url;
        }
        public async Task UploadImage(int productId, IFormFile file)
        {
            var product = await _appDbContext.Publications.FirstOrDefaultAsync(p => p.Id == productId);
            string url = _filesManager.UploadFiles(file);
            product!.ImageUrl = url;
        }
    }
}
