using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public enum POWER_UP
{
    SpeedBoost,
    DoubleJump
}
public class PowerUp : MonoBehaviour
{
    
    public POWER_UP power = new POWER_UP();

    public float speed_boost = 0;
    public float boost_time = 0;
   
    
    bool show_speed = false;


   /* #region Editor
#if UNITY_EDITOR

    [CustomEditor(typeof(PowerUp))]
    public class PowerUpEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PowerUp power_up = (PowerUp)target;



            if (power_up.power == POWER_UP.SpeedBoost)
            {
                power_up.show_speed = EditorGUILayout.Foldout(power_up.show_speed, "Speed details", true);

                if (power_up.show_speed)
                {
                    showDetails(power_up);
                }
            }


        }

        private static void showDetails(PowerUp power_up)
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Speed boost");
            power_up.value = EditorGUILayout.IntField(power_up.value);

            EditorGUILayout.EndHorizontal();
        }
    }

#endif
    #endregion*/
}





