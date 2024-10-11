using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Upgradable;

[System.Serializable]
public class MeshGroup
{
    public List<Mesh> meshes; // Mesh
    public GameObject deleted;
}

public class Upgradable : MonoBehaviour
{
    [SerializeField] private Vector3 buyingPosition = Vector3.zero;
    [SerializeField] private Vector3 punchScale = new Vector3(0.1f, 0.2f, 0.1f);
    protected int upgradeLevel = 0;

    [SerializeField] private MeshFilter[] meshFilter;
    [SerializeField] private List<MeshGroup> upgradeMeshes;

    public Vector3 BuyingPosition => transform.TransformPoint(buyingPosition);

    protected void Awake()
    {
        gameObject.SetActive(false);
    }

    public virtual void Upgrade(bool animate = true)
    {
        upgradeLevel++;
        if (upgradeLevel > 1)
        {
            for(int i = 0; i < meshFilter.Length; ++i)
            {
                meshFilter[i].mesh = upgradeMeshes[upgradeLevel - 2].meshes[i];
            }
            
            if (upgradeMeshes[upgradeLevel - 2].deleted)
                upgradeMeshes[upgradeLevel - 2].deleted.SetActive(false);

        }
        else
        {
            gameObject.SetActive(true);

            if(gameObject.GetComponent<AudioSource>())
                GameManager.instance.SoundManager.sounds_3d.Add(gameObject.GetComponent<AudioSource>());
        }
        UpgradeStats();

        if (!animate)
            return;

        transform.DOPunchScale(punchScale, 0.3f).OnComplete(()=> transform.localScale = Vector3.one);    
    }

    public virtual void UpgradeStats() { }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Vector3 center = BuyingPosition;
        Vector3 size = new Vector3(2.0f, 0.2f, 2.0f);
        Gizmos.DrawCube(center, size);

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;
        UnityEditor.Handles.Label(center + Vector3.up * 0.5f, "Buying\nPoint", style);
    }
#endif
}
