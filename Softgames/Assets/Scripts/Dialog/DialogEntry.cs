using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

[Serializable]
public class DialogEntry
{
    public string name = "Name";
    public string text = "...";
    
    [OnDeserialized]
    internal void OnDeserialized(StreamingContext context)
    {
        //These names and string formats are super arbitrary. In a real life scenario I would have a friendly
        //word with design to actually use damn Unicode, after the assignment already even mentioned it. :-)
        //Depending on the actual use case and time constraints, I would go for a third party plugin,
        //or dive deep to dynamically fill a system rendered emoji atlas. I have seen that done,
        //but it took a year for the developer to remove all the kinks.
        var pattern = @"\{(.*?)\}";
        var replacement = "<sprite name=\"$1\">";

        text = Regex.Replace(text, pattern, replacement);
    }
}