using UnityEditor;
using System.Collections;
using UnityEngine;

[CustomEditor(typeof(TN_Character))]
[CanEditMultipleObjects]
public class TN_CharacterEditor : Editor
{
    SerializedProperty m_EnumActoryType;
    SerializedProperty m_TransformMRITarget;
    SerializedProperty m_FloatMovementSpeed;
    SerializedProperty m_ActorActions;
    SerializedProperty m_ActorTarget;
    private bool addAction;
    private void OnEnable() {
        m_EnumActoryType = serializedObject.FindProperty("actorType");
        m_TransformMRITarget = serializedObject.FindProperty("mriTarget");
        m_FloatMovementSpeed = serializedObject.FindProperty("moveSpeed");
        m_ActorActions = serializedObject.FindProperty("actorActions");
        m_ActorTarget = serializedObject.FindProperty("target");
    }
    public override void OnInspectorGUI() {
        serializedObject.Update();
        TN_Character script = (TN_Character)target;

        EditorGUILayout.PropertyField(m_EnumActoryType);
        EditorGUILayout.PropertyField(m_ActorTarget);

        EditorGUILayout.PropertyField(m_FloatMovementSpeed);
        if(script.actorType == ActorType.Patient) {
            script.mriTarget = EditorGUILayout.ObjectField("MRI target trans", script.mriTarget, typeof(Transform), true) as Transform;
        }
       // EditorGUILayout.PropertyField(m_TransformMRITarget);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        if(GUILayout.Button("Add Action")) {
            if(addAction) {
                addAction = false;
            } else addAction = true;
        }
        if(GUILayout.Button("Remove Action")) {
            m_ActorActions.arraySize -= 1;
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.PropertyField((m_ActorActions), true);

      
        if(addAction){
            m_ActorActions.arraySize += 1;
            addAction = false;
        }

        serializedObject.ApplyModifiedProperties();
    }

}
