using System.Security.Claims;
using System.Reflection;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using deepu_dating.API.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace deepu_dating.API.Repo
{
    public class PhotoService : IPhotoService
    {
          private readonly Cloudinary _Cloudninary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
               config.Value.CloudName,
               config.Value.ApiKey,
               config.Value.ApiSecret
            );

            _Cloudninary = new Cloudinary(acc);

            
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile form)
        {
            var imageUploadResult = new ImageUploadResult();

            if(form.Length >0){
                  
                  using var stream = form.OpenReadStream();

                  var imageUploadParams = new ImageUploadParams{
                      File = new FileDescription(form.FileName,stream),
                      Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                      .Gravity("face")
                  };

                  imageUploadResult =await _Cloudninary.UploadAsync(imageUploadParams);

            }

            return imageUploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
           var deleteParams = new DeletionParams(publicId);

           var deletionResult = await _Cloudninary.DestroyAsync(deleteParams);

           return deletionResult ;
        }
    }
}