using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowFrog : MonoBehaviour
{
    public GameObject frog;
    public Vector3 offSet = new Vector3(0, 8, -10);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// LateUpdate refreshes slightly after update so that the camera
    /// has a smoother following mechanism
    /// </summary>
    void LateUpdate()
    {
        transform.position = frog.transform.position + offSet;
    }
}
