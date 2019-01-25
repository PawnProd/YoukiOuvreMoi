using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public OrderType type;

    public GameObject target;
    
    public Order (OrderType type, GameObject target)
    {
        this.type = type;
        this.target = target;
    }

    

}
