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
    public DialogPanel DialogPanel;
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
        DialogPanel.gameObject.SetActive(false);
        await LoadJsonAsync();
    }

    private bool first = true;
    private async Task LoadJsonAsync()
    {
        var request = UnityWebRequest.Get(MagicWordsUrl);

        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        Loading.SetActive(false);
        
        if (first | request.result != UnityWebRequest.Result.Success)
        {
            first = false;
            NoLoad.SetActive(true);
        }
        else
        {
            DialogPanel.gameObject.SetActive(true);
            var json = request.downloadHandler.text;
            var dialog = JsonConvert.DeserializeObject<MagicWords>(json);
            DialogPanel.Display(dialog);
        }
    }
}
