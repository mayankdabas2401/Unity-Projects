using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int score { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Enemy.OnDeathStatic += OnDeathTrigger;
        FindObjectOfType<Player>().OnDeath += OnPlayerDeath;
        score = 0;
    }

    void OnDeathTrigger()
    {
        score += 1;
    }

    void OnPlayerDeath()
    {
        Enemy.OnDeathStatic -= OnDeathTrigger;
    }
}
