using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ErksUnityLibrary;

namespace LudumDare50
{
    public class WorldGenerator : MonoBehaviour
    {
        public Vector2 bottomLeftCorner;
        public Vector2 topRightCorner;

        public float maxScale = 1.2f;

        public int treesAmount = 128;
        public int rocksAmount = 64;
        public int grassAmount = 128;

        public List<Transform> treesPrefabs;
        public List<Transform> rocksPrefabs;
        public List<Transform> grassPrefabs;

        public Transform treesHolder;
        public Transform rocksHolder;
        public Transform grassHolder;

        [ContextMenu("LevelGen/Create all")]
        public void CreateAll()
        {
            CreateTrees();
            CreateRocks();
            CreateGrass();
        }

        [ContextMenu("LevelGen/Create trees")]
        public void CreateTrees()
        {
            CreateElements(treesAmount, treesPrefabs, treesHolder);
        }

        [ContextMenu("LevelGen/Create rocks")]
        public void CreateRocks()
        {
            CreateElements(rocksAmount, rocksPrefabs, rocksHolder);
        }

        [ContextMenu("LevelGen/Create grass")]
        public void CreateGrass()
        {
            //CreateElements(grassAmount, grassPrefabs, grassHolder);
        }

        private void CreateElements(int amount, List<Transform> prefabsList, Transform holder)
        {
            for (int i = 0; i < amount; i++)
            {
                Quaternion rot = Quaternion.Euler(0f, Random.Range(0f, 359f), 0f);
                Transform element = Instantiate(prefabsList.GetRandomItem(), GetRandomPosition(), rot, holder);
                element.localScale *= Random.Range(1f, maxScale);
            }
        }

        [ContextMenu("LevelGen/Remove all")]
        public void RemoveAll()
        {
            RemoveTrees();
            RemoveRocks();
            RemoveGrass();
        }

        [ContextMenu("LevelGen/Remove trees")]
        public void RemoveTrees()
        {
            RemoveElements(treesHolder);
        }

        [ContextMenu("LevelGen/Remove rocks")]
        public void RemoveRocks()
        {
            RemoveElements(rocksHolder);
        }

        [ContextMenu("LevelGen/Remove grass")]
        public void RemoveGrass()
        {
            //RemoveElements(grassHolder);
        }

        private void RemoveElements(Transform holder)
        {
            Transform[] children = holder.GetComponentsInChildren<Transform>();

            for (int i = 0; i < children.Length; i++)
            {
                if (children[i] != holder && children[i] != null)
                {
                    DestroyImmediate(children[i].gameObject);
                }
            }
        }

        private Vector3 GetRandomPosition(float y = 0f)
        {
            return new Vector3(Random.Range(bottomLeftCorner.x, topRightCorner.x), y, Random.Range(bottomLeftCorner.y, topRightCorner.y));
        }
    }
}
