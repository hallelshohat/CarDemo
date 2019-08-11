using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject focus;
    public float distance = 5;
    public float height = 1;
    public float spd = 1;

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = new Vector3(0, height, -distance);

        transform.position = Vector3.Lerp(transform.position, focus.transform.position + focus.transform.TransformDirection(offset), spd * Time.deltaTime);
        transform.LookAt(focus.transform.position);
    }
}
