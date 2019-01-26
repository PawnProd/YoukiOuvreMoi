using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public float speed;
    public ObjectSize height = ObjectSize.Ground;

    public void MoveTo(List<Node> path)
    {
        StartCoroutine(CO_Move(path));
    }

    public void JumpTo(Node node)
    {
        List<Node> nodes = new List<Node>();
        nodes.Add(node);

        StartCoroutine(CO_Move(nodes));
    }

    public void Push(Object objToPush)
    {
        objToPush.BeingPushed(this, Vector3.forward);
    }

    public void Grab(Object objToGrab)
    {
        objToGrab.BeingGrabed(this);
    }

    public void Release(Object objToRelease)
    {
        objToRelease.BeingReleased(this);
    }

    public void Examine(Object objToExamine)
    {
        objToExamine.BeingExamined(this);
    }

    IEnumerator CO_Move(List<Node> path)
    {
        foreach(Node node in path)
        {
            while((node.worldPosition - transform.position).sqrMagnitude != 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, node.worldPosition, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        yield return null;
    }

}
