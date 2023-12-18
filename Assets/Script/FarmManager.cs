using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmManager : MonoBehaviour
{
    public PlantItem selectPlant;
    public bool isPlanting = false;
    public int money = 100;
    public Text moneyTxt;

    public int point = 0;
    public Text pointTxt;

    public int boughtPrice = 100;
    public Text boughtPriceTxt;

    public Color buyColor = Color.green;
    public Color cancelColor = Color.red;

    public bool isSelecting = false;
    public int selectedTool = 0;
    // 1- Water 2- Scythe 3- Fertilizing 4- Buy Plot

    public Image[] buttonsImg;
    public Sprite normalButton;
    public Sprite selectedButton;

    // Start is called before the first frame update
    void Start()
    {
        moneyTxt.text = "$ " + money;
        pointTxt.text = "Score : " + point;
        boughtPriceTxt.text = "$ " + boughtPrice;
    }

    public void SelectPlant(PlantItem newPlant)
    {
        if (selectPlant == newPlant) 
        {
            CheckSelection();
        }
        else
        {
            CheckSelection();
            selectPlant = newPlant;
            selectPlant.btnImage.color = cancelColor;
            selectPlant.btnTxt.text = "Cancel";
            // Debug.Log("Selected " + selectPlant.plant.plantName);
            isPlanting = true;
        }
    }

    public void SelectTool(int toolNumber)
    {
        if(toolNumber == selectedTool) 
        {
            // deselect
            CheckSelection();
        }
        else
        {
            // select tool number and check to see if anything was also selected
            CheckSelection();
            isSelecting = true;
            selectedTool = toolNumber;
            buttonsImg[toolNumber - 1].sprite = selectedButton;
        }
    }

    void CheckSelection()
    {
        if (isPlanting)
        {
            isPlanting = false;
            if (selectPlant != null)
            {
                selectPlant.btnImage.color = buyColor;
                selectPlant.btnTxt.text = "Buy";
                selectPlant = null;
            }
        }
        if (isSelecting)
        {
            if (selectedTool > 0)
            {
                buttonsImg[selectedTool - 1].sprite = normalButton;
            }
            isSelecting = false;
            selectedTool = 0;
        }
    }

    public void Transaction(int value)
    {
        money += value;
        moneyTxt.text = "$ " + money;
    }

    public void GainPoint(int value)
    {
        point += value;
        pointTxt.text = "Score : " + point;
    }

    public void PriceUp(int value)
    {
        boughtPrice += value;
        boughtPriceTxt.text = "$ " + boughtPrice;
    }
}
