
-------------------------------------------------------------------------
Basic setup:

1. Prepare a properly setup character, both Mecanim and legacy rigs are supported.
2. Select the game object you want to apply Dynamic Bone.
3. In the component menu, select Dynamic Bone -> Dynamic Bone.
4. In the inspector, select root object.
5. Adjust dynamic bone parameters (see detail descriptions in the following section).


You can add collider objects if required:

1. Select game object the collider will attached.
2. In the component menu, select Dynamic Bone -> Dynamic Bone Collider.
3. Adjust position and size of the collider.
4. In Dynamic Bone component, increase size of colliders and append corresponding object.


-------------------------------------------------------------------------
Dynamic Bone component description:

- Root
  The root of the transform hierarchy to apply physics.

- Roots
  Multiple roots are allowed. They all share the same parameters.

- Update Rate
  Internal physics simulation rate, measures in frames per seconds.

- Update Mode
  Normal: Update physics in fixed timestamp as specified rate.
  AnimatePhysics: Updates during the physic loop in order to synchronized with the physics engine.
  UnscaledTime: Updates independently of Time.timeScale.
  Default: Update physics every frame instead of specified rate, recommended.

- Damping
  How much the bones slowed down.

- Elasticity
  How much the force applied to return each bone to original orientation.

- Stiffness
  How much bone's original orientation are preserved.

- Inert
  How much character's position change is ignored in physics simulation.

- Friction
  How much the bones slowed down when collide.

- Radius
  Each bone can be a sphere to collide with colliders. Radius describe sphere's size.

- Damping Distrib, Elasticity Distrib, Stiffness Distrib, Inert Distrib, Radius Distrib
  How parameters change over hierarchy chain. Curve values are multiplied to corresponding parameters. 

- End Length
  If End Length is not zero, an extra bone is generated at the end of transform hierarchy, 
  length is multiplied by last two bone's distance.

- End Offset
  If End Offset is not zero, an extra bone is generated at the end of transform hierarchy, 
  offset is in character's local space.

- Gravity
  The force apply to bones, in world space. Partial force apply to character's initial pose is cancelled out.

- Force
  The force apply to bones, in world space.

- Blend Weight
  Control how physics blends with existing animation.

- Colliders
  Collider objects interact with the bones.

- Exclusions
  Bones exclude from physics simulation.
     
- Freeze Axis
  Constrain bones to move on specified plane.

- Distant Disable, Reference Object, Distance To Object
  Disable physics simulation automatically if character is far from camera or player.
  If there is no reference object, default main camera is used.

Dynamic Bone Collider component description:

- Center
  The center of the sphere or capsule, in the object's local space.

- Radius
  The radius of the sphere or capsule, will be scaled by the transform's scale.

- Height
  The height of the capsule, including two half-spheres, will be scaled by the transform's scale.

- Radius 2
  The other radius of the capsule. 0 means same as Radius.

- Direction
  The axis of the capsule's height.

- Bound
  Constrain bones to outside bound or inside bound.

-------------------------------------------------------------------------
Dynamic Bone script reference:

- public void SetWeight(float w);
  Control how physics blend with existing animation.

- public void UpdateParameters();
  Update parameters at runtime, call this funtion after modifing parameters.

- public bool m_Multithread
  Enable/disable multithread to improve physics simulation performace. Default is true.
-------------------------------------------------------------------------
