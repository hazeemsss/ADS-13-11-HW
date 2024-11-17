using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationUI : MonoBehaviour
{
    public TMP_InputField inputText;
    public Button findDrone;
    public Button destroyDrone;

    public TMP_Text position;

    public DroneBTCommunication lessThanBST;
    public DroneBTCommunication greaterThanBST;

    void Start()
    {
        findDrone.onClick.AddListener(findDroneButton);
        destroyDrone.onClick.AddListener(destroyDroneButton);
    }


    public void findDroneButton()
    {
        if (int.TryParse(inputText.text, out int ID))
        {
            var result = lessThanBST.searchDroneinTree(ID, drone => drone.ID);

            if (result.droneId != -1)
            {
                position.text = $"Drone ID: {result.droneId}\nPosition: {result.position}\nSearch Time: {result.searchTimeMilliseconds} ms";
            }
            else
            {
                position.text = "Drone not found.";
            }
        }
        else
        {
            position.text = "Invalid Input.";
        }
    }


    public void destroyDroneButton()
    {
        if (int.TryParse(inputText.text, out int ID))
        {
            var result = lessThanBST.searchDroneinTree(ID, drone => drone.ID);
            if (result.droneId != -1)
            {
                // Delete the drone if it exists
                lessThanBST.deleteDrone(ID, drone => drone.ID);
                greaterThanBST.deleteDrone(ID, drone => drone.ID);


                position.text = $"Destroyed Drone ID: {result.droneId}\n" +
                                     $"Last Position: {result.position}\n" +
                                     $"Time Taken: {result.searchTimeMilliseconds} ms";
            }
            else
            {
                position.text = $"Drone ID: {ID} not found.";
            }
        }
        else
        {
            position.text = "Invalid Drone ID.";
        }
    }

}