using UnityEngine;
using UnityEngine.Video;

public class TestVideo : MonoBehaviour
{
    public VideoPlayer vp;
    
    void Start()
    {
        // Auto-find VideoPlayer if not assigned
        if (vp == null)
        {
            vp = GetComponent<VideoPlayer>();
            
            // If still null, try finding on child objects
            if (vp == null)
            {
                vp = GetComponentInChildren<VideoPlayer>();
            }
            
            // If still null, log error
            if (vp == null)
            {
                Debug.LogError("No VideoPlayer component found! Please attach a VideoPlayer to this GameObject or assign one in the Inspector.");
                return;
            }
        }
        
        vp.Play();
    }
}