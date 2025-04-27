using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Carro : MonoBehaviour
{
    public WheelCollider[] guiar;

    float gui = 0f;
    float acc = 0f;
    Rigidbody rb;

    float velKMH;
    float rpm;

    public float[] racioMudancas;
    int mudancaAtual = 0;

    public float maxRPM;
    public float minRPM;

    public float forca;

    private PlayerInput playerInput;
    private InputAction moveAction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Configurações do Input System
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            // Atualize o nome da ação para "Move"
            moveAction = playerInput.actions["Move"];

            if (moveAction == null)
            {
                Debug.LogError("A ação 'Move' não foi encontrada no Input Actions Asset.");
            }
        }
        else
        {
            Debug.LogError("PlayerInput não encontrado no GameObject.");
        }
    }

    void Update()
    {
        // Verifica se a ação foi inicializada
        if (moveAction != null)
        {
            // Captura os inputs do jogador usando o Input System
            Vector2 moveInput = moveAction.ReadValue<Vector2>();
            gui = moveInput.x; // Eixo X para direção
            acc = moveInput.y; // Eixo Y para aceleração
        }
        else
        {
            Debug.LogWarning("A ação 'Move' não foi inicializada.");
        }
    }

    void FixedUpdate()
    {
        // Controle da direção e torque das rodas
        for (int i = 0; i < guiar.Length; i++)
        {
            guiar[i].steerAngle = gui * 15f; // Ângulo de direção
            guiar[i].motorTorque = forca * acc; // Torque do motor
        }

        // Calcula a velocidade em KM/H
        velKMH = rb.linearVelocity.magnitude * 3.6f;

        // Calcula o RPM
        if (mudancaAtual < racioMudancas.Length)
        {
            rpm = velKMH * racioMudancas[mudancaAtual] * 15f;
        }

        // Lógica de troca de marchas
        if (rpm > maxRPM && mudancaAtual < racioMudancas.Length - 1)
        {
            mudancaAtual++;
        }
        else if (rpm < minRPM && mudancaAtual > 0)
        {
            mudancaAtual--;
        }

        // Adiciona força ao carro
        rb.AddForce(transform.forward * forca * acc, ForceMode.Force);
    }

    void OnGUI()
    {
        // Exibe informações na tela
        GUI.Label(new Rect(20, 20, 128, 32), rpm.ToString("F0") + " RPM");
        GUI.Label(new Rect(20, 50, 128, 32), "Marcha: " + (mudancaAtual + 1).ToString());
        GUI.Label(new Rect(20, 80, 128, 32), velKMH.ToString("F1") + " KM/H");
    }
}