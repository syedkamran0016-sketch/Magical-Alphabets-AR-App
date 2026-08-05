using UnityEngine;
using Vuforia;

public class ARPlacementManager : MonoBehaviour
{
    public GameObject applePrefab;
    public GameObject catPrefab;
    public GameObject dogPrefab;

    private GameObject selectedPrefab;

    public void SelectApple()
    {
        selectedPrefab = applePrefab;
    }

    public void SelectCat()
    {
        selectedPrefab = catPrefab;
    }

    public void SelectDog()
    {
        selectedPrefab = dogPrefab;
    }

    void Update()
    {
        if (selectedPrefab == null)
            return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Instantiate(selectedPrefab, hit.point, Quaternion.identity);
                selectedPrefab = null;
            }
        }
    }
}