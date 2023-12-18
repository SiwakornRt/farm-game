using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlotManager : MonoBehaviour
{

    bool isPlanted = false;
    SpriteRenderer plant;
    BoxCollider2D plantCollider;

    int plantStage = 0;
    float timer;

    public Color availableColor = Color.green;
    public Color unavailableColor = Color.red;

    SpriteRenderer plot;

    PlantObject selectedPlant;

    FarmManager fm;

    bool isDry = true;
    public Sprite drySprite;
    public Sprite normalSprite;
    public Sprite unavailableSprite;

    float speed = 1f;
    public bool isBought = true;

    // Start is called before the first frame update
    void Start()
    {
        plant = transform.GetChild(0).GetComponent<SpriteRenderer>();   
        plantCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        fm = transform.parent.GetComponent<FarmManager>();
        plot = GetComponent<SpriteRenderer>();
        if (isBought ) 
        {
            plot.sprite = drySprite;
        }
        else
        {
            plot.sprite = unavailableSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlanted && !isDry) 
        {
            timer -= speed * Time.deltaTime;    

            if (timer < 0 && plantStage < selectedPlant.plantStages.Length - 1)
            {
                timer = selectedPlant.timeBtwStages;
                plantStage++;
                UpdatePlant();
            }
        }
    }

    private void OnMouseDown()
    {
        if (isPlanted)
        {
            if (plantStage == selectedPlant.plantStages.Length - 1 && !fm.isPlanting && !fm.isSelecting)
            {
                Harvest();
            }
        }
        else if (fm.isPlanting && fm.selectPlant.plant.buyPrice <= fm.money && isBought)
        {
            Plant(fm.selectPlant.plant); 
        }
        if(fm.isSelecting)
        {
            switch (fm.selectedTool)
            {
                // Water Button
                case 1:
                    if (isBought)
                    {
                        isDry = false;
                        plot.sprite = normalSprite;
                        if (isPlanted) UpdatePlant();
                    }
                    break;
                
                // Scythe Button
                case 2:

                // Fertilizing Button
                case 3:
                    if (fm.money >= 10 && isBought && plantStage != selectedPlant.plantStages.Length - 1)
                    {
                        fm.Transaction(-10);
                        if (speed < 2) speed += .2f;
                    }
                    break;

                // Buy Plot Button
                case 4:
                    if (fm.money > fm.boughtPrice && !isBought) 
                    {
                        fm.Transaction(-fm.boughtPrice);
                        fm.PriceUp(100);
                        isBought = true;
                        plot.sprite = drySprite;
                    }
                    break;

                default:
                    break;
            }
        }
    }

    private void OnMouseOver()
    {
        if (fm.isPlanting)
        {
            if (isPlanted || fm.selectPlant.plant.buyPrice > fm.money || !isBought)
            {
                // Can't buy
                plot.color = unavailableColor;
            }
            else
            {
                // Can buy
                plot.color = availableColor;
            }
        }

        if (fm.isSelecting)
        {
            switch (fm.selectedTool)
            {
                case 1:
                    if (!isBought)
                    {
                        plot.color = unavailableColor;
                    }
                    else
                    {
                        plot.color = availableColor;
                    }
                    break;   
                case 2:
                    if (isPlanted)
                    {
                        if (plantStage == selectedPlant.plantStages.Length - 1 && !fm.isPlanting)
                        {
                            Harvest();
                        }
                    }
                    break;
                case 3:
                    if (isBought && plantStage == selectedPlant.plantStages.Length - 1)
                    {
                        plot.color = unavailableColor;
                    }
                    else if (isBought && fm.money >= (fm.selectedTool - 1) * 10)
                    {
                        plot.color = availableColor;
                    }
                    else
                    {
                        plot.color = unavailableColor;
                    }
                    break;
                case 4:
                    if (!isBought && fm.money > fm.boughtPrice)
                    {
                        plot.color = availableColor;
                    }
                    else
                    {
                        plot.color= unavailableColor;
                    }
                    break;

                default:
                    plot.color = unavailableColor;
                    break;
            }
        }
    }

    private void OnMouseExit()
    {
        plot.color = Color.white;
    }

    void Harvest()
    {
        isPlanted = false;
        plant.gameObject.SetActive(false);
        fm.Transaction(selectedPlant.sellPrice);
        fm.GainPoint(selectedPlant.point);
        isDry = true;
        plot.sprite = drySprite;
        speed = 1f;
    }

    void Plant(PlantObject newPlant)
    {
        selectedPlant = newPlant;
        isPlanted = true;

        fm.Transaction(-selectedPlant.buyPrice);

        plantStage = 0;
        UpdatePlant();
        timer = selectedPlant.timeBtwStages;
        plant.gameObject.SetActive(true);

    }

    void UpdatePlant()
    {
        if (isDry)
        {
            plant.sprite = selectedPlant.dryPlanted;
        }
        else
        {
            plant.sprite = selectedPlant.plantStages[plantStage];
        }
        plantCollider.size = plant.sprite.bounds.size;
        plantCollider.offset = new Vector2(0, plant.bounds.size.y / 2);
    }

}
