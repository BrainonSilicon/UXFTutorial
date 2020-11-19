using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

public class StartPointState : MonoBehaviour
{
    public Session session;
    // define 3 public variables - we can then assign their color values in the inspector.
    public Color red;
    public Color amber;
    public Color green;

    // reference to the material we want to change the color of.
    Material material;

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        // get the material that is used to render this object (via the MeshRenderer component)
        material = GetComponent<MeshRenderer>().material;
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(0.5f);
        material.color = green;
        session.BeginNextTrial();
    }

    /// OnTriggerEnter is called when the Collider 'other' enters the trigger.
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Cursor" & !session.InTrial)
        {
            print("cursor collided with start point");
            material.color = amber;
            StartCoroutine(Countdown());
        }
    }

    /// OnTriggerExit is called when the Collider 'other' has stopped touching the trigger.
    void OnTriggerExit(Collider other)
    {
        if (other.name == "Cursor")
        {      
            print("cursor left start point");
            StopAllCoroutines();
            material.color = red;
        }        
    }
}