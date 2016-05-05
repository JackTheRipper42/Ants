using UnityEngine;

namespace Assets.Scripts
{
    public class InputController : MonoBehaviour
    {
        protected virtual void Update()
        {
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");
            var mouseWheel = Input.GetAxis("Mouse ScrollWheel");
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var upDown = Input.GetAxis("UpDown");

            if (Input.GetAxis("Fire2") > 0)
            {
                Camera.main.transform.Rotate(
                    new Vector3(0f, mouseX, 0f)*4,
                    Space.World);
                Camera.main.transform.Rotate(
                    new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z),
                    -mouseY*4,
                    Space.World);
                Camera.main.transform.Translate(new Vector3(horizontal, upDown, vertical), Space.Self);
            }
            if (Input.GetAxis("Fire3") > 0)
            {
                Camera.main.transform.Translate(new Vector3(mouseX, mouseY, 0f)*-4, Space.Self);
            }
            Camera.main.transform.Translate(new Vector3(0f, 0f, mouseWheel)*40, Space.Self);
        }
    }
}
