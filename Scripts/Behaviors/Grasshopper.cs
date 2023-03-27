using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grasshopper : MonoBehaviour
{
    
    public GameObject target;
    public float timeBuffer = 10f;
    public float launchForce = 5f;
    Rigidbody rb;
    Vector3 startingPosition;
    Vector3 targetStartingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        targetStartingPosition = target.transform.position;
        Time.timeScale = timeBuffer; // allow for slowing time to see what's happening
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //f to fire
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(rb.isKinematic) rb.isKinematic = false;
            FiringSolution firing = new FiringSolution();
            Debug.Log("In the Space function");
            Nullable<Vector3> aimVector = firing.calculateFiringSolution(transform.position, targetStartingPosition, launchForce, Physics.gravity);
            if (aimVector.HasValue)
                rb.AddForce(aimVector.Value.normalized * launchForce, ForceMode.Impulse);
        }
    } 


}