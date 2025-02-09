using UnityEngine;


public class FireLighting : MonoBehaviour
{
    public bool Enable;
    UnityEngine.Rendering.Universal.Light2D FireLight;
    bool bEnable;
    float fTime;
    float fMax;

    private void Start()
    {
        FireLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        fMax = FireLight.intensity;
        FireLight.intensity = 0;

        bEnable = false;
    }


    public void SetActive(bool active)
    {
        bEnable = active;
        fTime = 0;
    }

    private void Update()
    {
        fTime += Time.deltaTime * 3;
        if (bEnable)
            FireLight.intensity = Mathf.Lerp(0, fMax, fTime);
        else
            FireLight.intensity = Mathf.Lerp(fMax, 0, fTime);
    }
}
