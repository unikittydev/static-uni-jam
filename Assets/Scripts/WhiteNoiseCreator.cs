using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

using Random = Unity.Mathematics.Random;

namespace Game
{
    public class WhiteNoiseCreator : MonoBehaviour
    {
        private struct GenerateNoiseJob : IJobFor
        {
            [WriteOnly, NativeDisableParallelForRestriction]
            public NativeArray<Color32> colors;

            public int frame;

            public int2 minMaxValue;
            
            public void Execute(int index)
            {
                var random = Random.CreateFromIndex(math.asuint(index + frame));
                var color = (byte)random.NextInt(minMaxValue.x, minMaxValue.y);
                colors[index] = new Color32(color, color, color, 255);
            }
        }
        
        [SerializeField] private int width, height;
        [SerializeField] private int2 minMaxValue;
        
        [SerializeField] private float delayTime = 0.1f;
        private float delayCounter;
        
        [SerializeField] private Image background;

        private Texture2D noiseTexture;

        private void Start()
        {
            noiseTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

            background.sprite = Sprite.Create(noiseTexture, new Rect(Vector2.zero, new Vector2(width, height)), Vector2.zero);
        }

        private void Update()
        {
            delayCounter += Time.deltaTime;
            if (delayCounter <= delayTime)
                return;
            delayCounter -= delayTime;
            
            NativeArray<Color32> data = noiseTexture.GetPixelData<Color32>(0);
            var job = new GenerateNoiseJob()
            {
                colors = data,
                minMaxValue = minMaxValue,
                frame = UnityEngine.Random.Range(int.MinValue, int.MaxValue)
            }.ScheduleParallel(width * height, 0, default);
            job.Complete();
            
            noiseTexture.SetPixelData(data, 0);
            noiseTexture.Apply(false);
        }
    }
}