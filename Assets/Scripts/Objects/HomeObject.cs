﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeObject : MonoBehaviour
{
    // Attributes
    public ObjectSize size;

    public float speed;

    public bool open;
    public HomeObject key;
    
    public HomeObject containedObject;
    public HomeObject onTopObject;

    public bool pushable;
    public bool movable;
    public bool grabable;
    public bool examinable;

    private bool grabed;

    public Node currentNode;

    // Start is called before the first frame update
    void Start()
    {
        grabed = false;

        currentNode = GameController.Instance.gridSystem.NodeFromWorlPoint(transform.position);

        if (onTopObject != null)
        {
            onTopObject.size = size;
        }

        if (containedObject != null)
        {
            containedObject.gameObject.SetActive(false);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool BeingPushed (Dog dog, Vector3 position)
    {
        if (pushable)
        {
            bool actionSuccessful = false;
            
            if (onTopObject != null)
            {
                onTopObject.MoveObject(dog.transform.position);
                onTopObject.size = ObjectSize.Ground;

                if (onTopObject == key)
                {
                    Open();
                }
            }

            if (movable)
            {
                MoveObject(position);
            }

            return actionSuccessful;
        } else {
            return pushable;
        }
    }

    public bool BeingGrabed ()
    {
        if (grabable)
        {
            bool actionSuccessful = true;
            gameObject.SetActive(false);
            
            return actionSuccessful;
        } else {
            return grabable;
        }
    }

    public bool BeingReleased (Vector3 position)
    {
        if (grabed)
        {
            bool actionSuccessful = true;

            transform.position = position;
            gameObject.SetActive(true);

            return actionSuccessful;
        }
        else
        {
            return grabed;
        }
    }

    public bool BeingExamined (Dog dog)
    {
        if (examinable)
        {
            bool actionSuccessful = false;

            if (containedObject != null && open)
            {
                containedObject.gameObject.SetActive(false);
                containedObject.size = ObjectSize.Ground;
            }

            return actionSuccessful;
        }
        else
        {
            return examinable;
        }
    }

    public void Open ()
    {
        open = true;
    }

    public void MoveObject (Vector3 position)
    {
        StartCoroutine(CO_Move(position));
    }


    IEnumerator CO_Move (Vector3 position)
    {
        while ((position - transform.position).sqrMagnitude != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
