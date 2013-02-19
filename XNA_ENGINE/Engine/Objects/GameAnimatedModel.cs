using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using SkinnedModelData;
using System.Diagnostics;
using XNA_ENGINE.Engine.Scenegraph;

namespace XNA_ENGINE.Engine.Objects
{
    public class GameAnimatedModel : GameObject3D
    {
        private string _assetFile;
        private Model _model;

        private AnimationPlayer _animationPlayer;
        private SkinningData _skinningData;
        private float _speedScale = 1f;

        public GameAnimatedModel(string assetFile)
        {
            _assetFile = assetFile;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            _model = contentManager.Load<Model>(_assetFile);

            _skinningData = _model.Tag as SkinningData;

            Debug.Assert(_skinningData != null, "Model (" + _assetFile + ") contains no Skinning Data!");

            _animationPlayer = new AnimationPlayer(_skinningData);
            _animationPlayer.SetAnimationSpeed(_speedScale);
        }

        public override void Update(RenderContext renderContext)
        {
            base.Update(renderContext);

            if (_animationPlayer.CurrentClip != null)
                _animationPlayer.Update(renderContext.GameTime.ElapsedGameTime, true, WorldMatrix);
        }

        public override void Draw(RenderContext renderContext)
        {
            Matrix[] bones = null;
            if (_animationPlayer.CurrentClip != null)
            {
                bones = _animationPlayer.GetSkinTransforms();
            }

            // Render the skinned mesh.
            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    if (_animationPlayer.CurrentClip != null)
                    {
                        effect.SetBoneTransforms(bones);
                    }
                    else
                    {
                        effect.World = WorldMatrix;
                    }

                    effect.EnableDefaultLighting();

                    effect.View = renderContext.Camera.View;
                    effect.Projection = renderContext.Camera.Projection;

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }

                mesh.Draw();
            }

            base.Draw(renderContext);
        }

        public void PlayAnimation(string clipName)
        {
            PlayAnimation(clipName, true);
        }

        public void PlayAnimation(string clipName, bool loopAnimation)
        {
            PlayAnimation(clipName, loopAnimation, 0f);
        }

        public void PlayAnimation(string clipName, bool loopAnimation, float blendTime)
        {
            Debug.Assert(_skinningData.AnimationClips.ContainsKey(clipName), string.Format("This model contains no animation with the name {0}", clipName));

            var clip = _skinningData.AnimationClips[clipName];
            _animationPlayer.StartClip(clip, loopAnimation, blendTime);
        }

        public void SetAnimationSpeed(float speedScale)
        {
            if (_animationPlayer != null)
                _animationPlayer.SetAnimationSpeed(speedScale);

            _speedScale = speedScale;
        }

        public Matrix GetBoneTransform(string boneName)
        {
            if (_animationPlayer != null)
                return _animationPlayer.GetBoneTransform(boneName);

            return Matrix.Identity;
        }
    }
}