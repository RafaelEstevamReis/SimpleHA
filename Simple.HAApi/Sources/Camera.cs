namespace Simple.HAApi.Sources;

using Simple.API;
using System.IO;
using System.Threading.Tasks;

public class Camera : SourceBase
{
    public Camera(ClientInfo info)
       : base(info)
    { }

    public async Task<byte[]> GetImageAsync(string cameraEntityId)
        => await GetAsync<byte[]>($"/api/camera_proxy/{cameraEntityId}");
    public async Task SaveCameraImageAsync(string cameraEntityId, string path)
    {
        var bytes = await GetImageAsync(cameraEntityId);
        File.WriteAllBytes(path, bytes);
    }

}
