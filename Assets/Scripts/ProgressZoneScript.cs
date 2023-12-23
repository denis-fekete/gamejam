using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressZoneScript : MonoBehaviour
{

        public int storyProgress = 0;
        public bool forceText;
        public string text;
        private void OnTriggerEnter2D(Collider2D other)
        {
                if (other.gameObject.CompareTag("Player"))
                {
                        StoryProgressionDetectorScritp story;
                        story = GameObject.FindWithTag("StoryProgDetector").GetComponent<StoryProgressionDetectorScritp>();
                

                        if (forceText)
                        {
                                story.PrintStoryText(text, storyProgress);
                        }
                        
                        story.UpdateStory(storyProgress);
                }
        }
}
