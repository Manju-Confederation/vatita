using UnityEngine;
using System.Collections.Generic;

public class _BackgroundManager : MonoBehaviour
{
    public Sprite background;
    public float scale = 1;
    public bool tileY = false;

    readonly List<List<GameObject>> tiles = new();
    GameObject backgroundTiles;
    _CameraController view;
    float spriteWidth;
    float spriteHeight;
    int tileCountX;
    int tileCountY;
    Vector3 tileShiftX;
    Vector3 tileShiftY;

    void Awake()
    {
        view = GetComponent<_CameraController>();
    }

    void Start()
    {
        backgroundTiles = new("BackgroundTiles");
        if (background)
        {
            spriteWidth = background.bounds.size.x * scale;
            spriteHeight = background.bounds.size.y * scale;
        }
    }

    void Update()
    {
        if (!background) return;
        Vector3 origin = view.Origin;
        if (tiles.Count == 0)
        {
            Vector2 viewSize = view.Size;
            tileCountX = (int)(viewSize.x / spriteWidth) + 3;
            tileCountY = tileY ? (int)(viewSize.y / spriteHeight) + 3 : 1;
            tileShiftX = new(spriteWidth * tileCountX, 0);
            tileShiftY = new(0, spriteHeight * tileCountY);
            for (int i = -1; i < tileCountY - 1; i++)
            {
                List<GameObject> tileRow = new();
                tiles.Add(tileRow);
                for (int j = -1; j < tileCountX - 1; j++)
                {
                    GameObject tile = new("BackgroundTile", typeof(SpriteRenderer));
                    tile.transform.position = origin + new Vector3(spriteWidth * j, spriteHeight * (tileY ? i : 0.5f), 20);
                    tile.transform.localScale = new Vector3(scale, scale, 1);
                    tile.transform.SetParent(backgroundTiles.transform);
                    tile.GetComponent<SpriteRenderer>().sprite = background;
                    tileRow.Add(tile);
                }
            }
        }
        GameObject rootTile = tiles[0][0];
        while (rootTile.transform.position.x < origin.x - spriteWidth)
        {
            foreach (List<GameObject> tileRow in tiles)
            {
                GameObject tile = tileRow[0];
                tile.transform.position += tileShiftX;
                tileRow.RemoveAt(0);
                tileRow.Add(tile);
            }
            rootTile = tiles[0][0];
        }
        while (rootTile.transform.position.x > origin.x)
        {
            foreach (List<GameObject> tileRow in tiles)
            {
                GameObject tile = tileRow[^1];
                tile.transform.position -= tileShiftX;
                tileRow.RemoveAt(tileCountX - 1);
                tileRow.Insert(0, tile);
            }
            rootTile = tiles[0][0];
        }
        if (tileY)
        {
            while (rootTile.transform.position.y < origin.y - spriteHeight)
            {
                List<GameObject> tileRow = tiles[0];
                foreach (GameObject tile in tileRow)
                {
                    tile.transform.position += tileShiftY;
                }
                tiles.RemoveAt(0);
                tiles.Add(tileRow);
                rootTile = tiles[0][0];
            }
            while (rootTile.transform.position.y > origin.y)
            {
                List<GameObject> tileRow = tiles[^1];
                foreach (GameObject tile in tileRow)
                {
                    tile.transform.position -= tileShiftY;
                }
                tiles.RemoveAt(tileCountY - 1);
                tiles.Insert(0, tileRow);
                rootTile = tiles[0][0];
            }
        }
    }
}
