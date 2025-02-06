using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
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
        EventTrigger trigger = clickButton.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = clickButton.gameObject.AddComponent<EventTrigger>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Start hover animation
        clickButton.transform.localScale = Vector3.one * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset scale to original size
        clickButton.transform.localScale = Vector3.one;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Start click animation
        StartCoroutine(ClickAnimation());
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // Reset scale to original size
        clickButton.transform.localScale = Vector3.one;
    }


    IEnumerator ClickAnimation()
{
    Vector3 originalScale = clickButton.transform.localScale;
    Vector3 clickedScale = originalScale * 0.9f;
    Vector3 expandedScale = originalScale * 1.1f;
    float clickDuration = 0.1f;
    float expandDuration = 0.1f;

    // Scale down
    yield return ScaleOverTime(originalScale, clickedScale, clickDuration);

    // Scale up (slightly larger than original)
    yield return ScaleOverTime(clickedScale, expandedScale, expandDuration);

    // Scale back to original
    yield return ScaleOverTime(expandedScale, originalScale, expandDuration);
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
    }

    void UpdateUpgradeUI()
    {
        upgradeCostText.text = $"Upgrade: {upgradeCost} points";
        upgradeButton.interactable = score >= upgradeCost;
    }

    void UpdateCostTexts()
    {
        cannonCostText.text = $"Cannon: {cannonCost} points";
        slingshotCostText.text = $"Slingshot: {slingshotCost} points";
        catapultCostText.text = $"Catapult: {catapultCost} points";
        trebuchetCostText.text = $"Trebuchet: {trebuchetCost} points";
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
        pointsPerClickText.text = $"Points per Click: {pointsPerClick}";
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
        }
    }
}