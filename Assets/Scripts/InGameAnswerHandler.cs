using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InGameAnswerHandler : MonoBehaviour
{
    public static InGameAnswerHandler Instance;
    public TextMeshProUGUI[] text;
    public InputDataSO[] inputData;
    public Button[] answerButtons;
    [SerializeField] public EventChannelSO eventChannleSO;
    public int currentQuestionIndex;
    public Image[] resultBanner;
    public Image blackBg;
    public EventChannelSO questionFinished;
    public GameObject mainResultPanel,giftPanel,sorryPanel;
    public int score;
    public AudioChannelSO audioChannelSO;
    public bool isButtonPressed;
    public int questionCounter;
    public TextMeshProUGUI questionCounterTxt;


    private void Start()
    {
        Instance = this;
        currentQuestionIndex = Manager.Instance.currentImageIndex;
        questionCounter = 1;
        questionCounterTxt.text = questionCounter.ToString();


        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].onClick.AddListener(delegate { CheckAnswer(index); });
        }
    }

    private void OnEnable() 
    {
        eventChannleSO.OnEventRaise += AnswerHandle;
        questionFinished.OnEventRaise += TriggerMainResultPanel;
    }

    private void OnDisable()
    {
        eventChannleSO.OnEventRaise -= AnswerHandle;
        questionFinished.OnEventRaise -= TriggerMainResultPanel;

    }


    public void AnswerHandle()
    {
        
        text[0].text = inputData[currentQuestionIndex].answers[0];
        text[1].text = inputData[currentQuestionIndex].answers[1];
        text[2].text = inputData[currentQuestionIndex].answers[2];

    }

    public void CheckAnswer(int index) 
    {
        TimeManager.Instacne.isCountDownStart = false;
        int winningNumber = inputData[currentQuestionIndex].rightAnswer;


        if (winningNumber == index)
        {
            Debug.Log("win" + index);
            
            ResultBanner(0);
            score++;

        }
        else 
        {
       
            ResultBanner(1);

        }

    }

    public void ResultBanner(int index) 
    {

        blackBg.gameObject.SetActive(true);
        resultBanner[index].gameObject.SetActive(true);

        StartCoroutine(LoadNextQuestion(index));

    }

    IEnumerator LoadNextQuestion(int index) 
    {
       


        if (Manager.Instance.currentImageIndex != Manager.Instance.questionThresh-1 )
        {
            yield return new WaitForSeconds(0.5f);
            blackBg.gameObject.SetActive(false);
            resultBanner[index].gameObject.SetActive(false);

            Manager.Instance.LoadNextImage();
            questionCounter++;
            questionCounterTxt.text = questionCounter.ToString();
            TimeManager.Instacne.isCountDownStart = true;
            TimeManager.Instacne.StartTime();

        }

        else 
        {
            yield return new WaitForSeconds(3);
            questionFinished.RaiseEvent();
        
        }
       

       
    
    }

    public void TriggerMainResultPanel() 
    {
        Debug.Log("Open Gift/ Fail Panel");
        mainResultPanel.SetActive(true);

       // giftPanel.SetActive(true);
        Manager.Instance.gameStat.SetActive(true);
        Manager.Instance.OpenStatBox();
        
       
    
    }

    
}
