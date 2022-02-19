using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workshop : MonoBehaviour
{
    public Base[,] grid = new Base[25,25];

    public enum menuState { menu, node}
    public menuState state = menuState.menu;

    public GameObject baseUI;
    public GameObject nodeListUI;

    public GameObject previousSelect;
    public GameObject selection;

    public Color hoverColor;
    public Color defaultColor;
    public Color selectColor;

    string path;

    public void OnNodeSelect()
    {
        state = menuState.node;
        ToggleGameObject(baseUI);
        ToggleGameObject(nodeListUI);
        print(selection.name);
    }

    public void RemoveNode()
    {
        // Check if node has children before proceeding
        // TODO grey out button if node has children
        PartData data = selection.GetComponent<PartHolder>().partData;
        Part part = selection.GetComponent<PartHolder>().part;
        if(data.nodes.Length > 0)
        {
            if(part.links.Count == 0)
            {
                
            }
        }

    }

    void ToggleGameObject(GameObject obj) { obj.SetActive(!baseUI.activeInHierarchy); }

    public void CheckMouseSelect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            var obj = hit.transform.gameObject;
            if(previousSelect) previousSelect.GetComponent<MeshRenderer>().material.color = defaultColor;

            if(Input.GetAxisRaw("Fire1") > 0)
            {
                obj.GetComponent<MeshRenderer>().material.color = selectColor;
                if (previousSelect != obj) previousSelect.GetComponent<MeshRenderer>().material.color = defaultColor;
                selection = obj;
                OnNodeSelect();
            } else
            {
                obj.GetComponent<MeshRenderer>().material.color = hoverColor;
                previousSelect = obj;
            }
        } else
        {
            if (previousSelect) previousSelect.GetComponent<MeshRenderer>().material.color = defaultColor;
        }
    }





    public void Save(Base prefab) {
        string json = JsonUtility.ToJson(prefab);
        
       //Write some text to the test.txt file
        File.WriteAllText(path, json);
    }

    public void Load() {
        StreamReader reader = new StreamReader(path);
        string s = reader.ReadToEnd();
        reader.Close();        
        JsonUtility.FromJsonOverwrite(s, testObject);
        print("loaded from: " + path);

        grid[testObject.x,testObject.y] = testObject;

        // Create Gameobjects/nodes
        testObject.basePart.actor = RecursivelyInstantiate(testObject.loc, testObject.basePart);
    }

    GameObject RecursivelyInstantiate(Transform _base, Part p) {
        p.actor = (GameObject)Instantiate(p.prefab, _base);
        print(p.actor.transform.GetChild(0).childCount);
        foreach(Transform f in p.actor.transform.GetChild(0)) {
            p.nodes.Add(f);
        }
        for(int i = 0; i < p.parts.Count; i++) {
            GameObject v = RecursivelyInstantiate(p.nodes[i], p.parts[i]);
            p.links.Add(v);
        }
        return p.actor;
    }

    private void Start() {
        Debug.Log(Application.persistentDataPath);
        path = Application.persistentDataPath + "/save.txt";    
    }

    // In release this will be removed, just here to test saving and loading
    void Update() {
        if(state == menuState.menu) CheckMouseSelect();
        if(Input.GetKeyDown(KeyCode.Space)) {
            Load();
        }
        if(Input.GetKeyDown(KeyCode.Backspace)) {
            Save(testObject);
            grid[testObject.x,testObject.y] = new Base(0,0);
            testObject = new Base(0,0);
        }
    }

    public Base testObject;

    public int change = 1;

    string encode(string msg) {
        string code = "";
        foreach(char c in msg) {
            int v = c + change;
            code =  v + " ";
        }
        code.Substring(0, code.Length - 1);
        return code;
    }

    string decode(string msg) {
        string word = "";
        string[] code = msg.Split(' ');
        foreach(string s in code) {
            int i = int.Parse(s);
            i = i - change;
            word = word + i;
        }
        return word;
    }
}

[Serializable]
public class Part {
    public GameObject prefab;
    public GameObject actor;
    public List<Transform> nodes;
    public List<GameObject> links;
    public List<Part> parts;

    public Part(int _nodes) {
        nodes = new List<Transform>(_nodes);
        links = new List<GameObject>(_nodes);
        parts = new List<Part>(_nodes);
    }
}

[Serializable]
public class Base {
    public int x;
    public int y;
    public Part basePart;
    public Transform loc;

    public Base(int _x, int _y) {
        x = _x;
        y = _y;
    }
}

[CreateAssetMenu]
public class PartData : ScriptableObject {
    public GameObject model;
    public Transform[] nodes;   
}