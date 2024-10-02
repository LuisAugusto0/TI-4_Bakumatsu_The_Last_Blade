using UnityEngine;
using UnityEngine.UI;

public class BackgroundAnimator : MonoBehaviour
{
    public Image backgroundImage;   // Referência para o componente Image do Canvas
    public Sprite[] frames;         // Array de sprites que contém os frames da animação
    public float frameRate = 0.1f;  // Tempo entre cada frame

    private int currentFrame;
    private float timer;

    void Update()
    {
        // Atualiza o tempo
        timer += Time.deltaTime;

        // Verifica se é hora de trocar de frame
        if (timer >= frameRate)
        {
            // Reseta o timer
            timer = 0f;

            // Muda para o próximo frame
            currentFrame = (currentFrame + 1) % frames.Length;
            backgroundImage.sprite = frames[currentFrame];
        }
    }
}
