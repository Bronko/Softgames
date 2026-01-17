using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
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
}
