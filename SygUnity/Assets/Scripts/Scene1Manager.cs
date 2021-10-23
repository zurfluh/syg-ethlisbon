using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene1Manager : MonoBehaviour
{
    public Text planetName;
    public Text planetOwner;
    public GameObject selected;
    private string selectedName;
    public Vector3[] sizes;
    
    void Awake()
    {
        selectedName = "";
        sizes = new Vector3[10];
        for (int i = 0; i < sizes.Length; i++)
        {
            float f = 0.25f + i * 0.07f;
            sizes[i] = new Vector3(f, f, f);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.name != selectedName) {
                if (selected != null) {
                    selected.transform.GetChild(0).gameObject.SetActive(false);
                }
                selected = hit.collider.gameObject;
                selectedName = selected.name;
                Transform child = selected.transform.GetChild(0);
                child.gameObject.SetActive(true);

                PlanetManager pm = selected.gameObject.GetComponent<PlanetManager>();
                float eth = pm.GetStakedEth();
                
                // UI Texts
                planetOwner.text = pm.GetOwnerAddress();
                planetName.text = selectedName;

                // Size
                int index = Mathf.FloorToInt(eth);
                child.transform.localScale = sizes[index];

                // Color
                SpriteRenderer sr = child.gameObject.GetComponent<SpriteRenderer>();
                float r = 0.3f + 0.03f * index;
                float g = 0.1f * index;
                float b = 0.2f + 0.06f * index;
                Color color = new Color(r, g, b);
                sr.color = color;
            }
        }
    }
}
