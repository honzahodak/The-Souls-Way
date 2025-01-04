using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSoul : MonoBehaviour
{

    public enum UpgradeType
    {
        MaxCheckpoints1,
        MaxCheckpoints2,
        MaxSouls,
        AirJump,
        Dash,
        WallJump,
    }

    [SerializeField] private UpgradeType upgradeType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (upgradeType)
            {
                case UpgradeType.MaxCheckpoints1:
                    collision.gameObject.GetComponent<PlayerDeath>().IncreaseMaxCheckpoints(1);
                    break;
                case UpgradeType.MaxCheckpoints2:
                    collision.gameObject.GetComponent<PlayerDeath>().IncreaseMaxCheckpoints(2);
                    break;
                case UpgradeType.MaxSouls:
                    collision.gameObject.GetComponent<PlayerDeath>().IncreaseMaxSoulsCount();
                    break;
                case UpgradeType.AirJump:
                    collision.gameObject.GetComponent<characterJump>().IncreaseMaxAirJumps();
                    break;
                case UpgradeType.Dash:
                    collision.gameObject.GetComponent<characterJump>().UnlockDash();
                    break;
                case UpgradeType.WallJump:
                    collision.gameObject.GetComponent<PlayerMovement>().UnlockWallJump();
                    break;
            }

            //play animation
            StartCoroutine(RemoveSkillSoul());
        }
    }
    private IEnumerator RemoveSkillSoul()
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        while (sr.color.a > 0)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a -Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}