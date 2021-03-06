﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPlacer1 : MonoBehaviour
{
    public GameObject wallMesh;
    public GameObject wall;
    public GameObject WariorQueue;
    public GameObject Trap;
    public bool isBuildEnabled;
    public bool isBuildableWall;
    public bool isBuildableWarior;
    public int wallPrice;
    public int wariorPrice;
    public int trapPrice;

    private Grid grid;
    private GameObject buildable;
    private GameObject buildableBox;
    private int price;
    private CurrencyManager CM;
    private KillCounter KC;

    // Use this for initialization
    void Start()
    {
        CM = GameObject.FindObjectOfType<CurrencyManager>();
        KC = GameObject.FindObjectOfType<KillCounter>();
    }

    public void enableBuild()
    {
        price = wallPrice;
        grid = FindObjectOfType<Grid>();
        buildable = new GameObject("breakableWall");
        //buildable.AddComponent<FracturedObject>();
        buildable = Instantiate(wallMesh);
        isBuildableWall = true;
        isBuildableWarior = false;
        buildable.transform.localScale = wall.transform.localScale;
        buildable.transform.rotation = wall.transform.rotation;
        buildableBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        buildableBox.GetComponent<Collider>().isTrigger = true;
        buildableBox.transform.localScale = buildable.GetComponent<Renderer>().bounds.size;

        //buildableBox.GetComponent<Material>() = 

        Material buildMaterial = Resources.Load("Materials/BuildMaterials/BuildAlowed", typeof(Material)) as Material;
        //buildableBox.AddComponent<MeshRenderer>();
        buildableBox.GetComponent<Renderer>().material = buildMaterial;
        buildableBox.tag = "buildableBox";

        Renderer rend = buildableBox.GetComponent<Renderer>();
        rend.enabled = true;
        isBuildEnabled = true;
    }

    public void disableBuild()
    {
        isBuildEnabled = false;
        isBuildableWall = false;
        isBuildableWarior = false;
        Destroy(buildable);
        Destroy(buildableBox);
    }

    public void enableWariorBuild()
    {
        price = wariorPrice;
        grid = FindObjectOfType<Grid>();
        buildable = new GameObject("Warior");
        buildable = Instantiate(WariorQueue);
        buildableBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        buildableBox.GetComponent<Collider>().isTrigger = true;
        buildableBox.transform.localScale = buildable.GetComponent<Collider>().bounds.size;

        Material buildMaterial = Resources.Load("Materials/BuildMaterials/BuildAlowed", typeof(Material)) as Material;
        //buildableBox.AddComponent<MeshRenderer>();
        buildableBox.GetComponent<Renderer>().material = buildMaterial;
        buildableBox.tag = "buildableBox";

        Renderer rend = buildableBox.GetComponent<Renderer>();
        rend.enabled = true;
        isBuildEnabled = true;
        isBuildableWall = false;
        isBuildableWarior = true;
    }

    public void enableTrapBuild()
    {
        price = trapPrice;
        grid = FindObjectOfType<Grid>();
        buildable = new GameObject("Trap");
        buildable = Instantiate(Trap);
        buildableBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        buildableBox.GetComponent<Collider>().isTrigger = true;
        buildableBox.transform.localScale = buildable.transform.GetChild(0).GetComponent<Collider>().bounds.size;

        Material buildMaterial = Resources.Load("Materials/BuildMaterials/BuildAlowed", typeof(Material)) as Material;
        buildableBox.GetComponent<Renderer>().material = buildMaterial;
        buildableBox.tag = "buildableBox";

        Renderer rend = buildableBox.GetComponent<Renderer>();
        rend.enabled = true;
        isBuildEnabled = true;
        isBuildableWall = false;
        isBuildableWarior = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuildEnabled)
        {
            Ray pendingBuildSpot = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit pendingInfo;
            Physics.Raycast(pendingBuildSpot, out pendingInfo);

            Vector3 finalPendingBuildPosition = pendingInfo.point;
            finalPendingBuildPosition = grid.GetNearestPointOnGrid(finalPendingBuildPosition);
            finalPendingBuildPosition.y = 0;
            buildable.transform.position = finalPendingBuildPosition;
            buildableBox.transform.position = finalPendingBuildPosition + new Vector3(0, 0.7f, 0);
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hitInfo))
                {
                    placeBuildingNear(hitInfo.point);
                }
            }

            if (Input.GetKeyDown("r"))
            {
                buildable.transform.eulerAngles = new Vector3(buildable.transform.eulerAngles.x, buildable.transform.eulerAngles.y, buildable.transform.eulerAngles.z + 90);

                buildableBox.transform.position = buildable.transform.position + new Vector3(0, 0.8f, 0);
                buildableBox.transform.localScale = buildable.GetComponent<Renderer>().bounds.size;
            }
        }

        if (Input.GetKeyDown("c"))
        {
            if (!isBuildEnabled)
            {
                enableBuild();
            }
            else
            {
                disableBuild();
            }
        }

        if (Input.GetKeyDown("v"))
        {
            if (!isBuildEnabled)
            {
                enableWariorBuild();
            }
            else
            {
                disableBuild();
            }
        }

        if (Input.GetKeyDown("t"))
        {
            if (!isBuildEnabled)
            {
                enableTrapBuild();
            }
            else
            {
                disableBuild();
            }
        }
    }

    private void placeBuildingNear(Vector3 clickPoint)
    {
        if (CM.BuyFor(price))
        {
            if (buildableBox.GetComponent<Renderer>().material.color.r == 0.2512111f)
            {
                var finalPosition = buildable.transform.position;
                var finalRotation = buildable.transform.rotation;
                GameObject buildObject = new GameObject();
                if (isBuildableWall)
                {
                    buildObject = Instantiate(wall);
                }
                else
                {
                    buildObject = Instantiate(buildable);
                    if (isBuildableWarior)
                    {
                        KC.AddDef();
                    }
                }
                buildObject.transform.position = finalPosition;
                buildObject.transform.rotation = finalRotation;
                //GameObject wallInstance = Instantiate(wallToBuild, finalPosition, new Quaternion(-90, 0, 0, 90));
                //wallToBuild.AddComponent<Rigidbody>();
                //wallToBuild.AddComponent<BoxCollider>();
            }
        }
        //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = finalPosition;
    }
}
