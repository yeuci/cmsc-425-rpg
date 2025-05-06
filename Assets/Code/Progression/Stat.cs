using System;
using UnityEditor.Scripting;
using UnityEngine;

[System.Serializable]
public class Stat {
    // Level Counter
    public int level;
    // Experience Counter
    public float experience, expToNext;
    // Base stats
    public float health, attack, defense, speed, magic;

    public Stat() {
        level = 1;
        experience = 0;
        health = 10;
        attack = 10;
        defense = 10;
        speed = 10;
        magic = 10;
        expToNext = (50 - level) * (float)Math.Pow(2, level); 
    }


    //  Basic Stat scaling based on level
    public Stat(int level, float scaling) {
        scaling *= level;

        this.level = level;
        experience = 0;
        health = 10 * scaling;
        attack = 10 * scaling;
        defense = 10 * scaling;
        speed = 10 * scaling;
        magic = 10 * scaling;
        expToNext = (50 - level) * (float)Math.Pow(2, level); 
    }

    // Base stats with custom scaling for each stat, scalings[] order: hp, mana, atk, def, spd, mgk
    public Stat(int level, float[] scalings) {
        this.level = level;
        experience = 0;
        health = 10 * scalings[0];
        attack = 10 * scalings[2];
        defense = 10 * scalings[3];
        speed = 10 * scalings[4];
        magic = 10 * scalings[5];
        expToNext = (50 - level) * (float)Math.Pow(2, level); 
    }

    // Custom values for each stat, stats[] order: hp, atk, def, spd, mgk
    public Stat(int level, float hp, float atk, float def, float spd, float mgk) {
        this.level = level;
        experience = 0;
        health = hp;
        attack = atk;
        defense = def;
        speed = spd;
        magic = mgk;
        expToNext = (50 - level) * (float)Math.Pow(2, level); 
    }

    //This will only be used to create enemies
    public Stat(int level){
        int skillPoints = 50+(level-1)*5; //This gets the total number of skill points that the player has
        skillPoints *= (int) UnityEngine.Random.Range(0.8f, 1.2f);
        skillPoints -= 25;
        //Clamp all stats at a minimum of 5
        float [] statArr = {5f,5f,5f,5f,5f};
        //Randomly assign the remaining stats
        for(int i = 0; i < skillPoints; i++){
            statArr[UnityEngine.Random.Range(0, 5)] += 1;
        }
        level = 0;
        experience = 0;
        health = statArr[0];
        attack = statArr[1];
        defense = statArr[2];
        speed = statArr[3];
        magic = statArr[4];
        expToNext = (50 - level) * (float)Math.Pow(2, level); 
    }

    

    public float getStatTotal() {
        return health + attack + defense + speed + magic;
    }

    public float[] getStatArray() {
        float[] statArray = new float[] {health, attack, defense, speed, magic};
        return statArray;
    }
};
