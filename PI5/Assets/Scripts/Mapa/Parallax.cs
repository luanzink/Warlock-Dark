using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cam;
    public Transform[] layers;
    public float[] mult;
    private Vector3[] posOrigin;


    void Awake()
    {
        posOrigin = new Vector3[layers.Length];

        for (int i = 0; i < layers.Length; i++)
        {
            posOrigin[i] = layers[i].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].position = posOrigin[i] + mult[i] * (new Vector3(cam.position.x,cam.position.y, layers[i].position.z));
        }
    }
}
