using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public Player player;
    public Text coinText;
    public Text messageText;

    private float resetTimer = 3.0f;
    private bool gameOver;
    private int coins = 0;
    private int Coins
    {
        set
        {
            coins = value;
            coinText.text = "" + coins;
        }
        get
        {
            return coins;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player.OnCoinCollected += OnCoinCollected;
        player.OnWin += OnWin;
        player.OnLose += OnLose;

        gameOver = false;
        messageText.text = "";
        Coins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver)
        {
            resetTimer -= Time.deltaTime;
            if(resetTimer <= 0.0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    // Event subscriber
    void OnCoinCollected()
    {
        Coins++;
    }

    void OnWin()
    {
        messageText.text = "Level Complete";
        gameOver = true;
    }

    void OnLose()
    {
        messageText.text = "Game Over";
        gameOver = true;
    }
}
