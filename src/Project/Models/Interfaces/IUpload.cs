using Microsoft.AspNetCore.Http;
using System;

namespace Project.Models.Interfaces
{
    public interface IUpload
    {
        Uri Upload(IFormFile file); 
    }
}
