using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public AchievementManager achievementManager;

private int[] scoreMilestones = { 1000, 5000, 10000, 50000, 100000 };
private string[] milestoneTitles = { "First Steps", "Getting There", "On a Roll", "Halfway There", "Master Clicker" };
private string[] milestoneDescriptions = { "Reach 1,000 points", "Reach 5,000 points", "Reach 10,000 points", "Reach 50,000 points", "Reach 100,000 points" };

public class GameManager : MonoBehaviour
{
     private EventTrigger eventTrigger;
    // UI Elements
    public Text scoreText;
    public Button clickButton;
    public Image clickButtonImage;
    public Sprite bronzeSprite;
    public Sprite steelSprite;
    public Sprite goldSprite;
    public Text spriteNameText;
    public Button upgradeButton;
    public Text upgradeCostText;
    public Button cannonButton;
    public Button slingshotButton;
    public Button catapultButton;
    public Button trebuchetButton;
    public Text cannonMultiplierText;
    public Text slingshotMultiplierText;
    public Text catapultMultiplierText;
    public Text trebuchetMultiplierText;
    public Text cannonCostText;
    public Text slingshotCostText;
    public Text catapultCostText;
    public Text trebuchetCostText;
    public Text pointsPerClickText;

    // Mana Elements
    public Slider manaSlider;
    public Text manaText;
    public Button ultimateButton;

    // Projectile Animation
    public Transform projectileParent;
    public GameObject projectilePrefab;
    public float projectileSpeed = 5.0f;

    // Mana Variables
    private float mana = 0;
    private float manaMax = 100;
    private float manaRegenRate = 1f;
    private float manaDecayRate = 0.5f;
    private bool ultimateActive = false;
    private bool manaFullDelayActive = false;

    // Ultimate Variables
    private float ultimateDuration = 5f;
    private float ultimateMultiplier = 3;

    // Buyable Items
    public int cannonCost = 50;
    public int slingshotCost = 1000;
    public int catapultCost = 20000;
    public int trebuchetCost = 100000;

    // Gameplay Variables
    private int score = 0;
    private int pointsPerClick = 1;
    private int upgradeCost = 10;
    private int cannonMultiplier = 0;
    private int slingshotMultiplier = 0;
    private int catapultMultiplier = 0;
    private int trebuchetMultiplier = 0;

    private int bronzeThreshold = 15000;
    private int steelThreshold = 100000;
    private int goldThreshold = 1000000;

    // Sprite Names
    private string[] spriteNames = { "Kayou Classique", "Kayou de Bronze", "Kayou en Acier", "Kayou d'Or" };

    // Current Sprite State
    private int currentSpriteIndex = 0;

    string FormatScore(int score)
    {
        if (score >= 1000000000)
        {
            return $"{score / 1000000000f:0.0}B";
        }
        else if (score >= 1000000)
        {
            return $"{score / 1000000f:0.0}M";
        }
        else if (score >= 1000)
        {
            return $"{score / 1000f:0.0}K";
        }
        else
        {
            return score.ToString();
        }
    }

    string FormatNumber(int number)
    {
        if (number >= 1000000)
            return (number / 1000000D).ToString("0.##M");
        if (number >= 1000)
            return (number / 1000D).ToString("0.##k");

        return number.ToString();
    }

    void Start()
    {
        achievementManager = FindObjectOfType<AchievementManager>();
        scoreGainText = GameObject.Find("ScoreGainText").GetComponent<Text>();
        scoreGainText.gameObject.SetActive(false);

        score = 0;
        pointsPerClick = 1;
        upgradeCost = 10;

        UpdateScoreUI();
        UpdateUpgradeUI();
        UpdateMultipliersUI();
        UpdatePointsPerClickUI();
        UpdatePurchaseButtons();
        UpdateManaUI();
        UpdateSpriteName();
        UpdateCostTexts();

        slingshotButton.gameObject.SetActive(false);
        slingshotCostText.gameObject.SetActive(false);
        catapultButton.gameObject.SetActive(false);
        catapultCostText.gameObject.SetActive(false);
        trebuchetButton.gameObject.SetActive(false);
        trebuchetCostText.gameObject.SetActive(false);

        clickButton.onClick.AddListener(OnClick);
        upgradeButton.onClick.AddListener(OnUpgrade);
        cannonButton.onClick.AddListener(OnCannonButtonClick);
        slingshotButton.onClick.AddListener(OnSlingshotButtonClick);
        catapultButton.onClick.AddListener(OnCatapultButtonClick);
        trebuchetButton.onClick.AddListener(OnTrebuchetButtonClick);
        ultimateButton.onClick.AddListener(ActivateUltimate);

        StartCoroutine(DrainMana());
    }


IEnumerator ClickAnimation()
{
    Vector3 originalScale = clickButton.transform.localScale;
    Vector3 targetScale = originalScale * 0.9f; // Decrease to 90% of original size
    float animationDuration = 0.1f; // Animation duration in seconds

    // Scale down
    float elapsedTime = 0f;
    while (elapsedTime < animationDuration)
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / animationDuration);
        clickButton.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
        yield return null;
    }

    // Scale back to original
    elapsedTime = 0f;
    while (elapsedTime < animationDuration)
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / animationDuration);
        clickButton.transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
        yield return null;
    }
}

