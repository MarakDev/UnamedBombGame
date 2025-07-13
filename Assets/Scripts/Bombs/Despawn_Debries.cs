using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn_Debries : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(DespawnDebries());
    }

    private IEnumerator DespawnDebries()
    {
        yield return new WaitForSeconds(4f);

        while (true)
        {
            Color spriteColor = spriteRenderer.color;
            spriteColor.a -= 0.05f;
            spriteRenderer.color = spriteColor;

            if (spriteColor.a < 0.05f)
                Destroy(this.gameObject);

            yield return new WaitForSeconds(0.1f);
        }

    }
}
