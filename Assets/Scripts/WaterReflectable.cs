﻿using UnityEngine;
using System.Collections;

/// Automatically create a water reflect for a sprite.
[RequireComponent(typeof(SpriteRenderer))]
public class WaterReflectable : MonoBehaviour
{
        #region Members

        [Header("Reflect properties")]
        public Vector3 localPosition = new Vector3(0, -0.25f, 0);
        public Vector3 localRotation = new Vector3(0, 0, -180);
        public Vector3 localScale = new Vector3(0, 0, 0);
        [Tooltip("Optional: force the reflected sprite. If null it will be a copy of the source.")]
        public Sprite sprite;
        public string spriteLayer = "Default";
        public int spriteLayerOrder = -5;

        private SpriteRenderer spriteSource;
        private SpriteRenderer spriteRenderer;

        #endregion

        #region Timeline

        void Awake()
        {
            GameObject reflectGo = new GameObject("Water Reflect");
            reflectGo.transform.parent = this.transform;
            reflectGo.transform.localPosition = localPosition;
            reflectGo.transform.localRotation = Quaternion.Euler(localRotation);
            reflectGo.transform.localScale = localScale;

            spriteRenderer = reflectGo.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerName = spriteLayer;
            spriteRenderer.sortingOrder = spriteLayerOrder;

            spriteSource = GetComponent<SpriteRenderer>();
        }

        void OnDestroy()
        {
            if (spriteRenderer != null)
            {
                Destroy(spriteRenderer.gameObject);
            }
        }

        void LateUpdate()
        {
            if (spriteSource != null)
            {
                if (sprite == null)
                {
                    spriteRenderer.sprite = spriteSource.sprite;
                }
                else
                {
                    spriteRenderer.sprite = sprite;
                }
                spriteRenderer.flipX = spriteSource.flipX;
                spriteRenderer.flipY = spriteSource.flipY;
                spriteRenderer.color = spriteSource.color;
            }
        }

        #endregion
    }
