using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryProgressionDetectorScritp : MonoBehaviour
{
    private EnemySpawnerScript lastSpawner;
    private bool isInRoom = false;
    
    public int storyProgression = 0;
    public TextMeshProUGUI storyText;
    public bool textShown = false; 
    
    private float timeWhenPrinted;

    private string[] storyTextLines =
    {
        "Woah, where am I? Did I fall from up there?. (Move through the dungeon using A-D or jump using Space. Light around you indicates your health)",
        
        "What happened? I fell into that hole, but.. I am alive? And also what was that reverting sound? Well never mind, I better make some stretches before I try to jump again.",
        
        "Dog, a dog has bitten me, or did he? It looked more like some kind of magic. Let me try too ... (Press Left Mouse Button to summon wind slash)",
        
        "This doggo was ... throwing that weird magic at me? I wanna try it too. (Press Right Mouse Button to focus magic into projectile",
        
        "That guy was huge, I need to fight him, really? How?",
        
        "And now there was an ... there was an ... what was that again?",
        
        "Yes, this look like ... room of ... king? But where is he?",
        
        "This hole looks familiar ... oh no, I remember, I remember it all. I AM THE KING. The king of looped castle. And, and I got bored and I jumped? Did I hit my head before of after the fall?"
    };

    private void FixedUpdate()
    {
        if (textShown)
        {
            if (Time.realtimeSinceStartup > timeWhenPrinted)
            {
                storyText.text = "";
            }
        }
    }

    public void UpdateStory(int level)
    {
        if (level > storyProgression)
        {
            storyProgression = level;
        }
    }

    
    
    
    public void PrintStoryText()
    {
        int id = 0;
        switch (storyProgression)
        {
            case 0: id = 0; 
                break;
            case 3: id = 1;
                break;
            case 5: id = 2;
                break;
            case 15: id = 3;
                break;
            case 25: id = 4;
                break;
            case 35: id = 5;
                break;
            default: break;
        }

        storyText.text = storyTextLines[id];
        timeWhenPrinted = Time.realtimeSinceStartup + 10;
        textShown = true;
    }
    
    public void PrintStoryText(string text, int level)
    {
        if (level > storyProgression)
        {
            textShown = true;
            storyText.text = text;
            timeWhenPrinted = Time.realtimeSinceStartup + 10;
            textShown = true;
        }
    }
}
