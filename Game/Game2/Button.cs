using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game{
    class Button {
        private Texture2D imageOff;
        private Texture2D imageOn;
        public string Name { get; set; }
        public Rectangle Destination { get; set; }
        public bool IsOn { get; set; }

        public Button(string str, Texture2D imgOff, Texture2D imgOn, Rectangle dest) {
            Name = str;
            imageOff = imgOff;
            imageOn = imgOn;
            Destination = dest;
            IsOn = false;
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (IsOn == false) {
                spriteBatch.Draw(imageOff, Destination, Color.White);
            } else {
                spriteBatch.Draw(imageOn, Destination, Color.White);
            }
        }
    }
}
