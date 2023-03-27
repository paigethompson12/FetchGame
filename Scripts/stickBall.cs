using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stickBall : MonoBehaviour
{
    
    void OnCollisionEnter(Collision c) {
        if(c.gameObject.tag == "ground") 
        {
            var joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = c.rigidbody;
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
