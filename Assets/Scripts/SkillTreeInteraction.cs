using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeInteraction : MonoBehaviour
{
    
}

[CreateAssetMenu(fileName = "SkillTreeNodeObject", menuName ="Assets/Nodes/Tree")]
public class SkillTreeNodeObject : ScriptableObject {
    public List<SkillTreeInteraction> connections;    
}