IEnumerator ScaleOverTime(Vector3 startScale, Vector3 endScale, float duration)
{
    float elapsedTime = 0f;

    while (elapsedTime < duration)
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);
        clickButton.transform.localScale = Vector3.Lerp(startScale, endScale, t);
        yield return null;
    }

    clickButton.transform.localScale = endScale;
}

    void UpdatePurchaseButtons()
    {
        cannonButton.interactable = score >= cannonCost;
        slingshotButton.interactable = score >= slingshotCost;
        catapultButton.interactable = score >= catapultCost;
        trebuchetButton.interactable = score >= trebuchetCost;

        UpdateCostTexts();
    }

    void SpawnProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileParent);
        projectile.transform.position = clickButton.transform.position;
        StartCoroutine(MoveProjectile(projectile));
    }

    IEnumerator MoveProjectile(GameObject projectile)
    {
        Vector3 startPos = projectile.transform.position;
        Vector3 endPos = projectileParent.position;
        float elapsedTime = 0f;
        float duration = 1.0f / projectileSpeed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            projectile.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            yield return null;
        }

        Destroy(projectile);
    }

    void OnClick()
    {
        score += pointsPerClick;
        mana = Mathf.Clamp(mana + manaRegenRate, 0, manaMax);
        UpdateScoreUI();
        UpdateUpgradeUI();
        UpdateManaUI();
        SpawnProjectile();

        UpdateSprite();
        UpdateSpriteName();

    }

void UpdateSprite()
{
    if (score >= goldThreshold && currentSpriteIndex < 3)
    {
        clickButtonImage.sprite = goldSprite;
        currentSpriteIndex = 3;
    }
    else if (score >= steelThreshold && currentSpriteIndex < 2)
    {
        clickButtonImage.sprite = steelSprite;
        currentSpriteIndex = 2;
    }
    else if (score >= bronzeThreshold && currentSpriteIndex < 1)
    {
        clickButtonImage.sprite = bronzeSprite;
        currentSpriteIndex = 1;
    }

    Vector3 originalScale = clickButton.transform.localScale;
    Vector3 targetScale = originalScale * 0.5f; // Decrease to 50% of original size

    StartCoroutine(ScaleOverTime(originalScale, targetScale, 0.1f));
    StartCoroutine(ScaleOverTime(targetScale, originalScale, 0.1f));
}

    void UpdateSpriteName()
    {
        spriteNameText.text = spriteNames[currentSpriteIndex];
    }

    void OnUpgrade()
    {
        if (score >= upgradeCost)
        {
            score -= upgradeCost;
            pointsPerClick++;
            upgradeCost = Mathf.RoundToInt(upgradeCost * 1.75f);

            UpdateScoreUI();
            UpdateUpgradeUI();
            UpdatePointsPerClickUI();
        }
    }

void UpdateScoreUI()
{
    scoreText.text = $"K$: {FormatScore(score)}";
    UpdatePurchaseButtons();
    scoreGainText.gameObject.SetActive(false);
    CheckAchievements();
}

