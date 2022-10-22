using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    Player player;

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.z <= player.CurrentTravel-10 && audioSource.isPlaying){
            audioSource.Stop();
            return;
        }  
        
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        audioSource.PlayOneShot(audioClip);

        if(this.transform.position.z <= player.CurrentTravel && player.gameObject.activeInHierarchy){
            player.transform.SetParent(this.transform);
        }
    }

    public void SetUpTarget(Player target){
        this.player = target;  
    }
}
