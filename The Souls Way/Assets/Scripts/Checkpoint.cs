using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform trans;
    // Update is called once per frame
    private void Awake()
    {
        trans = transform;
    }
    void Update()
    {
        trans.Rotate(0, 0, 3);
    }
}
