using UnityEngine;
using Vuforia;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour
{
    public List<GameObject> allTargets;

    void Start()
    {
        foreach (var target in allTargets)
        {
            var observer = target.GetComponent<ObserverBehaviour>();
            observer.OnTargetStatusChanged += (behaviour, status) =>
            {
                if (status.Status == Status.TRACKED)
                {
                    ActivateOnly(target);
                }
            };
        }
    }

    void ActivateOnly(GameObject activeTarget)
    {
        foreach (var target in allTargets)
        {
            target.SetActive(target == activeTarget);
        }
    }
}