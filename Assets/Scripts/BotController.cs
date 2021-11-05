using UnityEngine;

public class BotController : MonoBehaviour
{
    private Type botType = Type.None;
    private TableItem[,] cloneTableItems;

    public void Awake()
    {
        GameData.botController = this;
    }

    public void Init(Type type)
    {
        botType = type;
    }

    public void Select()
    {
        cloneTableItems = null;
        cloneTableItems = GameData.gameManager.tableItems.Clone() as TableItem[,];
        TableItem winner = null;
        TableItem loser = null;
        TableItem select = null;
        winner = GameData.GetAI().CheckWinner(botType , cloneTableItems);
        if (winner)
        {
            winner.SetItem(botType);
            return;
        }

        loser = GameData.GetAI().CheckLoser(GameData.GetGameState().playerType , cloneTableItems);
        if (loser)
        {
            loser.SetItem(botType);
            return;
        }

        select = GameData.GetAI().SelectBestState(botType,GameData.GetGameState().playerType , cloneTableItems);
        select.SetItem(botType);
    }
}