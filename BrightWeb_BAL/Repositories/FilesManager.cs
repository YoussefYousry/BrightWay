﻿using BrightWeb_BAL.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Repositories
{
    public class FilesManager : IFilesManager
    {
        public FileStream GetFile(string fileName)
        {
            var folderName = Path.Combine("Resources", "Media");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var dbPath = Path.Combine(pathToSave, fileName);
            var fileStream = new FileStream(dbPath,FileMode.Open,FileAccess.Read);
            return fileStream;
        }
        public void DeleteFile(string fileName)
        {
            var folderName = Path.Combine("Resources", "Media");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory (), folderName);
            var dbPath = Path.Combine(pathToSave, fileName);
            if (File.Exists(dbPath))
            { File.Delete(dbPath); }
        }
        public byte[]? GetFileBytes(string fileName)
        {
            try
            {
                var folderName = Path.Combine("Resources", "Media");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var dbPath = Path.Combine(pathToSave, fileName);
                byte[] fileBytes = File.ReadAllBytes(dbPath);
                return fileBytes;
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }
        public string UploadFiles(IFormFile file)
        {
            var folderName = Path.Combine("Resources", "Media");
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            var fileName = Guid.NewGuid().ToString() +
                ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName!.Trim('"');
            var fullPath = Path.Combine(pathToSave, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }
    }
}
