using System.Collections;
using System.Collections.Generic;
using retrobarcelona.Utils.Singleton;
using UnityEngine;
using retrobarcelona.MusicSystem;

namespace retrobarcelona.UI
{
    public class SistemaDePuntos : Singleton<SistemaDePuntos>
    {
        [Header("Sistema de Puntos - Planos")]
        // Variables para almacenar los puntos por cada tipo de acción (nota)
        private float totalPuntos;
        private float puntosPorNotaCorrecta;
        private float puntosPorNotaCasiCorrecta;
        private float puntosPorNotaCasiIncorrecta;
        private float puntosPorNotaIncorrecta;
        private int rachaPerfecta = 0; // Para llevar el control de rachas perfectas

        [Header("Sistema de Puntos - Karma")]
        // Variables para almacenar los puntos de karma
        private int karmaTotal;

        void Start()
        {
            // Inicializa los valores predeterminados para los puntos y karma
            totalPuntos = 0;
            puntosPorNotaCorrecta = 50;
            puntosPorNotaCasiCorrecta = 25;
            puntosPorNotaCasiIncorrecta = 10;
            puntosPorNotaIncorrecta = -50;

            karmaTotal = 0;
        }

        // Inicializa los puntos por cada tipo de nota (desde la UI o otro sistema)
        public void InitializeSong(float puntosPorNotaCorrecta, int karma)
        {
            this.puntosPorNotaCorrecta = puntosPorNotaCorrecta;
            this.puntosPorNotaCasiCorrecta = puntosPorNotaCorrecta / 2;
            this.puntosPorNotaCasiIncorrecta = puntosPorNotaCorrecta / 3;
            this.puntosPorNotaIncorrecta = -puntosPorNotaCorrecta;

            AgregarKarma(karma);
        }

        // Método para agregar puntos al total
        public void AgregarPuntos(float puntos)
        {
            totalPuntos += puntos;
        }

        // Método temporal para agregar karma (se cambiará en futuras versiones)1
        public void AgregarKarma(int puntos)
        {
            karmaTotal += puntos;

            // Limitar el karma entre -100 y 100
            if (karmaTotal > 100)
                karmaTotal = 100;
            else if (karmaTotal < -100)
                karmaTotal = -100;
        }

        // Calcula los puntos según el tipo de nota (Correcta, Casi Correcta, Incorrecta, etc.)
        public void CalcularPuntos(retrobarcelona.MusicSystem.NoteHitTiming nota)
        {
            switch (nota)
            {
                case NoteHitTiming.Correct:
                    AgregarPuntos(puntosPorNotaCorrecta);
                    rachaPerfecta++;
                    break;
                case NoteHitTiming.AlmostCorrect:
                    AgregarPuntos(puntosPorNotaCasiCorrecta);
                    rachaPerfecta = 0;
                    break;
                case NoteHitTiming.AlmostIncorrect:
                    AgregarPuntos(puntosPorNotaCasiIncorrecta);
                    rachaPerfecta = 0;
                    break;
                case NoteHitTiming.Incorrect:
                    AgregarPuntos(puntosPorNotaIncorrecta);
                    rachaPerfecta = 0;
                    break;
            }
        }

        // Métodos de acceso para obtener valores
        public int GetKarma() => karmaTotal;
        public float TotalPuntos => totalPuntos;
        public int RachaPerfecta => rachaPerfecta;
    }
}