using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    private PlayerMovement playerMovement;
     
    public void Attack()
    {
        playerMovement.Attack();
    }
   

    
}
