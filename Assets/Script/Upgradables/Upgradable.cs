using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgradable : MonoBehaviour
{   
    [SerializeField] private Vector3 buyingPosition = Vector3.zero;
    protected int upgradeLevel = 0;

    //private List<UpgradeMesh> upgradeMeshes;
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
            // change mesh
        }
        else
        {
            gameObject.SetActive(true);
        }
        UpgradeStats();
        //unlock effect on
        if (!effectOn)
            return;
    }

    protected virtual void UpgradeStats() { }

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
