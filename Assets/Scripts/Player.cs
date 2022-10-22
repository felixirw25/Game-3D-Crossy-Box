using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    [SerializeField] ParticleSystem dieParticle;
    [SerializeField, Range(0.01f, 1f)] float moveDuration=0.5f;
    [SerializeField, Range(0.01f, 1f)] float jumpHeight=0.7f;
    private int minZPos;
    private int extent;
    private float leftBoundary;
    private float rightBoundary;
    private float backBoundary;
    [SerializeField] private int maxTravel;
    public int MaxTravel { get => maxTravel; }
    [SerializeField] private int currentTravel;
    public int CurrentTravel { get => currentTravel;}
    public bool IsDie { get => this.enabled == false;}
    public ScoreData Data;
    public void SetUp(int minZPos, int extent){
        backBoundary = minZPos-1;
        leftBoundary = -(extent+1);
        rightBoundary = (extent+1);
    }
    private void Update(){
        var moveDir = Vector3.zero;
        if(Input.GetKey(KeyCode.UpArrow))
            moveDir += new Vector3(0,0,1);

        if(Input.GetKey(KeyCode.DownArrow))
            moveDir += new Vector3(0,0,-1);

        if(Input.GetKey(KeyCode.RightArrow))
            moveDir += new Vector3(1,0,0);

        if(Input.GetKey(KeyCode.LeftArrow))
            moveDir += new Vector3(-1,0,0);

        else if(moveDir == Vector3.zero)
            return;
        
        if(moveDir != Vector3.zero && IsJumping()==false)
            Jump(moveDir);
    }

    private void Jump(Vector3 targetDirection)
    {
        // mengatur rotasi
        var targetPosition = transform.position + targetDirection;
        transform.LookAt(targetPosition);

        // loncat ke atas    
        var moveSeq = DOTween.Sequence(transform);
        moveSeq.Append(transform.DOMoveY(jumpHeight, moveDuration/2));
        moveSeq.Append(transform.DOMoveY(0f, moveDuration/2));

        // bergerak maju mundur dan diagonal
        if (Tree.AllPositions.Contains(targetPosition))
            return;
        
        if(targetPosition.z <= backBoundary || targetPosition.x <= leftBoundary || targetPosition.x >= rightBoundary)
            return;
            
        audioSource.PlayOneShot(audioClip);
        transform.DOMoveX(targetPosition.x, moveDuration);
        transform.DOMoveZ(targetPosition.z, moveDuration)
        .OnComplete(UpdateTravel);
        //
    }

    private void UpdateTravel()
    {
        currentTravel = (int) this.transform.position.z;
        // maxTravel = currentTravel > maxTravel ? currentTravel : maxTravel;
        if(currentTravel > maxTravel){
            maxTravel = currentTravel;
        }

        scoreText.text = "SCORE: " + maxTravel.ToString();

    }
    public bool IsJumping()
    {
        return DOTween.IsTweening(transform);
    }

    // di execute sekali frame selama collide (ketika nempel pertama kali)
    private void OnTriggerEnter(Collider other){
        /*
        var car = other.GetComponent<Car>();
        if(car != null){
            Debug.Log("Enter " + car.name);
            AnimateDie(car);
        }
        */

        //
        if(other.tag == "Car"){
            Debug.Log("Hit " + other.name);
            if(this.enabled == true)
                AnimateCrash(); 
        }
        //
    }

    /* 2
    private void AnimateDie(Car car)
    {
        var isRight = car.transform.rotation.y == 90;
        transform.DOMoveX(isRight ? 8 : -8, 0.2f);
        transform.DORotate(Vector3.forward*360, 1f).SetLoops(10, LoopType.Restart);
    }
    */

    //
    private void AnimateCrash()
    {
        transform.DOScaleY(0.1f, 0.2f);
        transform.DOScaleX(1f, 0.2f);
        transform.DOScaleZ(0.8f, 0.2f);
        this.enabled = false;
        dieParticle.Play();
    }
    //

    // di execute setiap frame selama collide (masih nempel)
    private void OnTriggerStay(Collider other){
        // Debug.Log("Stay");
    }

    // di execute sekali frame selama collide ketika tidak nempel
    private void OnTriggerExit(Collider other){
        // Debug.Log("Exit");
    }
    private void OnEnable() {
        Data = DataManager.LoadData();
    }

    private void OnDisable() {
        DataManager.SaveData(this);
    }
}
