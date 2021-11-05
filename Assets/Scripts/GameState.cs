using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public ResultState resultState = ResultState.None;
    private Type winnerType = Type.None;
    public Type playerType = Type.None;
    public bool playerTurn = false;
    private GameConfigs gameConfigs = null;
    private TableItem[,] table = null;
    public bool soundState = true;
    public void Init(GameConfigs gameConfigs, TableItem[,] table)
    {
        this.gameConfigs = gameConfigs;
        this.table = table;
    }
    
    public void SetPlayerType(Type type)
    {
        playerType = type;
    }
    
    public void SetPlayerTurn(bool state)
    {
        playerTurn = state;
    }

    public void ResetData()
    {
        playerTurn = false;
        winnerType = Type.None;
        resultState = ResultState.None;
        playerType = Type.None;
        table = null;
        gameConfigs = null;
    }
    public bool CheckToEnd(bool selectingChecker)
    {
        bool row = CheckRow();
        bool column = CheckColumn();
        bool cornerLeft = CheckCornerLeft();
        bool cornerRight = CheckCornerRight();
        bool full = CheckFull();
        if (row || column || cornerLeft || cornerRight)
        {
            if (!selectingChecker)
            {
                if (winnerType == playerType)
                    resultState = ResultState.Win;
                else
                    resultState = ResultState.Lose;
            }
            return true;
        }
        else if (full)
        {
            if (!selectingChecker)
            {
                resultState = ResultState.Equal;
            }
            return true;
        }

        return false;
    }

    private bool CheckRow()
    {
        for (int i = 0; i < gameConfigs.tableSize; i++)
        {
            var row = table[i, 0];
            if (row.type != Type.None)
            {
                for (int j = 0; j < gameConfigs.tableSize; j++)
                {
                    if (table[i, j].type != row.type)
                        break;
                    if (j == gameConfigs.tableSize - 1)
                    {
                        winnerType = row.type;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool CheckColumn()
    {
        for (int i = 0; i < gameConfigs.tableSize; i++)
        {
            var column = table[0, i];
            if (column.type != Type.None)
            {
                for (int j = 0; j < gameConfigs.tableSize; j++)
                {
                    if (table[j, i].type != column.type)
                        break;
                    if (j == gameConfigs.tableSize - 1)
                    {
                        winnerType = column.type;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool CheckCornerLeft()
    {
        var corner = table[0, 0];
        if (corner.type != Type.None)
        {
            for (int j = 0; j < gameConfigs.tableSize; j++)
            {
                if (table[j, j].type != corner.type)
                    return false;
                if (j == gameConfigs.tableSize - 1)
                {
                    winnerType = corner.type;
                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckCornerRight()
    {
        var corner = table[0, gameConfigs.tableSize - 1];
        if (corner.type != Type.None)
        {
            int counter = 0;
            for (int j = gameConfigs.tableSize - 1; j >= 0; j--)
            {
                if (table[counter, j].type != corner.type)
                    return false;
                if (j == 0)
                {
                    winnerType = corner.type;
                    return true;
                }

                counter += 1;
            }
        }

        return false;
    }

    private bool CheckFull()
    {
        for (int i = 0; i < gameConfigs.tableSize; i++)
        {
            for (int j = 0; j < gameConfigs.tableSize; j++)
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
