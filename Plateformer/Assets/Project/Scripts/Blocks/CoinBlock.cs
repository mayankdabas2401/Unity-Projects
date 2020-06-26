using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBlock : Block
{
    public GameObject coinPrefab;
    public int coinAmount = 5;
    public Sprite brickSprite;
    public override void onHit(Player player)
    {
        if(coinAmount > 0)
        {
            coinAmount--;
            GameObject coinInstant = GameObject.Instantiate(coinPrefab, transform.parent);
            coinInstant.transform.position = transform.position + Vector3.up * 1.2f;
            Destroy(coinInstant, 0.1f);
            player.AddCoins();

            if(coinAmount == 0)
            {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = brickSprite;
            }
        }
    }
}
