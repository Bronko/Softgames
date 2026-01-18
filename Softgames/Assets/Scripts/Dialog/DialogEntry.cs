using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using UnityEngine.Serialization;

[Serializable]
public class DialogEntry
{
    public string Name = "Name";
    public string Text = "...";
    
    [OnDeserialized]
    internal void OnDeserialized(StreamingContext context)
    {
        var pattern = @"\{(.*?)\}";
        var replacement = "<sprite name=\"$1\">";

        Text = Regex.Replace(Text, pattern, replacement);
    }
}