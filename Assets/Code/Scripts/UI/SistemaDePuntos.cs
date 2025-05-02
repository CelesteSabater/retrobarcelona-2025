using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum para clasificar el tipo de nota que se toca
public enum NoteHit
{
    Correct,
    AlmostCorrect,
    AlmostIncorrect,
    Incorrect
}

// Enum para clasificar acciones de karma (Temporales)
public enum KarmaHit
{
    Honor,
    HonorMenor,
    Odio,
    OdioMenor
}

public class SistemaDePuntos : MonoBehaviour
{
    [Header("Sistema de Puntos - Planos")]
    private int totalPuntos;
    private int puntosPorNotaCorrecta;
    private int puntosPorNotaCasiCorrecta;
    private int puntosPorNotaCasiInCorrecta;
    private int puntosPorNotaIncorrecta;
    private int rachaCorrecta = 0;

    [Header("Sistema de Puntos - Karma")]
    private int puntosHonor;
    private int puntosHonorMenor;
    private int PuntosOdio;
    private int PuntosOdioMenor;
    private int karmaToal;

    void Start()
    {
        // Puntaje
        totalPuntos = 0;
        puntosPorNotaCorrecta = 50;
        puntosPorNotaCasiCorrecta = 25;
        puntosPorNotaCasiInCorrecta = 10;
        puntosPorNotaIncorrecta = -50;

        // Karma
        karmaToal = 0;
        puntosHonor = 10;
        puntosHonorMenor = 5;
        PuntosOdio = -10;
        PuntosOdioMenor = -5;
    }

    void Update()
    {
        // Simulación de notas (Temporales)
        if (Input.GetKeyDown(KeyCode.Q))
            CalcularPuntos(NoteHit.Correct);
        if (Input.GetKeyDown(KeyCode.W))
            CalcularPuntos(NoteHit.AlmostCorrect);
        if (Input.GetKeyDown(KeyCode.D))
            CalcularPuntos(NoteHit.Incorrect);

        // Simulación de karma (Temporales)
        if (Input.GetKeyDown(KeyCode.Y))
            CalcularKarma(KarmaHit.Honor);
        if (Input.GetKeyDown(KeyCode.U))
            CalcularKarma(KarmaHit.HonorMenor);
        if (Input.GetKeyDown(KeyCode.I))
            CalcularKarma(KarmaHit.Odio);
        if (Input.GetKeyDown(KeyCode.O))
            CalcularKarma(KarmaHit.OdioMenor);

        // Mostrar estado
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Total Puntos: " + totalPuntos);
            Debug.Log("Racha Correcta: " + rachaCorrecta);
            Debug.Log("Karma Total: " + karmaToal);
        }
            
        
    }

    public void InicializarSistemaDePuntos(int puntosPorNotaCorrecta, int puntosPorNotaCasiCorrecta, int puntosPorNotaIncorrecta)
    {
        this.puntosPorNotaCorrecta = puntosPorNotaCorrecta;
        this.puntosPorNotaCasiCorrecta = puntosPorNotaCasiCorrecta;
        this.puntosPorNotaIncorrecta = puntosPorNotaIncorrecta;
    }

    public void InicializarSistemaDeKarma(int puntosHonor, int puntosHonorMenor, int PuntosOdio, int PuntosOdioMenor)
    {
        this.puntosHonor = puntosHonor;
        this.puntosHonorMenor = puntosHonorMenor;
        this.PuntosOdio = PuntosOdio;
        this.PuntosOdioMenor = PuntosOdioMenor;
    }

    public void AgregarPuntos(int puntos)
    {
        totalPuntos += puntos;
    }

// Este método es temporal, se cambiara por la selcceion de "Verosos" de la UI
    public void AgregarKarma(int puntos)
    {
        
        karmaToal += puntos;

        if (karmaToal > 100)
            karmaToal = 100;
        else if (karmaToal < -100)
            karmaToal = -100;
        float valorBarra = (karmaToal + 100f) / 200f;
    }

    public void CalcularPuntos(NoteHit nota)
    {
        switch (nota)
        {
            case NoteHit.Correct:
                AgregarPuntos(puntosPorNotaCorrecta);
                rachaCorrecta++;
                break;
            case NoteHit.AlmostCorrect:
                AgregarPuntos(puntosPorNotaCasiCorrecta);
                rachaCorrecta = 0;
                break;
            case NoteHit.Incorrect:
                AgregarPuntos(puntosPorNotaIncorrecta);
                rachaCorrecta = 0;
                break;
        }
    }
    //Este método es temporal, se cambiara por la selcceion de "Verosos" de la UI
    public void CalcularKarma(KarmaHit karma)
    {
        switch (karma)
        {
            case KarmaHit.Honor:
                AgregarKarma(puntosHonor);
                break;
            case KarmaHit.HonorMenor:
                AgregarKarma(puntosHonorMenor);
                break;
            case KarmaHit.Odio:
                AgregarKarma(PuntosOdio);
                break;
            case KarmaHit.OdioMenor:
                AgregarKarma(PuntosOdioMenor);
                break;
        }
    }
    public int GetKarma() => karmaToal;
    
}
