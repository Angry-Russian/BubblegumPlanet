using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Scripts
{
    public class Wave : MonoBehaviour
    {
        float scale = 1f;
        float speed = 1.0f;
        float noiseStrength = 1f;
        float noiseWalk = 1f;
        bool isWaving = false;
        int indexMinVertice = 0;
        int indexMaxVertice = 0;
        private Vector3[] baseHeight;

        void Update()
        {
            if (isWaving)
            {
                Mesh mesh = GetComponent<MeshFilter>().mesh;

                if (baseHeight == null)
                    baseHeight = mesh.vertices;

                Vector3[] vertices = new Vector3[baseHeight.Length];

                for (int i = indexMaxVertice; i < vertices.Length; i++)
                {
                    vertices[i] = calculeVerticeWave(baseHeight[i]);
                }
                indexMaxVertice++;

                for (int i = indexMinVertice; i >= 0; i--)
                {
                    vertices[i] = calculeVerticeWave(baseHeight[i]);
                }
                indexMinVertice--;

                mesh.vertices = vertices;
                mesh.RecalculateNormals();

                //GetComponent<MeshCollider>().sharedMesh = mesh;

                if (indexMinVertice <= 0 && indexMaxVertice > baseHeight.Length)
                    isWaving = false;
            }
        }

        public Vector3 calculeVerticeWave(Vector3 currentVertex)
        {
            Vector3 vertex = currentVertex;

            vertex.y += Mathf.Sin(Time.time * speed + currentVertex.x + currentVertex.y + currentVertex.z) * scale;
            vertex.y += Mathf.PerlinNoise(currentVertex.x + noiseWalk, currentVertex.y + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;

            return vertex;
        }
        public void makeWave(Transform transformPlayer)
        {
            Mesh mesh = GetComponent<MeshFilter>().mesh;

            if (baseHeight == null)
                baseHeight = mesh.vertices;

            Vector3[] vertices = new Vector3[baseHeight.Length];
            float closeDistance = 100;
            int indexCloseVertice = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                float distance = Vector3.Distance(transformPlayer.position, transform.TransformPoint(vertices[i]));
                if (distance < closeDistance)
                {
                    closeDistance = distance;
                    indexCloseVertice = i;
                }
            }
            Vector3 currentVertex = baseHeight[indexCloseVertice];
            Vector3 vertex = currentVertex;

            vertex.y += Mathf.Sin(Time.time * speed + currentVertex.x + currentVertex.y + currentVertex.z) * scale;
            vertex.y += Mathf.PerlinNoise(currentVertex.x + noiseWalk, currentVertex.y + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;

            vertices[indexCloseVertice] = vertex;
            mesh.vertices = vertices;
            mesh.RecalculateNormals();

// GetComponent<MeshCollider>().sharedMesh = mesh;

            isWaving = true;
            indexMinVertice = indexCloseVertice;
            indexMaxVertice = 0;
        }
    }
}

