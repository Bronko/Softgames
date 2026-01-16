using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class AvatarDefinition
{
    public enum Position
    {
        left,
        right
    }

    public event Action<Texture2D> avatarLoaded;
    public string name;
    public string url;
    
    [JsonConverter(typeof(StringEnumConverter))]
    public Position position;
    
    public Texture2D Texture { get; private set; }
    private bool isLoading;
    
    
    [OnDeserialized]
    internal void OnDeserialized(StreamingContext context)
    {
        TryLoadTexture();
    }

    public async void TryLoadTexture()
    {
        if (isLoading)
            return;
        
        isLoading = true;
        
        var request = UnityWebRequestTexture.GetTexture(url);
        var operation = request.SendWebRequest();

        while (!request.isDone)
            await Task.Yield();

        while (!operation.isDone)
            await Task.Yield();
        
        isLoading = false;
        
        if (request.result == UnityWebRequest.Result.Success)
             Texture = DownloadHandlerTexture.GetContent(request);
        
        avatarLoaded?.Invoke(Texture);
    }
}