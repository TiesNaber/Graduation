using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Characters/List", order = 1)]
public class TN_CharacterSO : ScriptableObject
{
    public string objectName = "New Character";
    public Transform objectTrans;
}
