using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models.Interfaces;
namespace Project.Models
{
    public abstract class ImageUpload : IUpload
    {
        protected abstract Task<Uri> streamUpload(string name, IFormFile file);
        protected abstract Task<bool> streamRemove(string path);
     

        public Uri Upload(IFormFile file)
        {
            if (file == null)
                return null;

            
            var name = file.FileName;

            if (file.ContentType == "image/jpeg")
                return streamUpload(name, file).Result;
            else
                return null;
        }
    }
}
