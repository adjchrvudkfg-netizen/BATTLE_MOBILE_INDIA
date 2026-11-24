using UnityEngine;
using BattleMobileIndia.UI;

namespace BattleMobileIndia.Gameplay
{
    public class CameraAimController : MonoBehaviour
    {
        [Header("References")]
        public Transform playerBody;
        public Transform cameraPivot;
        public Camera playerCamera;
        public SensitivityController sensitivityController;

        [Header("Aim Settings")]
        public string activeScopeMode = "General"; // General, RedDot, 2x, 4x, Sniper
        public float baseSensitivityMultiplier = 0.08f;
        public float minPitch = -75f;
        public float maxPitch = 80f;

        private float _pitch;

        private void Update()
        {
            float sensValue = sensitivityController != null
                ? sensitivityController.GetActiveSensitivity(activeScopeMode)
                : 85f;

            float finalSens = sensValue * baseSensitivityMultiplier;

            float mouseX = Input.GetAxis("Mouse X") * finalSens;
            float mouseY = Input.GetAxis("Mouse Y") * finalSens;

            _pitch -= mouseY;
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

            if (cameraPivot != null)
            {
                cameraPivot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
            }

            if (playerBody != null)
            {
                playerBody.Rotate(Vector3.up * mouseX);
            }
        }

        public void SetScopeMode(string mode, float fov = 60f)
        {
            activeScopeMode = mode;
            if (playerCamera != null)
            {
                playerCamera.fieldOfView = fov;
            }
        }
    }
}

