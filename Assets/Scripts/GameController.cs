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

    public AudioClip winSong;
    public AudioClip failedSong;
    public AudioSource jingleSource;
    public AudioSource confusedSource;
    public AudioSource giveOrderSource;

    private string nextAction;
    private Node target;



    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        phase = Phase.SELECTACTION;
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
                if(hits[0].collider.name == "Game")
                {
                    SelectTarget(gridSystem.NodeFromWorlPoint(hits[0].point));
                }
                else
                {
                    SelectTarget(hits[0].collider.GetComponent<HomeObject>().currentNode);
                }
            }
        }
        if (phase != Phase.APPLYORDER)
        {
            if (gridSystem.NodeFromWorlPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) != null)
            {
                ath.cursorOverlay.gameObject.SetActive(true);
                ath.MoveCursorOverlay(gridSystem.NodeFromWorlPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).worldPosition);
            }
            else
            {
                ath.cursorOverlay.gameObject.SetActive(false);
            }
                
        }
           
    }

    public void SelectAction(string actionName)
    {
        nextAction = actionName;
        int indexImgAction = 0;
        switch(actionName)
        {
            case "Move":
                indexImgAction = 0;
                break;
            case "Jump":
                indexImgAction = 1;
                break;
            case "Take":
                indexImgAction = 2;
                break;
            case "Drop":
                indexImgAction = 2;
                break;
            case "Push":
                indexImgAction = 3;
                break;
            case "Search":
                indexImgAction = 2;
                break;
            default:
                indexImgAction = 0;
                break;

        }
        ath.InitOrder(actionName, indexImgAction);
        phase = Phase.SELECTTARGET;
        ath.ChangeTodoText(phase);
    }

    public void SelectTarget(Node node)
    {
        target = node;
        if(node.objectOnNode == null)
        {
            Debug.Log("Pas d'objet");
            ath.AddTargetSprite(ath.GetSpriteOfTile(node.type));
        }
        else if(node.objectOnNode != null)
        {
            Debug.Log("objet");
            ath.AddTargetSprite(node.objectOnNode.GetComponent<SpriteRenderer>().sprite);
        }
        else if(node.objectOnNode.GetComponent<HomeObject>().onTopObject != null)
        {
            ath.AddTargetSprite(node.objectOnNode.GetComponent<HomeObject>().onTopObject.GetComponent<SpriteRenderer>().sprite);
        }
        
        phase = Phase.APPLYORDER;
        ath.ChangeTodoText(phase);
        ath.EnableOrDisableApplyOrderButton(true);
        ath.MoveCursorOverlay(node.worldPosition);
    }

    public void ApplyOrder()
    {
        Order order = CreateOrder(target.worldPosition);
        giveOrderSource.Play();
        order.ExecuteOrder();
        ath.EnableOrDisableApplyOrderButton(false);
    }

    public Order CreateOrder(Vector3 position)
    {
        OrderType type;
        Order order;
        switch(nextAction)
        {
            case "Move":
                type = OrderType.Move;
                order = new Order(type, position);
                return order;
            case "Jump":
                type = OrderType.Jump;
                order = new Order(type, position);
                return order;
            case "Take":
                type = OrderType.Take;
                Debug.Log(type);
                order = new Order(type, position);
                return order;
            case "Drop":
                type = OrderType.Release;
                order = new Order(type, position);
                return order;
            case "Push":
                type = OrderType.Push;
                order = new Order(type, position);
                return order;
            case "Search":
                type = OrderType.Examine;
                order = new Order(type, position);
                return order;
            default:
                type = OrderType.Move;
                order = new Order(type, position);
                return order;

        }
        
    
    }

    public void MoveDog(Vector3 position)
    {
        if(dog.height != ObjectSize.Ground)
        {
            if(gridSystem.NodeFromWorlPoint(position).objectOnNode != null)
            {
                if(dog.height == gridSystem.NodeFromWorlPoint(position).objectOnNode.GetComponent<HomeObject>().size)
                {
                    gridSystem.FindPath(dog.transform.position, position);
                    dog.MoveTo(gridSystem.GetGlobalPath());
                }
            }
        }
        else
        {
            if(gridSystem.NodeIsFree(gridSystem.NodeFromWorlPoint(position)))
            {
                gridSystem.FindPath(dog.transform.position, position);
                dog.MoveTo(gridSystem.GetGlobalPath());
            }
        }
        
    }

    public void JumpTo(Vector3 position)
    {
        Node node = gridSystem.NodeFromWorlPoint(position);
        GameObject target = node.objectOnNode;
        bool canJump = false;
        if (target != null)
        {
            Debug.Log("Coucou !");
            if (target.GetComponent<HomeObject>() != null)
            {
                Debug.Log(target.GetComponent<HomeObject>().name);
                if (target.GetComponent<HomeObject>().onTopObject == null)
                {
                    Debug.Log("Top Object = null");
                    HomeObject targetObj = target.GetComponent<HomeObject>();
                    if (dog.height == ObjectSize.Low)
                    {
                        if (targetObj.size == ObjectSize.High || targetObj.size == ObjectSize.Ground)
                        {
                            canJump = true;
                        }

                    }
                    else if (dog.height == ObjectSize.High || dog.height == ObjectSize.Ground)
                    {
                        if (targetObj.size == ObjectSize.Low)
                        {
                            canJump = true;
                        }
                    }
                }
            }
        }
        else if (dog.height == ObjectSize.Low && node.walkable && gridSystem.CheckIfPosIsNear(dog.gameObject, position))
        {
            dog.JumpTo(node, false);
            dog.height = ObjectSize.Ground;
            gridSystem.CleanUpdatedNode();
        }
        else
        {
            dog.BeConfused();
        }

        if (canJump)
        {
            if (gridSystem.CheckIfObjectIsNear(dog.gameObject, target))
            {
                if ((dog.height == ObjectSize.Ground && target.GetComponent<HomeObject>().size == ObjectSize.Low) || (dog.height == ObjectSize.Low && target.GetComponent<HomeObject>().size == ObjectSize.High))
                    dog.JumpTo(node, true);
                else
                    dog.JumpTo(node, false);

                dog.height = target.GetComponent<HomeObject>().size;
                
                gridSystem.CleanUpdatedNode();
                gridSystem.UpdateWalkableNode(dog.height, node);
            }
        }

    }

    public void PushObject(Vector3 position)
    {
        
        Vector3 targetPos = position + (position - dog.transform.position);
        Debug.Log(position - dog.transform.position);
        Debug.Log(targetPos);
        if (gridSystem.NodeIsFree(gridSystem.NodeFromWorlPoint(targetPos)) || (gridSystem.NodeFromWorlPoint(targetPos).objectOnNode.GetComponent<HomeObject>() == gridSystem.NodeFromWorlPoint(position).objectOnNode.GetComponent<HomeObject>().onTopObject ))
        {
            Debug.Log("Coucou");
            /* Ne pas décommenter ça pour le moment
            if (gridSystem.NodeFromWorlPoint(position).objectOnNode.GetComponent<HomeObject>().onTopObject != null)
            {
                gridSystem.NodeFromWorlPoint(position).objectOnNode.GetComponent<HomeObject>().onTopObject.size = ObjectSize.Ground;
                gridSystem.NodeFromWorlPoint(position).objectOnNode.GetComponent<HomeObject>().onTopObject = null;
            }*/

            
            dog.CalculDirection(gridSystem.NodeFromWorlPoint(dog.transform.position), gridSystem.NodeFromWorlPoint(position));
            dog.Push(gridSystem.NodeFromWorlPoint(position).objectOnNode.GetComponent<HomeObject>(), gridSystem.NodeFromWorlPoint(targetPos).worldPosition);

            if(gridSystem.NodeFromWorlPoint(targetPos).walkable)
            {
                gridSystem.SwitchWalkableNode(gridSystem.NodeFromWorlPoint(position), gridSystem.NodeFromWorlPoint(targetPos));
                gridSystem.NodeFromWorlPoint(targetPos).objectOnNode = gridSystem.NodeFromWorlPoint(position).objectOnNode;
                gridSystem.NodeFromWorlPoint(position).objectOnNode = null;
            }
            


        } else
        {
            dog.BeConfused();
        }
    }

    public void GrabObject(Vector3 position)
    {
        if ( inventory == null )
        {
            
            if (gridSystem.NodeFromWorlPoint(position).objectOnNode != null && dog.height == gridSystem.NodeFromWorlPoint(position).objectOnNode.GetComponent<HomeObject>().size)
            {
                HomeObject obj;
                if (gridSystem.NodeFromWorlPoint(position).objectOnNode.GetComponent<HomeObject>().onTopObject != null)
                {
                    Debug.Log("Test");
                    obj= gridSystem.NodeFromWorlPoint(position).objectOnNode.GetComponent<HomeObject>().onTopObject;
                    dog.CalculDirection(gridSystem.NodeFromWorlPoint(dog.transform.position), gridSystem.NodeFromWorlPoint(position));
                    if (dog.Grab(obj))
                    {
                        inventory = obj.gameObject;
                        gridSystem.NodeFromWorlPoint(obj.transform.position).objectOnNode.GetComponent<HomeObject>().onTopObject = null;
                        ath.AddObjetToInventory(obj.GetComponent<SpriteRenderer>().sprite);

                    }
                }
                else
                {
                    Debug.Log("Test 2 ");
                    obj = gridSystem.NodeFromWorlPoint(position).objectOnNode.GetComponent<HomeObject>();
                    dog.CalculDirection(gridSystem.NodeFromWorlPoint(dog.transform.position), gridSystem.NodeFromWorlPoint(position));
                    if (dog.Grab(obj))
                    {
                        Debug.Log("Grab 2 ");
                        inventory = obj.gameObject;
                        gridSystem.NodeFromWorlPoint(obj.transform.position).objectOnNode = null;
                        gridSystem.NodeFromWorlPoint(obj.transform.position).walkable = true;
                        ath.AddObjetToInventory(obj.GetComponent<SpriteRenderer>().sprite);

                    }
                }
            
            }
        } else
        {
            dog.BeConfused();
        }
    }

    public void ReleaseObject(Vector3 targetPos)
    {   
        if(gridSystem.CheckIfPosIsNear(dog.gameObject, targetPos))
        {
            if(dog.height == ObjectSize.Ground)
            {
                
                if (gridSystem.NodeIsFree(gridSystem.NodeFromWorlPoint(targetPos)))
                {
                    dog.CalculDirection(gridSystem.NodeFromWorlPoint(dog.transform.position), gridSystem.NodeFromWorlPoint(targetPos));
                    dog.Release(inventory.GetComponent<HomeObject>(), targetPos);
                    gridSystem.NodeFromWorlPoint(targetPos).objectOnNode = inventory;
                    gridSystem.NodeFromWorlPoint(targetPos).objectOnNode.GetComponent<HomeObject>().size = dog.height;
                    ath.RemoveObjetToInventory();
                    inventory = null;
                }
            }
            else if (dog.height != ObjectSize.Ground && gridSystem.NodeFromWorlPoint(targetPos).objectOnNode == null)
            {
                dog.CalculDirection(gridSystem.NodeFromWorlPoint(dog.transform.position), gridSystem.NodeFromWorlPoint(targetPos));
                dog.Release(inventory.GetComponent<HomeObject>(), targetPos);
                inventory.GetComponent<HomeObject>().size = ObjectSize.Ground;
                gridSystem.NodeFromWorlPoint(targetPos).objectOnNode = inventory;
                ath.RemoveObjetToInventory();
                inventory = null;
            }
            else if (dog.height == gridSystem.NodeFromWorlPoint(targetPos).objectOnNode.GetComponent<HomeObject>().size && dog.height == ObjectSize.Ground)
            {
                if (gridSystem.NodeIsFree(gridSystem.NodeFromWorlPoint(targetPos)))
                {
                    dog.CalculDirection(gridSystem.NodeFromWorlPoint(dog.transform.position), gridSystem.NodeFromWorlPoint(targetPos));
                    dog.Release(inventory.GetComponent<HomeObject>(), targetPos);
                    gridSystem.NodeFromWorlPoint(targetPos).objectOnNode = inventory;
                    gridSystem.NodeFromWorlPoint(targetPos).objectOnNode.GetComponent<HomeObject>().size = dog.height;
                    ath.RemoveObjetToInventory();
                    inventory = null;
                }
            }
            else if (dog.height == gridSystem.NodeFromWorlPoint(targetPos).objectOnNode.GetComponent<HomeObject>().size)
            {
                if (gridSystem.NodeFromWorlPoint(targetPos).objectOnNode.GetComponent<HomeObject>().onTopObject == null)
                {
                    dog.CalculDirection(gridSystem.NodeFromWorlPoint(dog.transform.position), gridSystem.NodeFromWorlPoint(targetPos));
                    dog.Release(inventory.GetComponent<HomeObject>(), targetPos);
                    gridSystem.NodeFromWorlPoint(targetPos).objectOnNode.GetComponent<HomeObject>().onTopObject = inventory.GetComponent<HomeObject>();
                    gridSystem.NodeFromWorlPoint(targetPos).objectOnNode.GetComponent<HomeObject>().onTopObject.GetComponent<HomeObject>().size = dog.height;
                    ath.RemoveObjetToInventory();
                    inventory = null;
                }
            }
           
        }
    }

    public void ExamineObject(Vector3 targetPos)
    {
        if(inventory == null)
        {
            Node node = gridSystem.NodeFromWorlPoint(targetPos);
            if(!gridSystem.NodeIsFree(node))
            {
                if(node.objectOnNode.GetComponent<HomeObject>().containedObject != null && node.objectOnNode.GetComponent<HomeObject>().examinable)
                {
                    dog.CalculDirection(gridSystem.NodeFromWorlPoint(dog.transform.position), gridSystem.NodeFromWorlPoint(targetPos));
                    ath.AddObjetToInventory(node.objectOnNode.GetComponent<HomeObject>().containedObject.GetComponent<SpriteRenderer>().sprite);
                    inventory = node.objectOnNode.GetComponent<HomeObject>().containedObject.gameObject;
                    dog.Examine(node.objectOnNode.GetComponent<HomeObject>());
                }
            }
            
        }
        
    }

    public void EndOfGame (bool victory)
    {
        ath.victoryActivation(true);
        if (victory)
        {
            ath.GetComponent<VictoryController>().Victory();
            jingleSource.clip = winSong;
            jingleSource.Play();
        } else
        {
            ath.GetComponent<VictoryController>().Defeat();
            jingleSource.clip = failedSong;
            jingleSource.Play();
        }
    }
}

public enum Phase
{
    SELECTACTION,
    SELECTTARGET,
    APPLYORDER
}
