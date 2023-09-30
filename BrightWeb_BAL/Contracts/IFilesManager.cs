using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Contracts
{
    public interface IFilesManager
    {
        FileStream GetFile(string fileName);
        byte[]? GetFileBytes(string fileName);
        string UploadFiles(IFormFile file);
        void DeleteFile(string fileName);
    }
}
