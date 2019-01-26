using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { private set; get; }
    public AthController ath;
    public GridSystem gridSystem;
    public Dog dog;
    public Phase phase;
    private string nextAction;
    private Node targetMove;
    private GameObject targetAction;


    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        phase = Phase.SELECTACTION;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && phase == Phase.SELECTTARGET)
        {
            Debug.Log("CLick !");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
            if(hits.Length > 0)
            {
                if (hits.Length == 1 && nextAction == "Déplacer")
                {
                    
                    SelectTargetForMove(gridSystem.NodeFromWorlPoint(hits[0].point));
                }
                else
                {
                    int i = 0;
                    bool find = false;
                    while(!find & i < hits.Length)
                    {
                        if (hits[i].collider.name != "Game")
                        {
                            find = true;
                            SelectTargetForAction(hits[i].collider.gameObject);
                        }

                        ++i;
                    }
                }
            }
        }
    }

    public void SelectAction(string actionName)
    {
        nextAction = actionName;
        ath.InitOrder(actionName);
        phase = Phase.SELECTTARGET;
        ath.ChangeTodoText(phase);
    }

    public void SelectTargetForMove(Node node)
    {
        targetMove = node;
        ath.AddTargetSprite(ath.GetSpriteOfTile(node.type));
        phase = Phase.APPLYORDER;
        ath.ChangeTodoText(phase);
        ath.EnableOrDisableApplyOrderButton(true);
    }

    public void SelectTargetForAction(GameObject target)
    {
        targetAction = target;
        ath.AddTargetSprite(target.GetComponent<SpriteRenderer>().sprite);
        phase = Phase.APPLYORDER;
        ath.ChangeTodoText(phase);
        ath.EnableOrDisableApplyOrderButton(true);
    }

    public void ApplyOrder()
    {
        Order order;

        if (nextAction == "Déplacer")
            order = CreateOrder(targetMove.worldPosition);
        else
            order = CreateOrder(targetAction.GetComponent<Object>());

        order.ExecuteOrder();
    }

    public Order CreateOrder(Vector3 position)
    {
        OrderType type;
        Order order;
        switch(nextAction)
        {
            case "Déplacer":
                type = OrderType.Move;
                order = new Order(type, position);
                break;
            default:
                type = OrderType.Move;
                order = new Order(type, position);
                break;

        }
        return order;
    
    }

    public Order CreateOrder(Object obj)
    {
        OrderType type;
        Order order;
        switch (nextAction)
        {
            case "Sauter":
                type = OrderType.Jump;
                order = new Order(type, obj);
                break;
            default:
                type = OrderType.Jump;
                order = new Order(type, obj);
                break;

        }
        return order;

    }

    public void MoveDog(GameObject target)
    {
        gridSystem.FindPath(dog.transform.position, target.transform.position);
        dog.MoveTo(gridSystem.GetGlobalPath());
    }

    public void MoveDog(Vector3 position)
    {
        gridSystem.FindPath(dog.transform.position, position);
        dog.MoveTo(gridSystem.GetGlobalPath());
    }

    public void JumpTo(GameObject target)
    {
        Debug.Log("Jump !");
        bool canJump = false;
        if(target.GetComponent<Object>() != null)
        {
            Object targetObj = target.GetComponent<Object>();
            if(dog.height == ObjectSize.Low)
            {
                if(targetObj.size == ObjectSize.High || targetObj.size == ObjectSize.Ground)
                {
                    canJump = true;
                }
                    
            }
            else if(dog.height == ObjectSize.High || dog.height == ObjectSize.Ground)
            {
                if (targetObj.size == ObjectSize.Low)
                {
                    canJump = true;
                }
            }

        }

        if (canJump)
        {
            Node node = gridSystem.NodeFromWorlPoint(target.transform.position);
            if (gridSystem.CheckIfObjectIsNear(dog.gameObject, target))
            {
                dog.height = target.GetComponent<Object>().size;
                dog.JumpTo(node);
            }
        }
    }

    public void JumpTo(Vector3 position)
    {
        Debug.Log("Jump position !");
        Node node = gridSystem.NodeFromWorlPoint(position);
        if (dog.height == ObjectSize.Low && node.walkable && gridSystem.CheckIfPosIsNear(dog.gameObject, position))
        {
            dog.JumpTo(node);
            dog.height = ObjectSize.Ground;
        }
    }
}

public enum Phase
{
    SELECTACTION,
    SELECTTARGET,
    APPLYORDER
}
