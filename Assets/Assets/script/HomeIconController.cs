using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeIconController : MonoBehaviour
{
    public void OpenProfile()
    {
        PlayerPrefs.SetString("OPEN_PANEL", "PROFILE");
        SceneManager.LoadScene("IconScen");
    }

    public void OpenSettings()
    {
        PlayerPrefs.SetString("OPEN_PANEL", "SETTINGS");
        SceneManager.LoadScene("IconScen");
    }

    public void OpenNotification()
    {
        PlayerPrefs.SetString("OPEN_PANEL", "NOTIFICATION");
        SceneManager.LoadScene("IconScen");
    }

    public void OpenScan()
    {
        SceneManager.LoadScene("ScanScene");
    }
}
