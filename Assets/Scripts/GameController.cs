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

    public GameObject inventory;
    public Transform objectLayer;

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
        targetMove = null;
        targetAction = null;
        gridSystem.PlaceObjectOnNodes(objectLayer);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && phase == Phase.SELECTTARGET)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
            if(hits.Length > 0)
            {
                if (hits.Length == 1)
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

        if (targetAction == null)
            order = CreateOrder(targetMove.worldPosition);
        else
            order = CreateOrder(targetAction.GetComponent<HomeObject>());

        order.ExecuteOrder();
        CleanOrder();
    }

    public void CleanOrder()
    {
        targetAction = null;
        targetMove = null;
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
            case "Sauter":
                type = OrderType.Jump;
                order = new Order(type, position);
                break;
            case "Relacher":
                type = OrderType.Release;
                order = new Order(type, position);
                break;
            default:
                type = OrderType.Move;
                order = new Order(type, position);
                break;

        }
        return order;
    
    }

    public Order CreateOrder(HomeObject obj)
    {
        OrderType type;
        Order order;
        switch (nextAction)
        {
            case "Sauter":
                type = OrderType.Jump;
                order = new Order(type, obj);
                break;
            case "Prendre":
                type = OrderType.Take;
                order = new Order(type, obj);
                break;
            case "Relacher":
                type = OrderType.Release;
                order = new Order(type, obj);
                break;
            case "Pousser":
                type = OrderType.Push;
                order = new Order(type, obj);
                break;
            case "Examiner":
                type = OrderType.Examine;
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
        if(target.GetComponent<HomeObject>() != null)
        {
            HomeObject targetObj = target.GetComponent<HomeObject>();
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
                dog.height = target.GetComponent<HomeObject>().size;
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

    public void PushObject(HomeObject obj)
    {
        dog.Push(obj);
    }

    public void GrabObject(HomeObject obj)
    {
        inventory = obj.gameObject;
        gridSystem.NodeFromWorlPoint(obj.transform.position).objectOnNode = null;
        ath.AddObjetToInventory(obj.GetComponent<SpriteRenderer>().sprite);
        dog.Grab(obj);
    }

    public void ReleaseObject(Vector3 targetPos)
    {   
        if(gridSystem.CheckIfPosIsNear(dog.gameObject, targetPos))
        {
            if (dog.height == inventory.GetComponent<HomeObject>().size && dog.height == ObjectSize.Ground)
            {
                if (gridSystem.NodeIsFree(gridSystem.NodeFromWorlPoint(targetPos)))
                {
                    dog.Release(inventory.GetComponent<HomeObject>(), targetPos);
                    gridSystem.NodeFromWorlPoint(targetPos).objectOnNode = inventory;
                    ath.RemoveObjetToInventory();
                    inventory = null;
                }
            }
            else if (dog.height == inventory.GetComponent<HomeObject>().size)
            {
                if (gridSystem.NodeFromWorlPoint(targetPos).objectOnNode.GetComponent<HomeObject>().onTopObject == null)
                {
                    dog.Release(inventory.GetComponent<HomeObject>(), targetPos);
                    gridSystem.NodeFromWorlPoint(targetPos).objectOnNode.GetComponent<HomeObject>().onTopObject = inventory.GetComponent<HomeObject>();
                    ath.RemoveObjetToInventory();
                    inventory = null;
                }
            }
        }
    }

    public void ExamineObject(HomeObject obj)
    {
        dog.Examine(obj);
    }
}

public enum Phase
{
    SELECTACTION,
    SELECTTARGET,
    APPLYORDER
}
