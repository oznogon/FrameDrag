using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Frame dragging method by why485/Brian Hernandez
// https://www.youtube.com/watch?v=1VFXQXjmdIk

namespace Oznogon.FrameDrag
{
    [RequireComponent(typeof(Rigidbody))]
    public class FrameDragger : MonoBehaviour
    {
        [Tooltip("List of objects being dragged. Updated automatically.")]
        public List<FrameDragReceiver> dragged;

        [Tooltip("Whether the effect is the same everywhere in the trigger " +
            "collider, or if it degrades linearly outside of the effect " +
            "radius.")]
        public bool degradeEffectByDistance = true;

        [Tooltip("The radius of maxium effect, if it doesn't degrade over " +
            "distance from the effect origin.")]
        public float dragEffectRadius = 3.0f;

        [Tooltip("The origin of the effect if it degrades over distance. If " +
            "left empty on start, this automatically uses this object's " +
            "transform.")]
        public Transform effectOrigin;

        private void Start()
        {
            // If there's no effect origin set, use this object's transform.
            if (effectOrigin == null)
            {
                effectOrigin = transform;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            FrameDragReceiver receiver = other.GetComponent<FrameDragReceiver>();

            // We have to track all dragged objects because trigger colliders
            // detect collisions even when the trigger's object is disabled.
            // If we don't track this, receivers of a disabled dragger retain
            // modified velocities; they're effectively still dragged as if the
            // object wasn't disabled. :( Thanks, Unity!
            if (receiver != null && receiver.isActiveAndEnabled)
            {
                bool listContainsItem = dragged.Contains(receiver);

                // Add a receiver to the list of dragged objects only if it's
                // not already on the list, and only on enter.
                if (!listContainsItem)
                {
                    dragged.Add(receiver);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            FrameDragReceiver receiver = other.GetComponent<FrameDragReceiver>();

            // We have to track all dragged objects because trigger colliders
            // detect collisions even when the trigger's object is disabled.
            // If we don't track this, receivers of a disabled dragger retain
            // modified velocities; they're effectively still dragged as if the
            // object wasn't disabled. :( Thanks, Unity!
            if (receiver != null && receiver.isActiveAndEnabled)
            {
                bool listContainsItem = dragged.Contains(receiver);

                // Remove a receiver from the list of dragged objects only if
                // it's not already on the list, and only on exit.
                if (listContainsItem)
                {
                    dragged.Remove(receiver);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            // If you don't plan on using degredation over distance, you can
            // remove OnTriggerStay entirely and lighten the load.
            FrameDragReceiver receiver = other.GetComponent<FrameDragReceiver>();

            if (receiver != null && receiver.isActiveAndEnabled)
            {
                if (degradeEffectByDistance)
                {
                    // A full dragMagnitude (1.0) means the receiver almost
                    // perfectly matches the dragger's rotation and velocity.
                    // The magnitude should be full if the receiver is within
                    // the dragEffectRadius of the effectOrigin.position, and
                    // degrade more the further the object is from that radius.
                    //
                    // There are many ways to do this, all of them smarter than
                    // mine.
                    float receiverDistance = Vector3.Distance(receiver.transform.position, effectOrigin.position);
                    receiver.dragMagnitude = Mathf.Clamp01(dragEffectRadius / receiverDistance);
                }
                else
                {
                    receiver.dragMagnitude = 1.0f;
                }
            }
        }

        private void OnDisable()
        {
            // We have to track all dragged objects because trigger colliders
            // detect collisions even when the trigger's object is disabled.
            // If we don't track this, receivers of a disabled dragger retain
            // modified velocities; they're effectively still dragged as if the
            // object wasn't disabled. :( Thanks, Unity!
            if (dragged != null)
            {
                // I probably shouldn't use foreach.
                foreach (FrameDragReceiver receiver in dragged)
                {
                    receiver.RemoveDrag();
                }

                dragged.Clear();
            }
        }
    }
}