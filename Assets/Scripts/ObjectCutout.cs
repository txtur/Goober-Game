using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCutout : MonoBehaviour
{
    [SerializeField] static int PosID = Shader.PropertyToID("_PlayerPos");
    [SerializeField] static int SizeID = Shader.PropertyToID("_Size");

    [SerializeField] Material mat;
    [SerializeField] Camera cam;
    [SerializeField] LayerMask mask;

    void Update()
    {
        Vector3 dir = cam.transform.position - transform.position;
        Ray ray = new Ray(transform.position, dir.normalized);

        if(Physics.Raycast(ray, 3000, mask)) 
            mat.SetFloat(SizeID, 1);
        else 
            mat.SetFloat(SizeID, 0);

        Vector3 view = cam.WorldToViewportPoint(transform.position);
        mat.SetVector(PosID, view);
    }
}
