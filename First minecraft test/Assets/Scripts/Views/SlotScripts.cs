using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class SlotScripts : MonoBehaviour
    {
        private GameObject gameViewGameObject;
        private GameView gameView;

        private void Start()
        {
            gameViewGameObject = GameObject.FindGameObjectWithTag("GameView");
            gameView = gameViewGameObject.GetComponent<GameView>();
        }

        public void ButtonPress()
        {
            var name = gameObject.transform.parent.name;

            gameView.SlotClicked(int.Parse(name));
        }
    }
}
