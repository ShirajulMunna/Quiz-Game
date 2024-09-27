using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionHandler : MonoBehaviour
{
    public Toggle[] togles;
    public TMP_InputField[] inputFields;
    public InputDataSO[] inputData;
    public EventChannelSO loadImage;
    [SerializeField] private Image questionSlot;
    private int currentImageIndex = 0;
    public Button next;
    public int questionCounter,questionThreshHold;
    public TextMeshProUGUI questionCounterTxt;

    void Start()
    {
        questionCounter = 1;
        questionCounterTxt.text=questionCounter.ToString();

        for (int i = 0; i < togles.Length; i++) 
        {
            togles[i].isOn = false;
        }
       
        for (int i =0 ; i < inputFields.Length; i++)
        {
            int index = i;  
            inputFields[i].onValueChanged.AddListener(delegate { OnInputFieldValueChanged(index); });
        }

        for (int i = 0; i < togles.Length; i++)
        {
            int index = i;
            togles[i].onValueChanged.AddListener(delegate { OnTogleValueChanged(index); });
        }

        next.onClick.AddListener(LoadNextImage);

        for (int i = 0; i < inputData.Length; i++) 
        {
            LoadInputFieldsData(i);


        }


    }

    private void OnEnable()
    {
       
        loadImage.OnEventRaise += QuestionImageLoad;
    }

    private void OnDisable()
    {
        
        loadImage.OnEventRaise -= QuestionImageLoad;

    }

    public void QuestionImageLoad() 
    {
        Debug.Log("Image loaded");
        LoadImage(currentImageIndex);

    }

    private void LoadImage(int index)
    {
        

        if (questionCounter < Manager.Instance.questionThresh)
        {
            Debug.Log("QT" + questionThreshHold);
            questionCounter++;
            questionCounterTxt.text = questionCounter.ToString();
            questionSlot.sprite = Manager.Instance.imageList[index];

        }
        else 
        {
            next.GetComponent<Button>().interactable = false;
        
        }
             
    }

    public void LoadNextImage()
    {
        currentImageIndex++;

        
        if (currentImageIndex >=Manager.Instance.imageList.Count)
        {
            currentImageIndex = 0;  
        }

        
        LoadImage(currentImageIndex);

        for (int i = 0; i < inputFields.Length; i++) 
        {
            inputFields[i].text = inputData[currentImageIndex].answers[i];
        
        }

        for (int i = 0; i < togles.Length; i++)
        {
            togles[i].isOn = false;

        }



    }

    public void InitialQuestion() 
    {
        for (int i = 0; i < inputFields.Length; i++)
        {
            inputFields[i].text = inputData[0].answers[i];

        }

    }

    void OnInputFieldValueChanged(int inputFieldIndex)
    {
      
        string currentInput = inputFields[inputFieldIndex].text;
        inputData[currentImageIndex].answers[inputFieldIndex] = currentInput;
        SaveInputFieldsData(currentImageIndex);





    }

    void SaveInputFieldsData(int setIndex)
    {
        // Save the text for three input fields in the current set (identified by setIndex)
        string inputField1Text = inputFields[0].text;
        string inputField2Text = inputFields[1].text;
        string inputField3Text = inputFields[2].text;

        // Save each input field in PlayerPrefs using a unique key for each set
        PlayerPrefs.SetString($"Set_{setIndex}_InputField1", inputField1Text);
        PlayerPrefs.SetString($"Set_{setIndex}_InputField2", inputField2Text);
        PlayerPrefs.SetString($"Set_{setIndex}_InputField3", inputField3Text);

        // Save PlayerPrefs
        PlayerPrefs.Save();

        Debug.Log($"Set {setIndex} input fields data saved!");
    }
    void LoadInputFieldsData(int setIndex)
    {
        // Load the text for three input fields in the current set (identified by setIndex)
        if (PlayerPrefs.HasKey($"Set_{setIndex}_InputField1"))
        {
          inputData[setIndex].answers[0] = PlayerPrefs.GetString($"Set_{setIndex}_InputField1");
        }

        if (PlayerPrefs.HasKey($"Set_{setIndex}_InputField2"))
        {
            inputData[setIndex].answers[1] = PlayerPrefs.GetString($"Set_{setIndex}_InputField2");
        }

        if (PlayerPrefs.HasKey($"Set_{setIndex}_InputField3"))
        {
            inputData[setIndex].answers[2] = PlayerPrefs.GetString($"Set_{setIndex}_InputField3");
        }

        Debug.Log($"Set {setIndex} input fields data loaded!");
    }

    void OnTogleValueChanged(int index) 
    {
        Debug.Log("Togle value" + index);
        if (togles[index].isOn) 
        {
            inputData[currentImageIndex].rightAnswer = index;

        }
    }



}
