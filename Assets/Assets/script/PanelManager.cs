using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject currentPanel;
    public GameObject nextPanel;

    public void GoToNextPanel()
    {
        currentPanel.SetActive(false);
        nextPanel.SetActive(true);
    }
}