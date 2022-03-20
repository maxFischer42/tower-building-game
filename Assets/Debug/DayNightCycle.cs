using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boxophobic;

public class DayNightCycle : MonoBehaviour
{
    public Vector3 rateOfChange;
    public Vector2 dayTime;
    public Vector2 nightTime;

    public enum set_time { day, night};
    public set_time world_time = set_time.day;

    private float current_time_rotation;
    private Transform t;

    public Material skyboxmat;
    public Color skyboxcolor;

    public Color dayColor = Color.white;
    public Color nightColor = Color.white;

    public float a;

    private void Start()
    {
        t = GetComponent<Transform>();
    }

    public void FixedUpdate()
    {
        skyboxmat.SetColor("Cubemap Tint Color", skyboxcolor);
        a = t.rotation.eulerAngles.x;
        //if (a > 180) t.rotation = Quaternion.Euler(-180, t.rotation.y, t.rotation.z);
        t.Rotate(rateOfChange.x,rateOfChange.y,rateOfChange.z);
        //if (a > 360) t.rotation = Quaternion.Euler(-360, t.rotation.y, t.rotation.z);
        if(a >= dayTime.x && a < dayTime.y && world_time != set_time.day)
        {
            world_time = set_time.day;
            LerpToColor(nightColor, dayColor);
            //     ChangeMaterial(skyboxmat, night);
        } else if(a >= nightTime.x && a < nightTime.y && world_time != set_time.night)
        {
            world_time = set_time.night;
            LerpToColor(dayColor, nightColor);
        }
    }
    public float timeToChangeSkybox;
/*

    public void ChangeMaterial(Material mat1, Material mat2)
    {
        float lerp = Mathf.PingPong(Time.time, timeToChangeSkybox) / timeToChangeSkybox;
        skyboxmat.Lerp(mat1, mat2, lerp);
    }*/

    public void LerpToColor(Color c1, Color c2)
    {
        skyboxcolor = Color.Lerp(c1, c2, Mathf.PingPong(Time.time, timeToChangeSkybox));
    }
}
