using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public float speed;
    public ObjectSize height = ObjectSize.Ground;
    public Vector3 direction;

    public int compteurConfusion = 3;
    public int nbOfConfusedTimes = 0;

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

    public void Push(HomeObject objToPush, Vector3 position)
    {
        objToPush.BeingPushed(this, position);
    }

    public bool Grab(HomeObject objToGrab)
    {
        return objToGrab.BeingGrabed();
    }

    public bool Release(HomeObject objToRelease, Vector3 position)
    {
        return objToRelease.BeingReleased(position);
    }

    public void Examine(HomeObject objToExamine)
    {
        objToExamine.BeingExamined(this);
    }

    public void CalculDirection(Node currentNode, Node nextNode)
    {
        direction = nextNode.worldPosition - currentNode.worldPosition;
    }

    public bool verifyLoosingConditions ()
    {
        return compteurConfusion <= 0;
    }

    public void BeConfused ()
    {
        compteurConfusion--;
        if ( verifyLoosingConditions() )
        {
            nbOfConfusedTimes++;
            // Activer Youki confus !
        }
    }

    IEnumerator CO_Move(List<Node> path)
    {
        for(int i = 0; i < path.Count; ++i)
        {
            Node node = path[i];
            if(i == path.Count - 2)
            {
                CalculDirection(node, path[i + 1]);
            }
            while((node.worldPosition - transform.position).sqrMagnitude != 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, node.worldPosition, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        yield return null;
    }

}
