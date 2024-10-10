using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 checkPointPos;

    //SpriteRenderer spriteRenderer;
    Rigidbody2D playerRb;
    private void Awake(){
        //playerRb=GetComponent<Rigidbody2D>();
        //spriteRenderer=GetComponent<SpriteRenderer>();
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

    }
    void Die()
    {
        StartCoroutine(Respawn(0.0f));
    }

    public void UpdateCheckPoint(Vector2 pos)
    {
        checkPointPos=pos;
    }
    IEnumerator Respawn(float duration)
    {
        playerRb.velocity=new Vector2(0,0);
        playerRb.simulated=false;
        transform.localScale=new Vector3(0,0,0);
        yield return new WaitForSeconds(duration);
        transform.position=checkPointPos;
        transform.localScale=new Vector3(1,1,1);
        playerRb.simulated=true;
    }
}
