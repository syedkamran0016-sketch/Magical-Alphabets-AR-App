using UnityEngine;

public class MatchItemButton : MonoBehaviour
{
    public string itemID;
    public MatchItemButton target;
    public MatchingGameController controller;

    public void OnClick()
    {
        if (target == null || controller == null)
            return;

        if (itemID == target.itemID)
        {
            controller.CorrectMatch();
        }
        else
        {
            Debug.Log("Wrong Match");
        }
    }
}
