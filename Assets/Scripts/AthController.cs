using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AthController : MonoBehaviour
{
    public Transform orderPanel;
    public TextMeshProUGUI todoText;

    public Sprite floorTile;

    public Button applyOrderButton;

    public Transform inventoryContent;

    public void InitOrder(string actionName)
    {
        orderPanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = actionName;
        
    }

    public void AddTargetSprite(Sprite targetSprite)
    {
        orderPanel.GetChild(2).GetComponent<Image>().sprite = targetSprite;
    }

    public void EnableOrDisableApplyOrderButton(bool enable)
    {
        applyOrderButton.interactable = enable;
    }

    public Sprite GetSpriteOfTile(TileType type)
    {
        switch(type)
        {
            case TileType.FLOOR:
                return floorTile;

            default:
                return floorTile;
        }
    }

    public void ChangeTodoText(Phase phase)
    {
        switch(phase)
        {
            case Phase.SELECTACTION:
                todoText.text = "Sélectionner une action";
                break;
            case Phase.SELECTTARGET:
                todoText.text = "Sélectionner une cible";
                break;
            case Phase.APPLYORDER:
                todoText.text = "Yuki ! Go !";
                break;
        }
    }

    public void AddObjetToInventory(Sprite sprite)
    {
        GameObject objet = new GameObject("Objet");
        objet.AddComponent<Image>().sprite = sprite;
        objet.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        objet.transform.parent = inventoryContent;
    }

    public void RemoveObjetToInventory()
    {
        Destroy(inventoryContent.GetChild(0).gameObject);
    }




}
