using UnityEngine;
    using TMPro; // If using TextMeshPro

    public class FPSCounter : MonoBehaviour
    {
        public TextMeshProUGUI fpsText; // Or public Text fpsText; if not using TextMeshPro

        private float pollingTime = 1f; // How often to update the FPS display
        private float time;
        private int frameCount;
        
        public bool fpsDisplayed = false;

        void Update()
        {
            if (!fpsDisplayed){ return;}
            time += Time.deltaTime;
            frameCount++;

            if (time >= pollingTime)
            {
                int frameRate = Mathf.RoundToInt(frameCount / time);
                fpsText.text = frameRate.ToString() + " FPS";

                time -= pollingTime;
                frameCount = 0;
            }
        }

        public void FpsDisplayOn(bool isOn)
        {
            fpsDisplayed = isOn;
            if (fpsDisplayed)
            {
                fpsText.gameObject.SetActive(true);
            }
            else
            {
                fpsText.gameObject.SetActive(false);
            }
        }
        
    }