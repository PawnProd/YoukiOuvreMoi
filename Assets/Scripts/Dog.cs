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

    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void MoveTo(List<Node> path)
    {
        animator.SetBool("Moving", true);
        StartCoroutine(CO_Move(path));
    }

    public void JumpTo(Node node, bool up)
    {
        GetComponent<SpriteRenderer>().sortingOrder = node.objectOnNode.GetComponent<SpriteRenderer>().sortingOrder + ((up) ? 1 : -1);
        animator.SetBool("Jumping", true);
        
        StartCoroutine(CO_Jump(node));
    }

    public void Push(HomeObject objToPush, Vector3 position)
    {
        if ( !objToPush.BeingPushed(this, position))
        {
            BeConfused();
        }
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
        if ( !objToExamine.BeingExamined(this) )
        {
            BeConfused();
        }
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
        nbOfConfusedTimes++;
        GameController.Instance.ath.UpdateCompteurConfusion(nbOfConfusedTimes);
        if ( verifyLoosingConditions() )
        {
            GameController.Instance.EndOfGame(false);
            // Activer Youki confus !
        }
    }

    IEnumerator CO_Move(List<Node> path)
    {
        for(int i = 0; i < path.Count; ++i)
        {
            Node node = path[i];
            if(i < path.Count - 2)
            {
                CalculDirection(node, path[i + 1]);
                Debug.Log("Direction = " + direction);
                animator.SetFloat("DirX", direction.x);
                animator.SetFloat("DirY", direction.y);
            }
            while((node.worldPosition - transform.position).sqrMagnitude != 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, node.worldPosition, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        animator.SetBool("Moving", false);
       
        GameController.Instance.phase = Phase.SELECTACTION;
        GameController.Instance.ath.CleanOrder();
        yield return null;
    }

    IEnumerator CO_Jump(Node destination)
    {
        CalculDirection(GameController.Instance.gridSystem.NodeFromWorlPoint(transform.position), destination);
        animator.SetFloat("DirX", direction.x);
        animator.SetFloat("DirY", direction.y);
        while ((destination.worldPosition - transform.position).sqrMagnitude != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination.worldPosition, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        animator.SetBool("Jumping", false);
        GameController.Instance.phase = Phase.SELECTACTION;
        GameController.Instance.ath.CleanOrder();
        yield return null;
    }

}
