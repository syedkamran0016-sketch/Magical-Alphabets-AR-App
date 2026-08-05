using UnityEngine;

public class SettingsDownloadManager : MonoBehaviour
{
    public GameObject downloadPanel; // Drag Download Panel here
    public string googleDriveURL = "https://drive.google.com/drive/folders/1gxrvQrb3N_83e-qJZGjCLIbPAUcfAz_5?usp=sharing";

    // Settings Button Click
    public void OnSettingsClicked()
    {
        downloadPanel.SetActive(true);
    }

    // Close Button Click
    public void OnCloseClicked()
    {
        downloadPanel.SetActive(false);
    }

    // Download Button Click
    public void OnDownloadClicked()
    {
        Application.OpenURL(googleDriveURL);
    }
}