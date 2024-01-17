using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(StepManager))]
public class StepManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StepManager stepManager = (StepManager)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Next Step"))
        {
            stepManager.NextStep();
        }

        if (GUILayout.Button("Previous Step"))
        {
            stepManager.PrevStep();
        }
    }
}
