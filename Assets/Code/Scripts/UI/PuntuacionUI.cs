using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace retrobarcelona.UI
{
    public class PuntuacionUI : MonoBehaviour
    {
        // Referencia al sistema de puntos que gestiona la puntuación total y la racha perfecta
        public SistemaDePuntos sistemaDePuntos;

        // Referencias a los componentes de texto (TextMeshPro) para mostrar los puntos y la racha perfecta
        public TextMeshProUGUI textoPuntos;
        public TextMeshProUGUI textoRachaPerfecta;

        void Update()
        {
            // Verifica que el sistema de puntos y los textos estén asignados
            if (sistemaDePuntos != null && textoPuntos != null)
            {
                // Actualiza el texto de los puntos totales
                int points = (int) sistemaDePuntos.TotalPuntos();
                textoPuntos.text = points.ToString("D4");
            }
            
            // Verifica que el sistema de puntos y el texto de la racha estén asignados
            if (sistemaDePuntos != null && textoRachaPerfecta != null)
            {
                // Actualiza el texto de la racha perfecta
                textoRachaPerfecta.text = "x"+sistemaDePuntos.Racha().ToString("D2");
            }
        }
    }
}