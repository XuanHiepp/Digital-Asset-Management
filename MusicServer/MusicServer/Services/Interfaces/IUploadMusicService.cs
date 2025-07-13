using Microsoft.AspNetCore.Http;
using MusicServer.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Services.Interfaces
{
    public interface IUploadMusicService
    {
        Task<(string, string)> UploadFileToAzureBlob(IFormFile file, ResourceTypes resourceType = ResourceTypes.Other);

        void DeleteFileInAzureBlob(string blobName, ResourceTypes resourceType = ResourceTypes.Other);

        void CopyFileEncryptAndUploadToAzureBlob(string blobName, string password, ResourceTypes resourceType = ResourceTypes.Other);

        string CopyFileEncryptAndUploadToAzureBlobOwnerShip(string blobName, string old_password, string new_password, ResourceTypes resourceType = ResourceTypes.Other);

        string SaveAzureBlobInfoToDatabaseAndReturnKey(string blobName, string uri);

        string GetFileUri(string fileName);

        void AES_Encrypt(string inputFile, string outputFile, string password);

        void AES_Decrypt(string inputFile, string outputFile, string password);
    }
}
