using DG.Tweening;
using UnityEngine;

namespace Grid
{
public class InputRotateObject : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 5f;

    private void RotateObject() {
        float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        transform.RotateAround(new Vector3(22.5f, 0.0f, 22.5f), Vector3.up, XaxisRotation);
    }
    private void Update() {
        if (Input.GetMouseButton(2) && !DOTween.IsTweening(transform))
        {
            RotateObject();
        }
    }
}
}