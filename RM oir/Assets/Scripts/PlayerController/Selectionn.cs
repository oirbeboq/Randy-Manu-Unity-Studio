using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectionn : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material hoverMaterial;
    [SerializeField] private Material defaultMaterial;

    private Transform _selection;
    void Update()
    {
        if(_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
        }
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawRay(transform.position, hit.point, Color.red);
            var selection = hit.transform;
            if (selection.CompareTag(selectableTag))
            {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = hoverMaterial;
                }
                _selection = selection;
            }
                       
        }
    }
}
