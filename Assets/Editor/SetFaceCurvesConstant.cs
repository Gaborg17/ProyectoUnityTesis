using UnityEditor;
using UnityEngine;

public class SetFaceCurvesConstant
{
    [MenuItem("Tools/Poner curvas de cara en Constant")]
    static void Run()
    {
        foreach (var obj in Selection.objects)
        {
            var clip = obj as AnimationClip;
            if (clip == null) continue;

            foreach (var binding in AnimationUtility.GetCurveBindings(clip))
            {
                if (!binding.propertyName.Contains("eyeIndex") &&
                    !binding.propertyName.Contains("mouthIndex")) continue;

                var curve = AnimationUtility.GetEditorCurve(clip, binding);
                for (int i = 0; i < curve.length; i++)
                {
                    AnimationUtility.SetKeyLeftTangentMode(curve, i, AnimationUtility.TangentMode.Constant);
                    AnimationUtility.SetKeyRightTangentMode(curve, i, AnimationUtility.TangentMode.Constant);
                }
                AnimationUtility.SetEditorCurve(clip, binding, curve);
            }
            Debug.Log("Curvas en Constant: " + clip.name);
        }
    }
}