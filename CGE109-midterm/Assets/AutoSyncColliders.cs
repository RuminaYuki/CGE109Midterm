using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class AutoSyncColliders : MonoBehaviour
{
    private Dictionary<Collider, Collider> syncedColliders = new Dictionary<Collider, Collider>();

    void Start()
    {
        SyncColliders();
        InvokeRepeating(nameof(SyncColliders), 0.5f, 0.5f); // เช็กทุก 0.5 วิ
    }

    void SyncColliders()
    {
        // หา Collider ทั้งหมดของลูก
        Collider[] childColliders = GetComponentsInChildren<Collider>(true);

        // ลบตัวเองออก (ถ้ามี collider ที่พ่ออยู่)
        List<Collider> children = new List<Collider>();
        foreach (var col in childColliders)
            if (col.transform != transform)
                children.Add(col);

        // 1️⃣ เพิ่ม collider ที่ยังไม่มีใน dictionary
        foreach (var childCol in children)
        {
            if (!syncedColliders.ContainsKey(childCol))
            {
                Collider newCol = CopyCollider(childCol);
                syncedColliders.Add(childCol, newCol);
            }
        }

        // 2️⃣ ลบ collider ที่หายไป
        List<Collider> removed = new List<Collider>();
        foreach (var pair in syncedColliders)
        {
            if (pair.Key == null || !children.Contains(pair.Key))
            {
                if (pair.Value != null)
                    Destroy(pair.Value);
                removed.Add(pair.Key);
            }
        }

        foreach (var r in removed)
            syncedColliders.Remove(r);
    }

    Collider CopyCollider(Collider source)
    {
        System.Type type = source.GetType();
        Collider newCol = gameObject.AddComponent(type) as Collider;

        // ก๊อบค่าพื้นฐานตามชนิด collider
        if (source is BoxCollider oldBox && newCol is BoxCollider newBox)
        {
            newBox.center = transform.InverseTransformPoint(oldBox.transform.TransformPoint(oldBox.center));
            newBox.size = oldBox.size;
            newBox.isTrigger = oldBox.isTrigger;
        }
        else if (source is SphereCollider oldSphere && newCol is SphereCollider newSphere)
        {
            newSphere.center = transform.InverseTransformPoint(oldSphere.transform.TransformPoint(oldSphere.center));
            newSphere.radius = oldSphere.radius;
            newSphere.isTrigger = oldSphere.isTrigger;
        }
        else if (source is CapsuleCollider oldCap && newCol is CapsuleCollider newCap)
        {
            newCap.center = transform.InverseTransformPoint(oldCap.transform.TransformPoint(oldCap.center));
            newCap.radius = oldCap.radius;
            newCap.height = oldCap.height;
            newCap.direction = oldCap.direction;
            newCap.isTrigger = oldCap.isTrigger;
        }
        else if (source is MeshCollider oldMesh && newCol is MeshCollider newMesh)
        {
            newMesh.sharedMesh = oldMesh.sharedMesh;
            newMesh.convex = oldMesh.convex;
            newMesh.isTrigger = oldMesh.isTrigger;
        }

        return newCol;
    }
}
