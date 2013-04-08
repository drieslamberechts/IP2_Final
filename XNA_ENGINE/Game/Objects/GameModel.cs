using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Scenegraph;

namespace XNA_ENGINE.Game.Objects
{
    class GameModel : GameObject3D
    {
        private readonly string _assetFile;
        private Model _model { get; set; }

        public GameModel(string assetFile)
        {
            _assetFile = assetFile;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            _model = contentManager.Load<Model>(_assetFile);
            base.LoadContent(contentManager);
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
                }

                mesh.Draw();
            }

            base.Draw(renderContext);
        }

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

        /*
        public void SetTexture(Texture2D texture)
        {
            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.Texture = texture;
                    // effect.FogEnabled = true;
                    //effect.EmissiveColor = color;

                    // effect.DiffuseColor = color;
                }
            }
        }
        */
    }
}
