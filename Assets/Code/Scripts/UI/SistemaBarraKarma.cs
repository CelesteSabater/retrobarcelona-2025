using UnityEngine;
using UnityEngine.UI;

namespace retrobarcelona.UI
{
    public class BarraSimetrica : MonoBehaviour
    {
        public RectTransform barraPositiva; // Barra azul (positiva)
        public RectTransform barraNegativa; // Barra roja (negativa)

        [Header("Puntuación Karma - Simetría")]
        public SistemaDePuntos sistemaDePuntos;

        [Range(0f, 1f)]
        public float valor = 0.5f;

        private void Update()
        {
            if (sistemaDePuntos == null) return;

            // Normaliza el karma entre -100 y 100 a un valor entre 0 y 1
            valor = Mathf.InverseLerp(-100f, 100f, sistemaDePuntos.GetKarma());

            // Cálculo de diferencia respecto al centro (0.5)
            float diferencia = valor - 0.5f;

            if (valor > 0.5f)
            {
                // Karma positivo: barra azul (positiva) crece hacia la derecha
                barraPositiva.localScale = new Vector3((valor - 0.5f) * 2f, 1f, 1f);
                barraNegativa.localScale = new Vector3(0f, 1f, 1f); // Barra roja (negativa) se oculta
            }
            else if (valor < 0.5f)
            {
                // Karma negativo: barra roja (negativa) crece hacia la izquierda
                barraNegativa.localScale = new Vector3((0.5f - valor) * 2f, 1f, 1f);
                barraPositiva.localScale = new Vector3(0f, 1f, 1f); // Barra azul (positiva) se oculta
            }
            else
            {
                // Cuando el valor está en el centro (karma neutro)
                barraPositiva.localScale = new Vector3(0f, 1f, 1f);
                barraNegativa.localScale = new Vector3(0f, 1f, 1f);
            }
        }

        // Métodos para simular acciones
        public void BuenaAccion() => valor = Mathf.Clamp01(valor + 0.05f);
        public void MalaAccion() => valor = Mathf.Clamp01(valor - 0.05f);
    }
}