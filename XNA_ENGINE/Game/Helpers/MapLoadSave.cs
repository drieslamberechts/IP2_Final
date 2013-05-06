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

namespace XNA_ENGINE.Game.Objects
{
    class MapLoadSave
    {
        //Singleton implementation
        private static MapLoadSave m_MapLoadSave;

        // Generate a new Map
        
        private const int MAX_ROWS = 30;
        private const int MAX_COLUMNS = 30;

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

        public GridTile[,] LoadMap(GameScene pGameScene, string fileName = "GeneratedTileMap")
        {
            GridTile[,] gridField = new GridTile[MAX_ROWS, MAX_COLUMNS];

            Stream stream = TitleContainer.OpenStream(".../.../.../.../XNA_DEMOContent/XMLFiles/" + fileName + ".xml");
            XDocument doc = XDocument.Load(stream);
            
            List<GridTile> gridList = (from tile in doc.Descendants("tile")
                       select new GridTile(pGameScene,Convert.ToInt32(tile.Element("positionX").Value),Convert.ToInt32(tile.Element("positionY").Value))
                       {
                           TileTypeValue = (GridTile.TileType)Convert.ToInt32(tile.Element("TileType").Value)
                       }).ToList();

            foreach (GridTile element in gridList)
            {
                gridField[element.Row, element.Column] = element;
                gridField[element.Row, element.Column].Initialize();
            }

            return gridField;
        }

        public void SaveMap(GridTile[,] gridField, string fileName = "GeneratedTileMap")
        {
            //"../../../../XNA_DEMOContent/XMLFiles/" + 
            var xmlFile = new FileStream("../../../../XNA_DEMOContent/XMLFiles/" + fileName + ".xml", FileMode.OpenOrCreate, FileAccess.Write);
            var writer = new StreamWriter(xmlFile);

            writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            writer.WriteLine("<tilemap>");

            for (int column = 0; column < MAX_COLUMNS; ++column)
            {
                for (int row = 0; row < MAX_ROWS; ++row)
                {
                    writer.WriteLine("\t<tile>");

                    writer.WriteLine("\t\t<positionX>" + row + "</positionX>");
                    writer.WriteLine("\t\t<positionY>" + column + "</positionY>");
                    writer.WriteLine("\t\t<TileType>" + (int)gridField[row, column].TileTypeValue + "</TileType>");

                    //writer.WriteLine("\t\t<settlement>" + gridField[row, column].GetTileSettlement() + "</settlement>");

                    writer.WriteLine("\t</tile>");
                }
            }

            writer.WriteLine("</tilemap>");

            writer.Close();
            xmlFile.Close();
        }

        public GridTile[,] GenerateMap(GameScene pGameScene)
        {
            GridTile[,] gridField = new GridTile[MAX_ROWS, MAX_COLUMNS]; ;
            // Map:
            List<GridTile> addTest = new List<GridTile>();

            for (int column = 0; column < MAX_COLUMNS; ++column)
            {
                for (int row = 0; row < MAX_ROWS; ++row)
                {
                    GridTile tileToAdd = new GridTile(pGameScene, row, column);
                    addTest.Add(tileToAdd);
                }
            }

            // Add Tiles to the right grid and give them the right attributes.
            for (int column = 0, teller = 0; column < MAX_COLUMNS; ++column)
            {
                for (int row = 0; row < MAX_ROWS; ++row, ++teller)
                {
                    gridField[row, column] = addTest.ElementAt(teller);

                    // Set the tile type (normal, tribe,...)
                    switch (m_Random.Next(0, 3))
                    {
                        case 0:
                            gridField[row, column].SetType(GridTile.TileType.NormalGrass);
                            break;
                        case 1:
                            gridField[row, column].SetType(GridTile.TileType.TreeLong);
                            break;
                        case 2:
                            gridField[row, column].SetType(GridTile.TileType.TreeShort);
                            break;
                        case 3:
                            gridField[row, column].SetType(GridTile.TileType.Normal4);
                            break;
                        case 4:
                            gridField[row, column].SetType(GridTile.TileType.Cliff);
                            break;
                        case 5:
                            gridField[row, column].SetType(GridTile.TileType.Water);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    // Use a color to create a new Tribe (none, green, red, blue,...)
                    //gridField[row, column].SetTileSettlement("none");
                }
            }

            // Save to XML file
            SaveMap(gridField);

            return gridField;
        }
    }
}