using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace deepu_dating.API.Repo
{
    public interface IPhotoService
    {
         Task<ImageUploadResult> AddPhotoAsync(IFormFile form);

         Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}