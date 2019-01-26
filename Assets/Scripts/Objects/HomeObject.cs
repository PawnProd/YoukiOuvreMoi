using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeObject : MonoBehaviour
{
    // Attributes
    public ObjectSize size;

    public float speed = 1.0f;

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

    public string title;
    public string content;

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
            bool actionSuccessful = true;
            
            if (onTopObject != null)
            {
                onTopObject.MoveObject(dog.transform.position);
                onTopObject.size = ObjectSize.Ground;
                onTopObject = null;
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

            grabed = true;
            GameController.Instance.phase = Phase.SELECTACTION;
            return actionSuccessful;
        } else {
            return grabable;
        }
    }

    public bool BeingReleased (Vector3 position)
    {
        if (grabed)
        {
            Debug.Log("Grab ok !");
            bool actionSuccessful = true;

            transform.position = position;
            gameObject.SetActive(true);

            grabed = false;

            currentNode = GameController.Instance.gridSystem.NodeFromWorlPoint(transform.position);

            if (gameObject.tag.Equals("FinalKey"))
            {
                if (verifyEndConditions())
                {
                    GameController.Instance.EndOfGame(true);
                }
            }

            if (gameObject.tag.Equals("Tips") && GameObject.FindGameObjectWithTag("FinalKey").transform.position == transform.position)
            {
                GameController.Instance.ath.LoadTips(title, content, this.gameObject);
            }
            GameController.Instance.phase = Phase.SELECTACTION;
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
            Debug.Log("Coucou");
            bool actionSuccessful = false;
            if (containedObject != null)
            {
                Debug.Log("containedObject = " +containedObject.name);
                containedObject.gameObject.SetActive(false);
                containedObject.GetComponent<HomeObject>().grabed = true;
                containedObject.size = ObjectSize.Ground;
                containedObject.currentNode = GameController.Instance.gridSystem.NodeFromWorlPoint(containedObject.transform.position);
                containedObject = null;
            }

            return actionSuccessful;
        }
        else
        {
            return examinable;
        }
    }

    public void MoveObject (Vector3 position)
    {
        StartCoroutine(CO_Move(position));
    }

    public bool verifyEndConditions ()
    {
        return GameObject.FindGameObjectWithTag("FinalKey").transform.position == GameObject.FindGameObjectWithTag("Door").transform.position;
    }

    IEnumerator CO_Move (Vector3 position)
    {
        while ((position - transform.position).sqrMagnitude != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        currentNode = GameController.Instance.gridSystem.NodeFromWorlPoint(transform.position);
        GameController.Instance.phase = Phase.SELECTACTION;
        yield return null;
    }
}
