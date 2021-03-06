﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private GameObject blockSparklesVFX;
    [SerializeField] Sprite[] hitSprites;

    private Level level;
    private GameSession gameSession;
    private SpriteRenderer spriteRenderer;

    private int maxHits = 1;
    private int timesHit = 0;

    // Start is called before the first frame update
    void Start()
    {
        level = FindObjectOfType<Level>();
        gameSession = FindObjectOfType<GameSession>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        maxHits = hitSprites.Length + 1;

        CountBreakableBlocks();
    }

    private void CountBreakableBlocks()
    {
        if (tag == "Breakable") {
            level.CountBlocks();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable") {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        timesHit++;
        if (timesHit >= maxHits) {
            DestroyBlock();
        } else {
            ShowNextHitSprite();
        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null) {
            spriteRenderer.sprite = hitSprites[spriteIndex];
        } else {
            Debug.LogError("Block sprite is missing from array: " + gameObject.name);
        }
    }

    private void DestroyBlock()
    {
        PlayBlockDestroySFX();
        Destroy(gameObject);
        level.BlockDestoyed();
        TriggerSparklesVFX();
    }

    private void PlayBlockDestroySFX()
    {
        gameSession.AddToScore();
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
    }

    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }
}
