using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PuntuacionUI : MonoBehaviour
{
    public SistemaDePuntos sistemaDePuntos;
    public TextMeshProUGUI textoPuntos;
    public TextMeshProUGUI textoRachaPerfecta;

    void Update()
    {
        if (sistemaDePuntos != null && textoPuntos != null)
        {
            textoPuntos.text = "Puntos: " + sistemaDePuntos.TotalPuntos.ToString();
        }
        if(sistemaDePuntos != null && textoRachaPerfecta != null)
        {
            textoRachaPerfecta.text = "Racha: " + sistemaDePuntos.RachaPerfecta.ToString();
        }
    }
}