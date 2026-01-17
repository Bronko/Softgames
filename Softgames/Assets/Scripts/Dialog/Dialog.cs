using System.Threading.Tasks;
using Newtonsoft.Json;
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

    private bool debugFail = false;

    private async Task LoadJsonAsync()
    {
        var request = UnityWebRequest.Get(MagicWordsUrl);

        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        LoadingObj.SetActive(false);

        if (debugFail || request.result != UnityWebRequest.Result.Success)
        {
            debugFail = false;
            NotLoadedPanel.SetActive(true);
        }
        else
        {
            DialogPanel.gameObject.SetActive(true);
            var json = request.downloadHandler.text;
            var dialog = JsonConvert.DeserializeObject<MagicWords>(json);
            DialogPanel.Display(dialog);
        }
    }

    public override string GetDescription()
    {
        return "Most of the magic happens directly after json deserialization" +
               "\n" +
               "These names and string formats are super arbitrary. In a real life scenario I would have a friendly" +
               "word with design to actually use damn Unicode, after the assignment already even mentioned it. <sprite name =\"intrigued\">\n" +
               "Depending on the actual use case and time constraints, I would go for a third party plugin," +
               "or dive deep to dynamically fill a system rendered emoji atlas. I have seen that done," +
               "but it took a year for the developer to remove all the kinks.\n" +
               "\n" +
               "To improve: Use nicer emoji";
    }
}

