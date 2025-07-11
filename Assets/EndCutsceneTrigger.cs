using System;
using UnityEngine;

public class EndCutsceneTrigger : MonoBehaviour
{
    public CutSceneManager EndCutscene;

    private void OnTriggerEnter(Collider other)
    {
        EndCutscene.ActivateEnd();
    }
}
