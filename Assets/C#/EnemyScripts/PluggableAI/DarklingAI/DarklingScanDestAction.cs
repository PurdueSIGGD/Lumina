using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Darkling/Scan Destination")]
public class DarklingScanDestAction : EnemyAction
{
    public override void Act(EnemyStateController controller)
    {
        DarklingAirEnemy darkling = (DarklingAirEnemy)controller.enemy;
        ScanDestination(darkling);
    }

    private void ScanDestination(DarklingAirEnemy darkling)
    {
        //set darkling state to scanning destination
        if (!darkling.destinationPending)
            darkling.destinationPending = true;

        //pick a new destination for darkling
        if (!darkling.nextDestinationDirectionPick)
        {
            Quaternion randomDirection = Quaternion.Euler(0, Random.Range(0, 360), 0);
            darkling.transform.rotation = randomDirection;
            darkling.nextDestinationDirectionPick = true;
        }

        //see if that destination is safe
        RaycastHit hit; //use later

        //if there is sth in the way, rotate a bit
        Debug.DrawRay(darkling.eyes.position, darkling.eyes.forward.normalized * darkling.distanceFlyMin, Color.green);

        if (Physics.Raycast(darkling.eyes.position, darkling.eyes.forward, darkling.distanceFlyMin))
        {
            darkling.transform.Rotate(0, darkling.turningSpeed * Time.deltaTime, 0);
        }

        //if found a destination
        else
        {
            Vector3 nextDestination = 
                darkling.transform.position + darkling.transform.forward * darkling.distanceFlyMin;

            darkling.destination = nextDestination;
            darkling.destinationPending = false;
            darkling.nextDestinationDirectionPick = false;
        }

    }
}
