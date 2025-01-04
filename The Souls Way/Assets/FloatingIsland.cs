using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FloatingIsland : MonoBehaviour
{
    public Vector2 pos1 = new Vector2(0,0);
    public Vector2 pos2 = new Vector2(0,0);
    public float speed = 5f;
    private float distanceX;
    private float distanceY;

    // Update is called once per frame
    private void Start()
    {
        if(pos2.x < pos1.x)
        {
            float temp = pos1.x;
            pos1.x = pos2.x;
            pos2.x = temp;
        }            
        if(pos2.y < pos1.y)
        {
            float temp = pos1.y;
            pos1.y = pos2.y;
            pos2.y = temp;
        }
        distanceX = pos2.x - pos1.x;
        distanceY = pos2.y - pos1.y;
    }
    void Update()
    {
        if (distanceX == 0)
        {
            transform.position = new Vector3(pos1.x, Mathf.PingPong(Time.time * speed, distanceY) + pos1.y, 0);
           // print(pos1.x + ", " + Mathf.PingPong(Time.time * speed, distanceY) + pos1.y);
        }
        else if (distanceY == 0)
        {
            transform.position = new Vector3(Mathf.PingPong(Time.time*speed , distanceX) + pos1.x, pos1.y, 0);
           // print(Mathf.PingPong(Time.time, distanceX) + pos1.x + ", " + pos1.y);
        }
        else
        {
            transform.position = new Vector3(Mathf.PingPong(Time.time * speed, distanceX) + pos1.x, Mathf.PingPong(Time.time * speed, distanceY) + pos1.y, 0);
          //  print(Mathf.PingPong(Time.time * speed, distanceX) + pos1.x + ", " + Mathf.PingPong(Time.time * speed, distanceY) + pos1.y);
        }
    }
    private void OnDrawGizmos()
    {
        //Draw the ground colliders on screen for debug purposes
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pos1,pos2);
        
    }
}
