using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class MagicWords
{
    public List<DialogEntry> Dialogue;
    public List<AvatarDefinition> Avatars;
    
    public Dictionary<string, AvatarDefinition> AvatarDict = new();
    
    [OnDeserialized]
    private void OnDeserialized(StreamingContext context)
    {
        foreach (var definition in Avatars)
        {
            if (!AvatarDict.TryAdd(definition.Name, definition))
            {
                Debug.LogWarning($"Double definition of {definition.Name}");
                AvatarDict[definition.Name] = definition; 
                //I would usually take the first definition, but the second Sheldon 
                //is nicer.
                //Of course, I could try all definitions to find the first one not broken.
                //In a real life scenario I would advice against that kind of complexity.
            }
        }
    }
}