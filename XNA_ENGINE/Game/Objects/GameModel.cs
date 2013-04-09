﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.Objects
{
    class GameModel : GameObject3D
    {
        private readonly string _assetFile;
        private Model _model { get; set; }
        private bool m_UseTexture { get; set; }
        private Texture2D m_Texture { get; set; }
        private bool m_Selected { get; set; }
        private Vector3 m_DiffuseColor { get; set; }


        public GameModel(string assetFile)
        {
            _assetFile = assetFile;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            _model = contentManager.Load<Model>(_assetFile);
            base.LoadContent(contentManager);
            
          /*  foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = part.Effect.Clone();
                }
            }*/
        }

        public override void Draw(RenderContext renderContext)
        {
            var transforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.View = renderContext.Camera.View;
                    effect.Projection = renderContext.Camera.Projection;
                    effect.World = transforms[mesh.ParentBone.Index] * WorldMatrix;
                    
                    effect.DiffuseColor = m_DiffuseColor;
                    
                    //Texture
                    if (m_UseTexture)
                    {
                       // effect.DiffuseColor = new Vector3(1, 1, 1);
                      //  effect.TextureEnabled = true;
                      //  effect.Texture = m_Texture;
                    }
                    else
                    {
                       // effect.DiffuseColor = new Vector3(0.345098f,0.694118f,0.105882f);
                      //  effect.TextureEnabled = false;
                    }

                    //Selecetd
                    if (m_Selected)
                    {
                        effect.EmissiveColor = new Vector3(0.1f, 0.1f, 0.1f);
                    }
                    else
                    {
                        effect.EmissiveColor = new Vector3(0.0f, 0.0f, 0.0f);
                    }
                }

                mesh.Draw();
            }

            base.Draw(renderContext);
        }

        //Sets and gets
        public Model Model
        {
            get
            {
                return _model;
            }

            set
            {
                _model = value;
            }
        }
        public bool UseTexture
        {
            get
            {
                return m_UseTexture;
            }

            set
            {
                m_UseTexture = value;
            }
        }
        public Texture2D Texture2D
        {
            get
            {
                return m_Texture;
            }

            set
            {
                m_Texture = value;
            }
        }
        public bool Selected
        {
            get
            {
                return m_Selected;
            }

            set
            {
                m_Selected = value;
            }
        }
        public Vector3 DiffuseColor
        {
            get
            {
                return m_DiffuseColor;
            }

            set
            {
                m_DiffuseColor = value;
            }
        }

        public void SetTexture(Texture2D texture)
        {
            m_UseTexture = true;
            m_Texture = texture;
        }
    }
}
