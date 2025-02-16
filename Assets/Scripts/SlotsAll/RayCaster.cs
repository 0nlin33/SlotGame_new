using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    public int rayCastID { get; set; } // for calcs

    public Symbols GetSymbol()
    {
        Collider2D hit = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), .1f);
        if (hit)
        {
            return hit.GetComponent<Symbols>();
        }
        else return null;
    }


}
