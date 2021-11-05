using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableItem : MonoBehaviour
{
    public Type type = Type.None;
    public GameObject icon = null;

    public void select()
    {
        if (GameData.GetGameState().resultState != ResultState.None)
            return;
        if (!GameData.GetGameState().playerTurn)
            return;
        if (type != Type.None)
            return;

        SetItem(GameData.GetGameState().playerType);
    }

    public void SetItem(Type type)
    {
        this.type = type;
        if (type == Type.O)
            icon.GetComponent<Image>().sprite = GameData.gameManager.gameConfigs.iconO;
        else
            icon.GetComponent<Image>().sprite = GameData.gameManager.gameConfigs.iconX;
        icon.SetActive(true);
        bool end = GameData.GetGameState().CheckToEnd(false);
        GameData.gameManager.SetTurn();
        if (end)
            GameData.gameManager.GameEnd();
    }
}