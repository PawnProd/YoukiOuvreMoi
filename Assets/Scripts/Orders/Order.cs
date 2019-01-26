using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    // Attributes
    public OrderType type;
    public HomeObject target;
    public Vector3 targetPos;
    
    public Order (OrderType type, HomeObject target)
    {
        this.type = type;
        this.target = target;
    }

    public Order(OrderType type, Vector3 targetNode)
    {
        this.type = type;
        this.targetPos = targetNode;
        target = null;    
    }

    public Order (OrderType type)
    {
        if (type == OrderType.Youki)
        {
            this.type = type;
        }
    }

    public void ExecuteOrder ()
    {
        Debug.Log("Order = " + type);
        switch (type)
        {
            case OrderType.Examine:
                GameController.Instance.ExamineObject(targetPos);
                break;
            case OrderType.Jump:
                GameController.Instance.JumpTo(targetPos);
                break;
            case OrderType.Move:
                GameController.Instance.MoveDog(targetPos);
                break;
            case OrderType.Push:
                GameController.Instance.PushObject(targetPos);
                break;
            case OrderType.Release:
                GameController.Instance.ReleaseObject(targetPos);
                break;
            case OrderType.Take:
                GameController.Instance.GrabObject(targetPos);
                break;
            case OrderType.Youki:
                // TODO Appeler la fonction pour récupérer l'attention de Youki
                break;
            default:
                Debug.Log("On est pas sensé passer là pour un ordre!");
                break;
        }
    }

}
