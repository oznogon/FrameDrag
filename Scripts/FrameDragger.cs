using UnityEngine;

// Frame dragging method by why485/Brian Hernandez
// https://www.youtube.com/watch?v=1VFXQXjmdIk

namespace Oznogon.FrameDrag
{
    [RequireComponent(typeof(Rigidbody))]

    public class FrameDragger : MonoBehaviour
    {
        public Vector3 carrierVelocity;

        private void FixedUpdate()
        {
            carrierVelocity = transform.GetComponent<Rigidbody>().velocity;
        }
    }
}