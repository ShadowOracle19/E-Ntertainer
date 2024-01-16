using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed; //speed of the platform
    public Transform[] points; //an array of transform points

    public int startingPoint; //starting index

    private int index;//Index of the array

    private void Start()
    {
        transform.position = points[startingPoint].position;//set the platform to the starting position 
    }

    // Update is called once per frame
    void Update()
    {
        //check the distance between the platform and the point
        if(Vector2.Distance(transform.position, points[index].position) < 0.02f)
        {
            index++;//increase index to next point
            if(index == points.Length)//check if platform is on the last point
            {
                index = 0;//reset index
            }
        }

        //move the platform to the point in the current index
        transform.position = Vector2.MoveTowards(transform.position, points[index].position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if what the collision is touching is the player
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(transform);
            collision.gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.None;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Check if what the collision is touching is the player
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(null);
            collision.gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }
}
