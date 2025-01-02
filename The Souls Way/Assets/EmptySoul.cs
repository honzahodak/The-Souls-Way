using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptySoul : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerDeath playerDeath = collision.gameObject.GetComponent<PlayerDeath>();
            
            playerDeath.SetSoulToReclaim(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerDeath playerDeath = collision.gameObject.GetComponent<PlayerDeath>();
            playerDeath.SetSoulToReclaim(gameObject, false);
        }
    }

    private void OnMouseEnter()
    {

    }

    private void OnMouseExit()
    {

    }
}
