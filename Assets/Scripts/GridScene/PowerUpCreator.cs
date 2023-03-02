using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerUpCreator : MonoBehaviour
{
    [SerializeField] Tilemap scenario;
    [SerializeField] Tilemap firecampAreaTilemap;
    [SerializeField] Transform[] powerUpPrefabs;
    [SerializeField] Player player;
    //[SerializeField] ParticleSystem powerUpParticles;

    private void Start()
    {
        //powerUpParticles.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CreatePowerUpAtRandomPosition();
        }
    }

    public void CreatePowerUpAtRandomPosition()
    {
        StartCoroutine(CreatePowerUpAtRandomPosition_Coroutine());

    }

    IEnumerator CreatePowerUpAtRandomPosition_Coroutine()
    {
        //yield return new WaitUntil(() => GameController.instance.IsPlayable());
        yield return null;

        Transform _powerUp = Instantiate(powerUpPrefabs[Random.Range(0,powerUpPrefabs.Length)]);

        Vector3Int _randomPos = TilemapsManager.instance.GetRandomTilePosition();

        while (IsBodyInTile(_randomPos) || IsTooCloseToFirecamp(_randomPos))
        {
            _randomPos = TilemapsManager.instance.GetRandomTilePosition();
        }

        _powerUp.position = TilemapsManager.instance.Scenario.GetCellCenterWorld(_randomPos);
        _powerUp.SetParent(transform);
    }

    bool IsBodyInTile(Vector3Int tilePosition)
    {
        for (int i = 0; i < player.Body.Count; i++)
        {
            Vector3Int _bodyPos = Vector3Int.FloorToInt(player.Body[i].position);

            if (_bodyPos == tilePosition)
                return true;
        }

        return false;
    }

    bool IsTooCloseToFirecamp(Vector3Int tilePosition)
    {
        return firecampAreaTilemap.HasTile(tilePosition);
    }

   //public void PlayEffectAt(Vector2 pos)
   // {
   //     powerUpParticles.transform.position = pos;
   //     StartCoroutine(PlayEffectCoroutine());
   // }

   // IEnumerator PlayEffectCoroutine()
   // {
   //     powerUpParticles.gameObject.SetActive(true);
   //     powerUpParticles.Play();
   //     yield return new WaitForSeconds(1);
   //     powerUpParticles.gameObject.SetActive(false);
   // }
}
