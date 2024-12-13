// New upgrades: Catapult and Trebuchet added to the script.
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

    // Projectile Animation
    public Transform projectileParent;
    public GameObject projectilePrefab;
    public float projectileSpeed = 5.0f;

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
    }

    void OnClick()
    {
        score += pointsPerClick;
        UpdateScoreUI();
        UpdateUpgradeUI();
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
        scoreText.text = $"Score: {score}";
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
