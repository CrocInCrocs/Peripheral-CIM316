using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public GameObject ENDCUSTSCENEN;

    public void ActivateEnd()
    {
        ENDCUSTSCENEN.SetActive(false);
    }
}
