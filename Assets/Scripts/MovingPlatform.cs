using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //public Transform posA, posB;
    public float speed;
    Vector3 targetPos;
    
    MoveController moveController;
    Rigidbody2D rb;
    Vector3 moveDirection;

    Rigidbody2D playerRb;


    public GameObject ways;
    public Transform[] wayPoints;
    int pointIndex;
    int pointCount;
    int direction=1;
    public float waitDuration;
    private void Awake(){
        moveController=GameObject.FindGameObjectWithTag("Player").GetComponent<MoveController>();
        rb=GetComponent<Rigidbody2D>();
        playerRb=GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        

        wayPoints=new Transform[ways.transform.childCount];
        for(int i = 0; i < ways.gameObject.transform.childCount;i++)
        {
            wayPoints[i]=ways.transform.GetChild(i).gameObject.transform;
        }

        }
    private void Start()
    {
        //targetPos=posB.position;
        pointIndex=1;
        pointCount=wayPoints.Length;
        targetPos=wayPoints[1].transform.position;
        DirectionCalculate();
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, targetPos)<0.05f)
        {
            NextPoint();
        }
        /*if(Vector2.Distance(transform.position,posA.position)<0.05f)
        {
            targetPos=posB.position;
            DirectionCalculate();
        }
        if(Vector2.Distance(transform.position,posB.position)<0.05f)
        {
            targetPos=posA.position;
            DirectionCalculate();
        }
        //transform.position=Vector3.MoveTowards(transform.position,targetPos, speed * Time.deltaTime);
        */
    }
    void NextPoint(){
        transform.position=targetPos;
        moveDirection=Vector3.zero;

        if(pointIndex==pointCount-1)
        {
            direction=-1;
        }
        if(pointIndex==0)
        {
            direction=1;
        }
        pointIndex+=direction;
        targetPos=wayPoints[pointIndex].transform.position;
        //DirectionCalculate();
        StartCoroutine(WaitNextPoint());
    }
    IEnumerator WaitNextPoint()
    {
        yield return new WaitForSeconds(waitDuration);
        DirectionCalculate();
    }
    private void FixedUpdate()
    {
        rb.velocity=moveDirection*speed;
    }
    void DirectionCalculate()
    {
        moveDirection=(targetPos-transform.position).normalized;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //collision.transform.parent = this.transform;
            moveController.isOnPlatform=true;
            moveController.platformRb=rb;
            playerRb.gravityScale=playerRb.gravityScale*50;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            moveController.isOnPlatform=false;
           // collision.transform.parent=null;
           playerRb.gravityScale=playerRb.gravityScale/50;
        }
    }

}
