using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    // Attributes
    public OrderType type;
    public Object target;
    
    public Order (OrderType type, Object target)
    {
        this.type = type;
        this.target = target;
    }

    public void ExecuteOrder ()
    {

    }

}
