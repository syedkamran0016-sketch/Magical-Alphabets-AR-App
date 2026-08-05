using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToHomeButton : MonoBehaviour
{
    public string HomeScene = "HomeScene"; // change if your scene name is different

    public void GoToHome()
    {
        SceneManager.LoadScene(HomeScene);
    }
}
