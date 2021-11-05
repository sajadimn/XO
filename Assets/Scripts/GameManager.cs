using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public enum ResultState
{
    None = 0,
    Equal = 1,
    Win = 2,
    Lose = 3,
}

public class GameManager : MonoBehaviour
{
    public GameConfigs gameConfigs = null;
    public GameObject menu = null;
    public GameObject game = null;
    public GameObject result = null;

    public GameObject playerTypeImage = null;
    public GameObject playerIcon = null;
    public GameObject botTypeImage = null;
    public GameObject botIcon = null;

    public GameObject resultObject = null;
    public Sprite equal = null;
    public Sprite win = null;
    public Sprite lose = null;
    
    public GameObject menuSound = null;
    public GameObject gameSound = null;
    public Sprite soundOff = null;
    public Sprite soundOn = null;
    
    public AudioSource audioMusic = null;
    public AudioSource audioSfx = null;

    public GameObject table = null;
    public GameObject tableItem = null;
    public GameObject tableRow = null;
    public TableItem[,] tableItems;

    public GameObject yourTurn = null;
    public GameObject botTurn = null;

    

    void Awake()
    {
        GameData.gameManager = this;
    }

    public void Show(string name)
    {
        if (name == "menu")
        {
            menu.SetActive(true);
            game.SetActive(false);
            result.SetActive(false);
            audioMusic.clip = gameConfigs.menuAudio;
            SetSound();
        }
        else if (name == "game")
        {
            menu.SetActive(false);
            game.SetActive(true);
            result.SetActive(false);
            audioMusic.clip = gameConfigs.gameAudio;
            SetSound();
            ResetGame();
            SetGame();
        }
        else if (name == "result")
        {
            menu.SetActive(false);
            game.SetActive(true);
            result.SetActive(true);
            SetResult();
        }
        audioSfx.clip = gameConfigs.clickAudio;
        audioSfx.Play();
    }

    public void Sound()
    {
        GameData.GetGameState().soundState = !GameData.GetGameState().soundState;
        if (GameData.GetGameState().soundState)
        {
            audioSfx.clip = gameConfigs.clickAudio;
            audioSfx.Play();
            audioMusic.volume = 1;
            audioSfx.volume = 1;
        }
        else
        {
            audioMusic.volume = 0;
            audioSfx.volume = 0;
        }

        SetSound();
    }

    public void SetSound()
    {
        if (GameData.GetGameState().soundState == false)
        {
            menuSound.GetComponent<Image>().sprite = soundOff;
            gameSound.GetComponent<Image>().sprite = soundOff;
            audioMusic.Pause();
        }
        else
        {
            menuSound.GetComponent<Image>().sprite = soundOn;
            gameSound.GetComponent<Image>().sprite = soundOn;
            audioMusic.Play();
        }
    }

    public void SetResult()
    {
        if (GameData.GetGameState().resultState == ResultState.Equal)
        {
            resultObject.GetComponent<Image>().sprite = equal;
        }
        else if (GameData.GetGameState().resultState == ResultState.Win)
        {
            resultObject.GetComponent<Image>().sprite = win;
        }
        else if (GameData.GetGameState().resultState == ResultState.Lose)
        {
            resultObject.GetComponent<Image>().sprite = lose;
        }
    }

    public void ResetGame()
    {
        foreach (Transform child in table.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        GameData.GetGameState().ResetData();
        tableItems = null;
    }
    
    private void SetGame()
    {
        tableItems = new TableItem[gameConfigs.tableSize, gameConfigs.tableSize];
        for (int i = 0; i < gameConfigs.tableSize; i++)
        {
            var itemRow = Instantiate(tableRow);
            itemRow.transform.SetParent(table.transform);
            itemRow.transform.localScale = Vector3.one;
            for (int j = 0; j < gameConfigs.tableSize; j++)
            {
                var item = Instantiate(tableItem);
                item.transform.parent = itemRow.transform;
                item.transform.localScale = Vector3.one;
                tableItems[i, j] = item.GetComponent<TableItem>();
            }
        }
        GameData.GetGameState().Init(gameConfigs , tableItems);
        SetPlayerData();
    }

    public void SetPlayerData()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            playerTypeImage.GetComponent<Image>().sprite = gameConfigs.iconO;
            botTypeImage.GetComponent<Image>().sprite = gameConfigs.iconX;
            GameData.GetGameState().SetPlayerType(Type.O);
            GameData.botController.Init(Type.X);
        }
        else
        {
            playerTypeImage.GetComponent<Image>().sprite = gameConfigs.iconX;
            botTypeImage.GetComponent<Image>().sprite = gameConfigs.iconO;
            GameData.GetGameState().SetPlayerType(Type.X);
            GameData.botController.Init(Type.O);
        }

        playerIcon.GetComponent<Image>().sprite = gameConfigs.playerIcon;
        botIcon.GetComponent<Image>().sprite = gameConfigs.botIcon;
        if (gameConfigs.starter == GameData.GetGameState().playerType)
            GameData.GetGameState().SetPlayerTurn(true);
        ShowTurn();
    }

    public void SetTurn()
    {
        if (GameData.GetGameState().playerTurn)
            audioSfx.clip = gameConfigs.playerSelectAudio;
        else
            audioSfx.clip = gameConfigs.botSelectAudio;
        audioSfx.Play();
        GameData.GetGameState().SetPlayerTurn(!GameData.GetGameState().playerTurn);
        ShowTurn();
    }

    public void ShowTurn()
    {
        if (GameData.GetGameState().resultState == ResultState.None && GameData.GetGameState().playerTurn)
        {
            yourTurn.SetActive(true);
            botTurn.SetActive(false);
        }
        else if (GameData.GetGameState().resultState == ResultState.None && !GameData.GetGameState().playerTurn)
        {
            yourTurn.SetActive(false);
            botTurn.SetActive(true);

            Invoke("BotTurnStart", 1.5f);
        }
        else if (GameData.GetGameState().resultState != ResultState.None)
        {
            yourTurn.SetActive(false);
            botTurn.SetActive(false);
        }
    }

    public void BotTurnStart()
    {
        GameData.botController.Select();
    }

    public void GameEnd()
    {
        GameData.GetGameState().SetPlayerTurn(false);
        Show("result");
    }
}