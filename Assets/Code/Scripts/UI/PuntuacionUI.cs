using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
            textoPuntos.text = "Puntos: " + sistemaDePuntos.TotalPuntos.ToString();
        }
        
        // Verifica que el sistema de puntos y el texto de la racha estén asignados
        if (sistemaDePuntos != null && textoRachaPerfecta != null)
        {
            // Actualiza el texto de la racha perfecta
            textoRachaPerfecta.text = "Racha: " + sistemaDePuntos.RachaPerfecta.ToString();
        }
    }
}
