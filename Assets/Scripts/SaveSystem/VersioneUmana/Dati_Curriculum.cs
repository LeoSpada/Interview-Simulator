using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "Curriculum", menuName = "Curriculum/Crea un nuovo curriculum")]
public class Dati_Curriculum : ScriptableObject
{
    [Header("Informazioni Personali")]
    [SerializeField] public TMP_InputField inputNome;

    [Header("Occupazione desiderata")]
    [SerializeField] public TMP_InputField amogus1;

    [Header("Esperienze Personali")]
    [SerializeField] public TMP_InputField amogus2;

    [Header("Istruzione e Formazione")]
    [SerializeField] public TMP_InputField amogus3;

    [Header("Capacità e Competenze Personali")]
    [SerializeField] Lingua Madrelingua;
    [SerializeField] Lingua AltreLingue;
    [SerializeField] Patente Patente;

    //Proprietà
    public TMP_InputField InputName { get { return inputNome; } }
}

public enum Lingua { Nessuno, Italiano, Inglese, Francese, Tedesco, Spagnolo, Portoghese }

public enum Patente { A, A1, A2, B, C, D, E }
