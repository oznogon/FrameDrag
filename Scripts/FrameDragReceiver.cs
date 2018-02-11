using UnityEngine;

// Frame dragging method by why485/Brian Hernandez
// https://www.youtube.com/watch?v=1VFXQXjmdIk

namespace Oznogon.FrameDrag
{
    public class FrameDragReceiver : MonoBehaviour
    {
        public Vector3 lastDragAngularVelocity;
        public Vector3 lastDragVelocity;
        public FrameDragger dragger;
        public Rigidbody draggerRb;
        public bool useFixedUpdate = true;

        private void OnTriggerEnter(Collider collision)
        {
            dragger = collision.transform.GetComponent<FrameDragger>();

            if (dragger == null)
                dragger = collision.transform.GetComponentInParent<FrameDragger>();

            if (dragger != null)
            {
                draggerRb = dragger.GetComponent<Rigidbody>();

                if (draggerRb == null)
                    draggerRb = dragger.GetComponentInParent<Rigidbody>();

                if (draggerRb != null)
                {
                    Debug.Log(name + ": dragger.name = " + dragger.name
                        + "\ndraggerRb.GetPointVelocity(transform.position) = " + draggerRb.GetPointVelocity(transform.position)
                        + "\ndraggerRb.angularVelocity * Mathf.Rad2Deg = " + (draggerRb.angularVelocity * Mathf.Rad2Deg)
                    );
                }
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            dragger = null;
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

        void GetDragged()
        {
            if (dragger != null)
            {
                lastDragVelocity = draggerRb.GetPointVelocity(transform.position);
                lastDragAngularVelocity = draggerRb.angularVelocity * Mathf.Rad2Deg;
                transform.position += lastDragVelocity * Time.deltaTime;
                transform.Rotate(lastDragAngularVelocity * Time.deltaTime, Space.World);
            }
        }
    }
}