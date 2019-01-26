using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    // Attributes
    public ObjectWeight weight;
    public ObjectSize size;
    public Vector3 location;

    public bool open;
    public GameObject key;

    public Object containedObject;
    
    private bool pushable;
    private bool movable;
    private bool grabable;
    private bool examinable;

    public Node currentNode;

    // Start is called before the first frame update
    void Start()
    {
        pushable = weight != ObjectWeight.Unmovable;
        movable = weight == ObjectWeight.Light || weight == ObjectWeight.Medium;
        grabable = weight != ObjectWeight.Unmovable;
        examinable = weight != ObjectWeight.Light || weight != ObjectWeight.Medium || !open;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool BeingPushed ()
    {
        if (pushable)
        {
            bool actionSuccessful = false;

            // TODO


            return actionSuccessful;
        } else {
            return pushable;
        }
    }





}
