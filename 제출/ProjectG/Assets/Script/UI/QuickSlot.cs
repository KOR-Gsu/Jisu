using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    public bool isCooldown = false;

    [SerializeField] private Image cooldownImage;
    [SerializeField] private float cooldown;

    public void StartCooldown()
    {
        cooldownImage.gameObject.SetActive(true);
        StartCoroutine(UpdateCooldown(cooldown));
    }

    private IEnumerator UpdateCooldown(float cooldown)
    {
        isCooldown = true;

        float nowDelayTime = 0;
        while (nowDelayTime < cooldown)
        {
            nowDelayTime += Time.deltaTime;
            cooldownImage.fillAmount = (1.0f - nowDelayTime / cooldown);

            yield return new WaitForFixedUpdate();
        }

        isCooldown = false;
    }
}
