using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Sweeper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print("STARTING SWEEPER");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void  OnTriggerEnter(Collider other)
    {
        print("KILL ALLL BALLS");
        if (other.gameObject.CompareTag("health"))
        {   
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("socreOrb"))
        {
            print("KILL ALLL BALLS");
            Destroy(other.gameObject);

        }
        if (other.gameObject.CompareTag("wall"))
        {
            Destroy(other.gameObject);

        }
    }
}
