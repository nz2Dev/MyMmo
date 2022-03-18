using System;
using MyMmo.Client;
using UnityEngine;

public class AvatarItem : MonoBehaviour {

    public Item source;

    private Rigidbody capsuleRigidbody;
    
    private void Awake() {
        capsuleRigidbody = GetComponentInChildren<Rigidbody>();
    }

    public void MoveTo(Vector3 position) {
        transform.position = position;
    }

    private void OnCollisionEnter(Collision other) {
        capsuleRigidbody.isKinematic = true;
    }

}