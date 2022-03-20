using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class TowerPlacement : MonoBehaviour
{

    public Vector3 boxCenter;
    public Vector3 boxSize;

    public LayerMask terrainLayer;

    public Color validColor = Color.green;
    public Color invalidColor = Color.red;

    private Color currentColor = Color.blue;
    private bool clippingGround = false;

    public Vector2[] lowerPoints;
    private StarterAssetsInputs _input;

    public GameObject towerToPlace;

    public float rayDistance = 100;

    public Material invalidMat;
    public Material validMat;

    private bool validPos;

    public Vector3[] offsets = new Vector3[4];
    [Range(0, 1)] public float slopeRangeMin;
    [Range(0, 1)] public float slopeRangeMax;
    public float[] slopes;

    private MeshRenderer renderer;

    public Vector3 towerCenter;
    public float towerSize;
    public LayerMask towerLayer;

    public void Start() {
        _input = GameObject.FindObjectOfType<StarterAssetsInputs>();
        renderer = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    public void Update() {
        CheckOverlap();
        MoveWithMouse();
        CheckSlopes();
    }

    public void MoveWithMouse() {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, rayDistance, terrainLayer))
        {
            transform.position = hit.point;
            if (_input.fire && validPos) {
                GameObject a = Instantiate(towerToPlace, transform.position, Quaternion.identity);
                a.transform.parent = null;
                _input.fire = false;
                gameObject.SetActive(false);
            }
            _input.fire = false;
        }
    }

    public void CheckOverlap() {
        var a = Physics.OverlapBox(transform.position + boxCenter, boxSize, Quaternion.identity, terrainLayer);
        clippingGround = a.Length > 0;
        if (clippingGround) {
            currentColor = validColor;
            
        } else {
            currentColor = invalidColor;

        }
        if(validPos)
        {
            renderer.material = validMat;
        } else
        {
            renderer.material = invalidMat;
        }
    }

    public int slopeCheckDistance = 50;

    public void CheckSlopes()
    {
        bool isInvalid = false;
        int pointsInvalid = 0;
        for(int i = 0; i < 4; i++)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position + offsets[i], Vector3.down, out hit, slopeCheckDistance, terrainLayer);
            if(hit.collider)
            {
                slopes[i] = hit.normal.y;
                if(!isInvalid)
                if(hit.normal.y < slopeRangeMin || hit.normal.y > slopeRangeMax ) {
                    pointsInvalid++;
                }
            }
        }
        if (pointsInvalid > 1) validPos = false;
        else validPos = true;

        var b = Physics.OverlapSphere(transform.position + towerCenter, towerSize, towerLayer);
        if (b.Length > 0) validPos = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = currentColor;
        Gizmos.DrawWireCube(boxCenter + transform.position, boxSize);
    }

}
