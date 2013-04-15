using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Managers;
using XNA_ENGINE.Game.Objects;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.Helpers
{
    class MapLoadSave
    {
        //Singleton implementation
        private static MapLoadSave m_MapLoadSave;

        // Generate a new Map
        private const int MAX_HEIGHT = 30;
        private const int MAX_WIDTH = 30;

        private Random m_Random;

        private MapLoadSave()
        {
            m_Random = new Random();
        }

        static public MapLoadSave GetInstance()
        {
            if (m_MapLoadSave == null)
                m_MapLoadSave = new MapLoadSave();

            return m_MapLoadSave;
        }

        public GridTile[,] LoadMap(GameScene pGameScene, string XMLFile = "GeneratedTileMap")
        {
            GridTile[,] gridField = new GridTile[MAX_WIDTH, MAX_HEIGHT];

            Stream stream = TitleContainer.OpenStream(XMLFile + ".xml");
            XDocument doc = XDocument.Load(stream);
            
            List<GridTile> gridList = (from tile in doc.Descendants("tile")
                       select new GridTile(pGameScene,Convert.ToInt32(tile.Element("positionX").Value),Convert.ToInt32(tile.Element("positionY").Value))
                       {
                           TileTypeValue = (GridTile.TileType)Convert.ToInt32(tile.Element("TileType").Value)
                       }).ToList();

            foreach (GridTile element in gridList)
            {
                gridField[element.Row, element.Column] = element;
            }

            return gridField;
        }

        public void SaveMap(GridTile[,] gridField, string fileName = "GeneratedTileMap")
        {
            //"../../../../XNA_DEMOContent/XMLFiles/" + 
            var xmlFile = new FileStream(fileName + ".xml", FileMode.OpenOrCreate, FileAccess.Write);
            var writer = new StreamWriter(xmlFile);

            writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            writer.WriteLine("<tilemap>");

            for (int height = 0; height < MAX_HEIGHT; ++height)
            {
                for (int width = 0; width < MAX_WIDTH; ++width)
                {
                    writer.WriteLine("\t<tile>");

                    writer.WriteLine("\t\t<positionX>" + width + "</positionX>");
                    writer.WriteLine("\t\t<positionY>" + height + "</positionY>");
                    writer.WriteLine("\t\t<TileType>" + (int)gridField[height, width].TileTypeValue + "</TileType>");
                    writer.WriteLine("\t\t<settlement>" + gridField[height, width].GetTileSettlement() + "</settlement>");

                    writer.WriteLine("\t</tile>");
                }
            }

            writer.WriteLine("</tilemap>");

            writer.Close();
            xmlFile.Close();
        }

        public GridTile[,] GenerateMap(GameScene pGameScene)
        {
            GridTile[,] gridField = new GridTile[MAX_WIDTH, MAX_HEIGHT]; ;
            // Map:
            List<GridTile> addTest = new List<GridTile>();

            for (int height = 0; height < MAX_HEIGHT; ++height)
            {
                for (int width = 0; width < MAX_WIDTH; ++width)
                {
                    GridTile tileToAdd = new GridTile(pGameScene, width, height);
                    addTest.Add(tileToAdd);
                }
            }

            // Add Tiles to the right grid and give them the right attributes.
            for (int height = 0, teller = 0; height < MAX_HEIGHT; ++height)
            {
                for (int width = 0; width < MAX_WIDTH; ++width, ++ teller)
                {
                    gridField[width, height] = addTest.ElementAt(teller);

                    // Set the tile type (normal, tribe,...)
                    switch (m_Random.Next(0, 3))
                    {
                        case 0:
                            gridField[width, height].TileTypeValue = GridTile.TileType.Normal;
                            break;
                        case 1:
                            gridField[width, height].TileTypeValue = GridTile.TileType.Cliff;
                            break;
                        case 2:
                            gridField[width, height].TileTypeValue = GridTile.TileType.Water;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    // Use a color to create a new Tribe (none, green, red, blue,...)
                    gridField[width, height].SetTileSettlement("none");
                }
            }

            // Save to XML file
            SaveMap(gridField);

            return gridField;
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
            // -----------------------------------------
            */