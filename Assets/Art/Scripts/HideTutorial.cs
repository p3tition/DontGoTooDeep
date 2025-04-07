using UnityEngine;

public class HideTutorial : MonoBehaviour
{ 
    [SerializeField]private GameObject tutorialUI;

    public void HideTutorialUI()
    {
        tutorialUI.SetActive(false);
    }
}
