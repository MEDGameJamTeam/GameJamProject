using UnityEngine;

namespace Player
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;

        [SerializeField] private float cameraSpeed = 0.001f;

        private Vector3 _targetOffset;

        // Start is called before the first frame update
        private void Start()
        {
            _targetOffset = transform.position;
            _targetOffset -= target.position;
        }

        // Update is called once per frame
        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, target.position + _targetOffset, cameraSpeed);
        }
    }
}