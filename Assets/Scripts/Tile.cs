using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [System.Serializable]
    public class State
    {
        public Color fillColor;
        public Color outlineColor;
    }

    public char Letter {  get; private set; }
    public State TileState { get; private set; }

    private TextMeshProUGUI m_LetterText;
    private Image m_fill;
    private Outline m_outline;

    private void Awake()
    {
        m_LetterText = GetComponentInChildren<TextMeshProUGUI>();
        m_fill = GetComponent<Image>();
        m_outline = GetComponent<Outline>();
        
    }


    public void SetLetter(char letter)
    {
        this.Letter = letter;
        m_LetterText.text = letter.ToString();
    }

    public void SetTileState(State state)
    {
        this.TileState = state;
        this.m_fill.color = state.fillColor;
        this.m_outline.effectColor = state.outlineColor;
    }
}
