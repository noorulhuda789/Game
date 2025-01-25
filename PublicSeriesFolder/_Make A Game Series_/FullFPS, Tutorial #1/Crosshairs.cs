using UnityEngine;

public class Crosshairs : MonoBehaviour
{
    [Header("Main Crosshair")]
    public Texture2D normalCrosshair;
    public float normalScale = 1f;
    [Space]
    [Header("Interactable Crosshair")]
    public Texture2D interactableCrosshair; 
    public float interactableScale = 1.5f;
    [Space]
    [Header("Interactable Crosshair Triggers")]
    public LayerMask interactableLayerMask; 

    private GUIStyle crosshairStyle = new GUIStyle();
    private bool showInteractableCrosshair = false;
    private RaycastHit hit;

    void OnGUI()
    {
        crosshairStyle.normal.background = showInteractableCrosshair ? interactableCrosshair : normalCrosshair;
        float scale = showInteractableCrosshair ? interactableScale : normalScale;
        Vector2 pivotPoint = new Vector2(crosshairStyle.normal.background.width / 2, crosshairStyle.normal.background.height / 2);
        Vector2 position = new Vector2(Screen.width / 2 - pivotPoint.x * scale, Screen.height / 2 - pivotPoint.y * scale);

        GUI.DrawTexture(new Rect(position.x, position.y, crosshairStyle.normal.background.width * scale, crosshairStyle.normal.background.height * scale), crosshairStyle.normal.background);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out hit, 4f, interactableLayerMask))
        {
            showInteractableCrosshair = true;
        }
        else
        {
            showInteractableCrosshair = false;
        }
    }
}
