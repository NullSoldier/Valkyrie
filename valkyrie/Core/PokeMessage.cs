using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace ValkyrieLibrary.Core
{
    class PokeMessage
    {
        static private Texture2D SignSprite = null;

        private String Title { get; set; }
        private String Text { get; set; }
 
        public PokeMessage(String title, String text)
        {
            this.Title = title;
            this.Text = text;
   
            if (SignSprite == null)
                SignSprite = TileEngine.TextureManager.GetTexture("SignSprite.png");
        }


        //returns false when there are more pages
        public bool Page()
        {

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destRectangle = new Rectangle(0, (TileEngine.Camera.Screen.Height*2)/3, TileEngine.Camera.Screen.Width, TileEngine.Camera.Screen.Height / 3);
            Rectangle imgRect = new Rectangle(0, 0, SignSprite.Width, SignSprite.Height);

            float titleX = 30;
            float titleY = (TileEngine.Camera.Screen.Height * 2) / 3 + 10;

            float textX = 30;
            float textY = (TileEngine.Camera.Screen.Height * 2) / 3 + 40;
           
            spriteBatch.Draw(SignSprite, destRectangle, imgRect, Color.White);
            spriteBatch.DrawString(PokeGame.font, this.Title, new Vector2(titleX,titleY) , Color.Black);
            spriteBatch.DrawString(PokeGame.font, this.Text, new Vector2(textX, textY), Color.Black);
        }
    }
}