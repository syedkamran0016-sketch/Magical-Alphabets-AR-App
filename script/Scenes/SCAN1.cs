using UnityEngine;
using UnityEngine.SceneManagement;

public class scanmodele : MonoBehaviour
{
    public GameObject warningPanel; // Drag your warning panel here in Inspector

    // Button click function
    public void ScanModel()
    {
        // Warning panel show karo
        warningPanel.SetActive(true);
    }

    // OK button function
    public void OnOkButtonClicked()
    {
        // Warning panel hide karo aur scanmodel scene load karo
        warningPanel.SetActive(false);
        SceneManager.LoadScene("scanmodel");
    }

    // Download Image button function
    public void OnDownloadButtonClicked()
    {
        // Yaha tum image download ka code daal sakte ho
        Debug.Log("Download Image button clicked!");
        // Optional: panel ko hide bhi kar do
        warningPanel.SetActive(false);
    }
}