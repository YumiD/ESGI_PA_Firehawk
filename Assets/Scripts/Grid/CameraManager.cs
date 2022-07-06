using DG.Tweening;
using UnityEngine;

namespace Grid
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private float speed;
        private bool _canMove = true;
        
        private void Update()
        {
            if (!_canMove) return;
            
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

        public void MoveCamera()
        {
            Vector3 eulerAngles = GetTransformEulerAngles(out float rotation);

            transform.DORotate(new Vector3(-rotation, eulerAngles.y, eulerAngles.z), 1f);
        }

        public void ActivateMovement(bool canMove)
        {
            _canMove = canMove;
        }

        private Vector3 GetTransformEulerAngles(out float rotation)
        {
            Vector3 eulerAngles = transform.eulerAngles;
            if (transform.eulerAngles.x <= 180f)
            {
                rotation = transform.eulerAngles.x;
            }
            else
            {
                rotation = transform.eulerAngles.x - 360f;
            }

            return eulerAngles;
        }
    }
}
