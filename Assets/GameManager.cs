using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // UI Elements
    public Text scoreText;
    public Button clickButton;
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
    public int slingshotCost = 100;
    public int catapultCost = 500;
    public int trebuchetCost = 1000;

    // Gameplay Variables
    private int score = 0;
    private int pointsPerClick = 1;
    private int upgradeCost = 10;
    private int cannonMultiplier = 0;
    private int slingshotMultiplier = 0;
    private int catapultMultiplier = 0;
    private int trebuchetMultiplier = 0;

    string FormatScore(int score)
{
    if (score >= 1000000000)
    {
        return $"{score / 1000000000}B";
    }
    else if (score >= 1000000)
    {
        return $"{score / 1000000}M";
    }
    else if (score >= 10000)
    {
        return $"{score / 10000}K";
    }
    else
    {
        return score.ToString();
    }
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

        Input.GetKeyDown(KeyCode.R) && ActivateUltimate();
        StartCoroutine(DrainMana());
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

    void UpdatePurchaseButtons()
    {
        cannonButton.interactable = score >= cannonCost;
        slingshotButton.interactable = score >= slingshotCost;
        catapultButton.interactable = score >= catapultCost;
        trebuchetButton.interactable = score >= trebuchetCost;

        cannonCostText.text = $"Cannon: {cannonCost} points";
        slingshotCostText.text = $"Slingshot: {slingshotCost} points";
        catapultCostText.text = $"Catapult: {catapultCost} points";
        trebuchetCostText.text = $"Trebuchet: {trebuchetCost} points";
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
                    yield return new WaitForSeconds(3f); // Wait for 3 seconds at full mana
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
    if (Input.GetKeyDown(KeyCode.R) && mana >= manaMax && !ultimateActive)
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
