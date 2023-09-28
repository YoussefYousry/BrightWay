using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Contracts
{
    public interface IFilesRepository
    {
        void UploadImageToUser(string userId, IFormFile img);
        Task<FileStream> GetImageToUser(string userId);


    }
}
