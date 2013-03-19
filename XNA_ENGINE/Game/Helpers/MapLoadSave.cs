using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Objects;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.Helpers
{
    class MapLoadSave
    {
        //Singleton implementation
        private static MapLoadSave m_MapLoadSave;

        private MapLoadSave()
        {

        }

        static public MapLoadSave GetInstance()
        {
            if (m_MapLoadSave == null)
                m_MapLoadSave = new MapLoadSave();

            return m_MapLoadSave;
        }

        public List<List<GridTile>> LoadMap(string XMLFile, GameScene pGameScene)
        {
            List<List<GridTile>> gridField = new List<List<GridTile>>();
            List<GridTile> addTest = new List<GridTile>();
            addTest.Add(new GridTile(pGameScene,0,0));
            addTest.Add(new GridTile(pGameScene, 1, 0));
            addTest.Add(new GridTile(pGameScene, 2, 0));
            gridField.Add(addTest);
            addTest.Clear();
            addTest.Add(new GridTile(pGameScene, 0, 1));
            addTest.Add(new GridTile(pGameScene, 1, 1));
            gridField.Add(addTest);

            System.IO.Stream stream = TitleContainer.OpenStream(XMLFile);
            XDocument doc = XDocument.Load(stream);
            /*
            gridField = (from tile in doc.Descendants("tile")
                       select new GridTile(pGameScene,Convert.ToInt32(tile.Element("positionX").Value),Convert.ToInt32(tile.Element("positionY").Value))
                       {
                         /*  position = new Vector2(Convert.ToInt32(tile.Element("positionX").Value), Convert.ToInt32(tile.Element("positionY").Value)),
                           type = Convert.ToString(tile.Element("type").Value),
                           settlement = Convert.ToString(tile.Element("settlement").Value)*/
                //       }).ToList();*/

            return gridField;
        }

        public void SaveMap(List<List<GridTile>> gridField)
        {
            //NOT IMPLEMENTED YET
        }
    }
}

/*
            // ------------------------------------------
            // OPEN AND READ XML FILE
            // ------------------------------------------
            // the file must be available in the Debug (or release) folder
            System.IO.Stream stream = TitleContainer.OpenStream("tilemap.xml");

            XDocument doc = XDocument.Load(stream);

            m_Tiles = new List<Tile>();

            m_Tiles = (from tile in doc.Descendants("tile")
                       select new Tile()
                       {
                           position = new Vector2(Convert.ToInt32(tile.Element("positionX").Value), Convert.ToInt32(tile.Element("positionY").Value)),
                           type = Convert.ToString(tile.Element("type").Value),
                           settlement = Convert.ToString(tile.Element("settlement").Value)
                       }).ToList();

            // Test if the xml reader worked (and it does)
            System.Diagnostics.Debug.WriteLine("Count: " + m_Tiles.ElementAt(0).type);
            // ------------------------------------------
            // END READING XML FILE
            // -----------------------------------------*/