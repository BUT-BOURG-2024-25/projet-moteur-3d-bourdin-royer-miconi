using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject upgradeMenu;
    public Transform powerUpgradeMenuButtonsContainer;
    public GameObject pauseMenu;
    public GameObject buttonPrefab;
    public Image healthIndicator;
    public Canvas ui;
    public Joystick joystick;

    private Queue<Action> upgradeMenuQueue = new Queue<Action>();
    private bool isUpgradeMenuOpen = false;
    private bool isPowerUpgradeMenuOpen = false;
    public bool shouldShowNextMenu = true;

    [Header("XP")]
    public Image xpFill;
    public TextMeshProUGUI lvlText;

    [Header("PauseMenu")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI regenText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI currentDamageText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI damageBoostText;
    public TextMeshProUGUI cooldownBoostText;
    public TextMeshProUGUI pickupRangeText;
    public TextMeshProUGUI xpBoostText;
    public TextMeshProUGUI killedEnnemiesText;
    public TextMeshProUGUI timePlayedText;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(ui.gameObject);
    }

    private void Start()
    {
        UpdateXPBar();
    }

    public void DisplayUpgradeMenu()
    {
        upgradeMenuQueue.Enqueue(() =>
        {
            Time.timeScale = 0;
            upgradeMenu.SetActive(true);
            isUpgradeMenuOpen = true;

            Transform choicesTransform = upgradeMenu.transform.Find("Choices");
            List<Button> choices = new List<Button>();
            for (int i = 0; i < 3; i++)
            {
                choices.Add(choicesTransform.GetChild(i).GetComponent<Button>());
                choices[i].gameObject.SetActive(false);
            }

            List<Power> availablePower = PowerManager.Instance.GetNonMaxedPower();
            availablePower.Shuffle();

            for (int i = 0; i < 3 && i < availablePower.Count; i++)
            {
                Power power = availablePower[i];
                choices[i].gameObject.SetActive(true);

                TextMeshProUGUI buttonText = choices[i].transform.Find("PowerName").GetComponent<TextMeshProUGUI>();
                buttonText.SetText(power.name);

                Image iconImage = choices[i].transform.Find("Icon").GetComponent<Image>();
                iconImage.sprite = power.icon;

                choices[i].onClick.RemoveAllListeners();
                choices[i].onClick.AddListener(() =>
                {
                    UpgradePower(power);
                    HideUpgradeMenu();
                });
            }
        });
    }

    private void ShowNextUpgradeMenu()
    {
        PlayerManager.Instance.remainingLevel--;
        Action upgradeAction = upgradeMenuQueue.Dequeue();
        upgradeAction.Invoke();
    }

    public void HideUpgradeMenu()
    {
        upgradeMenu.SetActive(false);
        isUpgradeMenuOpen = false;

        if (!isUpgradeMenuOpen && !isPowerUpgradeMenuOpen)
        {
            Time.timeScale = 1;
        }
    }

    public void UpgradePower(Power power)
    {
        power.LevelUp();
        HideUpgradeMenu();
    }

    public void UpdateXPBar()
    {
        lvlText.SetText("Level " + PlayerManager.Instance.level);
        xpFill.fillAmount = PlayerManager.Instance.currentXP / PlayerManager.Instance.XPToLevelUp;
    }

    public void DisplayUpgradePowerMenu(Power power, List<PowerUpgradeButton> infos)
    {
        Time.timeScale = 0;
        powerUpgradeMenuButtonsContainer.gameObject.SetActive(true);
        isPowerUpgradeMenuOpen = true;

        foreach (Transform child in powerUpgradeMenuButtonsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (PowerUpgradeButton info in infos)
        {
            GameObject newButton = Instantiate(buttonPrefab, powerUpgradeMenuButtonsContainer);
            TextMeshProUGUI textComponent = newButton.GetComponentInChildren<TextMeshProUGUI>();

            if (textComponent != null)
            {
                textComponent.text = info.text;
            }

            Button buttonComponent = newButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.RemoveAllListeners();
                buttonComponent.onClick.AddListener(() =>
                {
                    info.callback?.Invoke();
                    HideUpgradePowerMenu();
                    shouldShowNextMenu = true;
                });
            }
        }
    }

    public void HideUpgradePowerMenu()
    {
        powerUpgradeMenuButtonsContainer.gameObject.SetActive(false);
        isPowerUpgradeMenuOpen = false;

        if (!isUpgradeMenuOpen && !isPowerUpgradeMenuOpen)
        {
            Time.timeScale = 1;
        }
    }

    private void Update()
    {
        if(PlayerManager.Instance.remainingLevel > 0 && shouldShowNextMenu)
        {
            ShowNextUpgradeMenu();
            shouldShowNextMenu = false;
        }
    }

    public void UpdateHealthIndicator()
    {
        float healthPercentage = ((float)PlayerManager.Instance.currentHealth / (float)PlayerManager.Instance.maxHealth);
        Color newColor = healthIndicator.color;
        newColor.a = (float)((1 - healthPercentage));
        healthIndicator.color = newColor;
    }

    public void DisplayPauseMenu()
    {
        PlayerManager playerManager = PlayerManager.Instance;

        if (!pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }

        healthText.SetText(playerManager.currentHealth+"/"+playerManager.maxHealth);
        regenText.SetText(playerManager.pvPerSecond + " PV/s");
        armorText.SetText(playerManager.armor + "%");
        currentDamageText.SetText(playerManager.baseDamage.ToString());
        speedText.SetText((playerManager.moveSpeed * (1 + (playerManager.speedBoost/100))).ToString());

        damageBoostText.SetText($"{(playerManager.damageBoost >= 0 ? "+" : "-")}{playerManager.damageBoost}%");
        cooldownBoostText.SetText($"{(playerManager.cooldownReduction >= 0 ? "+" : "-")}{playerManager.cooldownReduction}%");
        xpBoostText.SetText($"{(playerManager.xpBoost >= 0 ? "+" : "-")}{playerManager.xpBoost}%");

        pickupRangeText.SetText(playerManager.collectRange.ToString());

        killedEnnemiesText.SetText(EnemyManager.Instance.killedEnemies.ToString());
        timePlayedText.SetText($"{GameManager.Instance.playTimeMinutes:D2}:{GameManager.Instance.playTimeSeconds:D2}");

    }

    public void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public override void Reload()
    {
    }

}
