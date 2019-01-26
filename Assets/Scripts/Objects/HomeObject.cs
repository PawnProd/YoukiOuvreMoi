using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeObject : MonoBehaviour
{
    // Attributes
    public ObjectSize size;

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

    public bool BeingPushed (Dog dog, Vector3 direction)
    {
        if (pushable)
        {
            bool actionSuccessful = false;
            
            if (onTopObject != null)
            {
                onTopObject.MoveObject(transform.forward);
                onTopObject.size = ObjectSize.Ground;

                if (onTopObject == key)
                {
                    Open();
                }
            }

            if (movable)
            {
                MoveObject(direction);
            }

            return actionSuccessful;
        } else {
            return pushable;
        }
    }

    public bool BeingGrabed (Dog dog)
    {
        if (grabable)
        {
            bool actionSuccessful = true;
            transform.SetParent(dog.transform);
            
            return actionSuccessful;
        } else {
            return grabable;
        }
    }

    public bool BeingReleased (Dog dog)
    {
        if (grabed)
        {
            bool actionSuccessful = false;

            // TODO changer le parent du transform via le GameController


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
                containedObject.gameObject.SetActive(true);
                containedObject.MoveObject(transform.forward);
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

    public bool MoveObject (Vector3 direction)
    {
        bool actionSuccessful = false;

        Vector3 newPosition = transform.position + direction;
        Node newNode = GameController.Instance.gridSystem.NodeFromWorlPoint(newPosition);

        // TODO

        return actionSuccessful;
    }
}
