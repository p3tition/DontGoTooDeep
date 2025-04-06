using UnityEngine;

public class HideTutorial : MonoBehaviour
{ 
    [SerializeField]private GameObject tutorialUI;

    public void HideTutorialUI()
    {
        Debug.Log("Hide Tutorial UI");
        tutorialUI.SetActive(false);
    }
}
