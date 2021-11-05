using System;
using UnityEngine;

public class AI
{
    public TableItem CheckWinner(Type myType , TableItem[,] table)
    {
        for (int i = 0; i < GameData.gameManager.gameConfigs.tableSize; i++)
        {
            for (int j = 0; j < GameData.gameManager.gameConfigs.tableSize; j++)
            {
                if (table[i, j].type == Type.None)
                {
                    GameData.gameManager.tableItems[i, j].type = myType;
                    bool botWin = GameData.GetGameState().CheckToEnd(true);
                    if (botWin)
                    {
                        return GameData.gameManager.tableItems[i, j];
                    }
                    else
                    {
                        GameData.gameManager.tableItems[i, j].type = Type.None;
                    }
                }
            }
        }

        return null;
    }

    public TableItem CheckLoser(Type opponentType, TableItem[,] table)
    {
        for (int i = 0; i < GameData.gameManager.gameConfigs.tableSize; i++)
        {
            for (int j = 0; j < GameData.gameManager.gameConfigs.tableSize; j++)
            {
                if (table[i, j].type == Type.None)
                {
                    GameData.gameManager.tableItems[i, j].type = opponentType;
                    bool playerWin = GameData.GetGameState().CheckToEnd(true);
                    if (playerWin)
                    {
                        return GameData.gameManager.tableItems[i, j];
                    }
                    else
                    {
                        GameData.gameManager.tableItems[i, j].type = Type.None;
                    }
                }
            }
        }

        return null;
    }

    public TableItem SelectBestState(Type myType, Type opponentType, TableItem[,] table)
    {
        Vector2 bestState = new Vector2(-1, -1);
        int bestStateCount = -1;
        for (int i = 0; i < GameData.gameManager.gameConfigs.tableSize; i++)
        {
            for (int j = 0; j < GameData.gameManager.gameConfigs.tableSize; j++)
            {
                var bestCount = 0;
                if (table[i, j].type == Type.None)
                {
                    table[i, j].type = myType;

                    if (CheckFull(table))
                    {
                        return GameData.gameManager.tableItems[i, j];
                    }

                    var rowState = CheckBestRow(opponentType , table);
                    bestCount += rowState;
                    var columnState = CheckBestColumn(opponentType , table);
                    bestCount += columnState;
                    var cornerLeftState = CheckBestCornerLeft(opponentType , table);
                    bestCount += cornerLeftState;
                    var cornerRightState = CheckBestCornerRight(opponentType , table);
                    bestCount += cornerRightState;

                    if (bestStateCount < bestCount)
                    {
                        bestStateCount = bestCount;
                        bestState = new Vector2(i, j);
                    }

                    table[i, j].type = Type.None;
                }
            }
        }

        if (bestStateCount == 0 && table[1, 1].type == Type.None)
        {
            bestState = new Vector2(1, 1);
        }

        int indexX = Int32.Parse(bestState.x.ToString());
        int indexY = Int32.Parse(bestState.y.ToString());
        return GameData.gameManager.tableItems[indexX, indexY];
    }

    private int CheckBestRow(Type opponentType , TableItem[,] table)
    {
        for (int i = 0; i < GameData.gameManager.gameConfigs.tableSize; i++)
        {
            var row = table[i, 0];
            int emptyCount = 0;
            if (row.type != opponentType)
            {
                for (int j = 0; j < GameData.gameManager.gameConfigs.tableSize; j++)
                {
                    if (table[i, j].type == opponentType)
                        break;
                    else if (table[i, j].type == Type.None)
                        emptyCount += 1;
                    if (j == GameData.gameManager.gameConfigs.tableSize - 1 && emptyCount == 1)
                    {
                        return 2;
                    }
                }
            }
        }

        return 0;
    }

    private int CheckBestColumn(Type opponentType , TableItem[,] table)
    {
        for (int i = 0; i < GameData.gameManager.gameConfigs.tableSize; i++)
        {
            var column = table[0, i];
            int emptyCount = 0;
            if (column.type != opponentType)
            {
                for (int j = 0; j < GameData.gameManager.gameConfigs.tableSize; j++)
                {
                    if (table[j, i].type == opponentType)
                        break;
                    else if (table[j, i].type == Type.None)
                        emptyCount += 1;
                    if (j == GameData.gameManager.gameConfigs.tableSize - 1 && emptyCount == 1)
                    {
                        return 2;
                    }
                }
            }
        }

        return 0;
    }

    private int CheckBestCornerLeft(Type opponentType , TableItem[,] table)
    {
        var corner = table[0, 0];
        int emptyCount = 0;
        if (corner.type != opponentType)
        {
            for (int j = 0; j < GameData.gameManager.gameConfigs.tableSize; j++)
            {
                if (table[j, j].type == opponentType)
                    return 0;
                if (table[j, j].type == Type.None)
                    emptyCount += 1;
                if (j == GameData.gameManager.gameConfigs.tableSize - 1 && emptyCount == 1)
                {
                    return 1;
                }
            }
        }

        return 0;
    }

    private int CheckBestCornerRight(Type opponentType , TableItem[,] table)
    {
        var corner = table[0, GameData.gameManager.gameConfigs.tableSize - 1];
        int emptyCount = 0;
        if (corner.type != opponentType)
        {
            int counter = 0;
            for (int j = GameData.gameManager.gameConfigs.tableSize - 1; j >= 0; j--)
            {
                if (table[counter, j].type == opponentType)
                    return 0;
                if (table[counter, j].type == Type.None)
                    emptyCount += 1;
                if (j == 0 && emptyCount == 1)
                {
                    return 1;
                }

                counter += 1;
            }
        }

        return 0;
    }

    private bool CheckFull(TableItem[,] table)
    {
        for (int i = 0; i < GameData.gameManager.gameConfigs.tableSize; i++)
        {
            for (int j = 0; j < GameData.gameManager.gameConfigs.tableSize; j++)
            {
                if (table[i, j].type == Type.None)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
