using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform platesTrans;
    private List<GameObject> platesVisualGOList;
    private void Awake() {
        platesVisualGOList=new List<GameObject>();
    }
    private void Start() {
        platesCounter.OnPlateSpawn += PlatesCounter_OnPlateSpawn;
        platesCounter.OnPlateRemove +=PlatesCounter_OnPlateRemove;
    }

    private void PlatesCounter_OnPlateRemove(object sender, EventArgs e)
    {
        GameObject plateRemove = platesVisualGOList[platesVisualGOList.Count-1];
        platesVisualGOList.Remove(plateRemove);
        Destroy(plateRemove);
    }

    private void PlatesCounter_OnPlateSpawn(object sender, EventArgs e)
    {
        Transform plateSpawn = Instantiate(platesTrans,counterTopPoint);
        float offSetSpawn=.1f;
        plateSpawn.localPosition=new Vector3(0,offSetSpawn*platesVisualGOList.Count,0);
        platesVisualGOList.Add(plateSpawn.gameObject);
    }
}
