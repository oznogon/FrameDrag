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
        public List<Collider> draggers;
    }
}