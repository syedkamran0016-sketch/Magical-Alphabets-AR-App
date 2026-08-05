using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    public GameObject warningPanel; // Drag your WarningPanel here
    public string downloadFolderURL = "https://drive.google.com/drive/folders/1gxrvQrb3N_83e-qJZGjCLIbPAUcfAz_5?usp=sharing";

    // Start button click
    public void OnStartButtonClicked()
    {
        // Warning panel dikhao
        warningPanel.SetActive(true);
    }

    // OK button click on warning panel
    public void OnOkButtonClicked()
    {
        // Panel hide karo
        warningPanel.SetActive(false);

        // ScanModel scene load karo
        SceneManager.LoadScene("scanmodel");
    }

    // Optional: Cancel button agar chahiye
    public void OnCancelButtonClicked()
    {
        warningPanel.SetActive(false);
    }

    // Download button click
    public void OnDownloadButtonClicked()
    {
        // Google Drive folder open karo
        Application.OpenURL(downloadFolderURL);
    }
}