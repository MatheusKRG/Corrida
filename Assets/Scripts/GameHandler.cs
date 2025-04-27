using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public Text timerText, recordText;
    private float timer = 0f, record;
    private bool timerRunning = false;
   // Start é chamado antes do primeiro frame de atualização
    void Start()
    {
        // Trava o cursor no centro da tela
        Cursor.lockState = CursorLockMode.Locked;
         // Verifica se há um recorde salvo
        if (PlayerPrefs.HasKey("record"))
        {
            record = PlayerPrefs.GetFloat("record");
        }
        else
        {
            record = 0f;
        }
         // Exibe o recorde na tela
        DisplayRecord(record);
    }

     // Update é chamado uma vez por frame
    void Update()
    {
        // Atualiza o cronômetro se ele estiver rodando
        if(timerRunning)
        {
            timer += Time.deltaTime;
            DisplayTime(timer);
        }
        else
        {
            DisplayTime(timer);
        }
    }
    // Exibe o tempo no formato MM:SS:MS
    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milisecs = Mathf.FloorToInt((timeToDisplay * 1000f) % 1000f);

        timerText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milisecs);
    }

    // Inicia o cronômetro
    public void StartTimer()
    {
        timerRunning = true;
    }
    // Para o cronômetro
    public void StopTimer()
    {
        timerRunning = false;
        // Salva o recorde se o tempo atual for menor que o recorde ou se não houver recorde
        if(timer < record || record == 0f)
        {
            record = timer;
            PlayerPrefs.SetFloat("record", record);
        }
         // Exibe o recorde atualizado
        DisplayRecord(record);
    }
      // Exibe o recorde no formato MM:SS:MS
    private void DisplayRecord(float _record)
    {
        float minutes = Mathf.FloorToInt(_record / 60);
        float seconds = Mathf.FloorToInt(_record % 60);
        float milisecs = Mathf.FloorToInt((_record * 1000f) % 1000f);

        recordText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milisecs);
    }
}