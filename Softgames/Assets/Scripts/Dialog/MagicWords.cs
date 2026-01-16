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
                Avatars[definition.name] = definition; 
                //I would usually take the first definition, but the second Sheldon 
                //is nicer.
                //Of course I could try all definitions, to find the first one not broken.
                //In a real life scenario I would advice against that kind of complexity.
            }
        }
    }
}