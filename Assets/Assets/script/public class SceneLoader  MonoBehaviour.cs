using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void OpenScanScene()
    {
        SceneManager.LoadScene("ScanScene");
    }
}
