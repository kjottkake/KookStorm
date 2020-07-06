using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

float yBound;
[SerializeField] GameObject board;

[SerializeField]float turnSpeed = 130f;
[SerializeField]float baseAccel = 450f;
[SerializeField]float gravEffect = 2f;
[SerializeField]float turnEffect = 2f;
[SerializeField]float heightEffect = 1f;


[SerializeField] Vector3 startPos = new Vector3(0, 0, 0);
[SerializeField] Vector3 wavePushVec = new Vector3(-1.5f, 1f, 0f);


float currentAngle = 0;
Vector3 prevNormal = Vector3.right;
Vector3 velocity = Vector3.zero;

float currentSpeed = 0;

    void Awake()
    {
        transform.position = startPos;
        yBound = Camera.main.orthographicSize;

    }

    float CalculateAngle(Vector2 input, float a, float r, float t)
    {
        //Up input turns CCW, down turns CW
        //X input influences turn speed

        float newAngle = a + (input.y * r / (input.x + 1) * t);  
        newAngle %=  360;

        return newAngle;
    }

    Vector3 CalculateMovement(float a,  float s, Vector3 pos, float t)
    {
        //Maintain momentum
        //When moving down increase speed
        //When turning upward from a downward trajecory, increase speed
        //Move faster at middle of the wave
        //Move slower further from the break maybe

        
        Vector3 normal = new Vector3(Mathf.Cos(Mathf.Deg2Rad * a), Mathf.Sin(Mathf.Deg2Rad * a), 0);
        float gravMod = (Vector3.Dot(normal, Vector3.down) *.5f) + .5f;

        float turnMod = normal.y < 0 && normal.y > prevNormal.y ? normal.y - prevNormal.y : 0;

        float heightMod = (Mathf.Sqrt(1 - Mathf.Abs(((yBound - pos.y) / yBound) - 1))) *  gravMod; 

        gravMod *= gravEffect;
        turnMod *= turnEffect;
        heightMod = Mathf.Lerp(1, heightMod, heightEffect);

        Debug.Log("grav " + gravMod);
        Debug.Log("turn " + turnMod);
        Debug.Log("height " + heightMod);


        s += (gravMod + turnMod) * heightMod * baseAccel * t;


        Vector3 moveVec = normal * s  * t;
        moveVec += wavePushVec * t;

        prevNormal = normal;
        return moveVec;
    }


    void Update()
    {

        /* When running the game in the editor, the first cycle takes about a second, which screws up dTime and sends you flying before you get control.  
        Shouldn't affect anything once the start menu is in place.*/

        //I'm getting occasional lagspikes, no idea why.

        float dTime = Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
            //Get mouse position normalized from -1 to 1
            float mouseRatioX = Mathf.Clamp((Input.mousePosition.x / Screen.width)- 0.5f, -1, 1);
            float mouseRatioY = Mathf.Clamp((Input.mousePosition.y / Screen.height)- 0.5f, -1, 1);
            Vector2 mousePos = new Vector2((mouseRatioX ) *2, (mouseRatioY) *2);


            currentAngle = CalculateAngle(mousePos, currentAngle, turnSpeed, dTime);

            board.transform.rotation = Quaternion.AngleAxis(currentAngle -90,Vector3.forward);
        }


        transform.position += CalculateMovement(currentAngle, currentSpeed, transform.position, dTime);
        


        //Keep player in bounds
        if (transform.position.y < -yBound)
        {
            transform.position = new Vector3(transform.position.x, -yBound, transform.position.z);
        }
        if (transform.position.y > yBound)
        {
            transform.position = new Vector3(transform.position.x, yBound, transform.position.z);
        }
    }
}
