using UnityEngine;
using UnityEngine.UI;

public class AreaChange : MonoBehaviour
{
    [SerializeField] Transform border;
    [SerializeField] Transform fill;

    float duration;
    float currentDuration;

    public void Setup(float changeDuration, float size)
    {
        duration = changeDuration;
        border.localScale = new Vector3(size, size, 1);
    }

    private void Update()
    {
        currentDuration += Time.deltaTime;
        if (currentDuration > duration)
        {
            gameObject.SetActive(false);
            currentDuration = 0;
        }

        if (gameObject.activeSelf)
        {
            float f = currentDuration / duration;
            fill.localScale = new Vector3(f, f, 0);
        }
    }

}
