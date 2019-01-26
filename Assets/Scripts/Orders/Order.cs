using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    // Attributes
    public OrderType type;
    public Object target;
    public Vector3 targetPos;
    
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
            this.targetPos = targetNode;
            target = null;
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
                if (target != null)
                {
                    // TODO Appeler la fonction pour examiner un Object
                }
                else
                {
                    Debug.Log("Le Object ciblé pour l'ordre Examine est null");
                }
                break;
            case OrderType.Jump:
                if (targetPos != null)
                {
                    if (target != null)
                    {
                        GameController.Instance.JumpTo(target.gameObject);
                    } else if (targetPos != null)
                    {
                        GameController.Instance.JumpTo(targetPos);
                    }
                    
                }
                else
                {
                    Debug.Log("Le Vector3 de la position ciblée pour l'ordre Jump est null");
                }
                break;
            case OrderType.Move:
                if (targetPos != null)
                {
                    GameController.Instance.MoveDog(targetPos);
                } else
                {
                    Debug.Log("Le Vector3 de la position ciblée pour l'ordre Move est null");
                }
                break;
            case OrderType.Push:
                if (target != null)
                {
                    // TODO Appeler la fonction pour pousser un Object
                }
                else
                {
                    Debug.Log("Le Object ciblé pour l'ordre Push est null");
                }
                break;
            case OrderType.Release:
                if (targetPos != null)
                {
                    // TODO Appeler la fonction pour release un Object
                }
                else
                {
                    Debug.Log("Le Vector3 de la position ciblée pour l'ordre Release est null");
                }
                break;
            case OrderType.Take:
                if (target != null)
                {
                    // TODO Appeler la fonction pour prendre un Object
                }
                else
                {
                    Debug.Log("Le Object ciblé pour l'ordre Take est null");
                }
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
