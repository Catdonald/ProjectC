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
    protected int upgradeLevel = 0;

    [SerializeField] private MeshFilter[] meshFilter;
    [SerializeField] private List<MeshGroup> upgradeMeshes;

    public Vector3 BuyingPosition => transform.TransformPoint(buyingPosition);

    protected void Awake()
    {
        gameObject.SetActive(false);
    }

    public virtual void Upgrade(bool effectOn = true)
    {
        upgradeLevel++;
        if (upgradeLevel > 1)
        {
            for(int i = 0; i < meshFilter.Length; ++i)
            {
                meshFilter[i].mesh = upgradeMeshes[upgradeLevel - 1].meshes[i];
            }

            if (upgradeMeshes[upgradeLevel - 1].deleted)
                upgradeMeshes[upgradeLevel - 1].deleted.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);

            if(gameObject.GetComponent<AudioSource>())
                GameManager.instance.SoundManager.sounds.Add(gameObject.GetComponent<AudioSource>());
        }
        UpgradeStats();

        if (!effectOn)
            return;
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
