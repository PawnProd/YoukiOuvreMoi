using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    // Attributes
    public OrderType type;
    public Object target;
    public Node targetNode;
    
    public Order (OrderType type, Object target)
    {
        this.type = type;
        this.target = target;
    }

    public Order(OrderType type, Node targetNode)
    {
        if (type == OrderType.Move)
        {
            this.type = type;
            this.targetNode = targetNode;
        }
        
    }

    public void ExecuteOrder ()
    {

    }

}
