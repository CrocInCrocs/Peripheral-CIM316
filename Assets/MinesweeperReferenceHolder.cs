using System;
using UnityEngine;

public class MinesweeperReferenceHolder : MonoBehaviour
{
    public Minesweeper Minesweeper;

    private void Start()
    {
        MinesweeperManager.Current.UpdateReferences(Minesweeper);
    }
}
