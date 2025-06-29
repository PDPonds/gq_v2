using UnityEngine;
using UnityEngine.UI;

public class AreaChange : MonoBehaviour
{
    [SerializeField] Transform border;
    [SerializeField] Transform fill;

    float duration;
    float currentDuration;

    private void Start()
    {
        Setup(2, 3);
    }

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
            Destroy(gameObject);
        }

        float f = currentDuration / duration;
        fill.localScale = new Vector3(f, f, 0);
    }

}
