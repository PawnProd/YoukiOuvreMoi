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
    public Object onTopObject;
    
    private bool pushable;
    private bool movable;
    private bool grabable;
    private bool examinable;

    private bool grabed;

    public Node currentNode;

    // Start is called before the first frame update
    void Start()
    {
        pushable = weight != ObjectWeight.Unmovable && weight != ObjectWeight.Light;
        movable = weight == ObjectWeight.Light || weight == ObjectWeight.Medium;
        grabable = weight != ObjectWeight.Unmovable;
        examinable = CheckExaminable();
        grabed = false;

        currentNode = GameController.Instance.gridSystem.NodeFromWorlPoint(transform.position);
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
                // TODO faire tomber l'objet
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
            bool actionSuccessful = false;
            switch (weight)
            {
                case ObjectWeight.Light:
                    transform.SetParent(dog.transform);
                    actionSuccessful = true;
                    break;
                case ObjectWeight.Medium:
                    transform.SetParent(dog.transform);
                    actionSuccessful = true;
                    break;
                default:
                    break;
            }
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

            // TODO


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

            if (containedObject != null)
            {
                // TODO mettre l'objet contenu sur la case d'en face
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
        examinable = CheckExaminable();
    }

    public bool CheckExaminable ()
    {
        return (weight != ObjectWeight.Light && weight != ObjectWeight.Medium) && open;
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
