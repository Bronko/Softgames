using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;


public class Dialog : AssignmentScreen
{
    private static readonly string MagicWordsUrl =
        "https://private-624120-softgamesassignment.apiary-mock.com/v3/magicwords";

    public GameObject Loading;
    public GameObject NoLoad;
    public GameObject DialogPanel;
    public Button RetryButton;
    void Awake()
    {
        RetryButton.onClick.AddListener(TryLoadJson);
        
        TryLoadJson();
    }

    private async void TryLoadJson()
    {
        Loading.SetActive(true);
        NoLoad.SetActive(false);
        await LoadJsonAsync();
    }

    private async Task LoadJsonAsync()
    {
        var request = UnityWebRequest.Get(MagicWordsUrl);

        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        Loading.SetActive(false);
        if (request.result != UnityWebRequest.Result.Success)

        {
            NoLoad.SetActive(true);
        }
        else
        {
            DialogPanel.SetActive(true);
            var json = request.downloadHandler.text;
            var dialog = JsonConvert.DeserializeObject<MagicWords>(json);
            var x = 5;
        }
    }
}
