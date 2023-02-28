using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerUpCreator : MonoBehaviour
{
    [SerializeField] Tilemap scenario;
    [SerializeField] Transform powerUpPrefab;
    [SerializeField] Player player;
    [SerializeField] ParticleSystem powerUpParticles;

    private void Start()
    {
        powerUpParticles.gameObject.SetActive(false);
    }

    public void CreatePowerUpAtRandomPosition()
    {
        StartCoroutine(CreatePowerUpAtRandomPosition_Coroutine());

    }

    IEnumerator CreatePowerUpAtRandomPosition_Coroutine()
    {
        yield return new WaitUntil(() => GameController.instance.IsPlayable());

        Transform _powerUp = Instantiate(powerUpPrefab);

        Vector3Int _randomPos = TilemapsManager.instance.GetRandomTilePosition();

        while (IsBodyInTile(_randomPos))
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

   public void PlayEffectAt(Vector2 pos)
    {
        powerUpParticles.transform.position = pos;
        StartCoroutine(PlayEffectCoroutine());
    }

    IEnumerator PlayEffectCoroutine()
    {
        powerUpParticles.gameObject.SetActive(true);
        powerUpParticles.Play();
        yield return new WaitForSeconds(1);
        powerUpParticles.gameObject.SetActive(false);
    }
}
