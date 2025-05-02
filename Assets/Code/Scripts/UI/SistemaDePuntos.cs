using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum que define los diferentes tipos de notas (Correcta, Casi Correcta, Casi Incorrecta, Incorrecta)
public enum NoteHit
{
    Correct,
    AlmostCorrect,
    AlmostIncorrect,
    Incorrect
}

// Enum que clasifica las acciones de karma (Honor y Odio)
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
    // Variables para almacenar los puntos por cada tipo de acción (nota)
    private int totalPuntos;
    private int puntosPorNotaCorrecta;
    private int puntosPorNotaCasiCorrecta;
    private int puntosPorNotaCasiIncorrecta;
    private int puntosPorNotaIncorrecta;
    private int rachaPerfecta = 0; // Para llevar el control de rachas perfectas

    [Header("Sistema de Puntos - Karma")]
    // Variables para almacenar los puntos de karma
    private int puntosHonor;
    private int puntosHonorMenor;
    private int PuntosOdio;
    private int PuntosOdioMenor;
    private int karmaToal;

    void Start()
    {
        // Inicializa los valores predeterminados para los puntos y karma
        totalPuntos = 0;
        puntosPorNotaCorrecta = 50;
        puntosPorNotaCasiCorrecta = 25;
        puntosPorNotaCasiIncorrecta = 10;
        puntosPorNotaIncorrecta = -50;

        karmaToal = 0;
        puntosHonor = 10;
        puntosHonorMenor = 5;
        PuntosOdio = -10;
        PuntosOdioMenor = -5;
    }

    void Update()
    {
        // Simula las acciones de puntos y karma con teclas para pruebas (temporal)
        if (Input.GetKeyDown(KeyCode.Q)) CalcularPuntos(NoteHit.Correct);
        if (Input.GetKeyDown(KeyCode.W)) CalcularPuntos(NoteHit.AlmostCorrect);
        if (Input.GetKeyDown(KeyCode.E)) CalcularPuntos(NoteHit.AlmostIncorrect);
        if (Input.GetKeyDown(KeyCode.R)) CalcularPuntos(NoteHit.Incorrect);

        if (Input.GetKeyDown(KeyCode.Y)) CalcularKarma(KarmaHit.Honor);
        if (Input.GetKeyDown(KeyCode.U)) CalcularKarma(KarmaHit.HonorMenor);
        if (Input.GetKeyDown(KeyCode.I)) CalcularKarma(KarmaHit.Odio);
        if (Input.GetKeyDown(KeyCode.O)) CalcularKarma(KarmaHit.OdioMenor);

        // Mostrar estado de puntos y karma al presionar P
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Total Puntos: " + totalPuntos);
            Debug.Log("Racha Correcta: " + rachaPerfecta);
            Debug.Log("Karma Total: " + karmaToal);
        }
    }

    // Inicializa los puntos por cada tipo de nota (desde la UI o otro sistema)
    public void InicializarSistemaDePuntos(int puntosPorNotaCorrecta, int puntosPorNotaCasiCorrecta, int puntosPorNotaIncorrecta, int puntosPorNotaCasiIncorrecta)
    {
        this.puntosPorNotaCorrecta = puntosPorNotaCorrecta;
        this.puntosPorNotaCasiCorrecta = puntosPorNotaCasiCorrecta;
        this.puntosPorNotaCasiIncorrecta = puntosPorNotaCasiIncorrecta;
        this.puntosPorNotaIncorrecta = puntosPorNotaIncorrecta;
    }

    // Inicializa los puntos de karma (desde la UI o otro sistema)
    public void InicializarSistemaDeKarma(int puntosHonor, int puntosHonorMenor, int PuntosOdio, int PuntosOdioMenor)
    {
        this.puntosHonor = puntosHonor;
        this.puntosHonorMenor = puntosHonorMenor;
        this.PuntosOdio = PuntosOdio;
        this.PuntosOdioMenor = PuntosOdioMenor;
    }

    // Método para agregar puntos al total
    public void AgregarPuntos(int puntos)
    {
        totalPuntos += puntos;
    }

    // Método temporal para agregar karma (se cambiará en futuras versiones)
    public void AgregarKarma(int puntos)
    {
        karmaToal += puntos;

        // Limitar el karma entre -100 y 100
        if (karmaToal > 100)
            karmaToal = 100;
        else if (karmaToal < -100)
            karmaToal = -100;
    }

    // Calcula los puntos según el tipo de nota (Correcta, Casi Correcta, Incorrecta, etc.)
    public void CalcularPuntos(NoteHit nota)
    {
        switch (nota)
        {
            case NoteHit.Correct:
                AgregarPuntos(puntosPorNotaCorrecta);
                rachaPerfecta++;
                break;
            case NoteHit.AlmostCorrect:
                AgregarPuntos(puntosPorNotaCasiCorrecta);
                rachaPerfecta = 0;
                break;
            case NoteHit.AlmostIncorrect:
                AgregarPuntos(puntosPorNotaCasiIncorrecta);
                rachaPerfecta = 0;
                break;
            case NoteHit.Incorrect:
                AgregarPuntos(puntosPorNotaIncorrecta);
                rachaPerfecta = 0;
                break;
        }
    }

    // Calcula el karma según la acción realizada (Honor, Odio, etc.)
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

    // Métodos de acceso para obtener valores
    public int GetKarma() => karmaToal;
    public int TotalPuntos => totalPuntos;
    public int RachaPerfecta => rachaPerfecta;
}
