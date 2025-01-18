using UnityEngine;
using UnityEngine.UI;

public class SpeedBoostButton : MonoBehaviour
{
    public Fish playerFish;
    public Button btn;

    public Image imageToFill;
    public float fillDuration; // Duration in seconds to fill the image.
    private float fillStartTime; // Time when filling started.

    private bool canBoost = true;

    public void Bite()
    {
        if (canBoost)
        {
            canBoost = false;
            btn.interactable = false;
            playerFish.Boost();
            StartFilling();
            Invoke("ReEnableBtn", fillDuration);
        }
    }

    private void ReEnableBtn()
    {
        canBoost = true;
        btn.interactable = true;
    }

    private void Update()
    {
        // If the image is currently filling, update its fill amount based on time.
        if (imageToFill.fillAmount < 1.0f)
        {
            float timeSinceStart = Time.time - fillStartTime;
            float fillProgress = timeSinceStart / fillDuration;
            imageToFill.fillAmount = Mathf.Clamp01(fillProgress);
        }
    }

    private void StartFilling()
    {
        // Reset the fill amount and mark the starting time.
        imageToFill.fillAmount = 0.0f;
        fillStartTime = Time.time;
    }
}