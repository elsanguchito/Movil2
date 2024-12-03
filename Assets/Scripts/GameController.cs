using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Vector2 checkPointPos;

    Rigidbody2D playerRb;
    SpriteRenderer spriteRenderer;

    private void Awake(){
        //playerRb=GetComponent<Rigidbody2D>();
        spriteRenderer=GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        checkPointPos=transform.position;
        playerRb=GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Obstacle"))
        {
            Die();
        }
        else if (collision.CompareTag("DarkZone"))
        {
            DarkenPlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DarkZone"))
        {
            ResetPlayerColor();
        }
    }

    public void Die()
    {
        StartCoroutine(Respawn(0.0f));
    }

    public void UpdateCheckPoint(Vector2 pos)
    {
        checkPointPos=pos;
    }
    IEnumerator Respawn(float duration)
    {
        playerRb.velocity = new Vector2(0, 0);
        playerRb.simulated = false;
        transform.localScale = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(duration);
        transform.position = checkPointPos;
        transform.localScale = new Vector3(1, 1, 1);
        playerRb.simulated = true;
    }

    private void DarkenPlayer()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f, 1f); // Cambia el color a un tono m�s oscuro
        }
    }

    private void ResetPlayerColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white; // Restablece el color original
        }
    }
}
