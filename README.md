# FrameDrag

Unity3d frame dragging scripts, based on [why485's video](https://www.youtube.com/watch?v=1VFXQXjmdIk).

## Usage

1.  Add the FrameDragRecipient.cs script to objects that should be "carried", such as the player vehicle.
1.  Add the FrameDragger.cs script to objects that should "carry" other objects, such as a carrier vehicle.
1.  Add a trigger collider to the FrameDragger object. The collider's area represents the area of effect.
