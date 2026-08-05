using UnityEngine;
using TMPro;

public class NotificationManagerUI : MonoBehaviour
{
    public static NotificationManagerUI Instance;

    [Header("UI Elements")]
    public Transform contentParent;          // ScrollView → Viewport → Content
    public GameObject notificationPrefab;    // Yellow box prefab with TextMeshProUGUI inside

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Call this to show a new notification
    /// </summary>
    /// <param name="message">Message text</param>
    public void ShowNotification(string message)
    {
        if (notificationPrefab == null || contentParent == null)
        {
            Debug.LogError("Assign notificationPrefab and contentParent in Inspector!");
            return;
        }

        // Instantiate prefab
        GameObject obj = Instantiate(notificationPrefab, contentParent);

        // Set message text
        TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = message;

        // Optional: Auto destroy after 5 seconds
        Destroy(obj, 5f);
    }
}