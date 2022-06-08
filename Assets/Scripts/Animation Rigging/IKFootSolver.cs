using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] Transform bodyJoint = default;
    [SerializeField] Transform LegPointer = default;
    [SerializeField] IKFootSolver otherFoot = default;
    [SerializeField] float speed = 1;
    [SerializeField] float stepDistance = 4;
    [SerializeField] float stepLength = 4;
    [SerializeField] float stepHeight = 1;
    [SerializeField] Vector3 footOffset = default;
    float footSpacing;

    Vector3 oldPosition, currentPosition, newPosition;

    // Used to calculate the orientation of the foot
    Vector3 oldNormal, currentNormal, newNormal;

    //Value between 0 and 1 Use to show the process of playing the step animation
    float lerp;
    

    // Start is called before the first frame update
    void Start()
    {
        // Record the position relative to the body
        // footSpacing = transform.localPosition.x;

        // Initializing values to be the transform.position that is the position of the foot target
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Position of the IK target is current position
        transform.position = currentPosition;
        transform.up = currentNormal;

        // Casting an invisible line in the scene and detecting intersections with other objects
        // Right direction of the body transform and timing that with the foot spacing
        //  And cast array the direction down
        Ray ray = new Ray(LegPointer.position, Vector3.down);

        Debug.DrawRay(LegPointer.position, Vector3.down, Color.yellow);

        // Check if the array actually hits anything
        // If we hit the ground, store the info in the raycast struct
        // The distance we want to cast the ray
        // colliding with the terrain layer
        if( Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
        {
            Debug.Log("Collision Point: " + info.point);
            // Check if the distance between the new position and the collision point
            // Is greater than the step distance, so the body has moved that the leg needs to move
            // Also check if the foot is not moving
            if(Vector3.Distance(newPosition, info.point) > stepDistance && !otherFoot.IsMoving() && lerp >= 1)
            {
                // Set lerp to 0 as in we start leg movement
                lerp = 0;

                // What direction do we want the new position to be in relative to the body
                // InverseTransformPoint transform world space to calculate is the z position of collision in front or behind character
                // Do we move foot forward or behind
                int direction = bodyJoint.InverseTransformPoint(info.point).z > bodyJoint.InverseTransformPoint(newPosition).z ? 1 : -1;
                
                // Position of IK target is the collision point plus the body.forward and the steplength and the direction back or forward.
                // And add offset to make sure foot is positioned flush on the ground
                newPosition = info.point + (bodyJoint.forward * stepLength * direction) + footOffset;
                
                // New normal is the new orientation, the angle of the ground is going to be set to the collision orientation
                newNormal = info.normal;
            }
        }

        // Now we actually move if the lerp is still in the movement cycle
        if( lerp < 1)
        {
            // Vector3.Lerp allows you to linearly lerp ( systematically change from 0 percent to 100 percent completed change) between the old position and new position 
            // This allows the movement on the foot on the z axis (forward)
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            // Updating the y position allows the foot to do an arc mostion over time with trig..moses..:( between 0 and step height
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            // set the current position to the tempPosition where the foot wants to go
            currentPosition =  tempPosition;
            //  set the current foot normal to the old normal and new normal and the progress of change between them
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);

            // Increment the process value, time.deltatime (time since last frame) times the speed of the character
            lerp += Time.deltaTime * speed;

        }else
        {
            // If the lerp value is larger than 1, it means that the leg has finished moving and we set
            // the oldPosition to the newPosition
            //  and the oldNormal (orientation) to the newNormal (orientation)
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    private void OnDrawGizmos() {
            
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.5f);
    }

    // Return the bool true or false if the lerp based on the lerp
    // Dont step while leg is still moving
    public bool IsMoving(){
        return lerp < 1;
    }
}
