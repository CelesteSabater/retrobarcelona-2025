using UnityEngine;
using UnityEngine.UI;

public class BarraSimetrica : MonoBehaviour
{
    public RectTransform barraPositiva;
    public RectTransform barraNegativa;
    [Header("Puntuación Karma - Simetría")]
    public SistemaDePuntos sistemaDePuntos;

    [Range(0f, 1f)]
    public float valor = 0.5f; // 0 = total negativo, 1 = total positivo, 0.5 = centro

    private void Update()
{
    if (sistemaDePuntos == null) return;

    // Normaliza el karma (-100 a 100) => (0 a 1)
    float valor = Mathf.InverseLerp(-100f, 100f, sistemaDePuntos.GetKarma());
    float diferencia = valor - 0.5f;

    if (diferencia > 0)
    {
        barraPositiva.localScale = new Vector3(diferencia * 2f, 1, 1);
        barraNegativa.localScale = new Vector3(0f, 1, 1);
    }
    else if (diferencia < 0)
    {
        barraNegativa.localScale = new Vector3(-diferencia * 2f, 1, 1);
        barraPositiva.localScale = new Vector3(0f, 1, 1);
    }
    else
    {
        barraPositiva.localScale = new Vector3(0f, 1, 1);
        barraNegativa.localScale = new Vector3(0f, 1, 1);
    }
    Debug.Log($"Valor: {valor}, Diferencia: {diferencia}");
}


    // Métodos para simular acciones
    public void BuenaAccion() => valor = Mathf.Clamp01(valor + 0.05f);
    public void MalaAccion() => valor = Mathf.Clamp01(valor - 0.05f);
}
