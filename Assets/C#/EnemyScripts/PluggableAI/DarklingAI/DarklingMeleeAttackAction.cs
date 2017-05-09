using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/******************************************************************************
 * 
 * Darkling Melee Attack
 * 
 * basically charge right into destination
 * 
 ******************************************************************************/

[CreateAssetMenu(menuName = "PluggableAI/Actions/Darkling/MeleeAttack")]
public class DarklingMeleeAttackAction : EnemyAction
{
    public override void Act(EnemyStateController controller)
    {
        DarklingAirEnemy darkling = (DarklingAirEnemy)controller.enemy;

        Attack(darkling);
    }

    private void Attack(DarklingAirEnemy darkling)
    {
       
        //set destination to charge right in
        if (!darkling.isAttacking)
        {
            darkling.isAttacking = true;
            darkling.isAllowedToAttack = true;
            darkling.hasDoneAttacking = false;

            //set destination for darkling to charge right in
            //careful with null ref
            if (darkling.target != null)
            {
                darkling.transform.LookAt(darkling.target);

                //make the destination further back, so it feels like darkling cannot stop ==
                darkling.destination =
                    darkling.target.position + darkling.transform.forward.normalized * darkling.distanceRunAfterAttack;
            }
            else
            {
                darkling.destination = darkling.transform.position;
            }
                   
        }

        ChargeAtTarget(darkling, darkling.destination);

    }
        
    void ChargeAtTarget(DarklingAirEnemy darkling, Vector3 target)
    {
        //charge into destination
        darkling.MoveToward(target, darkling.movementSpeed * 3);

        //if encounter player, hit it
        if (darkling.isCloseEnoughToTarget(darkling.target.position, darkling.distanceMeleeAttack))
        {
            //get Hittable component
            Hittable h;
            if (
                (h = darkling.target.GetComponent<Hittable>())
                && darkling.isAllowedToAttack)
            {
                //attack target
                Debug.Log("Darkling: hit");
                darkling.StartMeleeAttackAnimation();
                h.Hit(darkling.attackDamage, darkling.attackType);
                darkling.isAllowedToAttack = false;
            }
           
        }
       

       //when reach destination, ready for next attack
       if (darkling.isCloseEnoughToTarget(darkling.destination, 5))
        {
            darkling.hasDoneAttacking = true;
            darkling.isAttacking = false;
        }

    }
        
               
    
}
