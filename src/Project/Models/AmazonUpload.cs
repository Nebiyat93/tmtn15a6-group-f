using System;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Amazon.S3.Model;
namespace Project.Models
{
    public class AmazonUpload : ImageUpload
    {
            
            static IAmazonS3 _client; 
            private string _bucketName = "mjecipesstorage";
            private string _hostName = "https://s3.eu-central-1.amazonaws.com";
        private string folderName = "Images/";
        /// <summary>
        /// FIXA CREDENTIALS.
        /// </summary>
        private string awsAccessKeyId = "sampletext";
        private string awsSecretAccessKey = "Hola Senorita ";

        public AmazonUpload() 
            {
                _client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, Amazon.RegionEndpoint.EUCentral1);
            }

            protected override async Task<Uri> streamUpload(string name, IFormFile file)
            {
                var request = await _client.PutObjectAsync(new PutObjectRequest()
                {
                    
                    BucketName = _bucketName,
                    Key = folderName + name,
                    InputStream = file.OpenReadStream(),
                    ContentType = file.ContentType
                });
                return new Uri(_hostName + "/" + _bucketName + "/" + folderName + name);
            }

            protected override async Task<bool> streamRemove(string path)
            {
                var request = await _client.DeleteObjectAsync(new DeleteObjectRequest()
                {
                    BucketName = _bucketName,
                    Key = folderName + path
                });

            return request.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
            }


        }
    }

