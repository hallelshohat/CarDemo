using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPrefabController : MonoBehaviour
{
    public float speed = 5000;
    public List<WheelCollider> wheels;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        foreach (WheelCollider w in wheels)
        {
            w.motorTorque = speed * Time.deltaTime;

            Quaternion rot;
            Vector3 pos;
            w.GetWorldPose(out pos, out rot);
            w.gameObject.transform.rotation = rot;
        }
    }
}
