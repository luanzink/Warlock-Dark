using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma : MonoBehaviour
{
    private bool moveDown = true;

    [SerializeField]
    private float speed = 3;
    [SerializeField]
    private Transform pontoA,pontoB;
   

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > pontoA.position.y)
            moveDown = true;
        if (transform.position.y < pontoB.position.y)
            moveDown = false;

        if (moveDown)
            transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
        else
            transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);



    }
}
