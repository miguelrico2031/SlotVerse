using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]

public class LimitHandler : MonoBehaviour
{
    public float threshold = 100.0f;
    public RoadSpawner roadGenerator;

    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = 0f;

        if (cameraPosition.magnitude > threshold)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                foreach (GameObject g in SceneManager.GetSceneAt(i).GetRootGameObjects())
                {
                    g.transform.position -= cameraPosition;
                }
            }

            Vector3 originDelta = Vector3.zero - cameraPosition;
            //roadGenerator.UpdateSpawnOrigin(originDelta);
            Debug.Log("resetting. origin delta = " + originDelta);
        }
    }
}