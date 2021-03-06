﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallButton : MonoBehaviour{

    WallPlacer1 WP;
    CurrencyManager CM;
    public int price;

    // Use this for initialization
    void Start()
    {
        WP = FindObjectOfType<WallPlacer1>();
        CM = FindObjectOfType<CurrencyManager>();
        price = WP.wallPrice;
    }

    private void Update()
    {
        if(CM.GetCurrency() < price)
        {
            gameObject.GetComponent<Image>().color = Color.red;
        }
        else if (!WP.isBuildEnabled)
        {
            gameObject.GetComponent<Image>().color = Color.white;
            WP.disableBuild();
        }
    }

    public void enableBuild()
    {
        if (CM.GetCurrency() >= price)
        {
            if (!WP.isBuildEnabled)
            {
                WP.enableBuild();
                gameObject.GetComponent<Image>().color = Color.green;
            }
            else
            {
                gameObject.GetComponent<Image>().color = Color.white;
                WP.disableBuild();
            }
        }
    }
}

