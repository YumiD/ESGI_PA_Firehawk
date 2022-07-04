using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningObject : MonoBehaviour
{
    public Transform customPivot;
    public float rotateSpeed = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(customPivot.position, Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
