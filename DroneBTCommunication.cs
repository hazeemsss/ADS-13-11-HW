using System;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneBTCommunication : MonoBehaviour
{

    // Create node class
    public class BSTNode
    {
        public Drone drone;
        public BSTNode left, right;

        public BSTNode(Drone drone)
        {
            this.drone = drone;
            left = right = null;
        }
    }


    private BSTNode root;
    private TMP_Text position;


    public void initializeTree(Drone[] drones, Func<Drone, int> keySelector)
    {
        foreach (var drone in drones)
        {
            root = insertToTree(root, drone, keySelector);
        }
    }


    private BSTNode insertToTree(BSTNode node, Drone drone, Func<Drone, int> keySelector)
    {
        if (node == null) return new BSTNode(drone);

        int key = keySelector(drone);
        int nodeKey = keySelector(node.drone);

        if (key < nodeKey)
            node.left = insertToTree(node.left, drone, keySelector);
        else if (key > nodeKey)
            node.right = insertToTree(node.right, drone, keySelector);

        return node;
    }


    public (int droneId, Vector3 position, float searchTimeMilliseconds) searchDroneinTree(int droneId, Func<Drone, int> keySelector)
    {
        float totalDistance = 0f;
        var searchStatus = searchDrone(root, droneId, ref totalDistance, keySelector);

        if (searchStatus != null)
        {
            Debug.Log($"Drone {droneId} found at position {searchStatus.transform.position}");


            float searchSpeed = 2.0f;


            float searchTimeMilliseconds = (totalDistance / searchSpeed) * 1000f;

            return (droneId, searchStatus.transform.position, searchTimeMilliseconds);
        }

        else
        {
            Debug.Log($"Drone {droneId} not found.");
            return (-1, Vector3.zero, -1f);
        }
    }



    private Drone searchDrone(BSTNode node, int droneId, ref float totalTime, Func<Drone, int> keySelector)
    {
        if (node == null) return null;

        if (node.drone.ID == droneId) return node.drone;

        BSTNode nextNode = (droneId < keySelector(node.drone)) ? node.left : node.right;
        if (nextNode != null)
            totalTime += Vector3.Distance(node.drone.transform.position, nextNode.drone.transform.position);

        return searchDrone(nextNode, droneId, ref totalTime, keySelector);
    }


    public void deleteDrone(int droneId, Func<Drone, int> keySelector)
    {
        root = removeNode(root, droneId, keySelector);
    }


    private BSTNode removeNode(BSTNode node, int droneId, Func<Drone, int> keySelector)
    {
        if (node == null) return null;

        int nodeKey = keySelector(node.drone);


        if (droneId < nodeKey)
        {
            node.left = removeNode(node.left, droneId, keySelector);
        }
        else if (droneId > nodeKey)
        {
            node.right = removeNode(node.right, droneId, keySelector);
        }
        else
        {

            if (node.left == null) return node.right;
            if (node.right == null) return node.left;

            BSTNode successor = FindMinNode(node.right);
            node.drone = successor.drone;

            node.right = removeNode(node.right, keySelector(successor.drone), keySelector);
        }

        return node;
    }


    private BSTNode FindMinNode(BSTNode node)
    {
        while (node.left != null)
        {
            node = node.left;
        }
        return node;
    }

}