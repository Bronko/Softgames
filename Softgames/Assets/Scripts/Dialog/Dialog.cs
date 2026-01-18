using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Dialog : AssignmentScreen
{
    private static readonly string MagicWordsUrl =
        "https://private-624120-softgamesassignment.apiary-mock.com/v3/magicwords";

    public GameObject LoadingObj;
    public GameObject NotLoadedPanel;
    public DialogPanel DialogPanel;
    public Button RetryButton;

    void Awake()
    {
        RetryButton.onClick.AddListener(TryLoadJson);
        TryLoadJson();
    }

    private async void TryLoadJson()
    {
        LoadingObj.SetActive(true);
        NotLoadedPanel.SetActive(false);
        DialogPanel.gameObject.SetActive(false);
        await LoadJsonAsync();
    }

    public bool DebugFailFirst = false;

    private async Task LoadJsonAsync()
    {
        var request = UnityWebRequest.Get(MagicWordsUrl);

        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        LoadingObj.SetActive(false);

        if (DebugFailFirst || request.result != UnityWebRequest.Result.Success)
        {
            DebugFailFirst = false;
            NotLoadedPanel.SetActive(true);
        }
        else
        {
            var json = request.downloadHandler.text;
            try
            {
                var dialog = JsonConvert.DeserializeObject<MagicWords>(json, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver() //Following my own code conventions the "hard" way. ^^
                });
                DialogPanel.gameObject.SetActive(true);
                DialogPanel.Display(dialog);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Json parsing error {(e.InnerException != null ? e.InnerException.Message : e.Message)}");
                MessagePopup.Show("Ooops! Retrieving your dialog data didn't work out this time. Please try again later. \n" +
                                  "Did I say \"retrieving\"? I lied. It was totally retrieved, but we broke something. <sprite name=\"satisfied\">\n" +
                                  "But we would never actually admit it user facing like this now, would we? \n" +
                                  "\n" + 
                                  "Don't worry, I know how easily something like this can get shipped. I will not be like " +
                                  "this in project code, promise.^^");
            }
        }
    }

    public override string GetDescription()
    {
        return "Most of the magic happens directly after json deserialization triggered [OnDeserialized] methods" +
               "\n" +
               "These names and formats were super arbitrary. In a real life scenario I would have a friendly" +
               "word with design to actually use Unicode, after it was even already mentioned. <sprite name=\"intrigued\">\n" +
               "\n" +
               "Depending on the actual use case and time constraints, I would go for a third party plugin," +
               "or do the deep dive to dynamically fill a system rendered emoji atlas. I have seen that done," +
               "but it took a year for that developer to remove all the kinks.\n" +
               "\n" +
               "To improve: Use nicer emoji (Those free ones I took admittedly are... not pretty). Allow going back in dialog (But games usually don't). Animate the text slowly building up with skip button, etc.";
    }
}

