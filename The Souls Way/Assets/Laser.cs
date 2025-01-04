using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("Motion")]
    public Vector2 pos1 = new Vector2(0, 0);
    public Vector2 pos2 = new Vector2(0, 0);
    public bool canMove = false;
    public float speed = 5f;
    private float distanceX;
    private float distanceY;
    [Header("Laser")]
    public Vector3 direction;
    public float range = 10f;
    public Transform laserBeam;
    public GameObject laserImpact;
    // Update is called once per frame
    private void Start()
    {
        range += 0.5f;
        if (!canMove)
        {
            return;
        }
        if (pos2.x < pos1.x)
        {
            float temp = pos1.x;
            pos1.x = pos2.x;
            pos2.x = temp;
        }
        if (pos2.y < pos1.y)
        {
            float temp = pos1.y;
            pos1.y = pos2.y;
            pos2.y = temp;
        }
        distanceX = pos2.x - pos1.x;
        distanceY = pos2.y - pos1.y;
    }
    private void FixedUpdate()
    {

        Debug.DrawRay(transform.position, direction * range, Color.green);

        RaycastHit2D hit =  Physics2D.Raycast(transform.position, direction, range);
        if (hit)
        {
            laserImpact.SetActive(true);
            if (direction.y == 0)
            {
                laserBeam.localScale = new Vector3(hit.distance-0.5f, 1, 1);
                laserImpact.transform.position = new Vector3(transform.position.x + hit.distance * direction.x, transform.position.y, 0);
                if(direction.x < 0)
                {
                    laserImpact.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    laserImpact.transform.rotation = Quaternion.Euler(0, 0, -180);
                }
            }
            else if (direction.x == 0)
            {
                laserBeam.localScale = new Vector3(hit.distance-0.5f,1, 1);
                laserImpact.transform.position = new Vector3(transform.position.x, transform.position.y + hit.distance*direction.y, 0);
                if (direction.y < 0)
                {
                    laserImpact.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                else
                {
                    laserImpact.transform.rotation = Quaternion.Euler(0, 0, -90);
                }
            }
        }
        else
        {
            laserImpact.SetActive(false);
            laserBeam.localScale = new Vector3(range-0.5f, 1, 1);
        }
    }
    void Update()
    {
        if (!canMove)
        {
            return;
        }
        if (distanceX == 0)
        {
            transform.position = new Vector3(pos1.x, Mathf.PingPong(Time.time * speed, distanceY) + pos1.y, 0);
            // print(pos1.x + ", " + Mathf.PingPong(Time.time * speed, distanceY) + pos1.y);
        }
        else if (distanceY == 0)
        {
            transform.position = new Vector3(Mathf.PingPong(Time.time * speed, distanceX) + pos1.x, pos1.y, 0);
            // print(Mathf.PingPong(Time.time, distanceX) + pos1.x + ", " + pos1.y);
        }
        else
        {
            transform.position = new Vector3(Mathf.PingPong(Time.time * speed, distanceX) + pos1.x, Mathf.PingPong(Time.time * speed, distanceY) + pos1.y, 0);
            //  print(Mathf.PingPong(Time.time * speed, distanceX) + pos1.x + ", " + Mathf.PingPong(Time.time * speed, distanceY) + pos1.y);
        }
    }
}
