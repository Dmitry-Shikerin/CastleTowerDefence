using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DevDunk.AutoLOD.Samples
{
    public class DemoManager : MonoBehaviour
    {
        AnimatorLODManager animatorLOD;
        TMP_Text textmesh;

        void Awake()
        {
            animatorLOD = FindObjectOfType<AnimatorLODManager>(true);
            textmesh = FindObjectOfType<TMP_Text>(true);
        }

        private void Start()
        {
            textmesh.text = "AnimatorLOD State: " + (animatorLOD.IsRunning ? "Running" : "Not running");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0)
            {
                if (animatorLOD.IsRunning) animatorLOD.DisableAnimatorLOD();
                else animatorLOD.EnableAnimatorLOD();

                textmesh.text = "AnimatorLOD State: " + (animatorLOD.IsRunning ? "Running" : "Not running");
            }
        }
    }
}