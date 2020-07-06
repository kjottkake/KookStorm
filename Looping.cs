using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looping : MonoBehaviour
{
    [SerializeField] GameObject pref;
    [SerializeField]int count = 3;
    [SerializeField]float width = 31;
    [SerializeField] float speed= -1;
    List<GameObject> objects= new List<GameObject>();

    void Awake()
    {
      for (int i = 0; i < count; i++)
      {
          objects.Add(Instantiate(pref,new Vector3(width*(i-1),0,0), Quaternion.identity));
      }
      
    }

    void Update()
    {
        foreach (GameObject obj in objects)
        {
            obj.transform.position =new Vector3(obj.transform.position.x + speed * Time.deltaTime, 0 , 0 ) ;
            if (obj.transform.position.x <= -width * (count - ((float) count * .5f)))
            {
                obj.transform.position= new Vector3(width * ((float) count * .5f),0,0);
            }
            if (obj.transform.position.x >= width * (count + ((float) count * .5f)))
            {
                obj.transform.position= new Vector3(width * -((float) count * .5f),0,0);
            }
        }
    }
}