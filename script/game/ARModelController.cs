using UnityEngine;
using Vuforia;

public class ARModelController : MonoBehaviour
{
    ObserverBehaviour observer;

    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();
        observer.OnTargetStatusChanged += OnStatusChanged;
    }

    void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            HideAllModels();
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void HideAllModels()
    {
        ARModelController[] all = FindObjectsOfType<ARModelController>();
        foreach (var a in all)
        {
            if (a.transform.childCount > 0)
                a.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
