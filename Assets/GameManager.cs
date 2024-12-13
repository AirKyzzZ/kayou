using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // UI Elements
    public Text scoreText; // Displays the current score
    public Button clickButton; // Main click button
    public Button upgradeButton; // Upgrade button
    public Text upgradeCostText; // Displays the cost of the upgrade
    public Text pointsPerClickText; // Displays the points per click

    // New Upgrade Buttons and Texts
    public Button lanceKayouButton;
    public Button kanonButton;
    public Button pistoKayouButton;
    public Button kayouLaserButton;
    public Button mineKayouButton;
    public Button forgeKayouButton;
    public Button kayouArbaleteButton;
    public Button bombeKayouButton;
    public Button kayouAsteroideButton;
    public Button kayouSupernovaButton;
    public Button kayouExtracteurButton;
    public Button kayouRaffinerieButton;
    public Button kayouCentraleEnergetiqueButton;
    public Button kayouUsineProductionButton;
    public Button kayouVaisseauCollecteurButton;
    public Button kayouStationOrbitaleButton;
    public Button kayouTerraformeurButton;
    public Button kayouPortailInterstellaireButton;
    public Button kayouUniversParalleleButton;

    public Text lanceKayouCostText;
    public Text kanonCostText;
    public Text pistoKayouCostText;
    public Text kayouLaserCostText;
    public Text mineKayouCostText;
    public Text forgeKayouCostText;
    public Text kayouArbaleteCostText;
    public Text bombeKayouCostText;
    public Text kayouAsteroideCostText;
    public Text kayouSupernovaCostText;
    public Text kayouExtracteurCostText;
    public Text kayouRaffinerieCostText;
    public Text kayouCentraleEnergetiqueCostText;
    public Text kayouUsineProductionCostText;
    public Text kayouVaisseauCollecteurCostText;
    public Text kayouStationOrbitaleCostText;
    public Text kayouTerraformeurCostText;
    public Text kayouPortailInterstellaireCostText;
    public Text kayouUniversParalleleCostText;

    // Buyable Items Costs
    public int lanceKayouCost = 50;
    public int kanonCost = 100;
    public int pistoKayouCost = 500;
    public int kayouLaserCost = 1000;
    public int mineKayouCost = 2000;
    public int forgeKayouCost = 5000;
    public int kayouArbaleteCost = 10000;
    public int bombeKayouCost = 20000;
    public int kayouAsteroideCost = 50000;
    public int kayouSupernovaCost = 100000;
    public int kayouExtracteurCost = 2000;
    public int kayouRaffinerieCost = 5000;
    public int kayouCentraleEnergetiqueCost = 10000;
    public int kayouUsineProductionCost = 20000;
    public int kayouVaisseauCollecteurCost = 50000;
    public int kayouStationOrbitaleCost = 100000;
    public int kayouTerraformeurCost = 200000;
    public int kayouPortailInterstellaireCost = 500000;
    public int kayouUniversParalleleCost = 1000000;

    // Gameplay Variables
    private int score = 0; // Current score
    private int pointsPerClick = 1; // Points gained per click
    private int upgradeCost = 10; // Initial upgrade cost

    void Start()
    {
        // Initialize UI and button events
        score = 0;
        pointsPerClick = 1;
        upgradeCost = 10;
        UpdateScoreUI();
        UpdateUpgradeUI();
        UpdatePointsPerClickUI();
        UpdatePurchaseButtons();

        clickButton.onClick.AddListener(OnClick); // Action for clicking
        upgradeButton.onClick.AddListener(OnUpgrade); // Action for upgrading

        lanceKayouButton.onClick.AddListener(() => OnUpgradeButtonClick(ref lanceKayouCost, 5));
        kanonButton.onClick.AddListener(() => OnUpgradeButtonClick(ref kanonCost, 10));
        pistoKayouButton.onClick.AddListener(() => OnUpgradeButtonClick(ref pistoKayouCost, 50));
        kayouLaserButton.onClick.AddListener(() => OnUpgradeButtonClick(ref kayouLaserCost, 100));
        mineKayouButton.onClick.AddListener(() => OnUpgradeButtonClick(ref mineKayouCost, 200));
        forgeKayouButton.onClick.AddListener(() => OnUpgradeButtonClick(ref forgeKayouCost, 500));
        kayouArbaleteButton.onClick.AddListener(() => OnUpgradeButtonClick(ref kayouArbaleteCost, 1000));
        bombeKayouButton.onClick.AddListener(() => OnUpgradeButtonClick(ref bombeKayouCost, 2000));
        kayouAsteroideButton.onClick.AddListener(() => OnUpgradeButtonClick(ref kayouAsteroideCost, 5000));
        kayouSupernovaButton.onClick.AddListener(() => OnUpgradeButtonClick(ref kayouSupernovaCost, 10000));
        kayouExtracteurButton.onClick.AddListener(() => OnUpgradeButtonClick(ref kayouExtracteurCost, 200));
        kayouRaffinerieButton.onClick.AddListener(() => OnUpgradeButtonClick(ref kayouRaffinerieCost, 500));
        kayouCentraleEnergetiqueButton.onClick.AddListener(() => OnUpgradeButtonClick(ref kayouCentraleEnergetiqueCost, 1000));
        kayouUsineProductionButton.onClick.AddListener(() => OnUpgradeButtonClick(ref kayouUsineProductionCost, 2000));
        kayouVaisseauCollecteurButton.onClick.AddListener(() => OnUpgradeButtonClick(ref kayouVaisseauCollecteurCost, 5000));
        kayouStationOrbitaleButton.onClick.AddListener(() => OnUpgradeButtonClick(ref kayouStationOrbitaleCost, 10000));
        kayouTerraformeurButton.onClick.AddListener(() => OnUpgradeButtonClick(ref kayouTerraformeurCost, 20000));
        kayouPortailInterstellaireButton.onClick.AddListener(() => OnUpgradeButtonClick(ref kayouPortailInterstellaireCost, 50000));
        kayouUniversParalleleButton.onClick.AddListener(() => OnUpgradeButtonClick(ref kayouUniversParalleleCost, 100000));
    }

    void OnClick()
    {
        // Increment the score based on pointsPerClick
        score += pointsPerClick;
        UpdateScoreUI();
        UpdateUpgradeUI(); // Ensure the upgrade button state is updated after each click
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

    void OnUpgradeButtonClick(ref int itemCost, int pointsIncrement)
    {
        if (score >= itemCost)
        {
            score -= itemCost;
            pointsPerClick += pointsIncrement;
            itemCost = Mathf.RoundToInt(itemCost * 1.75f);
            UpdateScoreUI();
            UpdatePurchaseButtons();
        }
    }

    void UpdateScoreUI()
    {
        // Update the score display
        scoreText.text = $"Score: {score}";
        UpdatePurchaseButtons();
    }

    void UpdateUpgradeUI()
    {
        // Update the upgrade cost text and button interactivity
        upgradeCostText.text = $"Upgrade: {upgradeCost} points";
        upgradeButton.interactable = score >= upgradeCost;
    }

    void UpdatePointsPerClickUI()
    {
        // Update the points per click display
        pointsPerClickText.text = $"Points per Click: {pointsPerClick}";
    }

    void UpdatePurchaseButtons()
    {
        // Update the interactivity of the purchase buttons based on the current score
        lanceKayouButton.interactable = score >= lanceKayouCost;
        kanonButton.interactable = score >= kanonCost;
        pistoKayouButton.interactable = score >= pistoKayouCost;
        kayouLaserButton.interactable = score >= kayouLaserCost;
        mineKayouButton.interactable = score >= mineKayouCost;
        forgeKayouButton.interactable = score >= forgeKayouCost;
        kayouArbaleteButton.interactable = score >= kayouArbaleteCost;
        bombeKayouButton.interactable = score >= bombeKayouCost;
        kayouAsteroideButton.interactable = score >= kayouAsteroideCost;
        kayouSupernovaButton.interactable = score >= kayouSupernovaCost;
        kayouExtracteurButton.interactable = score >= kayouExtracteurCost;
        kayouRaffinerieButton.interactable = score >= kayouRaffinerieCost;
        kayouCentraleEnergetiqueButton.interactable = score >= kayouCentraleEnergetiqueCost;
        kayouUsineProductionButton.interactable = score >= kayouUsineProductionCost;
        kayouVaisseauCollecteurButton.interactable = score >= kayouVaisseauCollecteurCost;
        kayouStationOrbitaleButton.interactable = score >= kayouStationOrbitaleCost; kayouTerraformeurButton.interactable = score >= kayouTerraformeurCost; kayouPortailInterstellaireButton.interactable = score >= kayouPortailInterstellaireCost; kayouUniversParalleleButton.interactable = score >= kayouUniversParalleleCost;
         lanceKayouCostText.text = $"Lance Kayou: {lanceKayouCost} points";
    kanonCostText.text = $"Kanon: {kanonCost} points";
    pistoKayouCostText.text = $"Pisto Kayou: {pistoKayouCost} points";
    kayouLaserCostText.text = $"Kayou Laser: {kayouLaserCost} points";
    mineKayouCostText.text = $"Mine à Kayou: {mineKayouCost} points";
    forgeKayouCostText.text = $"Forge à Kayou: {forgeKayouCost} points";
    kayouArbaleteCostText.text = $"Kayou Arbalète: {kayouArbaleteCost} points";
    bombeKayouCostText.text = $"Bombe Kayou: {bombeKayouCost} points";
    kayouAsteroideCostText.text = $"Kayou Astéroïde: {kayouAsteroideCost} points";
    kayouSupernovaCostText.text = $"Kayou Supernova: {kayouSupernovaCost} points";
    kayouExtracteurCostText.text = $"Kayou Extracteur: {kayouExtracteurCost} points";
    kayouRaffinerieCostText.text = $"Kayou Raffinerie: {kayouRaffinerieCost} points";
    kayouCentraleEnergetiqueCostText.text = $"Kayou Centrale Énergétique: {kayouCentraleEnergetiqueCost} points";
    kayouUsineProductionCostText.text = $"Kayou Usine de Production: {kayouUsineProductionCost} points";
    kayouVaisseauCollecteurCostText.text = $"Kayou Vaisseau Collecteur: {kayouVaisseauCollecteurCost} points";
    kayouStationOrbitaleCostText.text = $"Kayou Station Orbitale: {kayouStationOrbitaleCost} points";
    kayouTerraformeurCostText.text = $"Kayou Terraformeur: {kayouTerraformeurCost} points";
    kayouPortailInterstellaireCostText.text = $"Kayou Portail Interstellaire: {kayouPortailInterstellaireCost} points";
    kayouUniversParalleleCostText.text = $"Kayou Univers Parallèle: {kayouUniversParalleleCost} points";
    }
}

    void SpawnProjectile()
    {
        // Create a projectile at the click button's position
        GameObject projectile = Instantiate(projectilePrefab, projectileParent);
        projectile.transform.position = clickButton.transform.position;

        // Animate the projectile moving to its target
        StartCoroutine(MoveProjectile(projectile));
    }

    IEnumerator MoveProjectile(GameObject projectile)
    {
        Vector3 startPos = projectile.transform.position;
        Vector3 endPos = projectileParent.position;
        float elapsedTime = 0f;
        float duration = 1.0f / projectileSpeed; // Adjust duration based on speed

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            projectile.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            yield return null;
        }

        // Destroy the projectile after it reaches its target
        Destroy(projectile);
    }

    void OnCannonButtonClick()
    {
        if (score >= cannonCost)
        {
            score -= cannonCost;
            cannonMultiplier++;
            UpdateScoreUI();
            UpdateMultipliersUI();

            // Increase the cannon cost by 1.75 times
            cannonCost = Mathf.RoundToInt(cannonCost * 1.75f);
            UpdatePurchaseButtons();

            // Start the coroutine to add score over time
            StartCoroutine(AddScoreOverTime(1, 1 * cannonMultiplier));
            Debug.Log($"Cannon upgraded! New multiplier: {cannonMultiplier}");

            // Show the slingshot button and its cost text after the first cannon purchase
            if (!slingshotButton.gameObject.activeSelf)
            {
                slingshotButton.gameObject.SetActive(true);
                slingshotCostText.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("Not enough score to buy Kanon.");
        }
    }

    void OnSlingshotButtonClick()
    {
        if (score >= slingshotCost)
        {
            score -= slingshotCost;
            slingshotMultiplier++;
            UpdateScoreUI();
            UpdateMultipliersUI();

            // Increase the slingshot cost by 1.75 times
            slingshotCost = Mathf.RoundToInt(slingshotCost * 1.75f);
            UpdatePurchaseButtons();

            // Start the coroutine to add score over time
            StartCoroutine(AddScoreOverTime(3, 5 * slingshotMultiplier));
            Debug.Log($"Slingshot upgraded! New multiplier: {slingshotMultiplier}");
        }
        else
        {
            Debug.LogWarning("Not enough score to buy Lance-Kayou.");
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