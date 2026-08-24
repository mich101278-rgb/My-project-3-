using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        public int coinsCounter = 0;

        private PlayerController player;
        public Text coinText;

        private void OnDestroy()
        {
            if (player != null)
            {
                player.Died -= OnPlayerDead;
            }
        }
        private void OnPlayerDead()
        {
            Invoke(nameof(ReloadLevel), 3);
        }

        void Start()
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
            player.Died += OnPlayerDead;
        }

        void Update()
        {
            coinText.text = coinsCounter.ToString();
            
        }

        private void ReloadLevel()
        {
            //Application.LoadLevel(Application.loadedLevel);
            SceneManager.LoadScene(0);
        }
    }
}
