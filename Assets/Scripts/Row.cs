using UnityEngine;

public class Row : MonoBehaviour
{
    public Tile[] Tiles;
    public string Word
    {
        get
        {
            string word = "";
            for (int index = 0; index < Tiles.Length; index++)
            {
                word += Tiles[index].Letter;
            }
            return word;
        }
    }

    private void Awake()
    {
        Tiles = GetComponentsInChildren<Tile>();
    }
}
