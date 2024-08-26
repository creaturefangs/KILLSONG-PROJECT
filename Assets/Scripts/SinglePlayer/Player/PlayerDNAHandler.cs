using System;
using UnityEngine;
public class PlayerDNAHandler : MonoBehaviour
{
    private PlayerUI _playerUI;
    public int totalDna;

    private void Awake()
    {
        _playerUI = FindObjectOfType<PlayerUI>();
    }

    private void Start()
    {
        totalDna = 0;
    }

    public void IncreaseDna(int numToIncrease)
    {
        totalDna += numToIncrease;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _playerUI.dnaCountText.text = "DNA: " + totalDna;
    }
}