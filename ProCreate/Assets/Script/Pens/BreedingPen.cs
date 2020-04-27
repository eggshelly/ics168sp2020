﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedingPen : BasicPen
{


    #region Built In Function

    protected override void Start()
    {
        base.Start();
        this.capacity = 2;
    }

    protected override void Update()
    {
        base.Update();

        if(CurrentCapacity == this.capacity)
        {
            BreedAnimals();
        }
    }

    #endregion


    #region Changing the Animals in the Pen

    #endregion

    #region Breeding Functions

    void BreedAnimals()
    {
        GameObject parent1 = AnimalsInPen[0];
        GameObject parent2 = AnimalsInPen[1];

        if(parent1.GetComponent<AnimalStatistics>().IsWillingToBreed() && parent2.GetComponent<AnimalStatistics>().IsWillingToBreed())
        {
            SpawnChild(parent1, parent2);
            parent1.GetComponent<AnimalStatistics>().FinishedBreeding();
            parent2.GetComponent<AnimalStatistics>().FinishedBreeding();
        }
    }

    void SpawnChild(GameObject parent1, GameObject parent2)
    {
        Material main = (Random.Range(0f, 1f) < 0.5f ? parent1.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().sharedMaterials[0] :
            parent2.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().sharedMaterials[0]);
        Material second = (Random.Range(0f, 1f) < 0.5f ? parent1.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().sharedMaterials[1] :
            parent2.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().sharedMaterials[1]);

        CreateChildObject(main, second, parent1, parent2);

    }

    void CreateChildObject(Material main, Material second, GameObject parent1, GameObject parent2)
    {
        GameObject Child = new GameObject();
        AnimalStatistics stats = Child.AddComponent<AnimalStatistics>();
        AnimalMovement move = Child.AddComponent<AnimalMovement>();
        BoxCollider coll = Child.AddComponent<BoxCollider>();

        PassInNewGeneralStats(ref stats, parent1.GetComponent<AnimalStatistics>(), parent2.GetComponent<AnimalStatistics>());
        PassInNewCollider(ref coll, (Random.value < 0.5f ? parent1.GetComponent<BoxCollider>() : parent2.GetComponent<BoxCollider>()));
        PassInNewMoveVars(ref move, (Random.value < 0.5f ? parent1.GetComponent<AnimalMovement>() : parent2.GetComponent<AnimalMovement>()));

        GameObject[] BodyParts = new GameObject[4];
        for(int i = 0; i < 4; ++i)
        {
            BodyParts[i] = (Random.value < 0.5f ? parent1.transform.GetChild(i).gameObject : parent2.transform.GetChild(i).gameObject);
        }


        GameObject Body = Instantiate(BodyParts[0]);
        Body.transform.parent = Child.transform;

        GameObject NewPart;
        MeshRenderer TempMeshRend;

        for(int i = 1; i < 4; ++i)
        {
            NewPart = Instantiate(BodyParts[i]);
            NewPart.transform.parent = Child.transform;
            NewPart.transform.position = Body.transform.GetChild(i - 1).position;

            if(i == 2)
            {
                for(int j = 0; j < NewPart.transform.childCount; ++j)
                {
                    TempMeshRend = NewPart.transform.GetChild(j).GetComponent<MeshRenderer>();
                    ApplyMaterial(main, second, ref TempMeshRend);
                }

            }
            else
            {
                TempMeshRend = NewPart.GetComponent<MeshRenderer>();
                ApplyMaterial(main, second, ref TempMeshRend);
            }
        }


        Child.transform.position = (parent1.transform.position + parent2.transform.position) / 2f;
    }

    void ApplyMaterial(Material main, Material second, ref MeshRenderer mesh)
    {
        Material[] TempMats = mesh.sharedMaterials;
        TempMats[0] = main;
        if(TempMats.Length == 2)
        {
            TempMats[1] = second;
        }
        mesh.materials = TempMats;
    }

    void PassInNewGeneralStats(ref AnimalStatistics childStats, AnimalStatistics parent1, AnimalStatistics parent2)
    {
        float StartWillPer = (Random.value < 0.5f ? parent1.GetStartingWTB() : parent2.GetStartingWTB());
        float MaxWillBreed = (Random.value < 0.5f ? parent1.GetMaxWTB() : parent2.GetMaxWTB());
        float ReqWillBreed = (Random.value < 0.5f ? parent1.GetReqWTBPercent() : parent2. GetReqWTBPercent());
        float WillChangeAm = (Random.value < 0.5f ? parent1.GetWillChangeAm() : parent2.GetWillChangeAm());
        float WillChangeTimer = (Random.value < 0.5f ? parent1.GetWillChangeTimer() : parent2.GetWillChangeTimer());
        float PostBreedPer = (Random.value < 0.5f ? parent1.GetPostBreedPercent() : parent2.GetPostBreedPercent());
        float StartHungPer = (Random.value < 0.5f ? parent1.GetStartHungPer() : parent2.GetStartHungPer());
        float StartThirstPer = (Random.value < 0.5f ? parent1.GetStartThirstPer() : parent2.GetStartThirstPer());
        float MaxFood = (Random.value < 0.5f ? parent1.GetMaxFood() : parent2.GetMaxFood());
        float MaxWater = (Random.value < 0.5f ? parent1.GetMaxWater() : parent2.GetMaxWater());
        float FoodDec = (Random.value < 0.5f ? parent1.GetFoodDec() : parent2.GetFoodDec());
        float WaterDec = (Random.value < 0.5f ? parent1.GetWaterDec() : parent2.GetWaterDec());
        float FoodDecTimer = (Random.value < 0.5f ? parent1.GetFoodDecTimer() : parent2.GetFoodDecTimer());
        float WaterDecTimer = (Random.value < 0.5f ? parent1.GetWaterDecTimer() : parent2.GetWaterDecTimer());

        childStats.SetAnimalCanvas(AnimalManager.instance.GetCanvasPrefab());

        childStats.IsNewChild(StartWillPer, MaxWillBreed, ReqWillBreed, WillChangeAm, WillChangeTimer, PostBreedPer, StartHungPer, StartThirstPer,
            MaxFood, MaxWater, FoodDec, WaterDec, FoodDecTimer, WaterDecTimer);
    }

    void PassInNewCollider(ref BoxCollider coll, BoxCollider parent)
    {
        coll.center = parent.center;
        coll.size = parent.size;
    }

    void PassInNewMoveVars(ref AnimalMovement move, AnimalMovement parent)
    {
        move.SetVariables(parent.GetMoveDist(), parent.GetMoveSpeed(), parent.GetDegTurn(), parent.GetRotSpeed(), parent.GetMoveTimer());
    }




    #endregion

}
