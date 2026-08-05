using UnityEngine;

public class ModelSwitcher : MonoBehaviour
{
    public GameObject[] models;

    void Start()
    {
        // Sab models start me hidden
        foreach (GameObject model in models)
        {
            model.SetActive(false);
        }
    }

    public void ShowModel(int index)
    {
        // Sabko hide karo
        foreach (GameObject model in models)
        {
            model.SetActive(false);
        }

        // Sirf selected model show karo
        models[index].SetActive(true);
    }
}