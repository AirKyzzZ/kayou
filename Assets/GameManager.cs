using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // UI Elements
    public Text scoreText;            // Displays the current score
    public Button clickButton;        // Main click button
    public Button upgradeButton;      // Upgrade button
    public Text upgradeCostText;      // Displays the cost of the upgrade
    public Button cannonButton;       // Cannon button
    public Button slingshotButton;    // Slingshot button
    public Text cannonMultiplierText; // Displays the cannon multiplier
    public Text slingshotMultiplierText; // Displays the slingshot multiplier

    // Projectile Animation
    public Transform projectileParent; // Target position for the projectiles
    public GameObject projectilePrefab; // Prefab for the projectiles
    public float projectileSpeed = 5.0f; // Speed of the projectile animation

    // Buyable Items
    public GameObject cannonPrefab;   // Cannon prefab
    public GameObject slingshotPrefab; // Slingshot prefab
    public int cannonCost = 50;       // Cost of the cannon
    public int slingshotCost = 100;   // Cost of the slingshot

    // Gameplay Variables
    private int score = 0;            // Current score
    private int pointsPerClick = 1;   // Points gained per click
    private int upgradeCost = 10;     // Initial upgrade cost
    private int cannonMultiplier = 0; // Cannon multiplier
    private int slingshotMultiplier = 0; // Slingshot multiplier

    void Start()
    {
        // Initialize UI and button events
        score = 0;
        pointsPerClick = 1;
        upgradeCost = 10;

        UpdateScoreUI();
        UpdateUpgradeUI();
        UpdateMultipliersUI();
        clickButton.onClick.AddListener(OnClick);       // Action for clicking
        upgradeButton.onClick.AddListener(OnUpgrade);   // Action for upgrading
        cannonButton.onClick.AddListener(OnCannonButtonClick); // Action for cannon button
        slingshotButton.onClick.AddListener(OnSlingshotButtonClick); // Action for slingshot button
    }

    void OnClick()
    {
        // Increment the score based on pointsPerClick
        score += pointsPerClick;
        UpdateScoreUI();
        UpdateUpgradeUI(); // Ensure the upgrade button state is updated after each click

        // Spawn and animate a projectile
        SpawnProjectile();
    }

    void OnUpgrade()
    {
        if (CanUpgrade())
        {
            ApplyUpgrade();
        }
        else
        {
            Debug.LogWarning($"Upgrade Failed. Not enough score. Current Score: {score}, Upgrade Cost: {upgradeCost}");
        }
    }

    bool CanUpgrade()
    {
        // Check if the player has enough score to upgrade
        return score >= upgradeCost;
    }

    void ApplyUpgrade()
    {
        // Deduct the upgrade cost from the score
        score -= upgradeCost;

        // Increase points per click
        pointsPerClick++;

        // Increase the upgrade cost more significantly
        upgradeCost = Mathf.RoundToInt(upgradeCost * 2f);

        // Update the UI
        UpdateScoreUI();
        UpdateUpgradeUI();

        Debug.Log($"Upgrade Successful! New Score: {score}, New PointsPerClick: {pointsPerClick}, New UpgradeCost: {upgradeCost}");
    }

    void UpdateScoreUI()
    {
        // Update the score display
        scoreText.text = $"Score: {score}";
    }

    void UpdateUpgradeUI()
    {
        // Update the upgrade cost text and button interactivity
        Debug.Log($"Updating Upgrade UI. Current Score: {score}, Upgrade Cost: {upgradeCost}");
        upgradeCostText.text = $"Upgrade: {upgradeCost} points";
        upgradeButton.interactable = score >= upgradeCost;
        Debug.Log($"Upgrade button state updated. Interactable: {upgradeButton.interactable}");
    }

    void UpdateMultipliersUI()
    {
        // Update the multipliers display
        cannonMultiplierText.text = $"x{cannonMultiplier}";
        slingshotMultiplierText.text = $"x{slingshotMultiplier}";
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
            GameObject cannon = Instantiate(cannonPrefab, projectileParent);
            StartCoroutine(AddScoreOverTime(5, 10 * cannonMultiplier, cannon));
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
            GameObject slingshot = Instantiate(slingshotPrefab, projectileParent);
            StartCoroutine(AddScoreOverTime(3, 5 * slingshotMultiplier, slingshot));
        }
        else
        {
            Debug.LogWarning("Not enough score to buy Lance-Kayou.");
        }
    }

    IEnumerator AddScoreOverTime(float interval, int points, GameObject item)
    {
        while (item != null)
        {
            yield return new WaitForSeconds(interval);
            score += points;
            UpdateScoreUI();
        }
    }
}