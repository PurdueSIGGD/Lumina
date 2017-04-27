using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*********************************************************************
 * 
 * This class is for Enemies who usually moves in Air
 * contains some good functions to deal with Air movement
 *      
 **********************************************************************/
 [RequireComponent(typeof(Rigidbody))]
public abstract class AirEnemy : BaseEnemy {

    
    public float lookSphereCastRadius = 3f;
    public float flyHeight = 10f;
    public float turningSpeed = 15f;
    public float stoppingDistance = 5f;
    
    public const string GROUND_TAG = "Ground";

    [HideInInspector] public Rigidbody rb;

    protected void Setup()
    {
        rb = GetComponent<Rigidbody>();
    }

    /*
     * MoveToward()
     * purely move Rigibody towards target
     * no animation involved
     */
    public void MoveToward(Vector3 target, float speed)
    {

        if (!isFacingTarget(target))
        {
            RotateTowardsTarget(target);
        }
        else
        {
            transform.LookAt(target);
            Vector3 forward =
                transform.position + transform.forward * Time.deltaTime * speed;
            rb.MovePosition(forward);
            //transform.position = forward;
        }
    }

    /**
    * rotate a bit toward target
    * helps animation
    */
    public virtual void RotateTowardsTarget(Vector3 target)
    {
        //get target direction
        Vector3 targetDirection = target - transform.position;

        //get new rotation
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, turningSpeed * Time.deltaTime);

        //set new rotation
        transform.rotation = newRotation;
    }

    /**
   * check if this.Enemy is facing target. such as Player, or next destination
   * just for animation
   */
    public bool isFacingTarget(Vector3 target)
    {
        if (target == null)
            return false;

        Vector3 targetDir = (target - transform.position).normalized;
        float diff = Vector3.Dot(transform.forward, targetDir);

        //if diff ~ 1.0, then it mostly look at target
        //stop Coroutine
        if (diff >= 0.99)
        {
            return true;
        }
        return false;
    }

    /**
     * just normal function,
     * use RayCast to detect ground and set height
     */
    public void SetFlyHeightFromGround(float height)
    {       
        RaycastHit[] raycastHits = Physics.RaycastAll(transform.position, Vector3.down, 100f);
        foreach (RaycastHit hit in raycastHits)
        {
            if (hit.transform.tag == GROUND_TAG)
            {
                transform.position = hit.point + new Vector3(0, height, 0);
                break;
            }
        }
    }


    /*
     * get flying height form Ground!
     */ 
    public float GetFlyHeightFromGround()
    {
        float distance = -1;
        RaycastHit[] raycastHits = Physics.RaycastAll(transform.position, Vector3.down, 100f);
        foreach (RaycastHit hit in raycastHits)
        {
            if (hit.transform.tag == GROUND_TAG)
            {
                distance = transform.position.y - hit.point.y;
                break;
            }
        }

        return distance;
    }


    /*
     * isNearDestination(): while golem is patrolling
     * return true if golem is near its current destination
     */
    public bool isNearDestination(Vector3 destination)
    {
        //calculate the distance
        Vector3 distanceVect = destination - transform.position;
        float distance = Vector3.Magnitude(distanceVect);

        //simple check.
        return distance < stoppingDistance;
    }
}
