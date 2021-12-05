using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Collector : MonoBehaviour
{
    #region What the Collector needs 
    [Tooltip("What types of collectables exist in the game?")]
    [SerializeField] List<Collectible> _collectibles;

    [Tooltip("What happens when you finish collecting everything?")]
    [SerializeField] UnityEvent _whenCollectionComplete;

    //How many "collectibles" have been collected
    int _countCollected = 0;

    //Text to show how many of said collectable remain to be picked up
    TMP_Text _remainingText = null;
    #endregion


    #region Gizmos for the collector
    //"What color to make the gizmo when not selected"
    static Color _gizmoColorWhenNotSelected = new Color(0.6f, 0.6f, 0.6f, 0.6f);

    [Tooltip("The Gizmo color if selected")]
    [SerializeField] Color _gizmoColorWhenSelected;
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        _remainingText = GetComponentInChildren<TMP_Text>();


        foreach (var collectible in _collectibles)
        {
            //The collectible will have an event and then do whatever ItemPickedUp needs!
            collectible.OnPickedUp += ItemPickedUp;
        }

        //Count how many collectables remain
        int countRemaining = _collectibles.Count - _countCollected;

        //If the remaining text component exists, set the string to show how many of said collectables remain
        _remainingText?.SetText(countRemaining.ToString());
    }


    // Update is called once per frame
    public void ItemPickedUp()
    {
        //Add one to how many collectibles have been picked up
        _countCollected++;

        //Calculate how many are remaining
        int countRemaining = _collectibles.Count - _countCollected;

        //If the remaining text component exists, set the string to show how many of said collectable remain
        _remainingText?.SetText(countRemaining.ToString());
        
        //if the count is bigger than zero,
        if (countRemaining > 0)
            return; //Just stop running the code

        //If all collectables have been gathered
        if (countRemaining == 0)
            _whenCollectionComplete.Invoke(); //invoke the on complete event!
    }

    //Because it's OnValidate, this will happen in the editor!
    private void OnValidate()
    {

        //In case we got duplicates in our list, the .Distinct method will fix that up.
        _collectibles = _collectibles.Distinct().ToList();
    }

    //Just helpful tools to check which collectors have which items
    private void OnDrawGizmosSelected()
    {
       //For each collectable we have in the list
        foreach (var collectible in _collectibles)
        {
            //Check to see if the object with the collector script was added
            if (UnityEditor.Selection.activeGameObject == gameObject)
                // If so, then change gizmo colors to something eye catching so we know its selected
                Gizmos.color = _gizmoColorWhenSelected; 
            else
                //If not, then change the gizmo colors to something when not selected
                Gizmos.color = _gizmoColorWhenNotSelected;

            //Draw the gizmo
            Gizmos.DrawLine(transform.position, collectible.transform.position);
        }
    }
}

