using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AnimationSync : MonoBehaviour
{
    // Variáveis públicas para definir o intervalo de randomização
    public float minSpeed = 0.5f;  // Velocidade mínima da animação
    public float maxSpeed = 2.0f;  // Velocidade máxima da animação
    public AnimatedTile[] tilesToSync;  // Array de tiles animados que vão compartilhar a mesma velocidade
    public float updateInterval = 1f;  // Intervalo de tempo (em segundos) para atualizar a velocidade

    private float animationSpeed;  // Velocidade calculada para os tiles

    void Start()
    {
        // Inicia uma corrotina para atualizar a velocidade periodicamente
        StartCoroutine(UpdateAnimationSpeed());
    }

    // Corrotina que atualiza a velocidade de animação a cada intervalo
    private IEnumerator UpdateAnimationSpeed()
    {
        while (true)  // Loop infinito (até o GameObject ser destruído)
        {
            // Randomiza a velocidade da animação no intervalo
            animationSpeed = Random.Range(minSpeed, maxSpeed);

            // Aplica a mesma velocidade em todos os AnimatedTiles selecionados
            foreach (AnimatedTile tile in tilesToSync)
            {
                SetTileAnimationSpeed(tile, animationSpeed);
            }

            // Aguarda pelo intervalo definido antes de atualizar novamente
            yield return new WaitForSeconds(updateInterval);
        }
    }

    // Função para definir a velocidade de animação do AnimatedTile
    void SetTileAnimationSpeed(AnimatedTile tile, float speed)
    {
        // Ajusta a velocidade de animação do AnimatedTile
        tile.m_MaxSpeed = speed;
        tile.m_MinSpeed = speed;
    }
}
