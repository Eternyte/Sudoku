// Button.cs
// Description: Implements a button.
// 
// Author: Stacy Chen (sjc5938@rit.edu)
// ///////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sudoku {
    /* Class: Button
     * Implements the button class which is composed of several images.
     * Buttons are drawn depending on the location of the mouse.
     */
    class Button {
        // The button when the mouse is not on the button. //
        private Texture2D imageOff;
        // The button when the mouse is on the button. //
        private Texture2D imageOn;
        // The name of the button. //
        public string Name { get; set; }
        // The location where the button is drawn. //
        public Rectangle Destination { get; set; }
        // If the mouse is on the button. //
        public bool IsOn { get; set; }

        /* Constructor creates a button given name, images, and location to draw to.
         * @param   str     the name of the button
         * @param   imgOff  the image when the mouse is not on the button
         * @param   imgOn   the image when the mouse is on the button
         * @param   dest    the Rectangle of where the button is to be drawn
         * @postcond        button is created
         */
        public Button(string str, Texture2D imgOff, Texture2D imgOn, Rectangle dest) {
            Name = str;
            imageOff = imgOff;
            imageOn = imgOn;
            Destination = dest;
            IsOn = false;
        }

        /* Draw draws the button.
         * @param   spriteBatch     enables sprites to be drawn
         */
        public void Draw(SpriteBatch spriteBatch) {
            if (IsOn == false) {
                spriteBatch.Draw(imageOff, Destination, Color.White);
            } else {
                spriteBatch.Draw(imageOn, Destination, Color.White);
            }
        }
    }
}
