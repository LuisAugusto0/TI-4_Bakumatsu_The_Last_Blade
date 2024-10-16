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
        // Obtém os cantos do botão em coordenadas de mundo
        Vector3[] buttonCorners = new Vector3[4];
        buttonRect.GetWorldCorners(buttonCorners);

        // Calcula o deslocamento para as espadas com base no tamanho do botão
        Vector3 leftOffset = new Vector3(-(buttonCorners[2].x - buttonCorners[0].x) * offsetFactor, 0, 0);
        Vector3 rightOffset = new Vector3((buttonCorners[2].x - buttonCorners[0].x) * offsetFactor, 0, 0);

        // Converte as coordenadas de tela para coordenadas locais do Canvas
        Vector2 leftLocalPosition;
        Vector2 rightLocalPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, 
            RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, buttonRect.position + leftOffset), 
            canvas.worldCamera, 
            out leftLocalPosition);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, 
            RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, buttonRect.position + rightOffset), 
            canvas.worldCamera, 
            out rightLocalPosition);

        // Atualiza a posição das espadas dentro do Canvas
        leftSwordImage.rectTransform.localPosition = leftLocalPosition;
        rightSwordImage.rectTransform.localPosition = rightLocalPosition;

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
