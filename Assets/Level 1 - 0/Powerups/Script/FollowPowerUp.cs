using UnityEngine;
using UnityEngine.UI;

public class FollowPowerUp : MonoBehaviour
{
    public Transform targetPowerUp;
    public Vector3 offset;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (targetPowerUp != null)
        {
            rectTransform.position = Camera.main.WorldToScreenPoint(targetPowerUp.position + offset);
        }
    }
}
