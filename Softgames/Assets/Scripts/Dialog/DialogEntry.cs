using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public class DialogEntry
{
    public string name = "Name";
    public string text = "...";
    
    [OnDeserialized]
    internal void OnDeserialized(StreamingContext context)
    {
        var pattern = @"\{(.*?)\}";
        var replacement = "<sprite name=\"$1\">";

        text = Regex.Replace(text, pattern, replacement);
    }
}