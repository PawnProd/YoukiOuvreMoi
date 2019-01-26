using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { private set; get; }
    public GridSystem gridSystem;
    public Dog dog;
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
            if(hits.Length > 0)
            {
                foreach(RaycastHit2D hit in hits)
                {
                    Debug.Log("Hit name = " + hit.collider.name);
                }
                if(hits.Length == 1)
                {
                    Debug.Log("J'ai cliqué sur Game");
                    JumpTo(hits[0].point);
                }
                else
                {
                    int i = 0;
                    bool find = false;

                    while(!find && i < hits.Length)
                    {
                        if(hits[i].collider.name != "Game")
                        {
                            find = true;
                            JumpTo(hits[i].collider.gameObject);
                        }

                        ++i;
                    }
                }
            }
        }
    }

    public void MoveDog(GameObject target)
    {
        gridSystem.FindPath(dog.transform.position, target.transform.position);
        dog.MoveTo(gridSystem.GetGlobalPath());
    }

    public void MoveDog(Vector3 position)
    {
        gridSystem.FindPath(dog.transform.position, position);
        dog.MoveTo(gridSystem.GetGlobalPath());
    }

    public void JumpTo(GameObject target)
    {
        bool canJump = false;
        if(target.GetComponent<Object>() != null)
        {
            Object targetObj = target.GetComponent<Object>();
            if(dog.height == ObjectSize.Low)
            {
                if(targetObj.size == ObjectSize.High || targetObj.size == ObjectSize.Ground)
                {
                    canJump = true;
                }
                    
            }
            else if(dog.height == ObjectSize.High || dog.height == ObjectSize.Ground)
            {
                if (targetObj.size == ObjectSize.Low)
                {
                    canJump = true;
                }
            }

        }

        if (canJump)
        {
            Node node = gridSystem.NodeFromWorlPoint(target.transform.position);
            if (gridSystem.CheckIfObjectIsNear(dog.gameObject, target))
            {
                dog.height = target.GetComponent<Object>().size;
                dog.JumpTo(node);
            }
        }
    }

    public void JumpTo(Vector3 position)
    {
        Node node = gridSystem.NodeFromWorlPoint(position);
        if (dog.height == ObjectSize.Low && node.walkable && gridSystem.CheckIfPosIsNear(dog.gameObject, position))
        {
            dog.JumpTo(node);
            dog.height = ObjectSize.Ground;
        }
    }
}
