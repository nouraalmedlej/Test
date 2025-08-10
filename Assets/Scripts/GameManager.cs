using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Mushrooms (spawn logic)")]
    public int mushroomsPerAnimal = 3;
    public int totalMushrooms = 0;
    private int towardNext = 0;

    [Header("Spawner Link")]
    public AnimalSpawner spawner;

    [Header("Win by Animals")]
    public int animalsToWin = 3;
    private int animalsSpawned = 0;

    [Header("Player Respawn")]
    public Transform player;
    public Transform startPoint; // نقطة البداية الثابتة

    public Action<int, float> OnMushroomsChanged;

    void Awake()
    {
        Instance = this;
        Debug.Log("[GM] GameManager started");

        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }

        if (!startPoint && player)
        {
            // إذا ما فيه StartPoint، نخلي مكان بداية اللاعب هو نفسه النقطة
            GameObject sp = new GameObject("AutoStartPoint");
            sp.transform.position = player.position;
            startPoint = sp.transform;
        }
    }

    void OnEnable() { AnimalSpawner.OnAnimalSpawned += HandleAnimalSpawned; }
    void OnDisable() { AnimalSpawner.OnAnimalSpawned -= HandleAnimalSpawned; }

    void HandleAnimalSpawned()
    {
        animalsSpawned++;
        Debug.Log($"[GM] Animals spawned: {animalsSpawned}/{animalsToWin}");
        if (animalsSpawned >= animalsToWin) Win();
    }

    public void AddMushroom(int amount)
    {
        totalMushrooms += amount;
        towardNext += amount;

        while (mushroomsPerAnimal > 0 && towardNext >= mushroomsPerAnimal)
        {
            towardNext -= mushroomsPerAnimal;
            if (spawner) spawner.SpawnAnimal();
            else Debug.LogWarning("[GM] Spawner not assigned!");
        }

        float progress01 = (mushroomsPerAnimal > 0)
            ? (towardNext % mushroomsPerAnimal) / (float)mushroomsPerAnimal
            : 0f;

        Debug.Log($"[GM] total={totalMushrooms}, towardNext={towardNext}");
        OnMushroomsChanged?.Invoke(totalMushrooms, Mathf.Clamp01(progress01));
    }

    public void RespawnPlayer()
    {
        if (!player || !startPoint) return;

        Vector3 target = startPoint.position + Vector3.up * 0.1f;
        Debug.Log($"[GM] Respawn to {target}");

        var cc = player.GetComponent<CharacterController>();
        if (cc)
        {
            cc.enabled = false;
            player.position = target;
            cc.enabled = true;
        }
        else
        {
            player.position = target;
        }

        var rb = player.GetComponent<Rigidbody>();
        if (rb) rb.linearVelocity = Vector3.zero;

        var pc = player.GetComponent<PlayerController>();
        if (pc) pc.ResetMotion();

        Debug.Log("[GM] Player respawned.");
    }

    void Win()
    {
        Debug.Log("[GM] WIN → loading WinScene");
        Time.timeScale = 1f;
        SceneManager.LoadScene("WinScene");
    }
}
