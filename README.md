# FrameDrag

Unity3d frame dragging scripts, based on [why485's video](https://www.youtube.com/watch?v=1VFXQXjmdIk).

This allows one object to carry another without changing the hierarchy, and while matching velocity and rotation even through complex maneuvers.

> **Hey!** I'm not great at this stuff yet. This code is unoptimized, probably quite buggy, and shouldn't be used in production.

## Usage

1.  Add the `FrameDragReciever.cs` script to objects that should be "carried", such as the player vehicle.
1.  Add the `FrameDragger.cs` script to objects that should "carry" other objects, such as a carrier vehicle.
1.  Add a trigger collider to the FrameDragger object. The collider's area represents the maximum area of effect for the dragging behavior.

When the reciever enters the dragger's trigger, it detects the dragger's velocity and angular velocity, then gets "dragged" by translating its position using those values.

### Degrading the effect

By default, the dragging effect affects the receiver immediately and fully when it enters the trigger collider. If an receiver has a velocity upon entering the trigger, it adds the dragger's velocities immediately, causing the object to appear to instantly accelerate forward within the dragger.

For applications like a vehicle entering a carrier, this can be undesirable. The receiver has a public Drag Magnitude property, which can be manipulated manually or by other scripts. The lower the magnitude, the less the receiver's position is dragged by the dragger.

The `FrameDragDegrader.cs` script implements what's probably the stupidest mitigation method: degrading the effect linearly over distance from a fixed radius.

This behavior ("Degrade By Distance" toggle on the dragger) is enabled by default in `FrameDragDegrader`. The dragging effect is reduced the further the receiver is from a radius ("Drag Effect Radius") centered around a point ("Drag Effect Origin"). If no origin is specified before the dragger is instantiated, the degrader script uses its own object's transform by default.

### Applications

The most straightforward application of frame dragging is to provide a carrier for other objects that move relative to another object's velocities, such as passengers or cargo in a vehicle, or vehicles being launched from or landing onto (or into) a carrier vehicle.

You can also use it to simulate the relative velocity of an object within another object's atmosphere, by applying the effect with degregation over distance as the receiver enters the dragger's atmosphere.

Frame dragging can also simulate a tractor effect, by dragging receivers that wind up inside the collider of a dragger that the player can control.

### Limitations

This implementation isn't built for 2D.

In this implementation, dragging relies on Rigidbodies, but the concept itself doesn't require them. Because this implementation uses Unity physics, its also reliant on the physics timestep, which can cause issues with objects that move or rotate at high velocities, or which change velocities by large amounts very quickly.

Nothing in this implementation reports the relative velocity between the receiver and the dragger. If you've implemented collision damage or other effects based on the dragger's or receiver's velocity or other physics properties, you might be surprised by the results when those interactions occur between receivers or a receiver and its dragger.

This implementation isn't optimized for large numbers of receivers affected by a single dragger.