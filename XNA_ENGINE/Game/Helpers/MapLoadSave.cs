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
        private const int MAX_HEIGHT = 40;
        private const int MAX_WIDTH = 40;

        private Random m_Random;

        private MapLoadSave()
        {
            m_Random = new Random();
            //m_GridField = new GridTile[MAX_WIDTH, MAX_HEIGHT];
        }

        static public MapLoadSave GetInstance()
        {
            if (m_MapLoadSave == null)
                m_MapLoadSave = new MapLoadSave();

            return m_MapLoadSave;
        }

        public GridTile[,] LoadMap(string XMLFile, GameScene pGameScene)
        {
            GridTile[,] gridField = new GridTile[MAX_WIDTH, MAX_HEIGHT];
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
                            gridField[width, height].SetTileType(GridTile.TileType.Normal);
                            break;
                        case 1:
                            gridField[width, height].SetTileType(GridTile.TileType.Cliff);
                            break;
                        case 2:
                            gridField[width, height].SetTileType(GridTile.TileType.Water);
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

        public void SaveMap(GridTile[,] gridField)
        {
            var xmlFile = new FileStream("GeneratedTileMap.xml", FileMode.OpenOrCreate, FileAccess.Write);
            var writer = new StreamWriter(xmlFile);

            writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            writer.WriteLine("<tilemap>");

            for (int height = 0; height < MAX_HEIGHT; ++height)
            {
                for (int width = 0; width < MAX_WIDTH; ++width)
                {
                    writer.WriteLine("<tile>");

                    writer.WriteLine("\t<positionX>" + width + "</positionX>");
                    writer.WriteLine("\t<positionY>" + height + "</positionY>");
                    writer.WriteLine("\t<type>" + gridField[height, width].GetTileType() + "</type>");
                    writer.WriteLine("\t<settlement>" + gridField[height, width].GetTileSettlement() + "</settlement>");

                    writer.WriteLine("</tile>");
                }
            }

            writer.WriteLine("</tilemap>");

            writer.Close();
            xmlFile.Close();
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