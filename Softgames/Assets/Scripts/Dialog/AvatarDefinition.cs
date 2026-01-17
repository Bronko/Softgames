using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

[Serializable]
public class AvatarDefinition
{
    public enum Positions
    {
        left,
        right
    }

    public event Action<Texture2D> avatarLoaded;
    
    public string Name;
    public string Url;
    [JsonConverter(typeof(StringEnumConverter))]
    public Positions Position;
    
    public Texture2D Texture { get; private set; }
    private bool isLoading;
    
    
    [OnDeserialized]
    private void OnDeserialized(StreamingContext context)
    {
        TryLoadTexture();
    }

    public async void TryLoadTexture()
    {
        if (isLoading)
            return;
        
        isLoading = true;
        
        var request = UnityWebRequestTexture.GetTexture(Url);
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