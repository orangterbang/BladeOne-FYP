using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class HealthUI : MonoBehaviour
{
    public Health actorHealth;
    private float currHealth, maxHealth, lastHealth;

    [SerializeField] private float width, height;

    [SerializeField] private TMP_Text healthText;
    [SerializeField] private RectTransform healthBar, damageBar;
    
    private Coroutine damageBarCoroutine;
    void Start()
    {
        healthText = GetComponent<TMP_Text>();

        maxHealth = actorHealth.GetMaxHealth();
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        currHealth = Mathf.Clamp(actorHealth.GetCurrentHealth(), 0, maxHealth);
        healthText.text = actorHealth.GetCurrentHealth() + "/" + maxHealth;

        if (currHealth != lastHealth)
        {
            UpdateHealthBar();
            lastHealth = currHealth; // Update our tracker
        }

        if (!actorHealth.IsHealthActive())
        {
            PauseMenuManager.Instance.UpdateGameOver(actorHealth.gameObject);
        }
    }

    void UpdateHealthBar()
    {
        if (maxHealth <= 0) return;

        float newWidth = (currHealth / maxHealth) * width;

        healthBar.sizeDelta = new Vector2(newWidth, height);

        if(damageBarCoroutine != null)
        {
            StopCoroutine(damageBarCoroutine);
        }

        damageBarCoroutine = StartCoroutine(DelayDamageBar(newWidth));
    }

    IEnumerator DelayDamageBar(float newWidth)
    {
        // Wait for a brief moment after taking damage
        yield return new WaitForSeconds(0.5f);

        // Smoothly shrink the damage bar until it matches the health bar
        while (damageBar.sizeDelta.x > newWidth + 0.1f)
        {
            float currentWidth = damageBar.sizeDelta.x;
            // Lerp creates a smooth catching-up animation
            float nextWidth = Mathf.Lerp(currentWidth, newWidth, Time.deltaTime * 10f);
            
            damageBar.sizeDelta = new Vector2(nextWidth, height);
            yield return null;
        }

        // Ensure it snaps perfectly to the final target width
        damageBar.sizeDelta = new Vector2(newWidth, height);
    }
}
