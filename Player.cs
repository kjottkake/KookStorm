using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

float yBound;

[SerializeField] float verticalSpeed = 8f;
[SerializeField] float forwardMomentum = 2f;
[SerializeField] float backwardFriction = .35f;
[SerializeField] Vector3 startPos = new Vector3(0,0,0);
[SerializeField] Vector3 wavePushVec = new Vector3 (-2f, 2f,0);
    void Awake()
    {
        transform.position = startPos;
        yBound = Camera.main.orthographicSize;
    }


    void Update()
    {
        float dTime = Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
        float mouseRatioX = (Input.mousePosition.x / Screen.width)- 0.5f;
        float mouseRatioY = (Input.mousePosition.y / Screen.height)- 0.5f;
 
        Vector2 mousePos = new Vector2((mouseRatioX ) *2, (mouseRatioY) *2);

        float xMove = mousePos.y > 0f ? mouseRatioY / -backwardFriction : mouseRatioY * -forwardMomentum;

        Vector3 moveVec = new Vector3 (xMove,mousePos.y, 0);

        //Debug.Log(mousePos);

        transform.position += moveVec * verticalSpeed * dTime;
        }

        transform.position += wavePushVec *dTime;

        if (transform.position.y <= -yBound)
        {
            transform.position = new Vector3(transform.position.x, -yBound, transform.position.z);
        }
        if (transform.position.y >= yBound)
        {
            transform.position = new Vector3(transform.position.x, yBound, transform.position.z);
        }
    }
}
