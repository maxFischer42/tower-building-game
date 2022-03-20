using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{

    public Vector2 origin = Vector2.zero;
    public Vector2 target = Vector2.zero;
    public float speed = 10f;
    public float arcHeight = 10f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = origin;
    }

    void Update() {
        GetTrajectory();
    }

    void GetTrajectory() {
        

        float x0 = origin.x;
        float x1 = target.x;
        float distance = x1 - x0;
        float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
        float baseY = Mathf.Lerp(origin.y, target.y, (nextX - x0) / distance);
        float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * distance * distance);
        Vector3 nextPos = new Vector3(nextX, baseY + arc, transform.position.z);
        transform.position = nextPos;
        if(nextPos == (Vector3)target) Start();

    }


}
