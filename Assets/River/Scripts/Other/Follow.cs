using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform ObjectToFollow;

    private void Update()
    {
        transform.position = ObjectToFollow.position;
        transform.rotation = ObjectToFollow.rotation;
    }
}
