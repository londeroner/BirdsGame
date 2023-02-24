using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public UIManager()
    {
        if (instance != null)
            throw new Exception("More than 1 instance");

        instance = this;
    }

    public TextMeshProUGUI activeButton;
    public TextMeshProUGUI CollectedResources;
    public TextMeshProUGUI LevelTimer;

    public GameObject attackButton;
    public GameObject defenceButton;
    public GameObject collectButton;

    public Sprite appleSprite;
    public Sprite emptySprite;
    public Sprite noResourceSprite;
    public List<GameObject> inventoryItems = new List<GameObject>();
    public List<GameObject> inventoryBoxes = new List<GameObject>();

    public void Start()
    {
        RedrawInventory();
    }

    public void PlayerChangeFormation(FormationType type)
    {
        var attackButtonColor = attackButton.GetComponent<Image>();
        var defenceButtonColor = defenceButton.GetComponent<Image>();
        var collectButtonColor = collectButton.GetComponent<Image>();

        attackButtonColor.color = Color.white;
        defenceButtonColor.color = Color.white;
        collectButtonColor.color = Color.white;

        switch (type)
        {
            case FormationType.AttackFormation:
                attackButtonColor.color = Color.red;
                break;
            case FormationType.DefenceFormation:
                defenceButtonColor.color = Color.red;
                break;
            case FormationType.CollectFormation:
                collectButtonColor.color = Color.red;
                break;
        }
    }

    public void ChangeActiveButton(float time)
    {
        if (time > 0f)
            activeButton.text = string.Format("{0:#.##}", time);
        else if (time == 0) activeButton.text = "Active ready";
        else activeButton.text = "Active used";
    }

    public void RedrawInventory()
    {
        var collectibles = PlayerManager.instance.birds.collectedResources;
        var maxSize = PlayerManager.instance.birds.maxResourceCount;
        foreach (var go in inventoryBoxes) go.SetActive(false);
        for (int i = 0; i < maxSize; i++)
        {
            inventoryBoxes[i].SetActive(true);
            if (collectibles.Count > i)
            {
                var collectible = collectibles[i];
                if (collectible is not null)
                {
                    switch (collectible.Type)
                    {
                        case CollectibleResource.Apple:
                            inventoryItems[i].GetComponentInChildren<Image>().sprite = appleSprite;
                            break;
                        case CollectibleResource.Coin:
                            inventoryItems[i].GetComponentInChildren<Image>().sprite = noResourceSprite;
                            break;
                        case CollectibleResource.Cap:
                            inventoryItems[i].GetComponentInChildren<Image>().sprite = noResourceSprite;
                            break;
                        case CollectibleResource.Feather:
                            inventoryItems[i].GetComponentInChildren<Image>().sprite = noResourceSprite;
                            break;
                        case CollectibleResource.Blueberry:
                            inventoryItems[i].GetComponentInChildren<Image>().sprite = noResourceSprite;
                            break;
                        case CollectibleResource.Cranberry:
                            inventoryItems[i].GetComponentInChildren<Image>().sprite = noResourceSprite;
                            break;
                    }
                }
                else inventoryItems[i].GetComponentInChildren<Image>().sprite = emptySprite;
            }
            else inventoryItems[i].GetComponentInChildren<Image>().sprite = emptySprite;
        }
    }

    public void ToggleCollectedResources(bool state)
    {
        CollectedResources.enabled = state;
    }

    public void UpdateCollectedResources()
    {
        CollectedResources.text = $"Еда: {PlayerManager.instance.LevelCollectedFood}\n" +
        $"Монеты: {PlayerManager.instance.LevelCollectedCoins}\n" +
        $"Крышки: {PlayerManager.instance.LevelCollectedCaps}\n" +
        $"Перья: {PlayerManager.instance.LevelCollectedFeathers}";
    }

    public void UpdateLevelTimer(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);

        LevelTimer.text = time.ToString(@"mm\:ss");
    }
}
