using System;

namespace Game.Scripts.GameplayLogic.AI.UtilityAI.Calculations
{
    public class AIScoreCalculator
    {
        public float AsIs(float input, AIContext context) => 
            input;
        
        public Func<float, AIContext, float> ScaleBy(int scale) => 
            (input, hero) => input * scale;
        
        public Func<float, AIContext, float> IncreaseBy(int value) => 
            (input, hero) => input * value;
    }
}