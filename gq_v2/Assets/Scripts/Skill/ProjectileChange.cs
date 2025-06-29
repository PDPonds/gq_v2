using UnityEngine;
using UnityEngine.UI;

public class ProjectileChange : MonoBehaviour
{
    [SerializeField] Transform border;
    [SerializeField] Image fill;
    float duration;
    float curDuration;

    private void Start()
    {
        Setup(2, 3, 1, 0.25f);
    }

    public void Setup(float speed, float projectileDuration, float changeDuration, float size)
    {
        duration = changeDuration;
        border.localScale = new Vector3(size, speed * projectileDuration, 1);
        border.position = new Vector3(0, 0.01f, speed * projectileDuration / 2);
    }

    private void Update()
    {
        curDuration += Time.deltaTime;
        if (curDuration > duration)
        {
            Destroy(gameObject);
        }
        fill.fillAmount = curDuration / duration;

    }

}
