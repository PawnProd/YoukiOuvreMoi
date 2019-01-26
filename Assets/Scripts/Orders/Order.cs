using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    // Attributes
    public OrderType type;
    public Object target;
    public Vector3 targetNode;
    
    public Order (OrderType type, Object target)
    {
        this.type = type;
        this.target = target;
    }

    public Order(OrderType type, Vector3 targetNode)
    {
        if (type == OrderType.Move || type == OrderType.Release || type == OrderType.Jump)
        {
            this.type = type;
            this.targetNode = targetNode;
        }
        
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
        switch (type)
        {
            case OrderType.Examine:

                break;
            case OrderType.Jump:

                break;
            case OrderType.Move:

                break;
            case OrderType.Push:

                break;
            case OrderType.Release:

                break;
            case OrderType.Take:

                break;
            case OrderType.Youki:

                break;
            default:
                Debug.Log("On est pas sensé passer là!");
                break;
        }
    }

}
