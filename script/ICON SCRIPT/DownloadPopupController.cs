using UnityEngine;
using UnityEngine.SceneManagement;

public class DownloadPopupController : MonoBehaviour
{
    public GameObject downloadPanel;

    public void OnStartClick()
    {
        // already downloaded?
        if (PlayerPrefs.GetInt("ScanDownloaded", 0) == 1)
        {
            SceneManager.LoadScene("scanmodel");
        }
        else
        {
            downloadPanel.SetActive(true);
        }
    }

    public void OnCancelClick()
    {
        downloadPanel.SetActive(false);
    }

    public void OnDownloadClick()
    {
        // mark downloaded
        PlayerPrefs.SetInt("ScanDownloaded", 1);
        PlayerPrefs.Save();

        downloadPanel.SetActive(false);

        // ❌ scene load yaha nahi kar rahe
        // user phir Start dabayega → tab scene open hoga
    }
}
