using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA_ENGINE.Game.Objects
{
    class Sjaman : Placeable
    {
        public Sjaman()
        {
        }

        public void Update()
        {
            if (m_LinkedTile.Selected)
            {
                Menu.GetInstance().SubMenu = Menu.SubMenuSelected.BuildMode;
            }
        }


    }
}
