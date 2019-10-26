using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    public Transform[] points;
    private int destPoint = 0;
    Vector3 currentdestination;
    public float maxDistance;

    // Start is called before the first frame update
    void Start()
    {
        GotoNextPoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Choose the next destination point when the agent gets
        this.transform.position = Vector3.MoveTowards(transform.position, currentdestination, maxDistance);



        // close to the current one.
        if (Vector3.Distance(transform.position, currentdestination) < maxDistance)
            GotoNextPoint();

    }
    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        currentdestination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.parent = transform;
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.parent = null;
    }

}
