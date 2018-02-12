using UnityEngine;

// Frame dragging method by why485/Brian Hernandez
// https://www.youtube.com/watch?v=1VFXQXjmdIk

namespace Oznogon.FrameDrag
{
    [RequireComponent(typeof(Rigidbody))]
    public class FrameDragReceiver : MonoBehaviour
    {
        // Last angular velocity of the dragger at the receiver's position.
        private Vector3 lastDragAngularVelocity;
        // Last velocity of the dragger at the receiver's position.
        private Vector3 lastDragVelocity;
        // Only one dragger should drag any given receiver.
        private FrameDragger dragger;
        private Rigidbody draggerRb;

        [Tooltip("Whether to use fixedUpdate (for physics) or Update " +
            "(non-physics). You should probably keep this enabled.")]
        public bool useFixedUpdate = true;

        [Range(0.0f, 1.0f)]
        [Tooltip("How effectively a dragger drags this object. Might be " +
            "automatically managed by the dragger.")]
        public float dragMagnitude = 1.0f;

        private void OnTriggerEnter(Collider other)
        {
            dragger = other.transform.GetComponent<FrameDragger>();

            if (dragger != null && dragger.isActiveAndEnabled)
            {
                draggerRb = dragger.GetComponent<Rigidbody>();
            }
            else
            {
                // For safety's sake, remove any drag settings if we detect
                // entering a dragger that doesn't exist or isn't enabled.
                RemoveDrag();
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            // Remove the dragger when we exit its trigger.
            RemoveDrag();
        }

        private void Update()
        {
            if (!useFixedUpdate)
                GetDragged();
        }

        private void FixedUpdate()
        {
            if (useFixedUpdate)
                GetDragged();
        }

        private void GetDragged()
        {
            if (draggerRb != null)
            {
                // Get the velocities of the dragger at the receivers position.
                lastDragVelocity = draggerRb.GetPointVelocity(transform.position);
                lastDragAngularVelocity = draggerRb.angularVelocity * Mathf.Rad2Deg;
                // Drag the receiver accordingly. The dragMagnitude influences
                // how effectively the receiver is dragged.
                transform.position += lastDragVelocity * Time.deltaTime * dragMagnitude;
                transform.Rotate(lastDragAngularVelocity * Time.deltaTime * dragMagnitude, Space.World);
            }
        }

        public void RemoveDrag()
        {
            dragger = null;
            draggerRb = null;
        }
    }
}