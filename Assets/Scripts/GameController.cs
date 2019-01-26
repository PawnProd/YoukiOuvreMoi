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
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if(hit && hit.collider.GetComponent<Object>())
            {
                if(hit.collider.GetComponent<Object>().size == ObjectSize.Low)
                {
                    dog.JumpTo(gridSystem.NodeFromWorlPoint(hit.point));
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
}
