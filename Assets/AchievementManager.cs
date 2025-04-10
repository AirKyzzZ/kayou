using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public GameObject achievementPopupPrefab;
    public Transform popupParent;
    private Queue<GameObject> popupQueue = new Queue<GameObject>();

    public void ShowAchievement(string title, string description)
    {
        GameObject popup = Instantiate(achievementPopupPrefab, popupParent);
        popup.transform.Find("TitleText").GetComponent<Text>().text = title;
        popup.transform.Find("DescriptionText").GetComponent<Text>().text = description;
        popupQueue.Enqueue(popup);
        StartCoroutine(DisplayPopup(popup));
    }

    private IEnumerator DisplayPopup(GameObject popup)
    {
        popup.SetActive(true);
        yield return new WaitForSeconds(3f); // Display duration
        popup.SetActive(false);
        Destroy(popup);
        popupQueue.Dequeue();
    }
}