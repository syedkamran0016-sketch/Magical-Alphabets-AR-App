using UnityEngine;

public class CenterModelSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject[] models;

    GameObject currentModel;

    public void ShowModel(int index)
    {
        if (currentModel != null)
            Destroy(currentModel);

        currentModel = Instantiate(models[index], spawnPoint.position, spawnPoint.rotation);
        currentModel.transform.SetParent(spawnPoint);
    }
}