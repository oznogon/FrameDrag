using UnityEngine;

namespace Oznogon.FrameDrag
{
    [RequireComponent(typeof(FrameDragger))]
    public class FrameDragDegrader : MonoBehaviour
    {
        private FrameDragger dragger;

        [Tooltip("Whether the effect is the same everywhere in the trigger " +
            "collider, or if it degrades linearly outside of the effect " +
            "radius.")]
        public bool degradeByDistance = true;

        [Tooltip("The radius of maximum effect, if it doesn't degrade over " +
            "distance from the effect origin. Has no effect if Degrade By " +
            "Distance is disabled.")]
        public float dragEffectRadius = 10.0f;

        [Tooltip("The origin of the effect if it degrades over distance. If " +
            "left empty on start, this automatically uses this object's " +
            "transform. Has no effect if Degrade By Distance is disabled.")]
        public Transform dragEffectOrigin;

        private void Start()
        {
            // If there's no effect origin set, use this object's transform.
            if (dragEffectOrigin == null)
            {
                dragEffectOrigin = transform;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            // If you don't plan on using degredation over distance, you can
            // remove OnTriggerStay entirely and lighten the load.
            dragger = GetComponent<FrameDragger>();
            FrameDragReceiver receiver = other.GetComponent<FrameDragReceiver>();

            if (receiver != null && receiver.isActiveAndEnabled)
            {
                if (degradeByDistance)
                {
                    // A full dragMagnitude (1.0) means the receiver almost
                    // perfectly matches the dragger's rotation and velocity.
                    // The magnitude should be full if the receiver is within
                    // the dragEffectRadius of the dragEffectOrigin.position,
                    // and degrade the further the object is from that radius.
                    //
                    // There are many ways to do this, and all of them should
                    // be smarter than this.
                    float receiverDistance = Vector3.Distance(receiver.transform.position, dragEffectOrigin.position);
                    receiver.dragMagnitude = Mathf.Clamp01(dragEffectRadius / receiverDistance);
                }
                else
                {
                    receiver.dragMagnitude = 1.0f;
                }
            }
        }
    }
}