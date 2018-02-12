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

        private void OnDisable()
        {
            // We have to track all dragged objects because trigger colliders
            // detect collisions even when the trigger's object is disabled.
            // If we don't track this, receivers of a disabled dragger retain
            // modified velocities; they're effectively still dragged as if the
            // object wasn't disabled. :( Thanks, Unity!
            if (dragged != null)
            {
                // I shouldn't use foreach.
                foreach (FrameDragReceiver receiver in dragged)
                {
                    receiver.RemoveDrag();
                }

                dragged.Clear();
            }
        }
    }
}
