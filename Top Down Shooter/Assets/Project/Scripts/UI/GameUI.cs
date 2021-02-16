using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public Image backGround;
    public Text waveNumberText;
    public Text enemyCountText;
    public Text scoreUI;
    public Text gameOverScore;


    public Transform gameOverUI;
    public RectTransform newBannerWave;
    public RectTransform healthBar;

    private Spawner spawner;
    private Player player;
    
    void Awake()
    {
       
        spawner = FindObjectOfType<Spawner>();
        spawner.OnNewWave += OnNewWave;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        player.OnDeath += GameOver;
        scoreUI.text = "";
    }

    private void Update()
    {
        scoreUI.text = Score.score.ToString();
        float healthPercent = 0f;
        if(player != null)
        {
            healthPercent = player.health / player.startingHealth;
        }
        healthBar.localScale = new Vector3(
                    healthPercent,
                    1f,
                    1f
                );
    }

    void OnNewWave(int waveIndex)
    {
        string[] numbers = { "One", "Two", "Three", "Four" };
        waveNumberText.text = "- Wave " + numbers[waveIndex - 1] + " -";
        string enemyCountString = ((spawner.waves[waveIndex - 1].infinite) ? "Infinite" : spawner.waves[waveIndex - 1].enemyCount + "");
        enemyCountText.text = "Enemies: " + enemyCountString;

        StartCoroutine("BannerRoutine");
    }

    IEnumerator BannerRoutine()
    {
        float delay = 2f;
        float speed = 2.5f;
        float animatePercent = 0f;
        float endDelayTime = Time.time + 1 / speed + delay;

        int direction = 1;

        while(animatePercent >= 0f)
        {
            animatePercent += Time.deltaTime * speed * direction;

            if(animatePercent >= 1f)
            {
                animatePercent = 1f;
                if(Time.time > endDelayTime)
                {
                    direction = -1;
                }
            }

            newBannerWave.anchoredPosition = Vector2.up * Mathf.Lerp(
                    -830f,
                    -401f,
                    animatePercent
                );
            yield return null;
        }
    }

    void GameOver()
    {
        Cursor.visible = true;
        StartCoroutine(FadeRoutine(Color.clear, new Color(0f, 0f, 0f, 0.8f), 1));
        gameOverScore.text = scoreUI.text;
        scoreUI.gameObject.SetActive(false);
        healthBar.transform.parent.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(true);
    }

    IEnumerator FadeRoutine(Color initialColor, Color finalColor, float time)
    {
        float speed = 1 / time;
        float percentage = 0f;

        while(time <= 1)
        {
            percentage += Time.deltaTime * speed;
            backGround.color = Color.Lerp(initialColor, finalColor, percentage);
            yield return null;
        }
    }

    public void OnPlayAgain()
    {
        SceneManager.LoadScene("Game");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
