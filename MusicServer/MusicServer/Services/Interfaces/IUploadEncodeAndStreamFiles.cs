using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Services.Interfaces
{
    public interface IUploadEncodeAndStreamFiles
    {
        Task<string> RunAsync(ConfigWrapper config, string blobName);
        Task<ServiceClientCredentials> GetCredentialsAsync(ConfigWrapper config);
        Task<IAzureMediaServicesClient> CreateMediaServicesClientAsync(ConfigWrapper config);
        Task<Asset> CreateInputAssetAsync(
            IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string assetName,
            string fileToUpload);
        Task<Asset> CreateOutputAssetAsync(IAzureMediaServicesClient client, string resourceGroupName, string accountName, string assetName);
        Task<Transform> GetOrCreateTransformAsync(
            IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string transformName);
        Task<Job> SubmitJobAsync(IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string transformName,
            string jobName,
            string inputAssetName,
            string outputAssetName);
        Task<Job> WaitForJobToFinishAsync(IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string transformName,
            string jobName);
        Task<StreamingLocator> CreateStreamingLocatorAsync(
            IAzureMediaServicesClient client,
            string resourceGroup,
            string accountName,
            string assetName,
            string locatorName);
        Task<IList<string>> GetStreamingUrlsAsync(
            IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            String locatorName);
        Task CleanUpAsync(
            IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string transformName,
            string contentKeyPolicyName,
            List<string> assetNames,
            string jobName);
    }
}
