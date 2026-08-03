using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialAction
{
    MoveForward,
    MoveBackward,
    MoveLeft,
    MoveRight,
    Jump,
    Attack,
    Interact,
    DefeatEnemies,
    HuntAnimals,
    Talk,
}
public class Tutorial : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public string message;
        public TutorialAction requiredAction;
        public int requiredKills;
        public bool isCompleted = false;
    }

    public static Tutorial Instance;

    public TextMeshProUGUI tutorialText;
    public List<TutorialStep> steps = new List<TutorialStep>();

    public GameObject[] tutorialObjects;

    private int currentStepIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (GameManager.Instance.tutorialCompleted == true) return;
        ShowStep(currentStepIndex);
    }

    void Update()
    {
        if (currentStepIndex >= steps.Count) return;

        TutorialStep currentStep = steps[currentStepIndex];

        if (currentStep.isCompleted) return;

        switch (currentStep.requiredAction)
        {
            case TutorialAction.MoveForward:
                if(InputManager.Instance.MoveDirection().y > 0.1f)
                    CompleteCurrentStep();
                break;
            case TutorialAction.MoveBackward:
                if (InputManager.Instance.MoveDirection().y < -0.1f)
                    CompleteCurrentStep();
                break;
            case TutorialAction.MoveRight:
                if (InputManager.Instance.MoveDirection().x > 0.1f)
                    CompleteCurrentStep();
                break;
            case TutorialAction.MoveLeft:
                if (InputManager.Instance.MoveDirection().x < -0.1f)
                    CompleteCurrentStep();
                break;
            case TutorialAction.Jump:
                if (InputManager.Instance.isJumpPressed())
                    CompleteCurrentStep();   
                break;
            case TutorialAction.Attack:
                if (InputManager.Instance.AttackIsPressed())
                    CompleteCurrentStep();
                break;
            case TutorialAction.Interact:
                tutorialObjects[0].SetActive(true);
                break;
        }

    }

    void ShowStep(int index)
    {
        if (index >= steps.Count)
        {
            tutorialText.text = "¡Tutorial completado! ¡Buena suerte!";
            GameManager.Instance.tutorialCompleted = true;
            StartCoroutine(HideTutorialAfterDelay(2f));
            return;
        }

        TutorialStep step = steps[index];
        tutorialText.text = step.message;
        step.isCompleted = false;
    }

    void CompleteCurrentStep()
    {
        steps[currentStepIndex].isCompleted = true;
        currentStepIndex++;
        ShowStep(currentStepIndex);
    }

    public void SkipTutorial()
    {
        StopAllCoroutines();
        currentStepIndex = steps.Count;
        tutorialText.text = "¡Tutorial omitido!";
        GameManager.Instance.tutorialCompleted = true;
        StartCoroutine(HideTutorialAfterDelay(1f));
    }

    IEnumerator HideTutorialAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        tutorialText.gameObject.SetActive(false);

        enabled = false;
    }

    public void AddKill()
    {

    }

    public void CompleteInteraction()
    {
        CompleteCurrentStep();
    }
}
