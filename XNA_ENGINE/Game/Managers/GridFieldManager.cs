using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Objects;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.Managers
{
    class GridFieldManager
    {
        public enum SelectionMode
        {
            select1x1,
            select2x2,
            select3x3,

            enumSize
        }

        //Singleton implementation
        private static GridFieldManager m_GridFieldManager;

        private GridTile[,] m_GridField;

        private const int GRID_ROW_LENGTH = 30;
        private const int GRID_COLUMN_LENGTH = 30;

        private SelectionMode m_SelectionMode = SelectionMode.select1x1;

        private GameScene m_GameScene;

        private List<Player> m_PlayersList;
        private Player m_UserPlayer;

        // private int GRID_OFFSET = 64;

        private Random m_Random;

        private GridFieldManager()
        {
            CreativeMode = false;

            m_Random = new Random();
        }

        static public GridFieldManager GetInstance()
        {
            if (m_GridFieldManager == null)
                m_GridFieldManager = new GridFieldManager();

            return m_GridFieldManager;
        }

        public void Initialize()
        {
             List<GridTile> settlementTiles = new List<GridTile>();
             settlementTiles.Add(m_GridField[2, 3]);
             settlementTiles.Add(m_GridField[2, 4]);
             settlementTiles.Add(m_GridField[3, 3]);
             settlementTiles.Add(m_GridField[3, 4]);
             BuildPlaceable(Placeable.PlaceableType.Settlement, m_UserPlayer, settlementTiles);

             settlementTiles.Clear();
             settlementTiles.Add(m_GridField[5, 2]);
             settlementTiles.Add(m_GridField[6, 2]);
             settlementTiles.Add(m_GridField[5, 3]);
             settlementTiles.Add(m_GridField[6, 3]);
             BuildPlaceable(Placeable.PlaceableType.School, m_UserPlayer, settlementTiles);

             settlementTiles.Clear();
             settlementTiles.Add(m_GridField[7, 2]);
             BuildPlaceable(Placeable.PlaceableType.Shrine, m_UserPlayer, settlementTiles);

             //TEST PLACEABLES
             m_UserPlayer.AddPlaceable(new Villager(m_GridField[5,5]));
             m_UserPlayer.AddPlaceable(new Army(m_GridField[6, 4]));
             m_UserPlayer.AddPlaceable(new Shaman(m_GridField[7, 3]));

             m_PlayersList.ElementAt(1).AddPlaceable(new Army(m_GridField[15, 6]));
        }

        public void Update(RenderContext renderContext)
        {
            //Iterate over every GridTile
            for (int i = 0; i < GRID_ROW_LENGTH; ++i)
            {
                for (int j = 0; j < GRID_COLUMN_LENGTH; ++j)
                {
                    m_GridField[i,j].Update(renderContext);
                }
            }
            
            //Iterate over every Player
            foreach (var player in m_PlayersList)
            {
                player.Update(renderContext);
            }
            
            //Check if armies need to be merged
            foreach (Player player in m_PlayersList)
            {
                List<Placeable> armyMergeList = new List<Placeable>();

                foreach (Placeable ownedObject in player.GetOwnedList())
                {
                    if(ownedObject.PlaceableTypeMeth == Placeable.PlaceableType.Army)
                        armyMergeList.Add(ownedObject);
                }

                List<Placeable> ArmiesAlreadyMerged = new List<Placeable>();

                foreach (Army army1 in armyMergeList)
                    foreach (Army army2 in armyMergeList)
                    {
                        if (army1.GetTargetTile() == army2.GetTargetTile() && army1 != army2)
                        {
                            bool mayProceed = true;
                            foreach (Army armyAlreadyMerged in ArmiesAlreadyMerged)
                            {
                                if (armyAlreadyMerged == army1 || armyAlreadyMerged == army2) mayProceed = false;
                            }

                            if (mayProceed)
                            {
                                ArmiesAlreadyMerged.Add(army1);
                                ArmiesAlreadyMerged.Add(army2);

                                army1.MergeArmies(army2);
                            }
                        }
                    }

                armyMergeList.Clear();
            }

            //GO OVER ALL PLACEABLES
            //CHECK IF AN ARMY AND VILLAGER INTERSECT
            /*List<Placeable> allPlaceables = GetAllPlaceables();
            foreach (Placeable placeable1 in allPlaceables)
                foreach (Placeable placeable2 in allPlaceables)
                {
                    if (placeable1 != placeable2)
                    {
                        if (placeable1.GetTargetTile() == placeable2.GetTargetTile())
                        {
                            if (placeable1.PlaceableTypeMeth == Placeable.PlaceableType.Villager &&
                                placeable2.PlaceableTypeMeth == Placeable.PlaceableType.Army)
                            {
                                placeable1.GetOwner().RemovePlaceable(placeable1);

                            }
                        }
                    }
                }*/
        }

        public void LoadMap(GameScene gameScene, string map)
        {
            m_GameScene = gameScene;

            // Load Map
            m_GridField = MapLoadSave.GetInstance().LoadMap(gameScene, map);

            //Iterate over every GridTile
            for (int i = 0; i < GRID_ROW_LENGTH; ++i)
            {
                for (int j = 0; j < GRID_COLUMN_LENGTH; ++j)
                {
                    m_GridField[i, j].Initialize();
                }
            }

            m_PlayersList = new List<Player>();
        }

        public void HandleInput(RenderContext renderContext)
        {
            var inputManager = PlayScene.GetInputManager();
            bool isMouseInScreen = PlayScene.IsMouseInScreen(renderContext);
            Menu.ModeSelected selectedMode = Menu.GetInstance().GetSelectedMode();

            Deselect();
            //Handle menu //If menu is hit don't do the grid test
            if (Menu.GetInstance().HandleInput(renderContext)) return; // hier in Menu -> klikken?
            if (m_UserPlayer.HandleInput(renderContext)) return;

            //Check if the mouse cursor is in the screen
            if (isMouseInScreen)
            {
                //Raycast to grid
                var hittedTile = HitTestField(PlayScene.CalculateCursorRay(renderContext));

                //Raycast to placeables
                var hittedPlaceable = HitTestPlaceables(PlayScene.CalculateCursorRay(renderContext));

                if (hittedPlaceable != null)
                {
                    Select(hittedPlaceable);
                }
                else if (hittedTile != null)
                {
                    Select(hittedTile);
                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered)
                    {
                        switch (selectedMode)
                        {
                            case Menu.ModeSelected.None:
                                PermanentDeselect();
                                break;
                            case Menu.ModeSelected.Attack:
                                break;
                            case Menu.ModeSelected.Defend:
                                break;
                            case Menu.ModeSelected.Gather:
                                break;
                            case Menu.ModeSelected.BuildSettlement:
                                BuildPlaceable(Placeable.PlaceableType.Settlement, m_UserPlayer);
                                break;
                            case Menu.ModeSelected.BuildShrine:
                                BuildPlaceable(Placeable.PlaceableType.Shrine, m_UserPlayer);
                                break;
                            case Menu.ModeSelected.BuildSchool:
                                BuildPlaceable(Placeable.PlaceableType.School, m_UserPlayer);
                                break;
                            case Menu.ModeSelected.Delete:
                                //RemoveSettlementModel();
                                break;

                            // CREATE TILES WITH SHAMAN
                            case Menu.ModeSelected.BuildTile1:
                                hittedTile.SetType(GridTile.TileType.Spiked);
                                break;
                            case Menu.ModeSelected.BuildTile2:
                                break;
                            case Menu.ModeSelected.BuildTile3:
                                break;
                            case Menu.ModeSelected.BuildTile4:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    if (inputManager.GetAction((int)PlayScene.PlayerInput.RightClick).IsTriggered)
                    {
                        var selectedPlaceable = GetPermanentSelected();
                    }
                }
            }
        }

        public GridTile HitTestField(Ray ray)
        {
            //Iterate over every GridTile
            for (int i = 0; i < GRID_ROW_LENGTH; ++i)
            {
                for (int j = 0; j < GRID_COLUMN_LENGTH; ++j)
                {
                    if (m_GridField[i,j].HitTest(ray))
                        return m_GridField[i, j];
                }
            }

            return null;
        }

        public Placeable HitTestPlaceables(Ray ray)
        {
            //Iterate over every placeable
            foreach (var player in m_PlayersList)
            {
                foreach (var ownedPlaceable in player.GetOwnedList())
                {
                    if (ownedPlaceable.HitTest(ray))
                        return ownedPlaceable;
                }
            }

            return null;
        }

        public List<Placeable> GetAllPlaceables()
        {
            List<Placeable> returnList = new List<Placeable>();
            foreach (Player player in m_PlayersList)
                foreach (Placeable placeable in player.GetOwnedList())
                {
                    returnList.Add(placeable);
                }

            return returnList;
        }

        public Placeable GetPermanentSelected()
        {
            foreach (var player in m_PlayersList)
                foreach (var placeable in player.GetOwnedList())
                    if (placeable.Model.PermanentSelected)
                        return placeable;

            return null;
        }

        public List<GridTile> GetSelectedTiles()
        {
            List<GridTile> returnList = new List<GridTile>();

            foreach (var gridTile in m_GridField)
            {
                if (gridTile.Selected)
                    returnList.Add(gridTile);
            }
            if (returnList.Any()) return returnList;

            return null;
        }

        public List<Placeable> GetSelectedPlaceables()
        {
            List<Placeable> returnList = new List<Placeable>();

            foreach (var player in m_PlayersList)
            {
                foreach (var ownedPlaceables in player.GetOwnedList())
                {
                    if (ownedPlaceables.Model.Selected)
                        returnList.Add(ownedPlaceables);
                }
            }
            if (returnList.Any()) return returnList;

            return null;
        }

        public bool IsTileAccesible(GridTile currentTile , GridTile target)
        {
            if (!currentTile.IsWalkable()) return false;
            if (GetNWTile(currentTile) == target) return true;
            if (GetSWTile(currentTile) == target) return true;
            if (GetNETile(currentTile) == target) return true;
            if (GetSETile(currentTile) == target) return true;
            return false;
        }

        public void Select(GridTile tile)
        {
            switch (m_SelectionMode)
            {
                case SelectionMode.select1x1:
                    tile.Selected = true;
                    break;
                case SelectionMode.select2x2:
                    tile.Selected = true;
                    if (GetSWTile(tile) != null) GetSWTile(tile).Selected = true;
                    if (GetSTile(tile) != null) GetSTile(tile).Selected = true;
                    if (GetSETile(tile) != null) GetSETile(tile).Selected = true;
                    break;
                case SelectionMode.select3x3:
                    tile.Selected = true;
                    if (GetNWTile(tile) != null) GetNWTile(tile).Selected = true;
                    if (GetNTile(tile) != null) GetNTile(tile).Selected = true;
                    if (GetNETile(tile) != null) GetNETile(tile).Selected = true;
                    if (GetETile(tile) != null) GetETile(tile).Selected = true;
                    if (GetSETile(tile) != null) GetSETile(tile).Selected = true;
                    if (GetSTile(tile) != null) GetSTile(tile).Selected = true;
                    if (GetSWTile(tile) != null) GetSWTile(tile).Selected = true;
                    if (GetWTile(tile) != null) GetWTile(tile).Selected = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("SelectionMode (1x1, 2x2 or 3x3) is out of range in GridFieldManager");
            }
        }

        public void Select(Placeable placeable)
        {
            placeable.Model.Selected = true;
        }

        public bool BuildPlaceable(Placeable.PlaceableType structureType, Player owner, List<GridTile> tileList = null)
        {
            List<GridTile> tileListToBuildOn  = new List<GridTile>();

            if (tileList != null)
                tileListToBuildOn = tileList;
            else
                tileListToBuildOn = GetSelectedTiles();

            bool proceed = true;
            foreach (GridTile gridTile in tileListToBuildOn)
                if (gridTile.IsOpen() == false) proceed = false;

            if (!proceed) return false;

            switch (structureType)
            {
                case Placeable.PlaceableType.Settlement:
                    owner.AddPlaceable(new Settlement(tileListToBuildOn));
                    break;
                case Placeable.PlaceableType.School:
                    owner.AddPlaceable(new School(tileListToBuildOn));
                    break;
                case Placeable.PlaceableType.Shrine:
                    owner.AddPlaceable(new Shrine(tileListToBuildOn));
                    break;
                case Placeable.PlaceableType.Villager:
                    break;
                case Placeable.PlaceableType.Army:
                    break;
                case Placeable.PlaceableType.Shaman:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("structureType");
            }

            Menu.GetInstance().SubMenu = Menu.SubMenuSelected.BaseMode;
            Menu.GetInstance().SetSelectedMode(Menu.ModeSelected.None);
            return true;
        }

        public void AddPlayer(Player player)
        {
            m_PlayersList.Add(player);
        }

        public void SetUserPlayer(Player player)
        {
            m_UserPlayer = player;
        }

        public void Deselect()
        {
            foreach (var player in m_PlayersList)
                foreach (var ownedPlaceable in player.GetOwnedList())
                    ownedPlaceable.Model.Selected = false;
            
            foreach (var gridTile in m_GridField)
                gridTile.Selected = false;
        }

        public void PermanentDeselect()
        {
            foreach (var player in m_PlayersList)
                foreach (var placeable in player.GetOwnedList())
                    placeable.Model.PermanentSelected = false;

            foreach (GridTile gridTile in m_GridField)
            {
                gridTile.Model.PermanentSelected = false;
            }
        }

        //Functions that pick a surrounding tile of another tile
        #region Surrounding tiles
        //NW and following names stand for north west....
        //NorthWest
        public GridTile GetNWTile(GridTile tile)
        {
            if (tile.Row <= 0) 
                return null;

            return m_GridField[tile.Row - 1, tile.Column];
        }

        //North
        public GridTile GetNTile(GridTile tile)
        {
            if (tile.Row <= 0 || tile.Column <= 0)
                return null;

            return m_GridField[tile.Row - 1, tile.Column - 1];
        }

        //NorthEast
        public GridTile GetNETile(GridTile tile)
        {
            if (tile.Column <= 0)
                return null;

            return m_GridField[tile.Row, tile.Column - 1];
        }

        //East
        public GridTile GetETile(GridTile tile)
        {
            if (tile.Row >= GRID_ROW_LENGTH - 1 || tile.Column <= 0)
                return null;

            return m_GridField[tile.Row +1, tile.Column - 1];
        }

        //SouthEast
        public GridTile GetSETile(GridTile tile)
        {
            if (tile.Row >= GRID_ROW_LENGTH -1)
                return null;

            return m_GridField[tile.Row +1, tile.Column];
        }

        //South
        public GridTile GetSTile(GridTile tile)
        {
            if (tile.Row >= GRID_ROW_LENGTH - 1 || tile.Column >= GRID_COLUMN_LENGTH - 1)
                return null;

            return m_GridField[tile.Row + 1, tile.Column + 1];
        }

        //SouthWest
        public GridTile GetSWTile(GridTile tile)
        {
            if (tile.Column >= GRID_COLUMN_LENGTH -1)
                return null;

            return m_GridField[tile.Row , tile.Column +1];
        }

        //West
        public GridTile GetWTile(GridTile tile)
        {
            if (tile.Row <= 0 || tile.Column >= GRID_COLUMN_LENGTH - 1)
                return null;

            return m_GridField[tile.Row -1 , tile.Column + 1];
        }

        //All surrounding tiles
        public List<GridTile> GetAllSurroundingTiles(GridTile tile)
        {
            List<GridTile> returnList = new List<GridTile>();

            if (GetNWTile(tile) != null) returnList.Add(GetNWTile(tile));
            if (GetNTile(tile) != null) returnList.Add(GetNTile(tile));
            if (GetNETile(tile) != null) returnList.Add(GetNETile(tile));
            if (GetETile(tile) != null) returnList.Add(GetETile(tile));
            if (GetSETile(tile) != null) returnList.Add(GetSETile(tile));
            if (GetSTile(tile) != null) returnList.Add(GetSTile(tile));
            if (GetSWTile(tile) != null) returnList.Add(GetSWTile(tile));
            if (GetWTile(tile) != null) returnList.Add(GetWTile(tile));

            return returnList;
        }

        #endregion

        public SelectionMode SelectionModeMeth
        {
            get { return m_SelectionMode; }
            set { m_SelectionMode = value; }
        }

        public GameScene GameScene
        {
            get { return m_GameScene; }
        }

        public Player UserPlayer
        {
            get { return m_UserPlayer; }
        }

        public Player AiPlayer
        {
            get { return m_PlayersList.ElementAt(1); }
        }

        public void NextSelectionMode()
        {
            ++m_SelectionMode;
            if ((int)m_SelectionMode >= (int)SelectionMode.enumSize) m_SelectionMode = 0;
        }

        public Random Random
        {
            get { return m_Random; }
        }
        public GridTile[,] GridField
        {
            get { return m_GridField; }
        }

        public bool CreativeMode { get; set; }
    }
}
