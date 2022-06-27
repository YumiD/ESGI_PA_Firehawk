using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float speed;

    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime,0,0));
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime,0,0));
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0,0,-speed * Time.deltaTime));
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0,0,speed * Time.deltaTime));
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(0,speed * Time.deltaTime,0));
        }
        if(Input.GetKey(KeyCode.E))
        {
            transform.Translate(new Vector3(0,-speed * Time.deltaTime,0));
        }
    }
}
