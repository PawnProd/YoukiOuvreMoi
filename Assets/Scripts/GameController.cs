using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GridSystem gridSystem;
    public Dog dog;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click !");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            Debug.Log("Hit something ! " + hit.point);
            if(hit)
            {
                gridSystem.FindPath(dog.transform.position, hit.point);
                dog.MoveTo(gridSystem.GetGlobalPath());
            }
        }
    }
}
