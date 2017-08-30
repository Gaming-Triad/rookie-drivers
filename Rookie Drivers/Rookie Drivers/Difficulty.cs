using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RookieDrivers
{
    class Difficulty
    {
        //Boss Attributes
        private float bossSpeed;
        private int bossHealth = 100;
        private float bossDamage;

        //Player Attributes
        private float playerSpeed;
        private static int playerHealth = 100;
        private float playerDamage;                   //% Health reduced

        //Traffic Attributes
        private int trafficDensity;                   //Traffic car spawn rate
        private float trafficSpeed;

        private int powerUpDensity;                   //powerUp spawn rate
        private float powerUpSpeed;

        //Read these parameters from a config file
        public Difficulty()
        {
            playerDamage = 5F;
            playerSpeed = 20;
            bossDamage = 2.5F;
            bossSpeed = 10F;
            trafficSpeed = 5F;
            trafficDensity = 6;
            powerUpSpeed = 8F;
            powerUpDensity = 6000;
        }

        public Difficulty(int level)
        {
            switch (level)
            {
                case 1:
                    playerDamage = 5F;
                    playerSpeed = 20;
                    bossDamage = 10F;
                    bossSpeed = 10F;
                    trafficSpeed = 5F;
                    trafficDensity = 3000;
                    powerUpSpeed = 12F;
                    powerUpDensity = 10000;
                    break;
                case 2:
                    playerDamage = 7.5F;
                    playerSpeed = 10;
                    bossDamage = 7.5F;
                    bossSpeed = 12F;
                    trafficSpeed = 8F;
                    trafficDensity = 200;
                    powerUpSpeed = 20F;
                    powerUpDensity = 15000;
                    break;
                default:
                    playerDamage = 5F;
                    playerSpeed = 8;
                    bossDamage = 10F;
                    bossSpeed = 10F;
                    trafficSpeed = 5F;
                    trafficDensity = 3000;
                    powerUpSpeed = 22F;
                    powerUpDensity = 20000;
                    break;
            }
        }


        public float getPlayerSpeed()
        {
            return this.playerSpeed;
        }

        public float getPlayerDamage()
        {
            return this.playerDamage;
        }

        public float getBossSpeed()
        {
            return this.bossSpeed;
        }

        public int getbossHealth()
        {
            return this.bossHealth;
        }

        public int getTrafficDensity()
        {
            return this.trafficDensity;
        }

        public float getTrafficSpeed()
        {
            return this.trafficSpeed;
        }

        public int getPowerUpDensity()
        {
            return this.powerUpDensity;
        }

        public float getPowerUpSpeed()
        {
            return this.powerUpSpeed;
        }

        public void setPlayerSpeed(float Speed)
        {
            this.playerSpeed = Speed;
        }

        public void setPlayerDamage(float Damage)
        {
            this.playerDamage = Damage;
        }

        public void setBossSpeed(float Speed)
        {
            this.bossSpeed = Speed;
        }

        public void setBossDamage(float Damage)
        {
            this.bossDamage = Damage;
        }

        public void setTrafficDensity(int TrafficDensity)
        {
            this.trafficDensity = TrafficDensity;
        }

        public void setTrafficSpeed(float TrafficSpeed)
        {
            this.trafficSpeed = TrafficSpeed;
        }

        public void setPowerUpDensity(int PowerUpDensity)
        {
            this.powerUpDensity = PowerUpDensity;
        }

        public void setPowerUpSpeed(float PowerUpSpeed)
        {
            this.powerUpSpeed = PowerUpSpeed;
        }
    }
}
