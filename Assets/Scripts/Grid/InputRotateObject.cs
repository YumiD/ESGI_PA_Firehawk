using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
public class InputRotateObject : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 5f;

    private bool dragging = false;

    private GameObject _customPivot;

    private bool _isTerrainGrid = false;

    private void Start() {
        if(gameObject.GetComponent<TerrainGrid>() != null){
            _isTerrainGrid = true;
            _customPivot = new GameObject();
            _customPivot.transform.position = GetComponent<TerrainGrid>().GetCenterGrid();
            _customPivot.name = "Grid Pivot";
        }
    }

    private void RotateObject() {
        float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        if(!_isTerrainGrid){
            transform.Rotate(Vector3.down, XaxisRotation);
        }
        else{
            transform.RotateAround(_customPivot.transform.position, Vector3.up, XaxisRotation);
        }
    }
    private void Update() {
        if(Input.GetMouseButtonDown(2))
        {
            RotateObject();
            dragging = true;
        }
        if(Input.GetMouseButtonUp(2))
        {
            RotateObject();
            dragging = false;
        }

        if(dragging )
        {
            RotateObject();
        }
    }
}
}