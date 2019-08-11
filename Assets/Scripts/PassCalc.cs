using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassCalc : MonoBehaviour
{
    public GameObject oppCars;
    public GameObject carPass;
    public Material laneMat;
    float xsize; // length of one car

    // Start is called before the first frame update
    void Start()
    {
        xsize = carPass.GetComponent<Collider>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float carVel = Mathf.Abs(GetComponent<Rigidbody>().velocity.x); // m/s
        float passVel = Mathf.Abs(carPass.GetComponent<Rigidbody>().velocity.x); // m/s

        float timePass = CalcTimeToPass(carVel, passVel);
        float timeInter = MinIntersection(carVel);

        Debug.Log("Pass: " + timePass + " inter: " + timeInter);
        bool canPass = false;
        if(timePass < timeInter && timePass >= 0)
        {
            canPass = true;
        }
        laneMat.color = (canPass) ? new Color(0, 1, 0) : new Color(1, 0, 0);
    }

    float CalcTimeToPass(float carVel, float passVel)
    {
        float dist = transform.position.x - (carPass.transform.position.x - xsize);

        float dV = carVel - passVel;

        float tPass = dist / dV;
        //Debug.Log("dist: " + dist + "\ndV : " + dV + "\ntPass: " + tPass);
        return tPass;
    }

    float MinIntersection(float carVel)
    {
        Transform[] t = oppCars.GetComponentsInChildren<Transform>();
        float minTime = float.MaxValue;
        foreach (Transform a in t)
        {
            if (a.CompareTag("Car"))
            {
                Rigidbody r = a.GetComponent<Rigidbody>();
                float oppVel = r.velocity.x;
                float dist = transform.position.x - a.position.x - xsize;

                float x = (dist * carVel) / (carVel + oppVel);
                float time = x / carVel;
                minTime = Mathf.Min(minTime, time);
            }
        }
        return minTime;
    }
}
