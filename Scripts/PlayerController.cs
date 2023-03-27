using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public GameObject ball;
    public GameObject thrower;
    public float timeBuffer = 10f;
    public float launchForce = 5f;
    Vector3 ballStartingPosition;
    bool launched = false;
    bool landed = false;
    bool retrieved = false;

    public UnityEngine.AI.NavMeshAgent dog;
    // Update is called once per frame
    

    void Start()
    {
        ballStartingPosition = ball.transform.position;
    }

    void Update()
    {
        launchBall();
        checkIfLanded();
        checkIfRetrieved();
        checkIfHome();
    }

    void launchBall()
    {
        if(Input.GetMouseButtonDown(0))
        {   
            RaycastHit hit = new RaycastHit();
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            {
                //throw the ball to a position and wait for it to land then fetch
                FiringSolution firing = new FiringSolution();
                Nullable<Vector3> aimVector = firing.calculateFiringSolution(transform.position, hit.point, launchForce, Physics.gravity);
                ball.GetComponent<Rigidbody>().isKinematic = false;
                if (aimVector.HasValue)
                    ball.GetComponent<Rigidbody>().AddForce(aimVector.Value.normalized * launchForce, ForceMode.VelocityChange);
                launched = true;
            }
            
            if(launched)
            {
                Debug.Log("In launched: " + hit.point.x + hit.point.y + hit.point.z);
                dog.SetDestination(hit.point);
            }
        }
    }

    void checkIfLanded()
    {
        if(launched && ball.transform.position.y < .1)
        {
            launched = false; landed = true;
            ball.GetComponent<Rigidbody>().velocity.Set(0,0,0);
        }
    }

    void checkIfRetrieved()
    {
        //if dog has gotten to ball then return the ball
        Vector3 closeEnough = new Vector3(2,2,2);
        if(landed && (dog.transform.position - ball.transform.position).magnitude < closeEnough.magnitude)
        {
            //have the dog grab the ball
            var dogPos = dog.transform.position;
            ball.transform.position = new Vector3(dogPos.x, dogPos.y + 1, dogPos.z);
            //destroy the jointed object
            var Container = ball.GetComponent<FixedJoint>();
            Destroy(Container);
            retrieved = true;
            //return to the thrower
            dog.SetDestination(thrower.transform.position);
        }
    }

    void checkIfHome()
    {
        var home = new Vector3(.5f,.5f,.5f);
        //check if the dog is back to the thrower
        if(retrieved && (dog.transform.position - thrower.transform.position).magnitude < home.magnitude) 
        {
            ball.transform.position = ballStartingPosition;
            launched = false; landed = false; retrieved = false;
        }
    }
}
