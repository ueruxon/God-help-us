using UnityEngine;

namespace Game.Scripts.UI
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Start() => 
            _mainCamera = Camera.main;

        private void LateUpdate()
        {
            Quaternion rotation = _mainCamera.transform.rotation;
            transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
        }
    }
}