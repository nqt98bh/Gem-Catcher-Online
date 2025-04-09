using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sync : MonoBehaviourPun,IPunObservable
{
  
        public MonoBehaviour[] localScripts;
        public GameObject[] localObjects;
        Rigidbody2D r;
        Vector3 latestPos;
        Quaternion latestRot;
        Vector3 latestVelocity;
        Vector3 latestAngularVelocity;

        float currentTime = 0;
        double currentPacketTime = 0;
        double lastPacketTime = 0;

        Vector3 positionAtLastPacket = Vector3.zero;
        Quaternion rotationAtLastPacket = Quaternion.identity;
        Vector3 velocityAtLastPacket = Vector3.zero;
        float angularVelocityAtLastPacket = 0;

        void Awake()
        {
            r = GetComponent<Rigidbody2D>();
            r.isKinematic = !photonView.IsMine;
            for (int i = 0; i < localScripts.Length; i++)
            {
                localScripts[i].enabled = photonView.IsMine;
            }
            for (int i = 0; i < localObjects.Length; i++)
            {
                localObjects[i].SetActive(photonView.IsMine);
            }
        }
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
                //stream.SendNext(r.velocity);
                //stream.SendNext(r.angularVelocity);
            }
            else
            {
                // Network player, receive data
                latestPos = (Vector3)stream.ReceiveNext();
                latestRot = (Quaternion)stream.ReceiveNext();
                //latestVelocity = (Vector3)stream.ReceiveNext();
                //latestAngularVelocity = (Vector3)stream.ReceiveNext();
                // Lag compensation
                currentTime = 0.0f;
                lastPacketTime = currentPacketTime;
                currentPacketTime = info.SentServerTime;
                positionAtLastPacket = transform.position;
                rotationAtLastPacket = transform.rotation;
                //velocityAtLastPacket = r.velocity;
                //angularVelocityAtLastPacket = r.angularVelocity;
            }
        }
        void Update()
        {
            if (!photonView.IsMine)
            {

                double timeToReachGoal = currentPacketTime - lastPacketTime;
                currentTime += Time.deltaTime;

                transform.position = Vector3.Lerp(positionAtLastPacket, latestPos, (float)(currentTime / timeToReachGoal));
                transform.rotation = Quaternion.Lerp(rotationAtLastPacket, latestRot, (float)(currentTime / timeToReachGoal));
                //r.velocity = Vector3.Lerp(velocityAtLastPacket, latestVelocity, (float)(currentTime / timeToReachGoal));
                //r.angularVelocity = Vector3.Lerp(angularVelocityAtLastPacket, latestAngularVelocity, (float)(currentTime / timeToReachGoal));
            }
        }
    
}
