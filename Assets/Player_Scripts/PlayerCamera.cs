using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror.Examples.TankTheftAuto
{
    public class PlayerCamera : NetworkBehaviour
    {
        Camera mainCam;

        public float lookSpeed = 2f;
        public float lookXLimit = 90; // Adjust this value for your desired vertical rotation limit

        float rotationX = 0;

        void Awake()
        {
            mainCam = Camera.main;
        }

        public override void OnStartLocalPlayer()
        {
            if (mainCam != null)
            {
                // configure and make camera a child of player with 1st person offset
                mainCam.orthographic = false;
                mainCam.transform.SetParent(transform);
                mainCam.transform.localPosition = new Vector3(0f, 0.6f, 0f); // Adjust the height to eye level
                mainCam.transform.localEulerAngles = Vector3.zero; // Reset rotation
            }
            else
            {
                Debug.LogWarning("PlayerCamera: Could not find a camera in the scene with 'MainCamera' tag.");
            }
        }

        public override void OnStopLocalPlayer()
        {
            if (mainCam != null)
            {
                mainCam.transform.SetParent(null);
                SceneManager.MoveGameObjectToScene(mainCam.gameObject, SceneManager.GetActiveScene());
                mainCam.orthographic = true;
                mainCam.orthographicSize = 15f;
                mainCam.transform.localPosition = new Vector3(0f, 70f, 0f);
                mainCam.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
            }
        }

        void Update()
        {
            if (!isLocalPlayer)
                return;

            HandleRotation();
        }

        void HandleRotation()
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            mainCam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}
