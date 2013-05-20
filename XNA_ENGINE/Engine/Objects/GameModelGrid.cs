using System;
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
    public class GameModelGrid : GameObject3D
    {
        private readonly string _assetFile;
        private Model _model { get; set; }
        private bool m_UseTexture { get; set; }
        private Texture2D m_Texture { get; set; }
        private bool m_Selected { get; set; }
        private bool m_PermanentSelected { get; set; }
        private bool m_DangerHighlight { get; set; }
        private bool m_ShamanGoalHighlight { get; set; }
        private bool m_GreenHighlight { get; set; }

        private Vector3 m_DiffuseColor { get; set; }
        private float m_Alpha { get; set; }

        public GameModelGrid(string assetFile)
        {
            _assetFile = assetFile;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            _model = contentManager.Load<Model>(_assetFile);
            base.LoadContent(contentManager);
            
            m_Alpha = 1;

            CanDraw = true;
            m_DiffuseColor = new Vector3(1, 1, 1);
        }

        public override void Draw(RenderContext renderContext)
        {
            if (!CanDraw) return;

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

                    //Texture
                    if (m_Texture != null)
                        effect.Texture = m_Texture;
                    effect.TextureEnabled = m_UseTexture;

                    //Diffuse
                    effect.DiffuseColor = m_DiffuseColor;
                    effect.SpecularColor = new Vector3(0.1f, 0.1f, 0.1f); 

                    //Alpha
                    effect.Alpha = m_Alpha;

                    //Selected
                    if (m_PermanentSelected)
                    {
                        effect.SpecularColor = new Vector3(0.5f, 0.5f, 0.5f); 
                        effect.EmissiveColor = new Vector3(0.5f, 0.5f, 0.5f);
                    }
                    else if (m_Selected)
                        effect.EmissiveColor = new Vector3(0.1f, 0.1f, 0.1f);
                    else 
                        effect.EmissiveColor = new Vector3(0.0f, 0.0f, 0.0f);
                    
                    if (m_DangerHighlight)
                    {
                        float totalTimeInMilliseconds = renderContext.GameTime.TotalGameTime.Milliseconds + (renderContext.GameTime.TotalGameTime.Seconds*1000);
                        totalTimeInMilliseconds/=300.0f;

                        float smooth = ((float)Math.Cos(totalTimeInMilliseconds) + 1)/2;

                        effect.SpecularColor += new Vector3(0.5f * smooth + 0.1f, 0.0f, 0.0f);
                        effect.EmissiveColor += new Vector3(0.1f * smooth + 0.02f, 0.0f, 0.0f);
                    }

                    if (m_ShamanGoalHighlight)
                    {
                        float totalTimeInMilliseconds = renderContext.GameTime.TotalGameTime.Milliseconds + (renderContext.GameTime.TotalGameTime.Seconds * 1000);
                        totalTimeInMilliseconds /= 150.0f;

                        float smooth = ((float)Math.Cos(totalTimeInMilliseconds) + 1) / 2;

                        effect.SpecularColor += new Vector3(0.0f, 0.0f,1.0f * smooth + 0.2f);
                        effect.EmissiveColor += new Vector3(0.0f, 0.0f, 0.1f * smooth + 0.04f);
                    }

                    if (m_GreenHighlight)
                    {
                        float totalTimeInMilliseconds = renderContext.GameTime.TotalGameTime.Milliseconds + (renderContext.GameTime.TotalGameTime.Seconds * 1000);
                        totalTimeInMilliseconds /= 150.0f;

                        float smooth = ((float)Math.Cos(totalTimeInMilliseconds) + 1) / 2;

                        effect.SpecularColor += new Vector3(0.0f, 0.8f * smooth, 0.0f);
                        effect.EmissiveColor += new Vector3(0.0f, 0.1f * smooth, 0.0f);
                    }
                    
                    effect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                }

                mesh.Draw();
            }

            base.Draw(renderContext);
        }

        //Setters and getters
        public Model Model
        {
            get{return _model;}
            set{_model = value;}
        }
        public bool UseTexture
        {
            get{return m_UseTexture;}
            set{m_UseTexture = value;}
        }
        public Texture2D Texture2D
        {
            get{return m_Texture;}
            set{m_Texture = value;}
        }
        public bool Selected
        {
            get{return m_Selected;}
            set{ m_Selected = value;}
        }
        public bool PermanentSelected
        {
            get { return m_PermanentSelected; }
            set { m_PermanentSelected = value; }
        }
        public bool Danger
        {
            get { return m_DangerHighlight; }
            set { m_DangerHighlight = value; }
        }
        public bool ShamanGoal
        {
            get { return m_ShamanGoalHighlight; }
            set { m_ShamanGoalHighlight = value; }
        }
        public bool GreenHighLight
        {
            get { return m_GreenHighlight; }
            set { m_GreenHighlight = value; }
        }

        public Vector3 DiffuseColor
        {
            get{return m_DiffuseColor;}
            set{ m_DiffuseColor = value;}
        }
        public float Alpha
        {
            get{return m_Alpha;}
            set{ m_Alpha = value;}
        }

        public void SetTexture(Texture2D texture)
        {
            m_UseTexture = true;
            m_Texture = texture;
        }
    }
}
