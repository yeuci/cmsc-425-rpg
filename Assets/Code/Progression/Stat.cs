using System;

[System.Serializable]
public class Stat {
    // Level Counter
    public int level;
    // Experience Counter
    public float experience, expToNext;
    // Base stats
    public float health, attack, defense, speed, magic;

    public Stat() { //This works for the Warrior Class
        level = 1;
        experience = 0;
        health = 10;
        attack = 15;
        defense = 10;
        speed = 10;
        magic = 5;
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
        attack = 10 * scalings[1];
        defense = 10 * scalings[2];
        speed = 10 * scalings[3];
        magic = 10 * scalings[4];
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

    public float getStatTotal() {
        return health + attack + defense + speed + magic;
    }

    public float[] getStatArray() {
        float[] statArray = new float[] {health, attack, defense, speed, magic};
        return statArray;
    }
};
