using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public bool isCastle;
    public SpriteRenderer endGoal;
    public Sprite castle;
    public Sprite house;
    // Start is called before the first frame update
    void Start()
    {
        endGoal.sprite = isCastle ? castle : house;
    }
}
