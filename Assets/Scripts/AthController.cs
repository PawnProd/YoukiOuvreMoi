﻿using System.Collections;
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

    public Transform cursorOverlay;

    public Transform victoryPanel;

    public Transform tipsPanel;

    public List<Sprite> actionsSprites;

    public TextMeshProUGUI compteurConfusion;

    private GameObject tipsGO;

    public void InitOrder(string actionName, int indexImgAction)
    {
        orderPanel.GetChild(0).GetComponent<Image>().enabled = true;
        orderPanel.GetChild(0).GetComponent<Image>().sprite = actionsSprites[indexImgAction];
        orderPanel.GetChild(1).GetComponent<TextMeshProUGUI>().text = actionName + " !";
        
    }

    public void CleanOrder()
    {
        RemoveTarget();
        orderPanel.GetChild(0).GetComponent<Image>().enabled = false;
        orderPanel.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
    }

    public void AddTargetSprite(Sprite targetSprite)
    {
        orderPanel.GetChild(orderPanel.childCount - 1).GetComponent<Image>().enabled = true;
        orderPanel.GetChild(orderPanel.childCount - 1).GetComponent<Image>().sprite = targetSprite;
    }

    public void RemoveTarget()
    {
        orderPanel.GetChild(orderPanel.childCount - 1).GetComponent<Image>().enabled = false;
    }

    public void EnableOrDisableApplyOrderButton(bool enable)
    {
        applyOrderButton.interactable = enable;
    }

    public void UpdateCompteurConfusion(int nbConfusion)
    {
        compteurConfusion.text = "x" + nbConfusion;
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
        objet.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    public void MoveCursorOverlay(Vector3 destination)
    {
        cursorOverlay.transform.position = destination;
    }

    public void RemoveObjetToInventory()
    {
        Destroy(inventoryContent.GetChild(0).gameObject);
    }

    public void victoryActivation (bool state)
    {
        victoryPanel.gameObject.SetActive(state);
    }

    public void LoadTips (string title, string content, GameObject go)
    {
        tipsPanel.Find("Title").GetComponent<TextMeshProUGUI>().text = title;
        tipsPanel.Find("Content").GetComponent<TextMeshProUGUI>().text = content;
        tipsPanel.gameObject.SetActive(true);
        tipsGO = go;
    }

    public void CloseTips ()
    {
        tipsPanel.gameObject.SetActive(false);
        Destroy(tipsGO);
    }
}
