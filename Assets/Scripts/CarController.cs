
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 7000;
    public float maxAng = 15f;
    public List<WheelCollider> frWheels;
    public List<WheelCollider> baWheels;
    public float brkspd = 3000;
    public GameObject backLightLeft, backLightRight;
    public Material backLightMaterial, blinkingMaterial;
    public int blinking = 0; // 0 - not blinking, 1 - right, -1 - left

    // Start is called before the first frame update
    void Start()
    {
        Vector3 c = GetComponent<Rigidbody>().centerOfMass;
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, c.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float tr = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float ro = Input.GetAxis("Horizontal") * maxAng;
        bool brk = Input.GetKey(KeyCode.Space);
        
        //to open/close blinking
        if (Input.GetKeyDown(KeyCode.Q))
        {
            blinking = (blinking!= -1) ? -1 : 0;
        }else if (Input.GetKeyDown(KeyCode.E))
        {
            blinking = (blinking!= 1) ? 1 : 0;
        }

        HandleBlinkers();

        foreach (WheelCollider wheel in frWheels)
        {
            //rotate the steering direction
            wheel.steerAngle = ro;
        }

        foreach (WheelCollider wheel in frWheels.Concat(baWheels)) 
        {
            if (brk)
            {
                backLightMaterial.SetColor("_EmissionColor", new Color(1, 0, 0));
                wheel.brakeTorque = brkspd * Time.deltaTime;
                wheel.motorTorque = 0;
            }
            else
            {
                backLightMaterial.SetColor("_EmissionColor", new Color(0, 0, 0));
                wheel.brakeTorque = 0;    
                //moves the car
                wheel.motorTorque = tr;
            }
            Quaternion rot;
            Vector3 pos;
            wheel.GetWorldPose(out pos, out rot);
            wheel.gameObject.transform.rotation = rot;
        }
    }

    void HandleBlinkers()
    {

        switch (blinking)
        {
            //change the material to blinking
            case 1:
                backLightRight.GetComponent<Renderer>().material = blinkingMaterial;
                backLightLeft.GetComponent<Renderer>().material = backLightMaterial;
                break;
            case -1:
                backLightLeft.GetComponent<Renderer>().material = blinkingMaterial;
                backLightRight.GetComponent<Renderer>().material = backLightMaterial;
                break;
            default:
                backLightRight.GetComponent<Renderer>().material = backLightMaterial;
                backLightLeft.GetComponent<Renderer>().material = backLightMaterial;
                break;
        }

        float r = Mathf.PingPong(Time.time * 2, 1);
        blinkingMaterial.color = new Color(1, 1, r);
        blinkingMaterial.SetColor("_EmissionColor", new Color(r*0.3f, r*0.3f, 0));
    }
}
