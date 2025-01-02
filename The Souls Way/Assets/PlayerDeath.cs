using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Lynq;
//using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerDeath: MonoBehaviour
{
    [Header("Checkpoints")]
    private List<GameObject> checkpoints = new List<GameObject>();
    [SerializeField] private GameObject checkpointChrgesGameObject;
    public List<Image> checkpointCharges = new List<Image>();
    private int checkpointsAvailable;
    public int maxCheckpoints = 3;

    [Header("Empty Souls")]
    public int maxEmptySouls = 1;
    private List<GameObject> emptySouls = new List<GameObject>();
    [SerializeField] private GameObject strengthBarParent;
    [SerializeField] private Image strengthbar;
    private List<GameObject> soulsToReclaim = new List<GameObject>();

    [Header("Components")]
    [SerializeField] private GameObject parentForPlacables;
    [SerializeField] AudioSource playerDeathSFX;
    [SerializeField] AudioSource playerLandSFX;
    [SerializeField] AudioSource placeCheckpointSFX;

    [SerializeField] private Sprite EmptySoul;
    [SerializeField] private Sprite FullSoul;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector3 spawnPoint;

    void Start()
    {
        spawnPoint = this.transform.position;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        checkpointsAvailable = maxCheckpoints;
        for (int i = 0; i < maxCheckpoints; i++)
        {
            GameObject go = Instantiate(Resources.Load("Soul"), checkpointChrgesGameObject.transform.position, Quaternion.identity, checkpointChrgesGameObject.transform) as GameObject;
            checkpointCharges.Add(go.GetComponent<Image>());
        }
        UpdateStrengthbar();
    }
    private void Update()
    {
        if (checkpointsAvailable == maxCheckpoints)
        {
            return;
        }
        if(checkpointCharges[checkpointsAvailable].fillAmount < 1)
        {
            Image image = checkpointCharges[checkpointsAvailable];
            image.fillAmount += 0.05f * Time.deltaTime;
            image.color = new Color(1, 1, 1, image.fillAmount);
        }
        else
        {
            checkpointsAvailable++;
        }
    }
    public void OnSpawnCheckpoint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (checkpointsAvailable > 0)
            {
                if(checkpointsAvailable == maxCheckpoints)
                {
                    checkpointsAvailable--;
                    checkpointCharges[checkpointsAvailable].fillAmount = 0;
                    checkpointCharges[checkpointsAvailable].color = new Color(1, 1, 1, 0);
                }
                else
                {
                    checkpointsAvailable--;
                    checkpointCharges[checkpointsAvailable].fillAmount = checkpointCharges[checkpointsAvailable+1].fillAmount;
                    checkpointCharges[checkpointsAvailable].color = checkpointCharges[checkpointsAvailable + 1].color;
                    checkpointCharges[checkpointsAvailable + 1].fillAmount = 0;
                    checkpointCharges[checkpointsAvailable].color = new Color(1, 1, 1, 0);
                }
                placeCheckpoint();
            }
            else
            {
                //play sth
            }
        }
    }
    public void placeCheckpoint()
    {
        placeCheckpointSFX.Play();
        GameObject go = Instantiate(Resources.Load("Soul-checkpoint"), this.gameObject.transform.position, Quaternion.identity, parentForPlacables.transform) as GameObject;
        checkpoints.Insert(0, go);
        if(checkpoints.Count > maxCheckpoints)
        {
            Destroy(checkpoints[checkpoints.Count - 1]);
            checkpoints.RemoveAt(checkpoints.Count - 1);
        }
    }
    public void OnDeleteCheckpoint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (checkpoints.Count >= 1)
            {
                Destroy(checkpoints[0]);
                checkpoints.RemoveAt(0);
            }
        }
    }

    public void OnSuicide(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (emptySouls.Count < maxEmptySouls)
            {
                StartCoroutine(Suicide());
            }
            else
            {
                //do sth
            }
        }
    }
    private IEnumerator Suicide()
    {
        sr.sprite = EmptySoul;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        playerDeathSFX.Play();
        Vector3 suicidePosition = transform.position;
        yield return new WaitForSeconds(1f);
        Respawn();
        rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        sr.sprite = FullSoul;
        GameObject go = Instantiate(Resources.Load("Empty Soul"), suicidePosition, Quaternion.identity, parentForPlacables.transform) as GameObject;
        emptySouls.Add(go);
        UpdateStrengthbar();
    }
    private void UpdateStrengthbar()
    {
        strengthBarParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxEmptySouls*100+20);
        strengthbar.fillAmount = 1 - (float)emptySouls.Count/(float)maxEmptySouls;
    }
    public void IncreaseMaxSoulsCount()
    {
        print("WE");
        maxEmptySouls++;
        UpdateStrengthbar();
    }
    public void SetSoulToReclaim(GameObject reclaimedSoul, bool insert = true)
    {
        if(insert)
            soulsToReclaim.Add(reclaimedSoul);
        else 
            soulsToReclaim.Remove(reclaimedSoul);
    }
    public void OnReclaimSoul(InputAction.CallbackContext context)
    {
        if (context.started)
        { 
            if (soulsToReclaim.Count > 0)
            {
                emptySouls.Remove(soulsToReclaim[0]); //remove from list of soles
                Destroy(soulsToReclaim[0]);
               // soulsToReclaim.RemoveAt(0);
                UpdateStrengthbar();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            StartCoroutine(Die());
        }
    }
    public IEnumerator Die()
    {
        sr.sprite = EmptySoul;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        playerDeathSFX.Play();
        playerLandSFX.Play();
        yield return new WaitForSeconds(1f);
        Respawn();
        rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        sr.sprite = FullSoul;
    }
    private void Respawn()
    {
        if (checkpoints.Count > 0)
            this.transform.position = checkpoints[0].transform.position;
        else
            this.transform.position = spawnPoint;
    }
    public void hurtRoutine()
    {
       /* myLimit.characterCanMove = false;

        if (optionsScript.screenShake)
        {
            //The screenshake is played in a Unity Event, provided the option is turned on
            onHurt?.Invoke();
        }

        playerDeathSFX.Play();

        Stop(0.1f);
        myAnim.SetTrigger("Hurt");
        Flash();

        //Start a timer, before respawning the player. This uses the (excellent) free Unity asset DOTween
        float timer = 0;
        DOTween.To(() => timer, x => timer = x, 1, respawnTime).OnComplete(respawnRoutine);*/
    }
}