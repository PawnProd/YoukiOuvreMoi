using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public float speed;

    public void MoveTo(List<Node> path)
    {
        StartCoroutine(CO_Move(path));
    }

    IEnumerator CO_Move(List<Node> path)
    {
        Debug.Log("Move ! ");
        foreach(Node node in path)
        {
            while((node.worldPosition - transform.position).sqrMagnitude > 0.01f)
            {
                Debug.Log("AH ! ");
                transform.position = Vector3.MoveTowards(transform.position, node.worldPosition, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        yield return null;
    }

}
