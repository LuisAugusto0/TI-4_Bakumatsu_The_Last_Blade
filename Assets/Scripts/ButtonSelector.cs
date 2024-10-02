using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image leftSwordImage;     // Imagem da espada esquerda
    public Image rightSwordImage;    // Imagem da espada direita
    public float offsetFactor = 0.1f;  // Fator de offset relativo ao tamanho do botão

    private RectTransform buttonRect;  // RectTransform do botão
    private Canvas canvas;             // Referência ao Canvas

    void Start()
    {
        // Obtém o RectTransform do botão e a referência ao Canvas
        buttonRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Obtém a posição do botão em coordenadas de tela
        Vector3[] buttonCorners = new Vector3[4];
        buttonRect.GetWorldCorners(buttonCorners);

        // Calcula a posição relativa das espadas baseado no tamanho do botão e na escala do Canvas
        Vector3 leftOffset = new Vector3(-(buttonCorners[2].x - buttonCorners[0].x) * offsetFactor, 0, 0);
        Vector3 rightOffset = new Vector3((buttonCorners[2].x - buttonCorners[0].x) * offsetFactor, 0, 0);

        // Converte as coordenadas do mundo para coordenadas de tela, levando em conta o Canvas
        Vector2 leftScreenPosition = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, buttonRect.position + leftOffset);
        Vector2 rightScreenPosition = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, buttonRect.position + rightOffset);

        // Atualiza a posição das espadas
        leftSwordImage.transform.position = leftScreenPosition;
        rightSwordImage.transform.position = rightScreenPosition;

        // Ativa as espadas
        leftSwordImage.enabled = true;
        rightSwordImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Desativa as espadas quando o mouse sai do botão
        leftSwordImage.enabled = false;
        rightSwordImage.enabled = false;
    }
}
