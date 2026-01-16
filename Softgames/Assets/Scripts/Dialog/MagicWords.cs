using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class MagicWords
{
    public List<DialogEntry> dialogue;
    public List<AvatarDefinition> avatars;
    
    public Dictionary<string, AvatarDefinition> Avatars = new();
    
    [OnDeserialized]
    internal void OnDeserialized(StreamingContext context)
    {
        foreach (var definition in avatars)
        {
            if (!Avatars.TryAdd(definition.name, definition))
            {
                Debug.LogWarning($"Double definition of {definition.name}");
            }
        }
    }
}