void CheckAchievements()
{
    for (int i = 0; i < scoreMilestones.Length; i++)
    {
        if (score >= scoreMilestones[i])
        {
            achievementManager.ShowAchievement(milestoneTitles[i], milestoneDescriptions[i]);
            // Remove the milestone to prevent duplicate pop-ups
            scoreMilestones[i] = int.MaxValue;
        }
    }
}

    void UpdateUpgradeUI()
    {
        upgradeCostText.text = $"Upgrade: {FormatScore(upgradeCost)} points";
        upgradeButton.interactable = score >= upgradeCost;
    }

    void UpdateCostTexts()
    {
        cannonCostText.text = $"Cannon: {FormatScore(cannonCost)} points";
        slingshotCostText.text = $"Slingshot: {FormatScore(slingshotCost)} points";
        catapultCostText.text = $"Catapult: {FormatScore(catapultCost)} points";
        trebuchetCostText.text = $"Trebuchet: {FormatScore(trebuchetCost)} points";
    }

    void UpdateMultipliersUI()
    {
        cannonMultiplierText.text = $"x{cannonMultiplier}";
        slingshotMultiplierText.text = $"x{slingshotMultiplier}";
        catapultMultiplierText.text = $"x{catapultMultiplier}";
        trebuchetMultiplierText.text = $"x{trebuchetMultiplier}";
    }

    void UpdatePointsPerClickUI()
    {
        pointsPerClickText.text = $"Points per Click: {FormatScore(pointsPerClick)}";
    }

    void UpdateManaUI()
    {
        manaText.text = $"Mana: {mana}/{manaMax}";
        manaSlider.value = mana / manaMax;
        ultimateButton.interactable = mana >= manaMax;
    }

    IEnumerator DrainMana()
    {
        while (true)
        {
            if (!ultimateActive && !manaFullDelayActive)
            {
                if (mana >= manaMax)
                {
                    manaFullDelayActive = true;
                    yield return new WaitForSeconds(3f);
                    manaFullDelayActive = false;
                }

                mana = Mathf.Clamp(mana - manaDecayRate, 0, manaMax);
                UpdateManaUI();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void ActivateUltimate()
    {
        if (mana >= manaMax && !ultimateActive)
        {
            ultimateActive = true;
            mana = 0;
            UpdateManaUI();
            StartCoroutine(UltimateEffect());
        }
    }

    IEnumerator UltimateEffect()
    {
        float originalPointsPerClick = pointsPerClick;
        pointsPerClick *= (int)ultimateMultiplier;

        float originalProjectileSpeed = projectileSpeed;
        projectileSpeed *= ultimateMultiplier;

        yield return new WaitForSeconds(ultimateDuration);

        pointsPerClick = (int)originalPointsPerClick;
        projectileSpeed = originalProjectileSpeed;
        ultimateActive = false;
    }

    void OnCannonButtonClick()
    {
        if (score >= cannonCost)
        {
            score -= cannonCost;
            cannonMultiplier++;
            cannonCost = Mathf.RoundToInt(cannonCost * 1.75f);
            StartCoroutine(AddScoreOverTime(5, 10 * cannonMultiplier));
            UpdateScoreUI();
            UpdateMultipliersUI();
            UpdatePurchaseButtons();

            if (!slingshotButton.gameObject.activeSelf)
            {
                slingshotButton.gameObject.SetActive(true);
                slingshotCostText.gameObject.SetActive(true);
            }
        }
    }

    void OnSlingshotButtonClick()
    {
        if (slingshotButton == null || slingshotCostText == null || catapultButton == null || catapultCostText == null)
        {
            Debug.LogError("One or more UI elements are not set");
            return;
        }

        if (score >= slingshotCost)
        {
            score -= slingshotCost;
            slingshotMultiplier++;
            slingshotCost = Mathf.RoundToInt(slingshotCost * 1.75f);
            StartCoroutine(AddScoreOverTime(3, 20 * slingshotMultiplier));
            UpdateScoreUI();
            UpdateMultipliersUI();
            UpdatePurchaseButtons();

            if (!catapultButton.gameObject.activeSelf)
            {
                catapultButton.gameObject.SetActive(true);
                catapultCostText.gameObject.SetActive(true);
            }
        }
    }

    void OnCatapultButtonClick()
    {
        if (catapultButton == null || catapultCostText == null || trebuchetButton == null || trebuchetCostText == null)
        {
            Debug.LogError("One or more UI elements are not set");
            return;
        }

        if (score >= catapultCost)
        {
            score -= catapultCost;
            catapultMultiplier++;
            catapultCost = Mathf.RoundToInt(catapultCost * 1.75f);
            StartCoroutine(AddScoreOverTime(2, 50 * catapultMultiplier));
            UpdateScoreUI();
            UpdateMultipliersUI();
            UpdatePurchaseButtons();

            if (!trebuchetButton.gameObject.activeSelf)
            {
                trebuchetButton.gameObject.SetActive(true);
                trebuchetCostText.gameObject.SetActive(true);
            }
        }
    }

    void OnTrebuchetButtonClick()
    {
        if (score >= trebuchetCost)
        {
            score -= trebuchetCost;
            trebuchetMultiplier++;
            trebuchetCost = Mathf.RoundToInt(trebuchetCost * 1.75f);
            StartCoroutine(AddScoreOverTime(1, 100 * trebuchetMultiplier));
            UpdateScoreUI();
            UpdateMultipliersUI();
            UpdatePurchaseButtons();
        }
    }

    IEnumerator AddScoreOverTime(float interval, int points)
{
    while (true)
    {
        yield return new WaitForSeconds(interval);
        score += points;
        UpdateScoreUI();

        // Update the score gain text
        scoreGainText.text = $"+{points} points";
        scoreGainText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        scoreGainText.gameObject.SetActive(false);
    }
}
}