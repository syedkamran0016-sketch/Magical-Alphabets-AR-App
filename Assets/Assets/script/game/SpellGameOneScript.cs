using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellGameOneScript : MonoBehaviour
{
    [System.Serializable]
    public class Row
    {
        public Button rowButton;          // Right side click button
        public Image resultBox;           // Yellow/Red result box
        public char correctLetter;        // Correct letter for this row
        public bool solved;
    }

    [Header("RIGHT SIDE ROWS")]
    public Row[] rows;

    [Header("LEFT LETTER BUTTONS")]
    public Button[] letterButtons;
    public TMP_Text[] letterTexts;

    [Header("COLORS")]
    public Color normalColor = Color.white;
    public Color selectedBlue = Color.blue;
    public Color rightYellow = Color.yellow;
    public Color wrongRed = Color.red;

    [Header("NEXT BUTTON")]
    public Button nextButton;

    int selectedRow = -1;

    void Start()
    {
        nextButton.gameObject.SetActive(false);

        // row button listeners
        for (int i = 0; i < rows.Length; i++)
        {
            int id = i;
            rows[i].rowButton.onClick.AddListener(() => SelectRow(id));
            rows[i].resultBox.color = normalColor;
            rows[i].solved = false;
        }

        // letter button listeners
        for (int i = 0; i < letterButtons.Length; i++)
        {
            int id = i;
            letterButtons[i].onClick.AddListener(() => SelectLetter(id));
        }
    }

    void SelectRow(int id)
    {
        if (rows[id].solved) return;

        selectedRow = id;

        // reset all row colors
        foreach (var r in rows)
            r.rowButton.image.color = normalColor;

        rows[id].rowButton.image.color = selectedBlue;
    }

    void SelectLetter(int letterId)
    {
        if (selectedRow == -1) return;

        char picked = letterTexts[letterId].text[0];

        if (picked == rows[selectedRow].correctLetter)
        {
            rows[selectedRow].resultBox.color = rightYellow;
            rows[selectedRow].solved = true;
        }
        else
        {
            rows[selectedRow].resultBox.color = wrongRed;
        }

        rows[selectedRow].rowButton.image.color = normalColor;
        selectedRow = -1;

        CheckAllSolved();
    }

    void CheckAllSolved()
    {
        foreach (var r in rows)
            if (!r.solved)
                return;

        nextButton.gameObject.SetActive(true);
    }
}
