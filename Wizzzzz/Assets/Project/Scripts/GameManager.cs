using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Text messageText;
    public Text gemsText;

    private bool gameOver;
    private int gemsCount;
    private float resetTimer = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        messageText.text = "";
        gemsText.text = "Score: 0";
        player.OnDead += OnDead;
        player.OnCollected += OnCollected;
        player.OnWin += OnWin;
        gameOver = false;
        gemsCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver)
        {
            resetTimer -= Time.deltaTime;
            if(player != null)  player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            if (resetTimer <= 0.0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }
    }

    void OnDead()
    {
        messageText.text = "You Lose!";
        gameOver = true;
    }

    void OnCollected()
    {
        gemsCount++;
        gemsText.text = "Score: " + gemsCount.ToString();
    }

    void OnWin()
    {
        messageText.text = "You Win!";
        gameOver = true;
    }
}
