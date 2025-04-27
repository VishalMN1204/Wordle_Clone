using UnityEngine;
using TMPro;
using System.Xml.Serialization;

public class Board : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TextMeshProUGUI m_TextMeshProUGUI;

    [Header("Buttons")]
    [SerializeField] GameObject m_TryAgainBtnGameObject;
    [SerializeField] GameObject m_NewWordBtnGameObject;

    [Header("States")]
    public Tile.State EmptyState;
    public Tile.State OccupiedState;
    public Tile.State CorrectState;
    public Tile.State IncorrectState;
    public Tile.State WrongSpotState;


    private string word;
    private Row[] rows;

    private static readonly KeyCode[] SUPPPORTED_KEYS =
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F,
        KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L,
        KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R,
        KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
        KeyCode.Y, KeyCode.Z
    };
    private int columnIndex;
    private int rowIndex;
    private string[] solutionWords;
    private string[] validWords;

    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
    }

    private void OnEnable()
    {
        m_TryAgainBtnGameObject.SetActive(false);
        m_NewWordBtnGameObject.SetActive(false);
    }

    private void OnDisable()
    {
        m_TryAgainBtnGameObject.SetActive(true);
        m_NewWordBtnGameObject.SetActive(true);
    }


    private void Start()
    {
        LoadData();
        SetRandomWord();
    }


    private void Update()
    {
        Row currentRow = rows[rowIndex];
        // if player press the backspace button
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            m_TextMeshProUGUI.gameObject.SetActive(false);
            columnIndex = Mathf.Max(columnIndex - 1, 0);
            currentRow.Tiles[columnIndex].SetLetter('\0');
            currentRow.Tiles[columnIndex].SetTileState(EmptyState);
        }

        // if player has submitted the word
        else if (columnIndex >= currentRow.Tiles.Length)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SubmitRow(currentRow);
            }
        }

        // if player has neither pressed the backspace button nor submitted the word then
        else
        {
            for (int index = 0; index < SUPPPORTED_KEYS.Length; index++)
            {
                if (Input.GetKeyDown(SUPPPORTED_KEYS[index]))
                {
                    currentRow.Tiles[columnIndex].SetLetter((char)SUPPPORTED_KEYS[index]);
                    currentRow.Tiles[columnIndex].SetTileState(OccupiedState);
                    columnIndex++;
                }
            }
        }
    }


    private void SubmitRow(Row row)
    {
        // Before submitting there will be check whether the word submitted is even a valid word if not a valid present a invalid text and row should not increment
        if (!IsValidWord(row.Word))
        {
            // add a invalid text in the UI
            m_TextMeshProUGUI.gameObject.SetActive(true);
            return;
        }

        string remaining = word;
        // this loop will check whether the tile letter is correct or not
        for (int index = 0; index < row.Tiles.Length; index++)
        {
            // Getting each tile from a row
            Tile tile = row.Tiles[index];
            // if the tile letter is at the same position 
            if (tile.Letter == word[index])
            {
                // change the original state to correct state
                tile.SetTileState(CorrectState);
                // remove the correct letter from the word and replace it with a empty space
                remaining = remaining.Remove(index, 1);
                remaining = remaining.Insert(index, " ");
            }
            // if the tile letter is not even part of the word
            else if (!word.Contains(tile.Letter))
            {
                // change the original state to incorrect state
                tile.SetTileState(IncorrectState);
            }
        }

        // this loop will check whether the tile letter is there in word but the spot is wrong
        for (int index = 0; index < row.Tiles.Length; index++)
        {
            Tile tile = row.Tiles[index];
            if (tile.TileState != CorrectState && tile.TileState != IncorrectState)
            {
                // if the tile letter is present on the string but the position of the letter is not correct then state will be wrong spot state this condition will check that
                if (remaining.Contains(tile.Letter))
                {
                    tile.SetTileState(WrongSpotState);
                    // Get the index of the letter which is in wrong spot state in the solution
                    int letterIndex = remaining.IndexOf(tile.Letter);
                    // replace the letter with empty space
                    remaining = remaining.Remove(letterIndex, 1);
                    remaining = remaining.Insert(letterIndex, " ");
                }
                else
                {
                    tile.SetTileState(IncorrectState);
                }
            }
        }

        if (HasWon(row))
        {
            enabled = false;
        }

        rowIndex++;
        columnIndex = 0;

        if (rowIndex >= rows.Length)
        {
            enabled = false;
        }
    }

    private void LoadData()
    {
        // load the data from the txt file
        TextAsset textFile = Resources.Load("official_wordle_all") as TextAsset;
        //convert the txt file to string and split it
        validWords = textFile.text.Split("\n");
        textFile = Resources.Load("official_wordle_common") as TextAsset;
        solutionWords = textFile.text.Split("\n");
    }

    private void SetRandomWord()
    {
        word = solutionWords[Random.Range(0, solutionWords.Length)];
    }

    private bool IsValidWord(string word)
    {
        for (int index = 0; index < validWords.Length; index++)
        {
            if (word == validWords[index])
            {
                return true;
            }
        }
        return false;
    }

    private bool HasWon(Row row)
    {
        for (int index = 0; index < row.Tiles.Length; index++)
        {
            if (row.Tiles[index].TileState != CorrectState)
            {
                return false;
            }
        }

        return true;
    }

    private void ClearBoard()
    {
        for (int rowIndex = 0; rowIndex < rows.Length; rowIndex++)
        {
            for (int colIndex = 0; colIndex < rows[rowIndex].Tiles.Length; colIndex++)
            {
                Tile tile = rows[rowIndex].Tiles[colIndex];
                tile.SetLetter('\0');
                tile.SetTileState(EmptyState);
            }
        }
        rowIndex = 0;
        columnIndex = 0;
        enabled = true;
    }

    public void TryAgain()
    {
        ClearBoard();
    }

    public void NewWord()
    {
        ClearBoard();
        SetRandomWord();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
