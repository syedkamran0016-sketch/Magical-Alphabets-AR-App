using UnityEngine;

public class IntroFlowManager : MonoBehaviour
{
    public GameObject intro1Panel;
    public GameObject intro2Panel;
    public GameObject intro3Panel;
    public GameObject panelChoice;

    // ---------- INTRO 1 ----------
    public void Intro1_Next()
    {
        intro1Panel.SetActive(false);
        intro2Panel.SetActive(true);
    }

    public void Intro1_Skip()
    {
        ShowChoicePanel();
    }

    // ---------- INTRO 2 ----------
    public void Intro2_Next()
    {
        intro2Panel.SetActive(false);
        intro3Panel.SetActive(true);
    }

    public void Intro2_Skip()
    {
        ShowChoicePanel();
    }

    public void Intro2_Back()
    {
        intro2Panel.SetActive(false);
        intro1Panel.SetActive(true);
    }

    // ---------- INTRO 3 ----------
    public void Intro3_Next()
    {
        ShowChoicePanel();
    }

    public void Intro3_Back()
    {
        intro3Panel.SetActive(false);
        intro2Panel.SetActive(true);
    }

    // ---------- COMMON ----------
    void ShowChoicePanel()
    {
        intro1Panel.SetActive(false);
        intro2Panel.SetActive(false);
        intro3Panel.SetActive(false);
        panelChoice.SetActive(true);
    }
}
