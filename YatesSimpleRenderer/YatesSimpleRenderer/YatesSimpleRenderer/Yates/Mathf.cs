#region Yates

// ┌──────────────────────────────────────────────────────────────┐
// │    描   述：                                                    
// │    作   者：Yates                                           
// │    版   本：1.0                                                 
// │    创建时间：2019-12-07-13:50 
// │    修改时间：2019-12-07-13:57                   
// └──────────────────────────────────────────────────────────────┘
// ┌──────────────────────────────────────────────────────────────┐
// │    命名空间：Yates                           
// │    文件名：Mathf.cs                                    
// └──────────────────────────────────────────────────────────────┘

#endregion

namespace YatesSimpleRenderer.Yates
{
    class Mathf
    {
        public static volatile float FloatMinDenormal = float.Epsilon;

        public static volatile float FloatMinNormal = 1.175494E-38f;

        public static bool IsFlushToZeroEnabled = (double)FloatMinDenormal == 0.0;

        public static readonly float Epsilon = !IsFlushToZeroEnabled ? FloatMinDenormal : FloatMinNormal;

        public static float Clamp(float value, float min, float max)
        {
            if ((double)value < (double)min)
            {
                value = min;
            }               
            else if ((double)value > (double)max)
            {
                value = max;
            }

            return value;                       
        }

        public static float Clamp01(float value)
        {
            return Clamp(value, 0.0f, 1f);
        }

        public static void Swap<T>(ref T x, ref T y)
        {
            T temp = x;
            x = y;
            y = temp;
        }
    }
}