using UnityEngine;
using DG.Tweening;

public class RotateSprite : MonoBehaviour
{
    public Transform _transform;
    void OnEnable()
    {
        // Rotate the sprite 360 degrees over 2 seconds and loop infinitely
        _transform.DORotate(new Vector3(0, 0, -360), 2f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear) // Smooth linear rotation
            .SetLoops(-1, LoopType.Restart); // Infinite loop
    }
}