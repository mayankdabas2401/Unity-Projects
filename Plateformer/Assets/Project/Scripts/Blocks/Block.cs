using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
   public virtual void onHit(Player player)
    {
        Destroy(gameObject);
    }
}
