using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStart : MonoBehaviour
{
    public Renderer[] renderers;

    public Material[] towerBuildMaterial;
    public Material[] towerMaterial;

    public bool isStarting = true;

    private float currentAmount;
    public float amountToAdd;

    public string valueName;
    public string noiseScaleName;
    private Vector2 noiseScaleRange = new Vector2(10.000f, 30.000f);

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < renderers.Length; i++) {
            towerBuildMaterial[i].SetFloat(noiseScaleName, Random.Range(noiseScaleRange.x, noiseScaleRange.y));
            towerBuildMaterial[i].SetFloat(valueName, 0f);
            renderers[i].material = towerBuildMaterial[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isStarting) {
            currentAmount += amountToAdd;
            for (int i = 0; i < renderers.Length; i++) {
                towerBuildMaterial[i].SetFloat(valueName, currentAmount);
            }
            if(currentAmount >= 1)
            {
                for(int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].material = towerMaterial[i];
                }
                isStarting = false;
            }
        }
    }
}
